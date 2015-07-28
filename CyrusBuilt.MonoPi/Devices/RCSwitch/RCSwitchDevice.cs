//
//  RCSwitch.cs
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
using CyrusBuilt.MonoPi.IO;
using CyrusBuilt.MonoPi.PiSystem;

namespace CyrusBuilt.MonoPi.Devices.RCSwitch
{
	/// <summary>
	/// A device abstraction for an RC (remote control) switched power outlets.
	/// </summary>
	public class RCSwitchDevice : DeviceBase, IRCSwitch
	{
		#region Constants
		/// <summary>
		/// The default pulse length for Protocol 1 (350).
		/// </summary>
		public const Int32 DEFAULT_PROTOCOL1_PULSE_LENGTH = 350;

		/// <summary>
		/// The default pulse length for Protocol 2 (650).
		/// </summary>
		public const Int32 DEFAULT_PROTOCOL2_PULSE_LENGTH = 650;

		private const Int32 PROTOCOL1_SYNC_LOW_PULSES = 31;
		private const Int32 PROTOCOL2_SYNC_LOW_PULSES = 10;
		#endregion

		#region Fields
		private IRaspiGpio _txPin = null;
		private Int32 _pulseLength = DEFAULT_PROTOCOL1_PULSE_LENGTH;
		private Int32 _repeatTransmit = 0;
		private RCProtocol _protocol = RCProtocol.P1;
		private static readonly Object _syncLock = new Object();
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice"/>
		/// class with the pin to use to transmit codes with.
		/// </summary>
		/// <param name="transmitPin">
		/// The native pin to use to transmit codes.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="transmitPin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="transmitPin"/> is not an output pin.
		/// </exception>
		public RCSwitchDevice(IRaspiGpio transmitPin)
			: base() {
			if (transmitPin == null) {
				throw new ArgumentNullException("pin");
			}

			if (transmitPin.Mode != PinMode.OUT) {
				throw new ArgumentException("The specified pin is not an output.", "pin");
			}

			this._txPin = transmitPin;
			this._txPin.Write(PinState.Low);
		}
			
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._txPin != null) {
				this._txPin.Dispose();
				this._txPin = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the operating protocol.
		/// </summary>
		/// <value>
		/// The protocol. If the <see cref="CyrusBuilt.MonoPi.Devices.RCSwitch.RCSwitchDevice.PulseLength"/>
		/// value is set to less than or equal to zero, then the default
		/// pulse length value for the specified protocol will be set.
		/// </value>
		public RCProtocol Protocol {
			get { return this._protocol; }
			set {
				this._protocol = value;
				if (this._pulseLength <= 0) {
					switch (this._protocol) {
						case RCProtocol.P1:
							this._pulseLength = DEFAULT_PROTOCOL1_PULSE_LENGTH;
							break;
						case RCProtocol.P2:
							this._pulseLength = DEFAULT_PROTOCOL2_PULSE_LENGTH;
							break;
						default:
							break;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the length of the pulse.
		/// </summary>
		/// <value>
		/// The length of the pulse.
		/// </value>
		public Int32 PulseLength {
			get { return this._pulseLength; }
			set { this._pulseLength = value; }
		}

		/// <summary>
		/// Gets or sets the transmit repititions.
		/// </summary>
		/// <value>
		/// The transmit repeats.
		/// </value>
		public Int32 RepeatTransmit {
			get { return this._repeatTransmit; }
			set { this._repeatTransmit = value; }
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Trasmits the specified number of high and low pulses
		/// </summary>
		/// <param name="highPulses">
		/// The number of high pulses.
		/// </param>
		/// <param name="lowPulses">
		/// The number of low pulses.
		/// </param>
		private void Transmit(Int32 highPulses, Int32 lowPulses) {
			if (this._txPin != null) {
				this._txPin.Write(PinState.High);
				CoreUtils.SleepMicroseconds(this._pulseLength * highPulses);
				this._txPin.Write(PinState.Low);
				CoreUtils.SleepMicroseconds(this._pulseLength * lowPulses);
			}
		}

		/// <summary>
		/// Sends a "Sync" bit.<br/><br/>
		///                       _           <br/>
		/// Waveform Protocol 1: | |_______________________________<br/>
		///                       _           <br/>
		/// Waveform Protocol 2: | |__________<br/>
		/// </summary>
		private void SendSync() {
			switch (this._protocol) {
				case RCProtocol.P1:
					this.Transmit(1, PROTOCOL1_SYNC_LOW_PULSES);
					break;
				case RCProtocol.P2:
					this.Transmit(1, PROTOCOL2_SYNC_LOW_PULSES);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Sends a tri-state "0" bit.<br/><br/>
		///            _     _       <br/>
		/// Waveform: | |___| |___   <br/>
		/// </summary>
		private void SendT0() {
			this.Transmit(1, 3);
			this.Transmit(1, 3);
		}

		/// <summary>
		/// Sends a tri-state "1" bit.<br/><br/>
		///            ___   ___     <br/>
		/// Waveform: |   |_|   |_   <br/>
		/// </summary>
		private void SendT1() {
			this.Transmit(3, 1);
			this.Transmit(3, 1);
		}

		/// <summary>
		/// Sends a tri-state "F" bit.<br/><br/>
		///            _     ___     <br/>
		/// Waveform: | |___|   |_   <br/>
		/// </summary>
		private void SendTF() {
			this.Transmit(1, 3);
			this.Transmit(3, 1);
		}

		/// <summary>
		/// Sends a "0" bit.<br/><br/>
		///                       _      <br/>
		/// Waveform Protocol 1: | |___  <br/><br/>
		///                       _      <br/>
		/// Waveform Protocol 2: | |__   <br/>
		/// </summary>
		private void Send0() {
			switch (this._protocol) {
				case RCProtocol.P1:
					this.Transmit(1, 3);
					break;
				case RCProtocol.P2:
					this.Transmit(1, 2);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Sends a "1" bit.<br/><br/>
		///                       ___    <br/>
		/// Waveform Protocol 1: |   |_  <br/><br/>
		///                       __     <br/>
		/// Waveform Protocol 2: |  |_   <br/>
		/// </summary>
		private void Send1() {
			switch (this._protocol) {
				case RCProtocol.P1:
					this.Transmit(3, 1);
					break;
				case RCProtocol.P2:
					this.Transmit(2, 1);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Returns a 13 character array, representing the code word to be sent.
		/// A code word consists of 9 address bits, 3 data bits, and on sync bit
		/// but in our case, only the first 8 address bits and the last 2 data
		/// bits are used. A code bit can have 4 different states: "F" (floating),
		/// "1" (high), "0" (low), and "S" (synchronous bit).<br/><br/>
		/// +-------------------------------+--------------------------------+-----------------------------------------+-----------------------------------------+----------------------+------------+
		/// | 4 bits address (switch group) | 4 bits address (switch number) | 1 bit address (not used, so never mind) | 1 bit address (not used, so never mind) | 2 data bits (on|off) | 1 sync bit |
		/// | 1=0FFF 2=F0FF 3=FF0F 4=FFF0   | 1=0FFF 2=F0FF 3=FF0F 4=FFF0    | F | F | on=FF off=F0 | S |
		/// +-------------------------------+--------------------------------+-----------------------------------------+-----------------------------------------+----------------------+------------+
		/// <br/>
		/// </summary>
		/// <param name="groupAddress">
		/// A bitset containing 5 bits that represent the switch group address.
		/// </param>
		/// <param name="chan">
		/// The channel (switch) to manipulate.
		/// </param>
		/// <param name="status">
		/// Whether to switch on (true) or off (false).
		/// </param>
		/// <returns>
		/// If successful, a 13 character array containing the code word; Otherwise,
		/// a single-character array containing only a null-character.
		/// </returns>
		private String GetCodeWordA(BitSet groupAddress, ChannelCode chan, Boolean status) {
			Int32 returnPos = 0;
			char[] word = new char[13];
			String[] code = new String[6] { "FFFFF", "0FFFF", "F0FFF", "FF0FF", "FFF0F", "FFFF0" };
			if (((Int32)chan < 1) || ((Int32)chan > 5)) {
				return new String(new char[1] { '\0' });
			}

			for (Int32 i = 0; i < 5; i++) {
				if (!groupAddress.Get(i)) {
					word[returnPos++] = 'F';
				}
				else {
					word[returnPos++] = '0';
				}
			}

			for (Int32 j = 0; j < 5; j++) {
				word[returnPos++] = code[(Int32)chan][j];
			}

			if (status) {
				word[returnPos++] = '0';
				word[returnPos++] = 'F';
			}
			else {
				word[returnPos++] = 'F';
				word[returnPos++] = '0';
			}

			word[returnPos] = '\0';
			return new String(word);
		}

		/// <summary>
		/// Returns a 13 character array, representing the code word to be sent.
		/// A code word consists of 9 address bits, 3 data bits, and on sync bit
		/// but in our case, only the first 8 address bits and the last 2 data
		/// bits are used. A code bit can have 4 different states: "F" (floating),
		/// "1" (high), "0" (low), and "S" (synchronous bit).<br/><br/>
		/// +-------------------------------+--------------------------------+-----------------------------------------+-----------------------------------------+----------------------+------------+
		/// | 4 bits address (switch group) | 4 bits address (switch number) | 1 bit address (not used, so never mind) | 1 bit address (not used, so never mind) | 2 data bits (on|off) | 1 sync bit |
		/// | 1=0FFF 2=F0FF 3=FF0F 4=FFF0   | 1=0FFF 2=F0FF 3=FF0F 4=FFF0    | F | F | on=FF off=F0 | S |
		/// +-------------------------------+--------------------------------+-----------------------------------------+-----------------------------------------+----------------------+------------+
		/// <br/>
		/// </summary>
		/// <param name="address">
		/// The switch group (address).
		/// </param>
		/// <param name="chan">
		/// The channel (switch) to manipulate.
		/// </param>
		/// <param name="status">
		/// Whether to switch on (true) or off (false).
		/// </param>
		/// <returns>
		/// a single-character array containing only a null-character.
		/// </returns>
		private String GetCodeWordB(AddressCode address, ChannelCode chan, Boolean status) {
			Int32 returnPos = 0;
			char[] word = new char[13];
			String[] code = new String[5] { "FFFF", "0FFF", "F0FF", "FF0F", "FFF0" };
			if (((Int32)address < 1) || ((Int32)address > 4) ||
				((Int32)chan < 1) || ((Int32)chan > 4)) {
				return new String(new char[1] { '\0' });
			}

			for (Int32 i = 0; i < 4; i++) {
				word[returnPos++] = code[(Int32)address][i];
			}

			for (Int32 j = 0; j < 4; j++) {
				word[returnPos++] = code[(Int32)chan][j];
			}

			word[returnPos++] = 'F';
			word[returnPos++] = 'F';
			word[returnPos++] = 'F';
			if (status) {
				word[returnPos++] = 'F';
			}
			else {
				word[returnPos++] = '0';
			}

			word[returnPos] = '\0';
			return new String(word);
		}

		/// <summary>
		/// Transmits the specified code word to the device.
		/// </summary>
		/// <param name="codeWord">
		/// The code word to transmit.
		/// </param>
		public void Send(char[] codeWord) {
			Int32 j = 0;
			for (Int32 i = 0; i < this._repeatTransmit; i++) {
				j = 0;
				while (codeWord[j] != '\0') {
					switch (codeWord[j]) {
						case '0':
							this.Send0();
							break;
						case '1':
							this.Send1();
							break;
						default:
							break;
					}
					j++;
				}
				this.SendSync();
			}
		}

		/// <summary>
		/// Transmits the specified code word to the device.
		/// </summary>
		/// <param name="code">
		/// A long represents the bits of the address.
		/// </param>
		/// <param name="length">
		/// The length of bits (count) to send.
		/// </param>
		public void Send(long code, Int32 length) {
			this.Send(Dec2BinWzeroFill(code, length));
		}

		/// <summary>
		/// Sends a code word.
		/// </summary>
		/// <param name="codeWord">
		/// The code word to send.
		/// </param>
		public void SendTriState(char[] codeWord) {
			Int32 j = 0;
			for (Int32 i = 0; i < this._repeatTransmit; i++) {
				j = 0;
				while (codeWord[j] != '\0') {
					switch (codeWord[j]) {
						case '0':
							this.SendT0();
							break;
						case 'F':
							this.SendTF();
							break;
						case '1':
							this.SendT1();
							break;
						default:
							break;
					}
					j++;
				}
				this.SendSync();
			}
		}

		/// <summary>
		/// Switch a remote switch on (Type A with 10 pole DIP switches).
		/// </summary>
		/// <param name="switchGroupAddress">
		/// Code of the switch group (refers to DIP switches 1 - 5, where
		/// "1" = on and "0" = off, if all DIP switches are on it's "11111").
		/// </param>
		/// <param name="device">
		/// The switch device number.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="switchGroupAddress"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="switchGroupAddress"/> cannot have more than 5 bits.
		/// </exception>
		public void SwitchOn(BitSet switchGroupAddress, RCSwitchDevNum device) {
			if (switchGroupAddress == null) {
				throw new ArgumentNullException("switchGroupAddress");
			}

			if (switchGroupAddress.Length > 5) {
				throw new ArgumentException("Cannot accept a switch group address with more than 5 bits.");
			}

			if (device == RCSwitchDevNum.None) {
				return;
			}

			String state = this.GetCodeWordA(switchGroupAddress, (ChannelCode)((Int32)device), true);
			this.SendTriState(state.ToCharArray());
		}
			
		/// <summary>
		/// Switch a remote switch off (Type A with 10 pole DIP switches).
		/// </summary>
		/// <param name="switchGroupAddress">
		/// Code of the switch group (refers to DIP switches 1 - 5, where
		/// "1" = on and "0" = off, if all DIP switches are on it's "11111").
		/// </param>
		/// <param name="device">
		/// The switch device number.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="switchGroupAddress"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="switchGroupAddress"/> cannot have more than 5 bits.
		/// </exception>
		public void SwitchOff(BitSet switchGroupAddress, RCSwitchDevNum device) {
			if (switchGroupAddress == null) {
				throw new ArgumentNullException("switchGroupAddress");
			}

			if (switchGroupAddress.Length > 5) {
				throw new ArgumentException("Cannot accept a switch group address with more than 5 bits.");
			}

			if (device == RCSwitchDevNum.None) {
				return;
			}

			String state = this.GetCodeWordA(switchGroupAddress, (ChannelCode)((Int32)device), false);
			this.SendTriState(state.ToCharArray());
		}

		/// <summary>
		/// Switch a remote switch on (Type B with two rotary/sliding switches).
		/// </summary>
		/// <param name="address">
		/// The address of the switch group.
		/// </param>
		/// <param name="channel">
		/// The channel (switch) itself.
		/// </param>
		public void SwitchOn(AddressCode address, ChannelCode channel) {
			this.SendTriState(this.GetCodeWordB(address, channel, true).ToCharArray());
		}

		/// <summary>
		/// Switch a remote switch off (Type B with two rotary/sliding switches).
		/// </summary>
		/// <param name="address">
		/// The address of the switch group.
		/// </param>
		/// <param name="channel">
		/// The channel (switch) itself.
		/// </param>
		public void SwitchOff(AddressCode address, ChannelCode channel) {
			this.SendTriState(this.GetCodeWordB(address, channel, false).ToCharArray());
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Converts a decimal value into its binary representation.
		/// </summary>
		/// <param name="dec">
		/// The decimal value to convert.
		/// </param>
		/// <param name="bitLength">
		/// The length in bits to fill with zeros.
		/// </param>
		/// <returns>
		/// The binary representation of the specified decimal.
		/// </returns>
		private static char[] Dec2BinWzeroFill(long dec, Int32 bitLength) {
			char[] bin = new char[64];
			Int32 i = 0;

			while (dec > 0) {
				bin[32 + i++] = ((dec & 1) > 0) ? '1' : '0';
				dec = dec >> 1;
			}

			for (Int32 j = 0; j < bitLength; j++) {
				if (j >= bitLength - i) {
					bin[j] = bin[31 + i - (j - (bitLength - i))];
				}
				else {
					bin[j] = '\0';
				}
			}

			bin[bitLength] = '\0';
			return bin;
		}

		/// <summary>
		/// Convenience method for converting a string like "11011" to a
		/// <see cref="CyrusBuilt.MonoPi.BitSet"/>.
		/// </summary>
		/// <param name="address">
		/// The a string containing the address bits in sequence.
		/// </param>
		/// <returns>
		/// A bitset containing the address that can be used for swithing
		/// devices on or off.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="address"/> cannot be a null or empty string.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="address"/> must contain exactly 5 bits.
		/// </exception>
		public static BitSet GetSwitchGroupAddress(String address) {
			if (String.IsNullOrEmpty(address)) {
				throw new ArgumentNullException("address");
			}

			if (address.Length != 5) {
				throw new ArgumentException("address must consist of exactly 5 bits!");
			}

			BitSet bits = new BitSet(5);
			for (Int32 i = 0; i < 5; i++) {
				bits.Set(i, address[i] == '1');
			}
			return bits;
		}
		#endregion
	}
}

