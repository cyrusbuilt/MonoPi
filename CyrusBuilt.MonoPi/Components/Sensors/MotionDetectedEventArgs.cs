//
//  MotionDetectedEventArgs.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.Components.Sensors
{
	/// <summary>
	/// Motion detected event arguments class.
	/// </summary>
	public class MotionDetectedEventArgs : EventArgs
	{
		#region Fields
		private Boolean _motionDetected = false;
		private DateTime _timestamp = DateTime.MinValue;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionDetectedEventArgs"/>
		/// class with a flag indicating whether or not motion was detected and the
		/// timestamp of when the state changed.
		/// </summary>
		/// <param name="motion">
		/// A flag indicating whether or not motion was detected.
		/// </param>
		/// <param name="timestamp">
		/// The timestamp of when the state changed.
		/// </param>
		public MotionDetectedEventArgs(Boolean motion, DateTime timestamp)
			: base() {
			this._motionDetected = motion;
			this._timestamp = timestamp;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.MonoPi.Components.Sensors.MotionDetectedEventArgs"/>
		/// detected motion.
		/// </summary>
		/// <value>
		/// <c>true</c> if motion detected; otherwise, <c>false</c>.
		/// </value>
		public Boolean MotionDetected {
			get { return this._motionDetected; }
		}

		/// <summary>
		/// Gets the timestamp.
		/// </summary>
		public DateTime Timestamp {
			get { return this._timestamp; }
		}
		#endregion
	}
}

