//
//  IServoDriver.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// Represents driver hardware used to produce pulses needed for driving
	/// a servo.
	/// </summary>
	public interface IServoDriver : IDisposable
	{
		/// <summary>
		/// Gets or sets the current servo pulse width. Zero may represent
		/// this driver stopped producing pulses. Also, value of -1
		/// may define undefined situation when this abstraction didn't get
		/// initial value yet and there is no way telling what real, hardware
		/// or software driver is sending.
		/// </summary>
		/// <value>
		/// Current servo pulse this driver is producing.
		/// </value>
		Int32 ServoPulseWidth { get; set; }

		/// <summary>
		/// Gets the servo pulse resolution.
		/// </summary>
		/// <value>
		/// The servo pulse resolution. Resolution is provided in 1/n (ms)
		/// where value returned is n.
		/// </value>
		Int32 ServoPulseResolution { get; }

		/// <summary>
		/// Gets the pin.
		/// </summary>
		/// <value>
		/// The pin.
		/// </value>
		IPin Pin { get; }
	}
}

