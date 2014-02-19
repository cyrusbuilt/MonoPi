//
//  ImageEncoding.cs
//
//  Author:
//       Chris Brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2014 Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.Devices.PiCamera
{
	/// <summary>
	/// Image encoding formats.
	/// </summary>
	public enum ImageEncoding
	{
		/// <summary>
		/// JPEG image encoding. NOTE: This is the only supported format that
		/// is hardware accelerated. All other image types will take much longer
		/// to save because they are not accelerated.
		/// </summary>
		JPEG,

		/// <summary>
		/// Bitmap image encoding.
		/// </summary>
		Bitmap,

		/// <summary>
		/// GIF image encoding.
		/// </summary>
		GIF,

		/// <summary>
		/// PNG image encoding.
		/// </summary>
		PNG
	}
}

