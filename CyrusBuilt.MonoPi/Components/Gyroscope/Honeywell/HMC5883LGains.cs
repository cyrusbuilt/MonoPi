//
//  HMC5883LGains.cs
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
	/// Possible Gyro gains. Gyro gain is essentially how
	/// aggressively the gyro attempts to correct drift.
	/// </summary>
	public enum HMC5883LGains : int
	{
		/// <summary>
		/// 0.88% gain.
		/// </summary>
		GAIN_0_88_GA = 0,

		/// <summary>
		/// 1.3% gain.
		/// </summary>
		GAIN_1_3_GA = 1,

		/// <summary>
		/// 1.9% gain.
		/// </summary>
		GAIN_1_9_GA = 2,

		/// <summary>
		/// 2.5% gain.
		/// </summary>
		GAIN_2_5_GA = 3,

		/// <summary>
		/// 4.0% gain.
		/// </summary>
		GAIN_4_0_GA = 4,

		/// <summary>
		/// 4.7% gain.
		/// </summary>
		GAIN_4_7_GA = 5,

		/// <summary>
		/// 5.6% gain.
		/// </summary>
		GAIN_5_6_GA = 6,

		/// <summary>
		/// 8.1% gain.
		/// </summary>
		GAIN_8_1_GA = 7
	}
}

