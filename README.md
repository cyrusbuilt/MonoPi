MonoPi
======

A Raspberry Pi GPIO manipulation library for Mono/.NET implementing libbcm2835 and wiringPi
=======
MonoPi v1.0.0.2 ALPHA
Copyright (c) CyrusBuilt 2013

A Raspberry Pi GPIO manipulation library for Mono/.NET implementing the wiringPi v2 library (https://github.com/WiringPi/WiringPi2-Python/tree/master/WiringPi/wiringPi), the libbcm2835 library (http://www.open.com.au/mikem/bcm2835/index.html), and the LibNativeI2C library that is part of the RPi.I2C.Net library (https://github.com/mshmelev/RPi.I2C.Net/tree/master/Lib/LibNativeI2C).  The purpose of this framework is to provide the ability to read/write the GPIO pins on the Raspberry Pi, as well as interface with the onboard serial (RS232 UARTs) pins, which is an alternate function of 2 of the GPIOs along with some additional interfacing options and includes APIs for a number of devices and components as well as interfaces and abstractions for creating your own.  This library is under development and not ready for production use.  This library has grown into a bit of a "Swiss Army Knife" given the breadth of functionality that is still growing as it is adapted to its underlying C libraries. Most of it is ready for testing however, and I'd love some feed back. Portions of this library are based on (or ported from) the Pi4J project (https://github.com/Pi4J/pi4j).

NOTE:
======
You will need to compile the dependent libraries in the "lib" directory and make
sure they are in the same path as the compiled CyrusBuilt.MonoPi.dll assembly.

LICENSE:
======
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
