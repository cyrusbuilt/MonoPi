//
//  StepperMotorComponent.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Motors
{
	/// <summary>
	/// A component that is an abstraction of a stepper motor.
	/// </summary>
	public class StepperMotorComponent : StepperMotorBase
	{
		#region Fields
		private volatile MotorState _state = MotorState.Stop;
		private Int32 _sequenceIndex = 0;
		private Thread _controlThread = null;
		private IRaspiGpio[] _pins = null;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/>
		/// class with the output pins for each controller in the stepper motor.
		/// </summary>
		/// <param name="pins">
		/// The output pins for each controller in the stepper motor.
		/// </param>
		public StepperMotorComponent(IRaspiGpio[] pins)
			: base() {
			this._pins = pins;
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected override void Dispose(Boolean disposing) {
			if (disposing) {
				this._sequenceIndex = 0;
				this.State = MotorState.Stop;

				if ((this._controlThread != null) && (this._controlThread.IsAlive)) {
					try {
						Thread.Sleep(50);
						this._controlThread.Abort();
					}
					catch (ThreadAbortException) {
						Thread.ResetAbort();
					}
					finally {
						this._controlThread = null;
					}
				}

				if (this._pins != null) {
					if (this._pins.Length > 0) {
						foreach (IRaspiGpio pin in this._pins) {
							pin.Dispose();
						}
						Array.Clear(this._pins, 0, this._pins.Length);
					}
					this._pins = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Releases all resources used by the <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/> was occupying.
		/// </remarks>
		public void Dipose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state of the motor.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed. Only thrown if trying to set the state
		/// after this instance has been disposed.
		/// </exception>
		public override MotorState State {
			get { return this._state; }
			set {
				if (base.IsDisposed) {
					throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent");
				}

				MotorState oldState = this._state;
 				if (this._state != value) {
 					lock (this) {
						this._state = value;
					}
					base.OnMotorStateChanged(new MotorStateChangeEventArgs(oldState, this._state));
					this.ExecuteMovement();
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Steps the the motor forward or backward.
		/// </summary>
		/// <param name="forward">
		/// Set <c>true</c> if moving forward.
		/// </param>
		private void DoStep(Boolean forward) {
			// Increment or decrement sequence.
			if (forward) {
				this._sequenceIndex++;
			}
			else {
				this._sequenceIndex--;
			}

			// Check sequence bounds; rollover if needed.
			if (this._sequenceIndex >= base.StepSequence.Length) {
				this._sequenceIndex = 0;
			}
			else if (this._sequenceIndex < 0) {
					this._sequenceIndex = (base.StepSequence.Length - 1);
			}

			// Start cycling through GPIO pins to move the motor forward or revers.
			for (Int32 i = 0; i < this._pins.Length; i++) {
				// Apply step sequence.
				Double nib = Math.Pow(2, i);
				if ((base.StepSequence[this._sequenceIndex] & (Int32)nib) > 0) {
					this._pins[i].Write(true);
				}
				else {
					this._pins[i].Write(false);
				}
			}

			Thread.Sleep(base.StepIntervalMillis + (base.StepIntervalNanos * 1000000));
		}

		/// <summary>
		/// Moves the motor forward or backward until stopped. This method is
		/// meant to be executed in a background thread.
		/// </summary>
		private void BackgroundExecuteMovement() {
			// Continuous loop until stopped.
			while (this.State != MotorState.Stop) {
				this.DoStep(this.State == MotorState.Forward);
			}

			// Turn all GPIO pins off.
			foreach (IRaspiGpio pin in this._pins) {
				pin.Write(false);
			}
		}

		/// <summary>
		/// Executes the motor movement corresponding to the current motor state.
		/// If stopping, then turns all controller pins off; Otherwise, the forward
		/// or reverse movent is executed in a background thread.
		/// </summary>
		private void ExecuteMovement() {
			lock (_syncLock) {
				if (this.State == MotorState.Stop) {
					foreach (IRaspiGpio pin in this._pins) {
						pin.Write(false);
					}
					return;
				}
			}

			if ((this._controlThread == null) || (!this._controlThread.IsAlive)) {
				this._controlThread = new Thread(new ThreadStart(this.BackgroundExecuteMovement));
				this._controlThread.IsBackground = true;
				this._controlThread.Name = "MotorMovementExecutive";
				this._controlThread.Start();
			}
		}

		/// <summary>
		/// Step the motor the specified number of steps.
		/// </summary>
		/// <param name="steps">
		/// The number of steps to step the motor forward or backward. Set 0
		/// to stop the motor.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public override void Step(Int32 steps) {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent");
			}

			if (steps == 0) {
				this.State = MotorState.Stop;
				return;
			}

			// Perform step in positive or negative direction from current position.
			base.OnMotorRotationStarted(new MotorRotateEventArgs(steps));
			if (steps > 0) {
				for (Int32 i = 1; i <= steps; i++) {
					this.DoStep(true);
				}
			}
			else {
				for (Int32 i = steps; i < 0; i++) {
					this.DoStep(false);
				}
			}

			// Stop movement.
			base.Stop();
			base.OnMotorRotationStopped(EventArgs.Empty);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Motors.StepperMotorComponent"/>.
		/// </returns>
		public override String ToString() {
			return base.Name;
		}
		#endregion
	}
}

