//
//  DeviceControlChannel.cs
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

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// This class represents the wiper. It is used for devices knowing
	/// more than one wiper.
	/// </summary>
	public class DeviceControlChannel
	{
		#region Constants
		/// <summary>
		/// Device control channel A.
		/// </summary>
		public static readonly DeviceControlChannel A = new DeviceControlChannel((Byte)RegisterMemoryAddress.WIPER0,
			                                         		(Byte)RegisterMemoryAddress.WIPER0_NV, (Byte)RegisterMemoryAddress.TCON01,
			                                         		(Int32)TerminalControlRegisterBit.TCON_RH02HW, (Int32)TerminalControlRegisterBit.TCON_RH02A,
															(Int32)TerminalControlRegisterBit.TCON_RH02B, (Int32)TerminalControlRegisterBit.TCON_RH02W,
			                                         		MicrochipPotChannel.A);

		/// <summary>
		/// Device control channel B.
		/// </summary>
		public static readonly DeviceControlChannel B = new DeviceControlChannel((Byte)RegisterMemoryAddress.WIPER1,
			                                                (Byte)RegisterMemoryAddress.WIPER1_NV, (Byte)RegisterMemoryAddress.TCON01,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH13HW, (Int32)TerminalControlRegisterBit.TCON_RH13A,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH13B, (Int32)TerminalControlRegisterBit.TCON_RH13W,
			                                                MicrochipPotChannel.B);

		/// <summary>
		/// Device control channel C.
		/// </summary>
		public static readonly DeviceControlChannel C = new DeviceControlChannel((Byte)RegisterMemoryAddress.WIPER2,
			                                                (Byte)RegisterMemoryAddress.WIPER2_NV, (Byte)RegisterMemoryAddress.TCON23,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH02HW, (Int32)TerminalControlRegisterBit.TCON_RH02A,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH02B, (Int32)TerminalControlRegisterBit.TCON_RH02W,
			                                                MicrochipPotChannel.C);

		/// <summary>
		/// Device control channel D.
		/// </summary>
		public static readonly DeviceControlChannel D = new DeviceControlChannel((Byte)RegisterMemoryAddress.WIPER3,
			                                                (Byte)RegisterMemoryAddress.WIPER3_NV, (Byte)RegisterMemoryAddress.TCON23,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH13HW, (Int32)TerminalControlRegisterBit.TCON_RH13A,
			                                                (Int32)TerminalControlRegisterBit.TCON_RH13B, (Int32)TerminalControlRegisterBit.TCON_RH13W,
			                                                MicrochipPotChannel.D);

		/// <summary>
		/// All device control channels.
		/// </summary>
		public static readonly DeviceControlChannel[] ALL = {
			DeviceControlChannel.A,
			DeviceControlChannel.B,
			DeviceControlChannel.C,
			DeviceControlChannel.D
		};
		#endregion

		#region Fields
		private Byte _volatileMemAddr = 0;
		private Byte _nonVolatileMemAddr = 0;
		private Byte _termConAddr = 0;
		private Int32 _hwConfigCtrlBit = 0;
		private Int32 _termAConnCtrlBit = 0;
		private Int32 _termBConnCtrlBit = 0;
		private Int32 _wiperConnCtrlBit = 0;
		private MicrochipPotChannel _chan = MicrochipPotChannel.None;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>
		/// class with the volatile memory address, non-volatile memory address,
		/// terminal control address, hardware-config control bit, terminal A
		/// connection control bit, terminal B connection control bit, wiper
		/// connection control bit, and MCP potentiometer channel.
		/// </summary>
		/// <param name="volatileMemAddr">
		/// The volatile memory address.
		/// </param>
		/// <param name="nonVolatileMemAddr">
		/// The non-volatile memory address.
		/// </param>
		/// <param name="termConAddr">
		/// The terminal control address.
		/// </param>
		/// <param name="hwConfigCtrlBit">
		/// The hardware config control bit.
		/// </param>
		/// <param name="termAConnCtrlBit">
		/// The terminal A connection control bit.
		/// </param>
		/// <param name="termBConnCtrlBit">
		/// The terminal B connection control bit.
		/// </param>
		/// <param name="wiperConnCtrlBit">
		/// The wiper connection control bit.
		/// </param>
		/// <param name="chan">
		/// The MCP potentiometer channel.
		/// </param>
		protected DeviceControlChannel(Byte volatileMemAddr, Byte nonVolatileMemAddr,
										Byte termConAddr, Int32 hwConfigCtrlBit,
										Int32 termAConnCtrlBit, Int32 termBConnCtrlBit,
										Int32 wiperConnCtrlBit, MicrochipPotChannel chan) {
			this._volatileMemAddr = volatileMemAddr;
			this._nonVolatileMemAddr = nonVolatileMemAddr;
			this._termConAddr = termConAddr;
			this._hwConfigCtrlBit = hwConfigCtrlBit;
			this._termAConnCtrlBit = termAConnCtrlBit;
			this._termBConnCtrlBit = termBConnCtrlBit;
			this._wiperConnCtrlBit = wiperConnCtrlBit;
			this._chan = chan;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the volatile memory address.
		/// </summary>
		/// <value>
		/// The volatile memory address.
		/// </value>
		public Byte VolatileMemoryAddress {
			get { return this._volatileMemAddr; }
		}

		/// <summary>
		/// Gets the non-volatile memory address.
		/// </summary>
		/// <value>
		/// The non-volatile memory address.
		/// </value>
		public Byte NonVolatileMemoryAddress {
			get { return this._nonVolatileMemAddr; }
		}

		/// <summary>
		/// Gets the terminal control address.
		/// </summary>
		/// <value>
		/// The terminal control address.
		/// </value>
		public Byte TerminalControlAddress {
			get { return this._termConAddr; }
		}

		/// <summary>
		/// Gets the hardware config control bit.
		/// </summary>
		/// <value>
		/// The hardware config control bit.
		/// </value>
		public Int32 HardwareConfigControlBit {
			get { return this._hwConfigCtrlBit; }
		}

		/// <summary>
		/// Gets the terminal A connection control bit.
		/// </summary>
		/// <value>
		/// The terminal A connection control bit.
		/// </value>
		public Int32 TerminalAConnectionControlBit {
			get { return this._termAConnCtrlBit; }
		}

		/// <summary>
		/// Gets the terminal B connection control bit.
		/// </summary>
		/// <value>
		/// The terminal B connection control bit.
		/// </value>
		public Int32 TerminalBConnectionControlBit {
			get { return this._termBConnCtrlBit; }
		}

		/// <summary>
		/// Gets the wiper connection control bit.
		/// </summary>
		/// <value>
		/// The wiper connection control bit.
		/// </value>
		public Int32 WiperConnectionControlBit {
			get { return this._wiperConnCtrlBit; }
		}

		/// <summary>
		/// Gets the channel.
		/// </summary>
		/// <value>
		/// The channel.
		/// </value>
		public MicrochipPotChannel Channel {
			get { return this._chan; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name. If no channel was specified, then returns an empty string.
		/// </value>
		public String Name {
			get {
				if (this._chan == MicrochipPotChannel.None) {
					return String.Empty;
				}
				return Enum.GetName(typeof(MicrochipPotChannel), this._chan);
			}
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing
		/// algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode() {
			Int32 hc = 13;
			hc = (hc * 7) + this._chan.GetHashCode();
			hc = (hc * 7) + this._hwConfigCtrlBit.GetHashCode();
			hc = (hc * 7) + this._nonVolatileMemAddr.GetHashCode();
			hc = (hc * 7) + this._termAConnCtrlBit.GetHashCode();
			hc = (hc * 7) + this._termBConnCtrlBit.GetHashCode();
			hc = (hc * 7) + this._termConAddr.GetHashCode();
			hc = (hc * 7) + this._volatileMemAddr.GetHashCode();
			hc = (hc * 7) + this._wiperConnCtrlBit.GetHashCode();
			return hc;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to
		/// the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="System.Object"/> to compare with the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			DeviceControlChannel dcc = obj as DeviceControlChannel;
			if ((Object)dcc == null) {
				return false;
			}

			return ((this._chan == dcc.Channel) && (this._hwConfigCtrlBit == dcc.HardwareConfigControlBit) &&
					(this._nonVolatileMemAddr == dcc.NonVolatileMemoryAddress) &&
					(this._termAConnCtrlBit == dcc.TerminalAConnectionControlBit) &&
					(this._termBConnCtrlBit == dcc.TerminalBConnectionControlBit) &&
					(this._termConAddr == dcc.TerminalControlAddress) &&
					(this._volatileMemAddr == dcc.VolatileMemoryAddress) &&
					(this._wiperConnCtrlBit == dcc.WiperConnectionControlBit));
		}

		/// <summary>
		/// Determines whether the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>
		/// is equal to the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>.
		/// </summary>
		/// <param name="dcc">
		/// The <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>
		/// to compare with the current <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>
		/// is equal to the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public Boolean Equals(DeviceControlChannel dcc) {
			if (dcc == null) {
				return false;
			}

			return ((this._chan == dcc.Channel) && (this._hwConfigCtrlBit == dcc.HardwareConfigControlBit) &&
					(this._nonVolatileMemAddr == dcc.NonVolatileMemoryAddress) &&
					(this._termAConnCtrlBit == dcc.TerminalAConnectionControlBit) &&
					(this._termBConnCtrlBit == dcc.TerminalBConnectionControlBit) &&
					(this._termConAddr == dcc.TerminalControlAddress) &&
					(this._volatileMemAddr == dcc.VolatileMemoryAddress) &&
					(this._wiperConnCtrlBit == dcc.WiperConnectionControlBit));
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Factory method for creating a device control channel based on the
		/// given potentiometer channel.
		/// </summary>
		/// <param name="channel">
		/// The MCP potentiometer channel.
		/// </param>
		/// <returns>
		/// A new instance of <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.DeviceControlChannel"/>.
		/// If no potentiometer channel was specified, then returns null.
		/// </returns>
		public static DeviceControlChannel ValueOf(MicrochipPotChannel channel) {
			if (channel == MicrochipPotChannel.None) {
				return null;
			}

			DeviceControlChannel result = null;
			String chanName = Enum.GetName(typeof(MicrochipPotChannel), channel);
			foreach (DeviceControlChannel dc in DeviceControlChannel.ALL) {
				if (dc.Name.Equals(chanName)) {
					result = dc;
					break;
				}
			}
			return result;
		}
		#endregion
	}
}

