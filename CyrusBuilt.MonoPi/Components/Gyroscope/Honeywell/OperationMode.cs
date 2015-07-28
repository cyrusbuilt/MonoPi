//
//  OperationMode.cs
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
	/// Possible operation modes for the Honeywell gyro.
	/// </summary>
	public enum OperationMode : int
	{
		/// <summary>
		/// Continuous sample mode. Continuously takes measurements.
		/// </summary>
		Continuous = 0,

		/// <summary>
		/// Single sample mode. Default power-up mode. In this mode,
		/// the gyro will take a single sample and then switch to
		/// idle mode.
		/// </summary>
		SingleSample = 1,

		/// <summary>
		/// Idle mode (no sampling).
		/// </summary>
		Idle = 2
	}
}

