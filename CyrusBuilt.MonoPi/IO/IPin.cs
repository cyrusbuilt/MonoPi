//
//  IPin.cs
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
	/// A physical pin interface.
	/// </summary>
	public interface IPin : IDisposable
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name of the GPIO.
		/// </value>
		String Name { get; set; }

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		/// <value>
		/// The object to tag the GPIO with.
		/// </value>
		Object Tag { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		Boolean IsDisposed { get; }

		/// <summary>
		/// Gets the state of the pin.
		/// </summary>
		PinState State { get; }

		/// <summary>
		/// Gets the pin mode.
		/// </summary>
		PinMode Mode { get; }

		/// <summary>
		/// Gets the pin address.
		/// </summary>
		/// <value>
		/// The address.
		/// </value>
		Int32 Address { get; }
	}
}

