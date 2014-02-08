//
//  PiFacePins.cs
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

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// PiFace I/O pins.
	/// </summary>
	public enum PiFacePins : int
	{
		/// <summary>
		/// Output pin 1 (relay 1).
		/// </summary>
		Output00 = 1,

		/// <summary>
		/// Output pin 2 (relay 2).
		/// </summary>
		Output01 = 2,

		/// <summary>
		/// Output pin 3.
		/// </summary>
		Output02 = 4,

		/// <summary>
		/// Output pin 4.
		/// </summary>
		Output03 = 8,

		/// <summary>
		/// Output pin 5.
		/// </summary>
		Output04 = 16,

		/// <summary>
		/// Output pin 6.
		/// </summary>
		Output05 = 32,

		/// <summary>
		/// Output pin 7.
		/// </summary>
		Output06 = 64,

		/// <summary>
		/// Output pin 8.
		/// </summary>
		Output07 = 128,

		/// <summary>
		/// Input pin 1 (switch 1).
		/// </summary>
		Input00 = 1001,

		/// <summary>
		/// Input pin 2 (switch 2).
		/// </summary>
		Input01 = 1002,

		/// <summary>
		/// Input pin 3 (switch 3).
		/// </summary>
		Input02 = 1004,

		/// <summary>
		/// Input pin 4 (switch 4).
		/// </summary>
		Input03 = 1008,

		/// <summary>
		/// Input pin 5.
		/// </summary>
		Input04 = 1016,

		/// <summary>
		/// Input pin 6.
		/// </summary>
		Input05 = 1032,

		/// <summary>
		/// Input pin 7.
		/// </summary>
		Input06 = 1064,

		/// <summary>
		/// Input pin 8.
		/// </summary>
		Input07 = 1128,

		/// <summary>
		/// No pin assignment.
		/// </summary>
		None = 0
	}
}

