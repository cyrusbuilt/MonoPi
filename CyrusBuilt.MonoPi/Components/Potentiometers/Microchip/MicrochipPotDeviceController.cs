//
//  MicrochipPotDeviceController.cs
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
using System.IO;
using CyrusBuilt.MonoPi.IO.I2C;

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// An MCP45XX and MCP46XX device controller component. This mostly a port of the
	/// <a href="https://github.com/Pi4J/pi4j/blob/master/pi4j-device/src/main/java/com/pi4j/component/potentiometer/microchip">
	/// device controller in the Pi4J project
	/// </a> (Java port author <a href="http://raspelikan.blogspot.co.at">Raspelikan</a>)
	/// which is a port of similar C++ code from <a href="http://blog.stibrany.com/?p=9">Stibro's code blog</a>.
	/// </summary>
	public class MicrochipPotDeviceController : IDisposable
	{
		#region Constants
		/// <summary>
		/// Flag to use when indicating a volatile wiper.
		/// </summary>
		public const Boolean VOLATILE_WIPER = true;

		/// <summary>
		/// Flag to use when indicating a non-volatile wiper.
		/// </summary>
		public const Boolean NONVOLATILE_WIPER = false;

		private const Byte MEMADDR_STATUS = 0x05;
		private const Byte MEMADDR_WRITEPROTECTION = 0x0F;
		#endregion

		#region Fields
		private Boolean _isDisposed = false;
		private II2CBus _device = null;
		private Int32 _busAddr = -1;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController"/>
		/// class with the I2C bus device this instance is connected to and
		/// the bus address of that device.
		/// </summary>
		/// <param name="device">
		/// The I2C bus device this instance is connected to.
		/// </param>
		/// <param name="busAddress">
		/// The bus address of the device.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="device"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to open the I2C bus.
		/// </exception>
		public MicrochipPotDeviceController(II2CBus device, Int32 busAddress) {
			if (device == null) {
				throw new ArgumentNullException("device");
			}

			this._busAddr = busAddress;
			this._device = device;
			if (!this._device.IsOpen) {
				this._device.Open();
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController"/> so the garbage
		/// collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._device != null) {
				this._device.Dispose();
				this._device = null;
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
		/// Gets the device status.
		/// </summary>
		/// <value>
		/// The device status.
		/// </value>
		/// <exception cref="IOException">
		/// Status bits 4 to 8 must be set to 1.
		/// </exception>
		public DeviceControllerDeviceStatus DeviceStatus {
			get {
				// Get status from device.
				Int32 stat = this.Read(MEMADDR_STATUS);

				// Check formal criterias.
				Int32 reservedVal = stat & (Int32)StatusBit.RESERVED_MASK;
				if (reservedVal != (Int32)StatusBit.RESERVED_VALUE) {
					throw new IOException("Status bits 4 to 8 must be 1 according to documenation chapter 4.2.2.1. Got: " + reservedVal.ToString());
				}

				// Build the result.
				Boolean eepromWriteActive = ((stat & (Int32)StatusBit.EEPROM_WRITEACTIVE) > 0);
				Boolean eepromWriteProt = ((stat & (Int32)StatusBit.EEPROM_WRITEPROTECTION) > 0);
				Boolean wiperlock0 = ((stat & (Int32)StatusBit.WIPER_LOCK0) > 0);
				Boolean wiperlock1 = ((stat & (Int32)StatusBit.WIPER_LOCK1) > 0);
				return new DeviceControllerDeviceStatus(eepromWriteActive, eepromWriteProt, wiperlock0, wiperlock1);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reads two bytes from the device at the given memory address.
		/// </summary>
		/// <param name="memAddr">
		/// The memory address to read from.
		/// </param>
		/// <returns>
		/// The two bytes read.
		/// </returns>
		/// <exception cref="IOException">
		/// Communication failed - or - device returned a malformed result.
		/// </exception>
		private Int32 Read(Byte memAddr) {
			// Command to ask device for reading data.
			Byte cmd = (Byte)((memAddr << 4) | (Byte)MCPCommand.READ);
			this._device.WriteByte(this._busAddr, cmd);

			// Read 2 bytes.
			Byte[] buf = this._device.ReadBytes(this._busAddr, 2);
			if (buf.Length != 2) {
				throw new IOException("Malformed response. Expected to read two bytes but got: " + buf.Length.ToString());
			}

			// Transform signed byte to unsigned byte stored as int.
			Int32 first = buf[0] & 0xFF;
			Int32 second = buf[1] & 0xFF;

			// Interpret both bytes as one integer.
			return (first << 8) | second;
		}

		/// <summary>
		/// Writes 9 bits of the give value to the device.
		/// </summary>
		/// <param name="memAddr">
		/// The memory address to write to.
		/// </param>
		/// <param name="val">
		/// The value to be written.
		/// </param>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		private void Write(Byte memAddr, Int32 val) {
			// Bit 8 of value.
			Byte firstBit = (Byte)((val >> 8) & 0x000001);

			// Command to ask device for setting a value.
			Byte cmd = (Byte)((memAddr << 4) | (Byte)MCPCommand.WRITE | firstBit);

			// Now the 7 bits of actual value.
			Byte data = (Byte)(val & 0x00FF);

			// Write the sequence of command and data.
			Byte[] seq = new Byte[] { cmd, data };
			this._device.WriteBytes(this._busAddr, seq);
		}

		/// <summary>
		/// Writes n (steps) bytes to the device holding the wiper's address
		/// and the increment or decrement command and value.
		/// </summary>
		/// <param name="memAddr">
		/// The memory address to write to.
		/// </param>
		/// <param name="increase">
		/// Set true to increment the wiper, or false to decrement.
		/// </param>
		/// <param name="steps">
		/// The number of steps the wiper has to be incremented/decremented.
		/// </param>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		private void IncreaseOrDecrease(Byte memAddr, Boolean increase, Int32 steps) {
			// 0 steps means 'do nothing'.
			if (steps == 0) {
				return;
			}

			// Negative steps means decrease on 'increase' or increase on 'decrease';
			Int32 actualSteps = steps;
			Boolean actualIncrease = increase;
			if (steps < 0) {
				actualIncrease = !increase;
				actualSteps = Math.Abs(steps);
			}
				
			// Ask device for increase or decrease.
			Byte cmd = (Byte)((memAddr << 4) | (actualIncrease ? (Byte)MCPCommand.INCREASE : (Byte)MCPCommand.DECREASE));

			// Build sequence of commands (one for each step).
			Byte[] sequence = new Byte[actualSteps];
			for (Int32 i = 0; i < sequence.Length; i++) {
				sequence[i] = cmd;
			}

			// Write sequence to the device.
			this._device.WriteBytes(this._busAddr, sequence);
		}

		/// <summary>
		/// Sets or clears a bit in the specified memory (integer).
		/// </summary>
		/// <param name="mem">
		/// The memory to modify.
		/// </param>
		/// <param name="mask">
		/// The mask which defines the bit to be set/cleared.
		/// </param>
		/// <param name="val">
		/// Whether to set the bit (true) or clear the bit (false).
		/// </param>
		/// <returns>
		/// The modified memory.
		/// </returns>
		private Int32 SetBit(Int32 mem, Int32 mask, Boolean val) {
			Int32 result = 0;
			if (val) {
				result = mem | mask;   // Set bit using OR.
			}
			else {
				result = mem & ~mask;  // Clear bit by using AND with inverted mask.
			}
			return result;
		}

		/// <summary>
		/// Enables or disables the device's write-protection.
		/// </summary>
		/// <param name="enabled">
		/// Set true to enable write protection, or false to disable.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void SetWriteProtection(Boolean enabled) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}
			this.IncreaseOrDecrease(MEMADDR_WRITEPROTECTION, enabled, 1);
		}

		/// <summary>
		/// Enables or disables the wiper's lock.
		/// </summary>
		/// <param name="channel">
		/// The channel of the wiper to set the lock for.
		/// </param>
		/// <param name="locked">
		/// Set true to enable the lock, or false to disable.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void SetWiperLock(DeviceControlChannel channel, Boolean locked) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			// Increasing or decreasing on non-volatile wipers
			// enables or disables WiperLock.
			Byte memAddr = channel.NonVolatileMemoryAddress;
			this.IncreaseOrDecrease(memAddr, locked, 1);
		}

		/// <summary>
		/// Sets the device's terminal configuration.
		/// </summary>
		/// <param name="config">
		/// The configuration to set.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="config"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified configuration cannot have a null channel.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void SetTerminalConfiguration(DeviceControllerTerminalConfiguration config) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (config == null) {
				throw new ArgumentNullException("config");
			}

			DeviceControlChannel chan = config.Channel;
			if (chan == null) {
				throw new ArgumentException("A configuration with a null channel is not permitted.", "config");
			}

			// Read current configuration.
			Byte memAddr = config.Channel.TerminalControlAddress;
			Int32 tcon = this.Read(memAddr);

			// Modify configuration.
			tcon = this.SetBit(tcon, chan.HardwareConfigControlBit, config.ChannelEnabled);
			tcon = this.SetBit(tcon, chan.TerminalAConnectionControlBit, config.PinAEnabled);
			tcon = this.SetBit(tcon, chan.WiperConnectionControlBit, config.PinWEnabled);
			tcon = this.SetBit(tcon, chan.TerminalBConnectionControlBit, config.PinBEnabled);

			// Write new config to device.
			this.Write(memAddr, tcon);
		}

		/// <summary>
		/// Gets the terminal configuration for the specified channel.
		/// </summary>
		/// <param name="channel">
		/// The channel to get the terminal configuration for.
		/// </param>
		/// <returns>
		/// The terminal configuration.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to read from device.
		/// </exception>
		public DeviceControllerTerminalConfiguration GetTerminalConfiguration(DeviceControlChannel channel) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			// Read the current config.
			Int32 tcon = this.Read(channel.TerminalControlAddress);

			// Build result;
			Boolean chanEnabled = ((tcon & channel.HardwareConfigControlBit) > 0);
			Boolean pinAEnabled = ((tcon & channel.TerminalAConnectionControlBit) > 0);
			Boolean pinWEnabled = ((tcon & channel.WiperConnectionControlBit) > 0);
			Boolean pinBEnabled = ((tcon & channel.TerminalBConnectionControlBit) > 0);
			return new DeviceControllerTerminalConfiguration(channel, chanEnabled, pinAEnabled, pinWEnabled, pinBEnabled);
		}

		/// <summary>
		/// Sets the wiper's value in the device.
		/// </summary>
		/// <param name="channel">
		/// The device channel the wiper is on.
		/// </param>
		/// <param name="value">
		/// The wiper's value.
		/// </param>
		/// <param name="nonVolatile">
		/// Set true to write to non-volatile memory, or false to write to volatile memory.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> cannot be a negative.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void SetValue(DeviceControlChannel channel, Int32 value, Boolean nonVolatile) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			if (value < 0) {
				throw new ArgumentException("Only positive values are permitted. Got value '" +
											value.ToString() + "' for writing to channel '" +
											channel.Name + "'.");
			}

			// Choose proper mem address.
			Byte memAddr = nonVolatile ? channel.NonVolatileMemoryAddress : channel.VolatileMemoryAddress;

			// Write value to device.
			this.Write(memAddr, value);
		}

		/// <summary>
		/// Receives the current wiper's value from the device.
		/// </summary>
		/// <param name="channel">
		/// The device channel the wiper is on.
		/// </param>
		/// <param name="nonVolatile">
		/// Set true to read from non-volatile memory, false to read from
		/// volatile memory.
		/// </param>
		/// <returns>
		/// The wiper's value.
		/// </returns>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to read from device.
		/// </exception>
		public Int32 GetValue(DeviceControlChannel channel, Boolean nonVolatile) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			// Select proper memory address, then read the value at that address.
			Byte memAddr = nonVolatile ? channel.NonVolatileMemoryAddress : channel.VolatileMemoryAddress;
			return this.Read(memAddr);
		}

		/// <summary>
		/// Decriments the volatile wiper for the given number of steps.
		/// </summary>
		/// <param name="channel">
		/// The device channel the wiper is on.
		/// </param>
		/// <param name="steps">
		/// The number of steps.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void Decrease(DeviceControlChannel channel, Int32 steps) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			// Decrease only works on volatile-wiper.
			Byte memAddr = channel.VolatileMemoryAddress;
			this.IncreaseOrDecrease(memAddr, false, steps);
		}

		/// <summary>
		/// Increments the volatile wiper for the given number of steps.
		/// </summary>
		/// <param name="channel">
		/// The device channel the wiper is on.
		/// </param>
		/// <param name="steps">
		/// The number of steps.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="channel"/> cannot be null.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public void Increase(DeviceControlChannel channel, Int32 steps) {
			if (this._isDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotDeviceController");
			}

			if (channel == null) {
				throw new ArgumentNullException("channel");
			}

			// Decrease only works on volatile-wiper.
			Byte memAddr = channel.VolatileMemoryAddress;
			this.IncreaseOrDecrease(memAddr, true, steps);
		}
		#endregion
	}
}

