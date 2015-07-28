//
//  PinStateChangeEventArgs.cs
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

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Pin state change event arguments class.
	/// </summary>
	public class PinStateChangeEventArgs : EventArgs
	{
		#region Fields
		private PinState _oldState = PinState.Low;
		private PinState _newState = PinState.High;
		private Int32 _pinAddress = -1;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PinStateChangeEventArgs"/>
		/// class with the old and new pin states.
		/// </summary>
		/// <param name="oldState">
		/// The previous state of the pin.
		/// </param>
		/// <param name="newState">
		/// The new state of the pin.
		/// </param>
		public PinStateChangeEventArgs(PinState oldState, PinState newState)
			: base() {
			this._oldState = oldState;
			this._newState = newState;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PinStateChangeEventArgs"/>
		/// class with the address of the pin and the old and new pin states.
		/// </summary>
		/// <param name="address">
		/// The pin address.
		/// </param>
		/// <param name="oldState">
		/// The previous state of the pin.
		/// </param>
		/// <param name="newState">
		/// The new state of the pin.
		/// </param>
		public PinStateChangeEventArgs(Int32 address, PinState oldState, PinState newState) {
			this._pinAddress = address;
			this._oldState = oldState;
			this._newState = newState;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the previous state of the pin.
		/// </summary>
		public PinState OldState {
			get { return this._oldState; }
		}

		/// <summary>
		/// Gets the new state of the pin.
		/// </summary>
		public PinState NewState {
			get { return this._newState; }
		}

		/// <summary>
		/// Gets the pin address.
		/// </summary>
		public Int32 PinAddress {
			get { return this._pinAddress; }
		}
		#endregion
	}
}

