//
//  StringUtils.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 Copyright (c) 2013 CyrusBuilt
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
using System.Text;

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// Static string utility methods.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// The default padding character.
		/// </summary>
		public static Char DEFAULT_PAD_CHAR = ' ';

		/// <summary>
		/// Creates a string from the specified character.
		/// </summary>
		/// <param name="c">
		/// The character to create the string from.
		/// </param>
		/// <param name="length">
		/// The number of instances of the specified character
		/// to construct the string from.
		/// </param>
		public static String Create(Char c, Int32 length) {
			StringBuilder sb = new StringBuilder(length);
			for (Int32 i = 0; i < length; i++) {
				sb.Append(c);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Creates a string from the specified string.
		/// </summary>
		/// <param name="s">
		/// The string to create the string from.
		/// </param>
		/// <param name="length">
		/// The number of instances of the specified string
		/// to construct the string from.
		/// </param>
		public static String Create(String s, Int32 length) {
			StringBuilder sb = new StringBuilder(length + s.Length);
			for (Int32 i = 0; i < length; i++) {
				sb.Append(s);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Creates a string of the specified length.
		/// </summary>
		/// <param name="length">
		/// The length of the string to create.
		/// </param>
		public static String Create(Int32 length) {
			return Create(DEFAULT_PAD_CHAR, length);
		}

		/// <summary>
		/// Pads the left side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The character to pad the specified string with.
		/// </param>
		/// <param name="length">
		/// The number of pad characters to inject.
		/// </param>
		public static String PadLeft(String data, Char pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + length);
			for (Int32 i = 0; i < length; i++) {
				sb.Append(pad);
			}

			sb.Append(data);
			return sb.ToString();
		}

		/// <summary>
		/// Pads the left side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The string to pad the specified string with.
		/// </param>
		/// <param name="length">
		/// The number of pad characters to inject.
		/// </param>
		public static String PadLeft(String data, String pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + (length * pad.Length));
			for (Int32 i = 0; i < length; i++) {
				sb.Append(pad);
			}

			sb.Append(data);
			return sb.ToString();
		}

		/// <summary>
		/// Pads the left side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="length">
		/// The number of pad characters to inject.
		/// </param>
		public static String PadLeft(String data, Int32 length) {
			return PadLeft(data, DEFAULT_PAD_CHAR, length);
		}

		/// <summary>
		/// Pads the right side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The character to pad the string with.
		/// </param>
		/// <param name="length">
		/// The number of padding characters to use.
		/// </param>
		public static String PadRight(String data, Char pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + length);
			sb.Append(data);
			for (Int32 i = 0; i < length; i++) {
				sb.Append(pad);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Pads the right side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The string to pad the string with.
		/// </param>
		/// <param name="length">
		/// The number of padding characters to use.
		/// </param>
		public static String PadRight(String data, String pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + (length * pad.Length));
			sb.Append(data);
			for (Int32 i = 0; i < length; i++) {
				sb.Append(pad);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Pads the right side of the specified string.
		/// </summary>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="length">
		/// The number of padding characters to use.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String PadRight(String data, Int32 length) {
			return PadRight(data, DEFAULT_PAD_CHAR, length);
		}

		/// <summary>
		/// Pads the specified string on both sides.
		/// </summary>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The character to pad the string with.
		/// </param>
		/// <param name="length">
		/// The number of characters to pad on both sides.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String Pad(String data, Char pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + length);
			sb.Append(Create(pad, length));
			sb.Append(data);
			sb.Append(Create(pad, length));
			return sb.ToString();
		}

		/// <summary>
		/// Pads the specified string on both sides.
		/// </summary>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="pad">
		/// The string to pad the string with.
		/// </param>
		/// <param name="length">
		/// The number of strings to pad on both sides.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String Pad(String data, String pad, Int32 length) {
			StringBuilder sb = new StringBuilder(data.Length + length);
			sb.Append(Create(pad, length));
			sb.Append(data);
			sb.Append(Create(pad, length));
			return sb.ToString();
		}

		/// <summary>
		/// Pads the specified string on both sides.
		/// </summary>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="length">
		/// The number of characters to pad on both sides.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String Pad(String data, Int32 length) {
			return Pad(data, DEFAULT_PAD_CHAR, length);
		}

		/// <summary>
		/// Pads the center of the specified string.
		/// </summary>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="c">
		/// The character to pad the center with.
		/// </param>
		/// <param name="length">
		/// The number of characters to pad.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String PadCenter(String data, Char c, Int32 length) {
			if (data.Length < length) {
				Int32 needed = (length - data.Length);
				Int32 padNeeded = (needed / 2);

				StringBuilder sb = new StringBuilder();
				sb.Append(Create(c, padNeeded));
				sb.Append(data);
				sb.Append(Create(c, padNeeded));

				Int32 remaining = (length - sb.Length);
				sb.Append(Create(c, remaining));
				return sb.ToString();
			}
			return data;
		}

		/// <summary>
		/// Pads the center of the specified string.
		/// </summary>
		/// <param name="data">
		/// The string to pad.
		/// </param>
		/// <param name="length">
		/// The number of characters to pad.
		/// </param>
		/// <returns>
		/// The padded version of the string.
		/// </returns>
		public static String PadCenter(String data, Int32 length) {
			return PadCenter(data, DEFAULT_PAD_CHAR, length);
		}
	}
}

