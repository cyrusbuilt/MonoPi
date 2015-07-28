//
//  UnrecognizedPinFoundEventArgs.cs
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

namespace CyrusBuilt.MonoPi.IO
{
	/// <summary>
	/// Unrecognized pin found event arguments class.
	/// </summary>
	public class UnrecognizedPinFoundEventArgs : EventArgs
	{
		private String _message = String.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.UnrecognizedPinFoundEventArgs"/>
		/// class with a message describing the event.
		/// </summary>
		/// <param name="eventMessage">
		/// A message describing the event.
		/// </param>
		public UnrecognizedPinFoundEventArgs(String eventMessage)
			: base() {
			this._message = eventMessage;
		}

		/// <summary>
		/// Gets the message describing the event.
		/// </summary>
		/// <value>
		/// The event message.
		/// </value>
		public String EventMessage {
			get { return this._message; }
		}
	}
}

