//
//  RPIServoBlasterDriver.cs
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

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// Represents a servo driver for servos attached to the Raspberry Pi and driven via ServoBlaster.
	/// </summary>
	public class RPIServoBlasterDriver : IServoDriver
	{
		#region Fields
		private Boolean _isDisposed = false;
		private RPIServoBlasterProvider _provider = null;
		private IRaspiGpio _pin = null;
		private Int32 _pos = 0;
		private Int32 _index = -1;
		private String _pinString = String.Empty;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterDriver"/>
		/// class with the servo pin, the index of the pin in the servo provider's pin map,
		/// The name of the pin, and the servo provider.
		/// </summary>
		/// <param name="pin">
		/// The pin the servo is attached to.
		/// </param>
		/// <param name="index">
		/// The index of the pin in the servo provider's pin map.
		/// </param>
		/// <param name="pinName">
		/// The name of the pin.
		/// </param>
		/// <param name="provider">
		/// The servo provider.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null - or -
		/// <paramref name="pinName"/> cannot be null or empty - or -
		/// <paramref name="provider"/> cannot be null.
		/// </exception>
		public RPIServoBlasterDriver(IRaspiGpio pin, Int32 index, String pinName, RPIServoBlasterProvider provider) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (String.IsNullOrEmpty(pinName)) {
				throw new ArgumentNullException("pinName");
			}

			if (provider == null) {
				throw new ArgumentNullException("provider");
			}

			this._pin = pin;
			this._index = index;
			this._pinString = pinName;
			this._provider = provider;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterDriver"/>. The <see cref="Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterDriver"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterDriver"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterDriver"/> was occupying.</remarks>
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

			this._isDisposed = true;
		}
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
		/// Gets or sets the current servo pulse width. Zero may represent
		/// this driver stopped producing pulses. Also, value of -1
		/// may define undefined situation when this abstraction didn't get
		/// initial value yet and there is no way telling what real, hardware
		/// or software driver is sending.
		/// </summary>
		/// <value>
		/// Current servo pulse this driver is producing.
		/// </value>
		public Int32 ServoPulseWidth {
			get { return this._pos; }
			set {
				this._pos = value; 
				this._provider.UpdateServo(this._pinString, this._pos);
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
			get { return 100; }
		}

		/// <summary>
		/// Gets the pin.
		/// </summary>
		/// <value>
		/// The pin.
		/// </value>
		public IPin Pin {
			get { return this._pin; }
		}
		#endregion
	}
}

