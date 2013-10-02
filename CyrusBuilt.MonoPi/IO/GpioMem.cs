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
		private static Boolean _initialized = false;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// class with the pin to initialize, the I/O direction, and the initial
		/// value to write to the pin.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		/// <param name="initialValue">
		/// The pin's initial value.
		/// </param>
		/// <remarks>
		/// Access to the specified GPIO setup with the specified direction
		/// with the specified initial value.
		/// </remarks>			
		public GpioMem(GpioPins pin, PinDirection direction, Boolean initialValue)
			: base(pin, direction, initialValue) {
			ExportPin(pin, direction);
			Write(pin, initialValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/>
		/// class with the pin to initialize and the I/O direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to access.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		/// <remarks>
		/// Access to the specified GPIO setup with the specified direction
		/// with an initial value of false (0).
		/// </remarks>			
		public GpioMem(GpioPins pin, PinDirection direction)
			: base(pin, direction, false) {
			ExportPin(pin, direction);
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
			: base(pin, PinDirection.OUT, false) {
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
		/// Gets a value indicating whether this instance is initialized.
		/// </summary>
		public Boolean IsInitialized {
			get { return _initialized; }
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
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		private static void internal_ExportPin(Int32 pin, PinDirection direction) {
			Initialize();

			// If the pin is already exported, check it's in the proper direction.
			if (ExportedPins.ContainsKey(pin)) {
				// If the direction matches, return out of the function. If not,
				// change the direction.
				if (ExportedPins[pin] == direction) {
					return;
				}
			}

			// Set the direction on the pin and update the exported list.
			// BCM2835_GPIO_FSEL_INPT = 0
			// BCM2835_GPIO_FSEL_OUTP = 1
			uint pindir = (direction == PinDirection.IN) ? (uint)0 : (uint)1;
			UnsafeNativeMethods.bcm2835_gpio_fsel((uint)pin, pindir);
			if (direction == PinDirection.IN) {
				// BCM2835_GPIO_PUD_OFF = 0b00 = 0
				// BCM2835_GPIO_PUD_DOWN = 0b01 = 1
				// BCM2835_GPIO_PUD_UP = 0b10 = 2
				UnsafeNativeMethods.bcm2835_gpio_set_pud((uint)pin, 0);
			}
			ExportedPins[pin] = direction;
		}

		/// <summary>
		/// Exports the pin setting the direction.
		/// </summary>
		/// <param name="pin">
		/// The pin on the board to export.
		/// </param>
		/// <param name="direction">
		/// The I/0 direction of the pin.
		/// </param>
		private static void ExportPin(GpioPins pin, PinDirection direction) {
			internal_ExportPin((Int32)pin, direction);
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
			UnsafeNativeMethods.bcm2835_gpio_write((uint)pin, 0);
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
		private static void internal_Write(Int32 pin, Boolean value, String pinname) {
			if (pin == (Int32)GpioPins.GPIO_NONE) {
				return;
			}

			uint val = value ? (uint)1 : (uint)0;
			internal_ExportPin(pin, PinDirection.OUT);
			UnsafeNativeMethods.bcm2835_gpio_write((uint)pin, val);
			Debug.WriteLine("Output to pin " + pinname + "/gpio" + pin.ToString() + ", value was " + value.ToString());
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
		public static void Write(GpioPins pin, Boolean value) {
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
		private static Boolean internal_Read(Int32 pin, String pinname) {
			internal_ExportPin(pin, PinDirection.IN);
			uint value = UnsafeNativeMethods.bcm2835_gpio_lev((uint)pin);
			Boolean returnValue = (value == 0) ? false : true;
			Debug.WriteLine("Input from pin " + pinname + "/gpio" + pin.ToString() + ", value was " + returnValue.ToString());
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
		public static Boolean Read(GpioPins pin) {
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
		/// Write the specified value to the pin.
		/// </summary>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		public override void Write(Boolean value) {
			Write(base.InnerPin, value);
			base.Write(value);
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
			if (base.Direction == PinDirection.IN) {
				throw new InvalidOperationException("You cannot pulse a pin set as an input.");
			}
			Write(base.InnerPin, true);
			base.Pulse(millis);
			Write(base.InnerPin, false);
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
		public override Boolean Read() {
			return Read(base.InnerPin);
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
			UnexportPin(base.InnerPin);
			Destroy();
			base.Write(false);
			base.Dispose();
		}
		#endregion
	}
}

