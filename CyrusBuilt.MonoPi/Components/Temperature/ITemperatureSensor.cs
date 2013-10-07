//
//  ITemperatureSensor.cs
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
using CyrusBuilt.MonoPi.Sensors;

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// An abstract temperature sensor interface.
	/// </summary>
	public interface ITemperatureSensor : IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		Boolean IsDisposed { get; }

		/// <summary>
		/// Gets the temperature.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		Double Temperature { get; }

		/// <summary>
		/// Gets the temperature scale.
		/// </summary>
		TemperatureScale Scale { get; }

		/// <summary>
		/// Gets the temperature value.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to get the temperature measurement in.
		/// </param>
		/// <exception cref="ObjectDisposedException">
		/// This instance has been disposed.
		/// </exception>
		Double GetTemperature(TemperatureScale scale);

		/// <summary>
		/// Gets or sets the name of the sensor.
		/// </summary>
		String Name { get; set; }

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		Object Tag { get; set; }

		/// <summary>
		/// Gets or sets the temperature sensor.
		/// </summary>
		DS1620 Sensor { get; set; }
	}
}

