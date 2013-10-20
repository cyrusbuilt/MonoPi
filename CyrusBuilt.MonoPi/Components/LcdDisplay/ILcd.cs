//
//  ILcd.cs
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

namespace CyrusBuilt.MonoPi.Components.LcdDisplay
{
	/// <summary>
	/// An LCD display device abstraction component interface.
	/// </summary>
	public interface ILcd : IComponent
	{
		/// <summary>
		/// Gets the row count.
		/// </summary>
		Int32 RowCount { get; }

		/// <summary>
		/// Gets the column count.
		/// </summary>
		Int32 ColumnCount { get; }

		/// <summary>
		/// Clears the entire display.
		/// </summary>
		void Clear();

		/// <summary>
		/// Clears the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to clear.
		/// </param>
		void Clear(Int32 row);

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
		void Clear(Int32 row, Int32 column, Int32 length);

		/// <summary>
		/// Sends the cursor to the home position which is in the top-left corner
		/// of the screen.
		/// </summary>
		void SendCursorHome();

		/// <summary>
		/// Positions the cursor at the beginning of the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to position the cursor in.
		/// </param>
		void SetCursorPosition(Int32 row);

		/// <summary>
		/// Positions the cursor at the specified column in the specified row.
		/// </summary>
		/// <param name="row">
		/// The number of the row to position the cursor in.
		/// </param>
		/// <param name="column">
		/// The number of the column in the specified row to position the cursor.
		/// </param>
		void SetCursorPosition(Int32 row, Int32 column);

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		void Write(String data);

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		/// <param name="arguments">
		/// The object representing the arguments.
		/// </param>
		void Write(String data, Object arguments);

		/// <summary>
		/// Writes a character array to the display.
		/// </summary>
		/// <param name="data">
		/// The array of characters to write.
		/// </param>
		void Write(Char[] data);

		/// <summary>
		/// Writes a byte array to the display.
		/// </summary>
		/// <param name="data">
		/// The array of bytes to write.
		/// </param>
		void Write(Byte[] data);

		/// <summary>
		/// Writes a single character to the display.
		/// </summary>
		/// <param name="data">
		/// The character to write.
		/// </param>
		void Write(Char data);

		/// <summary>
		/// Writes a single byte to the display.
		/// </summary>
		/// <param name="data">
		/// The byte to write.
		/// </param>
		void Write(Byte data);

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
		void Write(Int32 row, String data, LcdTextAlignment alignment);

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
		/// <param name="args">
		/// The arguments.
		/// </param>
		void Write(Int32 row, String data, LcdTextAlignment alignment, Object args);

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		void Write(Int32 row, String data);

		/// <summary>
		/// Writes text to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to write the text in.
		/// </param>
		/// <param name="data">
		/// The text string to write.
		/// </param>
		/// <param name="args">
		/// The arguments.
		/// </param>
		void Write(Int32 row, String data, Object args);

		/// <summary>
		/// Writes a character array to the display in the specified row.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The array of characters to write.
		/// </param>
		void Write(Int32 row, Char[] data);

		/// <summary>
		/// Writes a byte array to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The array of bytes to write.
		/// </param>
		void Write(Int32 row, Byte[] data);

		/// <summary>
		/// Writes a single character to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The character to write.
		/// </param>
		void Write(Int32 row, Char data);

		/// <summary>
		/// Writes a single byte to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The byte to write.
		/// </param>
		void Write(Int32 row, Byte data);

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
		void Write(Int32 row, Int32 column, String data);

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
		void Write(Int32 row, Int32 column, String data, Object arguments);

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
		void Write(Int32 row, Int32 column, Char[] data);

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
		void Write(Int32 row, Int32 column, Byte[] data);

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
		void Write(Int32 row, Int32 column, Char data);

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
		void Write(Int32 row, Int32 column, Byte data);

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="data">
		/// The string reperesentation of the data to write.
		/// </param>
		void WriteLine(Int32 row, String data);

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
		void WriteLine(Int32 row, String data, Object args);

		/// <summary>
		/// Write the specified data to the display.
		/// </summary>
		/// <param name="row">
		/// The row to position the data in.
		/// </param>
		/// <param name="alignment">
		/// The text alignment within the row.
		/// </param>
		void WriteLine(Int32 row, String data, LcdTextAlignment alignment);

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
		void WriteLine(Int32 row, String data, LcdTextAlignment alignment, Object args);
	}
}

