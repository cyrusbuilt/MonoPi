//
//  IStepperMotor.cs
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
	/// A stepper motor abstraction interface.
	/// </summary>
	public interface IStepperMotor : IMotor
	{
		/// <summary>
		/// Occurs when rotation starts.
		/// </summary>
		event MotorRotationStartEventHandler RotationStarted;

		/// <summary>
		/// Occurs when rotation stops.
		/// </summary>
		event MotorRotationStopEventHandler RotationStopped;

		/// <summary>
		/// Gets or sets the steps per revolution.
		/// </summary>
		Int32 StepsPerRevolution { get; set; }

		/// <summary>
		/// Gets or sets the step sequence.
		/// </summary>
		Byte[] StepSequence { get; set; }

		/// <summary>
		/// Sets the step interval.
		/// </summary>
		/// <param name="millis">
		/// The milliseconds between steps.
		/// </param>
		void SetStepInterval(Int32 millis);

		/// <summary>
		/// Sets the step interval.
		/// </summary>
		/// <param name="millis">
		/// The milliseconds between steps.
		/// </param>
		/// <param name="nanoseconds">
		/// The nanoseconds between steps.
		/// </param>
		void SetStepInterval(Int32 millis, Int32 nanoseconds);

		/// <summary>
		/// Rotate the specified revolutions.
		/// </summary>
		/// <param name="revolutions">
		/// The number of revolutions to rotate.
		/// </param>
		void Rotate(Double revolutions);

		/// <summary>
		/// Step the motor the specified steps.
		/// </summary>
		/// <param name="steps">
		/// The number of steps to rotate.
		/// </param>
		void Step(Int32 steps);
	}
}

