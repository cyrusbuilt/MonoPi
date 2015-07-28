//
//  II2CBus.cs
//
//  Author:
//       chris brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 CyrusBuilt
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
using System.IO;

namespace CyrusBuilt.MonoPi.IO.I2C
{
	/// <summary>
	/// Implemented by classes that represent an I2C bus.
	/// </summary>
	public interface II2CBus : IDisposable {
		/// <summary>
		/// Gets a value indicating whether the connection is open.
		/// </summary>
		Boolean IsOpen { get; }

		/// <summary>
		/// Open a connection to the I2C bus.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to open the bus connection.
		/// </exception>
		void Open();

		/// <summary>
		/// Closes the connection.
		/// </summary>
		void Close();

		/// <summary>
		/// Writes a single byte to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="b">
		/// The byte to write.
		/// </param>
		void WriteByte(Int32 address, Byte b);

		/// <summary>
		/// Writes an array of bytes to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="bytes">
		/// The byte array to write.
		/// </param>
		/// <remarks>
		/// Currently, RPi drivers do not allow writing more than 3 bytes at a time.
		/// As such, if an array of greater than 3 bytes is provided, an exception
		/// is thrown.
		/// </remarks>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// You must open a conection to the I2C bus by calling <see cref="Open()"/>
		/// first.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="bytes"/> contains more than 3 bytes.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		void WriteBytes(Int32 address, Byte[] bytes);

		/// <summary>
		/// Writes a command with data to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="command">
		/// The command to send to the device.
		/// </param>
		/// <param name="data">
		/// The data to send with the command.
		/// </param>
		void WriteCommand(Int32 address, Byte command, Byte data);

		/// <summary>
		/// Writes a command with data to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="command">
		/// The command to send to the device.
		/// </param>
		/// <param name="data1">
		/// The data to send as the first parameter.
		/// </param>
		/// <param name="data2">
		/// The data to send as the second parameter.
		/// </param>
		void WriteCommand(Int32 address, Byte command, Byte data1, Byte data2);

		/// <summary>
		/// Writes a command with data to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="command">
		/// The command to send to the device.
		/// </param>
		/// <param name="data">
		/// The data to send with the command.
		/// </param>
		void WriteCommand(Int32 address, Byte command, ushort data);

		/// <summary>
		/// Reads bytes from the device at the specified address.
		/// </summary>
		/// <returns>
		/// The bytes read.
		/// </returns>
		/// <param name="address">
		/// The address of the device to read from.
		/// </param>
		/// <param name="count">
		/// The number of bytes to read.
		/// </param>
		Byte[] ReadBytes(Int32 address, Int32 count);

		/// <summary>
		/// Reads a single byte from the device at the specified address.
		/// </summary>
		/// <param name="address">
		/// The address of the device to read from.
		/// </param>
		/// <returns>
		/// The byte read.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// You must open a conection to the I2C bus by calling <see cref="Open()"/>
		/// first.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		Byte Read(Int32 address);
	}
}

