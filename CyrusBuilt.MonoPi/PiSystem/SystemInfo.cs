//
//  SystemInfo.cs
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
using Mono.Unix.Native;

namespace CyrusBuilt.MonoPi.PiSystem
{
	/// <summary>
	/// Provides methods for getting system-specific info about the host OS
	/// and the board it is running on.
	/// </summary>
	public static class SystemInfo
	{
		private static Dictionary<String, String> _cpuInfo = null;

		/// <summary>
		/// Gets information about the CPU and returns the value
		/// from the specified target field.
		/// </summary>
		/// <returns>
		/// The value of the specified CPU info attribute.
		/// </returns>
		/// <param name="target">
		/// The target attribute to get the value of.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// The specified target is invalid.
		/// </exception>
		private static String GetCpuInfo(String target) {
			if (_cpuInfo == null) {
				_cpuInfo = new Dictionary<String, String>();
				String[] result = ExecUtil.ExecuteCommand("cat /proc/cpuinfo");
				if (result != null) {
					String[] parts = { };
					foreach (String line in result) {
						parts = line.Split(new Char[] { ':' }, 2);
						if ((parts.Length >= 2) &&
							(!String.IsNullOrEmpty(parts[0].Trim())) &&
							(!String.IsNullOrEmpty(parts[1].Trim()))) {
							_cpuInfo.Add(parts[0].Trim(), parts[1].Trim());
						}
					}
				}
			}

			if (_cpuInfo.ContainsKey(target)) {
				return _cpuInfo[target];
			}

			throw new InvalidOperationException("Invalid target: " + target);
		}

		/// <summary>
		/// Gets the processor.
		/// </summary>
		/// <returns>
		/// The processor.
		/// </returns>
		public static String GetProcessor() {
			return GetCpuInfo("Processor");
		}

		/// <summary>
		/// Gets the Bogo MIPS.
		/// </summary>
		/// <returns>
		/// The Bogo MIPS.
		/// </returns>
		public static String GetBogoMIPS() {
			return GetCpuInfo("BogoMIPS");
		}

		/// <summary>
		/// Gets the CPU features.
		/// </summary>
		/// <returns>
		/// The CPU features.
		/// </returns>
		public static String[] GetCpuFeatures() {
			return GetCpuInfo("Features").Split(' ');
		}

		/// <summary>
		/// Gets the CPU implementer.
		/// </summary>
		/// <returns>
		/// The CPU implementer.
		/// </returns>
		public static String GetCpuImplementer() {
			return GetCpuInfo("CPU implementer");
		}

		/// <summary>
		/// Gets the CPU architecture.
		/// </summary>
		/// <returns>
		/// The CPU architecture.
		/// </returns>
		public static String GetCpuArchitecture() {
			return GetCpuInfo("CPU architecture");
		}

		/// <summary>
		/// Gets the CPU variant.
		/// </summary>
		/// <returns>
		/// The CPU variant.
		/// </returns>
		public static String GetCpuVariant() {
			return GetCpuInfo("CPU variant");
		}

		/// <summary>
		/// Gets the CPU part.
		/// </summary>
		/// <returns>
		/// The CPU part.
		/// </returns>
		public static String GetCpuPart() {
			return GetCpuInfo("CPU part");
		}

		/// <summary>
		/// Gets the CPU revision.
		/// </summary>
		/// <returns>
		/// The CPU revision.
		/// </returns>
		public static String GetCpuRevision() {
			return GetCpuInfo("CPU revision");
		}

		/// <summary>
		/// Gets the hardware the system is implemented on.
		/// </summary>
		/// <returns>
		/// The hardware.
		/// </returns>
		public static String GetHardware() {
			return GetCpuInfo("Hardware");
		}

		/// <summary>
		/// Gets the system revision.
		/// </summary>
		/// <returns>
		/// The system revision.
		/// </returns>
		public static String GetSystemRevision() {
			return GetCpuInfo("Revision");
		}

		/// <summary>
		/// Gets the serial number.
		/// </summary>
		/// <returns>
		/// The serial number.
		/// </returns>
		public static String GetSerial() {
			return GetCpuInfo("Serial");
		}

		/// <summary>
		/// Gets the name of the OS.
		/// </summary>
		/// <returns>
		/// The OS name.
		/// </returns>
		public static String GetOsName() {
			PlatformID plat = Environment.OSVersion.Platform;
			return Enum.GetName(typeof(PlatformID), plat);
		}

		/// <summary>
		/// Gets the OS version.
		/// </summary>
		/// <returns>
		/// The OS version.
		/// </returns>
		public static String GetOsVersion() {
			return Environment.OSVersion.VersionString;
		}

		/// <summary>
		/// Gets the OS architecture.
		/// </summary>
		/// <returns>
		/// The OS architecture.
		/// </returns>
		public static String GetOsArch() {
			Utsname results = null;
			Int32 res = Syscall.uname(out results);
			if (res != 0) {
				return "Unknown";
			}

			return results.machine;
		}

		/// <summary>
		/// Gets the OS firmware build.
		/// </summary>
		/// <returns>
		/// The OS firmware build.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("version") or response.
		/// </exception>
		public static String GetOsFirmwareBuild() {
			String[] results = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd version");
			String val = String.Empty;
			if (results != null) {
				foreach (String line in results) {
					if (line.StartsWith("version ")) {
						val = line;
						break;
					}
				}
			}

			if (!String.IsNullOrEmpty(val)) {
				return val.Substring(8);
			}
			throw new InvalidOperationException("Invalid command or response.");
		}

		/// <summary>
		/// Gets the OS firmware date.
		/// </summary>
		/// <returns>
		/// The OS firmware date.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("version") or response.
		/// </exception>
		public static String GetOsFirmwareDate() {
			String[] results = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd version");
			String val = String.Empty;
			if (results != null) {
				foreach (String line in results) {
					val = line;
					break;
				}
			}

			if (!String.IsNullOrEmpty(val)) {
				return val;
			}
			throw new InvalidOperationException("Invalid command or response.");
		}

		/// <summary>
		/// Gets the system memory info.
		/// </summary>
		/// <returns>
		/// The memory info.
		/// </returns>
		private static List<long> GetMemory() {
			// Memory information is in the form
			// root@mypi:/home/pi# free -b
			//              total       used       free     shared    buffers     cached
			// Mem:     459771904  144654336  315117568          0   21319680   63713280
			// -/+ buffers/cache:   59621376  400150528
			// Swap:    104853504          0  104853504
			List<long> values = new List<long>();
			String[] result = ExecUtil.ExecuteCommand("free -b");
			if (result != null) {
				String[] parts = { };
				String linePart = String.Empty;
				long val = 0;
				foreach (String line in result) {
					if (line.StartsWith("Mem:")) {
						parts = line.Split(' ');
						foreach (String part in parts) {
							linePart = part.Trim();
							if ((!String.IsNullOrEmpty(linePart)) &&
								(String.Equals(line, "Mem:", StringComparison.InvariantCultureIgnoreCase))) {
								if (long.TryParse(linePart, out val)) {
									values.Add(val);
								}
							}
						}
					}
				}
				Array.Clear(parts, 0, parts.Length);
			}
			Array.Clear(result, 0, result.Length);
			return values;
		}

		/// <summary>
		/// Gets the total system memory.
		/// </summary>
		/// <returns>
		/// If successful, the total system memory; Otherwise, -1.
		/// </returns>
		public static long GetMemoryTotal() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 0)) {
				return values[0]; // Total memory value is the first position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the amount of memory consumed.
		/// </summary>
		/// <returns>
		/// If successful, the amount of memory that is in use; Otherwise, -1.
		/// </returns>
		public static long GetMemoryUsed() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 1)) {
				return values[1]; // Used memory value is the second position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the free memory available.
		/// </summary>
		/// <returns>
		/// If successful, the amount of memory available; Otherwise, -1.
		/// </returns>
		public static long GetMemoryFree() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 2)) {
				return values[2]; // Free memory value is the third position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the amount of shared memory.
		/// </summary>
		/// <returns>
		/// If successful, the shared memory; Otherwise, -1.
		/// </returns>
		public static long GetMemoryShared() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 3)) {
				return values[3]; // Shared memory value is the fourth position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the buffer memory.
		/// </summary>
		/// <returns>
		/// If successful, the buffer memory; Otherwise, -1.
		/// </returns>
		public static long GetMemoryBuffers() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 4)) {
				return values[4]; // Buffer memory value is the fifth position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the amount of cache memory
		/// </summary>
		/// <returns>
		/// If successful, the cache memory; Otherwise, -1;
		/// </returns>
		public static long GetMemoryCached() {
			List<long> values = GetMemory();
			if ((values != null) && (values.Count > 5)) {
				return values[5]; // Cached memory value is the sixth position.
			}
			return -1;
		}

		/// <summary>
		/// Gets the type of the board the executing assembly is running on.
		/// </summary>
		/// <returns>
		/// The board type.
		/// </returns>
		public static BoardType GetBoardType() {
			// The following info obtained from:
			// http://www.raspberrypi.org/archives/1929
			// http://raspberryalphaomega.org.uk/?p=428
			// http://www.raspberrypi.org/phpBB3/viewtopic.php?p=281039#p281039
			BoardType bt = BoardType.Unknown;
			switch (GetSystemRevision()) {
				case "0002":  // Model B Revision 1
				case "0003":  // Model B Revision 1 + Fuses mod and D14 removed
					bt = BoardType.ModelB_Rev1;
					break;
				case "0004":  // Model B Revision 2 256MB (Sony)
				case "0005":  // Model B Revision 2 256MB (Qisda)
				case "0006":  // Model B Revision 2 256MB (Egoman)
				case "000d":  // Model B Revision 2 512MB (Egoman)
				case "000e":  // Model B Revision 2 512MB (Sony)
				case "000f":  // Model B Revision 2 512MB (Qisda)
					bt = BoardType.ModelB_Rev2;
					break;
				default:
					break;
			}
			return bt;
		}

		/// <summary>
		/// Gets the CPU temperature.
		/// </summary>
		/// <returns>
		/// The CPU temperature.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_temp") or response.
		/// </exception>
		public static float GetCpuTemperature() {
			// CPU temperature is in the form
			// pi@mypi$ /opt/vc/bin/vcgencmd measure_temp
			// temp=42.3'C
			// Support for this was added around firmware version 3357xx per info
			// at http://www.raspberrypi.org/phpBB3/viewtopic.php?p=169909#p169909
			String[] result = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd measure_temp");
			if (result != null) {
				String[] parts = { };
				float val = -1f;
				foreach (String line in result) {
					parts = line.Split(new Char[] { '[', '=', ']', "'".ToCharArray()[0] }, 3);
					if (float.TryParse(parts[1], out val)) {
						break;
					}
				}
				return val;
			}
			throw new InvalidOperationException("Invalid command or response.");
		}

		/// <summary>
		/// Gets the voltage.
		/// </summary>
		/// <returns>
		/// The voltage.
		/// </returns>
		/// <param name="id">
		/// The ID of the voltage type to get (core, sdram_c, etc).
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_volts") or response.
		/// </exception>
		private static float GetVoltage(String id) {
			String[] result = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd measure_volts " + id);
			if (result != null) {
				float val = -1f;
				String[] parts = { };
				foreach (String line in result) {
					parts = line.Split(new Char[] { '[', '=', 'V', ']' }, 3);
					if (float.TryParse(parts[1], out val)) {
						break;
					}
				}
				return val;
			}
			throw new InvalidOperationException("Invalid command or response.");
		}

		/// <summary>
		/// Gets the CPU voltage.
		/// </summary>
		/// <returns>
		/// The CPU voltage.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_volts") or response.
		/// </exception>
		public static float GetCpuVoltage() {
			return GetVoltage("core");
		}

		/// <summary>
		/// Gets the memory voltage of SDRAM C.
		/// </summary>
		/// <returns>
		/// The memory voltage SDRAM C.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_volts") or response.
		/// </exception>
		public static float GetMemoryVoltageSDRamC() {
			return GetVoltage("sdram_c");
		}

		/// <summary>
		/// Gets the memory voltage of SDRAM I.
		/// </summary>
		/// <returns>
		/// The memory voltage SDRAM I.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_volts") or response.
		/// </exception>
		public static float GetMemoryVoltageSDRamI() {
			return GetVoltage("sdram_i");
		}

		/// <summary>
		/// Gets the memory voltage of SDRAM P.
		/// </summary>
		/// <returns>
		/// The memory voltage SDRAM P.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Invalid command ("measure_volts") or response.
		/// </exception>
		public static float GetMemoryVoltageSDRamP() {
			return GetVoltage("sdram_p");
		}

		/// <summary>
		/// Gets whether or not the specified codec is enabled.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the codec is enabled; Otherwise, <code>false</code>.
		/// </returns>
		/// <param name="codec">
		/// The codec to get.
		/// </param>
		private static Boolean GetCodecEnabled(String codec) {
			Boolean enabled = false;
			String[] result = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd codec_enabled " + codec);
			if (result != null) {
				String[] parts = { };
				foreach (String line in result) {
					parts = line.Split(new Char[] { '=' }, 2);
					if (String.Equals(parts[1].Trim(), "enabled", StringComparison.InvariantCultureIgnoreCase)) {
						enabled = true;
						break;
					}
				}
			}
			return enabled;
		}

		/// <summary>
		/// Determines if the H264 codec is enabled.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the H264 codec is enabled; Otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsCodecH264Enabled() {
			return GetCodecEnabled("H264");
		}

		/// <summary>
		/// Determines if the MPG2 codec is enabled.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the MPG2 codec is enabled; Otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsCodecMPG2Enabled() {
			return GetCodecEnabled("MPG2");
		}

		/// <summary>
		/// Determines if the WVC1 codec is enabled.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the WVC1 codec is enabled; Otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsCodecWVC1Enabled() {
			return GetCodecEnabled("WVC1");
		}

		/// <summary>
		/// Gets the clock frequency for the specified target.
		/// </summary>
		/// <returns>
		/// The clock frequency, if successful; Otherwise, -1.
		/// </returns>
		/// <param name="target">
		/// The target clock to get the frequency of.
		/// </param>
		private static long GetClockFrequency(String target) {
			long val = -1;
			String[] result = ExecUtil.ExecuteCommand("/opt/vc/bin/vcgencmd measure_clock " + target.Trim());
			if (result != null) {
				String[] parts = { };
				foreach (String line in result) {
					parts = line.Split(new Char[] { '=' }, 2);
					if (long.TryParse(parts[1].Trim(), out val)) {
						break;
					}
				}
			}
			return val;
		}

		/// <summary>
		/// Gets the clock frequency of the specified clock.
		/// </summary>
		/// <returns>
		/// The clock frequency, if successful; Otherwise, -1.
		/// </returns>
		/// <param name="type">
		/// The clock type to get the frequency of.
		/// </param>
		public static long GetClockFrequency(ClockType type) {
			return GetClockFrequency(Enum.GetName(typeof(ClockType), type).ToLower());
		}

		/// <summary>
		/// Gets the bash version info. This method is used to help
		/// determine the HARD-FLOAT / SOFT-FLOAT ABI of the system.
		/// </summary>
		/// <returns>
		/// The bash version info.
		/// </returns>
		private static String GetBashVersionInfo() {
			String ver = String.Empty;
			try {
				String[] result = ExecUtil.ExecuteCommand("bash --version");
				foreach(String line in result) {
					if (!String.IsNullOrEmpty(line)) {
						ver = line;  // Return only the first line.
						break;
					}
				}
			}
			catch {
			}
			return ver;
		}

		/// <summary>
		/// This method will obtain a specified tag value from the elf
		/// info in the '/proc/self/exe' program (this method is used
		/// to help determine the HARD-FLOAT / SOFT-FLOAT ABI of the system)
		/// </summary>
		/// <returns>
		/// The ABI tag value.
		/// </returns>
		/// <param name="tag">
		/// The tag to get the value of.
		/// </param>
		private static String GetReadElfTag(String tag) {
			String tagVal = String.Empty;
			try {
				String[] result = ExecUtil.ExecuteCommand("/usr/bin/readelf -A /proc/self/exe");
				if (result != null) {
					String[] lineParts = { };
					String part = String.Empty;
					foreach (String line in result) {
						part = line.Trim();
						if ((line.StartsWith(tag)) && (part.Contains(":"))) {
							lineParts = part.Split(new Char[] { ':' }, 2);
							if (lineParts.Length > 1) {
								tagVal = lineParts[1].Trim();
							}
							break;
						}
					}
				}
			}
			catch {
			}
			return tagVal;
		}

		/// <summary>
		/// This method will determine if a specified tag exists from the elf
		/// info in the '/proc/self/exe' program (this method is used to help
		/// determine the HARD-FLOAT / SOFT-FLOAT ABI of the system)
		/// </summary>
		/// <returns>
		/// true if contains the specified ELF tag.
		/// </returns>
		/// <param name="tag">
		/// The tag to check for.
		/// </param>
		private static Boolean HasReadElfTag(String tag) {
			String tagValue = GetReadElfTag(tag);
			return !String.IsNullOrEmpty(tagValue);
		}

		/// <summary>
		/// Determines if is hard float ABI.
		/// </summary>
		/// <returns>
		/// <c>true</c> if is hard float ABI; Otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsHardFloatABI() {
			return ((GetBashVersionInfo().Contains("gnueabihf")) ||
					(HasReadElfTag("Tag_ABI_HardFP_use")));
		}

		/// <summary>
		/// Gets the current system time in milliseconds.
		/// </summary>
		/// <returns>
		/// The current time millis.
		/// </returns>
		public static long GetCurrentTimeMillis() {
			return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
		}
	}
}

