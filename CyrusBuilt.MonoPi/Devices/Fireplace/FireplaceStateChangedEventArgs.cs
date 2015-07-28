//
//  FireplaceStateChangeEvent.cs
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

namespace CyrusBuilt.MonoPi.Devices.Fireplace
{
	/// <summary>
	/// Fireplace state change event arguments.
	/// </summary>
	public class FireplaceStateChangedEventArgs : EventArgs
	{
		#region Fields
		private FireplaceState _oldState = FireplaceState.Off;
		private FireplaceState _newState = FireplaceState.Off;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceStateChangedEventArgs"/>
		/// class with the new and old states.
		/// </summary>
		/// <param name="oldState">
		/// The previous (old) state of the fireplace.
		/// </param>
		/// <param name="newState">
		/// The new (current) state of the fireplace.
		/// </param>
		public FireplaceStateChangedEventArgs(FireplaceState oldState, FireplaceState newState)
			: base() {
			this._oldState = oldState;
			this._newState = newState;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the previous state of the fireplace.
		/// </summary>
		/// <value>
		/// The previous state.
		/// </value>
		public FireplaceState OldState {
			get { return this._oldState; }
		}

		/// <summary>
		/// Gets the new state state of the fireplace.
		/// </summary>
		/// <value>
		/// The new state.
		/// </value>
		public FireplaceState NewState {
			get { return this._newState; }
		}
		#endregion
	}
}

