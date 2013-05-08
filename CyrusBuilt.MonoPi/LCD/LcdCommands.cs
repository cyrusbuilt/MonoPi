//
//  LcdCommands.cs
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
	/// Flags for LCD commands.
	/// </summary>
	[Flags]
	public enum LcdCommands : byte
	{
		/// <summary>
		/// Clears the display.
		/// </summary>
		ClearDisplay = 0x01,

		/// <summary>
		/// Return cursor to home position.
		/// </summary>
		ReturnHome = 0x02,

		/// <summary>
		/// Set entry mode.
		/// </summary>
		EntryModeSet = 0x04,

		/// <summary>
		/// Display control.
		/// </summary>
		DisplayControl = 0x08,

		/// <summary>
		/// Shift the cursor.
		/// </summary>
		CursorShift = 0x10,

		/// <summary>
		/// Set function.
		/// </summary>
		FunctionSet = 0x20,

		/// <summary>
		/// Set CG RAM address.
		/// </summary>
		SetCgRamAddr = 0x40,

		/// <summary>
		/// Set DD RAM address.
		/// </summary>
		SetDdRamAddr = 0x80
	}
}

