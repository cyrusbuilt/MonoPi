//
//  LEDComponent.cs
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
using System.Threading;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Lights
{
	/// <summary>
	/// A component that is an abstraction of an LED.
	/// </summary>
	public class LEDComponent : LEDBase
	{
		#region Fields
		private IGpio _pin = null;
		private const PinState ON_STATE = PinState.High;
		private const PinState OFF_STATE = PinState.Low;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>
		/// class with the pin controlling the LED.
		/// </summary>
		/// <param name="pin">
		/// The output pin the LED is wired to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The pin cannot be null.
		/// </exception>
		public LEDComponent(IGpio pin)
			: base() {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._pin != null) {
				this._pin.Dispose();
				this._pin = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this LED is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this LED is on; otherwise, <c>false</c>.
		/// </value>
		public override Boolean IsOn {
			get { return this._pin.State == ON_STATE; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Switches the LED on.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The pin is configured for input instead of output.
		/// </exception>
		public override void On() {
			if (this._pin.Direction == PinDirection.IN) {
				throw new InvalidOperationException("Pin is not configured as an output pin.");
			}

			if (this._pin.State != ON_STATE) {
				this._pin.Write(true);
				base.OnStateChanged(new LightStateChangeEventArgs(true));
			}
		}

		/// <summary>
		/// Switches the LED off.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The pin is configured for input instead of output.
		/// </exception>
		public override void Off() {
			if (this._pin.Direction == PinDirection.IN) {
				throw new InvalidOperationException("Pin is not configured as an output pin.");
			}

			if (this._pin.State != OFF_STATE) {
				this._pin.Write(false);
				base.OnStateChanged(new LightStateChangeEventArgs(false));
			}
		}

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		public override void Blink(Int32 delay) {
			this.On();
			Thread.Sleep(delay);
			this.Off();
		}

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		/// <param name="duration">
		/// The amount of time to blink the LED (in milliseconds).
		/// </param>
		public override void Blink(Int32 delay, Int32 duration) {
			DateTime start = DateTime.Now;
			TimeSpan ts = TimeSpan.MinValue;
			while (ts.Milliseconds <= duration) {
				this.Blink(delay);
				ts = (DateTime.Now - start);
			}
		}

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		public override void Pulse(Int32 duration) {
			this._pin.Pulse(duration);
		}

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		/// <param name="blocking">
		/// Blocks the current thread while pulsing.
		/// </param>
		public override void Pulse(Int32 duration, Boolean blocking) {
			if (blocking) {
				lock (this._pin) {
					this.Pulse(duration);
				}
				return;
			}
			this.Pulse(duration);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDComponent"/>.
		/// </returns>
		public override String ToString() {
			return base.Name;
		}
		#endregion
	}
}

