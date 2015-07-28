//
//  Servo.cs
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

namespace CyrusBuilt.MonoPi.Components.Servos
{
	/// <summary>
	/// Represents characteristics of an R/C servo.
	/// </summary>
	public abstract class Servo : ComponentBase
	{
		#region Limits
		/// <summary>
		/// Consider this to be the maximum left position of an R/C-radio stick (-100.0f).
		/// </summary>
		public const float POS_MAX_LEFT = -100.0f;

		/// <summary>
		/// Consider this to be the neutral position of an R/C-radio stick (0.0f).
		/// </summary>
		public const float POS_NEUTRAL = 0.0f;

		/// <summary>
		/// Consider this to be the maximum right position of an R/C-radio stick (100.0f).
		/// </summary>
		public const float POS_MAX_RIGHT = 100.0f;

		/// <summary>
		/// The minimum end point value (0.0f)
		/// </summary>
		public const float END_POINT_MIN = 0.0f;

		/// <summary>
		/// The maximum end point value (150.0f).
		/// </summary>
		public const float END_POINT_MAX = 150.0f;

		/// <summary>
		/// Consider this the maximum right subtrim value (-200.0f).
		/// </summary>
		public const float SUBTRIM_MAX_LEFT = -200.0f;

		/// <summary>
		/// Consider this the neutral subtrim value (0.0f)
		/// </summary>
		public const float SUBTRIM_NEUTRAL = 0.0f;

		/// <summary>
		/// Consider this the maximum right subtrim value (200.0f).
		/// </summary>
		public const float SUBTRIM_MAX_RIGHT = 200.0f;
		#endregion

		#region Property Names
		/// <summary>
		/// Left end point property name. This property should be an integer value
		/// between 0 and 150.<br/>
		/// 0 = No travel<br/>
		/// 150 = Maximum travel (0.9ms pulse)
		/// </summary>
		public const String PROP_END_POINT_LEFT = "endPointLeft";

		/// <summary>
		/// Right end point property name. This property should be an integer value
		/// between 0 and 150.<br/>
		/// 0 = No travel<br/>
		/// 150 = Maximum travel (2.1ms pulse).
		/// </summary>
		public const String PROP_END_POINT_RIGHT = "endPointRight";

		/// <summary>
		/// Subtrim propery name. This property should be a value between -200 and
		/// +200.<br/>
		/// -200 = Neutral position changed to 1.3ms<br/>
		/// 0 = Neutral position changed at 1.5ms<br/>
		/// +200 = Neutral position changed to 1.7ms<br/>
		/// End points will be adjusted accordingly!
		/// </summary>
		public const String PROP_SUBTRIM = "subtrim";

		/// <summary>
		/// "isReverse" property name. If this propery value is set to TRUE, then
		/// the servo travelling direction is reversed.<br/>
		/// <b>Note:</b> subtrim and endpoints will not change!
		/// </summary>
		public const String PROP_IS_REVERSE = "isReverse";
		#endregion

		#region Property Defaults
		/// <summary>
		/// End point property value default (100.0f).
		/// </summary>
		public const float PROP_END_POINT_DEFAULT = POS_MAX_RIGHT;

		/// <summary>
		/// Subtrim property value default (0.0f).
		/// </summary>
		public const float PROP_SUBTRIM_DEFAULT = SUBTRIM_NEUTRAL;

		/// <summary>
		/// "isReverse" property value default (false).
		/// </summary>
		public const Boolean PROP_IS_REVERSE_DEFAULT = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Servos.Servo"/>
		/// class. This is the default constructor.
		/// </summary>
		protected Servo()
			: base() {
		}
		#endregion

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
		public abstract float Position { get; set; }

		/// <summary>
		/// Gets the servo driver this servo is attached to.
		/// </summary>
		/// <value>
		/// The servo driver.
		/// </value>
		public abstract IServoDriver ServoDriver { get; }
	}
}

