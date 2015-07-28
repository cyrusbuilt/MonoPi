//
//  MicrochipPotChannel.cs
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
	/// A channel and instance of MCP45XX or MCP46XX-series potentiometers
	/// can be configured for.
	/// </summary>
	public enum MicrochipPotChannel
	{
		/// <summary>
		/// Channel A. Pins P0A, P0W, and P0B.
		/// </summary>
		A,

		/// <summary>
		/// Channel B. Pins P1A, P1W, and P1B.
		/// </summary>
		B,

		/// <summary>
		/// Channel C. Pins P2A, P2W, and P2B.
		/// </summary>
		C,

		/// <summary>
		/// Channel D. Pins P3A, P3W, and P3B.
		/// </summary>
		D,

		/// <summary>
		/// No channel assignment.
		/// </summary>
		None
	}
}

