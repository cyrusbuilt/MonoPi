//
//  BitSet.cs
//
//  Author:
//       Chris.Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2015 Chris.Brunner
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
// This class is a C# port of the Java BitSet type (java.util.BitSet) found
// here http://www.docjar.com/html/api/java/util/BitSet.java.html.
//
// The class adheres to the GPL that accompanies the original Java source.
// Original Java version authors (JDK1.0):
// Arthur van Hoff
// Michael McCloskey
// Martin Buchholz
//
// Original copyright:
// Copyright (c) 1995, 2007, Oracle and/or its affiliates. All rights reserved.
// DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
// or visit www.oracle.com if you need additional information or have any
// questions.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// An implementation of a vector of bits that grows as needed. Each
	/// component of the bit set has a <code>boolean</code> value. The
	/// bits of a bit set are indexed by non-negative integers.
	/// Individual indexed bits can be examined, set, or cleared. One
	/// bit set may be used to modify the contents of another through
	/// logical AND, logical inclusive OR, and logical exclusive OR
	/// operations.<br/><br/>
	/// By default, all bits in the set intially have the value of
	/// <code>false</code>.<br/><br/>
	/// Every bit set has a current size, which is the number of bits of
	/// space currently in use by the bit set. Note that the size is
	/// related to the implementation of a bit set, so it may change
	/// with implementation. The length of a bit set relates to the
	/// logical length of a bit set and is defined independently of
	/// implementation.<br/><br/>
	/// Unless otherwise noted, passing a null parameter to any of the
	/// methods in a BitSet will result in a <see cref="ArgumentNullException"/>.
	/// </summary>
	/// <remarks>
	/// This class is not thread-safe without external synchronization.
	/// </remarks>
	[Serializable]
	public class BitSet : ICloneable
	{
		#region Constants
		/*
		 * BitSets are packed into arrays of "words."  Currently a word is
         * a long, which consists of 64 bits, requiring 6 address bits.
         * The choice of word size is determined purely by performance concerns.
         */
		private const Int32 ADDRESS_BITS_PER_WORD = 6;
		private const Int32 BITS_PER_WORD = 1 << ADDRESS_BITS_PER_WORD;
		private const Int32 BIT_INDEX_MASK = BITS_PER_WORD - 1;
		private const long _serialVersionUID = 7997698588986878753L;

		// Used to shift left or right for a partial word mask.
		private const long LONG_MASK = 0x3f;
		#endregion

		#region Fields

		private long[] _bits = null;
		private volatile Int32 _wordsInUse = 0;
		private volatile Boolean _sizeIsSticky = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.BitSet"/>
		/// class whose intial size is large enough to explicitly
		/// represent bits with indices in the range of 0 through
		/// <paramref name="nBits"/> - 1. All bits are intially
		/// <code>false</code>.
		/// </summary>
		/// <param name="nBits">
		/// The initial size of the bit set.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="nBits"/> cannot be negative.
		/// </exception>
		public BitSet(Int32 nBits) {
			if (nBits < 0) {
				throw new ArgumentOutOfRangeException("nBits", "Argument may not be negative.");
			}
				
			this._bits = new long[WordIndex(nBits - 1) + 1];
			this._sizeIsSticky = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.BitSet"/>
		/// class. This is the default constructor.
		/// </summary>
		public BitSet()
			: this(BITS_PER_WORD) {
			this._sizeIsSticky = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.BitSet"/>
		/// class from the specified array (words) as the internal
		/// representation. The last word (if there is one) must
		/// be non-zero.
		/// </summary>
		/// <param name="words">
		/// The array (words) to compose this instance from.
		/// </param>
		public BitSet(long[] words) {
			this._bits = words;
			this._wordsInUse = this._bits.Length;
			this.CheckInvariants();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.MonoPi.BitSet"/> is empty.
		/// </summary>
		/// <value>
		/// true if this instance contains no bits that are set to <code>true</code>;
		/// Otherwise, false.
		/// </value>
		public Boolean Empty {
			get { return this._wordsInUse == 0; }
		}

		/// <summary>
		/// Gets the "logical size" of this bit set: the index of
		/// the highest set bit in the bit set plus one.
		/// </summary>
		/// <value>
		/// The logical size of this bit set or zero if this instance
		/// contains no bits.
		/// </value>
		public Int32 Length {
			get {
				if (this._wordsInUse == 0) {
					return 0;
				}

				if ((this._bits == null) || (this._bits.Length == 0)) {
					this._wordsInUse = 0;
					return this._wordsInUse;
				}

				Int32 positions = NumberOfTrailingZeros(this._bits[this._wordsInUse - 1]);
				return BITS_PER_WORD * (this._wordsInUse - 1) + (BITS_PER_WORD - positions);
			}
		}

		/// <summary>
		/// Gets the number of bits of space actually in use by this
		/// bit set to represent bit values. The maximum element in
		/// the set is the size minus the first element.
		/// </summary>
		/// <value>
		/// The number of bits currently in this bit set.
		/// </value>
		public Int32 Size {
			get { return this._bits.Length * BITS_PER_WORD; }
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Every public method must preserve the invariants. This
		/// method checks to see if this is true using assertions.
		/// </summary>
		private void CheckInvariants() {
			Debug.Assert((this._wordsInUse == 0) || (this._bits[this._wordsInUse - 1] != 0));
			Debug.Assert((this._wordsInUse >= 0) && (this._wordsInUse <= this._bits.Length));
			Debug.Assert((this._wordsInUse == this._bits.Length) || (this._bits[this._wordsInUse] == 0));
		}

		/// <summary>
		/// Sets the internal word use count field to the logical
		/// size in words of the bit set. WARNING: This method
		/// assumes that the number of words actually in use is
		/// less than or equal to the current value of the words
		/// in use field!!!
		/// </summary>
		private void RecalculateWordsInUse() {
			Int32 i = 0;
			for (i = this._wordsInUse - 1; i >= 0; i--) {
				if (this._bits[i] != 0) {
					break;
				}
			}
			this._wordsInUse = i + 1;
		}

		/// <summary>
		/// Ensures that this BitSet can hold enough words.
		/// </summary>
		/// <param name="lastElt">
		/// The minimum acceptable number of words.
		/// </param>
		private void EnsureCapacity(Int32 lastElt) {
			if (lastElt >= this._bits.Length) {
				long[] nd = new long[lastElt + 1];
				Array.Copy(this._bits, 0, nd, 0, this._bits.Length);
				this._bits = nd;
				this._sizeIsSticky = false;
			}
		}

		/// <summary>
		/// Ensures that the BitSet can accomodate a given word
		/// index, temporarily violating the invariants. The caller
		/// must restore the invariants before returning to the
		/// user, possibly using <see cref="CyrusBuilt.MonoPi.BitSet.RecalculateWordsInUse()"/>.
		/// </summary>
		/// <param name="wordIndex">
		/// The index to be accommodated.
		/// </param>
		private void ExpandTo(Int32 wordIndex) {
			Int32 required = wordIndex + 1;
			if (this._wordsInUse < required) {
				this.EnsureCapacity(required);
				this._wordsInUse = required;
			}
		}

		/// <summary>
		/// Attempts to reduce internal storage used for the bits in this
		/// bit set. Calling this method may, but is not required to, affect
		/// the value returned by a subsequent call to the <see cref="CyrusBuilt.MonoPi.BitSet.Size"/>
		/// property.
		/// </summary>
		private void TrimToSize() {
			if (this._wordsInUse != this._bits.Length) {
				long[] copy = { };
				Array.Copy(this._bits, copy, this._wordsInUse);
				this._bits = copy;
				this.CheckInvariants();
			}
		}

		/// <summary>
		/// Returns the number of bits set to <code>true</code> in this
		/// bit set.
		/// </summary>
		/// <returns>
		/// The number of bits set.
		/// </returns>
		public Int32 Cardinality() {
			Int32 card = 0;
			long a = 0L;
			Int32 b = 0;
			for (Int32 i = this._bits.Length - 1; i >= 0; i--) {
				a = this._bits[i];
				// Take care of common cases.
				if (a == 0) {
					continue;
				}

				if (a == -1) {
					card += 64;
					continue;
				}

				// Successively collapse alternating bit groups into a sum.
				a = ((a >> 1) & 0x5555555555555555L) + (a & 0x5555555555555555L);
				a = ((a >> 2) & 0x3333333333333333L) + (a & 0x3333333333333333L);
				b = (Int32)((a >> 32) + a);
				b = ((b >> 4) & 0x0f0f0f0f) + (b & 0x0f0f0f0f);
				b = ((b >> 8) & 0x00ff00ff) + (b & 0x00ff00ff);
				card += ((b >> 16) & 0x0000ffff) + (b & 0x0000ffff);
			}
			return card;
		}

		/// <summary>
		/// Performs a logical <b>AND</b> of this target bit set with the
		/// argument bit set. This bit set is modified so that each bit in
		/// it has the value <code>true</code> if and only if it both
		/// initially had the value <code>true</code> and the corresponding
		/// bit in the specified bit set also had the value <code>true</code>.
		/// </summary>
		/// <param name="bs">
		/// A bit set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bs"/> cannot be null.
		/// </exception>
		public void And(BitSet bs) {
			if (bs == null) {
				throw new ArgumentNullException("bs");
			}

			if (this == bs) {
				return;
			}

			while (this._wordsInUse > bs._wordsInUse) {
				this._bits[--this._wordsInUse] = 0;
			}

			for (Int32 i = 0; i < this._wordsInUse; i++) {
				this._bits[i] &= bs._bits[i];
			}

			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Clears all of the bits in this bit set whose corresponding
		/// bit is set in the specified bit set.
		/// </summary>
		/// <param name="bs">
		/// The bit set with which to mask this instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bs"/> cannot be null.
		/// </exception>
		public void AndNot(BitSet bs) {
			if (bs == null) {
				throw new ArgumentNullException("bs");
			}

			Int32 i = Math.Min(this._bits.Length, bs._bits.Length);
			while (--i >= 0) {
				this._bits[i] &= ~bs._bits[i];
			}

			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Performs a logical <b>OR</b> of this bit set with the specified
		/// bit set. This bit set is modified so that a bit in it has the
		/// value <code>true</code> if and only if either it already had
		/// the value <code>true</code> or the corresponding bit in the
		/// specified bit set has the value <code>true</code>.
		/// </summary>
		/// <param name="bs">
		/// A bit set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bs"/> cannot be null.
		/// </exception>
		public void Or(BitSet bs) {
			if (bs == null) {
				throw new ArgumentNullException("bs");
			}

			if (this == bs) {
				return;
			}

			Int32 wordsInCommon = Math.Min(this._wordsInUse, bs._wordsInUse);
			if (this._wordsInUse < bs._wordsInUse) {
				this.EnsureCapacity(bs._wordsInUse);
				this._wordsInUse = bs._wordsInUse;
			}

			for (Int32 i = 0; i < wordsInCommon; i++) {
				this._bits[i] |= bs._bits[i];
			}

			if (wordsInCommon < bs._wordsInUse) {
				Array.Copy(bs._bits, wordsInCommon, this._bits, wordsInCommon, this._wordsInUse - wordsInCommon);
			}
			this.CheckInvariants();
		}

		/// <summary>
		/// Performs a logical <b>XOR</b> of this bit set with the specified
		/// bit set. This bit set is modified so that a bit in it has the
		/// value <code>true</code> if and only if one of the following
		/// statements holds true:<br/>
		/// - The bit initially has the value <code>true</code>, and the
		/// corresponding bit in the specified bit set has the value
		/// <code>false</code>.<br/><br/>
		/// - Thi bit initially has the value <code>false</code>, and the
		/// corresponding bit in the specified bit set has the value
		/// <code>true</code>.
		/// </summary>
		/// <param name="bs">
		/// A bit set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bs"/> cannot be null.
		/// </exception>
		public void XOr(BitSet bs) {
			if (bs == null) {
				throw new ArgumentNullException("bs");
			}

			// Calculate how many words with have in common with the other bit set.
			Int32 wordsInCommon = Math.Min(this._wordsInUse, bs._wordsInUse);
			if (this._wordsInUse < bs._wordsInUse) {
				this.EnsureCapacity(bs._wordsInUse);
				this._wordsInUse = bs._wordsInUse;
			}

			// Perform logical XOR on words in common.
			for (Int32 i = 0; i < wordsInCommon; i++) {
				this._bits[i] ^= bs._bits[i];
			}

			// Copy any remaining words.
			if (wordsInCommon < bs._wordsInUse) {
				Array.Copy(bs._bits, wordsInCommon, this._bits, wordsInCommon, bs._wordsInUse - wordsInCommon);
			}

			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Sets all of the bits in this <see cref="CyrusBuilt.MonoPi.BitSet"/>
		/// instance to <code>false</code>.
		/// </summary>
		public void Clear() {
			Array.Clear(this._bits, 0, this._bits.Length);
			this._wordsInUse = 0;
		}

		/// <summary>
		/// Sets the bit at the specified position (index) to <code>false</code>.
		/// </summary>
		/// <param name="pos">
		/// The index of the bit to be cleared.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="pos"/> cannot be less than zero.
		/// </exception>
		public void Clear(Int32 pos) {
			if (pos < 0) {
				throw new IndexOutOfRangeException("pos cannot be less than zero.");
			}

			Int32 offset = WordIndex(pos);
			if (offset >= this._wordsInUse) {
				return;
			}
	
			this._bits[offset] &= ~(1L << pos);
			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Sets the bits from the specified <b>from</b> index (inclusive)
		/// to the specified <b>to</b> index (exclusive) to <code>false</code>.
		/// </summary>
		/// <param name="from">
		/// The index of the first bit to be cleared.
		/// </param>
		/// <param name="to">
		/// The index after the last bit to be cleared.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> is less than zero - or -
		/// <paramref name="to"/> is less than zero - or -
		/// <paramref name="from"/> is greater than <paramref name="to"/>.
		/// </exception>
		public void Clear(Int32 from, Int32 to) {
			CheckRange(from, to);
			if (from == to) {
				return;
			}

			Int32 startWordIndex = WordIndex(from);
			if (startWordIndex >= this._wordsInUse) {
				return;
			}

			Int32 endWordIndex = WordIndex(to - 1);
			if (endWordIndex >= this._wordsInUse) {
				to = this.Length;
				endWordIndex = this._wordsInUse - 1;
			}

			long firstWordMask = LONG_MASK << from;
			long lastWordMask = LONG_MASK >> -to;
			if (startWordIndex == endWordIndex) {
				// Case 1: Single word.
				this._bits[startWordIndex] &= ~(firstWordMask & lastWordMask);
			}
			else {
				// Case 2: Multiple words.
				// Handle first word.
				this._bits[startWordIndex] &= ~firstWordMask;

				// Handle intermediate words, if any.
				for (Int32 i = startWordIndex + 1; i < endWordIndex; i++) {
					this._bits[i] = 0;
				}

				// Handle last word.
				this._bits[endWordIndex] &= ~lastWordMask;
			}
			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>
		/// 2
		/// </filterpriority>
		public Object Clone() {
			if (!this._sizeIsSticky) {
				this.TrimToSize();
			}

			try {
				BitSet bs = null;
				IFormatter formatter = new BinaryFormatter();
				using (Stream s = new MemoryStream()) {
					formatter.Serialize(s, this);
					s.Seek(0, SeekOrigin.Begin);
					bs = (BitSet)formatter.Deserialize(s);
				}
				bs._bits = (long[])this._bits.Clone();
				bs.CheckInvariants();
				return bs;
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is
		/// equal to the current <see cref="CyrusBuilt.MonoPi.BitSet"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current <see cref="CyrusBuilt.MonoPi.BitSet"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.BitSet"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			BitSet bs = obj as BitSet;
			if (bs == null) {
				return false;
			}

			this.CheckInvariants();
			bs.CheckInvariants();

			if (this._wordsInUse != bs._wordsInUse) {
				return false;
			}

			Boolean result = true;
			for (Int32 i = 0; i < this._wordsInUse; i++) {
				if (this._bits[i] != bs._bits[i]) {
					result = false;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Sets the bit at the specified index to the compliment
		/// of its current value.
		/// </summary>
		/// <param name="index">
		/// The index of the bit to flip.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="index"/> cannot be less than zero.
		/// </exception>
		public void Flip(Int32 index) {
			if (index < 0) {
				throw new IndexOutOfRangeException("index cannot be less than zero.");
			}
			Int32 offset = WordIndex(index);
			this.ExpandTo(offset);
			this._bits[offset] ^= 1L << index;
			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Sets each bit from the specified <paramref name="from"/> (inclusive)
		/// to the specified <paramref name="to"/> (exclusive) to the complement
		/// of its current value.
		/// </summary>
		/// <param name="from">
		/// The index of the first bit to flip.
		/// </param>
		/// <param name="to">
		/// The index after the last bit to flip.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> is less than zero - or -
		/// <paramref name="to"/> is less than zero - or -
		/// <paramref name="from"/> is greater than <paramref name="to"/>.
		/// </exception>
		public void Flip(Int32 from, Int32 to) {
			CheckRange(from, to);
			if (from == to) {
				return;
			}

			Int32 startWordIndex = WordIndex(from);
			Int32 endWordIndex = WordIndex(to - 1);
			this.ExpandTo(endWordIndex);

			long firstWordMask = LONG_MASK << from;
			long lastWordMask = LONG_MASK >> -to;
			if (startWordIndex == endWordIndex) {
				// Case 1: single word
				this._bits[startWordIndex] ^= (firstWordMask & lastWordMask);
			}
			else {
				// Case 2: multiple words
				// Handle first word
				this._bits[startWordIndex] ^= firstWordMask;

				// Handle intermediate words, if any.
				for (Int32 i = startWordIndex + 1; i < endWordIndex; i++) {
					this._bits[i] ^= LONG_MASK;
				}

				// Handle last word.
				this._bits[endWordIndex] ^= lastWordMask;
			}

			this.RecalculateWordsInUse();
			this.CheckInvariants();
		}

		/// <summary>
		/// Gets the value of the bit at the specified index.
		/// </summary>
		/// <param name="pos">
		/// The index at which to get the bit value.
		/// </param>
		/// <returns>
		/// If the requested bit is set, then returns true; Otherwise, false.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="pos"/> cannot be less than zero.
		/// </exception>
		public Boolean Get(Int32 pos) {
			if (pos < 0) {
				throw new IndexOutOfRangeException("pos cannot be less than zero.");
			}

			this.CheckInvariants();
			Int32 offset = WordIndex(pos);
			return ((offset < this._wordsInUse) &&
					((this._bits[pos] & (1L << pos)) != 0));
		}

		/// <summary>
		/// Returns a new bit set composed of bits from this bit set
		/// from the specified <b>from</b> index (inclusive) to the
		/// <b>to</b> index (exclusive).
		/// </summary>
		/// <param name="from">
		/// The index of the first bit to include.
		/// </param>
		/// <param name="to">
		/// The index after the last bit to include.
		/// </param>
		/// <returns>
		/// A new <see cref="CyrusBuilt.MonoPi.BitSet"/> instance
		/// composed of the specified range of bits from this instance.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> is less than zero - or -
		/// <paramref name="to"/> is less than zero - or -
		/// <paramref name="from"/> is greater than <paramref name="to"/>.
		/// </exception>
		public BitSet Get(Int32 from, Int32 to) {
			CheckRange(from, to);
			this.CheckInvariants();

			// If no set bits in range, the return empty BitSet.
			Int32 len = this.Length;
			if ((len <= from) || (from == to)) {
				return new BitSet(0);
			}

			// Optimize.
			if (to > len) {
				to = len;
			}

			BitSet bs = new BitSet(to - from);
			Int32 targetWords = WordIndex(to - from - 1) + 1;
			Int32 sourceIndex = WordIndex(from);
			Boolean aligned = ((from & BIT_INDEX_MASK) == 0);

			// Process all words but the last one.
			for (Int32 i = 0; i < targetWords - 1; i++, sourceIndex++) {
				bs._bits[i] = aligned ? this._bits[sourceIndex] :
					(this._bits[sourceIndex] >> from) |
					(this._bits[sourceIndex + 1] << -from);
			}

			// Process the last word.
			long lastWordMask = LONG_MASK >> -to;
			bs._bits[targetWords - 1] = ((to - 1) & BIT_INDEX_MASK) < (from & BIT_INDEX_MASK) ?
				((this._bits[sourceIndex] >> from) |
			(this._bits[sourceIndex + 1] & lastWordMask) << -from) :
				((this._bits[sourceIndex] & lastWordMask) >> from);

			bs._wordsInUse = targetWords;
			bs.RecalculateWordsInUse();
			bs.CheckInvariants();
			return bs;
		}

		/// <summary>
		/// Sets the bit at the specified index to <code>true</code>.
		/// </summary>
		/// <param name="index">
		/// The index of the bit to set.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="index"/> cannot be less than zero.
		/// </exception>
		public void Set(Int32 index) {
			if (index < 0) {
				throw new IndexOutOfRangeException("index cannot be less than zero.");
			}

			Int32 offset = WordIndex(index);
			this.ExpandTo(offset);
			this._bits[offset] |= (1L << index);  // Restores invariants.
			this.CheckInvariants();
		}

		/// <summary>
		/// Sets the bit at the specified index to the specified value.
		/// </summary>
		/// <param name="index">
		/// The index of the bit to set.
		/// </param>
		/// <param name="value">
		/// The value to set.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="index"/> cannot be less than zero.
		/// </exception>
		public void Set(Int32 index, Boolean value) {
			if (value) {
				this.Set(index);
			}
			else {
				this.Clear(index);
			}
		}

		/// <summary>
		/// Sets the bits from the specified <b>from</b> index (inclusive)
		/// to the specified <b>to</b> index (exclusive) to <code>true</code>.
		/// </summary>
		/// <param name="from">
		/// The index of the first bit to set.
		/// </param>
		/// <param name="to">
		/// The index after the last bit to set.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> is less than zero - or -
		/// <paramref name="to"/> is less than zero - or -
		/// <paramref name="from"/> is greater than <paramref name="to"/>.
		/// </exception>
		public void Set(Int32 from, Int32 to) {
			CheckRange(from, to);
			if (from == to) {
				return;
			}

			Int32 startWordIndex = WordIndex(from);
			Int32 endWordIndex = WordIndex(to - 1);
			this.ExpandTo(endWordIndex);

			long firstWordMask = LONG_MASK << from;
			long lastWordMask = LONG_MASK >> -to;
			if (startWordIndex == endWordIndex) {
				// Case 1: Single word.
				this._bits[startWordIndex] |= (firstWordMask & lastWordMask);
			}
			else {
				// Case 2: multiple words.
				// Handle first word.
				this._bits[startWordIndex] |= firstWordMask;

				// Handle intermediate words, if any.
				for (Int32 i = startWordIndex + 1; i < endWordIndex; i++) {
					this._bits[i] = LONG_MASK;
				}

				// Handle last word (restores invariants).
				this._bits[endWordIndex] |= lastWordMask;
			}
			this.CheckInvariants();
		}

		/// <summary>
		/// Sets the bits from the specified <b>from</b> index (inclusive)
		/// to the specified <b>to</b> index (exclusive) to the
		/// specified value.
		/// </summary>
		/// <param name="from">
		/// The index of the first bit to set.
		/// </param>
		/// <param name="to">
		/// The index after the last bit to set.
		/// </param>
		/// <param name="value">
		/// The value to set.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> is less than zero - or -
		/// <paramref name="to"/> is less than zero - or -
		/// <paramref name="from"/> is greater than <paramref name="to"/>.
		/// </exception>
		public void Set(Int32 from, Int32 to, Boolean value) {
			if (value) {
				this.Set(from, to);
			}
			else {
				this.Clear(from, to);
			}
		}

		/// <summary>
		/// Gets a hash code value for this bit set. The hash code depends
		/// only on which bits are set within this instance.
		/// </summary>
		/// <returns>
		/// The hash code value for this bit set.
		/// </returns>
		public override int GetHashCode() {
			long h = 1234L;
			for (Int32 i = this._bits.Length; --i >= 0;) {
				h ^= this._bits[i] * (i + 1);
			}
			return (Int32)((h >> 32) ^ h);
		}

		/// <summary>
		/// Returns true if the specified bit set has any bits set
		/// to true that are also set to true in this bit set.
		/// </summary>
		/// <param name="bs">
		/// The bit set to intersect with.
		/// </param>
		/// <returns>
		/// true if this instance intersects with the specified
		/// <see cref="CyrusBuilt.MonoPi.BitSet"/>.
		/// </returns>
		public Boolean Intersects(BitSet bs) {
			Boolean goodBits = false;
			Int32 i = Math.Min(this._bits.Length, bs._bits.Length);
			while (--i >= 0) {
				if ((this._bits[i] & bs._bits[i]) != 0) {
					goodBits = true;
					break;
				}
			}
			return goodBits;
		}

		/// <summary>
		/// Returns the index of the first bit that is set to <code>false</code>
		/// that occurs on or after the specified starting index.
		/// </summary>
		/// <param name="from">
		/// The index to start checking from (inclusive).
		/// </param>
		/// <returns>
		/// The index of the next clear bit; Otherwise, -1 if no such bit is found.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> cannot be less than zero.
		/// </exception>
		public Int32 NextClearBit(Int32 from) {
			if (from < 0) {
				throw new IndexOutOfRangeException("'from' index cannot be less than zero.");
			}

			this.CheckInvariants();

			Int32 offset = WordIndex(from);
			if (offset >= this._wordsInUse) {
				return from;
			}

			Int32 result = -1;
			long w = ~this._bits[offset] & (LONG_MASK << from);
			while (true) {
				if (w != 0) {
					result = (offset * BITS_PER_WORD) + NumberOfTrailingZeros(w);
					break;
				}

				if (++offset == this._wordsInUse) {
					result = this._wordsInUse * BITS_PER_WORD;
					break;
				}
				w = ~this._bits[offset];
			}
			return result;
		}

		/// <summary>
		/// Returns the index of the first bit that is set to <code>true</code>
		/// that occurs on or after the specified starting index.
		/// </summary>
		/// <param name="from">
		/// The index to start checking from (inclusive).
		/// </param>
		/// <returns>
		/// The index of the next set bit after the specified index. If no such
		/// bit exists, then returns -1.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="from"/> cannot be less than zero.
		/// </exception>
		public Int32 NextSetBit(Int32 from) {
			if (from < 0) {
				throw new IndexOutOfRangeException("'from' index cannot be less than zero.");
			}

			this.CheckInvariants();

			Int32 offset = WordIndex(from);
			if (offset >= this._wordsInUse) {
				return -1;
			}

			Int32 result = -1;
			long w = this._bits[offset] & (LONG_MASK << from);
			while (true) {
				if (w != 0) {
					result = (offset * BITS_PER_WORD) + NumberOfTrailingZeros(w);
					break;
				}

				if (++offset == this._wordsInUse) {
					break;
				}
				w = this._bits[offset];
			}

			return result;
		}

		/// <summary>
		/// Returns the index of the nearest bit that is set to <code>true</code>
		/// that occurs on or before the specified starting index.
		/// </summary>
		/// <param name="fromIndex">
		/// The index to start checking from (inclusive).
		/// </param>
		/// <returns>
		/// The index of the previous set bit, or -1 if there is no such bit
		/// or if <paramref name="fromIndex"/> is set to -1.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="fromIndex"/> cannot be less than zero.
		/// </exception>
		public Int32 PreviousSetBit(Int32 fromIndex) {
			if (fromIndex < 0) {
				if (fromIndex == -1) {
					return -1;
				}
				throw new IndexOutOfRangeException("fromIndex cannot be less than zero.");
			}

			this.CheckInvariants();
			Int32 offset = WordIndex(fromIndex);
			if (offset >= this._wordsInUse) {
				return this.Length - 1;
			}

			Int32 result = -1;
			long w = this._bits[offset] & (LONG_MASK >> -(fromIndex + 1));
			while (true) {
				if (w != 0) {
					result = (offset + 1) * BITS_PER_WORD - 1 - NumberOfTrailingZeros(w);
					break;
				}

				if (offset-- == 0) {
					break;
				}

				w = this._bits[offset];
			}
			return result;
		}

		/// <summary>
		/// Returns the index of the nearest bit that is set to <code>false</code>
		/// that occurs on or before the specified starting index.
		/// </summary>
		/// <param name="fromIndex">
		/// The index to start checking from (inclusive).
		/// </param>
		/// <returns>
		/// The index of the previous clear bit, or -1 if there is no such
		/// bit or <paramref name="fromIndex"/> is -1.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="fromIndex"/> cannot be less than zero.
		/// </exception>
		public Int32 PreviousClearBit(Int32 fromIndex) {
			if (fromIndex < 0) {
				if (fromIndex == -1) {
					return -1;
				}
				throw new IndexOutOfRangeException("fromIndex cannot be less than zero.");
			}

			this.CheckInvariants();
			Int32 offset = WordIndex(fromIndex);
			if (offset >= this._wordsInUse) {
				return fromIndex;
			}

			Int32 result = -1;
			long w = ~this._bits[offset] & (LONG_MASK >> -(fromIndex + 1));
			while (true) {
				if (w != 0) {
					result = (offset + 1) * BITS_PER_WORD - 1 - NumberOfTrailingZeros(w);
					break;
				}

				if (offset-- == 0) {
					break;
				}
				w = ~this._bits[offset];
			}
			return result;
		}
			
		/// <summary>
		/// This method is used by EnumSet for efficiency. It checks
		/// to see if this instance contains all the same bits
		/// as the specified bit set.
		/// </summary>
		/// <param name="other">
		/// The bit set to check.
		/// </param>
		/// <returns>
		/// true if the specified bit set contains all the same bits;
		/// Otherwise, false.
		/// </returns>
		public Boolean ContainsAll(BitSet other) {
			if (other == null) {
				return false;
			}

			Boolean result = true;
			for (Int32 i = 0; i < other._bits.Length; i++) {
				if ((this._bits[i] & other._bits[i]) != other._bits[i]) {
					result = false;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the
		/// current <see cref="CyrusBuilt.MonoPi.BitSet"/>. For every
		/// index for which this bit set contains a bit in the set
		/// state, the decimal representation of that index is included
		/// in the result. Such indices are listed in order from lowest
		/// to highest, separated by ", " (a comma and a space) and
		/// surrounded by braces, resulting in the usual mathematical
		/// notation for a set of integers.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.BitSet"/>.
		/// </returns>
		public override string ToString() {
			StringBuilder sb = new StringBuilder("{");
			Boolean first = true;
			long bit = 0L;
			long word = 0L;
			for (Int32 i = 0; i < this._bits.Length; ++i) {
				bit = 1L;
				word = this._bits[i];
				if (word == 0) {
					continue;
				}

				for (Int32 j = 0; j < 64; ++j) {
					if ((word & bit) != 0) {
						if (!first) {
							sb.Append(", ");
						}
						sb.Append((64 * i + j).ToString());
						first = false;
					}
					bit <<= 1;
				}
			}

			return sb.Append("}").ToString();
		}

		/// <summary>
		/// Returns a new byte array containing all the bits in
		/// this bit set instance.
		/// </summary>
		/// <returns>
		/// A byte array containing a little-endian representation
		/// of all the bits in this bit set.
		/// </returns>
		public Byte[] ToByteArray() {
			Int32 n = this._wordsInUse;
			if (n == 0) {
				return new Byte[0];
			}

			Int32 len = 8 * (n - 1);
			for (long x = this._bits[n - 1]; x != 0; x >>= 8) {
				len++;
			}

			Byte[] bytes = new Byte[len];
			using (MemoryStream ms = new MemoryStream(bytes)) {
				using (BinaryWriter writer = new BinaryWriter(ms)) {
					for (Int32 i = 0; i < n - 1; i++) {
						writer.Write(this._bits[i]);
					}

					for (long x = this._bits[n - 1]; x != 0L; x >>= 8) {
						writer.Write((Byte)(x & 0xff));
					}
				}
			}
			return bytes;
		}

		/// <summary>
		/// Returns a new array of longs containing all the bits
		/// in this bit set instance.
		/// </summary>
		/// <returns>
		/// A long array containing a little-endian representation
		/// of all the bits in this bit set.
		/// </returns>
		public long[] ToLongArray() {
			long[] copy = { };
			Array.Copy(this._bits, copy, this._wordsInUse);
			return copy;
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Given the specified bit index, returns the word index containing it.
		/// </summary>
		/// <param name="bitIndex">
		/// The bit index.
		/// </param>
		/// <returns>
		/// The word index containing the specified bit index.
		/// </returns>
		private static Int32 WordIndex(Int32 bitIndex) {
			return (bitIndex >> ADDRESS_BITS_PER_WORD);
		}

		/// <summary>
		/// Checks to see if the specified "from" index and "to" index
		/// is a valid range of bit indices.
		/// </summary>
		/// <param name="fromIndex">
		/// The starting index.
		/// </param>
		/// <param name="toIndex">
		/// The ending index.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// <paramref name="fromIndex"/> is less than zero - or -
		/// <paramref name="toIndex"/> is less than zero - or -
		/// <paramref name="fromIndex"/> is greater than <paramref name="toIndex"/>.
		/// </exception>
		private static void CheckRange(Int32 fromIndex, Int32 toIndex) {
			if (fromIndex < 0) {
				throw new IndexOutOfRangeException("fromIndex cannot be less than zero.");
			}

			if (toIndex < 0) {
				throw new IndexOutOfRangeException("toIndex cannot be less than zero.");
			}

			if (fromIndex > toIndex) {
				throw new IndexOutOfRangeException("fromIndex cannot be greater than toIndex.");
			}
		}

		/// <summary>
		/// Returns a new bit set containing all the bits in the specified
		/// array of longs.
		/// </summary>
		/// <param name="words">
		/// A long array containing a little-endian representation of a
		/// sequence of bits to be used as the intial bits of the new
		/// bit set.
		/// </param>
		/// <returns>
		/// A new <see cref="CyrusBuilt.MonoPi.BitSet"/> containing all the
		/// bits in the specified array.
		/// </returns>
		public static BitSet ValueOf(long[] words) {
			// Count the number of bits in the 
			Int32 n = 0;
			for (n = words.Length; n > 0 && words[n - 1] == 0; n--) {
				;
			}

			long[] wordsCopy = { };
			Array.Copy(words, wordsCopy, n);
			return new BitSet(wordsCopy);
		}

		/// <summary>
		/// Returns a new bit set containing all the bits in the specified
		/// byte array.
		/// </summary>
		/// <param name="bytes">
		/// A byte array containing a little-endian representation of a
		/// sequence of bits to be used as the intial set of the new bit set.
		/// </param>
		/// <returns>
		/// A new <see cref="CyrusBuilt.MonoPi.BitSet"/> instance containing
		/// all the bits in the specified array.
		/// </returns>
		public static BitSet ValueOf(Byte[] bytes) {
			Int32 n = 0;
			for (n = bytes.Length; n > 0 && (Int32)bytes.GetValue(n - 1) == 0; n--) {
				;
			}

			long[] words = new long[(n + 7) / 8];
			using (MemoryStream ms = new MemoryStream(bytes)) {
				ms.Capacity = n;
				using (BinaryReader reader = new BinaryReader(ms)) {
					Int32 i = 0;
					while (ms.Position >= 8) {
						words[i++] = reader.ReadInt64();
					}
						
					for (Int32 remaining = (Int32)ms.Position, j = 0; j < remaining; j++) {
						words[i] |= (reader.Read() & 0xffL) << (8 * j);
					}
				}
			}
			return new BitSet(words);
		}

		/// <summary>
		/// Gets the number of trailing zeros in the specified long.
		/// </summary>
		/// <param name="n">
		/// The long value to inspect.
		/// </param>
		/// <returns>
		/// The number of trailing zeros.
		/// </returns>
		private static Int32 NumberOfTrailingZeros(long n) {
			Int32 mask = 1;
			Int32 result = 64;
			for (Int32 i = 0; i < 64; i++, mask <<= 1) {
				if ((n & mask) != 0) {
					result = i;
					break;
				}
			}
			return result;
		}
		#endregion
	}
}

