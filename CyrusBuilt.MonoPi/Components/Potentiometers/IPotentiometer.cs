//
//  IPotentiometer.cs
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

namespace CyrusBuilt.MonoPi.Components.Potentiometers
{
	/// <summary>
	/// A digital potentiometer device abstraction component interface.
	/// </summary>
	public interface IPotentiometer : IComponent
	{
		#region Properties
		/// <summary>
		/// Gets the maximum wiper-value supported by the device.
		/// </summary>
		Int32 MaxValue { get; }

		/// <summary>
		/// Gets whether the device is a potentiometer or a rheostat.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is rheostat; otherwise, <c>false</c>.
		/// </value>
		Boolean IsRheostat { get; }

		/// <summary>
		/// Gets or sets the wiper's current value.
		/// </summary>
		/// <value>
		/// The current value. Values from 0 to <see cref="MaxValue"/>
		/// are valid. Values above or below these boundaries should be
		/// corrected quietly.
		/// </value>
		/// <returns>
		/// The wiper's current value. The implementation should
		/// cache the wiper's value and therefore should avoid
		/// accessing the device too often.
		/// </returns>
		Int32 CurrentValue { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Increase the wiper's value by one step. It is not an error
		/// if the wiper already hit the upper boundary. In this
		/// situation, the wiper doesn't change.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		void Increase();

		/// <summary>
		/// Increases the wiper's value by the specified number of steps.
		/// It is not an error if the wiper hits or already hit the upper
		/// boundary. In such situations, the wiper sticks to the upper
		/// boundary or doesn't change.
		/// </summary>
		/// <param name="steps">
		/// How many steps to increase.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		void Increase(Int32 steps);

		/// <summary>
		/// Decreases the wiper's value by one step. It is not an error
		/// if the wiper already hit the lower boundary (0). In this
		/// situation, the wiper doesn't change.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		void Decrease();

		/// <summary>
		/// Decreases the wiper's value by the specified number of
		/// steps. It is not an error if the wiper hits or already
		/// hit the lower boundary (0). In such situations, the
		/// wiper sticks to the lower boundary or doesn't change.
		/// </summary>
		/// <param name="steps">
		/// The number of steps to decrease by.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		void Decrease(Int32 steps);
		#endregion
	}
}

