//
//  DS1620.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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
//  Derived from https://github.com/cypherkey/RaspberryPi.Net
//  by Aaron Anderson <aanderson@netopia.ca>, which is derived from
//  work done by AdamS at 
//  http://forums.netduino.com/index.php?/topic/3335-netduino-plus-and-ds1620-anyone/page__view__findpost__p__22972
//
using System;
using System.Diagnostics;
using System.Threading;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Sensors
{
	/// <summary>
	/// This is a simple driver class for the Dallas Semiconductor DS1620 digital thermometer IC.
	/// </summary>
	public class DS1620 : IDS1620
	{
		#region Fields
		private IRaspiGpio _clock = null;
		private IRaspiGpio _data = null;
		private IRaspiGpio _reset = null;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/>
		/// class with the clock, data, and reset GPIO pins.
		/// </summary>
		/// <param name="clock">
		/// The clock pin.
		/// </param>
		/// <param name="data">
		/// The data pin.
		/// </param>
		/// <param name="reset">
		/// The reset pin.
		/// </param>
		public DS1620(IRaspiGpio clock, IRaspiGpio data, IRaspiGpio reset) {
			this._clock = clock;
			this._data = data;
			this._reset = reset;
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
		/// Gets the clock pin.
		/// </summary>
		public IRaspiGpio Clock {
			get { return this._clock; }
		}

		/// <summary>
		/// Gets the data pin.
		/// </summary>
		public IRaspiGpio Data {
			get { return this._data; }
		}

		/// <summary>
		/// Gets the reset pin.
		/// </summary>
		public IRaspiGpio Reset {
			get { return this._reset; }
		}
		#endregion

		#region Methdods
		/// <summary>
		/// Sends an 8-bit command to the DS1620.
		/// </summary>
		/// <param name="command">
		/// The command to send.
		/// </param>
		private void SendCommand(Int32 command) {
			// Send command on data output, least sig bit first.
			Int32 bit = 0;
			for (Int32 n = 0; n < 8; n++) {
				bit = ((command >> n) & (0x01));
				this._data.Write(bit == 1);
				this._clock.Write(false);
				this._clock.Write(true);
			}
		}

		/// <summary>
		/// Reads 8-bit data from the DS1620.
		/// </summary>
		/// <returns>
		/// The temperature in half degree increments.
		/// </returns>
		private Int32 ReadData() {
			Int32 bit = 0;
			Int32 raw_data = 0; // Go into input mode.
			for (Int32 n = 0; n < 9; n++) {
				this._clock.Write(false);
				bit = Convert.ToInt32(this._data.Read());
				this._clock.Write(true);
				raw_data = raw_data | (bit << n);
			}
			Debug.WriteLine("bin=" + Convert.ToInt32(raw_data.ToString(), 2));
			return raw_data;
		}

		/// <summary>
		/// Sends the commands to get the temperature from the sensor.
		/// </summary>
		/// <returns>
		/// The temperature with half-degree granularity.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		public Double GetTemperature() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Sensors.DS1620");
			}

			this._reset.Write(false);
			this._clock.Write(true);
			this._reset.Write(true);
			this.SendCommand(0x0c);    // write config command.
			this.SendCommand(0x02);    // cpu mode.
			this._reset.Write(false);

			Thread.Sleep(200);         // wait until the configuration register is written.
			this._clock.Write(true);
			this._reset.Write(true);
			this.SendCommand(0xEE);    // start conversion.
			this._reset.Write(false);

			Thread.Sleep(200);
			this._clock.Write(true);
			this._reset.Write(true);
			this.SendCommand(0xAA);
			Int32 raw = this.ReadData();
			this._reset.Write(false);
			return ((Double)raw / 2.0);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/> so the
		/// garbage collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.Sensors.DS1620"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._clock != null) {
				this._clock.Dispose();
				this._clock = null;
			}

			if (this._data != null) {
				this._data.Dispose();
				this._data = null;
			}

			if (this._reset != null) {
				this._reset.Dispose();
				this._reset = null;
			}
			this._isDisposed = true;
		}
		#endregion
	}
}

