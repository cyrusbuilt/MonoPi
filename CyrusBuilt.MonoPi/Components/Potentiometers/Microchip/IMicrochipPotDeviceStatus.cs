//
//  IMicrochipPotDeviceStatus.cs
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
	/// The device-status concerning the channel an instance
	/// of Microchip potentiometer is configured for.
	/// </summary>
	public interface IMicrochipPotDeviceStatus
	{
		/// <summary>
		/// Gets whether or not the device is currently writing to EEPROM.
		/// </summary>
		/// <value>
		/// <c>true</c> if the device is writing to EEPROM; otherwise, <c>false</c>.
		/// </value>
		Boolean IsEepromWriteActive { get; }

		/// <summary>
		/// Gets a value indicating whether the EEPROM is write protected.
		/// </summary>
		/// <value>
		/// <c>true</c> if the EEPROM is write protected; otherwise, <c>false</c>.
		/// </value>
		Boolean IsEepromWriteProtected { get; }

		/// <summary>
		/// Gets the channel the wiper-lock-active status is for.
		/// </summary>
		/// <value>
		/// The wiper lock channel.
		/// </value>
		MicrochipPotChannel WiperLockChannel { get; }

		/// <summary>
		/// Gets whether or not the wiper's lock is active.
		/// </summary>
		/// <value>
		/// <c>true</c> if the wiper's lock is active; otherwise, <c>false</c>.
		/// </value>
		Boolean IsWiperLockActive { get; }
	}
}

