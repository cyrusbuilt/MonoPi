//
//  OpenerDevice.cs
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

namespace CyrusBuilt.MonoPi.Devices.Access
{
	/// <summary>
	/// A device that is an abstraction of a door opener (such as a garage door opener).
	/// This is an implementation of the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerBase"/>.
	/// </summary>
	public class OpenerDevice : OpenerBase
	{
		#region Fields
		private IRelay _relay = null;
		private ISensor _sensor = null;
		private SensorState _openState = SensorState.Closed;
		private ISwitch _lock = null;
		private SwitchState _overridedLockState = SwitchState.Off;
		private Boolean _lockOverride = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/>
		/// class with the relay, sensor, and the sensor state that indicates
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
		public OpenerDevice(IRelay relay, ISensor sensor, SensorState openState)
			: base() {
			this._relay = relay;
			this._sensor = sensor;
			this._openState = openState;
			this._sensor.StateChanged += this.HandleSensorStateChanged;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/>
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
		public OpenerDevice(IRelay relay, ISensor sensor, SensorState openState, ISwitch lok)
			: this(relay, sensor, openState) {
			this._lock = lok;
			this._lock.StateChanged += this.HandleLockStateChanged;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/>
		/// so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerDevice"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._relay != null) {
				this._relay.Dispose();
				this._relay = null;
			}

			if (this._sensor != null) {
				this._sensor.Dispose();
				this._sensor = null;
			}

			if (this._lock == null) {
				this._lock.Dispose();
				this._lock = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this opener is locked and thus, cannot be opened.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is locked; otherwise, <c>false</c>.
		/// </value>
		public override Boolean IsLocked {
			get {
				if (this._lockOverride) {
					return (this._overridedLockState == SwitchState.On);
				}
				else {
					if (this._lock == null) {
						return false;
					}
					return this._lock.IsOn;
				}
			}
		}

		/// <summary>
		/// Gets the state of this opener.
		/// </summary>
		public override OpenerState State {
			get {
				if (this._sensor.State == this._openState) {
					return OpenerState.Open;
				}
				return OpenerState.Closed;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Handles the sensor state changed event.
		/// </summary>
		/// <param name="sender">
		/// The object firing the event. Should be a reference to
		/// the sensor insance that triggered the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void HandleSensorStateChanged(Object sender, SensorStateChangedEventArgs e) {
			OpenerState oldState = this.GetOpenerState(e.OldState);
			OpenerState newState = this.GetOpenerState(e.NewState);
			base.OnStateChanged(new OpenerStateChangeEventArgs(oldState, newState));
		}

		/// <summary>
		/// Handles the lock state changed.
		/// </summary>
		/// <param name="sender">
		/// The object firing the event. Should be a reference to
		/// the switch instance that triggered the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void HandleLockStateChanged(Object sender, SwitchStateChangeEventArgs e) {
			base.OnLockChanged(new OpenerLockChangeEventArgs(((ISwitch)sender).IsOn));
		}

		/// <summary>
		/// Closes the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be closed because it is locked.
		/// </exception>
		public override void Close() {
			if (this.IsLocked) {
				throw new OpenerLockedException(base.Name);
			}

			if (this._sensor.IsState(this._openState)) {
				this._relay.Pulse();
			}
		}

		/// <summary>
		/// Opens the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be opened because it is locked.
		/// </exception>
		public override void Open() {
			if (this.IsLocked) {
				throw new OpenerLockedException(base.Name);
			}

			if (!this._sensor.IsState(this._openState)) {
				this._relay.Pulse();
			}
		}

		/// <summary>
		/// Gets the state of the opener based on the specified sensor state.
		/// </summary>
		/// <returns>
		/// The opener state.
		/// </returns>
		/// <param name="sensState">
		/// Sensor state.
		/// </param>
		protected OpenerState GetOpenerState(SensorState sensState) {
			if (sensState == this._openState) {
				return OpenerState.Open;
			}
			return OpenerState.Closed;
		}

		/// <summary>
		/// Manually overrides the state of the lock. This can be used to force lock or
		/// force unlock the opener. This will cause this opener to ignore the state of
		/// the lock (if specified) and only read the specified lock state.
		/// </summary>
		public void OverrideLock(SwitchState overridedState) {
			lock (this) {
				this._overridedLockState = overridedState;
				this._lockOverride = true;
			}
		}

		/// <summary>
		/// Disables the lock override. This will cause this opener to resume reading
		/// the actual state of the lock (if specified).
		/// </summary>
		public void DisableOverride() {
			lock (this) {
				this._lockOverride = false;
			}
		}
		#endregion
	}
}

