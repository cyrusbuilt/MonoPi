//
//  GarageDoorOpenerDevice.cs
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
using CyrusBuilt.MonoPi.Components.Relays;
using CyrusBuilt.MonoPi.Components.Sensors;
using CyrusBuilt.MonoPi.Components.Switches;
using CyrusBuilt.MonoPi.Devices.Access;

namespace CyrusBuilt.MonoPi.Devices.Garage
{
	/// <summary>
	/// A device that is an abstraction of a garage door opener. This is
	/// an implementation of <see cref="CyrusBuilt.MonoPi.Devices.Garage.GarageDoorOpenerBase"/>.
	/// </summary>
	public class GarageDoorOpenerDevice : GarageDoorOpenerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Garage.GarageDoorOpenerDevice"/>
		/// class with the relay, door sensor, and sensor state that 
		/// indicates that the door is open.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the door.
		/// </param>
		/// <param name="doorSensor">
		/// The sensor that indicates the state of the door.
		/// </param>
		/// <param name="doorSensorOpenState">
		/// The sensor state that indicates the door is open.
		/// </param>
		public GarageDoorOpenerDevice(IRelay relay, ISensor doorSensor, SensorState doorSensorOpenState)
			: base(relay, doorSensor, doorSensorOpenState) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Garage.GarageDoorOpenerDevice"/>
		/// class with the relay, door sensor, the sensor state that indicates
		/// that the door is open, and the switch that controls the lock.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the door.
		/// </param>
		/// <param name="doorSensor">
		/// The sensor that indicates the state of the door.
		/// </param>
		/// <param name="doorSensorOpenState">
		/// The sensor state that indicates the door is open.
		/// </param>
		/// <param name="lok">
		/// The switch that controls the lock.
		/// </param>
		public GarageDoorOpenerDevice(IRelay relay, ISensor doorSensor, SensorState doorSensorOpenState, ISwitch lok)
			: base(relay, doorSensor, doorSensorOpenState, lok) {
		}
	}
}

