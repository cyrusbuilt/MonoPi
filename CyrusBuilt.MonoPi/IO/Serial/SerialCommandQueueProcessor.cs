//
//  SerialCommandQueueProcessor.cs
//
//  Author:
//       Chris.Brunner <>
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

namespace CyrusBuilt.MonoPi.IO.Serial
{
	/// <summary>
	/// Serial command queue processor.
	/// </summary>
	public class SerialCommandQueueProcessor : IDisposable
	{
		#region Fields
		/// <summary>
		/// The default amount of time (in milliseconds) to delay between
		/// processing commands.
		/// </summary>
		public const Int32 DEFAULT_DELAY = 100;

		private Rs232SerialPort _serial = null;
		private Thread _processor = null;
		private Queue<String> _queue = null;
		private Int32 _delay = 0;
		private Boolean _isDisposed = false;
		private volatile Boolean _exiting = false;
		private static readonly Object _padLock = new Object();
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/>
		/// class with the serial port to queue commands for and the delay
		/// between processing of commands.
		/// </summary>
		/// <param name="serial">
		/// The serial port to queue commands for.
		/// </param>
		/// <param name="delay">
		/// The amount of time (in milliseconds) to delay between commands.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="serial"/> cannot be null.
		/// </exception>
		public SerialCommandQueueProcessor(Rs232SerialPort serial, Int32 delay) {
			if (serial == null) {
				throw new ArgumentNullException("serial");
			}
			this._queue = new Queue<String>();
			this._serial = serial;
			this._delay = delay;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/>
		/// class with the serial port to queue commands for. This overload
		/// uses the default delay value.
		/// </summary>
		/// <param name="serial">
		/// The serial port to queue commands for.
		/// </param>
		public SerialCommandQueueProcessor(Rs232SerialPort serial)
			: this(serial, DEFAULT_DELAY) {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (!this._exiting) {
				lock (_padLock) {
					this._exiting = true;
				}
			}

			if (this._queue != null) {
				this._queue.Clear();
			}

			if (this._processor != null) {
				try {
					if (this._processor.IsAlive) {
						this._processor.Abort();
					}
				}
				catch (ThreadAbortException) {
					Thread.ResetAbort();
				}
			}

			if (this._serial != null) {
				this._serial.Dispose();
				this._serial = null;
			}

			this._processor = null;
			this._queue = null;
			this._isDisposed = true;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enqueue the specified serial command data.
		/// </summary>
		/// <param name="data">
		/// The command data to enqueue.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="data"/> cannot be null.
		/// </exception>
		public void Enqueue(String data) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor");
			}

			if (String.IsNullOrEmpty(data)) {
				throw new ArgumentNullException("data");
			}

			lock (this._queue) {
				this._queue.Enqueue(data);
			}
		}

		/// <summary>
		/// Stops processing the command queue.
		/// </summary>
		public void Stop() {
			lock (_padLock) {
				this._exiting = true;
			}
		}

		/// <summary>
		/// Processes the queue.
		/// </summary>
		private void ProcessQueue() {
			String cmdData = String.Empty;
			while (!this._exiting) {
				if (this._queue.Count > 0) {
					try {
						lock (this._queue) {
							cmdData = this._queue.Dequeue();
						}

						if (this._serial.IsOpen) {
							this._serial.PutString(cmdData);
						}
						Thread.Sleep(this._delay);
					}
					catch (ThreadInterruptedException) {
					}
				}
			}
		}

		/// <summary>
		/// Starts processing the command queue.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void Start() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.IO.Serial.SerialCommandQueueProcessor");
			}

			if (this._processor == null) {
				this._processor = new Thread(new ThreadStart(this.ProcessQueue));
				this._processor.Name = "SerialCmdQueueRunnerThread";
				this._processor.IsBackground = true;
			}
			this._processor.Start();
		}
		#endregion
	}
}

