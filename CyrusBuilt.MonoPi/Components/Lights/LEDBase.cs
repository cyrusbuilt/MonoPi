//
//  LEDBase.cs
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

namespace CyrusBuilt.MonoPi.Components.Lights
{
	/// <summary>
	/// Base class for LED component abstractions.
	/// </summary>
	public abstract class LEDBase : ComponentBase, ILED
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Lights.LEDBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected LEDBase()
			: base() {
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when light state changed.
		/// </summary>
		public event LightStateChangeEventHandler StateChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this light is on; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsOn { get; }

		/// <summary>
		/// Gets a value indicating whether this light is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if this light is off; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOff {
			get { return !this.IsOn; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Switches the light on.
		/// </summary>
		public abstract void On();

		/// <summary>
		/// Switches the light off.
		/// </summary>
		public abstract void Off();

		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(LightStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Toggles the state of the LED.
		/// </summary>
		public void Toggle() {
			if (this.IsOn) {
				this.Off();
			}
			else {
				this.On();
			}
		}

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		public abstract void Blink(Int32 delay);

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		/// <param name="duration">
		/// The amount of time to blink the LED (in milliseconds).
		/// </param>
		public abstract void Blink(Int32 delay, Int32 duration);

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		public abstract void Pulse(Int32 duration);

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		/// <param name="blocking">
		/// Blocks the current thread while pulsing.
		/// </param>
		public abstract void Pulse(Int32 duration, Boolean blocking);
		#endregion
	}
}

