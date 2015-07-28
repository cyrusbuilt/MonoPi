//
//  PowerBase.cs
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

namespace CyrusBuilt.MonoPi.Components.Power
{
	/// <summary>
	/// Base class for power control device abstraction components.
	/// </summary>
	public abstract class PowerBase : ComponentBase, IPower
	{
		#region Events
		/// <summary>
		/// Occurs when the state changes.
		/// </summary>
		public event PowerStateEventEventHandler StateChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Power.PowerBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected PowerBase()
			: base() {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public abstract PowerState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is on; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOn {
			get { return this.State == PowerState.On; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is off; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsOff {
			get { return this.State == PowerState.Off; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Raises the <see cref="StateChanged"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnStateChanged(PowerStateChangeEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
			}
		}

		/// <summary>
		/// Switches the device on.
		/// </summary>
		public void On() {
			this.State = PowerState.On;
		}

		/// <summary>
		/// Switches the device off.
		/// </summary>
		public void Off() {
			this.State = PowerState.Off;
		}
		#endregion
	}
}

