//
//  SimpleSPI.cs
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

namespace CyrusBuilt.MonoPi.SPI
{
	/// <summary>
	/// A simplified SPI data access mechanism. This wraps the wiringPiSPI module in
	/// wiringPi, thus relies on wiringPi.so lib.
	/// </summary>
	public static class SimpleSPI
	{
		private static Boolean _initialized = false;

		/// <summary>
		/// Gets a value indicating whether <see cref="CyrusBuilt.MonoPi.SPI.SimpleSPI"/>
		/// is initialized.
		/// </summary>
		public static Boolean Initialized {
			get { return _initialized; }
		}

		/// <summary>
		/// Open the SPI device and set it up, etc.
		/// </summary>
		/// <param name="channel">
		/// The channel to open.
		/// </param>
		/// <param name="speed">
		/// The speed to negotiate the transfer rate at (ie. 38400).
		/// </param>
		public static Boolean Init(AdcChannels channel, Int32 speed) {
			if (_initialized) {
				return true;
			}

			if ((channel == AdcChannels.Channel0) ||
			    (channel == AdcChannels.Channel1)) {
				_initialized = (UnsafeNativeMethods.wiringPiSPISetup((Int32)channel, speed) == 0);
			}
			else {
				_initialized = false;
			}
			return _initialized;
		}

		/// <summary>
		/// Gets the file descriptor for the given channel.
		/// </summary>
		/// <returns>
		/// If successful, the file descriptor; Otherwise, -1.
		/// </returns>
		/// <param name="channel">
		/// The channel to get the descriptor for.
		/// </param>
		public static Int32 GetFileDescriptor(AdcChannels channel) {
			Int32 descriptor = -1;
			if (_initialized) {
				if ((channel == AdcChannels.Channel0) ||
				    (channel == AdcChannels.Channel1)) {
					descriptor = UnsafeNativeMethods.wiringPiSPIGetFd((Int32)channel);
				}
			}
			return descriptor;
		}

		/// <summary>
		/// Write and read a block of data over the SPI bus. This is
		/// a full duplex operation.
		/// </summary>
		/// <returns>
		/// If successful, the data sent back in response; Otherwise,
		/// an empty string.
		/// </returns>
		/// <param name="channel">
		/// The channel to transfer on.
		/// </param>
		/// <param name="data">
		/// The data to transfer.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// You initialize the SPI by calling Init() first.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// This module only supports a 2-channel device. Valid channels are
		/// <see cref="AdcChannels.Channel0"/> and <see cref="AdcChannels.Channel1"/>.
		/// </exception>
		public static String FullDuplexTransfer(AdcChannels channel, String data) {
			if (!_initialized) {
				throw new InvalidOperationException("SPI not yet initialized.");
			}

			if ((channel != AdcChannels.Channel0) &&
				(channel != AdcChannels.Channel1)) {
				throw new ArgumentOutOfRangeException("Channel must be either either 0 or 1.");
			}

			Char[] buffer = data.ToCharArray();
			if (UnsafeNativeMethods.wiringPiSPIDataRW((Int32)channel, buffer, buffer.Length) == -1) {
				return String.Empty;
			}
			return new String(buffer);
		}
	}
}

