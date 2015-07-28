//
//  HMC5883LOutputRate.cs
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

namespace CyrusBuilt.MonoPi.Components.Gyroscope.Honeywell
{
	/// <summary>
	/// Possible output rates (resolution).
	/// </summary>
	public enum HMC5883LOutputRate : int
	{
		/// <summary>
		/// 0.75Hz
		/// </summary>
		Rate_0_75_HZ = 0,

		/// <summary>
		/// 1.5Hz
		/// </summary>
		Rate_1_5_HZ = 1,

		/// <summary>
		/// 3Hz
		/// </summary>
		Rate_3_HZ = 2,

		/// <summary>
		/// 7.5Hz
		/// </summary>
		Rate_7_5_HZ = 3,

		/// <summary>
		/// 15Hz
		/// </summary>
		Rate_15_HZ = 4,

		/// <summary>
		/// 30Hz
		/// </summary>
		Rate_30_HZ = 5,

		/// <summary>
		/// 75Hz
		/// </summary>
		Rate_75_HZ = 6
	}
}

