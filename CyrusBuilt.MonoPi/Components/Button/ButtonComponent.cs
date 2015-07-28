//
//  ButtonComponent.cs
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
using System.Threading;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Button
{
	/// <summary>
	/// A component that is an abstraction of a button. This is an implementation
	/// of <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/>.
	/// </summary>
	public class ButtonComponent : ButtonBase
	{
		#region Fields
		private IGpio _pin = null;
		private Thread _pollThread = null;
		private volatile Boolean _isPolling = false;
		private static readonly Object _syncLock = new Object();
		private const PinState PRESSED_STATE = PinState.High;
		private const PinState RELEASED_STATE = PinState.Low;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/>
		/// class with the pin the button is wired to.
		/// </summary>
		/// <param name="pin">
		/// The input pin the button is wired to.
		/// </param>
		public ButtonComponent(IGpio pin)
			: base() {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}
			this._pin = pin;
			this._pin.Provision();
			this._pin.StateChanged += this.OnPinStateChanged;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/> was occupying.</remarks>
		public override void Dispose() {
			lock (_syncLock) {
				this._isPolling = false;
			}

			if ((this._pollThread != null) && (this._pollThread.IsAlive)) {
				try {
					Thread.Sleep(50);
					this._pollThread.Abort();
				}
				catch (ThreadAbortException) {
					Thread.ResetAbort();
				}
				finally {
					this._pollThread = null;
				}
			}

			if (this._pin != null) {
				this._pin.Dispose();
				this._pin = null;
			}

			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public override ButtonState State {
			get {
				if (this._pin.State == PRESSED_STATE) {
					return ButtonState.Pressed;
				}
				return ButtonState.Released;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is polling.
		/// </summary>
		public Boolean IsPolling {
			get { return this._isPolling; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the pin state changed event.
		/// </summary>
		/// <param name="sender">
		/// The object raising the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void OnPinStateChanged(Object sender, PinStateChangeEventArgs e) {
			if (e.NewState != e.OldState) {
				base.OnStateChanged(new ButtonEventArgs(this));
			}
		}

		/// <summary>
		/// Executes the poll cycle. Does not return until
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent.InterruptPoll"/>
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
			lock (_syncLock) {
				this._isPolling = true;
			}

			if ((this._pollThread == null) || (!this._pollThread.IsAlive)) {
				this._pollThread = new Thread(new ThreadStart(this.ExecutePoll));
				this._pollThread.IsBackground = true;
				this._pollThread.Name = "ButtonStatePollExecutive";
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
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Button.ButtonComponent");
			}

			if (this._pin.Mode != PinMode.IN) {
				throw new InvalidOperationException("The specified pin is not configured as an input pin," +
					" which cannot be used to read switch states.");
			}

			lock (_syncLock) {
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
			lock (_syncLock) {
				if (!this._isPolling) {
					return;
				}
				this._isPolling = false;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonComponent"/>.
		/// </returns>
		public override String ToString() {
			return base.Name;
		}
		#endregion
	}
}

