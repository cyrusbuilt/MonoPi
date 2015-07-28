//
//  CoreUtils.cs
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
using System.Diagnostics;

namespace CyrusBuilt.MonoPi.PiSystem
{
	/// <summary>
	/// Core system utility methods.
	/// </summary>
	public static class CoreUtils
	{
		/// <summary>
		/// Blocks the current thread for the specified microseconds.
		/// </summary>
		/// <param name="micros">
		/// The amount of time in microseconds to sleep.
		/// </param>
		public static void SleepMicroseconds(double micros) {
			Stopwatch sw = Stopwatch.StartNew();
			while ((1e6 * sw.ElapsedTicks / (double)Stopwatch.Frequency) <= micros) {
				// This isn't a *real* sleep. We just spin the CPU because we
				// aren't operating on a real-time OS, so the best we can do
				// is spin the CPU. We can't *really* sleep less than 1ms, and
				// even then, we can still get preempted by the OS at any time.
			}
			sw.Stop();
			sw = null;
		}
	}
}

