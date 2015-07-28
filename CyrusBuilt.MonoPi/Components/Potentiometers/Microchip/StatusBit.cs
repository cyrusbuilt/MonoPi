//
//  StatusBit.cs
//
//  Author:
//       Chris.Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2015 Chris.Brunner
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

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// Device status bits.
	/// </summary>
	public enum StatusBit : int
	{
		/// <summary>
		/// Reserved mask.
		/// </summary>
		RESERVED_MASK = unchecked((Int32)0x0000111110000),

		/// <summary>
		/// Reserved value.
		/// </summary>
		RESERVED_VALUE = unchecked((Int32)0x0000111110000),

		/// <summary>
		/// EEPROM write is active.
		/// </summary>
		EEPROM_WRITEACTIVE = 0x1000,

		/// <summary>
		/// Wiper lock 1 enabled.
		/// </summary>
		WIPER_LOCK1 = 0x0100,

		/// <summary>
		/// Wiper lock 0 enabled.
		/// </summary>
		WIPER_LOCK0 = 0x0010,

		/// <summary>
		/// EEPROM write-protected.
		/// </summary>
		EEPROM_WRITEPROTECTION = 0x0001,

		/// <summary>
		/// Null bit.
		/// </summary>
		None = 0
	}
}

