//
//  LcdModule.cs
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
using System.Text;
using System.Threading;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.LCD
{
	/// <summary>
	/// Hitachi HD44780-based LCD module control class, largely derived from:
	/// Micro Liquid Crystal Library
	/// http://microliquidcrystal.codeplex.com
	/// Appache License Version 2.0
	/// This classes uses the <see cref="ILcdTransferProvider"/> to provide
	/// an interface between the Raspberry Pi and the LCD module via GPIO.
	/// </summary>
	public class LcdModule
	{
		#region Fields
		private static readonly Byte[] _rowOffsets = { 0x00, 0x40, 0x14, 0x54 };
		private readonly ILcdTransferProvider _provider = null;
		private Boolean _showCursor = true;
		private Boolean _blinkCursor = true;
		private Boolean _visible = true;
		// private Boolean _autoScroll = false;         // TODO Implement this?
		private Boolean _backLight = true;
		private Byte _numLines = Byte.MinValue;
		private Byte _numColumns = Byte.MinValue;
		// private Byte _currLine = Byte.MinValue;      // TODO Implement this?
		private Byte _displayFunction = Byte.MinValue;
		private Encoding _encoding = Encoding.UTF8;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.LcdModule"/>
		/// class with the LCD transfer provider.
		/// </summary>
		/// <param name="provider">
		/// The LCD transfer provider.
		/// </param>
		public LcdModule(ILcdTransferProvider provider) {
			if (provider == null) {
				throw new ArgumentNullException("provider");
			}

			this._provider = provider;
			if (this._provider.FourBitMode) {
				this._displayFunction = ((Byte)FunctionSetFlags.FourBitMode |
				                         (Byte)FunctionSetFlags.OneLine |
				                         (Byte)FunctionSetFlags.FiveByEightDots);
			}
			else {
				this._displayFunction = ((Byte)FunctionSetFlags.EightBitMode |
				                         (Byte)FunctionSetFlags.OneLine |
				                         (Byte)FunctionSetFlags.FiveByEightDots);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of rows.
		/// </summary>
		public Int32 Rows {
			get { return Convert.ToInt32(this._numLines); }
		}

		/// <summary>
		/// Gets the number of columns.
		/// </summary>
		public Int32 Columns {
			get { return Convert.ToInt32(this._numColumns); }
		}

		/// <summary>
		/// Gets the LCD transfer provider.
		/// </summary>
		public ILcdTransferProvider Provider {
			get { return this._provider; }
		}

		/// <summary>
		/// Get or set the encoding used to map the string into
		/// bytes codes that are sent LCD. UTF8 is used by default.
		/// </summary>
		public Encoding TextEncoding {
			get { return this._encoding; }
			set { this._encoding = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance
		/// should show or hide the LCD cursor (line) at the
		/// position to which the next character will be written.
		/// </summary>
		public Boolean ShowCursor {
			get { return this._showCursor; }
			set {
				if (this._showCursor != value) {
					this._showCursor = value;
					this.UpdateDisplayControl();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance
		/// should display or hide the blinking block cursor at the
		/// position to which the next character will be written.
		/// </summary>
		public Boolean BlinkCursor {
			get { return this._blinkCursor; }
			set {
				if (this._blinkCursor != value) {
					this._blinkCursor = value;
					this.UpdateDisplayControl();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the LCD display is
		/// turned on or off. This will restore the text (and cursor)
		/// that was on the display.
		/// </summary>
		public Boolean Visible {
			get { return this._visible; }
			set {
				if (this._visible != value) {
					this._visible = value;
					this.UpdateDisplayControl();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the backlight is
		/// turned on or off.
		/// </summary>
		public Boolean BacklightEnabled {
			get { return this._backLight; }
			set {
				if (this._backLight != value) {
					this._backLight = value;
					this.UpdateDisplayControl();
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sends HD44780 LCD interface command.
		/// </summary>
		/// <param name="data">
		/// The byte command to send.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// <see cref="Provider"/> is not specified.
		/// </exception>
		public void SendCommand(Byte data) {
			if (this._provider == null) {
				throw new InvalidOperationException("LCD provider not defined.");
			}
			this._provider.Send(data, PinState.Low, this._backLight);
		}

		/// <summary>
		/// Sends one data byte to the display.
		/// </summary>
		/// <param name="data">
		/// Data.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// <see cref="Provider"/> is not specified.
		/// </exception>
		public void WriteByte(Byte data) {
			if (this._provider == null) {
				throw new InvalidOperationException("LCD provider not defined.");
			}
			this._provider.Send(data, PinState.High, this._backLight);
		}

		/// <summary>
		/// Updates the display control. This method is called when any of
		/// the display properties are changed.
		/// </summary>
		protected void UpdateDisplayControl() {
			Int32 command = (Byte)LcdCommands.DisplayControl;
			command |= (this._visible) ? (Byte)DisplayOnOffControl.DisplayOn : (Byte)DisplayOnOffControl.DisplayOff;
			command |= (this._showCursor) ? (Byte)DisplayOnOffControl.CursorOn : (Byte)DisplayOnOffControl.CursorOff;
			command |= (this._blinkCursor) ? (Byte)DisplayOnOffControl.BlinkOn : (Byte)DisplayOnOffControl.BlinkOff;

			// NOTE: Backlight is updated with with each command.
			this.SendCommand((Byte)command);
		}

		/// <summary>
		/// Creates a custom character (glyph) for use on the LCD.
		/// </summary>
		/// <param name="location">
		/// Which character to create (0 - 7).
		/// </param>
		/// <param name="charmap">
		/// The character's pixel data.
		/// </param>
		/// <param name="offset">
		/// Offset in the charmap where character data is found.
		/// </param>
		/// <remarks>
		/// Up to eight characters of 5x8 pixels are supported (numbered 0 to 7).
		/// The appearance of each custom character is specified by an array of
		/// eight bytes, one for each row. The five least significant bits of each
		/// byte determine the pixels in that row. To display a custom character on
		/// the screen, call <see cref="WriteByte"/> and pass its number.
		/// </remarks>
		public void CreateChar(Int32 location, Byte[] charmap, Int32 offset) {
			location &= 0x07; // We only have 8 locations (0 - 7).
			this.SendCommand((Byte)((Byte)LcdCommands.SetCgRamAddr | (location << 3)));
			for (Int32 i = 0; i < 8; i++) {
				this.WriteByte(charmap[offset + i]);
			}
		}

		/// <summary>
		/// Creates a custom character (glyph) for use on the LCD.
		/// </summary>
		/// <param name="location">
		/// Which character to create (0 - 7).
		/// </param>
		/// <param name="charmap">
		/// The character's pixel data.
		/// </param>
		/// <remarks>
		/// Up to eight characters of 5x8 pixels are supported (numbered 0 to 7).
		/// The appearance of each custom character is specified by an array of
		/// eight bytes, one for each row. The five least significant bits of each
		/// byte determine the pixels in that row. To display a custom character on
		/// the screen, call <see cref="WriteByte"/> and pass its number.
		/// </remarks>
		public void CreateChar(Int32 location, Byte[] charmap) {
			this.CreateChar(location, charmap, 0);
		}

		/// <summary>
		/// Writes a specified number of bytes to the LCD using data from a buffer.
		/// </summary>
		/// <param name="buffer">
		/// The byte array containing data to write to the display.
		/// </param>
		/// <param name="offset">
		/// The zero-base byte offset in the buffer at which to begin copying bytes
		/// to the display.
		/// </param>
		/// <param name="count">
		/// The number of bytes to write.
		/// </param>
		public void Write(Byte[] buffer, Int32 offset, Int32 count) {
			Int32 len = (offset + count);
			for (Int32 i = offset; i < len; i++) {
				this.WriteByte(buffer[i]);
			}
		}

		/// <summary>
		/// Writes text to the LCD.
		/// </summary>
		/// <param name="text">
		/// The text to display.
		/// </param>
		public void Write(String text) {
			Byte[] buffer = this._encoding.GetBytes(text);
			this.Write(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Moves the cursor left or right.
		/// </summary>
		/// <param name="right">
		/// If set to true, move the cursor right; Otherwise, left.
		/// </param>
		public void MoveCursor(Boolean right) {
			this.SendCommand((Byte)(0x10 | ((right) ? 0x04 : 0x00)));
		}

		/// <summary>
		/// Scrolls the contents of the display (text and cursor) one
		/// space to the right.
		/// </summary>
		public void ScrollDisplayRight() {
			this.SendCommand(0x18 | 0x04);
		}

		/// <summary>
		/// Scrolls the contents of the display (text and cursor) one
		/// space to the left.
		/// </summary>
		public void ScrollDisplayLeft() {
			this.SendCommand(0x18 | 0x00);
		}

		/// <summary>
		/// Position the LCD cursor; that is, set the location at which
		/// subsequent text written to the LCD will be displayed.
		/// </summary>
		/// <param name="column">
		/// The column position.
		/// </param>
		/// <param name="row">
		/// The row position.
		/// </param>
		public void SetCursorPosition(Int32 column, Int32 row) {
			if (row > this._numLines) {
				row = (this._numLines - 1);
			}
			Int32 address = (column + _rowOffsets[row]);
			this.SendCommand((Byte)((Byte)LcdCommands.SetDdRamAddr | address));
		}

		/// <summary>
		/// Positions the cursor in the upper-left of the LCD (home position).
		/// That is, use that location in outputting subsequent text to the
		/// display. To also clear the display, use the <see cref="Clear()"/>
		/// method instead.
		/// </summary>
		public void ReturnHome() {
			this.SendCommand((Byte)LcdCommands.ReturnHome);
			Thread.Sleep(2);  // This command takes some time.
		}

		/// <summary>
		/// Clears the LCD screen and positions the cursor in the upper-left
		/// corner of the display.
		/// </summary>
		public void Clear() {
			this.SendCommand((Byte)LcdCommands.ClearDisplay);
			Thread.Sleep(2);  // This command takes some time.
		}

		/// <summary>
		/// Initializes the LCD. Specifies dimensions (width and height)
		/// of the display.
		/// </summary>
		/// <param name="columns">
		/// The number of columns that the display has.
		/// </param>
		/// <param name="lines">
		/// The number of rows the display has.
		/// </param>
		/// <param name="leftToRight">
		/// If set to true left to right, versus right to left.
		/// </param>
		/// <param name="dotSize">
		/// If set true and only one line set, then the font size will
		/// be set 10px high.
		/// </param>
		public void Begin(Int32 columns, Int32 lines, Boolean leftToRight, Boolean dotSize) {
			if (lines > 1) {
				this._displayFunction |= (Byte)FunctionSetFlags.TwoLine;
			}

			//this._currLine = 0;
			this._numLines = Convert.ToByte(lines);
			this._numColumns = Convert.ToByte(columns);

			// For some 1 line displays you can select 10 pixel high font.
			if ((dotSize) && (lines == 1)) {
				this._displayFunction |= (Byte)FunctionSetFlags.FiveByEightDots;
			}

			// LCD controller needs to warm-up time.
			Thread.Sleep(50);

			// rs, rw, and enable should be low by default.
			if (this._provider.FourBitMode) {
				// This is according to the Hitachi HD44780 datasheet.
				// figure 24, pg 46.

				// We start in 8bit mode, try to set 4bit mode.
				this.SendCommand(0x03);
				Thread.Sleep(5);  // Wait minimum 4.1ms.
				this.SendCommand(0x03);
				Thread.Sleep(5);  // Wait minimum 4.1ms.

				// Third go!
				this.SendCommand(0x03);
				Thread.Sleep(5);

				// Finally, set to 4bit interface.
				this.SendCommand(0x02);
			}
			else {
				// This is according to the Hitachi HD44780 datasheet
				// page 45, figure 23.

				// Send function set command sequence.
				this.SendCommand((Byte)((Byte)LcdCommands.FunctionSet | this._displayFunction));
				Thread.Sleep(5);

				// Second try.
				this.SendCommand((Byte)((Byte)LcdCommands.FunctionSet | this._displayFunction));
				Thread.Sleep(5);

				// Third go.
				this.SendCommand((Byte)((Byte)LcdCommands.FunctionSet | this._displayFunction));
			}

			// Finally, set # of lines, font size, etc.
			this.SendCommand((Byte)((Byte)LcdCommands.FunctionSet | this._displayFunction));

			// Turn the display on with no cursor or blinking default.
			this._visible = true;
			this._showCursor = false;
			this._blinkCursor = false;
			this._backLight = true;
			this.UpdateDisplayControl();

			// Clear it off.
			this.Clear();

			// Set the entry mode.
			Byte displayMode = (leftToRight ? (Byte)DisplayEntryModes.EntryLeft : (Byte)DisplayEntryModes.EntryRight);
			displayMode |= (Byte)DisplayEntryModes.EntryShiftDecrement;
			this.SendCommand((Byte)((Byte)LcdCommands.EntryModeSet | displayMode));
		}

		/// <summary>
		/// Initializes the LCD. Specifies dimensions (width and height)
		/// of the display.
		/// </summary>
		/// <param name="columns">
		/// The number of columns that the display has.
		/// </param>
		/// <param name="lines">
		/// The number of rows the display has.
		/// </param>
		public void Begin(Int32 columns, Int32 lines) {
			this.Begin(columns, lines, true, false);
		}
		#endregion
	}
}

