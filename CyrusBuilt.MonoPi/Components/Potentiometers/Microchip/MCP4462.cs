﻿//
//  MCP4462.cs
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
using CyrusBuilt.MonoPi.IO.I2C;

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// Hardware device abstraction component for the Microchip MCP4462.
	/// </summary>
	public class MCP4462 : MicrochipPotentiometerBase
	{
		private static readonly MicrochipPotChannel[] SUPPORTED_CHANNELS = {
			MicrochipPotChannel.A,
			MicrochipPotChannel.B,
			MicrochipPotChannel.C,
			MicrochipPotChannel.D
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MCP4462"/>
		/// class with the I2C device that is the connection to MCP4462,
		/// whether the address pin A0 is high or not, and the initial
		/// value of the wiper.
		/// </summary>
		/// <param name="device">
		/// The I2C bus device this instance is connected to.
		/// </param>
		/// <param name="pinA0">
		/// Whether the device's address pin A0 is high (true) or low (false).
		/// </param>
		/// <param name="initialValue">
		/// The initial value to set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="device"/> cannot be null.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// Unable to open the I2C bus.
		/// </exception>
		public MCP4462(II2CBus device, Boolean pinA0, Int32 initialValue)
			:base(device, pinA0, PIN_NOT_AVAILABLE, PIN_NOT_AVAILABLE,
				MicrochipPotChannel.A, MicrochipPotNonVolatileMode.VolatileOnly,
				initialValue) {
		}

		/// <summary>
		/// Gets whether or not the device is capable of non-volatile wipers.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is non volatile wiper capable;
		/// otherwise, <c>false</c>.
		/// </value>
		public override bool IsNonVolatileWiperCapable {
			get { return true; }
		}

		/// <summary>
		/// Gets the maximum wiper-value supported by the device.
		/// </summary>
		/// <value>
		/// The max value.
		/// </value>
		public override int MaxValue {
			get { return 256; }
		}

		/// <summary>
		/// Gets whether the device is a potentiometer or a rheostat.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is rheostat; otherwise, <c>false</c>.
		/// </value>
		public override bool IsRheostat {
			get { return true; }
		}

		/// <summary>
		/// Gets the channels that are supported by the underlying device.
		/// </summary>
		/// <value>
		/// The supported channels.
		/// </value>
		public override MicrochipPotChannel[] SupportedChannels {
			get { return SUPPORTED_CHANNELS; }
		}
	}
}

