//
//  RegisterMemoryAddress.cs
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
	/// Register memory addresses.
	/// </summary>
	public enum RegisterMemoryAddress : byte
	{
		/// <summary>
		/// Wiper 0.
		/// </summary>
		WIPER0 = 0x00,

		/// <summary>
		/// Wiper 1.
		/// </summary>
		WIPER1 = 0x01,

		/// <summary>
		/// Wiper 0 non-volatile.
		/// </summary>
		WIPER0_NV = 0x02,

		/// <summary>
		/// Wiper 1 non-volatile.
		/// </summary>
		WIPER1_NV = 0x03,

		/// <summary>
		/// Terminal control for wipers 0 and 1.
		/// </summary>
		TCON01 = 0x04,

		/// <summary>
		/// Wiper 2.
		/// </summary>
		WIPER2 = 0x06,

		/// <summary>
		/// Wiper 3.
		/// </summary>
		WIPER3 = 0x07,

		/// <summary>
		/// Wiper 2 non-volatile.
		/// </summary>
		WIPER2_NV = 0x08,

		/// <summary>
		/// Wiper 3 non-volatile.
		/// </summary>
		WIPER3_NV = 0x09,

		/// <summary>
		/// Terminal control for wipers 2 and 3.
		/// </summary>
		TCON23 = 0x04,

		/// <summary>
		/// No address.
		/// </summary>
		None = 0
	}
}

