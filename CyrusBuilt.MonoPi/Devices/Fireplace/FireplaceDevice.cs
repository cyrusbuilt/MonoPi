//
//  FireplaceDevice.cs
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
using CyrusBuilt.MonoPi.Components.Relays;
using CyrusBuilt.MonoPi.Components.Sensors;

namespace CyrusBuilt.MonoPi.Devices.Fireplace
{
	/// <summary>
	/// A device that is an abstraction of a gas fireplace. This in an
	/// implementation of <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/>.
	/// </summary>
	public class FireplaceDevice : FireplaceBase
	{
		#region Fields
		private IRelay _controlRelay = null;
		private RelayState _fireplaceOnRelayState = RelayState.Open;
		private ISensor _pilotLightSensor = null;
		private SensorState _pilotLightOnSensorState = SensorState.Open;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/>
		/// class with the relay used to control the fireplace, the
		/// relay state used to consider the fireplace to be "on", the
		/// sensor used to detect the pilot light, and the sensor state
		/// in which to consider the pilot light to be "on".
		/// </summary>
		/// <param name="controlRelay">
		/// The control relay.
		/// </param>
		/// <param name="onRelayState">
		/// The relay state used to consider the fireplace to be "on".
		/// </param>
		/// <param name="pilotLightSensor">
		/// The pilot light sensor.
		/// </param>
		/// <param name="pilotOnState">
		/// The pilot light state used to consider the pilot light to be "on".
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="controlRelay"/> cannot be null.
		/// </exception>
		public FireplaceDevice(IRelay controlRelay, RelayState onRelayState, ISensor pilotLightSensor, SensorState pilotOnState)
			: base() {
			if (controlRelay == null) {
				throw new ArgumentNullException("controlRelay");
			}

			this._controlRelay = controlRelay;
			this._fireplaceOnRelayState = onRelayState;
			this._pilotLightSensor = pilotLightSensor;
			this._pilotLightOnSensorState = pilotOnState;
			this._controlRelay.StateChanged += this.InternalHandleRelayStateChange;
			if (this._pilotLightSensor != null) {
				this._pilotLightSensor.StateChanged += this.InternalHandleSensorStateChange;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/>
		/// class with the relay used to control the fireplace and the
		/// sensor used to detect whether or not the pilot light is lit.
		/// </summary>
		/// <param name="controlRelay">
		/// The control relay.
		/// </param>
		/// <param name="pilotLightSensor">
		/// The pilot light sensor.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="controlRelay"/> cannot be null.
		/// </exception>
		public FireplaceDevice(IRelay controlRelay, ISensor pilotLightSensor)
			: this(controlRelay, RelayState.Closed, pilotLightSensor, SensorState.Closed) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/>
		/// class with the relay used to control the fireplace and
		/// the relay state used to consider the fireplace to be "on".
		/// </summary>
		/// <param name="controlRelay">
		/// The control relay.
		/// </param>
		/// <param name="onRelayState">
		/// The relay state used to consider the fireplace to be "on".
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="controlRelay"/> cannot be null.
		/// </exception>
		public FireplaceDevice(IRelay controlRelay, RelayState onRelayState)
			: this(controlRelay, onRelayState, null, SensorState.Closed) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/>
		/// class with the relay used to control the fireplace.
		/// </summary>
		/// <param name="controlRelay">
		/// The control relay.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="controlRelay"/> cannot be null.
		/// </exception>
		public FireplaceDevice(IRelay controlRelay)
			: this(controlRelay, RelayState.Closed, null, SensorState.Closed) {
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceDevice"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._controlRelay != null) {
				this._controlRelay.Dispose();
				this._controlRelay = null;
			}

			if (this._pilotLightSensor != null) {
				this._pilotLightSensor.Dispose();
				this._pilotLightSensor = null;
			}

			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public override FireplaceState State {
			get {
				if (this._controlRelay.State == this._fireplaceOnRelayState) {
					return FireplaceState.On;	
				}
				return FireplaceState.Off;
			}
			set {
				if (value == FireplaceState.Off) {
					if (this._controlRelay.State == this._fireplaceOnRelayState) {
						this._controlRelay.Toggle();
					}
				}
				else {
					if ((this._pilotLightSensor != null) && (this.IsPilotLightOff)) {
						throw new FireplacePilotLightException();
					}

					if (this._controlRelay.State != this._fireplaceOnRelayState) {
						this._controlRelay.State = this._fireplaceOnRelayState;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the pilot light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is on; otherwise, <c>false</c>.
		/// </value>
		public override bool IsPilotLightOn {
			get {
				if (this._pilotLightSensor == null) {
					return false;
				}
				return this._pilotLightSensor.IsState(this._pilotLightOnSensorState);
			}
		}

		/// <summary>
		/// Gets a value indicating whether pilot light is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is off; otherwise, <c>false</c>.
		/// </value>
		public override bool IsPilotLightOff {
			get { return !this.IsPilotLightOn; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Internal event handler for the relay state changed event.
		/// This fires the fireplace state changed event when the
		/// relay's state changes.
		/// </summary>
		/// <param name="sender">
		/// The object raising the event (a reference to the relay object).
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void InternalHandleRelayStateChange(Object sender, RelayStateChangedEventArgs e) {
			FireplaceStateChangedEventArgs stateChangeEvent = null;
			if (e.NewState == this._fireplaceOnRelayState) {
				stateChangeEvent = new FireplaceStateChangedEventArgs(FireplaceState.Off, FireplaceState.On);
			}
			else {
				stateChangeEvent = new FireplaceStateChangedEventArgs(FireplaceState.On, FireplaceState.Off);
			}

			base.OnStateChanged(stateChangeEvent);
		}

		/// <summary>
		/// Internal handler for the pilot light sensor state changed event.
		/// This fires pilot light state changed event when the pilot light's
		/// state changes.
		/// </summary>
		/// <param name="sender">
		/// The object raising the event (a reference to the sensor object).
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void InternalHandleSensorStateChange(Object sender, SensorStateChangedEventArgs e) {
			if (e.NewState != this._pilotLightOnSensorState) {
				this.Off();
			}

			base.OnPilotLightStateChanged(new FireplacePilotLightEventArgs(this.IsPilotLightOn));
		}
		#endregion
	}
}

