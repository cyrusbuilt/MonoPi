//
//  TM1638.cs
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
//  Derived from https://github.com/cypherkey/RaspberryPi.Net
//  by Aaron Anderson <aanderson@netopia.ca>
//
using System;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.LED
{
	/// <summary>
	/// Controller class for the TM1638/TM1640 board.
	/// It is a port of the TM1638 library by Ricardo Batista
	/// URL: http://code.google.com/p/tm1638-library/
	/// </summary>
	public class TM1638 : TM16XXBase
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LED.TM1638"/>
		/// class with the data pin, clock pin, strobe pin, a flag to set the
		/// display active, and the display intensity (brightness).
		/// </summary>
		/// <param name="data">
		/// The data pin.
		/// </param>
		/// <param name="clock">
		/// The clock pin.
		/// </param>
		/// <param name="strobe">
		/// The strobe pin.
		/// </param>
		/// <param name="active">
		/// Set true to activate the display.
		/// </param>
		/// <param name="intensity">
		/// The display intensity (brightness).
		/// </param>
		public TM1638(GpioBase data, GpioBase clock, GpioBase strobe, Boolean active, Byte intensity)
			: base(data, clock, strobe, 8, active, intensity) {
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the display to hex number.
		/// </summary>
		/// <param name="number">
		/// The number to set.
		/// </param>
		public void SetDisplayToHexNumber(ulong number) {
			base.SetDisplayToString(number.ToString("X"));
		}

		/// <summary>
		/// Sets the display to a decimal number at the specified starting
		/// position
		/// </summary>
		/// <param name="number">
		/// The number to set in the display (if out of range, display will be
		/// cleared).
		/// </param>
		/// <param name="dots">
		/// Set true to turn on dots.
		/// </param>
		/// <param name="startPos">
		/// The starting position to place the number at.
		/// </param>
		/// <param name="leadingZeros">
		/// Set true to lead the number with zeros.
		/// </param>
		public void SetDisplayToDecNumberAt(ulong number, Byte dots, Byte startPos, Boolean leadingZeros) {
			if (number > 99999999L) {
				base.SetDisplayToError();
			}
			else {
				Byte digit = Byte.MinValue;
				Byte pos = Byte.MinValue;
				Boolean ldot = false;
				for (Int32 i = 0; i < (base._displays - startPos); i++) {
					pos = (Byte)(base._displays - i - 1);
					ldot = ((dots  & (1 << i)) != 0);
					if (number != 0) {
						digit = (Byte)(number % 10);
						base.SetDisplayDigit(digit, pos, ldot);
						number /= 10;
					}
					else {
						if (leadingZeros) {
							digit = (Byte)0;
							this.SetDisplayDigit(digit, pos, ldot);
						}
						else {
							this.ClearDisplayDigit(pos, ldot);
						}
					}
				}
			}
		}

		/// <summary>
		/// Sets the display to a decimal number.
		/// </summary>
		/// <param name="number">
		/// The number to set in the display.
		/// </param>
		/// <param name="dots">
		/// Set true to turn on the dots.
		/// </param>
		/// <param name="leadingZeros">
		/// Set true to lead the number with zeros.
		/// </param>
		public void SetDisplayToDecNumber(ulong number, Byte dots, Boolean leadingZeros) {
			this.SetDisplayToDecNumberAt(number, dots, 0, leadingZeros);
		}

		/// <summary>
		/// Sends a character to the display.
		/// </summary>
		/// <param name="pos">
		/// The position at which to set the character.
		/// </param>
		/// <param name="data">
		/// The data (character) to set in the display.
		/// </param>
		/// <param name="dot">
		/// Set true to turn on the dots.
		/// </param>
		public override void SendChar(Byte pos, Byte data, Boolean dot) {
			Byte address = (Byte)(pos << 1);
			Byte ldata = (Byte)(data | (dot ? Convert.ToByte("10000000", 2) : Convert.ToByte("00000000", 2)));
			this.SendData(address, ldata);
		}

		/// <summary>
		/// Sets the display to signed a decimal number.
		/// </summary>
		/// <param name="number">
		/// The signed decimal number to set in the display.
		/// </param>
		/// <param name="dots">
		/// Set true to turn on the dots.
		/// </param>
		/// <param name="leadingZeros">
		/// Set true to lead the number with zeros.
		/// </param>
		public void SetDisplayToSignedDecNumber(long number, Byte dots, Boolean leadingZeros) {
			if (number >= 0) {
				this.SetDisplayToDecNumberAt((ulong)number, dots, 0, leadingZeros);
			}
			else {
				number = -number;
				if (number > 9999999L) {
					base.SetDisplayToError();
				}
				else {
					this.SetDisplayToDecNumberAt((ulong)number, dots, 1, leadingZeros);
					this.SendChar(0, (Byte)CharMap['-'], (dots & 0x80) != 0);
				}
			}
		}

		/// <summary>
		/// Sets the display to a binary number.
		/// </summary>
		/// <param name="number">
		/// The binary number to set in the display.
		/// </param>
		/// <param name="dots">
		/// Set true to turn on the dots.
		/// </param>
		public void SetDisplayToBinNumber(Byte number, Byte dots) {
			Byte digit = Byte.MinValue;
			Byte pos = Byte.MinValue;
			Boolean ldot = false;
			for (Int32 i = 0; i < base._displays; i++) {
				digit = (Byte)((number & (1 << i)) == 0 ? 0 : 1);
				pos = (Byte)(_displays - i - 1);
				ldot = ((dots & (1 << i)) != 0);
				base.SetDisplayDigit(digit, pos, ldot);
			}
		}

		/// <summary>
		/// Sets the color of the character or digit at the specified position.
		/// </summary>
		/// <param name="color">
		/// The color to set the digit/character to.
		/// </param>
		/// <param name="pos">
		/// The position of the character to change the color of.
		/// </param>
		public void SetLed(TM1638LedColor color, Byte pos) {
			base.SendData((Byte)((pos << 1) + 1), (Byte)color);
		}

		/// <summary>
		/// Gets a byte representing the buttons pushed. The display has 8
		/// buttons, each representing one bit in the byte.
		/// </summary>
		/// <returns>
		/// The push buttons.
		/// </returns>
		public Byte GetPushButtons() {
			Byte keys = Byte.MinValue;
			base._strobe.Write(false);
			base.Send(0x42);
			for (Byte i = 0; i < 4; i++) {
				keys |= (Byte)(base.Receive() << i);
			}

			base._strobe.Write(true);
			return keys;
		}
		#endregion
	}
}

