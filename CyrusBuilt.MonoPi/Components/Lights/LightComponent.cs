//
//  LightComponent.cs
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

namespace CyrusBuilt.MonoPi.Components.Lights
{
	/// <summary>
	/// A component that is an abstraction of a light.
	/// </summary>
	public class LightComponent : LightBase
	{
		#region Fields
		private GpioBase _pin = null;
		private const PinState ON_STATE = PinState.High;
		private const PinState OFF_STATE = PinState.Low;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/>
		/// class with the pin controlling the light.
		/// </summary>
		/// <param name="pin">
		/// The output pin the light is wired to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The pin cannot be null.
		/// </exception>
		public LightComponent(GpioBase pin)
			: base() {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.LightComponent"/> was occupying.
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
		/// Gets a value indicating whether this light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is on; otherwise, <c>false</c>.
		/// </value>
		public override Boolean IsOn {
			get { return this._pin.State == ON_STATE; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Switches the light on.
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
		/// Switches the light off.
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
		#endregion
	}
}

