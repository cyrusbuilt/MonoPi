//
//  TempScaleUtils.cs
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

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// Temperature scale utilities.
	/// </summary>
	public static class TempScaleUtils
	{
		/// <summary>
		/// Gets the name of the scale.
		/// </summary>
		/// <returns>
		/// The scale name.
		/// </returns>
		/// <param name="scale">
		/// The scale to get the name of.
		/// </param>
		public static String GetScaleName(TemperatureScale scale) {
			return Enum.GetName(typeof(TemperatureScale), scale);
		}

		/// <summary>
		/// Gets the scale postfix.
		/// </summary>
		/// <returns>
		/// The scale postfix
		/// .</returns>
		/// <param name="scale">
		/// The scale to get the postfix for.
		/// </param>
		public static Char GetScalePostfix(TemperatureScale scale) {
			return GetScaleName(scale).ToCharArray()[0];
		}
	}
}

