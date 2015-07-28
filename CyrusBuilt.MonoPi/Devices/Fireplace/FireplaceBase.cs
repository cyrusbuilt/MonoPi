//
//  FireplaceBase.cs
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
using System.Threading.Tasks;
using CyrusBuilt.MonoPi.PiSystem;

namespace CyrusBuilt.MonoPi.Devices.Fireplace
{
	/// <summary>
	/// Base class for fireplace device abstractions.
	/// </summary>
	public abstract class FireplaceBase : DeviceBase, IFireplace
	{
		#region Fields
		private double _timeoutDelay = 0;
		private TimeUnit _timeoutUnit = TimeUnit.Minutes;
		private CancellationTokenSource _cts = null;
		private CancellationToken _token = CancellationToken.None;
		private Task _backgroundTask = null;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a state change occurs.
		/// </summary>
		public event FireplaceStateChangedEventHandler StateChanged;

		/// <summary>
		/// Occurs when an operation times out.
		/// </summary>
		public event FireplaceTimeoutEventHandler OperationTimedOut;

		/// <summary>
		/// Occurs when the pilot light state changes.
		/// </summary>
		public event PilotLightEventHandler PilotLightStateChanged;
		#endregion
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected FireplaceBase()
			: base() {
			this._cts = new CancellationTokenSource();
			this._token = this._cts.Token;
			this.StateChanged += this.InternalStateChangeHandler;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Devices.Fireplace.FireplaceBase"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}
				
			if (this._cts != null) {
				if ((!this._cts.IsCancellationRequested) &&
				    (this._token.CanBeCanceled)) {
					this._cts.Cancel();
				}
					
				this._cts.Dispose();
				this._cts = null;
				this._token = CancellationToken.None;
			}

			if (this._backgroundTask != null) {
				this._backgroundTask.Dispose();
				this._backgroundTask = null;
			}

			this.StateChanged = null;
			this.OperationTimedOut = null;
			this.PilotLightStateChanged = null;
			base.Dispose();
		}
		#region Properties
		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public abstract FireplaceState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether the fireplace is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if the fireplace is on; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOn {
			get { return this.State == FireplaceState.On; }
		}

		/// <summary>
		/// Gets a value indicating whether the fireplace is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if the fireplace is off; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOff {
			get { return this.State == FireplaceState.Off; }
		}

		/// <summary>
		/// Gets a value indicating whether the pilot light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is on; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsPilotLightOn { get; }

		/// <summary>
		/// Gets a value indicating whether pilot light is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is off; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsPilotLightOff { get; }

		/// <summary>
		/// Gets the timeout delay.
		/// </summary>
		/// <value>
		/// The timeout delay.
		/// </value>
		public double TimeoutDelay {
			get { return this._timeoutDelay; }
		}

		/// <summary>
		/// Gets the timeout unit of time.
		/// </summary>
		/// <value>
		/// The timeout unit of time.
		/// </value>
		public TimeUnit TimeoutUnit {
			get { return this._timeoutUnit; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(FireplaceStateChangedEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the operation timed out event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnOperationTimedOut(FireplaceTimeoutEventArgs e) {
			if (this.OperationTimedOut != null) {
				this.OperationTimedOut(this, e);
			}
		}

		/// <summary>
		/// Raises the pilot light state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPilotLightStateChanged(FireplacePilotLightEventArgs e) {
			if (this.PilotLightStateChanged != null) {
				this.PilotLightStateChanged(this, e);
			}
		}

		/// <summary>
		/// An internal handler for the state change event.
		/// </summary>
		/// <param name="sender">
		/// The object raising the event (ourself).
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void InternalStateChangeHandler(Object sender, FireplaceStateChangedEventArgs e) {
			this.CancelTimeoutTask();
		}

		/// <summary>
		/// Sets the timeout delay.
		/// </summary>
		/// <param name="delay">
		/// The timeout delay.
		/// </param>
		/// <param name="unit">
		/// The time unit of measure for the timeout.
		/// </param>
		public void SetTimeout(double delay, TimeUnit unit) {
			if (this.IsOff) {
				throw new InvalidOperationException("Cannot set timeout when fireplace is off.");
			}

			this._timeoutDelay = delay;
			this._timeoutUnit = unit;

			this.CancelTimeoutTask();

			if (this._timeoutDelay > 0) {
				TimeSpan waitTime = TimeSpan.Zero;
				switch (unit) {
					case TimeUnit.Days:
						waitTime = TimeSpan.FromDays(delay);
						break;
					case TimeUnit.Hours:
						waitTime = TimeSpan.FromHours(delay);
						break;
					case TimeUnit.Minutes:
						waitTime = TimeSpan.FromMinutes(delay);
						break;
					case TimeUnit.Seconds:
						waitTime = TimeSpan.FromSeconds(delay);
						break;
					case TimeUnit.Milliseconds:
						waitTime = TimeSpan.FromMilliseconds(delay);
						break;
					default:
						break;
				}

				Action a = new Action(delegate {
					this._token.WaitHandle.WaitOne(waitTime);

					FireplaceTimeoutEventArgs evt = new FireplaceTimeoutEventArgs();
					this.OnOperationTimedOut(evt);

					if (!evt.IsHandled) {
						this.Off();
					}
				});

				TimeSpan killDelay = TimeSpan.FromTicks(waitTime.Ticks).Add(TimeSpan.FromSeconds(1));
				Timer killTimer = new Timer(state => {
					this._cts.Cancel();
				}, null, killDelay.Milliseconds, -1);

				this._backgroundTask = Task.Factory.StartNew(a);
				this._backgroundTask.ContinueWith(completed => {
					killTimer.Dispose();
				});
			}
		}

		/// <summary>
		/// Turns the fireplace on.
		/// </summary>
		public void On() {
			this.State = FireplaceState.On;
		}

		/// <summary>
		/// Turns the fireplace on with the specified timeout. If the operation
		/// is not successful within the allotted time, the operation is
		/// cancelled for safety reasons.
		/// </summary>
		/// <param name="timeoutDelay">
		/// The timeout delay.
		/// </param>
		/// <param name="timeoutUnit">
		/// The time unit of measure for the timeout.
		/// </param>
		public void On(double timeoutDelay, TimeUnit timeoutUnit) {
			this.On();
			this.SetTimeout(timeoutDelay, timeoutUnit);
		}

		/// <summary>
		/// Turns the fireplace off.
		/// </summary>
		public void Off() {
			this.State = FireplaceState.Off;
		}

		/// <summary>
		/// Cancels the timeout task (if running).
		/// </summary>
		protected void CancelTimeoutTask() {
			lock (_syncLock) {
				if ((this._cts != null) && (this._token != null)) {
					if ((!this._cts.IsCancellationRequested) &&
						(this._token.CanBeCanceled)) {
						this._cts.Cancel();
					}
				}
			}
		}

		/// <summary>
		/// Cancels a timeout.
		/// </summary>
		public void CancelTimeout() {
			this.CancelTimeoutTask();
		}

		/// <summary>
		/// Shutdown the fireplace.
		/// </summary>
		public void Shutdown() {
			this.CancelTimeoutTask();
			this.Off();
		}

		/// <summary>
		/// Starts the cancel task. This will start a task that turns off
		/// 
		/// </summary>
		protected void StartCancelTask() {
			lock (_syncLock) {
				if (this._backgroundTask != null) {
					if ((!this._backgroundTask.IsCanceled) &&
					    (!this._backgroundTask.IsCompleted) &&
					    (!this._backgroundTask.IsFaulted)) {
						this._backgroundTask.Wait();
					}
				}
			}
		}
		#endregion
	}
}

