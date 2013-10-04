//
//  ISensor.cs
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
	/// A sensor abstraction component interface.
	/// </summary>
	public interface ISensor : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this sensor is open.
		/// </summary>
		/// <value>
		/// <c>true</c> if this sensor is open; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOpen { get; }

		/// <summary>
		/// Gets a value indicating whether this sensor is closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this sensor is closed; otherwise, <c>false</c>.
		/// </value>
		Boolean IsClosed { get; }

		/// <summary>
		/// Gets the sensor state.
		/// </summary>
		SensorState State { get; }

		/// <summary>
		/// Gets or sets the name of the sensor.
		/// </summary>
		String Name { get; set; }

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		Object Tag { get; set; }

		/// <summary>
		/// Determines whether this sensor's state is the specified state.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this sensor's state is the specified state; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="state">
		/// The state to check.
		/// </param>
		Boolean IsState(SensorState state);
	}
}

