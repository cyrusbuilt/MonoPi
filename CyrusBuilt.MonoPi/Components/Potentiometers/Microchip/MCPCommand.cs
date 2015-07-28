//
//  MCPCommand.cs
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
	/// MCP45XX and MCP46XX commands.
	/// </summary>
	public enum MCPCommand : byte
	{
		/// <summary>
		/// Writes to the device.
		/// </summary>
		WRITE = (0x00 << 2),

		/// <summary>
		/// Increase the resistance.
		/// </summary>
		INCREASE = (0x01 << 2),

		/// <summary>
		/// Decrease the resistance.
		/// </summary>
		DECREASE = (0x02 << 2),

		/// <summary>
		/// Reads the current value.
		/// </summary>
		READ = (0x03 << 2)
	}
}

