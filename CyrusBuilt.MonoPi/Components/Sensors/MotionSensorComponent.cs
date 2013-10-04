//
//  MotionSensorComponent.cs
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

namespace CyrusBuilt.MonoPi.Components.Sensors
{
	/// <summary>
	/// A component that is an abstraction of a motion sensor device. This is an implementation
	/// of <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>.
	/// </summary>
	public class MotionSensorComponent : MotionSensorBase
	{
		#region Fields
		private Thread _pollThread = null;
		private Boolean _isPolling = false;
		private static Boolean _lastCheckDetected = false;
		private const PinState MOTION_DETECTED = PinState.High;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/>
		/// class. This is the default constructor.
		/// </summary>
		public MotionSensorComponent()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/>
		/// class with the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> I/O pin to use.
		/// </summary>
		/// <param name="pin">
		/// The <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> I/O pin to use.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot null.
		/// </exception>
		public MotionSensorComponent(GpioMem pin)
			: base(pin) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/>
		/// class with the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> I/O pin to use.
		/// </summary>
		/// <param name="pin">
		/// The <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> I/O pin to use.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot null.
		/// </exception>
		public MotionSensorComponent(GpioFile pin)
			: base(pin) {
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected override void Dispose(Boolean disposing) {
			if (disposing) {
				if ((this._pollThread != null) || (this._pollThread.IsAlive)) {
					try {
						this._pollThread.Abort();
					}
					catch (ThreadAbortException) {
					}
					finally {
						this._pollThread = null;
					}
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/> object.
		/// </summary>
		/// <remarks>Call <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent.Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/>. The <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent.Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/> in an unusable state. After calling
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent"/> was occupying.</remarks>
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
		/// Gets a value indicating whether motion was detected.
		/// </summary>
		/// <value>
		/// <c>true</c> if motion detected; otherwise, <c>false</c>.
		/// </value>
		public override bool MotionDetected {
			get { return (base.Pin.State == MOTION_DETECTED); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Executes the poll cycle. Does not return until
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorComponent.InterruptPoll"/>
		/// is called.
		/// </summary>
		private void ExecutePoll() {
			while (this._isPolling) {
				if (this.MotionDetected != _lastCheckDetected) {
					DateTime occurred = DateTime.Now;
					Boolean oldState = _lastCheckDetected;
					_lastCheckDetected = this.MotionDetected;
					base.OnMotionDetectionStateChanged(new MotionDetectedEventArgs(this.MotionDetected, occurred));
				}
				Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Executes the poll cycle on a background thread.
		/// </summary>
		private void BackgroundExecutePoll() {
			lock (this) {
				this._isPolling = true;
			}

			if ((this._pollThread == null) || (!this._pollThread.IsAlive)) {
				this._pollThread = new Thread(new ThreadStart(this.ExecutePoll));
				this._pollThread.IsBackground = true;
				this._pollThread.Name = "MotionSensorPollExecutive";
				this._pollThread.Start();
			}
		}

		/// <summary>
		/// Polls the input pin status.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The specified pin is configured for output instead of input.
		/// </exception>
		public void Poll() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Sensors.SensorComponent");
			}

			if (base.Pin.Direction == PinDirection.OUT) {
				throw new InvalidOperationException("The specified pin is configured as an output pin," +
				                                    " which cannot be used to read sensor data.");
			}

			lock (this) {
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
			lock (this) {
				if (!this._isPolling) {
					return;
				}
				this._isPolling = false;
			}
		}
		#endregion
	}
}

