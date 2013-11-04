//
//  SprinklerControllerBase.cs
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
using System.Collections.Generic;

namespace CyrusBuilt.MonoPi.Devices.Sprinkler
{
	/// <summary>
	/// Sprinkler controller base class.
	/// </summary>
	public abstract class SprinklerControllerBase : DeviceBase, ISprinklerController
	{
		private List<ISprinklerZone> _zones = null;

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected SprinklerControllerBase()
			: base() {
			this._zones = new List<ISprinklerZone>();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/>
		/// object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/>. The
		/// <see cref="Dispose"/> method leaves
		/// the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/> so the garbage
		/// collector can reclaim the memory that the <see cref="CyrusBuilt.MonoPi.Devices.Sprinkler.SprinklerControllerBase"/>
		/// was occupying.
		/// </remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._zones != null) {
				this._zones.Clear();
				this._zones = null;
			}
			this.ZoneStateChanged = null;
			base.Dispose();
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a zone state changes.
		/// </summary>
		public event ZoneStateChangeEventHandler ZoneStateChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the zone count.
		/// </summary>
		public Int32 ZoneCount {
			get { return this._zones.Count; }
		}

		/// <summary>
		/// Gets the zones assigned to this controller.
		/// </summary>
		public List<ISprinklerZone> Zones {
			get { return this._zones; }
		}

		/// <summary>
		/// Gets a value indicating whether this controller is on.
		/// </summary>
		public Boolean IsOn {
			get {
				Boolean isOn = false;
				foreach (ISprinklerZone zone in this._zones) {
					isOn = zone.IsOn;
					if (isOn) {
						break;
					}
				}
				return isOn;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the sprinklers are raining.
		/// </summary>
		public abstract Boolean IsRaining { get; }

		/// <summary>
		/// Gets a value indicating whether this controller is off.
		/// </summary>
		public Boolean IsOff {
			get { return !this.IsOn; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the zone state changed event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnZoneStateChanged(ZoneStateChangeEventArgs e) {
			if (this.ZoneStateChanged != null) {
				this.ZoneStateChanged(this, e);
			}
		}

		/// <summary>
		/// Determines whether the specified zone is on.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the zone is on; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="zone">
		/// The zone to check.
		/// </param>
		public Boolean IsOnForZone(Int32 zone) {
			return this._zones[zone].IsOn;
		}

		/// <summary>
		/// Determines whether the specified zone is off.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the zone is off; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="zone">
		/// The zone to check.
		/// </param>
		public Boolean IsOffForZone(Int32 zone) {
			return this._zones[zone].IsOff;
		}

		/// <summary>
		/// Turns the specified zone on.
		/// </summary>
		/// <param name="zone">
		/// The zone to turn on.
		/// </param>
		public void On(Int32 zone) {
			if (this.IsOffForZone(zone)) {
				this._zones[zone].On();
				this.OnZoneStateChanged(new ZoneStateChangeEventArgs(false, true, zone));
			}
		}

		/// <summary>
		/// Turns all zones on.
		/// </summary>
		public void OnAllZones() {
			foreach (ISprinklerZone zone in this._zones) {
				if (zone.IsOff) {
					zone.On();
					this.OnZoneStateChanged(new ZoneStateChangeEventArgs(false, true, this._zones.IndexOf(zone)));
				}
			}
		}

		/// <summary>
		/// Turns the specified zone off.
		/// </summary>
		/// <param name="zone">
		/// The zone to turn off.
		/// </param>
		public void Off(Int32 zone) {
			if (this.IsOnForZone(zone)) {
				this._zones[zone].Off();
				this.OnZoneStateChanged(new ZoneStateChangeEventArgs(true, false, zone));
			}
		}

		/// <summary>
		/// Turns off all zones.
		/// </summary>
		public void OffAllZones() {
			foreach (ISprinklerZone zone in this._zones) {
				if (zone.IsOn) {
					zone.Off();
					this.OnZoneStateChanged(new ZoneStateChangeEventArgs(true, false, this._zones.IndexOf(zone)));
				}
			}
		}

		/// <summary>
		/// Sets the state of the specified zone.
		/// </summary>
		/// <param name="zone">
		/// The zone to set the state of.
		/// </param>
		/// <param name="on">
		/// Set true to turn on the specified zone.
		/// </param>
		public void SetState(Int32 zone, Boolean on) {
			Boolean oldState = this.IsOnForZone(zone);
			this._zones[zone].SetState(on);
			this.OnZoneStateChanged(new ZoneStateChangeEventArgs(oldState, on, zone));
		}
		#endregion
	}
}

