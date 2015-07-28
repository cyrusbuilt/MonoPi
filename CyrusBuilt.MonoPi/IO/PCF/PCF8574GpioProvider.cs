//
//  PCF8574GpioProvider.cs
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
using System.Collections.Generic;
using System.Threading;
using CyrusBuilt.MonoPi.IO.I2C;

namespace CyrusBuilt.MonoPi.IO.PCF
{
	/// <summary>
	/// This GPIO provider implements the PCF8574 I2C GPIO expansion board as
	/// native Pi4J GPIO pins. More information about the board can be found here:
	/// http://www.ti.com/lit/ds/symlink/pcf8574.pdf
	/// <br/><br/>
	/// The PCF8574 is connected via I2C connection to the Raspberry Pi and provides
	/// 8 GPIO pins that can be used for either digital input or digital output pins.
	/// </summary>
	public class PCF8574GpioProvider : IDisposable
	{
		#region Constants
		/// <summary>
		/// The name of this provider.
		/// </summary>
		public const String NAME = "CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider";

		/// <summary>
		/// A description of this provider.
		/// </summary>
		public const String DESCRIPTION = "TI PCF8574 GPIO Provider";

		/// <summary>
		/// The maximum number of available I/O pins on the PCF8574 (8).
		/// </summary>
		public const Int32 MAX_IO_PINS = 8;
		#endregion

		#region Fields
		private Boolean _isDisposed = false;
		private II2CBus _device = null;
		private Int32 _busAddress = -1;
		private List<IPCF8574Pin> _pinCache = null;
		private BitSet _currentStates = null;
		private Thread _pollThread = null;
		private volatile Boolean _pollAbort = false;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a pin state changes.
		/// </summary>
		public PinStateChangeEventHandler PinStateChanged;

		/// <summary>
		/// Occurs if device polling fails.
		/// </summary>
		public PinPollFailEventHandler PinPollFailed;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/>
		/// class with the I2C bus device that is the connection to the PCF8574,
		/// and the bus address of the device.
		/// </summary>
		/// <param name="device">
		/// The I2C bus device that is the connection to the PCF8574.
		/// </param>
		/// <param name="address">
		/// The bus address of the device.
		/// </param>
		public PCF8574GpioProvider(II2CBus device, Int32 address) {
			if (device == null) {
				throw new ArgumentNullException("device");
			}

			this._device = device;
			if (!this._device.IsOpen) {
				this._device.Open();
			}

			this._busAddress = address;
			this._pinCache = new List<IPCF8574Pin>();
			this._currentStates = new BitSet(MAX_IO_PINS);

			foreach (IPCF8574Pin pin in PCF8574Pin.ALL) {
				this._pinCache.Add(pin);
				this._pinCache[this._pinCache.IndexOf(pin)].SetState(PinState.High);
				this._currentStates.Set(pin.Address, true);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/>
		/// class with the bus address of the device. This overload assumes
		/// a Rev 2 or higher board and creates a default I2C connection
		/// instance.
		/// </summary>
		/// <param name="address">
		/// The bus address of the device.
		/// </param>
		public PCF8574GpioProvider(Int32 address)
			: this(new I2CBus(BoardRevision.Rev2), address) {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			// Try to abort gracefully first.
			lock (_syncLock) {
				this._pollAbort = true;
			}

			Thread.Sleep(100);
			if ((this._pollThread != null) && (this._pollThread.IsAlive)) {
				try {
					// Graceful abort did not complete in time. Force terminate.
					this._pollThread.Abort();
				}
				catch (ThreadAbortException) {
					Thread.ResetAbort();
				}
				finally {
					this._pollThread = null;
				}
			}

			// Cleanup everything else.
			if (this._device != null) {
				this._device.Dispose();
				this._device = null;
			}

			if (this._pinCache != null) {
				this._pinCache.Clear();
				this._pinCache = null;
			}

			if (this._currentStates != null) {
				this._currentStates.Clear();
				this._currentStates = null;
			}

			this._isDisposed = true;
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is polling.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is polling; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPolling {
			get {
				lock (_syncLock) {
					return !this._pollAbort;
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the mode of the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to alter.
		/// </param>
		/// <param name="mode">
		/// The mode to set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified pin does not exist in the pin cache.
		/// </exception>
		public void SetPinMode(IPCF8574Pin pin, PinMode mode) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider");
			}
				
			if (!this._pinCache.Contains(pin)) {
				throw new ArgumentException("Cannot set the mode of a pin that does not exist in the pin cache.", "pin");
			}
			this._pinCache[this._pinCache.IndexOf(pin)].SetMode(mode);
		}

		/// <summary>
		/// Gets the mode of the specified pin.
		/// </summary>
		/// <param name="pin">
		/// A pin in the pin cache to get the mode from.
		/// </param>
		/// <returns>
		/// The current pin mode.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified pin does not exist in the pin cache.
		/// </exception>
		public PinMode GetPinMode(IPCF8574Pin pin) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider");
			}

			if (!this._pinCache.Contains(pin)) {
				throw new ArgumentException("The specified pin does not exist in the pin cache.", "pin");
			}
			return this._pinCache[this._pinCache.IndexOf(pin)].Mode;
		}

		/// <summary>
		/// Sets the state of the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to alter.
		/// </param>
		/// <param name="state">
		/// The state of the pin to set.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified pin does not exist in the pin cache.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// Failed to write the new state to the device.
		/// </exception>
		public void SetPinState(IPCF8574Pin pin, PinState state) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider");
			}

			if (!this._pinCache.Contains(pin)) {
				throw new ArgumentException("Cannot set the state of a pin that does not exist in the pin cache.", "pin");
			}

			// We only do this if the state is actually changing.
			PinState cstate = this._pinCache[this._pinCache.IndexOf(pin)].State;
			if (cstate != state) {
				Byte stateVal = this._currentStates.Empty ? (Byte)0 : this._currentStates.ToByteArray()[0];
				this._currentStates.Set(pin.Address, (state == PinState.High));
				this._device.WriteByte(this._busAddress, stateVal);
				this.OnPinStateChanged(new PinStateChangeEventArgs(pin.Address, cstate, state));
			}
		}

		/// <summary>
		/// Gets the current state of the specified pin in the pin cache.
		/// </summary>
		/// <param name="pin">
		/// The pin to get the state from.
		/// </param>
		/// <returns>
		/// The state of the pin.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified pin does not exist in the pin cache.
		/// </exception>
		public PinState GetPinState(IPCF8574Pin pin) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider");
			}

			if (!this._pinCache.Contains(pin)) {
				throw new ArgumentException("The specified pin does not exist in the pin cache.", "pin");
			}
			return this._pinCache[this._pinCache.IndexOf(pin)].State;
		}

		/// <summary>
		/// Raises the <see cref="PinStateChanged"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPinStateChanged(PinStateChangeEventArgs e) {
			if (this.PinStateChanged != null) {
				this.PinStateChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="PinPollFailed"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPinPollFailed(PinPollFailEventArgs e) {
			if (this.PinPollFailed != null) {
				this.PinPollFailed(this, e);
			}
		}

		/// <summary>
		/// Polls all the pins on the PCF8574 every ~50ms and refreshes their states.
		/// This will fire a state change event for any and all pins that have changed
		/// state since the last check. This will also fire a failure event if any
		/// exceptions are thrown.
		/// </summary>
		private void ExecutePoll() {
			while (!this._pollAbort) {
				try {
					// Read device pin states.
					Byte[] buffer = this._device.ReadBytes(this._busAddress, 1);
					BitSet pinStates = BitSet.ValueOf(buffer);

					// Determine if there is a pin state difference.
					IPCF8574Pin pin = null;
					PinState newState = PinState.Low;
					PinState oldState = PinState.Low;
					for (Int32 index = 0; index < pinStates.Size; index++) {
						if (pinStates.Get(index) != this._currentStates.Get(index)) {
							pin = PCF8574Pin.ALL[index];

							// Is the state actually changing?
							newState = pinStates.Get(index) ? PinState.High : PinState.Low;
							oldState = this._pinCache[this._pinCache.IndexOf(pin)].State;
							if (newState != oldState) {
								// Cache the new state.
								this._pinCache[this._pinCache.IndexOf(pin)].SetState(newState);
								this._currentStates.Set(index, pinStates.Get(index));

								// Only dispatch events for input pins.
								if (this.GetPinMode(pin) == PinMode.IN) {
									this.OnPinStateChanged(new PinStateChangeEventArgs(pin.Address, oldState, newState));
								}
							}
						}
					}

					Thread.Sleep(50);
				}
				catch (Exception ex) {
					// If we failed (likely due to an I/O error), then
					// fire the failure event and gracefully abort the
					// the poll thread.
					this.OnPinPollFailed(new PinPollFailEventArgs(ex));
					lock (_syncLock) {
						this._pollAbort = true;
					}
				}
			}
		}

		/// <summary>
		/// Executes the poll cycle on a background thread.
		/// </summary>
		private void BackgroundExecutePoll() {
			lock (_syncLock) {
				this._pollAbort = false;
			}

			if ((this._pollThread == null) || (!this._pollThread.IsAlive)) {
				this._pollThread = new Thread(new ThreadStart(this.ExecutePoll));
				this._pollThread.IsBackground = true;
				this._pollThread.Name = "PCF8574_Pin_Poll_Executive";
				this._pollThread.Start();
			}
		}

		/// <summary>
		/// Polls the state of the all the pins once every ~50ms.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		public void Poll() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.PCF.PCF8574GpioProvider");
			}

			lock (_syncLock) {
				if (!this._pollAbort) {
					return;
				}
			}

			this.BackgroundExecutePoll();
		}

		/// <summary>
		/// Interrupts a poll cycle (if running).
		/// </summary>
		public void InterruptPoll() {
			lock (_syncLock) {
				if (!this._pollAbort) {
					return;
				}
				this._pollAbort = true;
			}
		}
		#endregion
	}
}

