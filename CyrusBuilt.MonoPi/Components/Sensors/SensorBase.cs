//
//  SensorBase.cs
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
	/// Base class for sensor abstraction components.
	/// </summary>
	public abstract class SensorBase : ISensor
	{
		#region Fields
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private Object _tag = null;
		private IGpio _pin = null;
		private Dictionary<String, String> _props = null;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected SensorBase() {
			this._props = new Dictionary<String, String>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/>
		/// class with the pin being used to read the sensor input.
		/// </summary>
		/// <param name="pin">
		/// The pin being used to read the sensor input.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		protected SensorBase(IGpio pin) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
			this._pin.Provision();
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

			this.StateChanged = null;
			this._isDisposed = true;
		}

		#pragma warning disable 419
		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase.Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/>. The <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase.Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/> in an unusable state. After calling
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase.Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/> was occupying.</remarks>
		public virtual void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#pragma warning restore 419
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the sensor state changes.
		/// </summary>
		public event SensorStateChangedEventHandler StateChanged;
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
		/// Gets the sensor state.
		/// </summary>
		public abstract SensorState State { get; }

		/// <summary>
		/// Gets a value indicating whether this sensor is open.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is open; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOpen {
			get { return (this.State == SensorState.Open); }
		}

		/// <summary>
		/// Gets a value indicating whether this sensor is closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is closed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsClosed {
			get { return (this.State == SensorState.Closed); }
		}

		/// <summary>
		/// Gets or sets the pin.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// value cannot be null.
		/// </exception>
		public IGpio Pin {
			get { return this._pin; }
			set {
				if (value == null) {
					throw new ArgumentNullException("SensorBase.Pin");
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
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(SensorStateChangedEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Determines whether this sensor's state is the specified state.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this sensor's state is the specified state; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="state">
		/// The state to check.
		/// </param>
		public Boolean IsState(SensorState state) {
			return (this.State == state);
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
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Sensors.SensorBase"/>.
		/// </returns>
		public override string ToString() {
			return this._name;
		}
		#endregion
	}
}

