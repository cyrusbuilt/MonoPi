//
//  ButtonEventArgs.cs
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

namespace CyrusBuilt.MonoPi.Components.Button
{
	/// <summary>
	/// Button event arguments class.
	/// </summary>
	public class ButtonEventArgs : EventArgs
	{
		private IButton _button = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonEventArgs"/>
		/// class with the button that is changing state.
		/// </summary>
		/// <param name="button">
		/// The button that is changing state.
		/// </param>
		public ButtonEventArgs(IButton button)
			: base() {
			this._button = button;
		}

		/// <summary>
		/// Gets the button.
		/// </summary>
		/// <value>
		/// The button.
		/// </value>
		public IButton Button {
			get { return this._button; }
		}

		/// <summary>
		/// Gets a value indicating whether the button is pressed.
		/// </summary>
		/// <value>
		/// <c>true</c> if the button is pressed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPressed {
			get {
				if (this._button == null) {
					return false;
				}
				return this._button.IsPressed;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the button is released.
		/// </summary>
		/// <value>
		/// <c>true</c> if the button is released; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsReleased {
			get {
				if (this._button == null) {
					return false;
				}
				return this._button.IsReleased;
			}
		}

		/// <summary>
		/// Determines whether the button is in state the specified state.
		/// </summary>
		/// <param name="state">
		/// The button state to compare to.
		/// </param>
		/// <returns>
		/// <c>true</c> if the button is in state the specified state; otherwise, <c>false</c>.
		/// </returns>
		public Boolean IsState(ButtonState state) {
			if (this._button == null) {
				return false;			}
			return this._button.IsState(state);
		}
	}
}

