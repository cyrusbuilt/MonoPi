//
//  GpioPinsRev1.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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
//  Derived from https://github.com/cypherkey/RaspberryPi.Net
//  by Aaron Anderson <aanderson@netopia.ca>
//  
using System;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// The various GPIO pins on the Raspberry Pi Revision 1.0 board.
	/// </summary>
	/// <remarks>
	/// Refer to http://elinux.org/Rpi_Low-level_peripherals for diagram.
	/// P1-01 = bottom left, P1-02 = top left
	/// pi connector P1 pin    = GPIOnum
	///                  P1-03 = GPIO0
	///                  P1-05 = GPIO1
	///                  P1-07 = GPIO4
	///                  P1-08 = GPIO14 - alt function (UART0_TXD) on boot-up
	///                  P1-10 = GPIO15 - alt function (UART0_TXD) on boot-up
	///                  P1-11 = GPIO17
	///                  P1-12 = GPIO18
	///                  P1-13 = GPIO21
	///                  P1-15 = GPIO22
	///                  P1-16 = GPIO23
	///                  P1-18 = GPIO24
	///                  P1-19 = GPIO10
	///                  P1-21 = GPIO9
	///                  P1-22 = GPIO25
	///                  P1-23 = GPIO11
	///                  P1-24 = GPIO8
	///                  P1-26 = GPIO7
	/// So to turn on Pin7 on the GPIO connector, pass in enum GpioPinsRev1.GPIO04 as
	/// the pin parameter.
	/// </remarks>
	public enum GpioPinsRev1 : int
	{
		/// <summary>
		/// No pin (null).
		/// </summary>
		GPIO_NONE = -1,

		/// <summary>
		/// GPIO 00 (pin P1-03).
		/// </summary>
		GPIO00 = 0,

		/// <summary>
		/// GPIO 01 (pin P1-05).
		/// </summary>
		GPIO01 = 1,

		/// <summary>
		/// GPIO 04 (pin P1-07).
		/// </summary>
		GPIO04 = 4,

		/// <summary>
		/// GPIO 07 (pin P1-26).
		/// </summary>
		GPIO07 = 7,

		/// <summary>
		/// GPIO 08 (pin P1-24).
		/// </summary>
		GPIO08 = 8,

		/// <summary>
		/// GPIO pin 09 (pin P1-21).
		/// </summary>
		GPIO09 = 9,

		/// <summary>
		/// GPIO pin 10 (pin P1-19).
		/// </summary>
		GPIO10 = 10,

		/// <summary>
		/// GPIO pin 11 (pin P1-23).
		/// </summary>
		GPIO11 = 11,

		/// <summary>
		/// GPIO pin 14 (pin P1-08).
		/// </summary>
		GPIO14 = 14,

		/// <summary>
		/// GPIO pin 15 (pin P1-10).
		/// </summary>
		GPIO15 = 15,

		/// <summary>
		/// GPIO pin 17 (pin P1-11).
		/// </summary>
		GPIO17 = 17,

		/// <summary>
		/// GPIO pin 18 (pin P1-12).
		/// </summary>
		GPIO18 = 18,

		/// <summary>
		/// GPIO pin 21 (pin P1-13).
		/// </summary>
		GPIO21 = 21,

		/// <summary>
		/// GPIO pin 22 (pin P1-15).
		/// </summary>
		GPIO22 = 22,

		/// <summary>
		/// GPIO pin 23 (pin P1-16).
		/// </summary>
		GPIO23 = 23,

		/// <summary>
		/// GPIO pin 24 (pin P1-18).
		/// </summary>
		GPIO24 = 24,

		/// <summary>
		/// GPIO pin 25 (pin P1-22).
		/// </summary>
		GPIO25 = 25,

		/// <summary>
		/// Pin 3.
		/// </summary>
		Pin03 = 0,

		/// <summary>
		/// Pin 5.
		/// </summary>
		Pin05 = 1,

		/// <summary>
		/// Pin 7.
		/// </summary>
		Pin07 = 4,

		/// <summary>
		/// Pin 8.
		/// </summary>
		Pin08 = 14,

		/// <summary>
		/// Pin 10.
		/// </summary>
		Pin10 = 15,

		/// <summary>
		/// Pin 11.
		/// </summary>
		Pin11 = 17,

		/// <summary>
		/// Pin 12.
		/// </summary>
		Pin12 = 18,

		/// <summary>
		/// Pin 13.
		/// </summary>
		Pin13 = 21,

		/// <summary>
		/// Pin 15.
		/// </summary>
		Pin15 = 22,

		/// <summary>
		/// Pin 16.
		/// </summary>
		Pin16 = 23,

		/// <summary>
		/// Pin 18.
		/// </summary>
		Pin18 = 24,

		/// <summary>
		/// Pin 19.
		/// </summary>
		Pin19 = 10,

		/// <summary>
		/// Pin 21.
		/// </summary>
		Pin21 = 9,

		/// <summary>
		/// Pin 22.
		/// </summary>
		Pin22 = 25,

		/// <summary>
		/// Pin 23.
		/// </summary>
		Pin23 = 11,

		/// <summary>
		/// Pin 24.
		/// </summary>
		Pin24 = 8,

		/// <summary>
		/// Pin 26.
		/// </summary>
		Pin26 = 7,

		/// <summary>
		/// LED driver pin.
		/// </summary>
		LED = 16
	}
}

