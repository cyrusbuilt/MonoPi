//
//  MultiAxisGyro.cs
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
	/// A multi-axis gyroscope device abstraction component interface.
	/// </summary>
	public interface IMultiAxisGyro : IComponent
	{
		/// <summary>
		/// Gets the time difference (delta) since the last loop.
		/// </summary>
		/// <value>
		/// The time delta.
		/// </value>
		Int32 TimeDelta { get; }

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
		IGyroscope Init(IGyroscope triggeringAxis, GyroTriggerMode mode);

		/// <summary>
		/// Enables the gyro.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void Enable();

		/// <summary>
		/// Disables the gyro.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void Disable();

		/// <summary>
		/// Reads the gyro and stores the value internally.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// An error occurred while reading from the gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void ReadGyro();

		/// <summary>
		/// Recalibrates the offset.
		/// </summary>
		/// <exception cref="System.IO.IOException">
		/// Unable to write to gyro.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		void RecalibrateOffset();
	}
}

