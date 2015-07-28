//
//  PwmClockDivider.cs
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

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// PWM clock divider values.
	/// </summary>
	public enum PwmClockDivider : uint
	{
		/// <summary>
		/// Divide clock by a factor of 1.
		/// </summary>
		Divisor1 = 1,

		/// <summary>
		/// Divide clock by a factor of 2.
		/// </summary>
		Divisor2 = 2,

		/// <summary>
		/// Divide clock by a factor of 4.
		/// </summary>
		Divisor4 = 4,

		/// <summary>
		/// Divide clock by a factor of 8.
		/// </summary>
		Divisor8 = 8,

		/// <summary>
		/// Divide clock by a factor of 16.
		/// </summary>
		Divisor16 = 16,

		/// <summary>
		/// Divide clock by a factor of 32.
		/// </summary>
		Divisor32 = 32,

		/// <summary>
		/// Divide clock by a factor of 64.
		/// </summary>
		Divisor64 = 64,

		/// <summary>
		/// Divide clock by a factor of 128.
		/// </summary>
		Divisor128 = 128,

		/// <summary>
		/// Divide clock by a factor of 256.
		/// </summary>
		Divisor256 = 256,

		/// <summary>
		/// Divide clock by a factor of 512.
		/// </summary>
		Divisor512 = 512,

		/// <summary>
		/// Divide clock by a factor of 1024.
		/// </summary>
		Divisor1024 = 1024,

		/// <summary>
		/// Divide clock by a factor of 2048.
		/// </summary>
		Divisor2048 = 2048
	}
}

