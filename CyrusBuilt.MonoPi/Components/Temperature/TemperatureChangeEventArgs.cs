//
//  TemperatureChangeEventArgs.cs
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

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// Temperature change event arguments class.
	/// </summary>
	public class TemperatureChangeEventArgs : EventArgs
	{
		#region Fields
		private Double _oldTemp = 0;
		private Double _newTemp = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CyrusBuilt.MonoPi.Components.Temperature.TemperatureChangeEventArgs"/> class
		/// class with the old and new temperature values.
		/// </summary>
		/// <param name="oldTemp">
		/// The temperature value prior to the change event.
		/// </param>
		/// <param name="newTemp">
		/// The temperature value since the change event.
		/// </param>
		public TemperatureChangeEventArgs(Double oldTemp, Double newTemp)
			: base() {
			this._oldTemp = oldTemp;
			this._newTemp = newTemp;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the previous temperature value.
		/// </summary>
		public Double OldTemp {
			get { return this._oldTemp; }
		}

		/// <summary>
		/// The current temperature value.
		/// </summary>
		public Double NewTemp {
			get { return this._newTemp; }
		}

		/// <summary>
		/// Gets the temperature change.
		/// </summary>
		public Double TemperatureChange {
			get { return (this._newTemp - this._oldTemp); }
		}
		#endregion
	}
}

