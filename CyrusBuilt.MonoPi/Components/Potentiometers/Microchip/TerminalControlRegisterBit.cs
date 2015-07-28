//
//  TerminalControlRegisters.cs
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
	/// Terminal control register bits.
	/// </summary>
	public enum TerminalControlRegisterBit: int
	{
		/// <summary>
		/// Wiper 0 and 2, Hardware config.
		/// </summary>
		TCON_RH02HW = (1 << 3),

		/// <summary>
		/// Wiper 0 and 2, Pin A.
		/// </summary>
		TCON_RH02A = (1 << 2),

		/// <summary>
		/// Wiper 0 and 2, Pin W.
		/// </summary>
		TCON_RH02W = (1 << 1),

		/// <summary>
		/// Wiper 0 and 2, Pin B.
		/// </summary>
		TCON_RH02B = (1 << 0),

		/// <summary>
		/// Wiper 1 and 3, Hardware config.
		/// </summary>
		TCON_RH13HW = (1 << 7),

		/// <summary>
		/// Wiper 1 and 3, Pin A.
		/// </summary>
		TCON_RH13A = (1 << 6),

		/// <summary>
		/// Wiper 1 and 3, Pin W.
		/// </summary>
		TCON_RH13W = (1 << 5),

		/// <summary>
		/// Wiper 1 and 3, Pin B.
		/// </summary>
		TCON_RH13B = (1 << 4),

		/// <summary>
		/// Null bit.
		/// </summary>
		None = 0
	}
}

