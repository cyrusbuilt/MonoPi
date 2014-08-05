//
//  TemperatureSensorComponent.cs
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
using CyrusBuilt.MonoPi.Sensors;

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// A component that is an abstraction of a temperature sensor device.
	/// This is an implementation of <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>.
	/// </summary>
	public class TemperatureSensorComponent : TemperatureSensorBase
	{
		#region Fields
		private Thread _pollThread = null;
		private volatile Boolean _isPolling = false;
		private TemperatureScale _scale = TemperatureScale.Celcius;
		private static Double _lastTemp = 0;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>
		/// class. This is the default constructor.
		/// </summary>
		public TemperatureSensorComponent()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>
		/// class with the scale to get the temperature in.
		/// </summary>
		/// <param name="scale">
		/// The scale to get the temperature readings in.
		/// </param>
		public TemperatureSensorComponent(TemperatureScale scale)
			: base() {
			this._scale = scale;
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>
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
		public TemperatureSensorComponent(IRaspiGpio clock, IRaspiGpio data, IRaspiGpio reset)
			: base(clock, data, reset) {
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>
		/// class with the clock, data, and reset pins needed for the sensor,
		/// as well as the scale to get the temperature readings in.
		/// </summary>
		/// <param name="scale">
		/// The scale to get the temperature readings in.
		/// </param>
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
		public TemperatureSensorComponent(TemperatureScale scale, IRaspiGpio clock, IRaspiGpio data, IRaspiGpio reset)
			: base(clock, data, reset) {
			this._scale = scale;
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected override void Dispose(Boolean disposing) {
			if (disposing) {
				lock(_syncLock) {
					this._isPolling = false;
				}

				if ((this._pollThread != null) && (this._pollThread.IsAlive)) {
					try {
						Thread.Sleep(50);
						this._pollThread.Abort();
					}
					catch (ThreadAbortException) {
						Thread.ResetAbort();
					}
					finally {
						this._pollThread = null;
					}
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Releases all resource used by the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/> object.
		/// </summary>
		/// <remarks>Call <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent.Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/>. The <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent.Dispose"/>
		/// method leaves the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/> in an unusable
		/// state. After calling <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/> so the garbage collector can
		/// reclaim the memory that the <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent"/> was occupying.</remarks>
		public override void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is polling.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is polling; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPolling {
			get { return this._isPolling; }
		}

		/// <summary>
		/// Gets the temperature.
		/// </summary>
		/// <value>
		/// The temperature value.
		/// </value>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public override double Temperature {
			get {
				if (base.IsDisposed) {
					throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent");
				}

				TemperatureScale c = TemperatureScale.Celcius;
				Double temp = base.Sensor.GetTemperature();
				if (this._scale != c) {
					return TemperatureConversion.Convert(c, this._scale, temp);
				}
				return temp;
			}
		}

		/// <summary>
		/// Gets the temperature scale.
		/// </summary>
		public override TemperatureScale Scale {
			get { return this._scale; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Changes the temperature scale.
		/// </summary>
		/// <param name="scale">
		/// The scale to change to.
		/// </param>
		public void ChangeScale(TemperatureScale scale) {
			lock (_syncLock) {
				this._scale = scale;
			}
		}

		/// <summary>
		/// Executes the poll cycle. Does not return until
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent.InterruptPoll"/>
		/// is called.
		/// </summary>
		private void ExecutePoll() {
			Double oldTemp = 0;
			Double newTemp = 0;
			while (this._isPolling) {
				newTemp = this.Temperature;
				if (newTemp != _lastTemp) {
					oldTemp = _lastTemp;
					_lastTemp = newTemp;
					base.OnTemperatureChanged(new TemperatureChangeEventArgs(oldTemp, newTemp));
				}
				Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Executes the poll cycle on a background thread.
		/// </summary>
		private void BackgroundExecutePoll() {
			lock (_syncLock) {
				this._isPolling = true;
			}

			if ((this._pollThread == null) || (!this._pollThread.IsAlive)) {
				this._pollThread = new Thread(new ThreadStart(this.ExecutePoll));
				this._pollThread.IsBackground = true;
				this._pollThread.Name = "TemperatureSensorPollExecutive";
				this._pollThread.Start();
			}
		}

		/// <summary>
		/// Polls the input pin for temperature reading.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void Poll() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Temperature.TemperatureSensorComponent");
			}

			lock (_syncLock) {
				if (this._isPolling) {
					return;
				}
			}
			this.BackgroundExecutePoll();
		}

		/// <summary>
		/// Interrupts the poll cycle.
		/// </summary>
		public void InterruptPoll() {
			lock (_syncLock) {
				if (!this._isPolling) {
					return;
				}
				this._isPolling = false;
			}
		}
		#endregion
	}
}

