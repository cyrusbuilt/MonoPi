//
//  GertboardDevice.cs
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

namespace CyrusBuilt.MonoPi.Gertboard
{
	/// <summary>
	/// Provides read/write access to the MCP4802 Analog-to-Digital
	/// converter via SPI on the Gertboard. This wraps the Gertboard
	/// support found in wiringPi (https://github.com/WiringPi/WiringPi).
	/// </summary>
	public static class GertboardDevice
	{
		private static Boolean _initialized = false;

		/// <summary>
		/// Initializes SPI communication with the device.
		/// </summary>
		/// <returns>
		/// true if initialization was successful or if already
		/// initialized; Otherwise false.
		/// </returns>
		public static Boolean Initialize() {
			if (_initialized) {
				return true;
			}
			_initialized = (UnsafeNativeMethods.gertboardSPISetup() == 0);
			return _initialized;
		}

		/// <summary>
		/// Writes an 8-bit data value to ADC on the Gertboard.
		/// </summary>
		/// <param name="channel">
		/// The channel to write to.
		/// </param>
		/// <param name="value">
		/// The value to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Not yet initialized. Call <see cref="Initialize()"/> before use.
		/// </exception>
		public static void AnalogWrite(GertboardAdcChannels channel, Int32 value) {
			if (!_initialized) {
				throw new InvalidOperationException("You must call Initialize() before use.");
			}
			UnsafeNativeMethods.gertboardAnalogWrite((Int32)channel, value);
		}

		/// <summary>
		/// Reads an analog value from the ADC on the Gertboard.
		/// </summary>
		/// <returns>
		/// The analog value retrieved.
		/// </returns>
		/// <param name="channel">
		/// The channel to read from.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Not yet initialized. Call <see cref="Initialize()"/> before use.
		/// </exception>
		public static Int32 AnalogRead(GertboardAdcChannels channel) {
			if (!_initialized) {
				throw new InvalidOperationException("You must call Initialize() before use.");
			}
			return UnsafeNativeMethods.gertboardAnalogRead((Int32)channel);
		}
	}
}

