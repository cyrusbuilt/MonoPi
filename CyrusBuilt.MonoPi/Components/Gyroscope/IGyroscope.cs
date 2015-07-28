//
//  Gyroscope.cs
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
	/// A single-axis gyroscope device abstraction component interface.
	/// </summary>
	public interface IGyroscope : IComponent
	{
		#region Properties
		/// <summary>
		/// Gets the angular velocity.
		/// </summary>
		/// <value>
		/// The angular velocity.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		float AngularVelocity { get; }

		/// <summary>
		/// Gets or sets the raw value.
		/// </summary>
		/// <value>
		/// The raw value.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		Int32 RawValue { get; set; }

		/// <summary>
		/// Gets or sets the offset value, which is the value the gyro
		/// outputs when not rotating.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		Int32 Offset { get; set; }

		/// <summary>
		/// Gets or sets the gyro angle (angular position).
		/// </summary>
		/// <value>
		/// The angle.
		/// </value>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		float Angle { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Recalibrates the offset.
		/// </summary>
		void RecalibrateOffset();

		/// <summary>
		/// Sets the read trigger.
		/// </summary>
		/// <param name="readTrigger">
		/// The trigger mode to re-read the gyro value.
		/// </param>
		void SetReadTrigger(GyroTriggerMode readTrigger);
		#endregion
	}
}

