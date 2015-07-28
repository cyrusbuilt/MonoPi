//
//  IPCA9685Pin.cs
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

namespace CyrusBuilt.MonoPi.IO.PCA
{
	/// <summary>
	/// Represents a PCA9685 pin abstraction interface.
	/// </summary>
	public interface IPCA9685Pin : IPin
	{
		/// <summary>
		/// Gets or sets the PWM value that constitutes the ON position.
		/// </summary>
		/// <value>
		/// The PWM value that will be considered ON (high).
		/// </value>
		Int32 PwmOnValue { get; set; }

		/// <summary>
		/// Gets or sets the PWM value that constitutes the OFF position.
		/// </summary>
		/// <value>
		/// The PWM value that will be considered OFF (low).
		/// </value>
		Int32 PwmOffValue { get; set; }
	}
}

