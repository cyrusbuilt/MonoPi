//
//  IRCSwitch.cs
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

namespace CyrusBuilt.MonoPi.Devices.RCSwitch
{
	/// <summary>
	/// An RC switch device abstraction interface. These are remote-controlled
	/// power outlets that basically just switches a relay or SSR.
	/// </summary>
	public interface IRCSwitch : IDisposable
	{
		#region Properties
		/// <summary>
		/// Gets or sets the operating protocol.
		/// </summary>
		/// <value>
		/// The protocol. If the <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.IRCSwitch.PulseLength"/>
		/// value is set to less than or equal to zero, then the default
		/// pulse length value for the specified protocol will be set.
		/// </value>
		RCProtocol Protocol { get; set; }

		/// <summary>
		/// Gets or sets the length of the pulse.
		/// </summary>
		/// <value>
		/// The length of the pulse.
		/// </value>
		Int32 PulseLength { get; set; }

		/// <summary>
		/// Gets or sets the transmit repititions.
		/// </summary>
		/// <value>
		/// The transmit repeats.
		/// </value>
		Int32 RepeatTransmit { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Switch a remote switch on (Type A with 10 pole DIP switches).
		/// </summary>
		/// <param name="switchGroupAddress">
		/// Code of the switch group (refers to DIP switches 1 - 5, where
		/// "1" = on and "0" = off, if all DIP switches are on it's "11111").
		/// </param>
		/// <param name="device">
		/// The switch device number.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="switchGroupAddress"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="switchGroupAddress"/> cannot have more than 5 bits.
		/// </exception>
		void SwitchOn(BitSet switchGroupAddress, RCSwitchDevNum device);

		/// <summary>
		/// Switch a remote switch off (Type A with 10 pole DIP switches).
		/// </summary>
		/// <param name="switchGroupAddress">
		/// Code of the switch group (refers to DIP switches 1 - 5, where
		/// "1" = on and "0" = off, if all DIP switches are on it's "11111").
		/// </param>
		/// <param name="device">
		/// The switch device number.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="switchGroupAddress"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="switchGroupAddress"/> cannot have more than 5 bits.
		/// </exception>
		void SwitchOff(BitSet switchGroupAddress, RCSwitchDevNum device);

		/// <summary>
		/// Switch a remote switch on (Type B with two rotary/sliding switches).
		/// </summary>
		/// <param name="address">
		/// The address of the switch group.
		/// </param>
		/// <param name="channel">
		/// The channel (switch) itself.
		/// </param>
		void SwitchOn(AddressCode address, ChannelCode channel);

		/// <summary>
		/// Switch a remote switch off (Type B with two rotary/sliding switches).
		/// </summary>
		/// <param name="address">
		/// The address of the switch group.
		/// </param>
		/// <param name="channel">
		/// The channel (switch) itself.
		/// </param>
		void SwitchOff(AddressCode address, ChannelCode channel);
		#endregion
	}
}

