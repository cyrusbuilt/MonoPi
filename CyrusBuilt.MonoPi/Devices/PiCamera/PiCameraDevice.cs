//
//  PiCameraDevice.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2014 Copyright (c) 2013 CyrusBuilt
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
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CyrusBuilt.MonoPi.Devices.PiCamera
{
	/// <summary>
	/// An abstraction of the RaspiCam device. RaspiCam is a peripheral
	/// camera device designed specifically for use with the Raspberry Pi.
	/// The class provides a threaded wrapper around the raspistill utility
	/// and thus a means for still capture control.
	/// 
	/// See http://www.raspberrypi.org/wp-content/uploads/2013/07/RaspiCam-Documentation.pdf
	/// for instructions on how to install and configure RaspiCam support.
	/// </summary>
	public class PiCameraDevice : IDisposable
	{
		#region Fields
		private StillCaptureSettings _settings = null;
		private Process _captureProc = null;
		private Thread _captureMonitor = null;
		private Int32 _exitCode = 0;
		private Int32 _pid = -1;
		private volatile Boolean _isRunning = false;
		private Boolean _isDisposed = false;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the capture has started.
		/// </summary>
		public event CaptureStartEventHandler CaptureStarted;

		/// <summary>
		/// Occurs when capture output is recieved.
		/// </summary>
		public event CaptureOutputEventHandler CaptureOutputReceived;

		/// <summary>
		/// Occurs when capture done.
		/// </summary>
		public event CaptureDoneEventHandler CaptureDone;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/>
		/// class. This is the default constructor.
		/// </summary>
		public PiCameraDevice() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/>
		/// class with the image capture settings.
		/// </summary>
		/// <param name="settings">
		/// The image capture settings to use.
		/// </param>
		public PiCameraDevice(StillCaptureSettings settings) {
			this._settings = settings;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._captureProc != null) {
				if (!this._captureProc.HasExited) {
					try {
						this._captureProc.Kill();
					}
					catch {
					}
				}
			}

			if (this._captureMonitor != null) {
				if (this._captureMonitor.IsAlive) {
					try {
						this._captureMonitor.Abort();
					}
					catch (ThreadAbortException) {
					}
				}
				this._captureMonitor = null;
			}

			this._pid = -1;
			this._settings = null;
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
		/// Gets a value indicating whether this instance is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is running; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsRunning {
			get { return this._isRunning; }
		}

		/// <summary>
		/// Gets the process ID.
		/// </summary>
		public Int32 ProcessID {
			get { return this._pid; }
		}

		/// <summary>
		/// Gets the exit code of the underlying process.
		/// </summary>
		public Int32 ExitCode {
			get { return this._exitCode; }
		}

		/// <summary>
		/// Gets or sets the still image capture settings.
		/// </summary>
		public StillCaptureSettings CaptureSettings {
			get { return this._settings; }
			set { this._settings = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice.CaptureStarted"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnCaptureStarted(CaptureStartEventArgs e) {
			if (this.CaptureStarted != null) {
				this.CaptureStarted(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice.CaptureOutputReceived"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnCaptureOutputRecieved(CaptureOutputEventArgs e) {
			if (this.CaptureOutputReceived != null) {
				this.CaptureOutputReceived(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.PiCameraDevice.CaptureDone"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnCaptureDone(CaptureDoneEventArgs e) {
			if (this.CaptureDone != null) {
				this.CaptureDone(this, e);
			}
		}

		/// <summary>
		/// Monitors the capture process.
		/// </summary>
		private void MonitorCapture() {
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.Arguments = this._settings.ToArgumentString();
			psi.CreateNoWindow = true;
			psi.RedirectStandardError = false;
			psi.RedirectStandardInput = false;
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = true;
			psi.FileName = "raspistill";

			try {
				// Start the process and get the PID.
				this._captureProc = new Process();
				this._captureProc.StartInfo = psi;
				this._captureProc.Start();
				lock (_syncLock) {
					this._pid = this._captureProc.Id;
				}

				// Notify listeners that the process started, then
				// start reading the output. Every time we get something
				// back from the process, we notify the output listeners.
				this.OnCaptureStarted(new CaptureStartEventArgs(this._pid));
				String output = this._captureProc.StandardOutput.ReadLine();
				while (output != null) {
					this.OnCaptureOutputRecieved(new CaptureOutputEventArgs(output));
					output = this._captureProc.StandardOutput.ReadLine();
				}

				// Wait for the process to finish (if we go this far, it
				// likely already has) then get the exit code.
				this._captureProc.WaitForExit();
				lock (_syncLock) {
					this._exitCode = this._captureProc.ExitCode;
					this._isRunning = false;
				}

				// Notify listeners that we terminated.
				this.OnCaptureDone(new CaptureDoneEventArgs(this._exitCode));
			}
			catch (InvalidOperationException) {
				// This can happen if the process is cancelled.
			}
			finally {
				if (this._captureProc != null) {
					this._captureProc.Close();
				}
			}
		}

		/// <summary>
		/// Starts the capture process on a separate thread, then
		/// immediately returns will the process continues in the
		/// background.
		/// </summary>
		public void Start() {
			if (this._isRunning) {
				return;
			}

			this._pid = -1;
			this._exitCode = 0;
			this._captureMonitor = new Thread(new ThreadStart(this.MonitorCapture));
			this._captureMonitor.IsBackground = true;
			this._captureMonitor.Name = "raspistillMonitor";
			this._captureMonitor.Start();
			this._isRunning = true;
		}

		/// <summary>
		/// Cancels the still capture process, if running.
		/// </summary>
		public void Cancel() {
			if (!this._isRunning) {
				return;
			}

			// Try to do this gracefully first.
			lock (_syncLock) {
				this._isRunning = false;
			}

			Thread.Sleep(500);
			if (this._captureProc != null) {
				if (!this._captureProc.HasExited) {
					try {
						// Well to hell with it then. Brute force it is.
						this._captureProc.Kill();
					}
					catch {
					}
				}
			}
		}
		#endregion
	}
}

