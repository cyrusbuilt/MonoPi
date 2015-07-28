//
//  PCA9685GpioServoProvider.cs
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
using System.Collections.Generic;
using System.Linq;
using CyrusBuilt.MonoPi.IO;
using CyrusBuilt.MonoPi.IO.PCA;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// A servo provider for servos attached to a PCA9685 servo controller.
	/// </summary>
	public class PCA9685GpioServoProvider : IServoProvider
	{
		#region Fields
		private Boolean _isDisposed = false;
		private PCA9685GpioProvider _provider = null;
		private Dictionary<IPCA9685Pin, PCA9685GpioServoDriver> _allocatedDrivers = null;
		private static readonly Object _lock = new Object();
		#endregion

		#region Constructors and Destructors.
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/>
		/// class with the GPIO provider for the PCA9685.
		/// </summary>
		/// <param name="provider">
		/// A GPIO provider for the PCA9685
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="provider"/> cannot be null.
		/// </exception>
		public PCA9685GpioServoProvider(PCA9685GpioProvider provider) {
			if (provider == null) {
				throw new ArgumentNullException("provider");
			}
			this._provider = provider;
			this._allocatedDrivers = new Dictionary<IPCA9685Pin, PCA9685GpioServoDriver>();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/>. The <see cref="Dispose"/> method
		/// leaves the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/> so the garbage collector can reclaim
		/// the memory that the <see cref="CyrusBuilt.MonoPi.Components.Servos.PCA9685GpioServoProvider"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._provider != null) {
				this._provider.Dispose();
				this._provider = null;
			}

			// Dispose the drivers, but not the pins.
			if (this._allocatedDrivers != null) {
				foreach (KeyValuePair<IPCA9685Pin, PCA9685GpioServoDriver> drv in this._allocatedDrivers) {
					drv.Value.Dispose();
				}
				this._allocatedDrivers.Clear();
				this._allocatedDrivers = null;
			}

			this._isDisposed = true;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a list of pins this driver implementation can drive.
		/// </summary>
		/// <value>
		/// A list of defined servo pins.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while trying to get the list of pins.
		/// </exception>
		public List<IPin> DefinedServoPins {
			get { return PCA9685Pin.ALL.ToList<IPin>(); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets a driver for the requested pin.
		/// </summary>
		/// <param name="servoPin">
		/// The pin the driver is needed for.
		/// </param>
		/// <returns>
		/// The servo driver assigned to the pin. May be null if no driver is
		/// assigned or if the pin is unknown.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The specified pin cannot be driven by any available servo driver
		/// because it is not a defined servo pin.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// No driver is assigned to the specified pin - or - Cannot drive servo
		/// from specified pin - or - another initialization error occurred.
		/// </exception>
		public IServoDriver GetServoDriver(IPin servoPin) {
			List<IPin> servoPins = this.DefinedServoPins;
			Int32 index = servoPins.IndexOf(servoPin);
			if (index < 0) {
				throw new ArgumentException("Servo driver cannot drive pin " + servoPin.ToString());
			}

			PCA9685GpioServoDriver driver = null;
			lock (_lock) {
				if (this._allocatedDrivers.ContainsKey((PCA9685Pin)servoPin)) {
					driver = this._allocatedDrivers.Values.ElementAt(index);
				}
				else {
					driver = new PCA9685GpioServoDriver(this._provider, (PCA9685Pin)servoPin);
					this._allocatedDrivers.Add((PCA9685Pin)servoPin, driver);
				}
			}
			return driver;
		}
		#endregion
	}
}

