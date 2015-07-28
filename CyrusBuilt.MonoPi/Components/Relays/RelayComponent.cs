//
//  RelayComponent.cs
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
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Relays
{
	/// <summary>
	/// A component that is an abstraction of a relay.
	/// </summary>
	public class RelayComponent : RelayBase
	{
		private static readonly Object _stateLock = new Object();

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayComponent"/>
		/// class. This is the default constructor.
		/// </summary>
		public RelayComponent()
			: base() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Relays.RelayComponent"/>
		/// class with the <see cref="CyrusBuilt.MonoPi.IO.GpioMem"/> I/O pin to use.
		/// </summary>
		/// <param name="pin">
		/// The <see cref="CyrusBuilt.MonoPi.IO.IGpio"/> I/O pin to use.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="pin"/> cannot null.
		/// </exception>
		public RelayComponent(IGpio pin)
			: base(pin) {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state of the relay.
		/// </summary>
		public override RelayState State {
			get {
				if (base.Pin.State == RelayBase.OPEN_STATE) {
					return RelayState.Open;
				}
				return RelayState.Closed;
			}
			set {
 				RelayState oldState = this.State;
				if (this.State != value) {
					lock (_stateLock) {
						switch (value) {
							case RelayState.Open:
								if (!base.IsOpen) {
									base.Pin.Write(PinState.Low);
								}
								break;
							case RelayState.Closed:
								if (!base.IsClosed) {
									base.Pin.Write(PinState.High);
								}
								break;
							default:
								break;
						}
					}
					base.OnStateChanged(new RelayStateChangedEventArgs(oldState, this.State));
				}
			}
		}
		#endregion
	}
}

