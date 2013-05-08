//
//  GpioPins.cs
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
	/// The various GPIO pins on the Raspberry Pi Revision 1.0 and 2.0 boards.
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
	/// So to turn on Pin7 on the GPIO connector, pass in enum GpioPins.GPIO04 as
	/// the pin parameter.
	/// </remarks>
	public enum GpioPins : int
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
		LED = 16,

		#region Board Revision 2 pins
		/// <summary>
		/// Rev2 GPIO 02 (P1-03).
		/// </summary>
		V2_GPIO02 = 2,

		/// <summary>
		/// Rev2 GPIO 03 (P1-05).
		/// </summary>
		V2_GPIO03 = 3,

		/// <summary>
		/// Rev2 GPIO 04 (P1-07).
		/// </summary>
		V2_GPIO04 = 4,

		/// <summary>
		/// Rev2 GPIO 07 (P1-26).
		/// </summary>
		V2_GPIO07 = 7,

		/// <summary>
		/// Rev2 GPIO 08 (P1-24).
		/// </summary>
		V2_GPIO08 = 8,

		/// <summary>
		/// Rev2 GPIO 09 (P1-21).
		/// </summary>
		V2_GPIO09 = 9,

		/// <summary>
		/// Rev2 GPIO 10 (P1-19).
		/// </summary>
		V2_GPIO10 = 10,

		/// <summary>
		/// Rev2 GPIO 11 (P1-23).
		/// </summary>
		V2_GPIO11 = 11,

		/// <summary>
		/// Rev2 GPIO 14 (P1-08).
		/// </summary>
		V2_GPIO14 = 14,

		/// <summary>
		/// Rev2 GPIO 15 (P1-10).
		/// </summary>
		V2_GPIO15 = 15,

		/// <summary>
		/// Rev2 GPIO 17 (P1-11).
		/// </summary>
		V2_GPIO17 = 17,

		/// <summary>
		/// Rev2 GPIO 18 (P1-12).
		/// </summary>
		V2_GPIO18 = 18,

		/// <summary>
		/// Rev2 GPIO 22 (P1-15).
		/// </summary>
		V2_GPIO22 = 22,

		/// <summary>
		/// Rev2 GPIO 23 (P1-16).
		/// </summary>
		V2_GPIO23 = 23,

		/// <summary>
		/// Rev2 GPIO 24 (P1-18).
		/// </summary>
		V2_GPIO24 = 24,

		/// <summary>
		/// Rev2 GPIO 25 (P1-22).
		/// </summary>
		V2_GPIO25 = 25,

		/// <summary>
		/// Rev2 GPIO 27 (P1-13).
		/// </summary>
		V2_GPIO27 = 27,

		/// <summary>
		/// Rev2 Pin 3 (GPIO 02).
		/// </summary>
		V2_Pin03 = 2,

		/// <summary>
		/// Rev2 Pin 5 (GPIO 03).
		/// </summary>
		V2_Pin05 = 3,

		/// <summary>
		/// Rev2 Pin 7 (GPIO 04).
		/// </summary>
		V2_Pin07 = 4,

		/// <summary>
		/// Rev2 Pin 8 (GPIO 14).
		/// </summary>
		V2_Pin08 = 14,

		/// <summary>
		/// Rev2 Pin 10 (GPIO 15).
		/// </summary>
		V2_Pin10 = 15,

		/// <summary>
		/// Rev2 Pin 11 (GPIO 17).
		/// </summary>
		V2_Pin11 = 17,

		/// <summary>
		/// Rev2 Pin 12 (GPIO 18).
		/// </summary>
		V2_Pin12 = 18,

		/// <summary>
		/// Rev2 Pin 13 (GPIO 27).
		/// </summary>
		V2_Pin13 = 27,

		/// <summary>
		/// Rev2 Pin 15 (GPIO 22).
		/// </summary>
		V2_Pin15 = 22,

		/// <summary>
		/// Rev2 Pin 16 (GPIO 23).
		/// </summary>
		V2_Pin16 = 23,

		/// <summary>
		/// Rev2 Pin 18 (GPIO 24).
		/// </summary>
		V2_Pin18 = 24,

		/// <summary>
		/// Rev2 Pin 19 (GPIO 10).
		/// </summary>
		V2_Pin19 = 10,

		/// <summary>
		/// Rev2 Pin 21 (GPIO 09).
		/// </summary>
		V2_Pin21 = 9,

		/// <summary>
		/// Rev2 Pin 22 (GPIO 25).
		/// </summary>
		V2_Pin22 = 25,

		/// <summary>
		/// Rev2 Pin 23 (GPIO 11).
		/// </summary>
		V2_Pin23 = 11,

		/// <summary>
		/// Rev2 Pin 24 (GPIO 08).
		/// </summary>
		V2_Pin24 = 8,

		/// <summary>
		/// Rev2 Pin 26 (GPIO 07).
		/// </summary>
		V2_Pin26 = 7,
		#endregion

		#region Board Revision 2 New Plug P5
		/// <summary>
		/// Rev2 P5 header GPIO 28 (P5-03).
		/// </summary>
		V2_P5_Pin03 = 28,

		/// <summary>
		/// Rev2 P5 header GPIO 29 (P5-04).
		/// </summary>
		V2_P5_Pin04 = 29,

		/// <summary>
		/// Rev2 P5 header GPIO 30 (P5-05).
		/// </summary>
		V2_P5_Pin05 = 30,

		/// <summary>
		/// Rev2 P5 header GPIO 31 (P5-06).
		/// </summary>
		V2_P5_Pin06 = 31
		#endregion
	}
}

