//
//  Rs232SerialPort.cs
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

namespace CyrusBuilt.MonoPi.IO.Serial
{
	/// <summary>
	/// Provides access to the RS232 UARTs (serial port) on the Raspberry Pi.
	/// </summary>
	public class Rs232SerialPort : IDisposable
	{
		#region Fields
		private String _device = "/dev/ttyAMA0";
		private Int32 _id = 0;
		private BaudRates _baud = BaudRates.Baud9600;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/>
		/// class. This is the default constructor.
		/// </summary>
		public Rs232SerialPort() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/>
		/// class with the device to open.
		/// </summary>
		/// <param name="device">
		/// The device path to open (default is "/dev/ttyAMA0").
		/// </param>
		public Rs232SerialPort(String device) {
			this._device = device;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/>
		/// class with the BAUD rate to negotiate.
		/// </summary>
		/// <param name="baud">
		/// The BAUD rate to negotiate (default is <see cref="BaudRates.Baud9600"/>,
		/// which is 9600 BAUD).
		/// </param>
		public Rs232SerialPort(BaudRates baud) {
			this._baud = baud;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/>
		/// class with the device path and the BAUD rate to negotiate.
		/// </summary>
		/// <param name="device">
		/// The device path to open (default is "/dev/ttyAMA0").
		/// </param>
		/// <param name='baud'>
		/// The BAUD rate to negotiate (default is <see cref="BaudRates.Baud9600"/>,
		/// which is 9600 BAUD).
		/// </param>
		public Rs232SerialPort(String device, BaudRates baud) {
			this._device = device;
			this._baud = baud;
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
		/// Gets the port handle.
		/// </summary>
		public Int32 PortHandle {
			get { return this._id; }
		}

		/// <summary>
		/// Gets a value indicating whether or not the port has been opened.
		/// </summary>
		public Boolean IsOpen {
			get { return this._id > 0; }
		}

		/// <summary>
		/// Gets or sets the device path (default is "/dev/ttyAMA0").
		/// </summary>
		public String Device {
			get { return this._device; }
			set { this._device = value; }
		}

		/// <summary>
		/// Gets or sets the BAUD rate (default is <see cref="BaudRates.Baud9600"/>.
		/// </summary>
		public BaudRates Baud {
			get { return this._baud; }
			set { this._baud = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Opens the serial port.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The port is already open.
		/// </exception>
		public void Open() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("Rs232SerialPort");
			}

			if (this.IsOpen) {
				throw new InvalidOperationException("Port already open.");
			}
			this._id = UnsafeNativeMethods.serialOpen(this._device, (Int32)this._baud);
		}

		/// <summary>
		/// Open the specified device with the specified BAUD rate.
		/// </summary>
		/// <param name="device">
		/// The path to the serial port device to open.
		/// </param>
		/// <param name="baud">
		/// The BAUD rate to negotiate.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// 
		/// </exception>
		public void Open(String device, BaudRates baud) {
			this._device = device;
			this._baud = baud;
			this.Open();
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void Close() {
			UnsafeNativeMethods.serialClose(this._id);
			this._id = 0;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.Rs232SerialPort"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			lock (this) {
				if (this._id > 0) {
					UnsafeNativeMethods.serialFlush(this._id);
					UnsafeNativeMethods.serialClose(this._id);
					this._id = 0;
				}
			}
			this._isDisposed = true;
		}

		/// <summary>
		/// Sends a single character to the port.
		/// </summary>
		/// <param name="c">
		/// The character to send to the port.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The port is closed.
		/// </exception>
		public void PutChar(Char c) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("Rs232SerialPort");
			}

			if (!this.IsOpen) {
				throw new InvalidOperationException("Cannot send char to a closed port.");
			}
			UnsafeNativeMethods.serialPutChar(this._id, Convert.ToByte(c));
		}

		/// <summary>
		/// Sends a string to the port.
		/// </summary>
		/// <param name="s">
		/// The string to send to the port.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The port is closed.
		/// </exception>
		public void PutString(String s) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("Rs232SerialPort");
			}
			
			if (!this.IsOpen) {
				throw new InvalidOperationException("Cannot send char to a closed port.");
			}
			UnsafeNativeMethods.serialPuts(this._id, s);
		}

		/// <summary>
		/// Gets the bytes available to be read.
		/// </summary>
		/// <returns>
		/// The bytes available.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		public Int32 GetBytesAvailable() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("Rs232SerialPort");
			}
			
			if (!this.IsOpen) {
				return 0;
			}
			return UnsafeNativeMethods.serialDataAvail(this._id);
		}

		/// <summary>
		/// Gets a single character from the serial device.
		/// </summary>
		/// <returns>
		/// The character.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The port is not open.
		/// </exception>
		public Char GetCharacter() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("Rs232SerialPort");
			}
			
			if (!this.IsOpen) {
				throw new InvalidOperationException("Port not open.");
			}
			return (Char)UnsafeNativeMethods.serialGetChar(this._id);
		}
		#endregion
	}
}

