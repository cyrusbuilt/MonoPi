//
//  PCF8574Pin.cs
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

namespace CyrusBuilt.MonoPi.IO.PCF
{
	/// <summary>
	/// Represents a TI PCF8574 GPIO pin abstraction. This class also provides
	/// hard implementations of all 8 of the GPIO pins on the PCF8574.
	/// </summary>
	public class PCF8574Pin : IPCF8574Pin
	{
		#region Pin Constants
		/// <summary>
		/// The GPIO 0 pin (at address 0).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_00 = CreateDigitalPin(0, "GPIO 0");

		/// <summary>
		/// The GPIO 1 pin (at address 1).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_01 = CreateDigitalPin(1, "GPIO 1");

		/// <summary>
		/// The GPIO 2 pin (at address 2).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_02 = CreateDigitalPin(2, "GPIO 2");

		/// <summary>
		/// The GPIO 3 pin (at address 3).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_03 = CreateDigitalPin(3, "GPIO 3");

		/// <summary>
		/// The GPIO 4 pin (at address 4).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_04 = CreateDigitalPin(4, "GPIO 4");

		/// <summary>
		/// The GPIO 5 pin (at address 5).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_05 = CreateDigitalPin(5, "GPIO 5");

		/// <summary>
		/// The GPIO 6 pin (at address 6).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_06 = CreateDigitalPin(6, "GPIO 6");

		/// <summary>
		/// The GPIO 7 pin (at address 7).
		/// </summary>
		public static readonly IPCF8574Pin GPIO_07 = CreateDigitalPin(7, "GPIO 7");

		/// <summary>
		/// An array of all 8 pins.
		/// </summary>
		public static readonly IPCF8574Pin[] ALL = {
			GPIO_00,
			GPIO_01,
			GPIO_02,
			GPIO_03,
			GPIO_04,
			GPIO_05,
			GPIO_06,
			GPIO_07
		};
		#endregion

		#region Fields
		private Boolean _isDisposed = false;
		private String _name = String.Empty;
		private String _provName = String.Empty;
		private Object _tag = null;
		private PinState _state = PinState.Low;
		private PinMode _mode = PinMode.TRI;
		private Int32 _address = -1;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>
		/// class. This is the default constructor.
		/// </summary>
		public PCF8574Pin() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>
		/// class with the address of the pin.
		/// </summary>
		/// <param name="address">
		/// The address of the pin.
		/// </param>
		public PCF8574Pin(Int32 address) {
			this._address = address;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>
		/// class with the address and name of the pin.
		/// </summary>
		/// <param name="address">
		/// The address of the pin.
		/// </param>
		/// <param name="name">
		/// The name of the pin.
		/// </param>
		public PCF8574Pin(Int32 address, String name)
			: this(address) {
			this._name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>
		/// class with the GPIO provider, address, and name of the pin.
		/// </summary>
		/// <param name="provider">
		/// The GPIO pin provider.
		/// </param>
		/// <param name="address">
		/// The address of the pin.
		/// </param>
		/// <param name="name">
		/// The name of the pin.
		/// </param>
		public PCF8574Pin(String provider, Int32 address, String name)
			: this(address, name) {
			this._provName = provider;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			this._name = null;
			this._provName = null;
			this._tag = null;
			this._state = PinState.Low;
			this._mode = PinMode.TRI;
			this._address = -1;
			this._isDisposed = true;
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
		/// Gets the state of the pin.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public PinState State {
			get { return this._state; }
		}

		/// <summary>
		/// Gets the pin mode.
		/// </summary>
		/// <value>
		/// The mode.
		/// </value>
		public PinMode Mode {
			get { return this._mode; }
		}

		/// <summary>
		/// Gets the pin address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		public Int32 Address {
			get { return this._address; }
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574Pin"/>.
		/// </returns>
		public override string ToString() {
			return this._name;
		}

		/// <summary>
		/// Sets the state of the pin.
		/// </summary>
		/// <param name="state">
		/// The pin state.
		/// </param>
		public void SetState(PinState state) {
			this._state = state;
		}

		/// <summary>
		/// Sets the pin mode.
		/// </summary>
		/// <param name="mode">
		/// The pin mode.
		/// </param>
		public void SetMode(PinMode mode) {
			this._mode = mode;
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Factory method for creating a digital PCF8574 pin.
		/// </summary>
		/// <param name="address">
		/// The pin address.
		/// </param>
		/// <param name="name">
		/// The pin name.
		/// </param>
		/// <returns>
		/// A new pin instance.
		/// </returns>
		private static PCF8574Pin CreateDigitalPin(Int32 address, String name) {
			return new PCF8574Pin(PCF8574GpioProvider.NAME, address, name);
		}
		#endregion
	}
}

