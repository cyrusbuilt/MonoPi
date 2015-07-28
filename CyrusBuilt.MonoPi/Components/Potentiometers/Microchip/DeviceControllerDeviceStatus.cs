//
//  DeviceControllerDeviceStatus.cs
//
//  Author:
//       Chris.Brunner <>
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
	/// The device's status.
	/// </summary>
	public class DeviceControllerDeviceStatus
	{
		#region Fields
		private Boolean _eepromWriteActive = false;
		private Boolean _eepromWriteProtected = false;
		private Boolean _channelALocked = false;
		private Boolean _channelBLocked = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerDeviceStatus"/>
		/// class with whether or not the EEPROM is actively writing, whether or not
		/// the EEPROM is write-protected, whether or not channel A is locked, and
		/// whether or not channel B locked.
		/// </summary>
		/// <param name="writeActive">
		/// Set true if actively writing the EEPROM.
		/// </param>
		/// <param name="writeProtected">
		/// Set true if the EEPROM is write-protected.
		/// </param>
		/// <param name="chanALock">
		/// Set true if channel A is locked.
		/// </param>
		/// <param name="chanBLock">
		/// Set true if channel B is locked.
		/// </param>
		public DeviceControllerDeviceStatus(Boolean writeActive, Boolean writeProtected,
											Boolean chanALock, Boolean chanBLock) {
			this._eepromWriteActive = writeActive;
			this._eepromWriteProtected = writeProtected;
			this._channelALocked = chanALock;
			this._channelBLocked = chanBLock;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether nor not the EEPROM is actively writing.
		/// </summary>
		/// <value>
		/// <c>true</c> if the EEPROM is actively writing; otherwise, <c>false</c>.
		/// </value>
		public Boolean EepromWriteActive {
			get { return this._eepromWriteActive; }
		}

		/// <summary>
		/// Gets a value indicating whether or not the EEPROM is write-protected.
		/// </summary>
		/// <value>
		/// <c>true</c> write-protected; otherwise, <c>false</c>.
		/// </value>
		public Boolean EepromWriteProtected {
			get { return this._eepromWriteProtected; }
		}

		/// <summary>
		/// Gets a value indicating whether or not channel A is locked.
		/// </summary>
		/// <value>
		/// <c>true</c> if channel A locked; otherwise, <c>false</c>.
		/// </value>
		public Boolean ChannelALocked {
			get { return this._channelALocked; }
		}

		/// <summary>
		/// Gets a value indicating whether or not channel B is locked.
		/// </summary>
		/// <value>
		/// <c>true</c> if channel B locked; otherwise, <c>false</c>.
		/// </value>
		public Boolean ChannelBLocked {
			get { return this._channelBLocked; }
		}
		#endregion
	}
}

