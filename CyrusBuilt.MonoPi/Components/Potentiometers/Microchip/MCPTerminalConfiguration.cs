//
//  MCPTerminalConfiguration.cs
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
	/// Terminal configuration settings for the channel a given
	/// potentiometer instance is configured for.
	/// </summary>
	public class MCPTerminalConfiguration
	{
		#region Fields
		private MicrochipPotChannel _channel = MicrochipPotChannel.None;
		private Boolean _channelEnabled = false;
		private Boolean _pinAEnabled = false;
		private Boolean _pinWEnabled = false;
		private Boolean _pinBEnabled = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>
		/// class with the channel and flags to indicated whether the channel is enabled or disabled
		/// as well as flags to indicate whether or not each pin is enabled or disabled.
		/// </summary>
		/// <param name="chan">
		/// The channel this terminal configuration represents.
		/// </param>
		/// <param name="chanEnabled">
		/// Set true to enable the channel.
		/// </param>
		/// <param name="pinAEnabled">
		/// Set true to enable pin A.
		/// </param>
		/// <param name="pinWEnabled">
		/// Set true to enable pin W.
		/// </param>
		/// <param name="pinBEnabled">
		/// Set true to enable pin B.
		/// </param>
		public MCPTerminalConfiguration(MicrochipPotChannel chan, Boolean chanEnabled, Boolean pinAEnabled, Boolean pinWEnabled, Boolean pinBEnabled) {
			this._channel = chan;
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
		public MicrochipPotChannel Channel {
			get { return this._channel; }
		}

		/// <summary>
		/// Gets a value indicating whether the entire channel is enabled or disabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if the channel is enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsChannelEnabled {
			get { return this._channelEnabled; }
		}

		/// <summary>
		/// Gets whether or not pin A of this channel is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin A is enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPinAEnabled {
			get { return this._pinAEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether or not pin W of this channel is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin W is enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPinWEnabled {
			get { return this._pinWEnabled; }
		}

		/// <summary>
		/// Gets a value indicating whether or not pin B of this channel is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if pin B is enabled; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPinBEnabled {
			get { return this._pinBEnabled; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode() {
			Int32 hash = 13;
			hash = (hash * 7) + this._channel.GetHashCode();
			hash = (hash * 7) + this._channelEnabled.GetHashCode();
			hash = (hash * 7) + this._pinAEnabled.GetHashCode();
			hash = (hash * 7) + this._pinWEnabled.GetHashCode();
			hash = (hash * 7) + this._pinBEnabled.GetHashCode();
			return hash;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			MCPTerminalConfiguration config = obj as MCPTerminalConfiguration;
			if ((Object)config == null) {
				return false;
			}

			return ((this._channel == config.Channel) &&
					(this._channelEnabled == config.IsChannelEnabled) &&
					(this._pinAEnabled == config.IsPinAEnabled) &&
					(this._pinWEnabled == config.IsPinWEnabled) &&
					(this._pinBEnabled == config.IsPinBEnabled));
		}

		/// <summary>
		/// Determines whether the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/> is equal to the
		/// current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.
		/// </summary>
		/// <param name="config">The <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/> to compare with
		/// the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.</param>
		/// <returns><c>true</c> if the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/> is equal to the
		/// current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>; otherwise, <c>false</c>.</returns>
		public Boolean Equals(MCPTerminalConfiguration config) {
			if (config == null) {
				return false;
			}

			return ((this._channel == config.Channel) &&
					(this._channelEnabled == config.IsChannelEnabled) &&
					(this._pinAEnabled == config.IsPinAEnabled) &&
					(this._pinWEnabled == config.IsPinWEnabled) &&
					(this._pinBEnabled == config.IsPinBEnabled));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCPTerminalConfiguration"/>.
		/// </returns>
		public override string ToString() {
			return string.Format("[MCPTerminalConfiguration: Channel={0}, IsChannelEnabled={1}, IsPinAEnabled={2}, IsPinWEnabled={3}, IsPinBEnabled={4}]", Channel, IsChannelEnabled, IsPinAEnabled, IsPinWEnabled, IsPinBEnabled);
		}
		#endregion
	}
}

