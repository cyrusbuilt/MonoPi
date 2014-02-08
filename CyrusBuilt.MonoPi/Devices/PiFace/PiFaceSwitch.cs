//
//  PiFaceSwitch.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.Devices.PiFace
{
	/// <summary>
	/// The various switches on the PiFace.
	/// </summary>
	public enum PiFaceSwitch : int
	{
		/// <summary>
		/// Switch 1
		/// </summary>
		S1 = 0,

		/// <summary>
		/// Switch 2
		/// </summary>
		S2 = 1,

		/// <summary>
		/// Switch 3
		/// </summary>
		S3 = 2,

		/// <summary>
		/// Switch 4
		/// </summary>
		S4 = 3
	}
}

