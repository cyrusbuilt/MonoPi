//
//  MotorBase.cs
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
using System.Threading;

namespace CyrusBuilt.MonoPi.Components.Motors
{
	/// <summary>
	/// Base class for motor abstraction components.
	/// </summary>
	public abstract class MotorBase : ComponentBase, IMotor
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Motors.MotorBase"/>
		/// class. This is the default constructor.
		/// </summary>
		public MotorBase()
			: base() {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state of the motor.
		/// </summary>
		public abstract MotorState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether the motor is stopped.
		/// </summary>
		/// <value>
		/// <c>true</c> if this motor is stopped; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsStopped {
			get { return (this.State == MotorState.Stop); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Stops the motor's movement.
		/// </summary>
		public void Stop() {
			this.State = MotorState.Stop;
		}

		/// <summary>
		/// Tells the motor to move forward.
		/// </summary>
		public void Forward() {
			this.State = MotorState.Stop;
		}

		/// <summary>
		/// Tells the motor to move forward for the specified amount of time.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to continue moving forward for.
		/// </param>
		public void Forward(Int32 millis) {
			this.State = MotorState.Forward;
			Thread.Sleep(millis);
			this.State = MotorState.Stop;
		}

		/// <summary>
		/// Tells the motor to reverse direction.
		/// </summary>
		public void Reverse() {
			this.State = MotorState.Reverse;
		}

		/// <summary>
		/// Tells the motor to reverse direction for the specified amount of time.
		/// </summary>
		/// <param name="millis">
		/// The number of milliseconds to continue moving in reverse for.
		/// </param>
		public void Reverse(Int32 millis) {
			this.State = MotorState.Reverse;
			Thread.Sleep(millis);
			this.State = MotorState.Stop;
		}

		/// <summary>
		/// Determines whether the motor's current state is the specified state.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current state is the specified state; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="state">
		/// The state to check for.
		/// </param>
		public Boolean IsState(MotorState state) {
			return (this.State == state);
		}
		#endregion
	}
}

