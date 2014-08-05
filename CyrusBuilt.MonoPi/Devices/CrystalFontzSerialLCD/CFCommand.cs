//
//  CFCommand.cs
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

namespace CyrusBuilt.MonoPi.Devices.CrystalFontzSerialLCD
{
	/// <summary>
	/// The CrystalFontz command set.
	/// </summary>
	public enum CFCommand : int
	{
		/// <summary>
		/// Null.
		/// </summary>
		CFNull = 0,

		/// <summary>
		/// Send cursor to home position (Row 0, Column 0 [upper left-hand corner])
		/// </summary>
		CFCursorHome = 1,

		/// <summary>
		/// Hides the contents of the display.
		/// </summary>
		CFHideDisplay = 2,

		/// <summary>
		/// Restores the contents of the display.
		/// </summary>
		CFRestoreDisplay = 3,

		/// <summary>
		/// Hides the cursor.
		/// </summary>
		CFHideCursor = 4,

		/// <summary>
		/// Shows an underline cursor.
		/// </summary>
		CFUnderlineCursor = 5,

		/// <summary>
		/// Shows a block cursor.
		/// </summary>
		CFBlockCursor = 6,

		/// <summary>
		/// Shows an inverting block cursor (system default).
		/// </summary>
		CFInvertBlockCursor = 7,

		/// <summary>
		/// Backspaces one character (destructive).
		/// </summary>
		CFBackspace = 8,

		/// <summary>
		/// Control the boot screen.
		/// </summary>
		CFControlBootScreen = 9,

		/// <summary>
		/// Line feed.
		/// </summary>
		CFLineFeed = 10,

		/// <summary>
		/// Delete character in place.
		/// </summary>
		CFDeleteInPlace = 11,

		/// <summary>
		/// Form feed (clear display).
		/// </summary>
		CFFormFeed = 12,

		/// <summary>
		/// Carriage return.
		/// </summary>
		CFCarriageReturn = 13,

		/// <summary>
		/// Control the backlight.
		/// </summary>
		CFControlBacklight = 14,

		/// <summary>
		/// Control contrast.
		/// </summary>
		CFControlContrast = 15,

		/// <summary>
		/// Set the position of the cursor.
		/// </summary>
		CFSetCursorPosition = 17,

		/// <summary>
		/// Create/update horizontal bar graph.
		/// </summary>
		CFHorizontalBarGraph = 18,

		/// <summary>
		/// Turns text scrolling on.
		/// </summary>
		CFScrollOn = 19,

		/// <summary>
		/// Turns text scrolling off.
		/// </summary>
		CFScrollOff = 20,

		/// <summary>
		/// Set the scrolling marquee characters.
		/// </summary>
		CFSetScrollChars = 21,

		/// <summary>
		/// Enable scrolling marquee.
		/// </summary>
		CFMarqueeEnable = 22,

		/// <summary>
		/// Turns line wrapping on.
		/// </summary>
		CFWrapOn = 23,

		/// <summary>
		/// Turns line wrapping off.
		/// </summary>
		CFWrapOff = 24,

		/// <summary>
		/// Set custom character bitmap.
		/// </summary>
		CFSetCustomBitmap = 25,

		/// <summary>
		/// Reboot the display.
		/// </summary>
		CFReboot = 26,

		/// <summary>
		/// Escape sequence prefix.
		/// </summary>
		CFEscape = 27,

		/// <summary>
		/// Send data directly to the LCD controller.
		/// </summary>
		CFDirectSend = 30,

		/// <summary>
		/// Show the information screen.
		/// </summary>
		CFShowInfo = 31
	}
}

