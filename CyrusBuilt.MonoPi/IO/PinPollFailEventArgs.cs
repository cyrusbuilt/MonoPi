//
//  PinPollFailEventArgs.cs
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
	/// Pin poll failure event arguments class.
	/// </summary>
	public class PinPollFailEventArgs : EventArgs
	{
		private Exception _failCause = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PinPollFailEventArgs"/>
		/// class with the exception that is the cause of the failure event.
		/// </summary>
		/// <param name="cause">
		/// The exception that is the cause of the poll failure.
		/// </param>
		public PinPollFailEventArgs(Exception cause)
			: base() {
			this._failCause = cause;
		}

		/// <summary>
		/// Gets the exception that is the cause of the poll failure.
		/// </summary>
		public Exception FailureCause {
			get { return this._failCause; }
		}
	}
}

