//
//  MicrochipPotDeviceStatus.cs
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
	/// Microchip MCP 45XX and 46XX potentiometer IC device status.
	/// </summary>
	public class MicrochipPotDeviceStatus : IMicrochipPotDeviceStatus
	{
		#region Fields
		private MicrochipPotChannel _wiperLockChannel = MicrochipPotChannel.None;
		private Boolean _eepromWriteActive = false;
		private Boolean _eepromWriteProtected = false;
		private Boolean _wiperLockActive = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceStatus"/>
		/// class with the wiper-lock channel and flags to indicate if currently
		/// writing to the EEPROM, whether or not the EEPROM is write-protected,
		/// and whether or not the wiper is locked.
		/// </summary>
		/// <param name="chan">
		/// The wiper-lock channel.
		/// </param>
		/// <param name="writeActive">
		/// Set true if currently writing the EEPROM.
		/// </param>
		/// <param name="writeProtected">
		/// Set true if the EEPROM is write-protected.
		/// </param>
		/// <param name="wiperLocked">
		/// Set true if the wiper is locked.
		/// </param>
		public MicrochipPotDeviceStatus(MicrochipPotChannel chan, Boolean writeActive, Boolean writeProtected, Boolean wiperLocked) {
			this._wiperLockChannel = chan;
			this._eepromWriteActive = writeActive;
			this._eepromWriteProtected = writeProtected;
			this._wiperLockActive = wiperLocked;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets whether or not the device is currently writing to EEPROM.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is eeprom write active; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsEepromWriteActive {
			get { return this._eepromWriteActive; }
		}

		/// <summary>
		/// Gets a value indicating whether the EEPROM is write protected.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is eeprom write protected; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsEepromWriteProtected {
			get { return this._eepromWriteProtected; }
		}

		/// <summary>
		/// Gets the channel the wiper-lock-active status is for.
		/// </summary>
		/// <value>
		/// The wiper lock channel.
		/// </value>
		public MicrochipPotChannel WiperLockChannel {
			get { return this._wiperLockChannel; }
		}

		/// <summary>
		/// Gets whether or not the wiper's lock is active.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is wiper lock active; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsWiperLockActive {
			get { return this._wiperLockActive; }
		}
		#endregion
	}
}

