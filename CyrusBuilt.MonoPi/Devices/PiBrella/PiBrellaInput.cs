//
//  PiBrellaInput.cs
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
	/// PiBrella inputs.
	/// </summary>
	public static class PiBrellaInput
	{
		/// <summary>
		/// PiBrella input A.
		/// </summary>
		public static readonly GpioMem A = new GpioMem(GpioPins.Pin13, PinMode.IN, PinState.Low);

		/// <summary>
		/// PiBrella input B.
		/// </summary>
		public static readonly GpioMem B = new GpioMem(GpioPins.GPIO11, PinMode.IN, PinState.Low);

		/// <summary>
		/// PiBrella input C.
		/// </summary>
		public static readonly GpioMem C = new GpioMem(GpioPins.GPIO10, PinMode.IN, PinState.Low);

		/// <summary>
		/// PiBrell input D.
		/// </summary>
		public static readonly GpioMem D = new GpioMem(GpioPins.Pin12, PinMode.IN, PinState.Low);

		/// <summary>
		/// PiBrella button input.
		/// </summary>
		public static readonly GpioMem BUTTON = new GpioMem(GpioPins.GPIO14, PinMode.IN, PinState.Low);
	}
}

