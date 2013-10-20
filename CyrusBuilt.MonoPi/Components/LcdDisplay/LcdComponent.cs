//
//  LcdComponent.cs
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
using CyrusBuilt.MonoPi.LCD;

namespace CyrusBuilt.MonoPi.Components.LcdDisplay
{
	/// <summary>
	/// An LCD display device abstraction component.
	/// </summary>
	public class LcdComponent : LcdBase, ILcd
	{
		private LcdModule _module = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/>
		/// class with the LCD transfer provider and the rows and columns of the display.
		/// </summary>
		/// <param name="provider">
		/// The LCD transfer provider.
		/// </param>
		/// <param name="rows">
		/// The number of rows in the display.
		/// </param>
		/// <param name="columns">
		/// The number of columns.
		/// </param>
		public LcdComponent(ILcdTransferProvider provider, Int32 rows, Int32 columns)
			: base() {
			if (provider == null) {
				throw new ArgumentNullException("provider");
			}
			this._module = new LcdModule(provider);
			this._module.Begin(columns, rows);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/> so the garbage collector can reclaim the memory
		/// that the <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			if (this._module != null) {
				this._module.Clear();
				this._module.Provider.Dispose();
				this._module = null;
			}
			base.Dispose();
		}

		/// <summary>
		/// Gets the row count.
		/// </summary>
		public override Int32 RowCount {
			get { return this._module.Rows; }
		}

		/// <summary>
		/// Gets the column count.
		/// </summary>
		public override Int32 ColumnCount {
			get { return this._module.Columns; }
		}

		/// <summary>
		/// Positions the cursor at the specified column in the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to position the cursor in.
		/// </param>
		/// <param name="column">
		/// The number of the column in the specified row to position the cursor.
		/// </param>
		public override void SetCursorPosition(Int32 row, Int32 column) {
			base.ValidateCoordinates(row, column);
			this._module.SetCursorPosition(column, row);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		public override void Write(Byte data) {
			this._module.WriteByte(data);
		}

		/// <summary>
		/// Clears the display.
		/// </summary>
		public void ClearDisplay() {
			this._module.Clear();
		}

		/// <summary>
		/// Sets the cursor in the home position.
		/// </summary>
		public void SetCursorHome() {
			this._module.ReturnHome();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdComponent"/>.
		/// </returns>
		public override String ToString() {
			return base.Name;
		}
	}
}

