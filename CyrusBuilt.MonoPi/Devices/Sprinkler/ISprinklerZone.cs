//
//  ISprinklerZone.cs
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

namespace CyrusBuilt.MonoPi.Devices.Sprinkler
{
	/// <summary>
	/// A sprinkler zone interface.
	/// </summary>
	public interface ISprinklerZone
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		String Name { get; set; }

		/// <summary>
		/// Gets whether or not this zone is on.
		/// </summary>
		Boolean IsOn { get; }

		/// <summary>
		/// Gets whether or not this zone is off.
		/// </summary>
		Boolean IsOff { get; }

		/// <summary>
		/// Turns this zone on.
		/// </summary>
		void On();

		/// <summary>
		/// Turns this zone off.
		/// </summary>
		void Off();

		/// <summary>
		/// Sets the state of this zone..
		/// </summary>
		/// <param name="on">
		/// Set true to turn the zone on or false to turn it off.
		/// </param>
		void SetState(Boolean on);
	}
}

