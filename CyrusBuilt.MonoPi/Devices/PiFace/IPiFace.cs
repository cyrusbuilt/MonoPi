//
//  IPiFace.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 Copyright (c) 2013 CyrusBuilt
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
using CyrusBuilt.MonoPi.Components.Lights;
using CyrusBuilt.MonoPi.Components.Relays;
using CyrusBuilt.MonoPi.Components.Switches;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Devices.PiFace
{
	/// <summary>
	/// An interface for a PiFace device abstraction.
	/// </summary>
	public interface IPiFace : IDevice
	{
		/// <summary>
		/// Gets the input pins.
		/// </summary>
		IPiFaceGPIO[] InputPins { get; }

		/// <summary>
		/// Gets the output pins.
		/// </summary>
		IPiFaceGPIO[] OutputPins { get; }

		/// <summary>
		/// Gets the relays.
		/// </summary>
		IRelay[] Relays { get; }

		/// <summary>
		/// Gets the switches.
		/// </summary>
		ISwitch[] Switches { get; }

		/// <summary>
		/// Gets the LEDs.
		/// </summary>
		ILED[] LEDs { get; }
	}
}

