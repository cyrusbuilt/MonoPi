//
//  GpioPinsRev2.cs
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
using System;

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// The various GPIO pins on the Raspberry Pi Revision 2.0 board.
	/// </summary>
	/// <remarks>
	/// Refer to http://elinux.org/Rpi_Low-level_peripherals for diagram.
	/// </remarks>		
	public enum GpioPinsRev2 : int
	{
		/// <summary>
		/// No pin (null).
		/// </summary>
		GPIO_NONE = -1
	}
}

