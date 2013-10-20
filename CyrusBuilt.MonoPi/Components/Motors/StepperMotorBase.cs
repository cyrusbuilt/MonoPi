//
//  StepperMotorBase.cs
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
	/// A base class for stepper motor components.
	/// </summary>
	public abstract class StepperMotorBase : MotorBase, IStepperMotor
	{
		#region Fields
		private Int32 _stepIntervalMillis = 100;
		private Int32 _stepIntervalNanos = 0;
		private Byte[] _stepSequence = null;
		private Int32 _stepsPerRevolution = 0;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when rotation starts.
		/// </summary>
		public event MotorRotationStartEventHandler RotationStarted;

		/// <summary>
		/// Occurs when rotation stops.
		/// </summary>
		public event MotorRotationStopEventHandler RotationStopped;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected StepperMotorBase()
			: base() {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the steps per revolution.
		/// </summary>
		public Int32 StepsPerRevolution {
			get { return this._stepsPerRevolution; }
			set { this._stepsPerRevolution = value; }
		}

		/// <summary>
		/// Gets or sets the step sequence.
		/// </summary>
		public Byte[] StepSequence {
			get { return this._stepSequence; }
			set { this._stepSequence = value; }
		}

		/// <summary>
		/// Gets the step interval in milliseconds.
		/// </summary>
		public Int32 StepIntervalMillis {
			get { return this._stepIntervalMillis; }
		}

		/// <summary>
		/// Gets the step interval in nanoseconds.
		/// </summary>
		public Int32 StepIntervalNanos {
			get { return this._stepIntervalNanos; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the motor rotation started event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnMotorRotationStarted(MotorRotateEventArgs e) {
			if (this.RotationStarted != null) {
				this.RotationStarted(this, e);
			}
		}

		/// <summary>
		/// Raises the motor rotation stopped event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnMotorRotationStopped(EventArgs e) {
			if (this.RotationStopped != null) {
				this.RotationStopped(this, e);
			}
		}

		/// <summary>
		/// Sets the step interval.
		/// </summary>
		/// <param name="millis">
		/// The milliseconds between steps.
		/// </param>
		public void SetStepInterval(Int32 millis) {
			this._stepIntervalMillis = millis;
			this._stepIntervalNanos = 0;
		}

		/// <summary>
		/// Sets the step interval.
		/// </summary>
		/// <param name="millis">
		/// The milliseconds between steps.
		/// </param>
		/// <param name="nanoseconds">
		/// The nanoseconds between steps.
		/// </param>
		public void SetStepInterval(Int32 millis, Int32 nanoseconds) {
			this._stepIntervalMillis = millis;
			this._stepIntervalNanos = nanoseconds;
		}

		/// <summary>
		/// Step the motor the specified steps.
		/// </summary>
		/// <param name="steps">
		/// The number of steps to rotate.
		/// </param>
		public abstract void Step(Int32 steps);

		/// <summary>
		/// Rotate the specified revolutions.
		/// </summary>
		/// <param name="revolutions">
		/// The number of revolutions to rotate.
		/// </param>
		public void Rotate(Double revolutions) {
			Double steps = Math.Round(this._stepsPerRevolution * revolutions);
			Int32 stepsActual = Convert.ToInt32(steps);
			this.OnMotorRotationStarted(new MotorRotateEventArgs(stepsActual));
			this.Step(stepsActual);
			this.OnMotorRotationStopped(EventArgs.Empty);
		}
		#endregion
	}
}

