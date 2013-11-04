//
//  OpenerBase.cs
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
	/// Base class for opener device abstractions.
	/// </summary>
	public abstract class OpenerBase : DeviceBase, IOpener
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Access.OpenerBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected OpenerBase()
			: base() {
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when lock state changes.
		/// </summary>
		public event OpenerLockChangEventHandler LockChanged;

		/// <summary>
		/// Occurs when state changes.
		/// </summary>
		public event OpenerStateChangeEventHandler StateChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this opener is locked and thus, cannot be opened.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is locked; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsLocked { get; }

		/// <summary>
		/// Gets the state of this opener.
		/// </summary>
		public abstract OpenerState State { get; }

		/// <summary>
		/// Gets a value indicating whether this opener is open.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is open; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOpen {
			get { return (this.State == OpenerState.Open); }
		}

		/// <summary>
		/// Gets a value indicating whether this opner is in the the process of opening.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is opening; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOpening {
			get { return (this.State == OpenerState.Opening); }
		}

		/// <summary>
		/// Gets a value indicating whether this opener is closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is closed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsClosed {
			get { return (this.State == OpenerState.Closed); }
		}

		/// <summary>
		/// Gets a value indicating whether this opener is in the process of closing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is closing; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsClosing {
			get { return (this.State == OpenerState.Closing); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the lock changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnLockChanged(OpenerLockChangeEventArgs e) {
			if (this.LockChanged != null) {
				this.LockChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(OpenerStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Opens the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be opened because it is locked.
		/// </exception>
		public abstract void Open();

		/// <summary>
		/// Closes the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be closed because it is locked.
		/// </exception>
		public abstract void Close();
		#endregion
	}
}

