//
//  GateOpenerBase.cs
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
	/// Base class for gate opener abstractions.
	/// </summary>
	public abstract class GateOpenerBase : OpenerDevice, IGateOpener
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Gate.GateOpenerBase"/>
		/// classwith the relay, sensor, and the sensor state that indicates
		/// that the opener has opened.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the opener.
		/// </param>
		/// <param name="sensor">
		/// The reading the state of the opener.
		/// </param>
		/// <param name="openState">
		/// The sensor state that indicates the opener has opened.
		/// </param>
		protected GateOpenerBase(IRelay relay, ISensor doorSensor, SensorState doorSensorOpenState)
			: base(relay, doorSensor, doorSensorOpenState) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Gate.GateOpenerBase"/>
		/// class with the relay, sensor, the sensor state that indicates
		/// that the opener has opened, and the switch that controls the lock.
		/// </summary>
		/// <param name="relay">
		/// The relay that controls the opener.
		/// </param>
		/// <param name="sensor">
		/// The reading the state of the opener.
		/// </param>
		/// <param name="openState">
		/// The sensor state that indicates the opener has opened.
		/// </param>
		/// <param name="lok">
		/// The switch that controls the lock.
		/// </param>
		protected GateOpenerBase(IRelay relay, ISensor doorSensor, SensorState doorSensorOpenState, ISwitch lok)
			: base(relay, doorSensor, doorSensorOpenState, lok) {
		}
	}
}

