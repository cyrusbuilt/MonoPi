//
//  BCM2835PwmClockDivider.cs
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

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// 
	/// </summary>
	public enum BCM2835PWMClockDivider : uint
	{
		/// <summary>
		/// 
		/// </summary>
		ClockDivider1 = 1,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider2 = 2,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider4 = 4,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider8 = 8,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider16 = 16,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider32 = 32,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider64 = 64,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider128 = 128,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider256 = 256,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider512 = 512,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider1024 = 1024,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider2048 = 2048,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider4096 = 4096,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider8192 = 8192,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider16384 = 16384,

		/// <summary>
		/// 
		/// </summary>
		ClockDivider32768 = 32768
	}
}

