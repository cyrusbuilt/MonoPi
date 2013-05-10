//
//  I2CBus.cs
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
using Mono.Unix;
using Mono.Unix.Native;

namespace CyrusBuilt.MonoPi.I2C
{
	/// <summary>
	/// An I2C bus implementation for the Raspberry Pi. Derived from the RPi.I2C.Net
	/// library by mshmelev at https://github.com/mshmelev/RPi.I2C.Net. As such, this
	/// class (and the required methods in <see cref="UnsafeNativeMethods"/>) is
	/// dependent on the underlying LibNativeI2C native library which must be compiled
	/// and included with this library.
	/// </summary>
	public class I2CBus : II2CBus, IDisposable
	{
		#region Fields
		private Int32 _busHandle = 0;
		private String _busPath = String.Empty;
		private Boolean _isDisposed = false;
		private Boolean _isOpen = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/>
		/// class with the path the I2C bus.
		/// </summary>
		/// <param name="boardRev">
		/// Specifies the revision of the RPi board in use. This
		/// used to determine the path to the system file associated
		/// with the i2c bus.
		/// </param>
		public I2CBus(BoardRevision boardRev) {
			if (boardRev == BoardRevision.Rev1) {
				this._busPath = "/dev/i2c-0";
			}
			else {
				this._busPath = "/dev/i2c-1";
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/>
		/// object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources as well as unmanaged.
		/// </param>
		protected virtual void Dispose(Boolean disposing) {
			if (this._isDisposed) {
				return;
			}

			if (disposing) {
				this._busPath = null;
			}

			if (this._isOpen) {
				UnsafeNativeMethods.I2CCloseBus(this._busHandle);
				this._isOpen = false;
				this._busHandle = 0;
			}

			this._isDisposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/> is reclaimed by garbage collection.
		/// </summary>
		~I2CBus() {
			this.Dispose(false);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/>
		/// object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release
		/// all references to the <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.I2C.I2CBus"/> was occupying.
		/// </remarks>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets a value indicating whether the connection is open.
		/// </summary>
		public Boolean IsOpen {
			get { return this._isOpen; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Open a connection to the I2C bus.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to open the bus connection.
		/// </exception>
		public void Open() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.I2C.I2CBus");
			}

			if (this._isOpen) {
				return;
			}

			Int32 result = UnsafeNativeMethods.I2COpenBus(this._busPath);
			if (result < 0) {
				String errDesc = UnixMarshal.GetErrorDescription(Stdlib.GetLastError());
				throw new IOException("Error opening bus '" + this._busPath + "': " + errDesc);
			}

			this._busHandle = result;
			this._isOpen = true;
		}

		/// <summary>
		/// Gets an open I2C connection instance.
		/// </summary>
		/// <param name="boardRev">
		/// Specifies the revision of the RPi board in use. This
		/// used to determine the path to the system file associated
		/// with the i2c bus.
		/// </param>
		/// <returns>
		/// An open I2C connection instance.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to open the bus connection.
		/// </exception>
		public static I2CBus Open(BoardRevision boardRev) {
			I2CBus bus = new I2CBus(boardRev);
			bus.Open();
			return bus;
		}

		/// <summary>
		/// Closes the connection.
		/// </summary>
		public void Close() {
			if (this._isDisposed) {
				return;
			}

			if (this._isOpen) {
				UnsafeNativeMethods.I2CCloseBus(this._busHandle);
				this._isOpen = false;
				this._busHandle = 0;
			}
		}

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
		/// <paramref name="bytes"/> cannot be greater than 3 elements in length.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void WriteBytes(Int32 address, Byte[] bytes) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.I2C.I2CBus");
			}

			if (!this._isOpen) {
				throw new InvalidOperationException("No open connection to write to.");
			}

			if (bytes.Length > 3) {
				throw new ArgumentException("Cannot write more than 3 bytes at a time.");
			}

			Int32 result = UnsafeNativeMethods.I2CWriteBytes(this._busHandle, address, bytes, bytes.Length);
			if (result == -1) {
				String errDesc = UnixMarshal.GetErrorDescription(Stdlib.GetLastError());
				throw new IOException("Error accessing address '" + address.ToString() + "': " + errDesc);
			}

			if (result == -2) {
				throw new IOException("Error writing to address '" + address.ToString() + "': I2C transaction failed.");
			}
		}

		/// <summary>
		/// Writes a single byte to the specified device address.
		/// </summary>
		/// <param name="address">
		/// The address of the target device.
		/// </param>
		/// <param name="b">
		/// The byte to write.
		/// </param>
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
		public void WriteByte(Int32 address, Byte b) {
			Byte[] bytes = new Byte[1];
			bytes[0] = b;
			this.WriteBytes(address, bytes);
		}

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
		public void WriteCommand(Int32 address, Byte command, Byte data) {
			Byte[] bytes = new Byte[2];
			bytes[0] = command;
			bytes[1] = data;
			this.WriteBytes(address, bytes);
		}

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
		public void WriteCommand(Int32 address, Byte command, Byte data1, Byte data2) {
			Byte[] bytes = new Byte[3];
			bytes[0] = command;
			bytes[1] = data1;
			bytes[2] = data2;
			this.WriteBytes(address, bytes);
		}

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
		public void WriteCommand(Int32 address, Byte command, ushort data) {
			Byte[] bytes = new Byte[3];
			bytes[0] = command;
			bytes[1] = (Byte)(data & 0xff);
			bytes[2] = (Byte)(data >> 8);
			this.WriteBytes(address, bytes);
		}

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
		public Byte[] ReadBytes(Int32 address, Int32 count) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.I2C.I2CBus");
			}
			
			if (!this._isOpen) {
				throw new InvalidOperationException("No open connection to write to.");
			}

			Byte[] buffer = new Byte[count];
			Int32 result = UnsafeNativeMethods.I2CReadBytes(this._busHandle, address, buffer, buffer.Length);
			if (result == -1) {
				String errDesc = UnixMarshal.GetErrorDescription(Stdlib.GetLastError());
				throw new IOException("Error accessing address at '" + address.ToString() + "': " + errDesc);
			}

			if (result <= 0) {
				throw new IOException("Error reading from address '" + address.ToString() + "': I2C transaction failed.");
			}

			if (result < count) {
				Array.Resize(ref buffer, result);
			}
			return buffer;
		}
		#endregion
	}
}

