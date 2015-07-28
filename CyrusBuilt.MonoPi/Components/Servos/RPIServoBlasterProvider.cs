//
//  RPIServoBlasterProvider.cs
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
using System.Text;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// A GPIO provider for servos driven via ServoBlaster. This is an implementation
	/// of <a href="Implementation of https://github.com/richardghirst/PiBits/tree/master/ServoBlaster">Implementation of https://github.com/richardghirst/PiBits/tree/master/ServoBlaster</a>
	/// </summary>
	public class RPIServoBlasterProvider : IServoProvider
	{
		// Default servo mapping of ServoBlaster's servod:
		//
		//    0 on P1-7           GPIO-4
		//    1 on P1-11          GPIO-17
		//    2 on P1-12          GPIO-18
		//    3 on P1-13          GPIO-27
		//    4 on P1-15          GPIO-22
		//    5 on P1-16          GPIO-23
		//    6 on P1-18          GPIO-24
		//    7 on P1-22          GPIO-25

		#region Constants
		/// <summary>
		/// The ServoBlaster device path.
		/// </summary>
		public const String SERVO_BLASTER_DEV = "/dev/servoblaster";

		/// <summary>
		/// The ServoBlaster config path.
		/// </summary>
		public const String SERVO_BLASTER_DEV_CFG = "/dev/servoblaster-cfg";

		/// <summary>
		/// The name of pin 3 on the P1 header.
		/// </summary>
		public const String PIN_P1_3 = "P1-3";

		/// <summary>
		/// The name of pin 5 on the P1 header.
		/// </summary>
		public const String PIN_P1_5 = "P1-5";

		/// <summary>
		/// The name of pin 7 on the P1 header.
		/// </summary>
		public const String PIN_P1_7 = "P1-7";

		/// <summary>
		/// The name of pin 11 on the P1 header.
		/// </summary>
		public const String PIN_P1_11 = "P1-11";

		/// <summary>
		/// The name of pin 12 on the P1 header.
		/// </summary>
		public const String PIN_P1_12 = "P1-12";

		/// <summary>
		/// The name of pin 13 on the P1 header.
		/// </summary>
		public const String PIN_P1_13 = "P1-13";

		/// <summary>
		/// The name of pin 15 on the P1 header.
		/// </summary>
		public const String PIN_P1_15 = "P1-15";

		/// <summary>
		/// The name of pin 16 on the P1 header.
		/// </summary>
		public const String PIN_P1_16 = "P1_16";

		/// <summary>
		/// The name of pin 17 on the P1 header.
		/// </summary>
		public const String PIN_P1_18 = "P1_18";

		/// <summary>
		/// The name of pin 19 on the P1 header.
		/// </summary>
		public const String PIN_P1_19 = "P1-19";

		/// <summary>
		/// The name of pin 21 on the P1 header.
		/// </summary>
		public const String PIN_P1_21 = "P1-21";

		/// <summary>
		/// The name of pin 22 on the P1 header.
		/// </summary>
		public const String PIN_P1_22 = "P1-22";

		/// <summary>
		/// The name of pin 23 on the P1 header.
		/// </summary>
		public const String PIN_P1_23 = "P1-23";

		/// <summary>
		/// The name of pin 24 on the P1 header.
		/// </summary>
		public const String PIN_P1_24 = "P1-24";

		/// <summary>
		/// The name of pin 26 on the P1 header.
		/// </summary>
		public const String PIN_P1_26 = "P1-26";


		/// <summary>
		/// The name of pin 3 on the P5 header.
		/// </summary>
		public const String PIN_P5_3 = "P5-3";

		/// <summary>
		/// The name of pin 5 on the P5 header.
		/// </summary>
		public const String PIN_P5_4 = "P5-4";

		/// <summary>
		/// The name of pin 5 on the P5 header.
		/// </summary>
		public const String PIN_P5_5 = "P5-5";

		/// <summary>
		/// The name of pin 6 on the P5 header.
		/// </summary>
		public const String PIN_P5_6 = "P5-6";
		#endregion

		#region Fields
		/// <summary>
		/// A pin map. Maps pin GPIO pins to pin names.
		/// </summary>
		public static Dictionary<IRaspiGpio, String> PIN_MAP = null;

		/// <summary>
		/// A reverse-order pin map. Maps pin names to GPIO pins.
		/// </summary>
		public static Dictionary<String, IRaspiGpio> REVERSE_PIN_MAP = null;

		private Boolean _isDisposed = false;
		private FileInfo _servoBlasterDev = null;
		private FileInfo _servoBlasterDevCfg = null;
		private StreamWriter _writer = null;
		private List<IPin> _servoPins = null;
		private Dictionary<IPin, RPIServoBlasterDriver> _allocatedDrivers = null;
		private static readonly Object _padLock = new Object();
		#endregion

		#region Events
		/// <summary>
		/// Occurs when an unrecognized pin is found in the ServoBlaster config.
		/// </summary>
		public event UnrecognizedPinFoundEventHandler UnrecognizedPinFound;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/>
		/// class. This is a static initializer to populate the pin maps (which are also static).
		/// This initializer will only be called *ONCE* before any static members are referenced
		/// and will not be called again during the lifetime of the application domain. As such,
		/// it is NOT advisable to dispose of the pin maps (clear the dictionaries and/or set null)
		/// unless you know what you are doing as they are only initialized once.
		/// </summary>
		static RPIServoBlasterProvider() {
			// Init pin maps.
			PIN_MAP = new Dictionary<IRaspiGpio, String>();
			REVERSE_PIN_MAP = new Dictionary<String, IRaspiGpio>();

			// Init P1 pins.
			DefinePin(new GpioMem(GpioPins.V2_GPIO08, PinMode.PWM), PIN_P1_3);
			DefinePin(new GpioMem(GpioPins.V2_GPIO09, PinMode.PWM), PIN_P1_5);
			DefinePin(new GpioMem(GpioPins.V2_GPIO07, PinMode.PWM), PIN_P1_7);
			DefinePin(new GpioMem(GpioPins.GPIO00, PinMode.PWM), PIN_P1_11);
			DefinePin(new GpioMem(GpioPins.GPIO01, PinMode.PWM), PIN_P1_12);
			DefinePin(new GpioMem(GpioPins.V2_GPIO02, PinMode.PWM), PIN_P1_13);
			DefinePin(new GpioMem(GpioPins.V2_GPIO03, PinMode.PWM), PIN_P1_15);
			DefinePin(new GpioMem(GpioPins.V2_GPIO04, PinMode.PWM), PIN_P1_16);
			DefinePin(new GpioMem(GpioPins.V2_Pin05, PinMode.PWM), PIN_P1_18);
			DefinePin(new GpioMem(GpioPins.V2_Pin12, PinMode.PWM), PIN_P1_19);
			DefinePin(new GpioMem(GpioPins.V2_Pin13, PinMode.PWM), PIN_P1_21);
			DefinePin(new GpioMem(GpioPins.V2_GPIO25, PinMode.PWM), PIN_P1_22);
			DefinePin(new GpioMem(GpioPins.V2_GPIO14, PinMode.PWM), PIN_P1_23);
			DefinePin(new GpioMem(GpioPins.V2_GPIO10, PinMode.PWM), PIN_P1_24);
			DefinePin(new GpioMem(GpioPins.V2_GPIO11, PinMode.PWM), PIN_P1_26);

			// Init P5 pins.
			DefinePin(new GpioMem(GpioPins.V2_P5_Pin03, PinMode.PWM), PIN_P5_3);
			DefinePin(new GpioMem(GpioPins.V2_P5_Pin04, PinMode.PWM), PIN_P5_4);
			DefinePin(new GpioMem(GpioPins.V2_P5_Pin05, PinMode.PWM), PIN_P5_5);
			DefinePin(new GpioMem(GpioPins.V2_P5_Pin06, PinMode.PWM), PIN_P5_6);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/>
		/// class. This is the default constructor.
		/// </summary>
		/// <exception cref="FileNotFoundException">
		/// Either the /dev/servoblaster or /dev/servoblaster-cfg path does not exist.
		/// Is ServoBlaster installed and running?
		/// </exception>
		public RPIServoBlasterProvider() {
			this._servoBlasterDev = new FileInfo(SERVO_BLASTER_DEV);
			if (!this._servoBlasterDev.Exists) {
				throw new FileNotFoundException("File " + SERVO_BLASTER_DEV + " is not present." +
				" Please check https://github.com/richardghirst/PiBits/tree/master/ServoBlaster for details.");
			}

			this._servoBlasterDevCfg = new FileInfo(SERVO_BLASTER_DEV_CFG);
			if (!this._servoBlasterDevCfg.Exists) {
				throw new FileNotFoundException("File " + SERVO_BLASTER_DEV_CFG + " is not present." +
					" Please check https://github.com/richardghirst/PiBits/tree/master/ServoBlaster for details.");
			}
			this._allocatedDrivers = new Dictionary<IPin, RPIServoBlasterDriver>();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/>. The <see cref="Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/> so the garbage collector can reclaim the
		/// memory that the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider"/> was occupying.</remarks>
		public void Dispose() {
			if (this._isDisposed) {
				return;
			}

			if (this._writer != null) {
				this._writer.Flush();
				this._writer.Dispose();
				this._writer = null;
			}

			if (this._allocatedDrivers != null) {
				lock (_padLock) {
					foreach (KeyValuePair<IPin, RPIServoBlasterDriver> entry in this._allocatedDrivers) {
						entry.Value.Dispose();
					}
					this._allocatedDrivers.Clear();
				}
				this._allocatedDrivers = null;
			}

			if (this._servoPins != null) {
				this._servoPins.Clear();
				this._servoPins = null;
			}
			this._isDisposed = true;
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
		/// Gets a list of pins this driver implementation can drive. If a pin
		/// is found in the ServoBlaster config that is not in the pin map,
		/// the <see cref="CyrusBuilt.MonoPi.Components.Servos.RPIServoBlasterProvider.UnrecognizedPinFound"/>
		/// event will be raised for each unknown pin found, but will not throw
		/// an exception so the remaining good pins can be returned in the list.
		/// </summary>
		/// <value>
		/// A list of defined servo pins.
		/// </value>
		/// <exception cref="IOException">
		/// An error occurred while trying to get the list of pins.
		/// </exception>
		public List<IPin> DefinedServoPins {
			get {
				if ((this._servoPins == null) || (this._servoPins.Count == 0)) {
					this._servoPins = this.GetServoPinsFromConfig();
				}
				return this._servoPins;
			}
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Gets a list of servo pins from the config and validates them
		/// against the pin map.
		/// </summary>
		/// <returns>
		/// A list of servo pins read from the config.
		/// </returns>
		/// <exception cref="IOException">
		/// An error occurred while trying to read the device config.
		/// </exception>
		private List<IPin> GetServoPinsFromConfig() {
			// Open the config for read access.
			List<IPin> servoPins = new List<IPin>();
			using (StreamReader sr = new StreamReader(File.OpenRead(this._servoBlasterDevCfg.FullName))) {
				Int32 i = 0;
				Int32 index = -1;
				String pin = String.Empty;
				String p1pins = String.Empty;
				String p5pins = String.Empty;
				IRaspiGpio gpio = null;
				Boolean mappingStarted = false;

				// Process each line of the config.
				String line = sr.ReadLine();
				while (line != null) {
					// Basically what we are doing here is reading
					// each of the pin definitions in the ServoBlaster
					// config to make sure the config matches our
					// pin map. If we find a pin that is not in the
					// pin map, then we throw an exception.
					if (mappingStarted) {
						line = line.Trim();
						i = line.IndexOf(" on ");
						if (i > 0) {
							if (Int32.TryParse(line.Substring(0, i), out index)) {
								pin = line.Substring(i + 4).Trim();
								i = pin.IndexOf(' ');
								pin = pin.Substring(0, i);
								if (REVERSE_PIN_MAP.TryGetValue(pin, out gpio)) {
									if (index == servoPins.Count) {
										servoPins.Add(gpio);
									}
									else if (index > servoPins.Count) {
										while (servoPins.Count < index) {
											servoPins.Add(null);
										}
										servoPins.Add(gpio);
									}
									else {
										servoPins[index] = gpio;
									}
								}
								else {
									this.OnUnrecognizedPinFound(new UnrecognizedPinFoundEventArgs("Unrecognized pin in ServoBlaster config: " + pin));
								}
							}
						}
					}
					else {
						if ((line.StartsWith("p1pins=")) || (line.StartsWith("p5pins="))) {
							p1pins = line.Substring(7);
						}
						mappingStarted = line.Trim().Equals("Servo mapping:");
					}
					line = sr.ReadLine();
				}
			}
			return servoPins;
		}

		/// <summary>
		/// Raises the unrecognized pin found event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnUnrecognizedPinFound(UnrecognizedPinFoundEventArgs e) {
			if (this.UnrecognizedPinFound != null) {
				this.UnrecognizedPinFound(this, e);
			}
		}

		/// <summary>
		/// Ensures the writer is created.
		/// </summary>
		protected void EnsureWriterIsCreated() {
			lock (_padLock) {
				if (this._writer == null) {
					this._writer = new StreamWriter(File.OpenWrite(this._servoBlasterDev.FullName));
				}
			}
		}

		/// <summary>
		/// Gets a driver for the requested pin.
		/// </summary>
		/// <param name="servoPin">
		/// The pin the driver is needed for.
		/// </param>
		/// <returns>
		/// The servo driver assigned to the pin. May be null if no driver is
		/// assigned or if the pin is unknown.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The specified servo pin is not a defined servo pin.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// No driver is assigned to the specified pin - or - Cannot drive servo
		/// from specified pin - or - another initialization error occurred.
		/// </exception>
		public IServoDriver GetServoDriver(IPin servoPin) {
			Int32 index = this.DefinedServoPins.IndexOf(servoPin);
			if (index < 0) {
				throw new ArgumentException("Servo driver cannot drive pin " + servoPin.ToString());
			}

			RPIServoBlasterDriver driver = null;
			if (!this._allocatedDrivers.TryGetValue(servoPin, out driver)) {
				String pinName = String.Empty;
				if (PIN_MAP.TryGetValue((IRaspiGpio)servoPin, out pinName)) {
					driver = new RPIServoBlasterDriver((IRaspiGpio)servoPin, index, pinName, this);
					this.EnsureWriterIsCreated();
				}
			}
			return driver;
		}

		/// <summary>
		/// Updates the servo by writing the specified value to the specified pin.
		/// </summary>
		/// <param name="pinName">
		/// The name of the pin to write to.
		/// </param>
		/// <param name="value">
		/// The value to write to the pin.
		/// </param>
		/// <exception cref="IOException">
		/// Unable to write to the ServoBlaster device.
		/// </exception>
		public void UpdateServo(String pinName, Int32 value) {
			lock (_padLock) {
				StringBuilder sb = new StringBuilder();
				sb.Append(pinName).Append("=").Append(value.ToString()).Append("\n");
				try {
					this._writer.Write(sb.ToString());
					this._writer.Flush();
				}
				catch (IOException) {
					this._writer.Close();
				}

				try {
					this.EnsureWriterIsCreated();
					this._writer.Write(sb.ToString());
					this._writer.Flush();
				}
				catch (IOException ex) {
					throw new IOException("Failed to write to " + SERVO_BLASTER_DEV + " device.", ex);
				}
			}
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Helper method for adding pins to the pin maps.
		/// </summary>
		/// <param name="pin">
		/// The pin to add to both pin maps.
		/// </param>
		/// <param name="name">
		/// The pin name.
		/// </param>
		private static void DefinePin(IRaspiGpio pin, String name) {
			PIN_MAP.Add(pin, name);
			REVERSE_PIN_MAP.Add(name, pin);
		}
		#endregion
	}
}

