//
//  OpenerLockChangeEventArgs.cs
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

namespace CyrusBuilt.MonoPi.Devices.Access
{
	/// <summary>
	/// Opener lock change event arguments class.
	/// </summary>
	public class OpenerLockChangeEventArgs : EventArgs
	{
		private Boolean _isLocked = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerLockChangeEventArgs"/>
		/// class with the lock state.
		/// </summary>
		/// <param name="locked">
		/// Set true if locked.
		/// </param>
		public OpenerLockChangeEventArgs(Boolean locked)
			: base() {
			this._isLocked = locked;
		}

		/// <summary>
		/// Gets a value indicating whether the opener is locked.
		/// </summary>
		/// <value>
		/// <c>true</c> if the opener is locked; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsLocked {
			get { return this._isLocked; }
		}
	}
}

