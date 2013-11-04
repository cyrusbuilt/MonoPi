//
//  IRelay.cs
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

namespace CyrusBuilt.MonoPi.Components.Relays
{
	/// <summary>
	/// A relay component abstraction interface.
	/// </summary>
	public interface IRelay : IComponent
	{
		/// <summary>
		/// Occurs when relay state changes.
		/// </summary>
		event RelayStateChangeEventHandler StateChanged;

		/// <summary>
		/// Occurs when a relay pulse starts.
		/// </summary>
		event RelayPulseEventHandler PulseStart;

		/// <summary>
		/// Occurs when a relay pulse stops.
		/// </summary>
		event RelayPulseEventHandler PulseStop;

		/// <summary>
		/// Gets or sets the state of the relay.
		/// </summary>
		RelayState State { get; set; }

		/// <summary>
		/// Checks to see if the dry contacts are open.
		/// </summary>
		Boolean IsOpen { get; }

		/// <summary>
		/// Checks to see if the dry contacts are closed.
		/// </summary>
		Boolean IsClosed { get; }

		/// <summary>
		/// Opens the dry contacts on the relay.
		/// </summary>
		void Open();

		/// <summary>
		/// Closes the dry contacts on the relay.
		/// </summary>
		void Close();

		/// <summary>
		/// Toggles the state of the relay (closed, then open).
		/// </summary>
		void Toggle();

		/// <summary>
		/// Pulses the relay on and off.
		/// </summary>
		void Pulse();

		/// <summary>
		/// Pulses the relay on for the specified number of milliseconds, then
		/// back off again.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to wait before switching back off.
		/// </param>
		void Pulse(Int32 millis);
	}
}

