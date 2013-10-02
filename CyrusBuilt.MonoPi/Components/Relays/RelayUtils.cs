//
//  RelayUtils.cs
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

namespace CyrusBuilt.MonoPi.Components.Relays
{
	/// <summary>
	/// Relay utility methods.
	/// </summary>
	public static class RelayUtils
	{
		/// <summary>
		/// Gets the inverse of the specified state.
		/// </summary>
		/// <returns>
		/// The inverse of the specified state.
		/// </returns>
		/// <param name="state">
		/// The relay state to invert.
		/// </param>
		public static RelayState GetInverseState(RelayState state) {
			if (state == RelayState.Open) {
				return RelayState.Closed;
			}
			return RelayState.Open;
		}
	}
}

