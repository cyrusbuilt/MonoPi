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
	internal static class UnsafeNativeMethods
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
		public static extern Int32 bcm2835_init();

		/// <summary>
		/// Close the library, deallocating any allocated memory and closing
		/// /dev/mem.
		/// </summary>
		/// <returns>
		/// 1 if successful; Otherwise, 0;
		/// </returns>			
		[DllImport("libbcm2835.so", EntryPoint = "bcm2835_close")]
		public static extern Int32 bcm2835_close();

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
		public static extern void bcm2835_gpio_fsel(uint pin, uint mode);

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
		public static extern void bcm2835_gpio_write(uint pin, uint value);

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
		public static extern uint bcm2835_gpio_lev(uint pin);

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
		public static extern void bcm2835_gpio_set_pud(uint pin, uint pud);
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
		public static extern Int32 serialOpen(String device, Int32 baud);

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
		public static extern void serialClose(Int32 fd);

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
		public static extern void serialPutChar(Int32 fd, byte c);

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
		public static extern void serialPuts(Int32 fd, String s);

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
		public static extern Int32 serialDataAvail(Int32 fd);

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
		public static extern Int32 serialGetChar(Int32 fd);

		/// <summary>
		/// Initializes the SPI bus for communication with the SPI devices on
		/// the gertboard.
		/// </summary>
		/// <returns>
		/// If successful, 0; Otherwise, -1.
		/// </returns>
		[DllImport("libwiringPi.so", EntryPoint = "gertboardSPISetup")]
		public static extern Int32 gertboardSPISetup();

		/// <summary>
		/// Writes an 8-bit data value to the MCP4802 ADC.
		/// </summary>
		/// <param name="chan">
		/// The channel to write to 0 or 1.
		/// </param>
		/// <param name="value">
		/// The value to write.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "gertboardAnalogWrite")]
		public static extern void gertboardAnalogWrite(Int32 chan, Int32 value);

		/// <summary>
		/// Reads the analog value of the specified channel.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="chan">
		/// The channel to read from (0 or 1).
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "gertboardAnalogRead")]
		public static extern Int32 gertboardAnalogRead(Int32 chan);

		/// <summary>
		/// Read a value from an RTC register or RAM location on the chip.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="reg">
		/// The register or RAM location to read.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302rtcRead")]
		public static extern UInt32 ds1302rtcRead(Int32 reg);

		/// <summary>
		/// Writes a value to an RTC register or RAM location on the chip.
		/// </summary>
		/// <param name="reg">
		/// The register or RAM location to write to.
		/// </param>
		/// <param name="data">
		/// The data to write.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302rtcWrite")]
		public static extern void ds1302rtcWrite(Int32 reg, UInt32 data);

		/// <summary>
		/// Reads data from the RTC register.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="addr">
		/// The address of the register to read.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302ramRead")]
		public static extern UInt32 ds1302ramRead(Int32 addr);

		/// <summary>
		/// Writes data to the RTC register.
		/// </summary>
		/// <param name="addr">
		/// The address to read.
		/// </param>
		/// <param name="data">
		/// The data to write.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302ramWrite")]
		public static extern void ds1302ramWrite(Int32 addr, UInt32 data);

		/// <summary>
		/// Reads all 8 bytes from the clock in a single operation.
		/// </summary>
		/// <param name="clockData">
		/// The 8 byte buffer to receive the clock data read.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302clockRead")]
		public static extern void ds1302clockRead(Int32[] clockData);

		/// <summary>
		/// Writes all 8 bytes of the clock in a single operation.
		/// </summary>
		/// <param name="clockData">
		/// The 8 bytes of clock data to write.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302clockWrite")]
		public static extern void ds1302clockWrite(Int32[] clockData);

		/// <summary>
		/// Initializes the chip and remembers the pins being used.
		/// </summary>
		/// <param name="clockPin">
		/// The GPIO pin to use for the clock.
		/// </param>
		/// <param name="dataPin">
		/// The GPIO pin to use for data access.
		/// </param>
		/// <param name="csPin">
		/// The GPIO pin to use for chip select.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "ds1302setup")]
		public static extern void ds1302setup(Int32 clockPin, Int32 dataPin, Int32 csPin);

		/// <summary>
		/// Initialize the SPI interface.
		/// </summary>
		/// <returns>
		/// 0 if successful.
		/// </returns>
		/// <param name="channel">
		/// The channel to communicate on.
		/// </param>
		/// <param name="speed">
		/// The transfer speed to negotiate.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "wiringPiSPISetup")]
		public static extern Int32 wiringPiSPISetup(Int32 channel, Int32 speed);

		/// <summary>
		/// Get file descripter.
		/// </summary>
		/// <returns>
		/// The descripter value.
		/// </returns>
		/// <param name="channel">
		/// The channel to communicate on.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "wiringPiSPIGetFd")]
		public static extern Int32 wiringPiSPIGetFd(Int32 channel);

		/// <summary>
		/// Read and Write data on the specified channel.
		/// </summary>
		/// <returns>
		/// 0 on success; -1 on failure.
		/// </returns>
		/// <param name="channel">
		/// The channel to transfer on.
		/// </param>
		/// <param name="data">
		/// The data buffer used to transfer to the data to be written,
		/// and is also used to recieve the data read.
		/// </param>
		/// <param name="len">
		/// The buffer length.
		/// </param>
		[DllImport("libwiringPi.so", EntryPoint = "wiringPiSPIDataRW")]
		public static extern Int32 wiringPiSPIDataRW(Int32 channel, Char[] data, Int32 len);
		#endregion

		#region I2CNativeLib Imported Methods
		/// <summary>
		/// Opens the I2C bus connection.
		/// </summary>
		/// <returns>
		/// A handle ID to the open connection stream.
		/// </returns>
		/// <param name="busFileName">
		/// The I2C bus file (device) name.
		/// </param>
		[DllImport("libnativei2c.so", EntryPoint = "openBus", SetLastError = true)]
		public static extern Int32 I2COpenBus(String busFileName);

		/// <summary>
		/// Closes the specified I2C bus connection.
		/// </summary>
		/// <returns>
		/// Always returns 0.
		/// </returns>
		/// <param name="busHandle">
		/// The handle ID to an open I2C bus connection.
		/// </param>
		[DllImport("libnativei2c.so", EntryPoint = "closeBus", SetLastError = true)]
		public static extern Int32 I2CCloseBus(Int32 busHandle);

		/// <summary>
		/// Reads bytes from the I2C bus.
		/// </summary>
		/// <returns>
		/// 0 if successful; Otherwise, -1 if <paramref name="address"/> is
		/// inacessible or 0 (or less than 0) if the I2C transaction failed. 
		/// </returns>
		/// <param name="busHandle">
		/// The connection handle ID of an open bus connection.
		/// </param>
		/// <param name="address">
		/// The address of the device on the bus to read from.
		/// </param>
		/// <param name="buffer">
		/// The buffer to receive the bytes read.
		/// </param>
		/// <param name="length">
		/// The number of bytes to read from the bus.
		/// </param>
		[DllImport("libnativei2c.so", EntryPoint = "readBytes", SetLastError = true)]
		public static extern Int32 I2CReadBytes(Int32 busHandle, Int32 address, Byte[] buffer, Int32 length);

		/// <summary>
		/// Writes bytes to the I2C bus.
		/// </summary>
		/// <returns>
		/// 0 if successful; Otherwise -1 if <paramref name="address"/> is
		/// inacessible or -2 if the i2c transaction failed.
		/// </returns>
		/// <param name="busHandle">
		/// The connection handle ID of an open bus connection.
		/// </param>
		/// <param name="address">
		/// The address of the target device on the bus.
		/// </param>
		/// <param name="buffer">
		/// The buffer containing the bytes to write.
		/// </param>
		/// <param name="length">
		/// The number of bytes in the buffer to write.
		/// </param>
		[DllImport("libnativei2c.so", EntryPoint = "writeBytes", SetLastError = true)]
		public static extern Int32 I2CWriteBytes(Int32 busHandle, Int32 address, Byte[] buffer, Int32 length);
		#endregion
	}
}

