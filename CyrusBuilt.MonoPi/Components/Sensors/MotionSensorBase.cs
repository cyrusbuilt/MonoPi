//
//  MotionSensorBase.cs
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
using System.Collections.Generic;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Sensors
{
	/// <summary>
	/// Base class for motion sensor abstraction components.
	/// </summary>
	public abstract class MotionSensorBase : IMotionSensor
	{
		#region Fields
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private Object _tag = null;
		private GpioBase _pin = null;
		private DateTime _lastMotion = DateTime.MinValue;
		private DateTime _lastInactive = DateTime.MinValue;
		private Dictionary<String, String> _props = null;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>
		/// class.  This is the default constructor.
		/// </summary>
		protected MotionSensorBase() {
			this._props = new Dictionary<String, String>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>
		/// class.
		/// </summary>
		/// <param name="pin">
		/// The pin being used to read the sensor input.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		protected MotionSensorBase(GpioBase pin) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
			this._props = new Dictionary<String, String>();
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
				this._name = null;
				this._tag = null;
				if (this._pin != null) {
					this._pin.Dispose();
					this._pin = null;
				}

				if (this._props != null) {
					this._props.Clear();
					this._props = null;
				}
			}

			this.MotionDetectionStateChanged = null;
			this._isDisposed = true;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase.Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>. The <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase.Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/> in an unusable state. After calling
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/> was occupying.</remarks>
		public virtual void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when motion is detected.
		/// </summary>
		public event MotionDetectionEventHandler MotionDetectionStateChanged;
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

		/// <summary>
		/// Gets the timestamp of the last time motion was detected.
		/// </summary>
		public DateTime LastMotionTimestamp {
			get { return this._lastMotion; }
		}

		/// <summary>
		/// Gets the timestamp of the last time inactivity was detected.
		/// </summary>
		public DateTime LastInactivityTimestamp {
			get { return this._lastInactive; }
		}

		/// <summary>
		/// Gets a value indicating whether motion was detected.
		/// </summary>
		/// <value>
		/// <c>true</c> if motion detected; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean MotionDetected { get; }

		/// <summary>
		/// Gets or sets the pin.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// value cannot be null.
		/// </exception>
		public GpioBase Pin {
			get { return this._pin; }
			set {
				if (value == null) {
					throw new ArgumentNullException("MotionSensorBase.Pin");
				}
 				this._pin = value;
			}
		}

		/// <summary>
		/// Gets the property collection.
		/// </summary>
		/// <value>
		/// The property collection.
		/// </value>
		public Dictionary<String, String> PropertyCollection {
			get { return this._props; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the motion detection state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnMotionDetectionStateChanged(MotionDetectedEventArgs e) {
			if (e.MotionDetected) {
				this._lastMotion = DateTime.Now;
			}
			else {
				this._lastInactive = DateTime.Now;
			}

			if (this.MotionDetectionStateChanged != null) {
				this.MotionDetectionStateChanged(this, e);
			}
		}

		/// <summary>
		/// Determines whether this instance has property the specified key.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has property the specified by key; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="key">
		/// The key name of the property to check for.
		/// </param>
		public Boolean HasProperty(String key) {
			return this._props.ContainsKey(key);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionSensorBase"/>.
		/// </returns>
		public override String ToString() {
			return this._name;
		}
		#endregion
	}
}

