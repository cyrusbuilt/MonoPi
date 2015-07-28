//
//  DeviceControllerTerminalConfiguration.cs
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
	/// The device's terminal configuration for a certain channel.
	/// </summary>
	public class DeviceControllerTerminalConfiguration
	{
		#region Fields
		private DeviceControlChannel _channel = null;
		private Boolean _channelEnabled = false;
		private Boolean _pinAEnabled = false;
		private Boolean _pinWEnabled = false;
		private Boolean _pinBEnabled = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>
		/// class with the device control channel, whether or not the
		/// channel is enabled, whether or not pin A is enabled,
		/// whether or not pin W is enabled, and whether or pin
		/// B is enabled.
		/// </summary>
		/// <param name="dcc">
		/// The device control channel.
		/// </param>
		/// <param name="chanEnabled">
		/// Set <c>true</c> to enable the channel.
		/// </param>
		/// <param name="pinAEnabled">
		/// Set <c>true</c> to enable pin A.
		/// </param>
		/// <param name="pinWEnabled">
		/// Set <c>true</c> to enable pin W.
		/// </param>
		/// <param name="pinBEnabled">
		/// Set <c>true</c> to enable pin B.
		/// </param>
		public DeviceControllerTerminalConfiguration(DeviceControlChannel dcc,
													Boolean chanEnabled,
													Boolean pinAEnabled,
													Boolean pinWEnabled,
													Boolean pinBEnabled) {
			this._channel = dcc;
			this._channelEnabled = chanEnabled;
			this._pinAEnabled = pinAEnabled;
			this._pinWEnabled = pinWEnabled;
			this._pinBEnabled = pinBEnabled;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the channel.
		/// </summary>
		public DeviceControlChannel Channel {
			get { return this._channel; }
		}

		/// <summary>
		/// Gets a value indicating whether or not the channel is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if channel enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean ChannelEnabled {
			get { return this._channelEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether or not pin A is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin A enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean PinAEnabled {
			get { return this._pinAEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether or not pin W is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin W enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean PinWEnabled {
			get { return this._pinWEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether or not pin B is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin B enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean PinBEnabled {
			get { return this._pinBEnabled; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in
		/// hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode() {
			Int32 hc = 13;
			hc = (hc * 7) + this._channel.GetHashCode();
			hc = (hc * 7) + this._channelEnabled.GetHashCode();
			hc = (hc * 7) + this._pinAEnabled.GetHashCode();
			hc = (hc * 7) + this._pinWEnabled.GetHashCode();
			hc = (hc * 7) + this._pinBEnabled.GetHashCode();
			return hc;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the
		/// current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			DeviceControllerTerminalConfiguration dctc = obj as DeviceControllerTerminalConfiguration;
			if ((Object)dctc == null) {
				return false;
			}

			return ((this._channel.Equals(dctc.Channel)) &&
					(this._channelEnabled == dctc.ChannelEnabled) &&
					(this._pinAEnabled == dctc.PinAEnabled) &&
					(this._pinWEnabled == dctc.PinWEnabled) &&
					(this._pinBEnabled == dctc.PinBEnabled));
		}

		/// <summary>
		/// Determines whether the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/> is equal
		/// to the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>.
		/// </summary>
		/// <param name="dctc">The <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/> to
		/// compare with the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>.</param>
		/// <returns>
		/// <c>true</c> if the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>
		/// is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControllerTerminalConfiguration"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public Boolean Equals(DeviceControllerTerminalConfiguration dctc) {
			if (dctc == null) {
				return false;
			}

			return ((this._channel.Equals(dctc.Channel)) &&
					(this._channelEnabled == dctc.ChannelEnabled) &&
					(this._pinAEnabled == dctc.PinAEnabled) &&
					(this._pinWEnabled == dctc.PinWEnabled) &&
					(this._pinBEnabled == dctc.PinBEnabled));
		}
		#endregion
	}
}

