//
//  UnsafeNativeMethods.cs
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
using System;
using System.Runtime.InteropServices;

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// Unmanaged methods providing DMA to bcm2835 GPIOs.
	/// </summary>
	public static class UnsafeNativeMethods
	{
		#region Imported libbcm2835 Methods
		/// <summary>
		/// Initializes the Broadcom 2835 DMA library by opening /dev/mem and
		/// getting pointers to the internal memory for BCM 2835 device
		/// registers. You must call this (successfully) before calling any
		/// other functions in this library (except bcm2835_set_debug). If
		/// bcm2835_init() fails by returning 0, calling any other function may
		/// result in crashes or other failures. Prints messages to stderr in
		/// case of errors.
		/// </summary>
		/// <returns>
		/// 1 if successful; Otherwise, 0;
		/// </returns>			
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_init")]
		internal static extern Int32 bcm2835_init();

		/// <summary>
		/// Close the library, deallocating any allocated memory and closing
		/// /dev/mem.
		/// </summary>
		/// <returns>
		/// 1 if successful; Otherwise, 0;
		/// </returns>			
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_close")]
		internal static extern Int32 bcm2835_close();

		/// <summary>
		/// Sets the Function Select register for the given pin, which configures
		/// the pin as Input, Output or one of the 6 alternate functions.
		/// </summary>
		/// <param name="pin">
		/// GPIO number.
		/// </param>
		/// <param name="mode">
		/// The mode to set the pin to.
		/// </param>
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_gpio_fsel")]
		internal static extern void bcm2835_gpio_fsel(uint pin, uint mode);

		/// <summary>
		/// Sets the output state of the specified pin.
		/// </summary>
		/// <param name="pin">
		/// GPIO number.
		/// </param>
		/// <param name="value">
		/// On HIGH sets the output to HIGH and LOW to LOW.
		/// </param>
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_gpio_write")]
		internal static extern void bcm2835_gpio_write(uint pin, uint value);

		/// <summary>
		/// Reads the current level on the specified pin and returns either HIGH
		/// or LOW. Works whether or not the pin is an input or an output.
		/// </summary>
		/// <param name="pin">
		/// GPIO number.
		/// </param>
		/// <returns>
		/// the current level either HIGH or LOW.
		/// </returns>
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_gpio_lev")]
		internal static extern uint bcm2835_gpio_lev(uint pin);

		/// <summary>
		/// Sets the Pull-up/down mode for the specified pin.
		/// </summary>
		/// <param name="pin">
		/// GPIO number.
		/// </param>
		/// <param name="pud">
		/// The desired Pull-up/down mode.
		/// </param>
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_gpio_set_pud")]
		internal static extern void bcm2835_gpio_set_pud(uint pin, uint pud);
		#endregion

		#region Imported wiringPi Methods
		/// <summary>
		/// Opens the specified serial port device with the specified baud rate.
		/// </summary>
		/// <param name="device">
		/// The device to open (ie. /dev/ttyAMA0).
		/// </param>
		/// <param name="baud">
		/// The BAUD rate (ie. 9600).
		/// </param>
		/// <returns>
		/// A handle ID to the open port. Returns -2 if the specified BAUD rate
		/// is invalid. Returns -1 if the port is already open.
		/// </returns>
		[DllImport("libwiringPi.so", EntryPoint = "serialOpen")]
		internal static extern Int32 serialOpen(String device, Int32 baud);

		/// <summary>
		/// Flush the transmit and receive buffers.
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "serialFlush")]
		internal static extern void serialFlush(Int32 fd);

		/// <summary>
		/// Release the serial port.
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "serialClose")]
		internal static extern void serialClose(Int32 fd);

		/// <summary>
		/// Sends a single character to the serial port.
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		/// <param name="c">
		/// The character to send.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "serialPutchar")]
		internal static extern void serialPutChar(Int32 fd, byte c);

		/// <summary>
		/// Sends a string to the serial port.
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		/// <param name="s">
		/// The string to send.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "serialPuts")]
		internal static extern void serialPuts(Int32 fd, String s);

		/// <summary>
		/// Gets the number of bytes available to be read (if any).
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		/// <returns>
		/// The number of bytes of data avalable to be read in the serial port.
		/// Returns -1, if there is no data available.
		/// </returns>
		[DllImport("libwiringPi.so", EntryPoint = "serialDataAvail")]
		internal static extern Int32 serialDataAvail(Int32 fd);

		/// <summary>
		/// Gets a single character from the serial device.
		/// </summary>
		/// <param name="fd">
		/// The handle ID to the open serial port.
		/// </param>
		/// <returns>
		/// The integer code representing the ASCII character.
		/// </returns>
		/// <remarks>
		/// 0 is a valid character and this function will timeout after 10
		/// seconds if no data could be read.
		/// </remarks>
		[DllImport("libwiringPi.so", EntryPoint = "setGetchar")]
		internal static extern Int32 serialGetChar(Int32 fd);
		#endregion
	}
}

