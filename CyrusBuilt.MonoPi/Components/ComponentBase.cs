//
//  ComponentBase.cs
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

namespace CyrusBuilt.MonoPi.Components
{
	/// <summary>
	/// Base class for hardware abstraction components.
	/// </summary>
	public abstract class ComponentBase : IComponent
	{
		#region Fields
		private String _name = String.Empty;
		private Object _tag = null;
		private Dictionary<String, String> _properties = null;
		private Boolean _isDisposed = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/>
		/// class. This is the default contructor.
		/// </summary>
		protected ComponentBase() {
			this._properties = new Dictionary<String, String>();
		}

		/// <summary>
		/// Releaseses all resources used this object.
		/// </summary>
		/// <param name="disposing">
		/// Set true if disposing managed resources in addition to unmanaged.
		/// </param>
		protected virtual void Dispose(Boolean disposing) {
			if (this._isDisposed) {
				return;
			}

			if (disposing) {
				if (this._properties != null) {
					this._properties.Clear();
					this._properties = null;
				}

				this._tag = null;
				this._name = null;
			}
			this._isDisposed = true;
		}

		/// <summary>
		/// Releases all resources used by the <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/> so the garbage collector can reclaim the memory that the
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/> was occupying.</remarks>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsDisposed {
			get { return this._isDisposed; }
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public String Name {
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		public Object Tag {
			get { return this._tag; }
			set { this._tag = value; }
		}

		/// <summary>
		/// Gets the property collection.
		/// </summary>
		public Dictionary<String, String> PropertyCollection {
			get { return this._properties; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determines whether this instance has property the specified key.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has property the specified by key; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="key">
		/// The key name of the property to check for.
		/// </param>
		public Boolean HasProperty(String key) {
			return this._properties.ContainsKey(key);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.ComponentBase"/>.
		/// </returns>
		public override String ToString() {
			return this._name;
		}
		#endregion
	}
}

