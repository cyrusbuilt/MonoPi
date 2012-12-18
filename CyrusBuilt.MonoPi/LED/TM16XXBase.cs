//
//  TM16XX.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.LED
{
	/// <summary>
	/// 
	/// </summary>
	public class TM16XXBase : IDisposable
	{
		#region Fields
		protected GpioBase _data = null;
		protected GpioBase _clock = null;
		protected GpioBase _strobe = null;
		protected Int32 _displays = 0;
		#endregion

		#region LED Character Map
		/// <summary>
		/// The character map for the seven segment displays.
		/// The bits are displayed by mapping bellow
		///  -- 0 --
		/// |       |
		/// 5       1
		///  -- 6 --
		/// 4       2
		/// |       |
		///  -- 3 --  .7
		/// </summary>
		public readonly static Dictionary<Char, Byte> CharMap = new Dictionary<Char, Byte>() {
			{ ' ', Convert.ToByte("00000000",2) },
			{ '!', Convert.ToByte("10000110",2) },
			{ '"', Convert.ToByte("00100010",2) },
			{ '#', Convert.ToByte("01111110",2) },
			{ '$', Convert.ToByte("01101101",2) },
			{ '%', Convert.ToByte("00000000",2) },
			{ '&', Convert.ToByte("00000000",2) },
			{ '\'', Convert.ToByte("00000010",2) },
			{ '(', Convert.ToByte("00110000",2) },
			{ ')', Convert.ToByte("00000110",2) },
			{ '*', Convert.ToByte("01100011",2) },
			{ '+', Convert.ToByte("00000000",2) },
			{ ',', Convert.ToByte("00000100",2) },
			{ '-', Convert.ToByte("01000000",2) },
			{ '.', Convert.ToByte("10000000",2) },
			{ '/', Convert.ToByte("01010010",2) },
			{ '0', Convert.ToByte("00111111",2) },
			{ '1', Convert.ToByte("00000110",2) },
			{ '2', Convert.ToByte("01011011",2) },
			{ '3', Convert.ToByte("01001111",2) },
			{ '4', Convert.ToByte("01100110",2) },
			{ '5', Convert.ToByte("01101101",2) },
			{ '6', Convert.ToByte("01111101",2) },
			{ '7', Convert.ToByte("00100111",2) },
			{ '8', Convert.ToByte("01111111",2) },
			{ '9', Convert.ToByte("01101111",2) },
			{ ':', Convert.ToByte("00000000",2) },
			{ ';', Convert.ToByte("00000000",2) },
			{ '<', Convert.ToByte("00000000",2) },
			{ '=', Convert.ToByte("01001000",2) },
			{ '>', Convert.ToByte("00000000",2) },
			{ '?', Convert.ToByte("01010011",2) },
			{ '@', Convert.ToByte("01011111",2) },
			{ 'A', Convert.ToByte("01110111",2) },
			{ 'B', Convert.ToByte("01111111",2) },
			{ 'C', Convert.ToByte("00111001",2) },
			{ 'D', Convert.ToByte("00111111",2) },
			{ 'E', Convert.ToByte("01111001",2) },
			{ 'F', Convert.ToByte("01110001",2) },
			{ 'G', Convert.ToByte("00111101",2) },
			{ 'H', Convert.ToByte("01110110",2) },
			{ 'I', Convert.ToByte("00000110",2) },
			{ 'J', Convert.ToByte("00011111",2) },
			{ 'K', Convert.ToByte("01101001",2) },
			{ 'L', Convert.ToByte("00111000",2) },
			{ 'M', Convert.ToByte("00010101",2) },
			{ 'N', Convert.ToByte("00110111",2) },
			{ 'O', Convert.ToByte("00111111",2) },
			{ 'P', Convert.ToByte("01110011",2) },
			{ 'Q', Convert.ToByte("01100111",2) },
			{ 'R', Convert.ToByte("00110001",2) },
			{ 'S', Convert.ToByte("01101101",2) },
			{ 'T', Convert.ToByte("01111000",2) },
			{ 'U', Convert.ToByte("00111110",2) },
			{ 'V', Convert.ToByte("00101010",2) },
			{ 'W', Convert.ToByte("00011101",2) },
			{ 'X', Convert.ToByte("01110110",2) },
			{ 'Y', Convert.ToByte("01101110",2) },
			{ 'Z', Convert.ToByte("01011011",2) },
			{ '[', Convert.ToByte("00111001",2) },
			{ '\\', Convert.ToByte("01100100",2) },
			{ ']', Convert.ToByte("00001111",2) },
			{ '^', Convert.ToByte("00000000",2) },
			{ '_', Convert.ToByte("00001000",2) },
			{ '`', Convert.ToByte("00100000",2) },
			{ 'a', Convert.ToByte("01011111",2) },
			{ 'b', Convert.ToByte("01111100",2) },
			{ 'c', Convert.ToByte("01011000",2) },
			{ 'd', Convert.ToByte("01011110",2) },
			{ 'e', Convert.ToByte("01111011",2) },
			{ 'f', Convert.ToByte("00110001",2) },
			{ 'g', Convert.ToByte("01101111",2) },
			{ 'h', Convert.ToByte("01110100",2) },
			{ 'i', Convert.ToByte("00000100",2) },
			{ 'j', Convert.ToByte("00001110",2) },
			{ 'k', Convert.ToByte("01110101",2) },
			{ 'l', Convert.ToByte("00110000",2) },
			{ 'm', Convert.ToByte("01010101",2) },
			{ 'n', Convert.ToByte("01010100",2) },
			{ 'o', Convert.ToByte("01011100",2) },
			{ 'p', Convert.ToByte("01110011",2) },
			{ 'q', Convert.ToByte("01100111",2) },
			{ 'r', Convert.ToByte("01010000",2) },
			{ 's', Convert.ToByte("01101101",2) },
			{ 't', Convert.ToByte("01111000",2) },
			{ 'u', Convert.ToByte("00011100",2) },
			{ 'v', Convert.ToByte("00101010",2) },
			{ 'w', Convert.ToByte("00011101",2) },
			{ 'x', Convert.ToByte("01110110",2) },
			{ 'y', Convert.ToByte("01101110",2) },
			{ 'z', Convert.ToByte("01000111",2) },
			{ '{', Convert.ToByte("01000110",2) },
			{ '|', Convert.ToByte("00000110",2) },
			{ '}', Convert.ToByte("01110000",2) },
			{ '~', Convert.ToByte("00000001",2) }
		};
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LED.TM16XXBase"/>
		/// class
		/// </summary>
		/// <param name="data">
		/// 
		/// </param>
		/// <param name="clock">
		/// 
		/// </param>
		/// <param name="strobe">
		/// 
		/// </param>
		/// <param name="displays">
		/// 
		/// </param>
		/// <param name="activate">
		/// 
		/// </param>
		/// <param name="intensity">
		/// 
		/// </param>
		public TM16XXBase(GpioBase data, GpioBase clock, GpioBase strobe, Int32 displays, Boolean activate, Int32 intensity) {
			this._data = data;
			this._clock = clock;
			this._strobe = strobe;

			// TODO What is the acceptable range?
			this._displays = displays;

			this._strobe.Write(true);
			this._clock.Write(true);

			// TODO What is the acceptable range of "intensity"?
		}
		#endregion

		#region Properties

		#endregion

		#region Methods
		/// <summary>
		/// Send the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The byte of data to send.
		/// </param>
		public void Send(Byte data) {
			for (Int32 i = 0; i < 8; i++) {
				this._clock.Write(false);
				this._data.Write((data & 1) > 0 ? true : false);
				data >>= 1;
				this._clock.Write(true);
			}
		}

		/// <summary>
		/// Receives data from the display driver.
		/// </summary>
		public Byte Receive() {
			// Pull up on.
			Byte temp = 0;
			this._data.Write(true);

			for (Int32 i = 0; i < 8; i++) {
				temp >>= 1;
				this._clock.Write(false);
				if (this._data.Read()) {
					temp |= 0x80;
				}
				this._clock.Write(true);
			}

			this._data.Write(false);
			return temp;
		}

		/// <summary>
		/// Sends the command.
		/// </summary>
		/// <param name="cmd">
		/// A byte representing the command.
		/// </param>
		public void SendCommand(Byte cmd) {
			this._strobe.Write(false);
			this.Send(cmd);
			this._strobe.Write(true);
		}

		/// <summary>
		/// Sends the specified data to the device.
		/// </summary>
		/// <param name="address">
		/// The address to write the data at.
		/// </param>
		/// <param name="data">
		/// The data to send.
		/// </param>
		public void SendData(Byte address, Byte data) {
			this.SendCommand(0x44);
			this._strobe.Write(false);
			this.Send((Byte)(0xC0 | address));
			this.Send(data);
			this._strobe.Write(true);
		}

		/// <summary>
		/// Clears the display.
		/// </summary>
		public void ClearDisplay() {
			for (Int32 i = 0; i < this._displays; i++) {
				this.SendData((Byte)(i << 1), (Byte)0);
			}
		}

		/// <summary>
		/// Sends the specified character to the display.
		/// </summary>
		/// <param name="pos">
		/// The position to set the character at.
		/// </param>
		/// <param name="data">
		/// The character data to send.
		/// </param>
		/// <param name="dot">
		/// Set true to enable the dot.
		/// </param>
		public abstract void SendChar(Byte pos, Byte data, Boolean dot);

		/// <summary>
		/// Sets the display to the specified string.
		/// </summary>
		/// <param name="s">
		/// The string to set the display to.
		/// </param>
		/// <param name="dots">
		/// Set true to turn on dots.
		/// </param>
		/// <param name="pos">
		/// The character position to start the string at.
		/// </param>
		public void SetDisplayToString(String s, Byte dots, Byte pos) {
			if (String.IsNullOrEmpty(s)) {
				this.ClearDisplay();
				return;
			}

			Byte lpos = null;
			Byte ldata = null;
			Boolean ldot = false;
			Int32 len = s.Length;
			for (Int32 i = 0; i < this._displays; i++) {
				if (i < len) {
					lpos = (Byte)(i + (Int32)pos);
					ldata = (Byte)(CharMap[s[i]]);
					ldot = ((dots & (1 << (this._displays - i - 1))) != 0);
					this.SendChar(lpos, ldata, ldot);
				}
				else {
					break;
				}
			}
		}

		/// <summary>
		/// Sets the display to the specified string.
		/// </summary>
		/// <param name="s">
		/// The string to set the display to.
		/// </param>
		public void SetDisplayToString(String s) {
			this.SetDisplayToString(s, 0, 0);
		}

		/// <summary>
		/// Sets the display to the specified values.
		/// </summary>
		/// <param name="values">
		/// The values to set to the display.
		/// </param>
		/// <param name="size">
		/// 
		/// </param>
		public void SetDisplay(Byte[] values, Int32 size) {
			for (Int32 i = 0; i < size; i++) {
				this.SendChar((Byte)i, values[i], false);
			}
		}

		/// <summary>
		/// Clears the display digit.
		/// </summary>
		/// <param name="pos">
		/// The position to start clearing the display at.
		/// </param>
		/// <param name="dot">
		/// Set true to clear dots.
		/// </param>
		public void ClearDisplayDigit(Byte pos, Boolean dot) {
			this.SendChar(pos, 0, dot);
		}

		/// <summary>
		/// Sets the display to error.
		/// </summary>
		public void SetDisplayToError() {
			Byte[] error = new Byte[5] {
				CharMap['E'],
				CharMap['r'],
				CharMap['r'],
				CharMap['o'],
				CharMap['r']
			};

			this.SetDisplay(error, 5);
			for (Int32 i = 8; i < this._displays; i++) {
				this.ClearDisplayDigit((Byte)i, false);
			}

			Array.Clear(error, 0, error.Length);
		}

		public void SetDisplayDigit(Byte digit, Byte pos, Boolean dot) {
			Char chr = Char.Parse(digit.ToString());

		}
		#endregion
	}
}

