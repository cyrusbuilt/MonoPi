//
//  ExecUtil.cs
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
using System.Collections.Generic;
using System.Diagnostics;

namespace CyrusBuilt.MonoPi
{
	/// <summary>
	/// Process execution utilities.
	/// </summary>
	public static class ExecUtil
	{
		/// <summary>
		/// Executes the specified command string.
		/// </summary>
		/// <returns>
		/// The output from the specified command, if successfully executed.
		/// </returns>
		/// <param name="command">
		/// The command to execute.
		/// </param>
		public static String[] ExecuteCommand(String command) {
			if (String.IsNullOrEmpty(command)) {
				return new String[0];
			}

			// Parse the provided command string. The first value is the
			// command we're actually going to execute. Everything that
			// comes after that is just arguments to that command.
			String[] cmdline = command.Split(' ');
			String args = String.Empty;
			if (cmdline.Length > 1) {
				for (Int32 i = 1; i <= (cmdline.Length - 1); i++) {
					args += cmdline[i] + " ";
				}

				if (args.EndsWith(" ")) {
					args = args.TrimEnd(' ');
				}
			}

			// Setup the process and launch it.
			List<String> result = new List<String>();
			Process p = new Process();
			p.StartInfo = new ProcessStartInfo();
			p.StartInfo.FileName = cmdline[0];          // First value is command.
			p.StartInfo.Arguments = args;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			if (p.Start()) {
				// Read everything out of the process
				String output = p.StandardOutput.ReadLine();
				while (output != null) {
					if (!String.IsNullOrEmpty(output)) {
						result.Add(output.Trim());
					}
					output = p.StandardOutput.ReadLine();
				}

				// Wait for process to finish, then return output if successful.
				p.WaitForExit();
				if ((p.ExitCode == 0) && (result.Count > 0)) {
					return result.ToArray();
				}
			}
			return new String[0];
		}
	}
}

