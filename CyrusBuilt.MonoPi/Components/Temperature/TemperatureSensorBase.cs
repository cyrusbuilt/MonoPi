//
//  TemperatureSensorBase.cs
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
using CyrusBuilt.MonoPi.Sensors;

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// Base class for temperature sensor abstraction components.
	/// </summary>
	public abstract class TemperatureSensorBase : ComponentBase, ITemperatureSensor
	{
		private DS1620 _tempSensor = null;

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected TemperatureSensorBase()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>
		/// class with the clock, data, and reset pins needed for the sensor.
		/// </summary>
		/// <param name="clock">
		/// The GPIO pin used for the clock.
		/// </param>
		/// <param name="data">
		/// The GPIO pin used for data.
		/// </param>
		/// <param name="reset">
		/// The GPIO pin used to trigger reset.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Pins cannot be null.
		/// </exception>
		protected TemperatureSensorBase(IRaspiGpio clock, IRaspiGpio data, IRaspiGpio reset)
			: base() {
			if (clock == null) {
				throw new ArgumentNullException("clock");
			}

			if (data == null) {
				throw new ArgumentNullException("data");
			}

			if (reset == null) {
				throw new ArgumentNullException("reset");
			}

			this._tempSensor = new DS1620(clock, data, reset);
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected virtual void Dispose(Boolean disposing) {
			if (base.IsDisposed) {
				return;
			}

			if (disposing) {
				if (this._tempSensor != null) {
					this._tempSensor.Dispose();
					this._tempSensor = null;
				}
			}

			this.TemperatureChanged = null;
			base.Dispose(true);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase.Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>. The <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase.Dispose"/> method
		/// leaves the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/> in an unusable state.
		/// After calling <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/> so the garbage collector can reclaim
		/// the memory that the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/> was occupying.</remarks>
		public virtual void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when temperature changes.
		/// </summary>
		public event TemperatureChangeEventHandler TemperatureChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the temperature sensor.
		/// </summary>
		public DS1620 Sensor {
			get { return this._tempSensor; }
			set {
				if (value == null) {
					throw new ArgumentNullException("CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase.Sensor");
				}
				this._tempSensor = value;
			}
		}

		/// <summary>
		/// Gets the temperature.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public abstract Double Temperature { get; }

		/// <summary>
		/// Gets the temperature scale.
		/// </summary>
		public abstract TemperatureScale Scale { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Raises the temperature changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnTemperatureChanged(TemperatureChangeEventArgs e) {
			if (this.TemperatureChanged != null) {
				this.TemperatureChanged(this, e);
			}
		}

		/// <summary>
		/// Gets the temperature value.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to get the temperature measurement in.
		/// </param>
		public Double GetTemperature(TemperatureScale scale) {
			return TemperatureConversion.Convert(this.Scale, scale, this.Temperature);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>.
		/// </returns>
		public override string ToString() {
			return base.Name;
		}
		#endregion
	}
}

