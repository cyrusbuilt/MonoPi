//
//  DimmableLightBase.cs
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

namespace CyrusBuilt.MonoPi.Components.Lights
{
	/// <summary>
	/// Base class for dimmable light component abstractions.
	/// </summary>
	public abstract class DimmableLightBase : ComponentBase, IDimmableLight
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Lights.DimmableLightBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected DimmableLightBase()
			: base() {
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the light state changed.
		/// </summary>
		public event LightStateChangeEventHandler StateChanged;

		/// <summary>
		/// Occurs when the light level changed.
		/// </summary>
		public event LightLevelChangeEventHandler LevelChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this light is on; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOn { 
			get { return this.Level > this.MinLevel; } 
		}

		/// <summary>
		/// Gets a value indicating whether this light is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if this light is off; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOff {
			get { return this.Level <= this.MinLevel; }
		}

		/// <summary>
		/// Gets or sets the brightness level.
		/// </summary>
		/// <value>
		/// The brightness level.
		/// </value>
		public abstract Int32 Level { get; set; }

		/// <summary>
		/// Gets the minimum brightness level.
		/// </summary>
		public abstract Int32 MinLevel { get; }

		/// <summary>
		/// Gets the maximum brightness level.
		/// </summary>
		public abstract Int32 MaxLevel { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Switches the light on.
		/// </summary>
		public void On() {
			this.Level = this.MaxLevel;
		}

		/// <summary>
		/// Switches the light off.
		/// </summary>
		public void Off() {
			this.Level = this.MinLevel;
		}

		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(LightStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the level changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnLevelChanged(LightLevelChangeEventArgs e) {
			if (this.LevelChanged != null) {
				this.LevelChanged(this, e);
			}
		}

		/// <summary>
		/// Gets the current brightness level percentage.
		/// </summary>
		/// <returns>
		/// The brightness percentage level.
		/// </returns>
		/// <param name="level">
		/// The brightness level.
		/// </param>
		public float GetLevelPercentage(Int32 level) {
			Int32 min = Math.Min(this.MinLevel, this.MaxLevel);
			Int32 max = Math.Max(this.MinLevel, this.MaxLevel);
			Int32 range = (max - min);
			float percentage = ((level * 100) / range);
			return percentage;
		}

		/// <summary>
		/// Gets the current brightness level percentage.
		/// </summary>
		/// <returns>
		/// The brightness percentage level.
		/// </returns>
		public float GetLevelPercentage() {
			return this.GetLevelPercentage(this.Level);
		}
		#endregion
	}
}

