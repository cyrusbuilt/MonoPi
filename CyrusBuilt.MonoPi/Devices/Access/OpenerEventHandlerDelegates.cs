//
//  OpenerEventHandlerDelegates.cs
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
	/// Opener lock chang event handler delegate.
	/// </summary>
	/// <param name="sender">
	/// The object firing the event.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void OpenerLockChangEventHandler(Object sender, OpenerLockChangeEventArgs e);

	/// <summary>
	/// Opener state change event handler delegate.
	/// </summary>
	/// <param name="sender">
	/// The object firing the event.
	/// </param>
	/// <param name="e">
	/// The event arguments.
	/// </param>
	public delegate void OpenerStateChangeEventHandler(Object sener, OpenerStateChangeEventArgs e);
}

