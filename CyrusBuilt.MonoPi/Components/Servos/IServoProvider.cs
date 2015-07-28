//
//  IServoProvider.cs
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
using System.Collections.Generic;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// An interface that provides factory methods to create/cache
	/// <see cref="CyrusBuilt.MonoPi.Components.Servos.IServoDriver"/>
	/// objects.
	/// </summary>
	public interface IServoProvider
	{
		/// <summary>
		/// Gets a list of pins this driver implementation can drive.
		/// </summary>
		/// <value>
		/// A list of defined servo pins.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while trying to get the list of pins.
		/// </exception>
		List<IPin> DefinedServoPins { get; }

		/// <summary>
		/// Gets a driver for the requested pin.
		/// </summary>
		/// <param name="servoPin">
		/// The pin the driver is needed for.
		/// </param>
		/// <returns>
		/// The servo driver assigned to the pin. May be null if no driver is
		/// assigned or if the pin is unknown.
		/// </returns>
		/// <exception cref="System.IO.IOException">
		/// No driver is assigned to the specified pin - or - Cannot drive servo
		/// from specified pin - or - another initialization error occurred.
		/// </exception>
		IServoDriver GetServoDriver(IPin servoPin);
	}
}

