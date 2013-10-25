//
//  ToggleSwitchComponent.cs
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

namespace CyrusBuilt.MonoPi.Components.Switches
{
	/// <summary>
	/// A component that is an abstraction of a toggle switch. This is an
	/// implementation of <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchBase"/>.
	/// </summary>
	public class ToggleSwitchComponent : ToggleSwitchBase
	{
		#region Fields
		private GpioBase _pin = null;
		private Boolean _isPolling = false;
		private Thread _pollThread = null;
		private const PinState OFF_STATE = PinState.Low;
		private const PinState ON_STATE = PinState.High;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/>
		/// class with the pin the switch is wired to.
		/// </summary>
		/// <param name="pin">
		/// The input pin to check switch state on.
		/// </param>
		public ToggleSwitchComponent(GpioBase pin)
			: base() {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
			this._pin.StateChanged += this.OnStateChanged;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/>. The
		/// <see cref="Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/> in an
		/// unusable state. After calling <see cref="Dispose"/>, you must release all references
		/// to the <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/> so
		/// the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (this._pin != null) {
				this._pin.Dispose();
				this._pin = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the current state of the switch.
		/// </summary>
		public override SwitchState State {
			get {
				if (this._pin.State == ON_STATE) {
					return SwitchState.On;
				}
				return SwitchState.Off;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="sender">
		/// The object firing the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void OnStateChanged(Object sender, PinStateChangeEventArgs e) {
			if (e.NewState != e.OldState) {
				SwitchStateChangeEventArgs changeArgs = null;
				if (e.NewState == ON_STATE) {
					changeArgs = new SwitchStateChangeEventArgs(SwitchState.Off, SwitchState.On);
				}
				else {
					changeArgs = new SwitchStateChangeEventArgs(SwitchState.On, SwitchState.Off);
				}

				base.OnSwitchStateChanged(changeArgs);
			}
		}

		/// <summary>
		/// Executes the poll cycle. Does not return until
		/// <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent.InterruptPoll"/>
		/// is called.
		/// </summary>
		private void ExecutePoll() {
			while (this._isPolling) {
				// Executing a read on the pin will trigger a state change event.
				this._pin.Read();
				Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Executes the poll cycle on a background thread.
		/// </summary>
		private void BackgroundExecutePoll() {
			lock (this) {
				this._isPolling = true;
			}

			if ((this._pollThread == null) || (!this._pollThread.IsAlive)) {
				this._pollThread = new Thread(new ThreadStart(this.ExecutePoll));
				this._pollThread.IsBackground = true;
				this._pollThread.Name = "ToggleSwitchStatePollExecutive";
				this._pollThread.Start();
			}
		}

		/// <summary>
		/// Polls the input pin status.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The specified pin is configured for output instead of input.
		/// </exception>
		public void Poll() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent");
			}

			if (this._pin.Direction == PinDirection.OUT) {
				throw new InvalidOperationException("The specified pin is configured as an output pin," +
				                                    " which cannot be used to read switch states.");
			}

			lock (this) {
				if (this._isPolling) {
					return;
				}
			}
			this.BackgroundExecutePoll();
		}

		/// <summary>
		/// Interrupts the poll cycle.
		/// </summary>
		public void InterruptPoll() {
			lock (this) {
				if (!this._isPolling) {
					return;
				}
				this._isPolling = false;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Switches.ToggleSwitchComponent"/>.
		/// </returns>
		public override String ToString() {
			return base.Name;
		}
		#endregion
	}
}

