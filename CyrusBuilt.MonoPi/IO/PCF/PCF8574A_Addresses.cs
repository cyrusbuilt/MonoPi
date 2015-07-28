//
//  PCF8574A_Addresses.cs
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

namespace CyrusBuilt.MonoPi.IO.PCF
{
	/// <summary>
	/// Theses addresses belong to the PCF8574A(P).
	/// </summary>
	public enum PCF8574A_Addresses : int
	{
		/// <summary>
		/// The address 0x38.
		/// </summary>
		Address_0x38 = 0x38,   // 000

		/// <summary>
		/// The address 0x39.
		/// </summary>
		Address_0x39 = 0x39,   // 001

		/// <summary>
		/// The address 0x3A.
		/// </summary>
		Address_0x3A = 0x3A,   // 010

		/// <summary>
		/// The address 0x3B.
		/// </summary>
		Address_0x3B = 0x3B,   // 011

		/// <summary>
		/// The address 0x3C.
		/// </summary>
		Address_0x3C = 0x3C,   // 100

		/// <summary>
		/// The address 0x3D.
		/// </summary>
		Address_0x3D = 0x3D,   // 101

		/// <summary>
		/// The address 0x3E.
		/// </summary>
		Address_0x3E = 0x3E,   // 110

		/// <summary>
		/// The address 0x3F.
		/// </summary>
		Address_0x3F = 0x3F    // 111
	}
}

