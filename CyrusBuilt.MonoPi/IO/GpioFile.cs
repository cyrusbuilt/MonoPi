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
		/// <summary>
		/// The path on the Raspberry Pi for the GPIO interface.
		/// </summary>
		private const String GPIO_PATH = "/sys/class/gpio/";

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev1 pin to access, the I/O direction, and the initial value.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 1.0 board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		/// <param name="initialValue">
		/// The pin's initial value.
		/// </param>
		public GpioFile(GpioPinsRev1 pin, PinDirection direction, Boolean initialValue)
			: base(pin, direction, initialValue) {
			ExportPin(pin, direction);
			Write(pin, initialValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev2 pin to access, the I/O direction, and the intial value.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 2.0 board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		/// <param name="initialValue">
		/// The pin's initial value.
		/// </param>
		public GpioFile(GpioPinsRev2 pin, PinDirection direction, Boolean initialValue)
			: base(pin, direction, initialValue) {
			ExportPin(pin, direction);
			Write(pin, initialValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev1 pin to access and the I/O direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 1.0 board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		public GpioFile(GpioPinsRev1 pin, PinDirection direction)
			: base(pin, direction, false) {
			ExportPin(pin, direction);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev2 pin to access and the I/O direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 2.0 board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		public GpioFile(GpioPinsRev2 pin, PinDirection direction)
			: base(pin, direction, false) {
			ExportPin(pin, direction);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev1 pin to access.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 1.0 board to access.
		/// </param>
		public GpioFile(GpioPinsRev1 pin)
			: base(pin, PinDirection.OUT, false) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.GpioFile"/>
		/// class with the Rev2 pin to access.
		/// </summary>
		/// <param name="pin">
		/// The pin on the Revision 2.0 board to access.
		/// </param>
		public GpioFile(GpioPinsRev2 pin)
			: base(pin, PinDirection.OUT, false) {
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
		/// <param name="direction">
		/// The I/O direction.
		/// </param>
		/// <param name="pinnum">
		/// 
		/// </param>
		private static void internal_ExportPin(Int32 pin, PinDirection direction, String pinnum, String pinname) {
			String pinpath = GPIO_PATH + "gpio" + pinnum;
			String dir = Enum.GetName(typeof(PinDirection), direction);

			// If the pin is already exported, check it's in the proper direction.
			if (_exportedPins.ContainsKey(pin)) {
				// If the direction matches, return out of the function. If not,
				// change the direction.
				if (_exportedPins[pin] == direction) {
					return;
				}
				else {
					// Set the direction on the pin and update the exported list.
					File.WriteAllText(pinpath + "/direction", dir);
					_exportedPins[pin] = direction;
					return;
				}
			}

			// Export.
			if (!Directory.Exists(pinpath)) {
				Debug.WriteLine("Exporting pin " + pinnum);
				File.WriteAllText(GPIO_PATH + "export", pinnum);
			}

			// Set I/O direction.
			Debug.WriteLine("Setting direction on pin " + pinname + "/gpio" + pin.ToString() + " as " + dir);
			File.WriteAllText(pinpath + "/direction", dir);

			// Update the pin.
			_exportedPins[pin] = direction;
		}

		/// <summary>
		/// Exports the GPIO setting the direction. This creates the
		/// /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin on a Revision 1.0 board.
		/// </param>
		/// <param name="direction">
		/// The I/O direction.
		/// </param>
		private static void ExportPin(GpioPinsRev1 pin, PinDirection direction) {
			String name = Enum.GetName(typeof(GpioPinsRev1), pin);
			internal_ExportPin((Int32)pin, direction, GetGpioPinNumber(pin), name);
		}

		/// <summary>
		/// Exports the GPIO setting the direction. This creates the
		/// /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The GPIO pin on a Revision 2.0 board.
		/// </param>
		/// <param name="direction">
		/// The I/O direction.
		/// </param>
		private static void ExportPin(GpioPinsRev2 pin, PinDirection direction) {
			String name = Enum.GetName(typeof(GpioPinsRev2), pin);
			internal_ExportPin((Int32)pin, direction, GetGpioPinNumber(pin), name);
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
			_exportedPins.Remove(pin);
		}

		/// <summary>
		/// Unexport the GPIO. This removes the /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The Revision 1.0 GPIO pin to unexport.
		/// </param>
		private static void UnexportPin(GpioPinsRev1 pin) {
			internal_UnexportPin((Int32)pin, GetGpioPinNumber(pin));
		}

		/// <summary>
		/// Unexport the GPIO. This removes the /sys/class/gpio/gpioXX directory.
		/// </summary>
		/// <param name="pin">
		/// The Revision 2.0 GPIO pin to unexport.
		/// </param>
		private static void UnexportPin(GpioPinsRev2 pin) {
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
		private static void internal_Write(Int32 pin, Boolean value, String gpionum, String pinname) {
			// GPIO_NONE is the same value for both Rev1 and Rev2 boards.
			if (pin == (Int32)GpioPinsRev1.GPIO_NONE) {
				return;
			}

			internal_ExportPin(pin, PinDirection.OUT, gpionum, pinname);
			String val = value ? "1" : "0";
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
		public static void Write(GpioPinsRev1 pin, Boolean value) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPinsRev1), pin);
			internal_Write((Int32)pin, value, num, name);
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
		public static void Write(GpioPinsRev2 pin, Boolean value) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPinsRev2), pin);
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
		private static Boolean internal_Read(Int32 pin, String gpionum, String gpioname) {
			Boolean returnValue = false;
			internal_ExportPin(pin, PinDirection.IN, gpionum, gpioname);
			String filename = GPIO_PATH + "gpio" + gpionum + "/value";
			if (File.Exists(filename)) {
				String readvalue = File.ReadAllText(filename);
				if ((readvalue.Length > 0) && (readvalue[0] == 1)) {
					returnValue = true;
				}
			}
			else {
				throw new IOException("Cannot read from pin " + gpionum + ". Device does not exist.");
			}

			Debug.WriteLine("Input from pin " + gpioname + "/gpio" + gpionum + ", value was " + returnValue.ToString());
			return returnValue;
		}

		/// <summary>
		/// Read a value from the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The Revision 1.0 pin to read from.
		/// </param>
		/// <exception cref="IOException">
		/// The specified pin could not be read (device does path does not exist).
		/// </exception>
		public static Boolean Read(GpioPinsRev1 pin) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPinsRev1), pin);
			return internal_Read((Int32)pin, num, name);
		}

		/// <summary>
		/// Read a value from the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The Revision 2.0 pin to read from.
		/// </param>
		/// <exception cref="IOException">
		/// The specified pin could not be read (device does path does not exist).
		/// </exception>
		public static Boolean Read(GpioPinsRev2 pin) {
			String num = GetGpioPinNumber(pin);
			String name = Enum.GetName(typeof(GpioPinsRev2), pin);
			return internal_Read((Int32)pin, num, name);
		}

		/// <summary>
		/// Unexports all pins in the registry.
		/// </summary>
		public static void Cleanup() {
			if (_exportedPins != null) {
				if (_exportedPins.Count > 0) {
					foreach (Int32 pin in _exportedPins.Keys) {
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
		public override void Write(Boolean value) {
			if (_revision == BoardRevision.Rev1) {
				Write(_rev1pin, value);
			}
			else {
				Write(_rev2pin, value);
			}
		}

		/// <summary>
		/// Read a value from the pin.
		/// </summary>
		/// <exception cref="IOException">
		/// Cannot read the value from the pin. The path does not exist.
		/// </exception>
		public override Boolean Read() {
			if (_revision == BoardRevision.Rev1) {
				return Read(_rev1pin);
			}
			return Read(_rev2pin);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.GpioFile"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.GpioFile"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.GpioFile"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="CyrusBuilt.MonoPi.GpioFile"/> so
		/// the garbage collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.GpioFile"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (_revision == BoardRevision.Rev1) {
				UnexportPin(_rev1pin);
			}
			else {
				UnexportPin(_rev2pin);
			}
			Cleanup();
			base.Dispose();
		}
		#endregion
	}
}

