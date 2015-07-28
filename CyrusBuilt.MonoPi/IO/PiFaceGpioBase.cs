//
//  PiFaceGpioBase.cs
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
using System.Threading;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Base class for the GPIO pins on the PiFace.
	/// </summary>
	public abstract class PiFaceGpioBase : IPiFaceGPIO
	{
		#region Fields
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private Object _tag = null;
		private PinState _state = PinState.Low;
		private PiFacePins _innerPin = PiFacePins.None;
		private UInt32 _pwm = 0;
		private UInt32 _pwmRange = 1024;
		#pragma warning disable 1591
		protected PinMode _mode = PinMode.IN;
		protected PinState _initValue = PinState.Low;
		#pragma warning restore 1591
		private static Dictionary<Int32, PinMode> _exportedPins = new Dictionary<Int32, PinMode>();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the pin state changes.
		/// </summary>
		public event PinStateChangeEventHandler StateChanged;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>
		/// class with the physical pin represented by this class.
		/// </summary>
		/// <param name="pin">
		/// The physical pin being wrapped by this class.
		/// </param>
		protected PiFaceGpioBase(PiFacePins pin) {
			this._innerPin = pin;
			switch (pin) {
				case PiFacePins.Input00:
				case PiFacePins.Input01:
				case PiFacePins.Input02:
				case PiFacePins.Input03:
				case PiFacePins.Input04:
				case PiFacePins.Input05:
				case PiFacePins.Input06:
				case PiFacePins.Input07:
					this._mode = PinMode.IN;
					break;
				case PiFacePins.Output00:
				case PiFacePins.Output01:
				case PiFacePins.Output02:
				case PiFacePins.Output03:
				case PiFacePins.Output04:
				case PiFacePins.Output05:
				case PiFacePins.Output06:
				case PiFacePins.Output07:
					this._mode = PinMode.OUT;
					break;
				case PiFacePins.None:
				default:
					break;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>
		/// class with the physical pin represented by this class.
		/// </summary>
		/// <param name="pin">
		/// The physical pin being wrapped by this class.
		/// </param>
		/// <param name="initialValue">
		/// Initial value.
		/// </param>
		protected PiFaceGpioBase(PiFacePins pin, PinState initialValue)
			: this(pin) {
			this._initValue = initialValue;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>
		/// in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/> was occupying.
		/// </remarks>
		public virtual void Dispose() {
			if (this._isDisposed) {
				return;
			}

			_exportedPins.Clear();
			_exportedPins = null;
			this.StateChanged = null;
			this._innerPin = PiFacePins.None;
			this._mode = PinMode.IN;
			this._isDisposed = true;
			this._name = null;
			this._tag = null;
			GC.SuppressFinalize(this);
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name of the GPIO.
		/// </value>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		/// <value>
		/// The object to tag the GPIO with.
		/// </value>
		public Object Tag {
			get { return this._tag; }
			set { this._tag = value; }
		}

		/// <summary>
		/// Gets the state of the pin. Default is <see cref="CyrusBuilt.MonoPi.IO.PinState.Low"/>.
		/// </summary>
		public PinState State {
			get {
				this._state = this.Read();
				return this._state;
			}
		}

		/// <summary>
		/// Gets the physical pin being represented by this class.
		/// </summary>
		public PiFacePins InnerPin {
			get { return this._innerPin; }
		}

		/// <summary>
		/// Gets the direction (mode) for the pin (Input or Output).
		/// </summary>
		public PinMode Mode {
			get { return this._mode; }
		}

		/// <summary>
		/// Gets or sets the PWM (Pulse Width Modulation) value.
		/// </summary>
		/// <value>
		/// The PWM value.
		/// </value>
		public UInt32 PWM {
			get { return this._pwm; }
			set { this._pwm = value; }
		}

		/// <summary>
		/// Gets or sets the PWM range.
		/// </summary>
		/// <value>
		/// The PWM range. Default is 1024.
		/// </value>
		/// <remarks>>
		/// See <a href="http://wiringpi.com/reference/raspberry-pi-specifics/">http://wiringpi.com/reference/raspberry-pi-specifics/</a>
		/// </remarks>
		public UInt32 PWMRange {
			get { return this._pwmRange; }
			set { this._pwmRange = value; }
		}

		/// <summary>
		/// Gets the exported pins.
		/// </summary>
		protected static Dictionary<Int32, PinMode> ExportedPins {
			get { return _exportedPins; }
		}

		/// <summary>
		/// Gets the pin address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		public Int32 Address {
			get { return (Int32)this._innerPin; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Provisions the pin (initialize to specified mode and make active).
		/// </summary>
		public abstract void Provision();

		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(PinStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Gets the GPIO pin number.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		/// <returns>
		/// The GPIO pin number.
		/// </returns>
		protected static String GetGpioPinNumber(GpioPins pin) {
			return ((Int32)pin).ToString();
		}

		/// <summary>
		/// Write the specified value to the pin.
		/// </summary>
		/// <param name="value">
		/// If set to <c>true</c> value.
		/// </param>
		public virtual void Write(PinState value) {
			this._state = value;
		}

		/// <summary>
		/// Pulse the pin output for the specified number of milliseconds.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to wait between states.
		/// </param>
		public virtual void Pulse(Int32 millis) {
			this.Write(PinState.High);
			Thread.Sleep(millis);
			this.Write(PinState.Low);
		}

		/// <summary>
		/// Read a value from the pin.
		/// </summary>
		/// <returns>
		/// The value read from the pin.
		/// </returns> 
		public abstract PinState Read();

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.IO.PiFaceGpioBase"/>.
		/// </returns>
		public override string ToString() {
			return this._name;
		}
		#endregion
	}
}

