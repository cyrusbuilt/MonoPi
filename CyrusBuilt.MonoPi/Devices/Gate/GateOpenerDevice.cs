//
//  GateOpenerDevice.cs
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

namespace CyrusBuilt.MonoPi.Devices.Gate
{
	/// <summary>
	/// An abstraction of a gate opener device. This is an implementation of the
	/// <see cref="CyrusBuilt.MonoPi.Devices.Gate.GateOpenerBase"/>.
	/// </summary>
	public class GateOpenerDevice : GateOpenerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Gate.GateOpenerDevice"/>
		/// class with the relay, gate sensor, and sensor state that 
		/// indicates that the gate is open.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the gate.
		/// </param>
		/// <param name="gateSensor">
		/// The sensor that indicates the state of the gate.
		/// </param>
		/// <param name="doorSensorOpenState">
		/// The sensor state that indicates the gate is open.
		/// </param>
		public GateOpenerDevice(IRelay relay, ISensor gateSensor, SensorState gateSensorOpenState)
			: base(relay, gateSensor, gateSensorOpenState ){
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Gate.GateOpenerDevice"/>
		/// class with the relay, gate sensor, the sensor state that indicates
		/// that the gate is open, and the switch that controls the lock.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the gate.
		/// </param>
		/// <param name="doorSensor">
		/// The sensor that indicates the state of the gate.
		/// </param>
		/// <param name="doorSensorOpenState">
		/// The sensor state that indicates the gate is open.
		/// </param>
		/// <param name="lok">
		/// The switch that controls the lock.
		/// </param>
		public GateOpenerDevice(IRelay relay, ISensor gateSensor, SensorState gateSensorOpenState, ISwitch lok)
			: base(relay, gateSensor, gateSensorOpenState, lok) {
		}
	}
}

