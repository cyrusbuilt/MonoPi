//
//  PiBrellaOutput.cs
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

namespace CyrusBuilt.MonoPi.Devices.PiBrella
{
	/// <summary>
	/// PiBrella outputs.
	/// </summary>
	public static class PiBrellaOutput
	{
		/// <summary>
		/// PiBrella output E.
		/// </summary>
		public static GpioMem E = new GpioMem(GpioPins.V2_GPIO03, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella output F.
		/// </summary>
		public static GpioMem F = new GpioMem(GpioPins.GPIO04, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella output G.
		/// </summary>
		public static GpioMem G = new GpioMem(GpioPins.Pin05, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella output H.
		/// </summary>
		public static GpioMem H = new GpioMem(GpioPins.V2_P5_Pin06, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella red LED.
		/// </summary>
		public static GpioMem LED_RED = new GpioMem(GpioPins.V2_GPIO02, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella yellow LED.
		/// </summary>
		public static GpioMem LED_YELLOW = new GpioMem(GpioPins.GPIO00, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella green LED.
		/// </summary>
		public static GpioMem LED_GREEN = new GpioMem(GpioPins.GPIO07, PinMode.OUT, PinState.Low);

		/// <summary>
		/// PiBrella buzzer.
		/// </summary>
		public static GpioMem BUZZER = new GpioMem(GpioPins.GPIO01, PinMode.PWM, PinState.Low);
	}
}

