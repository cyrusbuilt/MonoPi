//
//  IPiBrella.cs
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
using CyrusBuilt.MonoPi.Components.Button;
using CyrusBuilt.MonoPi.Components.Buzzer;
using CyrusBuilt.MonoPi.Components.Lights;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Devices.PiBrella
{
	/// <summary>
	/// PiBrella device abastraction interface.
	/// </summary>
	public interface IPiBrella : IDevice
	{
		/// <summary>
		/// Gets the red LED.
		/// </summary>
		/// <value>
		/// The red LED output component.
		/// </value>
		ILED RedLED { get; }

		/// <summary>
		/// Gets the yellow LED.
		/// </summary>
		/// <value>
		/// The yellow LED output component.
		/// </value>
		ILED YellowLED { get; }

		/// <summary>
		/// Gets the green LED.
		/// </summary>
		/// <value>
		/// The green LED output component.
		/// </value>
		ILED GreenLED { get; }

		/// <summary>
		/// Gets the LEDs.
		/// </summary>
		/// <value>
		/// The LEDs.
		/// </value>
		ILED[] LEDs { get; }

		/// <summary>
		/// Gets the button.
		/// </summary>
		/// <value>
		/// The PiBrella button input.
		/// </value>
		IButton Button { get; }

		/// <summary>
		/// Gets the buzzer.
		/// </summary>
		/// <value>
		/// The buzzer output component.
		/// </value>
		IBuzzer Buzzer { get; }

		/// <summary>
		/// Gets PiBrella input A.
		/// </summary>
		/// <value>
		/// Input A.
		/// </value>
		IRaspiGpio InputA { get; }

		/// <summary>
		/// Gets PiBrella input B.
		/// </summary>
		/// <value>
		/// Input B.
		/// </value>
		IRaspiGpio InputB { get; }

		/// <summary>
		/// Gets PiBrella input C.
		/// </summary>
		/// <value>
		/// Input C.
		/// </value>
		IRaspiGpio InputC { get; }

		/// <summary>
		/// Gets PiBrella input D.
		/// </summary>
		/// <value>
		/// Input D.
		/// </value>
		IRaspiGpio InputD { get; }

		/// <summary>
		/// Gets all the PiBrella inputs.
		/// </summary>
		/// <value>
		/// The inputs.
		/// </value>
		IRaspiGpio[] Inputs { get; }

		/// <summary>
		/// Gets PiBrella output E.
		/// </summary>
		/// <value>
		/// Output E.
		/// </value>
		IRaspiGpio OutputE { get; }

		/// <summary>
		/// Gets PiBrella output F.
		/// </summary>
		/// <value>
		/// Output F.
		/// </value>
		IRaspiGpio OutputF { get; }

		/// <summary>
		/// Gets PiBrella output G.
		/// </summary>
		/// <value>
		/// Output G.
		/// </value>
		IRaspiGpio OutputG { get; }

		/// <summary>
		/// Gets PiBrella output H.
		/// </summary>
		/// <value>
		/// Output H.
		/// </value>
		IRaspiGpio OutputH { get; }

		/// <summary>
		/// Gets all the PiBrella outputs.
		/// </summary>
		/// <value>
		/// The outputs.
		/// </value>
		IRaspiGpio[] Outputs { get; }
	}
}

