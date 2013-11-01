//
//  SprinklerBase.cs
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

namespace CyrusBuilt.MonoPi.Devices.Sprinkler
{
	/// <summary>
	/// Base class for sprinler zones.
	/// </summary>
	public abstract class SprinklerZoneBase : ISprinklerZone
	{
		private String _name = String.Empty;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerZoneBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected SprinklerZoneBase() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerZoneBase"/>
		/// class with the name of the zone.
		/// </summary>
		/// <param name="name">
		/// The name of the zone.
		/// </param>
		protected SprinklerZoneBase(String name) {
			this._name = name;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the zone.
		/// </summary>
		/// <value>
		/// The name of the zone.
		/// </value>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets whether or not this zone is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is on; otherwise, <c>false</c>.
		/// </value>
		public abstract Boolean IsOn { get; }

		/// <summary>
		/// Gets whether or not this zone is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is off; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOff {
			get { return !this.IsOn; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the state of this zone..
		/// </summary>
		/// <param name="on">
		/// Set true to turn the zone on or false to turn it off.
		/// </param>
		public abstract void SetState(Boolean on);

		/// <summary>
		/// Turns this zone on.
		/// </summary>
		public void On() {
			this.SetState(true);
		}

		/// <summary>
		/// Turns this zone off.
		/// </summary>
		public void Off() {
			this.SetState(false);
		}
		#endregion
	}
}

