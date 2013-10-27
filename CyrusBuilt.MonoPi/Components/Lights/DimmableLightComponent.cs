//
//  DimmableLightComponent.cs
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
	/// A component that is an abstraction of a dimmable light.
	/// </summary>
	public class DimmableLightComponent : DimmableLightBase
	{
		#region Fields
		private GpioBase _pin = null;
		private Int32 _min = 0;
		private Int32 _max = 0;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/>
		/// class with the pin controlling the light and the minimum and
		/// maximum light level.
		/// </summary>
		/// <param name="pin">
		/// The pin used to control the dimmable light.
		/// </param>
		/// <param name="min">
		/// The minimum brightness level.
		/// </param>
		/// <param name="max">
		/// The maximum brightness level.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The pin cannot be null.
		/// </exception>
		public DimmableLightComponent(GpioBase pin, Int32 min, Int32 max)
			: base() {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
			this._min = min;
			this._max = max;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release all
		/// references to the <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightComponent"/> was occupying.
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
		/// Gets the minimum brightness level.
		/// </summary>
		public override Int32 MinLevel {
			get { return this._min; }
		}

		/// <summary>
		/// Gets the maximum brightness level.
		/// </summary>
		public override Int32 MaxLevel {
			get { return this._max; }
		}

		/// <summary>
		/// Gets or sets the brightness level.
		/// </summary>
		/// <value>
		/// The brightness level.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Value cannot be less than <see cref="MinLevel"/> or
		/// more than <see cref="MaxLevel"/>.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The pin is configured as in input pin instead of output.
		/// </exception>
		public override int Level {
			get { return this._pin.PWM; }
			set {
				if (value < this._min) {
					throw new ArgumentOutOfRangeException("Value cannot be less than MinLevel.");
				}

				if (value > this._max) {
					throw new ArgumentOutOfRangeException("Value cannot be more than MaxLevel.");
				}

				try {
 					Boolean isOnBeforeChange = base.IsOn;
					this._pin.PWM = value;
					Boolean isOnAfterChange = base.IsOn;
					base.OnLevelChanged(new LightLevelChangeEventArgs(value));
					if (isOnBeforeChange != isOnAfterChange) {
						base.OnStateChanged(new LightStateChangeEventArgs(isOnAfterChange));
					}
				}
				catch (InvalidOperationException) {
					throw;
				}
			}
		}
		#endregion
	}
}

