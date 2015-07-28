//
//  BuzzerBase.cs
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
using System.Threading;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Components.Buzzer
{
	/// <summary>
	/// A buzzer device abstraction component.
	/// </summary>
	public class BuzzerComponent : ComponentBase, IBuzzer
	{
		private IGpio _pwm = null;

		/// <summary>
		/// The minimum PWM frequency value used to stop the pulse (0).
		/// </summary>
		public const UInt32 STOP_FREQUENCY = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Buzzer.BuzzerComponent"/>
		/// class with the pin the buzzer is attached to.
		/// </summary>
		/// <param name="pwmPin">
		/// The pin the buzzer is attached to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The pin cannot be null.
		/// </exception>
		public BuzzerComponent(IGpio pwmPin)
			: base() {
			if (pwmPin == null) {
				throw new ArgumentNullException("pwmPin");
			}
			this._pwm = pwmPin;
			this._pwm.Provision();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Buzzer.BuzzerComponent"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Buzzer.BuzzerComponent"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Buzzer.BuzzerComponent"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Components.Buzzer.BuzzerComponent"/> was occupying.</remarks>
		public override void Dispose() {
			if (this._pwm != null) {
				this._pwm.Dispose();
				this._pwm = null;
			}
			base.Dispose();
		}

		/// <summary>
		/// Start the buzzer at the specified frequency.
		/// </summary>
		/// <param name="freq">
		/// The frequency to buzz at.
		/// </param>
		public void Buzz(UInt32 freq) {
			if (freq == STOP_FREQUENCY) {
				this._pwm.PWM = freq;
			}
			else {
				UInt32 range = (600000 / freq);
				this._pwm.PWMRange = range;
				this._pwm.PWM = (freq / 2);
			}
		}

		/// <summary>
		/// Stops the buzzer.
		/// </summary>
		public void Stop() {
			this.Buzz(STOP_FREQUENCY);
		}

		/// <summary>
		/// Start the buzzer at the specified frequency for the specified duration.
		/// </summary>
		/// <param name="freq">
		/// The frequency to buzz at.
		/// </param>
		/// <param name="duration">
		/// The duration in milliseconds.
		/// </param>
		public void Buzz(UInt32 freq, Int32 duration) {
			this.Buzz(freq);
			Thread.Sleep(duration);
			this.Stop();
		}
	}
}

