//
//  GpioMemLcdTransferProvider.cs
//
//  Author:
//       chris brunner <cyrusbuilt at gmail dot com>
//
//  Copyright (c) 2013 CyrusBuilt
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

namespace CyrusBuilt.MonoPi.LCD
{
	public class GpioMemLcdTransferProvider : ILcdTransferProvider
	{
		#region Fields
		private readonly GpioMem _registerSelectPort = null;
		private readonly GpioMem _readWritePort = null;
		private readonly GpioMem _enablePort = null;
		private readonly GpioMem[] _dataPorts = { };
		private readonly Boolean _fourBitMode = false;
		private Boolean _isDisposed = false;
		#endregion

		public GpioMemLcdTransferProvider() {
		}
	}
}

