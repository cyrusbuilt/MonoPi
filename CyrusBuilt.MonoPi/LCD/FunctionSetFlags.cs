//
//  FunctionSetFlags.cs
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
	/// Function set flags.
	/// </summary>
	[Flags]
	public enum FunctionSetFlags : byte
	{
		/// <summary>
		/// Set 4 bit mode.
		/// </summary>
		FourBitMode = 0x00,

		/// <summary>
		/// Set 8 bit mode.
		/// </summary>
		EightBitMode = 0x10,

		/// <summary>
		/// Set one line display.
		/// </summary>
		OneLine = 0x00,

		/// <summary>
		/// Set two line display.
		/// </summary>
		TwoLine = 0x08,

		/// <summary>
		/// Set 5x8 dots.
		/// </summary>
		FiveByEightDots = 0x00,

		/// <summary>
		/// Set 5x1 dots.
		/// </summary>
		FiveByOneDots = 0x04
	}
}

