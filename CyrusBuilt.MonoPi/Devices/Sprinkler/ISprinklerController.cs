//
//  ISprinklerController.cs
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
using System.Collections.Generic;

namespace CyrusBuilt.MonoPi.Devices.Sprinkler
{
	/// <summary>
	/// A sprinkler controller abstraction interface.
	/// </summary>
	public interface ISprinklerController
	{
		/// <summary>
		/// Gets the zone count.
		/// </summary>
		Int32 ZoneCount { get; }

		/// <summary>
		/// Gets the zones assigned to this controller.
		/// </summary>
		List<ISprinklerZone> Zones { get; }

		/// <summary>
		/// Gets a value indicating whether this controller is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this controller is on; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOn { get; }

		/// <summary>
		/// Gets a value indicating whether this controller is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if this controller is off; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOff { get; }

		/// <summary>
		/// Gets a value indicating whether the sprinklers are raining.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is raining; otherwise, <c>false</c>.
		/// </value>
		Boolean IsRaining { get; }

		/// <summary>
		/// Determines whether the specified zone is on.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the zone is on; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="zone">
		/// The zone to check.
		/// </param>
		Boolean IsOnForZone(Int32 zone);

		/// <summary>
		/// Determines whether the specified zone is off.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the zone is off; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="zone">
		/// The zone to check.
		/// </param>
		Boolean IsOffForZone(Int32 zone);

		/// <summary>
		/// Turns the specified zone on.
		/// </summary>
		/// <param name="zone">
		/// The zone to turn on.
		/// </param>
		void On(Int32 zone);

		/// <summary>
		/// Turns all zones on.
		/// </summary>
		void OnAllZones();

		/// <summary>
		/// Turns the specified zone off.
		/// </summary>
		/// <param name="zone">
		/// The zone to turn off.
		/// </param>
		void Off(Int32 zone);

		/// <summary>
		/// Turns off all zones.
		/// </summary>
		void OffAllZones();

		/// <summary>
		/// Sets the state of the specified zone.
		/// </summary>
		/// <param name="zone">
		/// The zone to set the state of.
		/// </param>
		/// <param name="on">
		/// Set true to turn on the specified zone.
		/// </param>
		void SetState(Int32 zone, Boolean on);
	}
}

