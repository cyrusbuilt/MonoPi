//
//  ButtonBase.cs
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
using System.Timers;

namespace CyrusBuilt.MonoPi.Components.Button
{
	/// <summary>
	/// Base class for button device abstraction components.
	/// </summary>
	public abstract class ButtonBase : ComponentBase, IButton
	{
		private Timer _holdTimer = null;

		#region Events
		/// <summary>
		/// Occurs when the button state has changed.
		/// </summary>
		public event ButtonStateChangeHandler StateChanged;

		/// <summary>
		/// Occurs when the button is pressed.
		/// </summary>
		public event ButtonStateChangeHandler Pressed;

		/// <summary>
		/// Occurs when the button is released.
		/// </summary>
		public event ButtonStateChangeHandler Released;

		/// <summary>
		/// Occurs when the button is held.
		/// </summary>
		public event ButtonStateChangeHandler Hold;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected ButtonBase()
			: base() {
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Components.Button.ButtonBase"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._holdTimer != null) {
				this._holdTimer.Stop();
				this._holdTimer.Elapsed -= this.HoldTimer_Elapsed;
				this._holdTimer.Dispose();
				this._holdTimer = null;
			}

			this.Pressed = null;
			this.Released = null;
			this.Hold = null;
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>
		/// The state.
		/// </value>
		public abstract ButtonState State { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is pressed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is pressed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsPressed {
			get { return (this.State == ButtonState.Pressed); }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is released.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is released; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsReleased {
			get { return (this.State == ButtonState.Released); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Handles the button hold timer elapsed event.
		/// </summary>
		/// <param name="sender">
		/// The object raising the event.
		/// </param>
		/// <param name="e">
		/// The timer elapsed event arguments.
		/// </param>
		private void HoldTimer_Elapsed(Object sender, ElapsedEventArgs e) {
			if (this.IsPressed) {
				this.OnHold(new ButtonEventArgs(this));
			}
		}

		/// <summary>
		/// Raises the <see cref="ButtonBase.Pressed"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPressed(ButtonEventArgs e) {
			if (this.Pressed != null) {
				this.Pressed(this, e);
			}

			if (this._holdTimer == null) {
				this._holdTimer = new Timer(2000);
				this._holdTimer.AutoReset = true;
				this._holdTimer.Elapsed += this.HoldTimer_Elapsed;
			}

			if (this._holdTimer.Enabled) {
				this._holdTimer.Stop();
			}
			this._holdTimer.Start();
		}

		/// <summary>
		/// Raises the <see cref="ButtonBase.Released"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnReleased(ButtonEventArgs e) {
			if (this.Released != null) {
				this.Released(this, e);
			}

			if ((this._holdTimer != null) && (this._holdTimer.Enabled)) {
				this._holdTimer.Stop();
			}
		}

		/// <summary>
		/// Raises the <see cref="ButtonBase.Hold"/> event.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnHold(ButtonEventArgs e) {
			if (this.Hold != null) {
				this.Hold(this, e);
			}
		}

		/// <summary>
		/// Raises the state changed event.
		/// </summary>
		/// <param name="e">
		/// The button event arguments.
		/// </param>
		protected virtual void OnStateChanged(ButtonEventArgs e) {
			if (this.StateChanged != null) {
				this.StateChanged(this, e);
				if (e.IsPressed) {
					this.OnPressed(e);
				}
					
				if (e.IsReleased) {
					this.OnReleased(e);
				}
			}
		}

		/// <summary>
		/// Determines whether this button's state is the specified state.
		/// </summary>
		/// <param name="state">
		/// The state to check.
		/// </param>
		/// <returns>
		/// <c>true</c> if this instance is state the specified state; otherwise, <c>false</c>.
		/// </returns>
		public Boolean IsState(ButtonState state) {
			return (this.State == state);
		}
		#endregion
	}
}

