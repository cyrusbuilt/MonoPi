//
//  DisplayOnOffControl.cs
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

namespace CyrusBuilt.MonoPi.LCD
{
	/// <summary>
	/// Display on/off controls.
	/// </summary>
	[Flags]
	public enum DisplayOnOffControl : byte
	{
		/// <summary>
		/// Turn the display on.
		/// </summary>
		DisplayOn = 0x04,

		/// <summary>
		/// Turn the display off.
		/// </summary>
		DisplayOff = 0x00,

		/// <summary>
		/// Turn the cursor on.
		/// </summary>
		CursorOn = 0x02,

		/// <summary>
		/// Turn the cursor off.
		/// </summary>
		CursorOff = 0x00,

		/// <summary>
		/// Turn cursor blink on.
		/// </summary>
		BlinkOn = 0x01,

		/// <summary>
		/// Turn cursor blink off.
		/// </summary>
		BlinkOff = 0x00
	}
}

