//
//  ILED.cs
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

namespace CyrusBuilt.MonoPi.Components.Lights
{
	/// <summary>
	/// An interface for LED abstraction components.
	/// </summary>
	public interface ILED : ILight
	{
		/// <summary>
		/// Toggles the state of the LED.
		/// </summary>
		void Toggle();

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		void Blink(Int32 delay);

		/// <summary>
		/// Blinks the LED.
		/// </summary>
		/// <param name="delay">
		/// The delay between state change.
		/// </param>
		/// <param name="duration">
		/// The amount of time to blink the LED (in milliseconds).
		/// </param>
		void Blink(Int32 delay, Int32 duration);

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		void Pulse(Int32 duration);

		/// <summary>
		/// Pulses the state of the LED.
		/// </summary>
		/// <param name="duration">
		/// The amount of time to pulse the LED.
		/// </param>
		/// <param name="blocking">
		/// Blocks the current thread while pulsing.
		/// </param>
		void Pulse(Int32 duration, Boolean blocking);
	}
}

