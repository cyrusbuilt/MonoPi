//
//  SensorStateChangedEventArgs.cs
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
	/// Sensor state changed event arguments class.
	/// </summary>
	public class SensorStateChangedEventArgs : EventArgs
	{
		#region Fields
		private ISensor _sensor = null;
		private SensorState _oldState = SensorState.Open;
		private SensorState _newState = SensorState.Open;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorStateChangedEventArgs"/>
		/// class with the sensor that changed state, as well as the old and new states.
		/// </summary>
		/// <param name="sensor">
		/// The sensor that changed state.
		/// </param>
		/// <param name="oldState">
		/// The state of the sensor prior to the change.
		/// </param>
		/// <param name="newState">
		/// The current state of the sensor.
		/// </param>
		public SensorStateChangedEventArgs(ISensor sensor, SensorState oldState, SensorState newState)
			: base() {
			this._sensor = sensor;
			this._oldState = oldState;
			this._newState = newState;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the sensor that changed state.
		/// </summary>
		public ISensor Sensor {
			get { return this._sensor; }
		}

		/// <summary>
		/// Gets the sensor state prior to changing.
		/// </summary>
		public SensorState OldState {
			get { return this._oldState; }
		}

		/// <summary>
		/// Gets the current state of the sensor.
		/// </summary>
		public SensorState NewState {
			get { return this._newState; }
		}
		#endregion
	}
}

