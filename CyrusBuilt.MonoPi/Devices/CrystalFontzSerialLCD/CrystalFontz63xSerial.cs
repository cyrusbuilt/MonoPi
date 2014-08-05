//
//  CrystalFontz63xSerial.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2014 Copyright (c) 2013 CyrusBuilt
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
using System.Threading;
using CyrusBuilt.MonoPi.IO;
using CyrusBuilt.MonoPi.IO.Serial;

namespace CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD
{
	/// <summary>
	/// Provides an API for manipulating a CrystalFontz 632/634
	/// serial LCD display. This API currently *only* works with
	/// serial (SPI not yet supported) displays that are 16x2
	/// (16 column, 2 row).
	/// </summary>
	public class CrystalFontz63xSerial
	{
		#region Display Constants
		/// <summary>
		/// Minimum contrast level.
		/// </summary>
		public const UInt16 CONTRAST_MIN = 0;

		/// <summary>
		/// Maximum contrast level.
		/// </summary>
		public const UInt16 CONTRAST_MAX = 100;

		/// <summary>
		/// Default contrast level.
		/// </summary>
		public const UInt16 CONTRAST_DEFAULT = 50;

		/// <summary>
		/// Minimum brigthness level.
		/// </summary>
		public const UInt16 BRIGHTNESS_MIN = 0;

		/// <summary>
		/// Maximum brightness level.
		/// </summary>
		public const UInt16 BRIGHTNESS_MAX = 100;

		/// <summary>
		/// The default brightness level.
		/// </summary>
		public const UInt16 BRIGHTNESS_DEFAULT = BRIGHTNESS_MAX;
		#endregion

		#region Fields
		private IRaspiGpio _backlightPin = null;
		private Boolean _isDisposed = false;
		private Boolean _blEnabled = false;
		private Boolean _hidden = false;
		private Boolean _scroll = false;
		private Boolean _wrap = false;
		private CFCursorType _cursor = CFCursorType.InvertingBlock;
		private UInt16 _contrast = CONTRAST_DEFAULT;
		private UInt16 _brightness = BRIGHTNESS_DEFAULT;
		private Rs232SerialPort _lcd = null;
		private BaudRates _baud = BaudRates.Baud9600;
		private DisplayType _type = DisplayType.SixteenByTwo;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// class. This is the default constructor.
		/// </summary>
		public CrystalFontz63xSerial() {
			this._lcd = new Rs232SerialPort(this._baud);
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// class with a GPIO pin to use to control the backlight.
		/// </summary>
		/// <param name="backlightPin">
		/// The GPIO pin to use to control the backlight.
		/// </param>
		public CrystalFontz63xSerial(IRaspiGpio backlightPin) {
			this._lcd = new Rs232SerialPort(this._baud);
			this._backlightPin = backlightPin;
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// class with the BAUD rate to negotiate with display.
		/// </summary>
		/// <param name="baud">
		/// The BAUD rate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// BAUD rate cannot be less than 2400 or greater than 19200.
		/// </exception>
		public CrystalFontz63xSerial(BaudRates baud) {
			if ((Int32)baud < 2400) {
				throw new ArgumentOutOfRangeException("baud", "Cannot be less than 2400 baud.");
			}

			if ((Int32)baud > 19200) {
				throw new ArgumentOutOfRangeException("baud", "Cannot be greater than 19200 baud.");
			}
			this._baud = baud;
			this._lcd = new Rs232SerialPort(baud);
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// class with the GPIO pin to control the backlight with
		/// and the BAUD rate to negotiate with the display.
		/// </summary>
		/// <param name="backlightPin">
		/// The GPIO pin to use to control the backlight.
		/// </param>
		/// <param name="baud">
		/// The BAUD rate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// BAUD rate cannot be less than 2400 or greater than 19200.
		/// </exception>
		public CrystalFontz63xSerial(IRaspiGpio backlightPin, BaudRates baud) {
			this._baud = baud;
			if ((Int32)baud < 2400) {
				throw new ArgumentOutOfRangeException("baud", "Cannot be less than 2400 baud.");
			}

			if ((Int32)baud > 19200) {
				throw new ArgumentOutOfRangeException("baud", "Cannot be greater than 19200 baud.");
			}
			this._lcd = new Rs232SerialPort(baud);
			this._backlightPin = backlightPin;
		}

		/// <summary>
		/// Releases all resource used by the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/> object.
		/// </summary>
		/// <param name="disposing">
		/// If set to <c>true</c> disposing.
		/// </param>
		private void Dispose(Boolean disposing) {
			if (this._isDisposed) {
				return;
			}

			if (disposing) {
				if (this._lcd != null) {
					this._lcd.Dispose();
				}

				if (this._backlightPin != null) {
					this._backlightPin.Dispose();
				}
			}

			this._isDisposed = true;
		}

		/// <summary>
		/// Releases all resource used by the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>.
		/// The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must release all
		/// references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// was occupying.
		/// </remarks>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~CrystalFontz63xSerial() {
			this.Dispose(false);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether the backlight is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if backlight enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean BacklightEnabled {
			get { return this._blEnabled; }
			set {
				if (this._blEnabled != value) {
					this._blEnabled = value;
					if (this._blEnabled) {
						// If user provided a backlight pin, then turn it on.
						// Otherwise, just set the brighness all the way up.
						if (this._backlightPin != null) {
							this._backlightPin.Write(true);
						}
						else {
							Byte[] buf = new Byte[2];
							buf[0] = (Byte)CFCommand.CFControlBacklight;
							buf[1] = (Byte)BRIGHTNESS_MAX;
							this.SendCommand(buf);
							Array.Clear(buf, 0, buf.Length);
						}
					}
					else {
						// If user provided a backlight pin, then turn it off.
						// Otherwise, just set the brightness all the way down.
						if (this._backlightPin != null) {
							this._backlightPin.Write(false);
						}
						else {
							Byte[] buf = new Byte[2];
							buf[0] = (Byte)CFCommand.CFControlBacklight;
							buf[1] = (Byte)BRIGHTNESS_MIN;
							this.SendCommand(buf);
							Array.Clear(buf, 0, buf.Length);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the display is hidden.
		/// </summary>
		/// <value>
		/// <c>true</c> if display hidden; otherwise, <c>false</c>.
		/// </value>
		public Boolean DisplayHidden {
			get { return this._hidden; }
			set {
				if (this._hidden != value) {
					this._hidden = value;
					if (this._hidden) {
						this.SendCommand((Byte)CFCommand.CFHideCursor);
					}
					else {
						this.SendCommand((Byte)CFCommand.CFRestoreDisplay);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the cursor.
		/// </summary>
		/// <value>
		/// The cursor type to use.
		/// </value>
		public CFCursorType Cursor {
			get { return this._cursor; }
			set {
				if (this._cursor != value) {
					this._cursor = value;
					switch (this._cursor) {
						case CFCursorType.Hidden:
							this.SendCommand((Byte)CFCommand.CFHideCursor);
							break;
						case CFCursorType.Underline:
							this.SendCommand((Byte)CFCommand.CFUnderlineCursor);
							break;
						case CFCursorType.Block:
							this.SendCommand((Byte)CFCommand.CFBlockCursor);
							break;
						case CFCursorType.InvertingBlock:
							this.SendCommand((Byte)CFCommand.CFInvertBlockCursor);
							break;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether or not the cursor is visible.
		/// </summary>
		public Boolean IsCursorShown {
			get { return (this._cursor != CFCursorType.Hidden); }
		}

		/// <summary>
		/// Gets or sets the contrast level.
		/// </summary>
		/// <value>
		/// The contrast value.
		/// </value>
		public UInt16 Contrast {
			get { return this._contrast; }
			set {
				if (this._contrast != value) {
					if (value < CONTRAST_MIN) {
						value = CONTRAST_MIN;
					}

					if (value > CONTRAST_MAX) {
						value = CONTRAST_MAX;
					}

					this._contrast = value;
					Byte[] buf = new Byte[2];
					buf[0] = (Byte)CFCommand.CFControlContrast;
					buf[1] = (Byte)this._contrast;
					this.SendCommand(buf);
					Array.Clear(buf, 0, buf.Length);
				}
			}
		}

		/// <summary>
		/// Gets or sets the brightness level.
		/// </summary>
		/// <value>
		/// The brightness level.
		/// </value>
		public UInt16 Brightness {
			get { return this._brightness; }
			set {
				if (this._brightness != value) {
					if (value < BRIGHTNESS_MIN) {
						value = BRIGHTNESS_MIN;
					}

					if (value > BRIGHTNESS_MAX) {
						value = BRIGHTNESS_MAX;
					}

					this._brightness = value;
					Byte[] buf = new Byte[2];
					buf[0] = (Byte)CFCommand.CFControlBacklight;
					buf[1] = (Byte)this._brightness;
					this.SendCommand(buf);
					Array.Clear(buf, 0, buf.Length);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether scrolling is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if scroll enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean ScrollEnabled {
			get { return this._scroll; }
			set {
				if (this._scroll != value) {
					this._scroll = value;
					if (this._scroll) {
						this.SendCommand((Byte)CFCommand.CFScrollOn);
					}
					else {
						this.SendCommand((Byte)CFCommand.CFScrollOff);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether or not line wrapping is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if line wrapping is on; otherwise, <c>false</c>.
		/// </value>
		public Boolean LineWrap {
			get { return this._wrap; }
			set {
				if (this._wrap != value) {
					this._wrap = value;
					if (this._wrap) {
						this.SendCommand((Byte)CFCommand.CFWrapOn);
					}
					else {
						this.SendCommand((Byte)CFCommand.CFWrapOff);
					}
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sends the specified command to the display controller.
		/// </summary>
		/// <param name="cmd">
		/// The command to send.
		/// </param>
		private void SendCommand(Byte cmd) {
			this._lcd.Write(cmd);
		}

		/// <summary>
		/// Sends the specified command buffer to the display controller.
		/// </summary>
		/// <param name="buffer">
		/// The command buffer to send.
		/// </param>
		private void SendCommand(Byte[] buffer) {
			for (Int32 i = 0; i < buffer.Length; i++) {
				this._lcd.Write(buffer[i]);
			}
		}

		/// <summary>
		/// Begins communication with the display.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Cannot initialize the backlight control pin because it is configured as an input.
		/// </exception>
		public void Begin() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD.CrystalFontz63xSerial");
			}

			if (this._lcd.IsOpen) {
				return;
			}

			this._lcd.Open();
			if (this._backlightPin != null) {
				if (this._backlightPin.Direction == PinDirection.IN) {
					throw new InvalidOperationException("Backlight pin MUST be defined as an output, not an input.");
				}
			}
		}

		/// <summary>
		/// Begins communication with the display.
		/// </summary>
		/// <param name="type">
		/// The display type
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Cannot initialize the backlight control pin because it is configured as an input.
		/// </exception>
		public void Begin(DisplayType type) {
			this._type = type;
			this.Begin();
		}

		/// <summary>
		/// Ends communication with the display.
		/// </summary>
		public void End() {
			this._lcd.Close();
			if (this._backlightPin != null) {
				if (this._backlightPin.Direction == PinDirection.OUT) {
					this._backlightPin.Write(false);
				}
			}
		}

		/// <summary>
		/// Reboot the LCD display. This should only be used to
		/// return the display to a usable state, perhaps due to invalid contrast or
		/// brightness values or if the display is printing garbage. However, this
		/// method is not likely to succeed if the display's controller is in an
		/// inoperable or indeterminate state. In such cases, it is best to power-cycle
		/// the controller instead.
		/// </summary>
		public void RebootDisplay() {
			Byte[] buf = new Byte[2];
			buf[0] = (Byte)CFCommand.CFReboot;
			// The manual indicates this command needs to be sent twice.
			buf[1] = (Byte)CFCommand.CFReboot;
			this.SendCommand(buf);
			Array.Clear(buf, 0, buf.Length);
		}

		/// <summary>
		/// Clear the contents of the display and return the
		/// cursor to its home position (upper left corner, or position 0, 0).
		/// </summary>
		public void ClearDisplay() {
			this.SendCommand((Byte)CFCommand.CFFormFeed);
			Thread.Sleep(20);
			this.SendCommand((Byte)CFCommand.CFCursorHome);
			Thread.Sleep(20);
		}

		/// <summary>
		/// Returns the cursor to its home position (upper left corner).
		/// </summary>
		public void CursorHome() {
			this.SendCommand((Byte)CFCommand.CFCursorHome);
			Thread.Sleep(10);
		}

		/// <summary>
		/// Moves the cursor back one space and erases the character
		/// in that space.
		/// </summary>
		public void Backspace() {
			this.SendCommand((Byte)CFCommand.CFBackspace);
		}

		/// <summary>
		/// Moves the cursor down one row. If scrolling is enabled and
		/// the cursor is in the bottom row, the display will scroll up one row and the
		/// bottom row will be cleared. If scrolling is NOT enabled, and the cursor is
		/// in the bottom row, then it will wrap up to the same character position on
		/// the top row.
		/// </summary>
		public void LineFeed() {
			this.SendCommand((Byte)CFCommand.CFLineFeed);
			Thread.Sleep(10);
		}

		/// <summary>
		/// Deletes the character at the current cursor position
		/// but does not move the cursor.
		/// </summary>
		public void DeleteInPlace() {
			this.SendCommand((Byte)CFCommand.CFDeleteInPlace);
		}

		/// <summary>
		/// Clears the display. All data is erased.
		/// </summary>
		public void FormFeed() {
			this.SendCommand((Byte)CFCommand.CFFormFeed);
			Thread.Sleep(10);
		}

		/// <summary>
		/// Moves the cursor to the left-most column of the
		/// current row.
		/// </summary>
		public void CarriageReturn() {
			this.SendCommand((Byte)CFCommand.CFCarriageReturn);
		}

		/// <summary>
		/// Sets the cursor position.
		/// </summary>
		/// <param name="column">
		/// The column to put the cursor in (0 - 15).
		/// </param>
		/// <param name="row">
		/// The row to put the cursor in (0 or 1).
		/// </param>
		public void SetCursorPosition(UInt16 column, UInt16 row) {
			if (column < 0) {
				column = 0;
			}

			if (row < 0) {
				row = 0;
			}

			switch (this._type) {
				case DisplayType.SixteenByTwo:
					if (column > 15) {
						column = 15;
					}

					if (row > 1) {
						row = 1;
					}
					break;
				case DisplayType.TwentyByFour:
					if (column > 19) {
						column = 19;
					}

					if (row > 3) {
						row = 3;
					}
					break;
			}

			Byte[] buf = new Byte[3];
			buf[0] = (Byte)CFCommand.CFSetCursorPosition;
			buf[1] = (Byte)column;
			buf[2] = (Byte)row;
			this.SendCommand(buf);
			Array.Clear(buf, 0, buf.Length);
		}

		/// <summary>
		/// Displays/updates a horizontal bar graph.
		/// </summary>
		/// <param name="index">
		/// The graph index to use.
		/// </param>
		/// <param name="style">
		/// The graph style to use.
		/// </param>
		/// <param name="startCol">
		/// The starting column for the graph (0 - 15).
		/// </param>
		/// <param name="endCol">
		/// The ending column for the graph (0 - 15).
		/// </param>
		/// <param name="len">
		/// The graph bar length (should be a value between startCol and endCol).
		/// </param>
		/// <param name="row">
		/// The row to place the bar graph in (0 or 1).
		/// </param>
		public void SetBarGraph(GraphIndices index, GraphStyle style, UInt16 startCol, UInt16 endCol, UInt16 len, UInt16 row) {
			// Make sure start and end columns are valid. Also make sure the row is legit.
			if (startCol < 0) {
				startCol = 0;
			}

			if (endCol < 0) {
				endCol = 0;
			}

			if (row < 0) {
				row = 0;
			}

			switch (this._type) {
				case DisplayType.SixteenByTwo:
					if (startCol > 15) {
						startCol = 15;
					}

					if (endCol > 15) {
						endCol = 15;
					}

					if (row > 1) {
						row = 1;
					}
					break;
				case DisplayType.TwentyByFour:
					if (startCol > 19) {
						startCol = 19;
					}

					if (endCol > 19) {
						endCol = 19;
					}

					if (row > 3) {
						row = 3;
					}
					break;
			}

			if (endCol <= startCol) {
				throw new IndexOutOfRangeException("End column must be greater than start column.");
			}

			// Create the command buffer with all params.
			Byte[] buf = new Byte[7];
			buf[0] = (Byte)CFCommand.CFHorizontalBarGraph;
			buf[1] = (Byte)index;
			buf[2] = (Byte)style;
			buf[3] = (Byte)startCol;
			buf[4] = (Byte)endCol;
			buf[5] = (Byte)len;
			buf[6] = (Byte)row;

			// Send the command and destroy the buffer.
			this.SendCommand(buf);
			Array.Clear(buf, 0, buf.Length);
		}

		/// <summary>
		/// Displays the LCD controller info screen.
		/// </summary>
		public void ShowInfoScreen() {
			this.ClearDisplay();
			this.SendCommand((Byte)CFCommand.CFShowInfo);
		}

		/// <summary>
		/// Prints text to the display.
		/// </summary>
		/// <param name="text">
		/// The text to print.
		/// </param>
		public void Print(String text) {
			this._lcd.PutString(text);
		}

		/// <summary>
		/// Carriage return, line feed. This will move the
		/// cursor to the left-most column of the next row (same as calling
		/// <see cref="CarriageReturn"/> followed by <see cref="LineFeed"/>).
		/// </summary>
		public void CrLf() {
			this.CarriageReturn();
			this.LineFeed();
		}
		#endregion
	}
}

