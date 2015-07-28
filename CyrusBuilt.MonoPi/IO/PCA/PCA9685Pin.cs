//
//  PCA9685Pin.cs
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

namespace CyrusBuilt.MonoPi.IO.PCA
{
	/// <summary>
	/// Provides all 16 PWM channels provided by the PCA9685 I2C 12-bit PWM
	/// LED/Servo controller.
	/// </summary>
	public class PCA9685Pin : IPCA9685Pin
	{
		#region Pin Constants
		/// <summary>
		/// PWM channel 0.
		/// </summary>
		public static readonly IPCA9685Pin PWM_00 = CreatePwmPin(0, "PWM 0");

		/// <summary>
		/// PWM channel 1.
		/// </summary>
		public static readonly IPCA9685Pin PWM_01 = CreatePwmPin(1, "PWM 1");

		/// <summary>
		/// PWM channel 2.
		/// </summary>
		public static readonly IPCA9685Pin PWM_02 = CreatePwmPin(2, "PWM 2");

		/// <summary>
		/// PWM channel 3.
		/// </summary>
		public static readonly IPCA9685Pin PWM_03 = CreatePwmPin(3, "PWM 3");

		/// <summary>
		/// PWM channel 4.
		/// </summary>
		public static readonly IPCA9685Pin PWM_04 = CreatePwmPin(4, "PWM 4");

		/// <summary>
		/// PWM channel 5.
		/// </summary>
		public static readonly IPCA9685Pin PWM_05 = CreatePwmPin(5, "PWM 5");

		/// <summary>
		/// PWM channel 6.
		/// </summary>
		public static readonly IPCA9685Pin PWM_06 = CreatePwmPin(6, "PWM 6");

		/// <summary>
		/// PWM channel 7.
		/// </summary>
		public static readonly IPCA9685Pin PWM_07 = CreatePwmPin(7, "PWM 7");

		/// <summary>
		/// PWM channel 8.
		/// </summary>
		public static readonly IPCA9685Pin PWM_08 = CreatePwmPin(8, "PWM 8");

		/// <summary>
		/// PWM channel 9.
		/// </summary>
		public static readonly IPCA9685Pin PWM_09 = CreatePwmPin(9, "PWM 9");

		/// <summary>
		/// PWM channel 10.
		/// </summary>
		public static readonly IPCA9685Pin PWM_10 = CreatePwmPin(10, "PWM 10");

		/// <summary>
		/// PWM channel 11.
		/// </summary>
		public static readonly IPCA9685Pin PWM_11 = CreatePwmPin(11, "PWM 11");

		/// <summary>
		/// PWM channel 12.
		/// </summary>
		public static readonly IPCA9685Pin PWM_12 = CreatePwmPin(12, "PWM 12");

		/// <summary>
		/// PWM channel 13.
		/// </summary>
		public static readonly IPCA9685Pin PWM_13 = CreatePwmPin(13, "PWM 13");

		/// <summary>
		/// PWM channel 14.
		/// </summary>
		public static readonly IPCA9685Pin PWM_14 = CreatePwmPin(14, "PWM 14");

		/// <summary>
		/// PWM channel 15.
		/// </summary>
		public static readonly IPCA9685Pin PWM_15 = CreatePwmPin(15, "PWM 15");

		/// <summary>
		/// All channels.
		/// </summary>
		public static readonly IPCA9685Pin[] ALL = {
			PCA9685Pin.PWM_00,
			PCA9685Pin.PWM_01,
			PCA9685Pin.PWM_02,
			PCA9685Pin.PWM_03,
			PCA9685Pin.PWM_04,
			PCA9685Pin.PWM_05,
			PCA9685Pin.PWM_06,
			PCA9685Pin.PWM_07,
			PCA9685Pin.PWM_08,
			PCA9685Pin.PWM_09,
			PCA9685Pin.PWM_10,
			PCA9685Pin.PWM_11,
			PCA9685Pin.PWM_12,
			PCA9685Pin.PWM_13,
			PCA9685Pin.PWM_14,
			PCA9685Pin.PWM_15
		};
		#endregion

		#region Fields
		private String _name = String.Empty;
		private String _provName = String.Empty;
		private Object _tag = null;
		private Boolean _isDisposed = false;
		private PinState _state = PinState.Low;
		private PinMode _mode = PinMode.PWM;
		private Int32 _address = -1;
		private Int32 _pwmOnValue = 100;
		private Int32 _pwmOffValue = 0;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>
		/// class. This is the default constructor.
		/// </summary>
		public PCA9685Pin() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>
		/// class with the address of the pin.
		/// </summary>
		/// <param name="address">
		/// The pin address.
		/// </param>
		public PCA9685Pin(Int32 address) {
			this._address = address;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>
		/// class with the address and name of the pin.
		/// </summary>
		/// <param name="address">
		/// The pin address.
		/// </param>
		/// <param name="name">
		/// The name of the pin.
		/// </param>
		public PCA9685Pin(Int32 address, String name) {
			this._address = address;
			this._name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>
		/// class
		/// </summary>
		/// <param name="provider">
		/// The name of the pin provider.
		/// </param>
		/// <param name="address">
		/// The pin address.
		/// </param>
		/// <param name="name">
		/// The name of the pin.
		/// </param>
		public PCA9685Pin(String provider, Int32 address, String name) {
			this._provName = provider;
			this._address = address;
			this._name = name;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}
			this._name = String.Empty;
			this._provName = String.Empty;
			this._tag = null;
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
		/// The name of the pin.
		/// </value>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		/// <value>
		/// The object to tag the pin with.
		/// </value>
		public Object Tag {
			get { return this._tag; }
			set { this._tag = value; }
		}

		/// <summary>
		/// Gets the name of the provider.
		/// </summary>
		/// <value>
		/// The name of the provider.
		/// </value>
		public String ProviderName {
			get { return this._provName; }
		}

		/// <summary>
		/// Gets the state of the pin.
		/// </summary>
		/// <value>
		/// The pin state.
		/// </value>
		public PinState State {
			get { return this._state; }
		}

		/// <summary>
		/// Gets the pin mode.
		/// </summary>
		/// <value>
		/// The pin mode.
		/// </value>
		public PinMode Mode {
			get { return this._mode; }
		}

		/// <summary>
		/// Gets the pin address.
		/// </summary>
		/// <value>
		/// The pin address.
		/// </value>
		public Int32 Address {
			get { return this._address; }
		}

		/// <summary>
		/// Gets or sets the PWM value that constitutes the ON position.
		/// </summary>
		/// <value>
		/// The PWM value that will be considered ON (high).
		/// </value>
		public Int32 PwmOnValue {
			get { return this._pwmOnValue; }
			set { this._pwmOnValue = value; }
		}

		/// <summary>
		/// Gets or sets the PWM value that constitutes the OFF position.
		/// </summary>
		/// <value>
		/// The PWM value that will be considered OFF (low).
		/// </value>
		public Int32 PwmOffValue {
			get { return this._pwmOffValue; }
			set { this._pwmOffValue = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Factory method for creating instances of this class.
		/// </summary>
		/// <returns>
		/// A <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/> instance.
		/// </returns>
		/// <param name="channel">
		/// The channel (address) of the pin.
		/// </param>
		/// <param name="name">
		/// The name of the pin.
		/// </param>
		private static IPCA9685Pin CreatePwmPin(Int32 channel, String name) {
			return new PCA9685Pin(PCA9685GpioProvider.NAME, channel, name);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685Pin"/>.
		/// </returns>
		public override String ToString() {
			if (!String.IsNullOrEmpty(this._name)) {
				return this._name;
			}
			return this._address.ToString();
		}
		#endregion
	}
}

