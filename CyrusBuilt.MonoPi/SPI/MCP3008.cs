//
//  MCP3008.cs
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
//  by Aaron Anderson <aanderson@netopia.ca>
//
//  Original author: Mikey Sklar - git://gist.github.com/3249416.git
//  Adafruit article: http://learn.adafruit.com/reading-a-analog-in-and-controlling-audio-volume-with-the-raspberry-pi
//  Ported from python and modified by: Gilberto Garcia <ferraripr@gmail.com>;
//  twitter: @ferraripr
//
using System;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.SPI
{
	/// <summary>
	/// Raspberry Pi using MCP3008 A/D Converters with SPI Serial Interface
	/// See also http://ww1.microchip.com/downloads/en/DeviceDoc/21295d.pdf
	/// </summary>
	public class MCP3008 : IDisposable
	{
		#region Fields
		private Boolean _isDisposed = false;
		private AdcChannels _channel = AdcChannels.None;
		private IRaspiGpio _clock = null;
		private IRaspiGpio _masterOutSlaveIn = null;
		private IRaspiGpio _masterInSlaveOut = null;
		private IRaspiGpio _chipSelect = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/>
		/// class the analog-to-digital channel, clock pin, SPI Master Output/
		/// Slave Input (MOSI), SPI Master Input/Slave Output (MISO), and SPI
		/// chip select pin.
		/// </summary>
		/// <param name="channel">
		/// MCP3008 channel number 0 - 7 (pin 1 -8 on chip).
		/// </param>
		/// <param name="spiclk">
		/// SPI clock pin.
		/// </param>
		/// <param name="mosi">
		/// Master Output, Slave Input (MOSI).
		/// </param>
		/// <param name="miso">
		/// Master Input, Slave Output (MISO).
		/// </param>
		/// <param name="cs">
		/// Chip Select pin.
		/// </param>
		public MCP3008(AdcChannels channel, IRaspiGpio spiclk, IRaspiGpio mosi, IRaspiGpio miso, IRaspiGpio cs) {
			this._channel = channel;
			this._clock = spiclk;
			this._masterOutSlaveIn = mosi;
			this._masterInSlaveOut = miso;
			this._chipSelect = cs;
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
		/// Gets the channel.
		/// </summary>
		public AdcChannels Channel {
			get { return this._channel; }
		}

		/// <summary>
		/// Gets the SPI clock pin.
		/// </summary>
		public IRaspiGpio SpiClock {
			get { return this._clock; }
		}

		/// <summary>
		/// Gets the master out, slave in (MOSI) pin.
		/// </summary>
		public IRaspiGpio MasterOutSlaveIn {
			get { return this._masterOutSlaveIn; }
		}

		/// <summary>
		/// Gets the master in, slave out (MISO) pin.
		/// </summary>
		public IRaspiGpio MasterInSlaveOut {
			get { return this._masterInSlaveOut; }
		}

		/// <summary>
		/// Gets the chip select.
		/// </summary>
		public IRaspiGpio ChipSelect {
			get { return this._chipSelect; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Performs the Analog-to-Digital conversion.
		/// </summary>
		/// <returns>
		/// The converted value.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and is no longer usable.
		/// </exception>
		public Int32 ReadADC() {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.SPI.MCP3008");
			}

			if (this._channel == AdcChannels.None) {
				return -1;
			}

			this._chipSelect.Write(true);
			this._clock.Write(false);       // Drive clock pin low.
			this._chipSelect.Write(false);  // Drive chip-select low.

			Int32 cmd = (Int32)this._channel;
			cmd |= 0x18;                    // Start bit + single-ended bit.
			cmd <<= 3;                      // We only need to send 5 bits here.

			for (Int32 i = 0; i < 5; i++) {
				if ((cmd & 0x80) == 128) {
					this._masterOutSlaveIn.Write(true);
				}
				else {
					this._masterOutSlaveIn.Write(false);
				}
				cmd <<= 1;
				this._clock.Write(true);
				this._clock.Write(false);
			}

			// Read one empty bit, one null bit, and 10 ADC bits.
			Int32 adcout = 0;
			for (Int32 j = 0; j < 12; j++) {
				this._clock.Write(true);
				this._clock.Write(false);
				adcout <<= 1;
				if (this._masterInSlaveOut.Read()) {
					adcout |= 0x1;
				}
			}

			this._chipSelect.Write(true);
			adcout /= 2;                    // First bit is 'null' so drop it.
			return adcout;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/>
		/// so the garbage collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.SPI.MCP3008"/> was occupying.
		/// </remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			this._channel = AdcChannels.None;
			if (this._chipSelect != null) {
				this._chipSelect.Dispose();
				this._chipSelect = null;
			}

			if (this._clock != null) {
				this._clock.Dispose();
				this._clock = null;
			}

			if (this._masterInSlaveOut != null) {
				this._masterInSlaveOut.Dispose();
				this._masterInSlaveOut = null;
			}

			if (this._masterOutSlaveIn != null) {
				this._masterOutSlaveIn.Dispose();
				this._masterOutSlaveIn = null;
			}
			this._isDisposed = true;
		}
		#endregion
	}
}

