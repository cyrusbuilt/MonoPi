//
//  IFireplace.cs
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
using CyrusBuilt.MonoPi.PiSystem;

namespace CyrusBuilt.MonoPi.Devices.Fireplace
{
	/// <summary>
	/// Fireplace device abstraction interface.
	/// </summary>
	public interface IFireplace : IDevice
	{
		#region Events
		/// <summary>
		/// Occurs when a state change occurs.
		/// </summary>
		event FireplaceStateChangedEventHandler StateChanged;

		/// <summary>
		/// Occurs when an operation times out.
		/// </summary>
		event FireplaceTimeoutEventHandler OperationTimedOut;

		/// <summary>
		/// Occurs when the pilot light state changes.
		/// </summary>
		event PilotLightEventHandler PilotLightStateChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		FireplaceState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether the fireplace is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if the fireplace is on; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOn { get; }

		/// <summary>
		/// Gets a value indicating whether the fireplace is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if the fireplace is off; otherwise, <c>false</c>.
		/// </value>
		Boolean IsOff { get; }

		/// <summary>
		/// Gets a value indicating whether the pilot light is on.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is on; otherwise, <c>false</c>.
		/// </value>
		Boolean IsPilotLightOn { get; }

		/// <summary>
		/// Gets a value indicating whether pilot light is off.
		/// </summary>
		/// <value>
		/// <c>true</c> if the pilot light is off; otherwise, <c>false</c>.
		/// </value>
		Boolean IsPilotLightOff { get; }

		/// <summary>
		/// Gets the timeout delay.
		/// </summary>
		/// <value>
		/// The timeout delay.
		/// </value>
		double TimeoutDelay { get; }

		/// <summary>
		/// Gets the timeout unit of time.
		/// </summary>
		/// <value>
		/// The timeout unit of time.
		/// </value>
		TimeUnit TimeoutUnit { get; } 
		#endregion

		#region Methods
		/// <summary>
		/// Turns the fireplace on.
		/// </summary>
		void On();

		/// <summary>
		/// Turns the fireplace on with the specified timeout. If the operation
		/// is not successful within the allotted time, the operation is
		/// cancelled for safety reasons.
		/// </summary>
		/// <param name="timeoutDelay">
		/// The timeout delay.
		/// </param>
		/// <param name="timeoutUnit">
		/// The time unit of measure for the timeout.
		/// </param>
		void On(double timeoutDelay, TimeUnit timeoutUnit);

		/// <summary>
		/// Turns the fireplace off.
		/// </summary>
		void Off();

		/// <summary>
		/// Sets the timeout delay.
		/// </summary>
		/// <param name="delay">
		/// The timeout delay.
		/// </param>
		/// <param name="unit">
		/// The time unit of measure for the timeout.
		/// </param>
		void SetTimeout(double delay, TimeUnit unit);

		/// <summary>
		/// Cancels a timeout.
		/// </summary>
		void CancelTimeout();

		/// <summary>
		/// Shutdown the fireplace.
		/// </summary>
		void Shutdown();
		#endregion
	}
}

