//
//  NetworkInfo.cs
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
using System.Net;
using System.Net.NetworkInformation;

namespace CyrusBuilt.MonoPi.PiSystem
{
	/// <summary>
	/// Provides methods for getting network-specific info about the
	/// local host and its integrated and/or attached network hardware.
	/// </summary>
	public static class NetworkInfo
	{
		/// <summary>
		/// Gets the name of the host.
		/// </summary>
		/// <returns>
		/// The host name.
		/// </returns>
		public static String GetHostName() {
			return Environment.MachineName;
		}

		/// <summary>
		/// Gets the fully-qualified domain name of the local host.
		/// </summary>
		/// <returns>
		/// The fully-qualified domain name (FQDN).
		/// </returns>
		public static String GetFQDN() {
			String dn = IPGlobalProperties.GetIPGlobalProperties().DomainName;
			String hn = Dns.GetHostName();
			String fqdn = hn;
			if (!hn.Contains(dn)) {
				fqdn += "." + dn;
			}
			return fqdn;
		}

		/// <summary>
		/// Gets an array of all the IP addresses assigned to all the network interfaces.
		/// </summary>
		/// <returns>
		/// The IP addresses.
		/// </returns>
		public static String[] GetIPAddresses() {
			List<String> addrs = new List<String>();
			String hn = Dns.GetHostName();
			IPHostEntry host = Dns.GetHostEntry(hn);
			foreach (IPAddress ip in host.AddressList) {
				addrs.Add(ip.ToString());
			}
			return addrs.ToArray();
		}

		/// <summary>
		/// Gets the IP address of the local system's hostname. This only works
		/// if the hostname can be resolved.
		/// </summary>
		/// <returns>
		/// The IP address.
		/// </returns>
		public static String GetIPAddress() {
			return ExecUtil.ExecuteCommand("hostname --ip-address")[0];
		}

		/// <summary>
		/// Gets all FQDNs of the machine. This enumerates all configured
		/// network addresses on all configured network interfaces, and
		/// translates them to DNS domain names. Addresses that cannot be
		/// translated (i.e. because they do not have an appropriate reverse
		/// DNS entry) are skipped. Note that different addresses may resolve
		/// to the same name, therefore the return value may contain duplicate
		/// entries. Do not make any assumptions about the order of the items
		/// in the array.
		/// </summary>
		/// <returns>
		/// An array of FQDNS associated with the host.
		/// </returns>
		public static String[] GetAllFQDNs() {
			String names = ExecUtil.ExecuteCommand("hostname --all-fqdns")[0];
			return names.Split(new Char[] { ' ' });
		}

		/// <summary>
		/// Gets an array of all available name servers.
		/// </summary>
		/// <returns>
		/// The name servers.
		/// </returns>
		public static String[] GetNameServers() {
			List<String> addrs = new List<String>();
			IPInterfaceProperties props = null;
			IPAddressCollection dnsServers = null;
			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface adapter in adapters) {
				props = adapter.GetIPProperties();
				dnsServers = props.DnsAddresses;
				if (dnsServers.Count > 0) {
					foreach (IPAddress ip in dnsServers) {
						addrs.Add(ip.ToString());
					}
				}
			}

			Array.Clear(adapters, 0, adapters.Length);
			return addrs.ToArray();
		}
	}
}

