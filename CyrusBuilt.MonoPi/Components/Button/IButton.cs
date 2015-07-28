//
//  IButton.cs
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
	/// A button device abstraction component interface.
	/// </summary>
	public interface IButton : IComponent
	{
		#region Events
		/// <summary>
		/// Occurs when the button state has changed.
		/// </summary>
		event ButtonStateChangeHandler StateChanged;

		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
		event ButtonStateChangeHandler Pressed;

		/// <summary>
		/// Occurs when the button is released.
		/// </summary>
		event ButtonStateChangeHandler Released;

		/// <summary>
		/// Occurs when the button is held.
		/// </summary>
		event ButtonStateChangeHandler Hold;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is pressed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
		/// </value>
		Boolean IsPressed { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is released.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is released; otherwise, <c>false</c>.
		/// </value>
		Boolean IsReleased { get; }

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		ButtonState State { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Determines whether this button's state is the specified state.
		/// </summary>
		/// <param name="state">
		/// The state to check.
		/// </param>
		/// <returns>
		/// <c>true</c> if this button's state is the specified state; otherwise, <c>false</c>.
		/// </returns>
		Boolean IsState(ButtonState state);
		#endregion
	}
}

