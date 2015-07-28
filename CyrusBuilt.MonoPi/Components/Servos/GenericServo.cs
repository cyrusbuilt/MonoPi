//
//  GenericServo.cs
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
using System.Globalization;
using System.Text;

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// A component that is an abstraction of a servo device. This is an implementation
	/// of <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo"/>.
	/// </summary>
	public class GenericServo : Servo
	{
		#region Constants
		/// <summary>
		/// Minimum PWM value in microseconds (900).
		/// </summary>
		public const float PWM_MIN = 900;

		/// <summary>
		/// Neutral PWM value in microseconds (1500).
		/// </summary>
		public const float PWM_NEUTRAL = 1500;

		/// <summary>
		/// Maximum PWM value in microseconds (2100).
		/// </summary>
		public const float PWM_MAX = 2100;
		#endregion

		#region Fields
		private IServoDriver _driver = null;
		private float _pos = 0.0f;
		private Int32 _duration = 0;
		private float _pwmDurationEndPointLeft = -1;
		private float _pwmDurationNeutral = -1;
		private float _pwmDurationEndPointRight = -1;
		private Boolean _isReverse = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/>
		/// class with the servo driver, name, and properties.
		/// </summary>
		/// <param name="driver">
		/// The servo driver to use.
		/// </param>
		/// <param name="name">
		/// The name of this servo.
		/// </param>
		/// <param name="props">
		/// A dictionary containing servo properties.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified end point property value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MIN"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MAX"/>.
		/// - or -
		/// The specified subtrim property value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_LEFT"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_RIGHT"/>.
		/// </exception>
		public GenericServo(IServoDriver driver, String name, Dictionary<String, String> props)
			: base() {
			this._driver = driver;
			base.Name = name;
			if ((props != null) && (props.Count > 0)) {
				foreach (KeyValuePair<String, String> entry in props) {
					this.SetProperty(entry.Key, entry.Value);
				}
			}

			this.Init();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/>
		/// class with the servo driver and name. This overload will use default property values.
		/// </summary>
		/// <param name="driver">
		/// The servo driver to use.
		/// </param>
		/// <param name="name">
		/// The name of this servo.
		/// </param>
		public GenericServo(IServoDriver driver, String name)
			: this(driver, name, null) {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._driver != null) {
				this._driver.Dispose();
				this._driver = null;
			}
				
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the servo driver this servo is attached to.
		/// </summary>
		/// <value>
		/// The servo driver.
		/// </value>
		public override IServoDriver ServoDriver {
			get { return this._driver; }
		}

		/// <summary>
		/// Gets or sets the servo's desired position.
		/// </summary>
		/// <value>
		/// The position value in percentage. E.g.: A value of -100 would
		/// force the servo to travel to it's maximum left position. The
		/// value should be between -100 and +100.
		/// </value>
		/// <returns>
		/// The current position value between -100 and +100[%].
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified position value must be between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.POS_MAX_LEFT"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.POS_MAX_RIGHT"/>.
		/// </exception>
		public override float Position {
			get { return this._pos; }
			set {
				this.ValidatePosition(value);
				this._pos = value;
				this._duration = this.CalcPwmDuration(this._pos);
				this._driver.ServoPulseWidth = this._duration;
			}
		}

		/// <summary>
		/// Gets the PWM duration.
		/// </summary>
		/// <value>
		/// The PWM duration.
		/// </value>
		protected Int32 PwmDuration {
			get { return this._duration; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Validates the specified end point. If the end point is invalid an
		/// <see cref="ArgumentOutOfRangeException"/> is thrown.
		/// </summary>
		/// <param name="endpoint">
		/// The endpoint to validate.
		/// </param>
		/// <param name="propName">
		/// The endpoint property name.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified end point value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MIN"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MAX"/>.
		/// </exception>
		private void ValidateEndPoint(float endpoint, String propName) {
			if ((endpoint < END_POINT_MIN) || (endpoint > END_POINT_MAX)) {
				String errMsg = "Value of property [" + propName + "] must be between " +
								END_POINT_MIN.ToString() + " and " + END_POINT_MAX.ToString() +
								" but it is [" + endpoint.ToString() + "]";
				throw new ArgumentOutOfRangeException(errMsg);
			}
		}

		/// <summary>
		/// Validates the specified subtrim value. If the subtrim is invalid, then an
		/// <see cref="ArgumentOutOfRangeException"/> is thrown.
		/// </summary>
		/// <param name="subtrim">
		/// The subtrim value to validate.
		/// </param>
		/// <param name="propName">
		/// The subtrim property name.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified subtrim value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_LEFT"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_RIGHT"/>.
		/// </exception>
		private void ValidateSubTrim(float subtrim, String propName) {
			if ((subtrim < SUBTRIM_MAX_LEFT) || (subtrim > SUBTRIM_MAX_RIGHT)) {
				String errMsg = "Value of property [" + propName + "] must be between " +
				                SUBTRIM_MAX_LEFT.ToString() + " and " + SUBTRIM_MAX_RIGHT.ToString() +
				                " but is [" + subtrim.ToString() + "]";
				throw new ArgumentOutOfRangeException(errMsg);
			}
		}

		/// <summary>
		/// Validates the specified position value. If the position is invalid, then an
		/// <see cref="ArgumentOutOfRangeException"/> is thrown.
		/// </summary>
		/// <param name="pos">
		/// The position value to validate.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified position value must be between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.POS_MAX_LEFT"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.POS_MAX_RIGHT"/>.
		/// </exception>
		private void ValidatePosition(float pos) {
			if ((pos < POS_MAX_LEFT) || (pos > POS_MAX_RIGHT)) {
				String errMsg = "Position [" + pos.ToString() + "] must be between " +
				                POS_MAX_LEFT.ToString() + "(%) and +" + POS_MAX_RIGHT.ToString() +
				                "(%) but is [" + pos.ToString() + "]";
				throw new ArgumentOutOfRangeException(errMsg);
			}
		}

		/// <summary>
		/// Calculates the duration of the neutral pwm.
		/// </summary>
		/// <returns>
		/// PWM pulse duration in microseconds for neutral position considering subtrim.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The subtrim property value is not within the acceptable range (SUBTRIM_MAX_LEFT
		/// and SUBTRIM_MAX_RIGHT).
		/// </exception>
		public float CalcNeutralPwmDuration() {
			float val = 0f;
			float neutralVal = PROP_SUBTRIM_DEFAULT;
			if (float.TryParse(base.PropertyCollection[PROP_SUBTRIM], out val)) {
				neutralVal = val;
			}

			this.ValidateSubTrim(neutralVal, PROP_SUBTRIM);
			return (PWM_NEUTRAL + neutralVal);
		}

		/// <summary>
		/// Calculates end point PWM duration for the specified servo orientation.
		/// </summary>
		/// <returns>
		/// The end point PWM duration.
		/// </returns>
		/// <param name="orientation">
		/// The servo orientation.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// An unsupported orientation was specified.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The end point property value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MIN"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MAX"/>.
		/// </exception>
		public float CalcEndPointPwmDuration(ServoOrientation orientation) {
			float result = 0f;
			String propName = String.Empty;
			switch (orientation) {
				case ServoOrientation.Left:
					propName = PROP_END_POINT_LEFT;
					break;
				case ServoOrientation.Right:
					propName = PROP_END_POINT_RIGHT;
					break;
				default:
					throw new InvalidOperationException("Unsupported orientation: " + Enum.GetName(typeof(ServoOrientation), orientation));
			}

			float val = 0f;
			float epValue = PROP_END_POINT_DEFAULT;
			if (float.TryParse(base.PropertyCollection[propName], out val)) {
				epValue = val;
				this.ValidateEndPoint(epValue, propName);
			}

			float computedPwmDuration = 0f;
			float p = ((PWM_MAX - PWM_NEUTRAL) / 150 * epValue);
			float neutral = this.CalcNeutralPwmDuration();
			if (orientation == ServoOrientation.Left) {
				computedPwmDuration = (neutral - p);
			}
			else {
				computedPwmDuration = (neutral + p);
			}

			if (computedPwmDuration < PWM_MIN) {
				result = PWM_MIN;
			}
			else if (computedPwmDuration > PWM_MAX) {
				result = PWM_MAX;
			}
			else {
				result = computedPwmDuration;
			}
			return result;
		}

		/// <summary>
		/// Calculates the PWM duration.
		/// </summary>
		/// <param name="pos">
		/// Position value between -100 and +100%.
		/// </param>
		/// <returns>
		/// The PWM pulse duration in servo driver resolution.
		/// </returns>
		public Int32 CalcPwmDuration(float pos) {
			if (this._isReverse) {
				pos = -pos;
			}

			float result = this._pwmDurationNeutral;
			if (pos < 0) {
				result += ((this._pwmDurationNeutral - this._pwmDurationEndPointLeft) * pos / 100.00f); 
			}
			else if (pos > 0) {
				result += ((this._pwmDurationEndPointRight - this._pwmDurationNeutral) * pos / 100.00f);
			}

			return (Int32)((result * this._driver.ServoPulseResolution) / 1000);
		}

		/// <summary>
		/// Initializes the servo.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified end point property value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MIN"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.END_POINT_MAX"/>.
		/// - or -
		/// The specified subtrim property value is not between
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_LEFT"/>
		/// and <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo.SUBTRIM_MAX_RIGHT"/>.
		/// </exception>
		protected void Init() {
			this._pwmDurationEndPointLeft = this.CalcEndPointPwmDuration(ServoOrientation.Left);
			this._pwmDurationEndPointRight = this.CalcEndPointPwmDuration(ServoOrientation.Right);
			this._pwmDurationNeutral = this.CalcNeutralPwmDuration();
			Boolean outBool = false;
			if (Boolean.TryParse(base.PropertyCollection[PROP_IS_REVERSE], out outBool)) {
				this._isReverse = outBool;
			}
		}

		/// <summary>
		/// Sets the value of the specified property. If the property does not already exist
		/// in the property collection, it will be added. Once set, the servo will
		/// re-initialize using the new property values.
		/// </summary>
		/// <param name="key">
		/// The property name (key).
		/// </param>
		/// <param name="value">
		/// The value to assign to the property.
		/// </param>
		public override void SetProperty(String key, String value) {
			base.SetProperty(key, value);
			this.Init();
		}
			
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.Servos.GenericServo"/>.
		/// </returns>
		public override String ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("Position [" + this._pos.ToString() + "]");
			foreach (KeyValuePair<String, String> entry in base.PropertyCollection) {
				sb.Append(", " + entry.Key + " [" + entry.Value + "]");
			}
			return sb.ToString();
		}
		#endregion
	}
}

