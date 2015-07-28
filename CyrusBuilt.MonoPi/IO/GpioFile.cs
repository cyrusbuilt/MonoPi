//
//  GpioFile.cs
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
using System.IO;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Raspberry Pi GPIO using the file-based access method.
	/// </summary>
	public class GpioFile : GpioBase
	{
		#region Fields
		private PinState _lastState = PinState.Low;
		private UInt32 _pwm = 0;
		private UInt32 _pwmRange = 1024;
		private Boolean _isPWM = false;

		/// <summary>
		/// The path on the Raspberry Pi for the GPIO interface.
		/// </summary>
		private const String GPIO_PATH = "/sys/class/gpio/";
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/>
		/// class with the Rev1 pin to access, the I/O direction, and the initial value.
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
		public GpioFile(GpioPins pin, PinMode mode, PinState initialValue)
			: base(pin, mode, initialValue) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/>
		/// class with the Rev1 pin to access and the I/O direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <param name="mode">
		/// The I/0 mode of the pin.
		/// </param>
		public GpioFile(GpioPins pin, PinMode mode)
			: base(pin, mode, PinState.Low) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/>
		/// class with the Rev1 pin to access.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		public GpioFile(GpioPins pin)
			: base(pin, PinMode.OUT, PinState.Low) {
		}
		#endregion

		#region Properties
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
				if (base.Mode != PinMode.PWM) {
					throw new InvalidOperationException("Cannot set PWM value on a pin not configured for PWM.");
				}

				if (value < 0) {
					value = 0;
				}

				if (value > 1023) {
					value = 1023;
				}

				if (this._pwm != value) {
					this._pwm = value;
					String cmd = String.Empty;
					if (!this._isPWM) {
						cmd = "gpio mode " + GetGpioPinNumber(base.InnerPin) + " pwm";
						Process.Start(cmd);
						this._isPWM = true;
					}
					cmd = "gpio pwm " + GetGpioPinNumber(base.InnerPin) + " " + this._pwm.ToString();
					Process.Start(cmd);
				}
			}
		}

		/// <summary>
		/// Gets or sets the PWM range.
		/// </summary>
		/// <value>
		/// The PWM range. Default is 1024.
		/// </value>
		/// <remarks>
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

				if (this._pwmRange != value) {
					this._pwmRange = value;
				}
			}
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Exports the GPIO setting the direction. This creates the
		/// /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin.
		/// </param>
		/// <param name="mode">
		/// The I/O mode.
		/// </param>
		/// <param name="pinnum">
		/// The pin number.
		/// </param>
		/// <param name="pinname">
		/// The name of the pin.
		/// </param>
		private static void internal_ExportPin(Int32 pin, PinMode mode, String pinnum, String pinname) {
			String pinpath = GPIO_PATH + "gpio" + pinnum;
			String m = Enum.GetName(typeof(PinMode), mode);

			// If the pin is already exported, check it's in the proper direction.
			if (ExportedPins.ContainsKey(pin)) {
				// If the direction matches, return out of the function. If not,
				// change the direction.
				if (ExportedPins[pin] == mode) {
					return;
				}
				else {
					// Set the direction on the pin and update the exported list.
					File.WriteAllText(pinpath + "/direction", m);
					ExportedPins[pin] = mode;
					return;
				}
			}

			// Export.
			if (!Directory.Exists(pinpath)) {
				Debug.WriteLine("Exporting pin " + pinnum);
				File.WriteAllText(GPIO_PATH + "export", pinnum);
			}

			// Set I/O direction.
			Debug.WriteLine("Setting direction on pin " + pinname + "/gpio" + pin.ToString() + " as " + m);
			File.WriteAllText(pinpath + "/direction", m);

			// Update the pin.
			ExportedPins[pin] = mode;
		}

		/// <summary>
		/// Exports the GPIO setting the direction. This creates the
		/// /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin on the board.
		/// </param>
		/// <param name="mode">
		/// The I/O mode.
		/// </param>
		private static void ExportPin(GpioPins pin, PinMode mode) {
			String name = Enum.GetName(typeof(GpioPins), pin);
			internal_ExportPin((Int32)pin, mode, GetGpioPinNumber(pin), name);
		}

		/// <summary>
		/// Provisions the pin (initialize to specified mode and make active).
		/// </summary>
		public override void Provision() {
			ExportPin(base.InnerPin, base.Mode);
			Write(base.InnerPin, base._initValue);
		}

		/// <summary>
		/// Unexport the GPIO. This removes the /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin to unexport.
		/// </param>
		/// <param name="gpioNum">
		/// The GPIO number associated with the pin.
		/// </param>
		private static void internal_UnexportPin(Int32 pin, String gpioNum) {
			Debug.WriteLine("Unexporting pin " + pin.ToString());
			File.WriteAllText(GPIO_PATH + "unexport", gpioNum);
			ExportedPins.Remove(pin);
		}

		/// <summary>
		/// Unexport the GPIO. This removes the /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin to unexport.
		/// </param>
		private static void UnexportPin(GpioPins pin) {
			Write(pin, PinState.Low);
			internal_UnexportPin((Int32)pin, GetGpioPinNumber(pin));
		}

		/// <summary>
		/// Writes the specified value to the specified GPIO pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to write the value to.
		/// </param>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		/// <param name="gpionum">
		/// The GPIO number associated with the pin.
		/// </param>
		/// <param name="pinname">
		/// The name of the pin.
		/// </param>
		private static void internal_Write(Int32 pin, PinState value, String gpionum, String pinname) {
			// GPIO_NONE is the same value for both Rev1 and Rev2 boards.
			if (pin == (Int32)GpioPins.GPIO_NONE) {
				return;
			}

			internal_ExportPin(pin, PinMode.OUT, gpionum, pinname);
			String val = ((Int32)value).ToString();
			String path = GPIO_PATH + "gpio" + gpionum + "/value";

			File.WriteAllText(path, val);
			Debug.WriteLine("Output to pin " + pinname + "/gpio" + pin.ToString() + ", value was " + val);
		}

		/// <summary>
		/// Writes the specified value to the specified GPIO pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to write the value to.
		/// </param>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		public static void Write(GpioPins pin, PinState value) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPins), pin);
			internal_Write((Int32)pin, value, num, name);
		}

		/// <summary>
		/// Reads the value of the specified GPIO pin.
		/// </summary>
		/// <param name="pin">
		/// The physical pin associated with the GPIO pin.
		/// </param>
		/// <param name="gpionum">
		/// The GPIO pin number.
		/// </param>
		/// <param name="gpioname">
		/// The name of the GPIO.
		/// </param>
		/// <returns>
		/// The value of the pin.
		/// </returns>
		/// <exception cref="IOException">
		/// The specified pin could not be read (device does path does not exist).
		/// </exception>
		private static PinState internal_Read(Int32 pin, String gpionum, String gpioname) {
			PinState returnValue = PinState.Low;
			internal_ExportPin(pin, PinMode.IN, gpionum, gpioname);
			String filename = GPIO_PATH + "gpio" + gpionum + "/value";
			if (File.Exists(filename)) {
				String readvalue = File.ReadAllText(filename);
				if ((readvalue.Length > 0) && (Int32.Parse(readvalue.Substring(0, 1)) == 1)) {
					returnValue = PinState.High;
				}
			}
			else {
				throw new IOException("Cannot read from pin " + gpionum + ". Device does not exist.");
			}

			Debug.WriteLine("Input from pin " + gpioname + "/gpio" + gpionum + ", value was " + ((Int32)returnValue).ToString());
			return returnValue;
		}

		/// <summary>
		/// Read a value from the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to read from.
		/// </param>
		/// <returns>
		/// The value read from the pin (high or low).
		/// </returns>
		/// <exception cref="IOException">
		/// The specified pin could not be read (device does path does not exist).
		/// </exception>
		public static PinState Read(GpioPins pin) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPins), pin);
			return internal_Read((Int32)pin, num, name);
		}

		/// <summary>
		/// Unexports all pins in the registry.
		/// </summary>
		public static void Cleanup() {
			if (ExportedPins != null) {
				if (ExportedPins.Count > 0) {
					foreach (Int32 pin in ExportedPins.Keys) {
						internal_UnexportPin(pin, pin.ToString());
					}
				}
			}
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Write the specified value to the pin.
		/// </summary>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		public override void Write(PinState value) {
			base.Write(value);
			Write(base.InnerPin, value);
			if (this._lastState != base.State) {
				this.OnStateChanged(new PinStateChangeEventArgs(this._lastState, base.State));
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
		/// Pulse the pin output for 500ms.
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
		/// <exception cref="IOException">
		/// Cannot read the value from the pin. The path does not exist.
		/// </exception>
		public override PinState Read() {
			PinState val = Read(base.InnerPin);
			if (this._lastState != val) {
				this.OnStateChanged(new PinStateChangeEventArgs(this._lastState, val));
			}
			return val;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> so
		/// the garbage collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.IO.GpioFile"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			UnexportPin(base.InnerPin);
			Cleanup();
			if (this._isPWM) {
				String cmd = "gpio unexport " + GetGpioPinNumber(base.InnerPin);
				Process.Start(cmd);
			}
			base.Write(PinState.Low);
			base.Dispose();
		}
		#endregion
	}
}

