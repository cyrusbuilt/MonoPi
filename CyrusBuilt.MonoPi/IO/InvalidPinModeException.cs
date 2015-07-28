//
//  InvalidPinModeException.cs
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
	/// The exception that is thrown when an invalid pin mode is used.
	/// </summary>
	public class InvalidPinModeException : Exception
	{
		private IPin _pin = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.InvalidPinModeException"/>
		/// class with the pin in question and a message describing the exception.
		/// </summary>
		/// <param name="pin">
		/// The pin that is the cause of the exception.
		/// </param>
		/// <param name="message">
		/// The message describing the exception.
		/// </param>
		public InvalidPinModeException(IPin pin, String message)
			: base(message) {
			this._pin = pin;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.InvalidPinModeException"/>
		/// class with the pin in question
		/// </summary>
		/// <param name="pin">
		/// The pin that is the cause of the exception.
		/// </param>
		public InvalidPinModeException(IPin pin)
			: base() {
			this._pin = pin;
		}

		/// <summary>
		/// Gets the pin that is the cause of the exception.
		/// </summary>
		public IPin Pin {
			get { return this._pin; }
		}
	}
}

