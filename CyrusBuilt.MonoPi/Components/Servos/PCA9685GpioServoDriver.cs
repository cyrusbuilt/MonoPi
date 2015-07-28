//
//  PCA9685GpioServerDriver.cs
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
using CyrusBuilt.MonoPi.IO.PCA;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// Represents a servo driver for servos attached to a PCA9685 servo controller.
	/// </summary>
	public class PCA9685GpioServoDriver : IServoDriver
	{
		#region Fields
		private PCA9685GpioProvider _provider = null;
		private IPCA9685Pin _pin = null;
		private Int32 _pos = 0;
		private Int32 _res = 0;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors and Destructors.
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver"/>
		/// class with a PCA9685 GPIO provider and pin.
		/// </summary>
		/// <param name="provider">
		/// The PCA9685 GPIO provider.
		/// </param>
		/// <param name="pin">
		/// The PCA9685 pin (channel) the servo is attached to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="provider"/> cannot be null. - or -
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		public PCA9685GpioServoDriver(PCA9685GpioProvider provider, IPCA9685Pin pin) {
			if (provider == null) {
				throw new ArgumentNullException("provider");
			}

			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			this._provider = provider;
			this._pin = pin;
			this.UpdateResolution();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver"/>. The <see cref="Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._provider != null) {
				this._provider.Dispose();
				this._provider = null;
			}

			if (this._pin != null) {
				this._pin.Dispose();
				this._pin = null;
			}

			this._pos = 0;
			this._res = 0;
			this._isDisposed = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the PCA9685 GPIO provider.
		/// </summary>
		/// <value>
		/// The GPIO provider.
		/// </value>
		public PCA9685GpioProvider Provider {
			get { return this._provider; }
		}

		/// <summary>
		/// Gets the pin.
		/// </summary>
		/// <value>
		/// The pin.
		/// </value>
		public IPin Pin {
			get { return this._pin; }
			set { this._pin = value as PCA9685Pin; }
		}

		/// <summary>
		/// Gets or sets the current servo pulse width. Zero may represent
		/// this driver stopped producing pulses. Also, value of -1
		/// may define undefined situation when this abstraction didn't get
		/// initial value yet and there is no way telling what real, hardware
		/// or software driver is sending.
		/// </summary>
		/// <value>
		/// Current servo pulse this driver is producing.
		/// </value>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		public Int32 ServoPulseWidth {
			get { return this._pos; }
			set {
				if (this._isDisposed) {
					throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoDriver");
				}
				this._pos = value;
				this._provider.SetPWM(this._pin, this._pos);
			}
		}

		/// <summary>
		/// Gets the servo pulse resolution.
		/// </summary>
		/// <value>
		/// The servo pulse resolution. Resolution is provided in 1/n (ms)
		/// where value returned is n.
		/// </value>
		public Int32 ServoPulseResolution {
			get { return this._res; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Updates the resolution.
		/// </summary>
		protected void UpdateResolution() {
			this._res = Convert.ToInt32(Decimal.Divide(4096, this._provider.Frequency));
		}
		#endregion
	}
}

