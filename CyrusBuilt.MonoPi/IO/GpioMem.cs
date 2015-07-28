//
//  GpioMem.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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
//  Derived from https://github.com/cypherkey/RaspberryPi.Net
//  by Aaron Anderson <aanderson@netopia.ca>
//
using System;
using System.Diagnostics;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Raspberry Pi GPIO using the direct memory access method.
	/// This requires the bcm2835 GPIO library provided by 
	/// Mike McCauley (mikem@open.com.au) at http://www.open.com.au/mikem/bcm2835/index.html.
	/// 
	/// To create the shared object, download the source code from the link above.
	/// The standard Makefile compiles a statically linked library. To build a
	/// shared object, do:
	///    tar -zxf bcm2835-1.3.tar.gz
	///    cd bcm2835-1.3/src
	///    make libbcm2835.a
	///    cc -shared bcm2835.o -o libbcm2835.so
	/// Place the shared object in the same directory as the executable and
	/// other assemblies.
	/// </summary>
	public class GpioMem : GpioBase
	{
		#region Fields
		private Boolean _isPWM = false;
		private UInt32 _pwm = 0;
		private PinState _lastState = PinState.Low;
		private PwmChannel _pwmChannel = PwmChannel.Channel0;
		private PwmMode _pwmMode = PwmMode.Balanced;
		private PwmClockDivider _divisor = PwmClockDivider.Divisor1;
		private UInt32 _pwmRange = 0;
		private static Boolean _initialized = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// class with the pin to initialize, the I/O direction, and the initial
		/// value to write to the pin.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <param name="mode">
		/// The I/0 mode of the pin.
		/// </param>
		/// <param name="initialValue">
		/// The pin's initial value.
		/// </param>
		/// <remarks>
		/// Access to the specified GPIO setup with the specified direction
		/// with the specified initial value.
		/// </remarks>			
		public GpioMem(GpioPins pin, PinMode mode, PinState initialValue)
			: base(pin, mode, initialValue) {
			base._initValue = initialValue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// class with the pin to initialize and the I/O direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <param name="mode">
		/// The I/0 mode of the pin.
		/// </param>
		/// <remarks>
		/// Access to the specified GPIO setup with the specified direction
		/// with an initial value of LOW (0).
		/// </remarks>			
		public GpioMem(GpioPins pin, PinMode mode)
			: base(pin, mode, PinState.Low) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// class with the pin to initialize.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <remarks>
		/// Access to the specified GPIO setup as an output port with an initial
		/// value of false (0).
		/// </remarks>			
		public GpioMem(GpioPins pin)
			: base(pin, PinMode.OUT, PinState.Low) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether or not the GPIO is initialized.
		/// </summary>
		public static Boolean Initialized {
			get { return _initialized; }
		}

		/// <summary>
		/// Gets or sets the selected PWM channel.
		/// </summary>
		/// <value>
		/// The selected PWM channel. Default is <see cref="CyrusBuilt.MonoPi.IO.PwmChannel.Channel0"/>.
		/// </value>
		public PwmChannel SelectedPwmChannel {
			get { return this._pwmChannel; }
			set { this._pwmChannel = value; }
		}

		/// <summary>
		/// Gets or sets the selected PWM mode.
		/// </summary>
		/// <value>
		/// The selected PWM mode.
		/// </value>
		public PwmMode SelectedPwmMode {
			get { return this._pwmMode; }
			set { this._pwmMode = value; }
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
		public override UInt32 PWMRange {
			get { return this._pwmRange; }
			set {
				if (value < 0) {
					value = 0;
				}

				if (value > 1024) {
					value = 1024;
				}
				this._pwmRange = value;
			}
		}

		/// <summary>
		/// Gets or sets the PWM (Pulse Width Modulation) value.
		/// </summary>
		/// <value>
		/// The PWM value.
		/// </value>
		/// <exception cref="InvalidOperationException">
		/// The pin is configured as in input pin instead of output.
		/// </exception>
		public override UInt32 PWM {
			get { return this._pwm; }
			set {
				if (base.Mode == PinMode.IN) {
					throw new InvalidOperationException("Cannot set PWM value on an input pin.");
				}

				if (value < 0) {
					value = 0;
				}

				if (value > this._pwmRange) {
					value = this._pwmRange;
				}

				if (this._pwm != value) {
					this._pwm = value;
					if (!this._isPWM) {
						base._mode = PinMode.PWM;
						this._isPWM = true;
						this.Provision();
					}
					UnsafeNativeMethods.bcm2835_pwm_set_data((uint)this._pwmChannel, value);
				}
			}
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Initialize the memory access to the GPIO.
		/// </summary>
		/// <returns>
		/// true if initialized; Otherwise, false.
		/// </returns>
		public static Boolean Initialize() {
			Int32 ret = 1;
			if (!_initialized) {
				// Init the mapped memory.
				ret = UnsafeNativeMethods.bcm2835_init();
				_initialized = true;
			}
			return (ret == 0) ? false : true;
		}

		/// <summary>
		/// Export the GPIO setting the direction.
		/// </summary>
		/// <param name="pin">
		/// The pin to export.
		/// </param>
		/// <param name="mode">
		/// The I/0 mode of the pin.
		/// </param>
		private static void internal_ExportPin(Int32 pin, PinMode mode) {
			Initialize();

			// If the pin is already exported, check it's in the proper direction.
			if (ExportedPins.ContainsKey(pin)) {
				// If the direction matches, return out of the function. If not,
				// change the direction.
				if (ExportedPins[pin] == mode) {
					return;
				}
			}

			// Set the direction on the pin and update the exported list.
			// BCM2835_GPIO_FSEL_INPT = 0
			// BCM2835_GPIO_FSEL_OUTP = 1
			UnsafeNativeMethods.bcm2835_gpio_fsel((uint)pin, (uint)mode);
			if (mode == PinMode.IN) {
				// BCM2835_GPIO_PUD_OFF = 0b00 = 0
				// BCM2835_GPIO_PUD_DOWN = 0b01 = 1
				// BCM2835_GPIO_PUD_UP = 0b10 = 2
				UnsafeNativeMethods.bcm2835_gpio_set_pud((uint)pin, 0);
			}
			ExportedPins[pin] = mode;
		}

		/// <summary>
		/// Exports the pin setting the direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to export.
		/// </param>
		/// <param name="mode">
		/// The I/0 mode of the pin.
		/// </param>
		private static void ExportPin(GpioPins pin, PinMode mode) {
			internal_ExportPin((Int32)pin, mode);
		}

		/// <summary>
		/// Unexports an exported pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to unexport.
		/// </param>
		/// <param name="gpionum">
		/// The GPIO number.
		/// </param>
		private static void internal_UnexportPin(Int32 pin, String gpionum) {
			Debug.WriteLine("Unexporting pin " + pin.ToString());
			// TODO Somehow reverse what we did in internal_ExportPin? Is there
			// a way to "free" the pin in the libbcm2835 library?
			UnsafeNativeMethods.bcm2835_gpio_write((uint)pin, (uint)PinState.Low);
			UnsafeNativeMethods.bcm2835_gpio_fsel((uint)pin, (uint)PinMode.TRI);
			ExportedPins.Remove(pin);
		}

		/// <summary>
		/// Unexport the GPIO.
		/// </summary>
		/// <param name="pin">
		/// The pin to unexport.
		/// </param>
		private static void UnexportPin(GpioPins pin) {
			internal_UnexportPin((Int32)pin, GetGpioPinNumber(pin));
		}

		/// <summary>
		/// Write the value to the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to write to.
		/// </param>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		/// <param name="pinname">
		/// The name of the GPIO associated with the pin.
		/// </param>
		private static void internal_Write(Int32 pin, PinState value, String pinname) {
			if (pin == (Int32)GpioPins.GPIO_NONE) {
				return;
			}
				
			UnsafeNativeMethods.bcm2835_gpio_write((uint)pin, (uint)value);
			Debug.WriteLine("Output to pin " + pinname + "/gpio" + pin.ToString() + ", value was " + ((Int32)value).ToString());
		}

		/// <summary>
		/// Write the specified pin and value.
		/// </summary>
		/// <param name="pin">
		/// The pin to write to.
		/// </param>
		/// <param name="value">
		/// The value to write.
		/// </param>
		public static void Write(GpioPins pin, PinState value) {
			String name = Enum.GetName(typeof(GpioPins), pin);
			internal_Write((Int32)pin, value, name);
		}

		/// <summary>
		/// Read the value of the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to read.
		/// </param>
		/// <param name="pinname">
		/// The name of the GPIO associated with the pin.
		/// </param>
		/// <returns>
		/// The value read from the pin.
		/// </returns>
		private static PinState internal_Read(Int32 pin, String pinname) {
			internal_ExportPin(pin, PinMode.IN);
			uint value = UnsafeNativeMethods.bcm2835_gpio_lev((uint)pin);
			PinState returnValue = (value == 0) ? PinState.High : PinState.Low;
			Debug.WriteLine("Input from pin " + pinname + "/gpio" + pin.ToString() + ", value was " + ((Int32)returnValue).ToString());
			return returnValue;
		}

		/// <summary>
		/// Read the specified Revision 1.0 pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to read.
		/// </param>
		/// <returns>
		/// The value read from the pin.
		/// </returns>			
		public static PinState Read(GpioPins pin) {
			String name = Enum.GetName(typeof(GpioPins), pin);
			return internal_Read((Int32)pin, name);
		}

		/// <summary>
		/// Destroy this GPIO factory.
		/// </summary>
		public static void Destroy() {
			if (ExportedPins != null) {
				if (ExportedPins.Count > 0) {
					foreach (Int32 pin in ExportedPins.Keys) {
						internal_UnexportPin(pin, pin.ToString());
					}
					ExportedPins.Clear();
				}
			}
			UnsafeNativeMethods.bcm2835_close();
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Provisions the pin (initialize to specified mode and make active).
		/// </summary>
		public override void Provision() {
			ExportPin(base.InnerPin, base.Mode);
			Write(base.InnerPin, base._initValue);
			if (base.Mode == PinMode.PWM) {
				UnsafeNativeMethods.bcm2835_pwm_set_clock((uint)this._divisor);
				UnsafeNativeMethods.bcm2835_pwm_set_mode((uint)this._pwmChannel, (uint)this._pwmMode, 1);
				UnsafeNativeMethods.bcm2835_pwm_set_range((uint)this._pwmChannel, (uint)this._pwmRange);
			}
		}

		/// <summary>
		/// Write the specified value to the pin.
		/// </summary>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		public override void Write(PinState value) {
			Write(base.InnerPin, value);
			base.Write(value);
			if (this._lastState != value) {
				this.OnStateChanged(new PinStateChangeEventArgs(this._lastState, value));
			}
		}

		/// <summary>
		/// Pulse the pin output for the specified number of milliseconds.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to wait between states.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// An attempt was made to pulse an input pin.
		/// </exception>
		public override void Pulse(Int32 millis) {
			if (base.Mode == PinMode.IN) {
				throw new InvalidOperationException("You cannot pulse a pin set as an input.");
			}
			Write(base.InnerPin, PinState.High);
			this.OnStateChanged(new PinStateChangeEventArgs(base.State, PinState.High));
			base.Pulse(millis);
			Write(base.InnerPin, PinState.Low);
			this.OnStateChanged(new PinStateChangeEventArgs(base.State, PinState.Low));
		}

		/// <summary>
		/// Pulses the pin for 500ms.
		/// </summary>
		public void Pulse() {
			this.Pulse(500);
		}

		/// <summary>
		/// Read a value from the pin.
		/// </summary>
		/// <returns>
		/// The value read from the pin.
		/// </returns>
		public override PinState Read() {
			PinState newState = Read(base.InnerPin);
			if (this._lastState != newState) {
				this.OnStateChanged(new PinStateChangeEventArgs(this._lastState, newState));
			}
			return newState;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// so the garbage collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			this.Write(PinState.Low);
			if (this.Mode == PinMode.PWM) {
				UnsafeNativeMethods.bcm2835_pwm_set_mode((uint)this._pwmChannel, (uint)this._pwmMode, 0);
			}

			UnexportPin(base.InnerPin);
			Destroy();
			if (this._isPWM) {
				String cmd = "gpio unexport " + GetGpioPinNumber(base.InnerPin);
				Process.Start(cmd);
			}
			base.Dispose();
		}
		#endregion
	}
}

