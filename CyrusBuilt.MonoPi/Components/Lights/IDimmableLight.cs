//
//  IDimmableLight.cs
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
	/// An interface for dimmable light component abstractions.
	/// </summary>
	public interface IDimmableLight : ILight
	{
		/// <summary>
		/// Occurs when the light level changed.
		/// </summary>
		event LightLevelChangeEventHandler LevelChanged;

		/// <summary>
		/// Gets or sets the brightness level.
		/// </summary>
		/// <value>
		/// The brightness level.
		/// </value>
		Int32 Level { get; set; }

		/// <summary>
		/// Gets the minimum brightness level.
		/// </summary>
		Int32 MinLevel { get; }

		/// <summary>
		/// Gets the maximum brightness level.
		/// </summary>
		Int32 MaxLevel { get; }

		/// <summary>
		/// Gets the current brightness level percentage.
		/// </summary>
		/// <returns>
		/// The brightness percentage level.
		/// </returns>
		float GetLevelPercentage();

		/// <summary>
		/// Gets the current brightness level percentage.
		/// </summary>
		/// <returns>
		/// The brightness percentage level.
		/// </returns>
		/// <param name="level">
		/// The brightness level.
		/// </param>
		float GetLevelPercentage(Int32 level);
	}
}

