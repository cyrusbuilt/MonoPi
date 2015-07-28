//
//  FireplaceTimeoutEventArgs.cs
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
	/// Fireplace timeout event arguments class.
	/// </summary>
	public class FireplaceTimeoutEventArgs : EventArgs
	{
		private Boolean _isHandled = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceTimeoutEventArgs"/>
		/// class. This is the default constructor.
		/// </summary>
		public FireplaceTimeoutEventArgs()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceTimeoutEventArgs"/>
		/// class with a flag indicating whether or not it was handled.
		/// </summary>
		/// <param name="handled">
		/// Set true if handled.
		/// </param>
		public FireplaceTimeoutEventArgs(Boolean handled)
			: base() {
			this._isHandled = handled;
		}

		/// <summary>
		/// Gets a value indicating whether this event was handled.
		/// </summary>
		/// <value>
		/// <c>true</c> if this event was handled; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsHandled {
			get { return this._isHandled; }
		}
	}
}

