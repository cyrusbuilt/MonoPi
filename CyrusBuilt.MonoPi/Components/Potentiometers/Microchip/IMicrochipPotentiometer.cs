//
//  IMicrochipPotentiometer.cs
//
//  Author:
//       Chris.Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2015 Chris.Brunner
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

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// An MCP45XX or MCP46XX IC device abstraction interface.
	/// </summary>
	public interface IMicrochipPotentiometer : IPotentiometer
	{
		#region Properties
		/// <summary>
		/// Gets the channel this device is configured for..
		/// </summary>
		MicrochipPotChannel Channel { get; }

		/// <summary>
		/// Gets whether or not the device is capable of non-volatile wipers.
		/// </summary>
		/// <value>
		/// <c>true</c> if the device is capable of non-volatile wipers;
		/// otherwise, <c>false</c>.
		/// </value>
		Boolean IsNonVolatileWiperCapable { get; }

		/// <summary>
		/// Gets the way non-volatile reads and/or writes are done.
		/// </summary>
		MicrochipPotNonVolatileMode NonVolatileMode { get; }

		/// <summary>
		/// Gets the channels that are supported by the underlying device.
		/// </summary>
		MicrochipPotChannel[] SupportedChannels { get; }

		/// <summary>
		/// Gets the device and wiper status.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		IMicrochipPotDeviceStatus DeviceStatus { get; }

		/// <summary>
		/// Gets or sets the current terminal configuration.
		/// </summary>
		/// <value>
		/// The configuration to set.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		MCPTerminalConfiguration TerminalConfiguration { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Updates the cache to the wiper's value.
		/// </summary>
		/// <returns>
		/// The wiper's current value.
		/// </returns>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		Int32 UpdateCacheFromDevice();

		/// <summary>
		/// Determines whether or not the specified channel is supported by
		/// the underlying device.
		/// </summary>
		/// <param name="channel">
		/// The channel to check.
		/// </param>
		/// <returns>
		/// <c>true</c> if the channel is supported; otherwise, <c>false</c>.
		/// </returns>
		Boolean IsChannelSupported(MicrochipPotChannel channel);

		/// <summary>
		/// Enables or disables the wiper lock.
		/// </summary>
		/// <param name="enabled">
		/// Set true to enable.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		void SetWiperLock(Boolean enabled);

		/// <summary>
		/// Enables or disables write-protection for devices capable of
		/// non-volatile memory. Enabling write-protection does not only
		/// protect non-volatile wipers, it also protects any other
		/// non-volatile information stored (i.e. wiper-locks).
		/// </summary>
		/// <param name="enabled">
		/// Set true to enable.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		void SetWriteProtection(Boolean enabled);
		#endregion
	}
}

