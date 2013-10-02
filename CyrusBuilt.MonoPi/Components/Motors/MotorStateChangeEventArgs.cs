//
//  MotorStateChangeEventArgs.cs
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

namespace CyrusBuilt.MonoPi.Components.Motors
{
	/// <summary>
	/// Motor state change event arguments class.
	/// </summary>
	public class MotorStateChangeEventArgs : EventArgs
	{
		#region Fields
		private MotorState _oldState = MotorState.Stop;
		private MotorState _newState = MotorState.Stop;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Motors.MotorStateChangeEventArgs"/>
		/// class with the old and new states.
		/// </summary>
		/// <param name="oldState">
		/// The state the motor was in prior to the change.
		/// </param>
		/// <param name="newState">
		/// The current state of the motor since the change occurred.
		/// </param>
		public MotorStateChangeEventArgs(MotorState oldState, MotorState newState)
			: base() {
			this._oldState = oldState;
			this._newState = newState;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state the motor was in prior to the change.
		/// </summary>
		public MotorState OldState {
			get { return this._oldState; }
		}

		/// <summary>
		/// Gets the new (current) state.
		/// </summary>
		public MotorState NewState {
			get { return this._newState; }
		}
		#endregion
	}
}

