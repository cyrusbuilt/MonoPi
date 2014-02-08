//
//  IDS1302.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.RTC
{
	/// <summary>
	/// A Dallas Semiconductor DS1302 Real-Time Clock interface.
	/// </summary>
	public interface IDS1302 : IDisposable
	{
		/// <summary>
		/// Gets the pin being used for clock.
		/// </summary>
		IRaspiGpio Clock { get; }

		/// <summary>
		/// Gets the GPIO pin being used for data.
		/// </summary>
		IRaspiGpio Data { get; }

		/// <summary>
		/// Gets the GPIO pin being used for chip-select.
		/// </summary>
		IRaspiGpio ChipSelect { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		Boolean IsDisposed { get; }

		/// <summary>
		/// Reads a value from an RTC register or RAM location.
		/// </summary>
		/// <returns>
		/// The value read.
		/// </returns>
		/// <param name="register">
		/// The register or RAM location to read.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		UInt32 ReadRTC(Int32 register);

		/// <summary>
		/// Writes a value to an RTC register or RAM location.
		/// </summary>
		/// <param name="register">
		/// The register or RAM location to write to.
		/// </param>
		/// <param name="data">
		/// The data value to write.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void WriteRTC(Int32 register, UInt32 data);

		/// <summary>
		/// Writes a value to the specified RAM address.
		/// </summary>
		/// <param name="address">
		/// The RAM address to write to.
		/// </param>
		/// <param name="data">
		/// The value to write.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void WriteRAM(Int32 address, UInt32 data);

		/// <summary>
		/// Reads the clock.
		/// </summary>
		/// <returns>
		/// The 8-byte buffer containing the clock data.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		Int32[] ReadClock();

		/// <summary>
		/// Writes to the clock.
		/// </summary>
		/// <param name="data">
		/// The byte buffer to write to the clock.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="data"/> is not 8 bytes in length.
		/// </exception>
		void WriteClock(Int32[] data);
	}
}

