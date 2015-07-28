//
//  GpioPowerComponent.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Power
{
	/// <summary>
	/// A power control component implemented using a single native GPIO
	/// configured as an output.
	/// </summary>
	public class GpioPowerComponent : PowerBase
	{
		#region Fields
		private IRaspiGpio _output = null;
		private PinState _onState = PinState.High;
		private PinState _offState = PinState.Low;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/>
		/// class with the GPIO pin that will be used to control the component.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin that will be used to control the component.
		/// </param>
		public GpioPowerComponent(IRaspiGpio pin)
			: base() {
			this._output = pin;
			this._output.StateChanged += Output_StateChanged;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/>
		/// class with the GPIO pin that will be used to control the component
		/// and the pin states that will be used to consider the device on or off.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin that will be used to control the component.
		/// </param>
		/// <param name="onState">
		/// The pin state to consider the device "on".
		/// </param>
		/// <param name="offState">
		/// The pin state to consider the device "off".
		/// </param>
		public GpioPowerComponent(IRaspiGpio pin, PinState onState, PinState offState)
			: this(pin) {
			this._onState = onState;
			this._offState = offState;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._output != null) {
				this._output.Dispose();
				this._output = null;
			}

			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the component's state.
		/// </summary>
		/// <value>
		/// The power state to use.
		/// </value>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The pin being used to control the component is not configured
		/// as an output.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Attempting to set an invalid power state.
		/// </exception>
		public override PowerState State {
			get {
				if (this._output.State == this._onState) {
					return PowerState.On;
				}
				else if (this._output.State == this._offState) {
					return PowerState.Off;
				}
				else {
					return PowerState.Uknown;
				}
			}
			set {
				if (base.IsDisposed) {
					throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Power.GpioPowerComponent");
				}

				if (this._output.Mode != PinMode.OUT) {
					throw new InvalidPinModeException(this._output, "Pins in use by power components MUST be configured as outputs.");
				}

				switch (value) {
					case PowerState.Off:
						this._output.Write(this._offState);
						break;
					case PowerState.On:
						this._output.Write(this._onState);
						break;
					default:
						String badState = Enum.GetName(typeof(PowerState), value);
						throw new InvalidOperationException("Cannot set power state: " + badState);
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Handles the pin state change event by firing the corresponding
		/// power state change event.
		/// </summary>
		/// <param name="sender">
		/// The object (pin) that fired the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void Output_StateChanged(Object sender, PinStateChangeEventArgs e)
		{
			if (e.NewState == this._onState) {
				base.OnStateChanged(new PowerStateChangeEventArgs(PowerState.Off, PowerState.On));
			}
			else {
				base.OnStateChanged(new PowerStateChangeEventArgs(PowerState.On, PowerState.Off));
			}
		}
		#endregion
	}
}

