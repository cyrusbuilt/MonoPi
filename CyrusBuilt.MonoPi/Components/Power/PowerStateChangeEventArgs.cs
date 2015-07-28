//
//  PowerStateChangeEventArgs.cs
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

namespace CyrusBuilt.MonoPi.Components.Power
{
	/// <summary>
	/// Power state change event arguments class.
	/// </summary>
	public class PowerStateChangeEventArgs : EventArgs
	{
		private PowerState _oldState = PowerState.Uknown;
		private PowerState _newState = PowerState.Uknown;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Power.PowerStateChangeEventArgs"/>
		/// class with the old and new states.
		/// </summary>
		/// <param name="oldState">
		/// The previous state of the device.
		/// </param>
		/// <param name="newState">
		/// The new state of the device.
		/// </param>
		public PowerStateChangeEventArgs(PowerState oldState, PowerState newState)
			: base() {
			this._oldState = oldState;
			this._newState = newState;
		}

		/// <summary>
		/// Gets the old state.
		/// </summary>
		public PowerState OldState {
			get { return this._oldState; }
		}

		/// <summary>
		/// Gets the new state.
		/// </summary>
		public PowerState NewState {
			get { return this._newState; }
		}
	}
}

