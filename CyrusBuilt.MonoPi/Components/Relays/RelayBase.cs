//
//  RelayBase.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Relays
{
	/// <summary>
	/// Base class for relay abstraction components.
	/// </summary>
	public abstract class RelayBase : IRelay
	{
		#region Fields
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private Object _tag = null;
		private GpioBase _pin = null;

		/// <summary>
		/// The pin state when the relay is open.
		/// </summary>
		public const PinState OPEN_STATE = PinState.Low;

		/// <summary>
		/// The pin state when the relay is closed.
		/// </summary>
		public const PinState CLOSED_STATE = PinState.High;

		/// <summary>
		/// The default pulse time (500ms [.5s]).
		/// </summary>
		public const Int32 DEFAULT_PULSE_MILLISECONDS = 500;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected RelayBase() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/>
		/// class with the pin being used to control the relay.
		/// </summary>
		/// <param name="pin">
		/// The output pin being used to control the relay.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot null.
		/// </exception>
		protected RelayBase(GpioBase pin) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected virtual void Dispose(Boolean disposing) {
			if (this._isDisposed) {
				return;
			}

			if (disposing) {
				this._name = null;
				this._tag = null;
				if (this._pin != null) {
					this._pin.Dispose();
					this._pin = null;
				}
			}
			this.StateChanged = null;
			this.PulseStart = null;
			this.PulseStop = null;
			this._isDisposed = true;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/> in an unusable state. After calling
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/> was occupying.</remarks>
		public virtual void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when relay state changes.
		/// </summary>
		public event RelayStateChangeEventHandler StateChanged;

		/// <summary>
		/// Occurs when a relay pulse starts.
		/// </summary>
		public event RelayPulseEventHandler PulseStart;

		/// <summary>
		/// Occurs when a relay pulse stops.
		/// </summary>
		public event RelayPulseEventHandler PulseStop;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets or sets the state of the relay.
		/// </summary>
		public abstract RelayState State { get; set; }

		/// <summary>
		/// Checks to see if the dry contacts are open.
		/// </summary>
		/// <value>
		/// <c>true</c> if the dry contacts are open; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOpen {
			get { return (this.State == RelayState.Open); }
		}

		/// <summary>
		/// Checks to see if the dry contacts are closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if dry contacts are closed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsClosed {
			get { return (this.State == RelayState.Closed); }
		}

		/// <summary>
		/// Gets or sets the name of this relay.
		/// </summary>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		public Object Tag {
			get { return this._tag; }
			set { this._tag = value; }
		}

		/// <summary>
		/// Gets or sets the pin.
		/// </summary>
		protected GpioBase Pin {
			get { return this._pin; }
			set { this._pin = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(RelayStateChangedEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the pulse start event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPulseStart(EventArgs e) {
			if (this.PulseStart != null) {
				this.PulseStart(this, e);
			}
		}

		/// <summary>
		/// Raises the pulse stop event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPulseStop(EventArgs e) {
			if (this.PulseStop != null) {
				this.PulseStop(this, e);
			}
		}

		/// <summary>
		/// Opens the dry contacts on the relay.
		/// </summary>
		public virtual void Open() {
			this.State = RelayState.Open;
		}

		/// <summary>
		/// Closes the dry contacts on the relay.
		/// </summary>
		public virtual void Close() {
			this.State = RelayState.Closed;
		}

		/// <summary>
		/// Pulses the relay on for the specified number of milliseconds, then
		/// back off again.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to wait before switching back off.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Cannot pulse using a pin that is not configured as an output.
		/// </exception>
		public void Pulse(Int32 millis) {
			this.OnPulseStart(EventArgs.Empty);
			this.Close();
			this._pin.Pulse(millis);
			this.Open();
			this.OnPulseStop(EventArgs.Empty);
		}

		/// <summary>
		/// Pulses the relay on and off.
		/// </summary>
		public void Pulse() {
			this.Pulse(DEFAULT_PULSE_MILLISECONDS);
		}

		/// <summary>
		/// Toggles the state of the relay (closed, then open).
		/// </summary>
		public void Toggle() {
			if (this.IsOpen) {
				this.Close();
			}
			else {
				this.Open();
			}
		}

		/// <summary>
		/// Determines whether this relay's state matches the specified state.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this relay's state matches the specified state; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="state">
		/// The state to check for.
		/// </param>
		public Boolean IsState(RelayState state) {
			return (this.State == state);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current 
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayBase"/>.
		/// </returns>
		public override string ToString() {
			return this._name;
		}
		#endregion
	}
}

