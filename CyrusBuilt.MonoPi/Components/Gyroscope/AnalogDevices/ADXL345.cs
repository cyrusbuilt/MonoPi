//
//  ADXL345.cs
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
using System.Threading;
using CyrusBuilt.MonoPi.IO.I2C;
using CyrusBuilt.MonoPi.PiSystem;

namespace CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices
{
	/// <summary>
	/// Represents a device abstraction component for an Analog Devices ADXL345
	/// High Resolution 3-axis Accelerometer. 
	/// </summary>
	/// <remarks>
	/// See <a href="http://www.analog.com/media/en/technical-documentation/data-sheets/ADXL345.pdf">
	/// ADXL345.pdf</a> for details.
	/// </remarks>
	public class ADXL345 : ComponentBase, IMultiAxisGyro
	{
		#region Constants
		/// <summary>
		/// The default physical bus address of the ADXL345.
		/// </summary>
		public const Byte ADXL345_ADDRESS = 0x53;

		private const Int32 CALIBRATION_READS = 50;
		private const Int32 CALIBRATION_SKIPS = 5;
		#endregion

		#region Fields
		private II2CBus _device = null;
		private IGyroscope _x = null;
		private IGyroscope _y = null;
		private IGyroscope _z = null;
		private AxisGyroscope _aX = null;
		private AxisGyroscope _aY = null;
		private AxisGyroscope _aZ = null;
		private Int32 _address = (Int32)ADXL345_ADDRESS;
		private Int32 _timeDelta = 0;
		private long _lastRead = 0L;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/>
		/// class with the I2C device that represents the physical connection
		/// to the gyro.
		/// </summary>
		/// <param name="device">
		/// The I2C device that represents the physical connection to the gyro.
		/// If null, then it is assumed that the host is a revision 2 or higher
		/// board and a default <see cref="CyrusBuilt.MonoPi.IO.I2C.I2CBus"/>
		/// using the rev 2 I2C bus path will be used instead.
		/// </param>
		/// <exception cref="IOException">
		/// Unable to open the specified I2C bus device.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// The specified device instance has been disposed.
		/// </exception>
		public ADXL345(II2CBus device)
			: base() {
			if (device == null) {
				device = new I2CBus(BoardRevision.Rev2);
			}

			this._device = device;
			if (!this._device.IsOpen) {
				this._device.Open();
			}

			this._x = new AxisGyroscope(this, 20f);
			this._y = new AxisGyroscope(this, 20f);
			this._z = new AxisGyroscope(this, 20f);

			this._aX = (AxisGyroscope)this._x;
			this._aY = (AxisGyroscope)this._y;
			this._aZ = (AxisGyroscope)this._z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/>
		/// class with the I2C device that represents the physical connection
		/// to the gyro and the bus address of the device.
		/// </summary>
		/// <param name="device">
		/// The I2C device that represents the physical connection to the gyro.
		/// If null, then it is assumed that the host is a revision 2 or higher
		/// board and a default <see cref="CyrusBuilt.MonoPi.IO.I2C.I2CBus"/>
		/// using the rev 2 I2C bus path will be used instead.
		/// </param>
		/// <param name="busAddress">
		/// The bus address of the device.
		/// </param>
		/// <exception cref="IOException">
		/// Unable to open the specified I2C bus device.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// The specified device instance has been disposed.
		/// </exception>
		public ADXL345(II2CBus device, Int32 busAddress)
			: this(device) {
			this._address = busAddress;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/>
		/// class. This is the default constructor. The default constructor
		/// assumes a Revision 2 or higher Raspberry Pi host and that the
		/// ADXL345 is at address <see cref="ADXL345.ADXL345_ADDRESS"/>.
		/// </summary>
		public ADXL345()
			: this(null) {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/>. The <see cref="Dispose"/> method
		/// leaves the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/> so the garbage collector can reclaim
		/// the memory that the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._device != null) {
				this._device.Dispose();
				this._device = null;
			}

			if (this._aX != null) {
				this._aX.Dispose();
				this._aX = null;
			}

			if (this._aY != null) {
				this._aY.Dispose();
				this._aY = null;
			}

			if (this._aZ != null) {
				this._aZ.Dispose();
				this._aZ = null;
			}

			if (this._x != null) {
				this._x.Dispose();
				this._x = null;
			}

			if (this._y != null) {
				this._y.Dispose();
				this._y = null;
			}

			if (this._z != null) {
				this._z.Dispose();
				this._z = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the time difference (delta) since the last loop.
		/// </summary>
		/// <value>
		/// The time delta.
		/// </value>
		public Int32 TimeDelta {
			get { return this._timeDelta; }
		}

		/// <summary>
		/// Gets a reference to the X axis.
		/// </summary>
		/// <value>
		/// The X axis.
		/// </value>
		public IGyroscope X {
			get { return this._x; }
		}

		/// <summary>
		/// Gets a reference to the Y axis.
		/// </summary>
		/// <value>
		/// The Y axis.
		/// </value>
		public IGyroscope Y {
			get { return this._y; }
		}

		/// <summary>
		/// Gets a reference to the Z axis.
		/// </summary>
		/// <value>
		/// The Z axis.
		/// </value>
		public IGyroscope Z {
			get { return this._z; }
		}

		/// <summary>
		/// Gets a reference to the X axis implementation.
		/// </summary>
		/// <value>
		/// The X axis implementation.
		/// </value>
		protected AxisGyroscope aX {
			get { return this._aX; }
		}

		/// <summary>
		/// Gets a reference to the Y axis implementation.
		/// </summary>
		/// <value>
		/// The Y axis implementation.
		/// </value>
		protected AxisGyroscope aY {
			get { return this._aY; }
		}

		/// <summary>
		/// Gets a reference to the Z axis implementation.
		/// </summary>
		/// <value>
		/// The Z axis implementation.
		/// </value>
		protected AxisGyroscope aZ {
			get { return this._aZ; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enables the gyro.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void Enable() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345");
			}
			this._device.WriteBytes(this._address, new Byte[] { (Byte)0x31, (Byte)0x0B });
		}

		/// <summary>
		/// Disables the gyro.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void Disable() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345");
			}
			// TODO HTF do you *disable*???
		}

		/// <summary>
		/// Initializes the Gyro.
		/// </summary>
		/// <param name="triggeringAxis">
		/// The gyro that represents the single axis responsible for
		/// the triggering of updates.
		/// </param>
		/// <param name="mode">
		/// The gyro update trigger mode.
		/// </param>
		/// <returns>
		/// Reference to the specified triggering axis, which may or may
		/// not have been modified.
		/// </returns>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public IGyroscope Init(IGyroscope triggeringAxis, GyroTriggerMode mode) {
			this.Enable();

			if (triggeringAxis == this.aX) {
				this.aX.SetReadTrigger(mode);
			}
			else {
				this.aX.SetReadTrigger(GyroTriggerMode.ReadNotTriggered);
			}

			if (triggeringAxis == this.aY) {
				this.aY.SetReadTrigger(mode);
			}
			else {
				this.aY.SetReadTrigger(GyroTriggerMode.ReadNotTriggered);
			}

			if (triggeringAxis == this.aZ) {
				this.aZ.SetReadTrigger(mode);
			}
			else {
				this.aZ.SetReadTrigger(GyroTriggerMode.ReadNotTriggered);
			}
			return triggeringAxis;
		}

		/// <summary>
		/// Reads the gyro and stores the value internally.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void ReadGyro() {
			if (base.IsDisposed) {
				throw new ObjectDisposedException("CyrusBuilt.MonoPi.Components.Gyroscope.AnalogDevices.ADXL345");
			}
			long now = SystemInfo.GetCurrentTimeMillis();
			this._timeDelta = (Int32)(now - this._lastRead);
			this._lastRead = now;

			this._device.WriteByte(this._address, (Byte)0x08);
			Thread.Sleep(10);

			Byte[] data = this._device.ReadBytes(this._address, 6);
			if (data.Length != 6) {
				throw new IOException("Couldn't read compass data; Return buffer size: " + data.Length.ToString());
			}

			this.aX.RawValue = ((data[0] & 0xff) << 8) + (data[1] & 0xff);
			this.aY.RawValue = ((data[2] & 0xff) << 8) + (data[3] & 0xff);
			this.aZ.RawValue = ((data[3] & 0xff) << 8) + (data[5] & 0xff);
		}

		/// <summary>
		/// Recalibrates the offset.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		public void RecalibrateOffset() {
			long totalX = 0;
			long totalY = 0;
			long totalZ = 0;

			Int32 minX = 10000;
			Int32 minY = 10000;
			Int32 minZ = 10000;

			Int32 maxX = -10000;
			Int32 maxY = -10000;
			Int32 maxZ = -10000;

			Int32 x = 0;
			Int32 y = 0;
			Int32 z = 0;

			for (Int32 i = 0; i < CALIBRATION_SKIPS; i++) {
				this.ReadGyro();
				Thread.Sleep(1);
			}

			for (Int32 j = 0; j < CALIBRATION_READS; j++) {
				this.ReadGyro();

				x = this.aX.RawValue;
				y = this.aY.RawValue;
				z = this.aZ.RawValue;

				totalX += x;
				totalY += y;
				totalZ += z;

				if (x < minX) {
					minX = x;
				}

				if (y < minY) {
					minY = y;
				}

				if (z < minZ) {
					minZ = z;
				}

				if (x > maxX) {
					maxX = x;
				}

				if (y > maxY) {
					maxY = y;
				}

				if (z > maxZ) {
					maxZ = z;
				}

				this.aX.Offset = (Int32)(totalX / CALIBRATION_READS);
				this.aY.Offset = (Int32)(totalY / CALIBRATION_READS);
				this.aZ.Offset = (Int32)(totalZ / CALIBRATION_READS);
			}
		}
		#endregion
	}
}

