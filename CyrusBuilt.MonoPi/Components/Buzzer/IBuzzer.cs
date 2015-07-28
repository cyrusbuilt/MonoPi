//
//  IBuzzer.cs
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

namespace CyrusBuilt.MonoPi.Components.Buzzer
{
	/// <summary>
	/// A piezo buzzer device abstraction component interface.
	/// </summary>
	public interface IBuzzer : IComponent
	{
		/// <summary>
		/// Start the buzzer at the specified frequency.
		/// </summary>
		/// <param name="freq">
		/// The frequency to buzz at.
		/// </param>
		void Buzz(UInt32 freq);

		/// <summary>
		/// Start the buzzer at the specified frequency for the specified duration.
		/// </summary>
		/// <param name="freq">
		/// The frequency to buzz at.
		/// </param>
		/// <param name="duration">
		/// The duration in milliseconds.
		/// </param>
		void Buzz(UInt32 freq, Int32 duration);

		/// <summary>
		/// Stops the buzzer.
		/// </summary>
		void Stop();
	}
}

