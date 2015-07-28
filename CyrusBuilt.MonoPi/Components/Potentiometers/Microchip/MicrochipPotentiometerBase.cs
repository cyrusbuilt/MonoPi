//
//  MicrochipPotentiometerBase.cs
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
using CyrusBuilt.MonoPi.IO.I2C;

namespace CyrusBuilt.MonoPi.Components.Potentiometers.Microchip
{
	/// <summary>
	/// Base class for Microchip MCP45XX and MCP46XX IC device
	/// abstraction components.
	/// </summary>
	public abstract class MicrochipPotentiometerBase : ComponentBase, IMicrochipPotentiometer
	{
		#region Internal Helpers
		/// <summary>
		/// Wiper event arguments class.
		/// </summary>
		private class WiperEventArgs : EventArgs
		{
			private DeviceControlChannel _chan = null;
			private MicrochipPotDeviceController _ctlr = null;
			private Int32 _value = 0;

			/// <summary>
			/// Initializes a new instance of the
			/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase.WiperEventArgs"/>
			/// class with the device control channel, device controller, and
			/// device reading value.
			/// </summary>
			/// <param name="channel">
			/// The control channel for the wiper.
			/// </param>
			/// <param name="controller">
			/// The device controller.
			/// </param>
			/// <param name="val">
			/// The device reading value.
			/// </param>
			public WiperEventArgs(DeviceControlChannel channel, MicrochipPotDeviceController controller, Int32 val)
				: base() {
				this._chan = channel;
				this._ctlr = controller;
			}

			/// <summary>
			/// Sets the channel value.
			/// </summary>
			/// <param name="nonVol">
			/// Set true if setting the channel value of a non-volatile wiper,
			/// or false for a volatile wiper.
			/// </param>
			public void SetChannelValue(Boolean nonVol) {
				this._ctlr.SetValue(this._chan, this._value, nonVol);
			}
		}

		/// <summary>
		/// Wiper event handler delegate.
		/// </summary>
		/// <param name="sender">
		/// The object firing the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private delegate void WiperEventHandler(Object sender, WiperEventArgs e);
		#endregion

		#region Constants
		/// <summary>
		/// The value which is used for address-bit if the device's package
		/// does not provide a matching address-pin.
		/// </summary>
		protected const Boolean PIN_NOT_AVAILABLE = true;

		/// <summary>
		/// The value which is used for devices capable of non-volatile wipers.
		/// For those devices, the initial value is loaded from EEPROM.
		/// </summary>
		protected const Int32 INITIALVALUE_LOADED_FROM_EEPROM = 0;
		#endregion

		#region Fields
		private MicrochipPotDeviceController _controller = null;
		private MicrochipPotChannel _channel = MicrochipPotChannel.None;
		private Int32 _currentValue = 0;
		#pragma warning disable 1591
		protected MicrochipPotNonVolatileMode _nonVolMode = MicrochipPotNonVolatileMode.VolatileAndNonVolatile;
		#pragma warning restore 1591
		#endregion

		#region Events
		/// <summary>
		/// Occurs when wiper value is changing.
		/// </summary>
		private event WiperEventHandler WiperActionEvent;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase"/>
		/// class with the I2C device connection, pin A0,A1, and A2 states,
		/// the potentiometer (channel) provided by the device, how to do
		/// non-volatile I/O and the initial value for devices which are not
		/// capable of non-volatile wipers.
		/// </summary>
		/// <param name="device">
		/// The I2C bus device this instance is connected to.
		/// </param>
		/// <param name="pinA0">
		/// Whether the device's address pin A0 is high (true) or low (false).
		/// </param>
		/// <param name="pinA1">
		/// Whether the device's address pin A1 is high (true) or low (false).
		/// </param>
		/// <param name="pinA2">
		/// Whether the device's address pin A2 is high (true) or low (false).
		/// </param>
		/// <param name="channel">
		/// Which of the potentiometers provided by the device to control.
		/// </param>
		/// <param name="nonVolatileMode">
		/// The way non-volatile reads or writes are done.
		/// </param>
		/// <param name="initialNonVolWiperValue">
		/// The value for devices which are not capable of non-volatile wipers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="device"/> cannot be null. - or - <paramref name="channel"/>
		/// cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="channel"/> is not supported by this device.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// Unable to open the I2C bus.
		/// </exception>
		protected MicrochipPotentiometerBase(II2CBus device, Boolean pinA0, Boolean pinA1, Boolean pinA2,
			MicrochipPotChannel channel, MicrochipPotNonVolatileMode nonVolatileMode, Int32 initialNonVolWiperValue)
			: base() {
			if (device == null) {
				throw new ArgumentNullException("device");
			}
				
			if (!this.IsChannelSupported(channel)) {
				throw new ArgumentException("Specified channel not supported by device.", "channel");
			}
				
			this._channel = channel;
			this._nonVolMode = nonVolatileMode;
			Int32 deviceAddr = BuildI2CAddress(pinA0, pinA1, pinA2);
			this._controller = new MicrochipPotDeviceController(device, deviceAddr);
			this.WiperActionEvent += this.MicrochipPotentiometerBase_WiperActionEvent;
			this.Initialize(initialNonVolWiperValue);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase"/>. The
		/// <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase"/> so the garbage
		/// collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase"/> was occupying.
		/// </remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._controller != null) {
				this._controller.Dispose();
				this._controller = null;
			}

			this._currentValue = -1;
			this._channel = MicrochipPotChannel.None;
			this._nonVolMode = MicrochipPotNonVolatileMode.VolatileAndNonVolatile;
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the channels that are supported by the underlying device.
		/// </summary>
		/// <value>
		/// The supported channels.
		/// </value>
		public abstract MicrochipPotChannel[] SupportedChannels { get; }

		/// <summary>
		/// Gets whether or not the device is capable of non-volatile wipers.
		/// </summary>
		/// <value>
		/// <c>true</c> if the device is capable of non-volatile wipers;
		/// otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsNonVolatileWiperCapable { get; }

		/// <summary>
		/// Gets the way non-volatile reads and/or writes are done.
		/// </summary>
		public MicrochipPotNonVolatileMode NonVolatileMode {
			get { return this._nonVolMode; }
		}

		/// <summary>
		/// Gets the channel this device is configured for..
		/// </summary>
		public MicrochipPotChannel Channel {
			get { return this._channel; }
		}

		/// <summary>
		/// Gets or sets the wiper's current value.
		/// </summary>
		/// <value>
		/// The current value. Values from 0 to <see cref="MaxValue"/>
		/// are valid. Values above or below these boundaries should be
		/// corrected quietly.
		/// </value>
		/// <returns>
		/// The wiper's current value. This value is read from cache.
		/// The cache is updated on any modifying action or the method
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase.UpdateCacheFromDevice()"/>.
		/// </returns>
		public Int32 CurrentValue {
			get {
				return this._currentValue;
			}
			set {
				// Check boundaries.
				Int32 newVal = this.GetValueAccordingBoundaries(value);

				// Set wipers according to mode.
				DeviceControlChannel chan = DeviceControlChannel.ValueOf(this._channel);
				this.OnWiperActionEvent(new WiperEventArgs(chan, this._controller, newVal));

				// Set value only if volatile wiper is affected.
				if (this._nonVolMode == MicrochipPotNonVolatileMode.NonVolatileOnly) {
					return;
				}

				this._currentValue = newVal;
			}
		}

		/// <summary>
		/// Gets the maximum wiper-value supported by the device.
		/// </summary>
		public abstract Int32 MaxValue { get; }

		/// <summary>
		/// Gets the device and wiper status.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		public IMicrochipPotDeviceStatus DeviceStatus {
			get {
				DeviceControllerDeviceStatus devStat = this._controller.DeviceStatus;
				Boolean wiperLockActive = this._channel == MicrochipPotChannel.A ?
										devStat.ChannelALocked : devStat.ChannelBLocked;
				return new MicrochipPotDeviceStatus(this._channel, devStat.EepromWriteActive,
													devStat.EepromWriteProtected, wiperLockActive);
			}
		}

		/// <summary>
		/// Gets or sets the current terminal configuration.
		/// </summary>
		/// <value>
		/// The configuration to set.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// value canoot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Configuration channel and device channel must match.
		/// </exception>
		public MCPTerminalConfiguration TerminalConfiguration {
			get {
				DeviceControlChannel chan = DeviceControlChannel.ValueOf(this._channel);
				DeviceControllerTerminalConfiguration tcon = this._controller.GetTerminalConfiguration(chan);
				return new MCPTerminalConfiguration(this._channel, tcon.ChannelEnabled, tcon.PinAEnabled,
																	tcon.PinWEnabled, tcon.PinBEnabled);
			}
			set {
				if (value == null) {
					throw new ArgumentNullException("value cannot be null");
				}

				if (value.Channel != this._channel) {
					throw new ArgumentException("Setting a configuration with a channel " +
												"other than the potentiometer's channel is not permitted.");
				}

				DeviceControlChannel chan = DeviceControlChannel.ValueOf(this._channel);
				DeviceControllerTerminalConfiguration devcon = new DeviceControllerTerminalConfiguration(chan,
																	value.IsChannelEnabled, value.IsPinAEnabled,
																	value.IsPinWEnabled, value.IsPinBEnabled);
				this._controller.SetTerminalConfiguration(devcon);
			}
		}

		/// <summary>
		/// Gets whether the device is a potentiometer or a rheostat.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is rheostat; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsRheostat { get; }
		#endregion

		#region Instance Methods
		/// <summary>
		/// Raises the wiper action event event.
		/// </summary>
		/// <param name="e">
		/// The wiper event arguments.
		/// </param>
		private void OnWiperActionEvent(WiperEventArgs e) {
			if (this.WiperActionEvent != null) {
				this.WiperActionEvent(this, e);
			}
		}

		/// <summary>
		/// The potentiometer base wiper action event handler. This sets the channel
		/// value for either volatile or non-volatile wipers (depending on volatilty
		/// mode).
		/// </summary>
		/// <param name="sender">
		/// The object that fired the event.
		/// </param>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		private void MicrochipPotentiometerBase_WiperActionEvent(object sender, WiperEventArgs e)
		{
			// Do nothing if no event specified.
			if (e == null) {
				return;
			}

			// For volatile wiper.
			switch (this._nonVolMode) {
				case MicrochipPotNonVolatileMode.VolatileOnly:
				case MicrochipPotNonVolatileMode.VolatileAndNonVolatile:
					e.SetChannelValue(MicrochipPotDeviceController.VOLATILE_WIPER);
					break;
				case MicrochipPotNonVolatileMode.NonVolatileOnly:
				default:
					break;
			}

			// For non-volatile wiper.
			switch (this._nonVolMode) {
				case MicrochipPotNonVolatileMode.NonVolatileOnly:
				case MicrochipPotNonVolatileMode.VolatileAndNonVolatile:
					e.SetChannelValue(MicrochipPotDeviceController.NONVOLATILE_WIPER);
					break;
				case MicrochipPotNonVolatileMode.VolatileOnly:
				default:
					break;
			}
		}

		/// <summary>
		/// Adjusts the given value according to the boundaries (0 and
		/// <see cref="CyrusBuilt.MonoPi.Components.Potentiometers.Microchip.MicrochipPotentiometerBase.MaxValue"/>).
		/// </summary>
		/// <param name="val">
		/// The wiper's value to be set.
		/// </param>
		/// <returns>
		/// A valid wiper value.
		/// </returns>
		private Int32 GetValueAccordingBoundaries(Int32 val) {
			Int32 newVal = 0;
			if (val < 0) {
				newVal = 0;
			}
			else if (val > this.MaxValue) {
				newVal = this.MaxValue;
			}
			else {
				newVal = val;
			}
			return newVal;
		}

		/// <summary>
		/// Sets the non-volatility mode.
		/// </summary>
		/// <param name="mode">
		/// The way non-volatile reads or writes are done.
		/// </param>
		/// <remarks>
		/// The visibility of this method is protected because not all
		/// devices support non-volatile wipers. Any derived class may
		/// publish this method.
		/// </remarks>
		/// <exception cref="InvalidOperationException">
		/// This device is not capable of non-volatile wipers.
		/// </exception>
		protected void SetNonVolatileMode(MicrochipPotNonVolatileMode mode) {
			if ((!this.IsNonVolatileWiperCapable) &&
				(this._nonVolMode != MicrochipPotNonVolatileMode.VolatileOnly)) {
				throw new InvalidOperationException("This device is not capable of non-volatile wipers." +
													" You *must* use MicrochipPotNonVolatileMode.VolatileOnly.");
			}
			this._nonVolMode = mode;
		}

		/// <summary>
		/// Updates the cache to the wiper's value.
		/// </summary>
		/// <returns>
		/// The wiper's current value.
		/// </returns>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		public Int32 UpdateCacheFromDevice() {
			this._currentValue = this._controller.GetValue(DeviceControlChannel.ValueOf(this._channel), false);
			return this._currentValue;
		}

		/// <summary>
		/// Gets the non-volatile wiper's value.
		/// </summary>
		/// <returns>
		/// The non-volatile wiper's value.
		/// </returns>
		/// <remarks>
		/// The visibility of this method is protected because not all
		/// devices support non-volatile wipers. Any derived class may
		/// publish this method.
		/// </remarks>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// The device is not capable of non-volatile wipers.
		/// </exception>
		protected Int32 GetNonVolatileValue() {
			if (!this.IsNonVolatileWiperCapable) {
				throw new InvalidOperationException("This device is not capable of non-volatile wipers!");
			}
			return this._controller.GetValue(DeviceControlChannel.ValueOf(this._channel), true);
		}

		/// <summary>
		/// Determines whether or not the specified channel is supported by
		/// the underlying device.
		/// </summary>
		/// <param name="channel">
		/// The channel to check.
		/// </param>
		/// <returns>
		/// <c>true</c> if this instance is channel supported the specified channel; otherwise, <c>false</c>.
		/// </returns>
		public Boolean IsChannelSupported(MicrochipPotChannel channel) {
			Boolean supported = false;
			foreach (MicrochipPotChannel chan in this.SupportedChannels) {
				if (channel == chan) {
					supported = true;
					break;
				}
			}
			return supported;
		}

		/// <summary>
		/// Initializes the wiper to a defined status. For devices capable of non-volatile
		/// wipers, the non-volatile value is loaded. For devices not capable, the given
		/// value is set in the device.
		/// </summary>
		/// <param name="initialValForNonVolWipers">
		/// The initial value for devices not capable of non-volatile wipers.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		protected void Initialize(Int32 initialValForNonVolWipers) {
			DeviceControlChannel ctrlchan = DeviceControlChannel.ValueOf(this._channel);
			if (this.IsNonVolatileWiperCapable) {
				this._currentValue = this._controller.GetValue(ctrlchan, false);
			}
			else {
				Int32 newInitialVolWiperVal = this.GetValueAccordingBoundaries(initialValForNonVolWipers);
				this._controller.SetValue(DeviceControlChannel.ValueOf(this._channel),
											newInitialVolWiperVal,
											MicrochipPotDeviceController.VOLATILE_WIPER);
				this._currentValue = newInitialVolWiperVal;
			}
		}

		/// <summary>
		/// Decreases the wiper's value by the specified number of
		/// steps. It is not an error if the wiper hits or already
		/// hit the lower boundary (0). In such situations, the
		/// wiper sticks to the lower boundary or doesn't change.
		/// </summary>
		/// <param name="steps">
		/// The number of steps to decrease by.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		public void Decrease(Int32 steps) {
			if (this._currentValue == 0) {
				return;
			}

			if (steps < 0) {
				throw new ArgumentException("Only positive values are permitted.", "steps");
			}

			if (this.NonVolatileMode != MicrochipPotNonVolatileMode.VolatileOnly) {
				throw new InvalidOperationException("Decrease is only permitted for volatile-only wipers.");
			}

			// Check boundaries.
			Int32 actualSteps = steps;
			if (steps > this._currentValue) {
				actualSteps = this._currentValue;
			}

			Int32 newVal = (this._currentValue - actualSteps);
			if ((newVal == 0) || (steps > 5)) {
				this.CurrentValue = newVal;
			}
			else {
				this._controller.Decrease(DeviceControlChannel.ValueOf(this._channel), actualSteps);
				this._currentValue = newVal;
			}
		}

		/// <summary>
		/// Decreases the wiper's value by one step. It is not an error
		/// if the wiper already hit the lower boundary (0). In this
		/// situation, the wiper doesn't change.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		public void Decrease() {
			this.Decrease(1);
		}

		/// <summary>
		/// Increases the wiper's value by the specified number of steps.
		/// It is not an error if the wiper hits or already hit the upper
		/// boundary. In such situations, the wiper sticks to the upper
		/// boundary or doesn't change.
		/// </summary>
		/// <param name="steps">
		/// How many steps to increase.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		public void Increase(Int32 steps) {
			Int32 maxVal = this.MaxValue;
			if (this._currentValue == maxVal) {
				return;
			}

			if (steps < 0) {
				throw new ArgumentException("Only positive values are permitted.", "steps");
			}

			if (this.NonVolatileMode != MicrochipPotNonVolatileMode.VolatileOnly) {
				throw new InvalidOperationException("Increase only permitted for volatile-only wipers.");
			}

			// Check boundaries.
			Int32 actualSteps = steps;
			if ((steps + this._currentValue) > maxVal) {
				actualSteps = (maxVal - this._currentValue);
			}

			Int32 newVal = (this._currentValue + actualSteps);
			if ((newVal == maxVal) || (steps > 5)) {
				this.CurrentValue = newVal;
			}
			else {
				this._controller.Increase(DeviceControlChannel.ValueOf(this._channel), actualSteps);
				this._currentValue = newVal;
			}
		}

		/// <summary>
		/// Increase the wiper's value by one step. It is not an error
		/// if the wiper already hit the upper boundary. In this
		/// situation, the wiper doesn't change.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Communication with the device failed.
		/// </exception>
		public void Increase() {
			this.Increase(1);
		}

		/// <summary>
		/// Enables or disables the wiper lock.
		/// </summary>
		/// <param name="enabled">
		/// Set true to enable.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		public void SetWiperLock(Boolean enabled) {
			this._controller.SetWiperLock(DeviceControlChannel.ValueOf(this._channel), enabled);
		}

		/// <summary>
		/// Enables or disables write-protection for devices capable of
		/// non-volatile memory. Enabling write-protection does not only
		/// protect non-volatile wipers, it also protects any other
		/// non-volatile information stored (i.e. wiper-locks).
		/// </summary>
		/// <param name="enabled">
		/// Set true to enable.
		/// </param>
		/// <exception cref="System.IO.IOException">
		/// Communication with device failed or malformed result.
		/// </exception>
		public void SetWriteProtection(Boolean enabled) {
			this._controller.SetWriteProtection(enabled);
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Builds the I2C bus address of the device based on which which address
		/// pins are set.
		/// </summary>
		/// <param name="pinA0">
		/// Whether the device's address pin A0 is high (true) or low (false).
		/// </param>
		/// <param name="pinA1">
		/// Whether the device's address pin A1 (if available) is high (true) or low (false).
		/// </param>
		/// <param name="pinA2">
		/// Whether the device's address pin A2 (if available) is high (true) or low (false).
		/// </param>
		/// <returns>
		/// The I2C-address based on the address-pins given.
		/// </returns>
		protected static Int32 BuildI2CAddress(Boolean pinA0, Boolean pinA1, Boolean pinA2) {
			// Constant component.
			Int32 i2cAddress = 0x0101000;

			// Dynamic component if device knows A0.
			if (pinA0) {
				i2cAddress |= 0x0000001;
			}

			// Dynamic component if device knows A1.
			if (pinA1) {
				i2cAddress |= 0x0000010;
			}

			// Dynamic component if device knows A2.
			if (pinA2) {
				i2cAddress |= 0x0000100;
			}
			return i2cAddress;
		}
		#endregion
	}
}

