//
//  PiFaceBase.cs
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
using CyrusBuilt.MonoPi.Components.Lights;
using CyrusBuilt.MonoPi.Components.Relays;
using CyrusBuilt.MonoPi.Components.Switches;
using CyrusBuilt.MonoPi.IO;
using CyrusBuilt.MonoPi.SPI;

namespace CyrusBuilt.MonoPi.Devices.PiFace
{
	public abstract class PiFaceBase : DeviceBase, IPiFace
	{
		#region Fields
		private IPiFaceGPIO[] _inputPins = { };
		private IPiFaceGPIO[] _outputPins = { };
		private IRelay[] _relays = { };
		private ISwitch[] _switches = { };
		private ILED[] _leds = { };
		#endregion

		#region Constructors and Destructors
		protected PiFaceBase(AdcChannels channel, Int32 speed)
			: base() {
			SimpleSPI.Init(channel, speed);

			this._inputPins = new IPiFaceGPIO[] {

			};

			this._outputPins = new IPiFaceGPIO[] {

			};

			this._relays = new IRelay[] {
				new RelayComponent(this._outputPins[0]),
				new RelayComponent(this._outputPins[1])
			};

			this._switches = new ISwitch[] {
				new SwitchComponent(this._inputPins[0]),
				new SwitchComponent(this._inputPins[1]),
				new SwitchComponent(this._inputPins[2]),
				new SwitchComponent(this._inputPins[3])
			};

			this._leds = new ILED[] {
				new LEDComponent(this._outputPins[0]),
				new LEDComponent(this._outputPins[1]),
				new LEDComponent(this._outputPins[2]),
				new LEDComponent(this._outputPins[3]),
				new LEDComponent(this._outputPins[4]),
				new LEDComponent(this._outputPins[5]),
				new LEDComponent(this._outputPins[6]),
				new LEDComponent(this._outputPins[7])
			};
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceBase"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceBase"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._inputPins != null) {
				if (this._inputPins.Length > 0) {
					foreach (IPiFaceGPIO inpin in this._inputPins) {
						inpin.Dispose();
					}
					Array.Clear(this._inputPins, 0, this._inputPins.Length);
				}
				this._inputPins = null;
			}

			if (this._outputPins != null) {
				if (this._outputPins.Length > 0) {
					foreach (IPiFaceGPIO outpin in this._outputPins) {
						outpin.Dispose();
					}
					Array.Clear(this._outputPins, 0, this._outputPins.Length);
				}
			}

			if (this._relays != null) {
				if (this._relays.Length > 0) {
					foreach (IRelay rel in this._relays) {
						rel.Dispose();
					}
					Array.Clear(this._relays, 0, this._relays.Length);
				}
				this._relays = null;
			}

			if (this._switches != null) {
				if (this._switches.Length > 0) {
					foreach (ISwitch sw in this._switches) {
						sw.Dispose();
					}
					Array.Clear(this._switches, 0, this._switches.Length);
				}
				this._switches = null;
			}

			if (this._leds != null) {
				if (this._leds.Length > 0) {
					foreach (ILED led in this._leds) {
						led.Dispose();
					}
					Array.Clear(this._leds, 0, this._leds.Length);
				}
				this._leds = null;
			}

			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the input pins.
		/// </summary>
		public IPiFaceGPIO[] InputPins {
			get { return this._inputPins; }
		}

		/// <summary>
		/// Gets the output pins.
		/// </summary>
		public IPiFaceGPIO[] OutputPins {
			get { return this._outputPins; }
		}

		/// <summary>
		/// Gets the relays.
		/// </summary>
		public IRelay[] Relays {
			get { return this._relays; }
		}

		/// <summary>
		/// Gets the switches.
		/// </summary>
		public ISwitch[] Switches {
			get { return this._switches; }
		}

		/// <summary>
		/// Gets the LEDs.
		/// </summary>
		public ILED[] LEDs {
			get { return this._leds; }
		}
		#endregion
	}
}

