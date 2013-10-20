//
//  LcdBase.cs
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
using System.Text;

namespace CyrusBuilt.MonoPi.Components.LcdDisplay
{
	/// <summary>
	/// Base class for LCD display abstractions.
	/// </summary>
	public abstract class LcdBase : ComponentBase, ILcd
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Components.LcdDisplay.LcdBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected LcdBase()
			: base() {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the row count.
		/// </summary>
		public abstract Int32 RowCount { get; }

		/// <summary>
		/// Gets the column count.
		/// </summary>
		public abstract Int32 ColumnCount { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Validates the index of the row.
		/// </summary>
		/// <param name="row">
		/// The index of the row to validate.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row index is invalid for the display.
		/// </exception>
		protected void ValidateRowIndex(Int32 row) {
			if ((row >= this.RowCount) || (row < 0)) {
				throw new InvalidOperationException("Invalid row index.");
			}
		}

		/// <summary>
		/// Validates the index of the column.
		/// </summary>
		/// <param name="column">
		/// The index of the column to validate.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The column index is invalid for the display.
		/// </exception>
		protected void ValidateColumnIndex(Int32 column) {
			if ((column >= this.ColumnCount) || (column < 0)) {
				throw new InvalidOperationException("Invalid column index.");
			}
		}

		/// <summary>
		/// Validates the coordinates.
		/// </summary>
		/// <param name="row">
		/// The index of the row to validate.
		/// </param>
		/// <param name="column">
		/// The index of the column to validate.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		protected void ValidateCoordinates(Int32 row, Int32 column) {
			this.ValidateRowIndex(row);
			this.ValidateColumnIndex(column);
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
		public abstract void SetCursorPosition(Int32 row, Int32 column);

		/// <summary>
		/// Positions the cursor at the beginning of the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to position the cursor in.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void SetCursorPosition(Int32 row) {
			this.ValidateRowIndex(row);
			this.SetCursorPosition(row, 0);
		}

		/// <summary>
		/// Sends the cursor to the home position which is in the top-left corner
		/// of the screen.
		/// </summary>
		public void SendCursorHome() {
			this.SetCursorPosition(0);
		}

		/// <summary>
		/// Writes a single byte to the display.
		/// </summary>
		/// <param name="data">
		/// The byte to write.
		/// </param>
		public abstract void Write(Byte data);

		/// <summary>
		/// Writes a single character to the display.
		/// </summary>
		/// <param name="data">
		/// The character to write.
		/// </param>
		public void Write(Char data) {
			this.Write((Byte)data);
		}

		/// <summary>
		/// Writes a byte array to the display.
		/// </summary>
		/// <param name="data">
		/// The array of bytes to write.
		/// </param>
		public void Write(Byte[] data) {
			foreach (Byte b in data) {
				this.Write(b);
			}
		}

		/// <summary>
		/// Writes a character array to the display.
		/// </summary>
		/// <param name="data">
		/// The array of characters to write.
		/// </param>
		public void Write(Char[] data) {
			foreach (Char c in data) {
				this.Write(c);
			}
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="data"/> cannot be null.
		/// </exception>
		/// <exception cref="EncoderFallbackException">
		/// An encoder fallback occurred.
		/// </exception>
		public void Write(String data) {
			try {
				this.Write(Encoding.UTF8.GetBytes(data));
			}
			catch {
				throw;
			}
		}

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		/// <param name="column">
		/// The column position within the row to start the write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, String data) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <param name="arguments">
		/// The object representing the arguments.
		/// </param>
		public void Write(String data, Object arguments) {
			this.Write(String.Format(data, arguments));
		}

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, String data) {
			this.Write(row, 0, data);
		}

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		/// <param name="alignment">
		/// The text alignment within the row.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, String data, LcdTextAlignment alignment) {
			Int32 columnIndex = 0;
			if ((alignment != LcdTextAlignment.Left) && (data.Length < this.ColumnCount)) {
				Int32 remaining = (this.ColumnCount - data.Length);
				if (alignment == LcdTextAlignment.Right) {
					columnIndex = remaining;
				}
				else if (alignment == LcdTextAlignment.Center) {
						columnIndex = (remaining / 2);
				}
			}
			this.Write(row, columnIndex, data);
		}

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		/// <param name="arguments">
		/// The arguments.
		/// </param>
		public void Write(Int32 row, String data, Object arguments) {
			this.Write(row, 0, data, arguments);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="alignment">
		/// The text alignment within the row.
		/// </param>
		/// <param name="arguments">
		/// The arguments.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, String data, LcdTextAlignment alignment, Object arguments) {
			this.Write(row, String.Format(data, arguments), alignment);
		}

		/// <summary>
		/// Writes a character array to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The array of characters to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Char[]data) {
			this.Write(row, 0, data);
		}

		/// <summary>
		/// Writes a byte array to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The array of bytes to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Byte[] data) {
			this.Write(row, 0, data);
		}

		/// <summary>
		/// Writes a single character to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The character to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Char data) {
			this.Write(row, 0, data);
		}

		/// <summary>
		/// Writes a single byte to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The byte to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Byte data) {
			this.Write(row, 0, data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="column">
		/// The column within the row to start the write.
		/// </param>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <param name="arguments">
		/// The arguments.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, String data, Object arguments) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data, arguments);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="column">
		/// The column within the row to start the write.
		/// </param>
		/// <param name="data">
		/// The array of characters to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, Char[] data) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="column">
		/// The column within the row to start the write.
		/// </param>
		/// <param name="data">
		/// The array of bytes to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, Byte[] data) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="column">
		/// The column within the row to start the write.
		/// </param>
		/// <param name="data">
		/// The character to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, Char data) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="column">
		/// The column within the row to start the write.
		/// </param>
		/// <param name="data">
		/// The byte to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void Write(Int32 row, Int32 column, Byte data) {
			this.ValidateCoordinates(row, column);
			this.SetCursorPosition(row, column);
			this.Write(data);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <param name="alignment">
		/// The text alignment.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void WriteLine(Int32 row, String data, LcdTextAlignment alignment) {
			String result = data;
			if (data.Length < this.ColumnCount) {
				switch (alignment) {
					case LcdTextAlignment.Left:
						result = StringUtils.PadRight(data, (this.ColumnCount - data.Length));
						break;
					case LcdTextAlignment.Right:
						result = StringUtils.PadLeft(data, (this.ColumnCount - data.Length));
						break;
					case LcdTextAlignment.Center:
						result = StringUtils.PadCenter(data, this.ColumnCount);
						break;
				}
			}
			this.Write(row, 0, result);
		}

		/// <summary>
		/// Write the specified data to the display with the text aligned to the left.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void WriteLine(Int32 row, String data) {
			this.WriteLine(row, data, LcdTextAlignment.Left);
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <param name="args">
		/// The arguments.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void WriteLine(Int32 row, String data, Object args) {
			this.WriteLine(row, String.Format(data, args));
		}

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="alignment">
		/// The text alignment within the row.
		/// </param>
		/// <param name="args">
		/// The arguments.
		/// </param>
		/// <param name="data">
		/// The text to write to the display.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row or column index is invalid for the display.
		/// </exception>
		public void WriteLine(Int32 row, String data, LcdTextAlignment alignment, Object args) {
			this.WriteLine(row, String.Format(data, args), alignment);
		}

		/// <summary>
		/// Clear one or more characters starting at the specified row and column.
		/// </summary>
		/// <param name="row">
		/// The number of the row to clear the character(s) from.
		/// </param>
		/// <param name="column">
		/// The column that is the starting position.
		/// </param>
		/// <param name="length">
		/// The number of characters to clear.
		/// </param>
		public void Clear(Int32 row, Int32 column, Int32 length) {
			StringBuilder sb = new StringBuilder(this.ColumnCount);
			for (Int32 i = 0; i < this.RowCount; i++) {
				sb.Append(String.Empty);
			}

			for (Int32 i = 0; i < this.RowCount; i++) {
				this.Write(i, 0, sb.ToString());
			}
		}

		/// <summary>
		/// Clears the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to clear.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The row index is invalid for the display.
		/// </exception>
		public void Clear(Int32 row) {
			this.ValidateRowIndex(row);
			this.Clear(row, 0, this.ColumnCount);
		}

		/// <summary>
		/// Clears the entire display.
		/// </summary>
		public void Clear() {
			this.Clear(0, 0, this.ColumnCount);
			this.Clear(1, 0, this.ColumnCount);
		}
		#endregion
	}
}

