//
//  AxisGyroscope.cs
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

namespace CyrusBuilt.MonoPi.Components.Gyroscope
{
	/// <summary>
	/// A generic gyroscope device abstraction component.
	/// </summary>
	public class AxisGyroscope : ComponentBase, IGyroscope
	{
		#region Fields
		private IMultiAxisGyro _multiAxisGyro = null;
		private GyroTriggerMode _trigger = GyroTriggerMode.ReadNotTriggered;
		private Int32 _value = 0;
		private Int32 _offset = 0;
		private float _angle = 0f;
		private float _degPerSecondFactor = 0f;
		private Boolean _factorSet = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/>
		/// class with the multi-axis gyro to read from.
		/// </summary>
		/// <param name="multiAxisGyro">
		/// The multi-axis gyro to read from.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="multiAxisGyro"/> cannot be null.
		/// </exception>
		public AxisGyroscope(IMultiAxisGyro multiAxisGyro)
			: base() {
			if (multiAxisGyro == null) {
				throw new ArgumentNullException("multiAxisGyro");
			}
			this._multiAxisGyro = multiAxisGyro;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/>
		/// class with the multi-axis gyro to read from and the
		/// degrees-per-second factor value.
		/// </summary>
		/// <param name="multiAxisGyro">
		/// The multi-axis gyro to read from.
		/// </param>
		/// <param name="degPerSecondFactor">
		/// The degrees-per-second factor value.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="multiAxisGyro"/> cannot be null.
		/// </exception>
		public AxisGyroscope(IMultiAxisGyro multiAxisGyro, float degPerSecondFactor)
			: this(multiAxisGyro) {
			this._degPerSecondFactor = degPerSecondFactor;
			this._factorSet = true;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Components.Gyroscope.AxisGyroscope"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._multiAxisGyro != null) {
				this._multiAxisGyro.Dispose();
				this._multiAxisGyro = null;
			}

			this._trigger = GyroTriggerMode.ReadNotTriggered;
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the raw value.
		/// </summary>
		/// <value>
		/// The raw value.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		public Int32 RawValue {
			get {
				if (this._trigger == GyroTriggerMode.GetRawValueTriggerRead) {
					this.ReadAndUpdateAngle();	
				}
				return this._value;
			}
			set { this._value = value; }
		}

		/// <summary>
		/// Gets or sets the offset value, which is the value the gyro
		/// outputs when not rotating.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public Int32 Offset {
			get { return this._offset; }
			set { this._offset = value; }
		}

		/// <summary>
		/// Gets or sets the gyro angle (angular position).
		/// </summary>
		/// <value>
		/// The angle.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		public float Angle {
			get {
				if (this._trigger == GyroTriggerMode.GetAngleTriggerRead) {
					this.ReadAndUpdateAngle();
				}
				return this._angle;
			}
			set { this._angle = value; }
		}

		/// <summary>
		/// Gets the angular velocity.
		/// </summary>
		/// <value>
		/// The angular velocity.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		public float AngularVelocity {
			get {
				if (this._trigger == GyroTriggerMode.GetAngularVelocityTriggerRead) {
					return this.ReadAndUpdateAngle();
				}
				else {
					Int32 adjusted = (this._value - this._offset);
					if (this._factorSet) {
						return (adjusted / this._degPerSecondFactor);
					}
					return adjusted;
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reads the and updates angle.
		/// </summary>
		/// <returns>
		/// The angular velocity of the gyro.
		/// </returns>
		public float ReadAndUpdateAngle() {
			this._multiAxisGyro.ReadGyro();
			Int32 adjusted = (((this._value - this._offset) / 40) * 40);
			float angularVelocity = adjusted;
			if (this._factorSet) {
				angularVelocity /= this._degPerSecondFactor;
			}

			this._angle = (this._angle + angularVelocity * this._multiAxisGyro.TimeDelta / 1000f);
			return angularVelocity;
		}

		/// <summary>
		/// Sets the read trigger.
		/// </summary>
		/// <param name="readTrigger">
		/// The trigger mode to re-read the gyro value.
		/// </param>
		public void SetReadTrigger(GyroTriggerMode readTrigger) {
			this._trigger = readTrigger;
		}

		/// <summary>
		/// Recalibrates the offset.
		/// </summary>
		public void RecalibrateOffset() {
			this._multiAxisGyro.RecalibrateOffset();
		}
		#endregion
	}
}

