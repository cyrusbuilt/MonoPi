//
//  ClockType.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2014 Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.PiSystem
{
	/// <summary>
	/// The various on-board clock types.
	/// </summary>
	public enum ClockType
	{
		/// <summary>
		/// The ARM clock.
		/// </summary>
		ARM,

		/// <summary>
		/// The core clock.
		/// </summary>
		Core,

		/// <summary>
		/// The H264 clock.
		/// </summary>
		H264,

		/// <summary>
		/// The ISP clock.
		/// </summary>
		ISP,

		/// <summary>
		/// The V3D clock.
		/// </summary>
		V3D,

		/// <summary>
		/// The UART clock.
		/// </summary>
		UART,

		/// <summary>
		/// The PWM clock.
		/// </summary>
		PWM,

		/// <summary>
		/// The EMMC clock.
		/// </summary>
		EMMC,

		/// <summary>
		/// The pixel clock.
		/// </summary>
		Pixel,

		/// <summary>
		/// The VEC clock.
		/// </summary>
		VEC,

		/// <summary>
		/// The HDMI clock.
		/// </summary>
		HDMI,

		/// <summary>
		/// The DPI clock.
		/// </summary>
		DPI
	}
}

