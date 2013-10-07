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
	public abstract class TemperatureSensorBase : ITemperatureSensor
	{
		#region Fields
		private DS1620 _tempSensor = null;
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private Object _tag = null;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected TemperatureSensorBase() {
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
		protected TemperatureSensorBase(GpioBase clock, GpioBase data, GpioBase reset) {
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
			if (this._isDisposed) {
				return;
			}

			if (disposing) {
				if (this._tempSensor != null) {
					this._tempSensor.Dispose();
					this._tempSensor = null;
				}
				this._tag = null;
				this._name = null;
			}

			this.TemperatureChanged = null;
			this._isDisposed = true;
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
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

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

		/// <summary>
		/// Gets or sets the name of the sensor.
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
			return this._name;
		}
		#endregion
	}
}

