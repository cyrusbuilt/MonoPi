//
//  IMotionSensor.cs
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

namespace CyrusBuilt.MonoPi.Components.Sensors
{
	/// <summary>
	/// A motion sensor abstraction component interface.
	/// </summary>
	public interface IMotionSensor : IComponent
	{
		/// <summary>
		/// Occurs when motion is detected.
		/// </summary>
		event MotionDetectionEventHandler MotionDetectionStateChanged;

		/// <summary>
		/// Gets the timestamp of the last time motion was detected.
		/// </summary>
		DateTime LastMotionTimestamp { get; }

		/// <summary>
		/// Gets the timestamp of the last time inactivity was detected.
		/// </summary>
		DateTime LastInactivityTimestamp { get; }

		/// <summary>
		/// Gets a value indicating whether motion was detected.
		/// </summary>
		/// <value>
		/// <c>true</c> if motion detected; otherwise, <c>false</c>.
		/// </value>
		Boolean MotionDetected { get; }
	}
}

