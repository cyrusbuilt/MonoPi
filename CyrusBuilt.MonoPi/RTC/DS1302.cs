//
//  DS1302.cs
//
//  Author:
//       chris brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 CyrusBuilt
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
using System;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.RTC
{
	/// <summary>
	/// A Real-Time Clock (RTC) component for interfacing with a Dallas Semiconductor
	/// DS1302 RTC. This wraps the ds1302 module in wiringPi, and thus requires
	/// the wiringPi.so lib.
	/// </summary>
	public class DS1302 : IDisposable
	{
		#region Fields
		private GpioBase _clockPin = null;
		private GpioBase _dataPin = null;
		private GpioBase _csPin = null;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/>
		/// class with the clock, data, and chip-select pins.
		/// </summary>
		/// <param name="clockPin">
		/// The GPIO pin to use for the clock.
		/// </param>
		/// <param name="dataPin">
		/// The GPIO pin to use for data.
		/// </param>
		/// <param name="csPin">
		/// The GPIO pin to use for chip-select.
		/// </param>
		public DS1302(GpioBase clockPin, GpioBase dataPin, GpioBase csPin) {
			this._clockPin = clockPin;
			this._dataPin = dataPin;
			this._csPin = csPin;
<<<<<<< HEAD
			UnsafeNativeMethods.ds1302setup((Int32)this._clockPin.Pin, (Int32)this._dataPin.Pin, (Int32)this._csPin.Pin);
=======
			UnsafeNativeMethods.ds1302setup((Int32)this._clockPin.InnerPin,
			                                (Int32)this._dataPin.InnerPin,
			                                (Int32)this._csPin.InnerPin);
>>>>>>> aab190be0089803bcb76e6d25884c14980d215f7
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the pin being used for clock.
		/// </summary>
		public GpioBase Clock {
			get { return this._clockPin; }
		}

		/// <summary>
		/// Gets the GPIO pin being used for data.
		/// </summary>
		public GpioBase Data {
			get { return this._dataPin; }
		}

		/// <summary>
		/// Gets the GPIO pin being used for chip-select.
		/// </summary>
		public GpioBase ChipSelect {
			get { return this._csPin; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reads a value from an RTC register or RAM location.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="register">
		/// The register or RAM location to read.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public UInt32 ReadRTC(Int32 register) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}
			return UnsafeNativeMethods.ds1302rtcRead(register);
		}

		/// <summary>
		/// Writes a value to an RTC register or RAM location.
		/// </summary>
		/// <param name="register">
		/// The register or RAM location to write to.
		/// </param>
		/// <param name="data">
		/// The data value to write.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void WriteRTC(Int32 register, UInt32 data) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}
			UnsafeNativeMethods.ds1302rtcWrite(register, data);
		}

		/// <summary>
		/// Reads data from the specified RTC register.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="address">
		/// The RAM address to read.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public UInt32 ReadRAM(Int32 address) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}
			return UnsafeNativeMethods.ds1302ramRead(address);
		}

		/// <summary>
		/// Writes a value to the specified RAM address.
		/// </summary>
		/// <param name="address">
		/// The RAM address to write to.
		/// </param>
		/// <param name="data">
		/// The value to write.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void WriteRAM(Int32 address, UInt32 data) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}
			UnsafeNativeMethods.ds1302ramWrite(address, data);
		}

		/// <summary>
		/// Reads the clock.
		/// </summary>
		/// <returns>
		/// The 8-byte buffer containing the clock data.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public Int32[] ReadClock() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}
<<<<<<< HEAD
			Byte[] buffer = new Byte[8];
			Int32[] bytesAsInts = Array.ConvertAll(buffer, c => (Int32)c);
			UnsafeNativeMethods.ds1302clockRead(bytesAsInts);
			return Array.ConvertAll(bytesAsInts, c => (Byte)c);
=======
			Int32[] buffer = new Int32[8];
			UnsafeNativeMethods.ds1302clockRead(buffer);
			return buffer;
>>>>>>> aab190be0089803bcb76e6d25884c14980d215f7
		}

		/// <summary>
		/// Writes to the clock.
		/// </summary>
		/// <param name="data">
		/// The byte buffer to write to the clock.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="data"/> is not 8 bytes in length.
		/// </exception>
		public void WriteClock(Int32[] data) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.RTC.DS1302");
			}

			if (data.Length != 8) {
				throw new ArgumentException("Byte array must 8 bytes in length.");
			}
			Int32[] bytesAsInts = Array.ConvertAll(data, c => (Int32)c);
			UnsafeNativeMethods.ds1302clockWrite(bytesAsInts);
			Array.Clear(bytesAsInts, 0, bytesAsInts.Length);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/>
		/// object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.RTC.DS1302"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._clockPin != null) {
				this._clockPin.Write(false);
				this._clockPin.Dispose();
				this._clockPin = null;
			}

			if (this._dataPin != null) {
				this._dataPin.Write(false);
				this._dataPin.Dispose();
				this._dataPin = null;
			}

			if (this._csPin != null) {
				this._csPin.Write(false);
				this._csPin.Dispose();
				this._csPin = null;
			}
			this._isDisposed = true;
		}
		#endregion
	}
}

