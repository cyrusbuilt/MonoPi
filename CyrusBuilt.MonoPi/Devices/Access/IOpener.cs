//
//  IOpener.cs
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
	/// Opener device abstraction interface.
	/// </summary>
	public interface IOpener : IDevice
	{
		/// <summary>
		/// Gets a value indicating whether this opener is open.
		/// </summary>
		/// <value>
		/// <c>true</c> if this opener is open; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOpen { get; }

		/// <summary>
		/// Gets a value indicating whether this opner is in the the process of opening.
		/// </summary>
		/// <value>
		/// <c>true</c> if this opener is opening; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOpening { get; }

		/// <summary>
		/// Gets a value indicating whether this opener is closed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this opener is closed; otherwise, <c>false</c>.
		/// </value>
		Boolean IsClosed { get; }

		/// <summary>
		/// Gets a value indicating whether this opener is in the process of closing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this opener is closing; otherwise, <c>false</c>.
		/// </value>
		Boolean IsClosing { get; }

		/// <summary>
		/// Gets the state of this opener.
		/// </summary>
		OpenerState State { get; }

		/// <summary>
		/// Gets a value indicating whether this opener is locked and thus, cannot be opened.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is locked; otherwise, <c>false</c>.
		/// </value>
		Boolean IsLocked { get; }

		/// <summary>
		/// Opens the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be opened because it is locked.
		/// </exception>
		void Open();

		/// <summary>
		/// Closes the opener.
		/// </summary>
		/// <exception cref="OpenerLockedException">
		/// The opener cannot be closed because it is locked.
		/// </exception>
		void Close();
	}
}

