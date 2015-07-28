//
//  ILcdTransferProvider.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2012 CyrusBuilt
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
//  Derived from Micro Liquid Crystal Library
//  http://microliquidcrystal.codeplex.com
//  Appache License Version 2.0
//
using System;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.LCD
{
	/// <summary>
	/// LCD data transfer provider interface.
	/// </summary>
	public interface ILcdTransferProvider : IDisposable
	{
		/// <summary>
		/// Send the specified data, mode and backlight.
		/// </summary>
		/// <param name="data">
		/// The data to send.
		/// </param>
		/// <param name="mode">
		/// Mode for register-select pin (PinState.High = on, PinState.Low = off).
		/// </param>
		/// <param name="backlight">
		/// Turns on the backlight.
		/// </param>
		void Send(Byte data, PinState mode, Boolean backlight);

		/// <summary>
		/// Gets a value indicating whether this <see cref="CyrusBuilt.MonoPi.LCD.ILcdTransferProvider"/>
		/// is in four-bit mode.
		/// </summary>
		Boolean FourBitMode { get; }
	}
}

