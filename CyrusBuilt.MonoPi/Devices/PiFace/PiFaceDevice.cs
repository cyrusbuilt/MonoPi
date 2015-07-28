//
//  PiFaceDevice.cs
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
using CyrusBuilt.MonoPi.IO.SPI;

namespace CyrusBuilt.MonoPi.Devices.PiFace
{
	/// <summary>
	/// PiFace device abstraction.
	/// </summary>
	public class PiFaceDevice : PiFaceBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/>
		/// class wtih the SPI channel and speed used to communicate with the PiFace.
		/// </summary>
		/// <param name="channel">
		/// The SPI channel.
		/// </param>
		/// <param name="speed">
		/// The SPI speed.
		/// </param>
		public PiFaceDevice(AdcChannels channel, Int32 speed)
			: base(channel, speed) {
		}

		/// <summary>
		/// Releases all resource used by the <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Devices.PiFace.PiFaceDevice"/> was occupying.</remarks>
		public override void Dispose() {
			base.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}

