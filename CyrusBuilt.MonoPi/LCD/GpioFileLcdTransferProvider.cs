//
//  GpioFileLcdTransferProvider.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.LCD
{
	/// <summary>
	/// Raspberry Pi GPIO (via filesystem) provider for the Micro Liquid Crystal library.
	/// </summary>
	public class GpioFileLcdTransferProvider : ILcdTransferProvider
	{
		#region Fields
		private readonly GpioFile _registerSelectPort = null;
		private readonly GpioFile _readWritePort = null;
		private readonly GpioFile _enablePort = null;
		private readonly GpioFile[] _dataPorts = { };
		private readonly Boolean _fourBitMode = false;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// class with the mode, register select pin, read/write pin, enable
		/// pin, and data lines.
		/// </summary>
		/// <param name="fourBitMode">
		/// If set to true, then use 4 bit mode instead of 8 bit.
		/// </param>
		/// <param name="rs">
		/// The number of the CPU pin that is connected to the RS (Register Select)
		/// pin on the LCD.
		/// </param>
		/// <param name="rw">
		/// The number of the CPU pin that is connected to the RW (Read/Write)
		/// pin on the LCD.
		/// </param>
		/// <param name="enable">
		/// The number of the CPU pin that is connected to the enable pin on
		/// the LCD.
		/// </param>
		/// <param name="d0">
		/// Data line 0.
		/// </param>
		/// <param name="d1">
		/// Data line 1.
		/// </param>
		/// <param name="d2">
		/// Data line 2.
		/// </param>
		/// <param name="d3">
		/// Data line 3.
		/// </param>
		/// <param name="d4">
		/// Data line 4.
		/// </param>
		/// <param name="d5">
		/// Data line 5.
		/// </param>
		/// <param name="d6">
		/// Data line 6.
		/// </param>
		/// <param name="d7">
		/// Data line 7.
		/// </param>
		/// <remarks>
		/// The display can be controlled using 4 or 8 data lines. If the former,
		/// omit the pin numbers for d0 to d3 and leave those lines disconnected.
		/// The RW pin can be tied to ground instead of connected to a pin on the
		/// Arduino; If so, omit it from this constructor's parameters.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// <paramref name="rs"/> and <paramref name="enable"/> cannot be set to
		/// <see cref="GpioPins.GPIO_NONE"/>.
		/// </exception>
		public GpioFileLcdTransferProvider(Boolean fourBitMode, GpioPins rs, GpioPins rw, GpioPins enable,
		                                   GpioPins d0, GpioPins d1, GpioPins d2, GpioPins d3, GpioPins d4,
		                                   GpioPins d5, GpioPins d6, GpioPins d7) {
			this._fourBitMode = fourBitMode;
			if (rs == GpioPins.GPIO_NONE) {
				throw new ArgumentException("rs");
			}

			this._registerSelectPort = new GpioFile(rs);

			// We can save 1 pin by not using RW. Indicate this by passing GpioPins.GPIO_NONE
			// instead of pin #.
			if (rw != GpioPins.GPIO_NONE) {
				this._readWritePort = new GpioFile(rw);
			}

			if (enable == GpioPins.GPIO_NONE) {
				throw new ArgumentException("enable");
			}

			this._enablePort = new GpioFile(enable);
			GpioPins[] dataPins = { d0, d1, d2, d3, d4, d5, d6, d7 };
			this._dataPorts = new GpioFile[8];
			for (Int32 i = 0; i < 8; i++) {
				if (dataPins[i] != GpioPins.GPIO_NONE) {
					this._dataPorts[i] = new GpioFile(dataPins[i]);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// class with the register select pin, read/write pin, enable
		/// pin, and data lines.
		/// </summary>
		/// <param name="rs">
		/// The number of the CPU pin that is connected to the RS (Register Select)
		/// pin on the LCD.
		/// </param>
		/// <param name="rw">
		/// The number of the CPU pin that is connected to the RW (Read/Write)
		/// pin on the LCD.
		/// </param>
		/// <param name="enable">
		/// The number of the CPU pin that is connected to the enable pin on
		/// the LCD.
		/// </param>
		/// <param name="d0">
		/// Data line 0.
		/// </param>
		/// <param name="d1">
		/// Data line 1.
		/// </param>
		/// <param name="d2">
		/// Data line 2.
		/// </param>
		/// <param name="d3">
		/// Data line 3.
		/// </param>
		/// <param name="d4">
		/// Data line 4.
		/// </param>
		/// <param name="d5">
		/// Data line 5.
		/// </param>
		/// <param name="d6">
		/// Data line 6.
		/// </param>
		/// <param name="d7">
		/// Data line 7.
		/// </param>
		/// <remarks>
		/// The display can be controlled using 4 or 8 data lines. If the former,
		/// omit the pin numbers for d0 to d3 and leave those lines disconnected.
		/// The RW pin can be tied to ground instead of connected to a pin on the
		/// Arduino; If so, omit it from this constructor's parameters.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// <paramref name="rs"/> and <paramref name="enable"/> cannot be set to
		/// <see cref="GpioPins.GPIO_NONE"/>.
		/// </exception>
		public GpioFileLcdTransferProvider(GpioPins rs, GpioPins rw, GpioPins enable, GpioPins d0, GpioPins d1,
		                                   GpioPins d2, GpioPins d3, GpioPins d4, GpioPins d5, GpioPins d6, GpioPins d7)
			: this(true, rs, rw, enable, d0, d1, d2, d3, d4, d5, d6, d7) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// class with the register select pin, enable pin, and data lines.
		/// </summary>
		/// <param name="rs">
		/// The number of the CPU pin that is connected to the RS (Register Select)
		/// pin on the LCD.
		/// </param>
		/// <param name="enable">
		/// The number of the CPU pin that is connected to the enable pin on
		/// the LCD.
		/// </param>
		/// <param name="d0">
		/// Data line 0.
		/// </param>
		/// <param name="d1">
		/// Data line 1.
		/// </param>
		/// <param name="d2">
		/// Data line 2.
		/// </param>
		/// <param name="d3">
		/// Data line 3.
		/// </param>
		/// <param name="d4">
		/// Data line 4.
		/// </param>
		/// <param name="d5">
		/// Data line 5.
		/// </param>
		/// <param name="d6">
		/// Data line 6.
		/// </param>
		/// <param name="d7">
		/// Data line 7.
		/// </param>
		/// <remarks>
		/// The display can be controlled using 4 or 8 data lines. If the former,
		/// omit the pin numbers for d0 to d3 and leave those lines disconnected.
		/// The RW pin can be tied to ground instead of connected to a pin on the
		/// Arduino; If so, omit it from this constructor's parameters.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// <paramref name="rs"/> and <paramref name="enable"/> cannot be set to
		/// <see cref="GpioPins.GPIO_NONE"/>.
		/// </exception>
		public GpioFileLcdTransferProvider(GpioPins rs, GpioPins enable, GpioPins d0, GpioPins d1, GpioPins d2,
		                                   GpioPins d3, GpioPins d4, GpioPins d5, GpioPins d6, GpioPins d7)
			: this(true, rs, GpioPins.GPIO_NONE, enable, d0, d1, d2, d3, d4, d5, d6, d7) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// class with the mode, register select pin, read/write pin, enable
		/// pin, and data lines.
		/// </summary>
		/// <param name="rs">
		/// The number of the CPU pin that is connected to the RS (Register Select)
		/// pin on the LCD.
		/// </param>
		/// <param name="rw">
		/// The number of the CPU pin that is connected to the RW (Read/Write)
		/// pin on the LCD.
		/// </param>
		/// <param name="enable">
		/// The number of the CPU pin that is connected to the enable pin on
		/// the LCD.
		/// </param>
		/// <param name="d4">
		/// Data line 4.
		/// </param>
		/// <param name="d5">
		/// Data line 5.
		/// </param>
		/// <param name="d6">
		/// Data line 6.
		/// </param>
		/// <param name="d7">
		/// Data line 7.
		/// </param>
		/// <remarks>
		/// The display can be controlled using 4 or 8 data lines. If the former,
		/// omit the pin numbers for d0 to d3 and leave those lines disconnected.
		/// The RW pin can be tied to ground instead of connected to a pin on the
		/// Arduino; If so, omit it from this constructor's parameters.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// <paramref name="rs"/> and <paramref name="enable"/> cannot be set to
		/// <see cref="GpioPins.GPIO_NONE"/>.
		/// </exception>
		public GpioFileLcdTransferProvider(GpioPins rs, GpioPins rw, GpioPins enable, GpioPins d4, GpioPins d5, GpioPins d6, GpioPins d7)
			: this(false, rs, rw, enable, GpioPins.GPIO_NONE, GpioPins.GPIO_NONE, GpioPins.GPIO_NONE, GpioPins.GPIO_NONE,
			       d4, d5, d6, d7) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// class with the mode, register select pin, read/write pin, enable
		/// pin, and data lines.
		/// </summary>
		/// <param name="rs">
		/// The number of the CPU pin that is connected to the RS (Register Select)
		/// pin on the LCD.
		/// </param>
		/// <param name="enable">
		/// The number of the CPU pin that is connected to the enable pin on
		/// the LCD.
		/// </param>
		/// <param name="d4">
		/// Data line 4.
		/// </param>
		/// <param name="d5">
		/// Data line 5.
		/// </param>
		/// <param name="d6">
		/// Data line 6.
		/// </param>
		/// <param name="d7">
		/// Data line 7.
		/// </param>
		/// <remarks>
		/// The display can be controlled using 4 or 8 data lines. If the former,
		/// omit the pin numbers for d0 to d3 and leave those lines disconnected.
		/// The RW pin can be tied to ground instead of connected to a pin on the
		/// Arduino; If so, omit it from this constructor's parameters.
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// <paramref name="rs"/> and <paramref name="enable"/> cannot be set to
		/// <see cref="GpioPins.GPIO_NONE"/>.
		/// </exception>
		public GpioFileLcdTransferProvider(GpioPins rs, GpioPins enable, GpioPins d4, GpioPins d5, GpioPins d6, GpioPins d7)
			: this(false, rs, GpioPins.GPIO_NONE, enable, GpioPins.GPIO_NONE, GpioPins.GPIO_NONE, GpioPins.GPIO_NONE,
			       GpioPins.GPIO_NONE, d4, d5, d6, d7) {
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
		/// Gets a value indicating whether this instance is in 4-bit mode.
		/// </summary>
		public Boolean FourBitMode {
			get { return this._fourBitMode; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Pulses the enable pin.
		/// </summary>
		private void PulseEnable() {
			this._enablePort.Write(false);
			this._enablePort.Write(true);   // enable pulse must be > 450ns.
			this._enablePort.Write(false);  // commands need > 37us to settle.
		}

		/// <summary>
		/// Writes the command or data in 4bit mode (the last 4 data lines).
		/// </summary>
		/// <param name="value">
		/// The command or data to write.
		/// </param>
		private void Write4Bits(Byte value) {
			for (Int32 i = 0; i < 4; i++) {
				this._dataPorts[i + 4].Write(((value >> i) & 0x01) == 0x01);
			}
			this.PulseEnable();
		}

		/// <summary>
		/// Writes the command or data in 8bit mode (all 8 data lines).
		/// </summary>
		/// <param name="value">
		/// The command or data to write.
		/// </param>
		private void Write8Bits(Byte value) {
			for (Int32 i = 0; i < 8; i++) {
				this._dataPorts[i].Write(((value >> i) & 0x01) == 0x01);
			}
			this.PulseEnable();
		}

		/// <summary>
		/// Write either command or data, with automatic 4/8-bit selection.
		/// </summary>
		/// <param name="value">
		/// The data or command to send.
		/// </param>
		/// <param name="mode">
		/// Mode for register-select pin (true = on, false = off).
		/// </param>
		/// <param name="backlight">
		/// Turns the backlight on or off.
		/// </param>
		public void Send(Byte value, Boolean mode, Boolean backlight) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("GpioFileLcdTransferProvider");
			}

			// TODO Set backlight

			this._registerSelectPort.Write(mode);

			// If there is a RW pin indicated, set it low to write.
			if (this._readWritePort != null) {
				this._readWritePort.Write(false);
			}

			if (this._fourBitMode) {
				this.Write4Bits((Byte)(value >> 4));
				this.Write4Bits(value);
			}
			else {
				this.Write8Bits(value);
			}
		}
		#endregion

		#region Destructors
		/// <summary>
		/// Dispose this instance and release managed resources.
		/// </summary>
		/// <param name="disposing">
		/// If set to <c>true</c> disposing and not finalizing.
		/// </param>
		private void Dispose(Boolean disposing) {
			if (this._isDisposed) {
				return;
			}

			if (this._registerSelectPort != null) {
				this._registerSelectPort.Dispose();
			}

			if (this._readWritePort != null) {
				this._readWritePort.Dispose();
			}

			if (this._enablePort != null) {
				this._enablePort.Dispose();
			}

			if ((this._dataPorts != null) && (this._dataPorts.Length > 0)) {
				for (Int32 i = 0; i < 8; i++) {
					if (this._dataPorts[i] != null) {
						this._dataPorts[i].Dispose();
					}
				}
				Array.Clear(this._dataPorts, 0, this._dataPorts.Length);
			}

			if (disposing) {
				GC.SuppressFinalize(this);
			}
			this._isDisposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations
		/// before the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// is reclaimed by garbage collection.
		/// </summary>
		~GpioFileLcdTransferProvider() {
			this.Dispose(false);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>.
		/// The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/> in
		/// an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/>
		/// so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.LCD.GpioFileLcdTransferProvider"/> was occupying.
		/// </remarks>
		public void Dispose() {
			this.Dispose(true);
		}
		#endregion
	}
}

