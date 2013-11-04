//
//  IMotor.cs
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

namespace CyrusBuilt.MonoPi.Components.Motors
{
	/// <summary>
	/// A motor abstraction interface.
	/// </summary>
	public interface IMotor : IComponent
	{
		/// <summary>
		/// Occurs when motor state changes.
		/// </summary>
		event MotorStateChangeEventHandler StateChanged;

		/// <summary>
		/// Gets or sets the state of the motor.
		/// </summary>
		MotorState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether the motor is stopped.
		/// </summary>
		/// <value>
		/// <c>true</c> if this motor is stopped; otherwise, <c>false</c>.
		/// </value>
		Boolean IsStopped { get; }

		/// <summary>
		/// Tells the motor to move forward.
		/// </summary>
		void Forward();

		/// <summary>
		/// Tells the motor to move forward for the specified amount of time.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to continue moving forward for.
		/// </param>
		void Forward(Int32 millis);

		/// <summary>
		/// Tells the motor to reverse direction.
		/// </summary>
		void Reverse();

		/// <summary>
		/// Tells the motor to reverse direction for the specified amount of time.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to continue moving in reverse for.
		/// </param>
		void Reverse(Int32 millis);

		/// <summary>
		/// Stops the motor's movement.
		/// </summary>
		void Stop();

		/// <summary>
		/// Determines whether the motor's current state is the specified state.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current state is the specified state; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="state">
		/// The state to check for.
		/// </param>
		Boolean IsState(MotorState state);
	}
}

