//
//  PCA9685GpioProvider.cs
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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CyrusBuilt.MonoPi.IO;
using CyrusBuilt.MonoPi.IO.I2C;

namespace CyrusBuilt.MonoPi.IO.PCA
{
	/// <summary>
	/// A GPIO provider that implements the PCA9685 16-channel, 12-bit PWM I2C-bus
	/// LED/Servo controller as native MonoPi GPIO pins. The PCA9685 is connected
	/// via I2C connection to the Raspberry Pi and provides 16 PWM pins that can
	/// be used for PWM output.
	/// </summary>
	/// <remarks>
	/// More information about the PCA9685 can be found here:<br/>
	/// <a href="http://www.nxp.com/documents/data_sheet/PCA9685.pdf">PCA9685.pdf</a>
	/// <br/><br/>And especially about the board here:<br/>
	/// <a href="http://www.adafruit.com/products/815">Adafruit 16-Channel 12-bit PWM/Servo Driver</a>
	/// </remarks>
	public class PCA9685GpioProvider : IDisposable
	{
		#region Constants
		/// <summary>
		/// The name of this provider.
		/// </summary>
		public const String NAME = "CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider";

		/// <summary>
		/// A description of the provider.
		/// </summary>
		public const String DESCRIPTION = "PCA9685 PWM Provider";

		/// <summary>
		/// The PCA9685's internal clock frequency (25MHz).
		/// </summary>
		public const Int32 INTERNAL_CLOCK_FREQ = (25 * 1000 * 1000);

		/// <summary>
		/// The minimum PWM frequency (40Hz).
		/// </summary>
		public const Decimal MIN_FREQUENCY = 40;

		/// <summary>
		/// The maximum PWM frequency (1KHz).
		/// </summary>
		public const Decimal MAX_FREQUENCY = 1000;

		/// <summary>
		/// Analog servo frequency. This value would result in a period duration
		/// frequency of ~22ms which is safe for all types of servos.
		/// </summary>
		public const Decimal ANALOG_SERVO_FREQ = 45.454M;

		/// <summary>
		/// Digital servo frequency. This value would result in a period duration
		/// of ~11ms which is recommended when using digital servos only.
		/// </summary>
		public const Decimal DIGITAL_SERVO_FREQ = 90.909M;

		/// <summary>
		/// The default frequency (same as ANALOG_SERVO_FREQ).
		/// </summary>
		public const Decimal DEFAULT_FREQUENCY = ANALOG_SERVO_FREQ;

		/// <summary>
		/// Total number of steps for a 12bit controller (4096).
		/// </summary>
		public const Int32 PWM_STEPS = 4096;
		#endregion

		#region Registers
		private const Byte PCA9685_MODE1 = 0x00;
		private const Byte PCA9685_PRESCALE = 0xFE;
		private const Byte PCA9685_LED0_ON_L = 0x06;
		private const Byte PCA9685_LED0_ON_H = 0x07;
		private const Byte PCA9685_LED0_OFF_L = 0x08;
		private const Byte PCA9685_LED0_OFF_H = 0x09;
		#endregion

		#region Fields
		private Boolean _isDisposed = false;
		private II2CBus _busDevice = null;
		private Decimal _freq = 0;
		private Int32 _periodDurationMicros = 0;
		private Int32 _busAddr = 0;
		private List<IPCA9685Pin> _pinCache = null;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/>
		/// class with the I2C bus device, bus address of PCA9685 device,
		/// target frequency, and frequency correction factor.
		/// </summary>
		/// <param name="bus">
		/// The I2C bus device used to communicate with the PCA9685 chip.
		/// </param>
		/// <param name="address">
		/// The address of the PCA9685 on the I2C bus.
		/// </param>
		/// <param name="targetFreq">
		/// The target PWM frequency to set. 
		/// </param>
		/// <param name="freqCorrectionFactor">
		/// The PWM frequency correction factor.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="bus"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="targetFreq"/> must be between 40Hz and 1000Hz.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// The specified I2C bus instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public PCA9685GpioProvider(II2CBus bus, Int32 address, Decimal targetFreq, Decimal freqCorrectionFactor) {
			if (bus == null) {
				throw new ArgumentNullException("bus");
			}
			this._pinCache = new List<IPCA9685Pin>();
			this._busAddr = address;
			this._busDevice = bus;
			if (!this._busDevice.IsOpen) {
				this._busDevice.Open();
			}
			this._busDevice.WriteBytes(this._busAddr, new Byte[] { PCA9685_MODE1, (Byte)0x00 });
			this.SetFrequency(targetFreq, freqCorrectionFactor);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/>
		/// class with the I2C bus device, bus address of PCA9685 device,
		/// and target frequency.
		/// </summary>
		/// <param name="bus">
		/// The I2C bus device used to communicate with the PCA9685 chip.
		/// </param>
		/// <param name="address">
		/// The address of the PCA9685 on the I2C bus.
		/// </param>
		/// <param name="targetFreq">
		/// The target PWM frequency to set. 
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="targetFreq"/> must be between 40Hz and 1000Hz.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// The specified I2C bus instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public PCA9685GpioProvider(II2CBus bus, Int32 address, Decimal targetFreq)
			: this(bus, address, targetFreq, Decimal.One) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/>
		/// class with the I2C bus device and bus address of PCA9685 device.
		/// This overload uses the default target frequency value.
		/// </summary>
		/// <param name="bus">
		/// The I2C bus device used to communicate with the PCA9685 chip.
		/// </param>
		/// <param name="address">
		/// The address of the PCA9685 on the I2C bus.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// The specified I2C bus instance has been disposed and can no longer be used.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public PCA9685GpioProvider(II2CBus bus, Int32 address)
			: this(bus, address, DEFAULT_FREQUENCY) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/>
		/// class with the Raspberry Pi board revision and address of the
		/// PCA9685 on the I2C bus.
		/// </summary>
		/// <param name="rev">
		/// The Raspiberry Pi board revision being used. This is used to determine
		/// which bus to use for I2C.
		/// </param>
		/// <param name="address">
		/// The address of the PCA9685 on the I2C bus.
		/// </param>
		/// <exception cref="IOException">
		/// An I/O error occurred. The specified address is inacessible or the
		/// I2C transaction failed.
		/// </exception>
		public PCA9685GpioProvider(BoardRevision rev, Int32 address)
			: this(new I2CBus(rev), address) {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.IO.PCA.PCA9685GpioProvider"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._busDevice != null) {
				this._busDevice.Dispose();
				this._busDevice = null;
			}

			if (this._pinCache != null) {
				this._pinCache.Clear();
				this._pinCache = null;
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
		/// Gets the current PWM pulse frequency.
		/// </summary>
		public Decimal Frequency {
			get { return this._freq; }
		}

		/// <summary>
		/// Gets the pin cache.
		/// </summary>
		/// <value>
		/// The internal cache of PCA9685 pins.
		/// </value>
		public List<IPCA9685Pin> PinCache {
			get { return this._pinCache; }
		}

		/// <summary>
		/// Gets the period duration in microseconds.
		/// </summary>
		/// <value>
		/// The period duration in microseconds.
		/// </value>
		public Int32 PeriodDurationMicros {
			get { return this._periodDurationMicros; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the specified pin from the pin cache. If the specified pin
		/// does not already exist in the cache, it will be cached first,
		/// then returned to the caller.
		/// </summary>
		/// <param name="pin">
		/// The pin to get from the cache.
		/// </param>
		/// <returns>
		/// The cached instance of the specified pin.
		/// </returns>
		public IPCA9685Pin GetPin(IPCA9685Pin pin) {
			if (!this._pinCache.Contains(pin)) {
				this._pinCache.Add(pin);
			}
			return this._pinCache[this._pinCache.IndexOf(pin)];
		}

		/// <summary>
		/// Determines whether this instance has pin the specified pin
		/// in the internal pin cache.
		/// </summary>
		/// <param name="pin">
		/// The pin to check for.
		/// </param>
		/// <returns>
		/// <c>true</c> if this instance has pin the specified pin; otherwise, <c>false</c>.
		/// </returns>
		public Boolean HasPin(IPCA9685Pin pin) {
			return this._pinCache.Contains(pin);
		}

		/// <summary>
		/// Validates the frequency. Throws an <see cref="ArgumentOutOfRangeException"/>
		/// if the frequency is not in range.
		/// </summary>
		/// <param name="freq">
		/// The frequency to validate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="freq"/> must be between 40Hz and 1000Hz.
		/// </exception>
		private void ValidateFrequency(Decimal freq) {
			if ((freq.CompareTo(MIN_FREQUENCY) == -1) || (freq.CompareTo(MAX_FREQUENCY) == 1)) {
				throw new ArgumentOutOfRangeException("Frequency [" + freq.ToString() + "] must be between 40.0 and 1000.0 Hz.");
			}
		}

		/// <summary>
		/// Validates the specified PWM value and throws an
		/// <see cref="ArgumentOutOfRangeException"/> if the value is not in range..
		/// </summary>
		/// <param name="pwm">
		/// The PWM value to validate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="pwm"/> must be between 0 and 4095.
		/// </exception>
		private void ValidatePwmValueInRange(Int32 pwm) {
			if ((pwm < 0) || (pwm > 4095)) {
				throw new ArgumentOutOfRangeException("PWM position value [" + pwm.ToString() + "] must be between 0 and 4095.");
			}
		}

		/// <summary>
		/// Validates the duration of the PWM.
		/// </summary>
		/// <param name="duration">
		/// The duration value to validate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The duration is less than 1 or less than or equal to the period duration.
		/// </exception>
		private void ValidatePwmDuration(Int32 duration) {
			if (duration < 1) {
				throw new ArgumentOutOfRangeException("Duration [" + duration.ToString() + "] must be >= 1us.");
			}

			if (duration >= this._periodDurationMicros) {
				throw new ArgumentOutOfRangeException("Duration [" + duration.ToString() + "] must be <= period duration [" +
					this._periodDurationMicros.ToString() + "].");
			}
		}

		/// <summary>
		/// Validates the specified pin and throws an exception if invalid.
		/// </summary>
		/// <param name="pin">
		/// The pin to validate.
		/// </param>
		/// <param name="onPosition">
		/// The PWM value to be considered in an "on" state.
		/// </param>
		/// <param name="offPosition">
		/// The PWM value to be considered in an "off" state.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="pin"/> does not exist in the pin cache.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The specified pin is not in PWM mode.
		/// </exception>
		private void ValidatePin(IPCA9685Pin pin, Int32 onPosition, Int32 offPosition) {
			if (pin == null) {
				throw new ArgumentNullException("pin");
			}

			if (!this.HasPin(pin)) {
				throw new ArgumentException("The specified pin does not exist in the pin cache.");
			}

			if (pin.Mode != PinMode.PWM) {
				String errMsg = "Invalid pin mode [" + Enum.GetName(typeof(PinMode), pin.Mode) +
				                "]; Unable to set PWM value " + onPosition.ToString() + ", " +
								offPosition.ToString();
				throw new InvalidPinModeException(pin, errMsg);
			}
		}

		/// <summary>
		/// Calculates the duration of the period.
		/// </summary>
		/// <returns>
		/// The period duration.
		/// </returns>
		private Int32 CalcPeriodDuration() {
			Decimal tFreq = Decimal.Divide(1000000, this._freq);
			tFreq = Decimal.Round(tFreq, 0, MidpointRounding.ToEven);
			return Convert.ToInt32(tFreq);
		}

		/// <summary>
		/// Calculates the prescale based on the internal clock frequency and
		/// the specified frequency correction factor.
		/// </summary>
		/// <param name="freqCorrectionFactor">
		/// The frequency correction factor.
		/// </param>
		/// <returns>
		/// The prescale value.
		/// </returns>
		private Int32 CalcPrescale(Decimal freqCorrectionFactor) {
			Decimal steps = new Decimal(PWM_STEPS);
			Decimal theoreticalPrescale = new Decimal(INTERNAL_CLOCK_FREQ);
			theoreticalPrescale = Decimal.Round(Decimal.Divide(theoreticalPrescale, steps), 3, MidpointRounding.AwayFromZero);
			theoreticalPrescale = Decimal.Round(Decimal.Divide(theoreticalPrescale, this._freq), 0, MidpointRounding.AwayFromZero);
			theoreticalPrescale -= Decimal.One;
			return (Int32)(theoreticalPrescale * freqCorrectionFactor);
		}

		/// <summary>
		/// Calculates the OFF position for the specified pulse duration.
		/// </summary>
		/// <param name="duration">
		/// Pulse duration in microseconds.
		/// </param>
		/// <returns>
		/// The OFF position (value between 1 and 4095).
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The duration is less than 1 or less than or equal to the period duration.
		/// </exception>
		public Int32 CalcOffPosForPulseDuration(Int32 duration) {
			this.ValidatePwmDuration(duration);
			Int32 result = ((PWM_STEPS - 1) * duration / this._periodDurationMicros);
			if (result < 1) {
				result = 1;
			}
			else if (result > (PWM_STEPS - 1)) {
				result = (PWM_STEPS - 1);
			}
			return result;
		}

		/// <summary>
		/// Caches the pin values.
		/// </summary>
		/// <param name="pin">
		/// The pin to cache the on/off values for.
		/// </param>
		/// <param name="onPosition">
		/// The PWM threshold to consider the pin "on".
		/// </param>
		/// <param name="offPosition">
		/// The PWM threshold to consider the pin "off".
		/// </param>
		private void CachePinValues(IPCA9685Pin pin, Int32 onPosition, Int32 offPosition) {
			this.GetPin(pin).PwmOnValue = onPosition;
			this.GetPin(pin).PwmOffValue = offPosition;
		}

		/// <summary>
		/// The built-in oscillator runs at ~25MHz. For better accuracy, this method
		/// can be used to provide a correction factor to meet desired frequency.
		/// </summary>
		/// <param name="targetFreq">
		/// The desired frequency.
		/// </param>
		/// <param name="freqCorrectionFactor">
		/// Actual frequency/target frequency.
		/// </param>
		/// <remarks>
		/// Note: Correction is limited to a certain degree because the calculated
		/// prescale value has to be rounded to an integer value!<br/>
		/// <b>Example:</b><br/>
		/// Target frequency: 50Hz<br/>
		/// Actual frequency: 52.93Hz<br/>
		/// Correction factor: 52.93 / 50 = 1.0586
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="targetFreq"/> must be between 40Hz and 1000Hz.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// No open connection to the PCA9585 I2C device.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred while trying to access I2C address.
		/// </exception>
		public void SetFrequency(Decimal targetFreq, Decimal freqCorrectionFactor) {
			this.ValidateFrequency(targetFreq);
			this._freq = targetFreq;
			this._periodDurationMicros = this.CalcPeriodDuration();
			Int32 prescale = this.CalcPrescale(freqCorrectionFactor);
			this._busDevice.WriteByte(this._busAddr, PCA9685_MODE1); // Set the register to read from.
			Int32 oldMode = (Int32)this._busDevice.Read(this._busAddr); // Read the mode from register.
			Int32 newMode = (oldMode & 0x7F) | 0x10; // Sleep
			this._busDevice.WriteBytes(this._busAddr, new Byte[] { PCA9685_MODE1, (Byte)newMode });     // Go to sleep
			this._busDevice.WriteBytes(this._busAddr, new Byte[] { PCA9685_PRESCALE, (Byte)prescale }); // Set prescale
			this._busDevice.WriteBytes(this._busAddr, new Byte[] { PCA9685_MODE1, (Byte)oldMode });
			Thread.Sleep(1);
			this._busDevice.WriteBytes(this._busAddr, new Byte[] { PCA9685_MODE1, (Byte)(oldMode | 0x80) });
		}

		/// <summary>
		/// Sets the target frequency (accuracy is around +/- 5%!).
		/// </summary>
		/// <param name="freq">
		/// The desired PWM frequency.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="freq"/> must be between 40Hz and 1000Hz.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// No open connection to the PCA9585 I2C device.
		/// </exception>
		/// <exception cref="IOException">
		/// An I/O error occurred while trying to access I2C address.
		/// </exception>
		public void SetFrequency(Decimal freq) {
			this.SetFrequency(freq, Decimal.One);
		}

		/// <summary>
		/// Sets the PWM values for the specified pin. The LEDn_ON and LEDn_OFF
		/// counts can vary from 0 to 4095 max.<br/>
		/// The LEDn_ON and LEDn_OFF count registers should never be programmed
		/// with the same values.<br/><br/>
		/// Because the loading of the LEDn_ON and LEDn_OFF registers is via I2C-bus,
		/// and asynchronous to the internal oscillator, it is best to ensure that
		/// there are no visual artifacts of changing the ON and OFF values. This
		/// is achieved by updating the changes at the end of the LOW cycle.
		/// </summary>
		/// <param name="pin">
		/// The pin to set the PWM values for (Channel 0 .. 15).
		/// </param>
		/// <param name="onPos">
		/// The PWM threshold to consider the pin "on" (value between 0 and 4095).
		/// </param>
		/// <param name="offPos">
		/// The PWM threshold to consider the pin "off" (value between 0 and 4095).
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="pin"/> does not exist in the pin cache. - or -
		/// <paramref name="onPos"/> and <paramref name="offPos"/> are equal.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The specified pin is not in PWM mode.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to write to the specified channel (pin address).
		/// </exception>
		public void SetPWM(IPCA9685Pin pin, Int32 onPos, Int32 offPos) {
			this.ValidatePin(pin, onPos, offPos);
			this.ValidatePwmValueInRange(onPos);
			this.ValidatePwmValueInRange(offPos);
			if (onPos == offPos) {
				throw new ArgumentException("ON [" + onPos.ToString() + "] and OFF [" + offPos.ToString() + "] values must be different.");
			}

			Int32 chan = pin.Address;
			try {
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_L + 4 * chan), (Byte)((Byte)onPos & 0xFF) });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_H + 4 * chan), (Byte)((Byte)onPos >> 8) });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_L + 4 * chan), (Byte)((Byte)offPos & 0xFF) });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_H + 4 * chan), (Byte)((Byte)offPos >> 8) });
			}
			catch (IOException ioEx) {
				throw new IOException("Unable to write to channel: " + chan.ToString(), ioEx);
			}

			this.CachePinValues(pin, onPos, offPos);
		}

		/// <summary>
		/// Sets the pulse duration in microseconds.<br/>
		/// Make sure duration doesn't exceed period time (1,000,000/freq)!
		/// </summary>
		/// <param name="pin">
		/// The pin to set the pulse duration for (Channel 0 .. 15).
		/// </param>
		/// <param name="duration">
		/// Pulse duration in microseconds.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="pin"/> does not exist in the pin cache.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The specified pin is not in PWM mode.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to write to the specified channel (pin address).
		/// </exception>
		public void SetPWM(IPCA9685Pin pin, Int32 duration) {
			Int32 offPos = this.CalcOffPosForPulseDuration(duration);
			this.SetPWM(pin, duration);
		}
		/// <summary>
		/// Permanently sets the output to HIGH (no PWM anymore).<br/>
		/// The LEDn_ON_H output control bit 4, when set to logic 1, causes
		/// the output to be always ON.
		/// </summary>
		/// <param name="pin">
		/// The pin to set always on (Channel 0 .. 15).
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="pin"/> does not exist in the pin cache.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The specified pin is not in PWM mode.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to write to the specified channel (pin address).
		/// </exception>
		public void SetAlwaysOn(IPCA9685Pin pin) {
			this.ValidatePin(pin, 1, 0);
			Int32 chan = pin.Address;
			try {
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_L + 4 * chan), (Byte)0x00 });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_H + 4 * chan), (Byte)0x10 }); // set bit 4 to high
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_L + 4 * chan), (Byte)0x00 });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_H + 4 *chan), (Byte)0x00 });
			}
			catch (IOException ioEx) {
				throw new IOException("Error while trying to write to channel: " + chan.ToString(), ioEx);
			}

			this.CachePinValues(pin, 1, 0);
		}

		/// <summary>
		/// Sets the always off.
		/// </summary>
		/// <param name="pin">
		/// The pin to set always off (Channel 0 .. 15).
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="pin"/> does not exist in the pin cache.
		/// </exception>
		/// <exception cref="InvalidPinModeException">
		/// The specified pin is not in PWM mode.
		/// </exception>
		/// <exception cref="IOException">
		/// Unable to write to the specified channel (pin address).
		/// </exception>
		public void SetAlwaysOff(IPCA9685Pin pin) {
			this.ValidatePin(pin, 1, 0);
			Int32 chan = pin.Address;
			try {
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_L + 4 * chan), (Byte)0x00 });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_ON_H + 4 * chan), (Byte)0x00 });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_L + 4 * chan), (Byte)0x00 });
				this._busDevice.WriteBytes(this._busAddr, new Byte[] { (Byte)(PCA9685_LED0_OFF_H + 4 * chan), (Byte)0x10 });
			}
			catch (IOException ioEx) {
				throw new IOException("Error while trying to write to channel: " + chan.ToString(), ioEx);
			}
			this.CachePinValues(pin, 1, 0);
		}

		/// <summary>
		/// Gets the PWM on/off values for the specified pin.
		/// </summary>
		/// <param name="pin">
		/// The pin to get the on/off values for (Channel 0 .. 15).
		/// </param>
		/// <returns>
		/// A tuple containing the on/off PWM values where Item1 contains the
		/// "on" value and Item2 contains the "Off" value.
		/// </returns>
		public Tuple<Int32, Int32> GetPwmOnOffValues(IPCA9685Pin pin) {
			if (!this.HasPin(pin)) {
				throw new ArgumentException("The specified pin does not exist in the pin cache.");
			}

			return new Tuple<Int32, Int32>(
				this.GetPin(pin).PwmOnValue,
				this.GetPin(pin).PwmOffValue
			);
		}

		/// <summary>
		/// Resets all outputs (set to always OFF).
		/// </summary>
		public void Reset() {
			foreach (IPCA9685Pin pin in PCA9685Pin.ALL) {
				this.SetAlwaysOff(pin);
			}
		}
		#endregion
	}
}

