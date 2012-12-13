//
//  BaudRates.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// Baud rates.
	/// </summary>
	public enum BaudRates : int
	{
		/// <summary>
		/// 1200 BAUD.
		/// </summary>
		Baud1200,

		/// <summary>
		/// 2400 BAUD.
		/// </summary>
		Baud2400,

		/// <summary>
		/// 9600 BAUD.
		/// </summary>
		Baud9600,

		/// <summary>
		/// 19200 BAUD.
		/// </summary>
		Baud19200,

		/// <summary>
		/// 38400 BAUD.
		/// </summary>
		Baud38400,

		/// <summary>
		/// 57600 BAUD (56k).
		/// </summary>
		Baud57600,

		/// <summary>
		/// 115200 BAUD.
		/// </summary>
		Baud115200,

		/// <summary>
		/// 230400 BAUD.
		/// </summary>
		Baud230400
	}
}

