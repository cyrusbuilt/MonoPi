﻿//
//  CFCursorType.cs
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
	/// LCD cursor types.
	/// </summary>
	public enum CFCursorType : int
	{
		/// <summary>
		/// The cursor is hidden.
		/// </summary>
		Hidden = 0,

		/// <summary>
		/// A non-blinking underline cursor at the current position.
		/// </summary>
		Underline = 1,

		/// <summary>
		/// A blinking block cursor at the current position.
		/// </summary>
		Block = 2,

		/// <summary>
		/// A blinking block cursor at the current position. This cursor
		/// inverts the character rather than replacing the character with
		/// a block (default cursor style at boot).
		/// </summary>
		InvertingBlock = 3
	}
}

