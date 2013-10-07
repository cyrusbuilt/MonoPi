//
//  TemperatureConversion.cs
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

namespace CyrusBuilt.MonoPi.Components.Temperature
{
	/// <summary>
	/// Temperature conversion utilities.
	/// </summary>
	public static class TemperatureConversion
	{
		/// <summary>
		/// Absolute zero temperature in Celcius scale.
		/// </summary>
		public const Double ABSOLUTE_ZERO_CELCIUS = -273.15;

		/// <summary>
		/// Absolute zero temperature in Farenheit scale.
		/// </summary>
		public const Double ABSOLUTE_ZERO_FARENHEIT = -459.67;

		/// <summary>
		/// Absolute zero temperature in Kelvin scale.
		/// </summary>
		public const Double ABSOLUTE_ZERO_KELVIN = 0;

		/// <summary>
		/// Absolute zero temperature in Rankine scale.
		/// </summary>
		public const Double ABSOLUTE_ZERO_RANKINE = 0;

		/// <summary>
		/// Converts the temperature from Rankine to Kelvin scale.
		/// Formula = [K] = [R] x 5/9.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Kelvin.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Rankine.
		/// </param>
		public static Double ConvertRankineToKelvin(Double temp) {
			return ((temp * 5) / 9);
		}

		/// <summary>
		/// Converts the temperature from Rankine to Celcius scale.
		/// Formula = [C] = ([R] - 491.67) x 5/9.
		/// </summary>
		/// <returns>
		/// The temperature in degrees Celcius.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Rankine.
		/// </param>
		public static Double ConvertRankineToCelcius(Double temp) {
			return (((temp - 491.67) * 5) / 9);
		}

		/// <summary>
		/// Converts the temperature from Rankine to Farenheit scale.
		/// Formula = [F] = [R] - 459.67.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Farenheit.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Rankine.
		/// </param>
		public static Double ConvertRankineToFarenheit(Double temp) {
			return (temp - 459.67);
		}

		/// <summary>
		/// Converts the temperature from Kelvin to Rankine scale.
		/// Formula = [R] = [K] x 9/5.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Rankine.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Kelvin.
		/// </param>
		public static Double ConvertKelvinToRankine(Double temp) {
			return ((temp * 9) / 5);
		}

		/// <summary>
		/// Converts the temperature from Kelvin to Farenheit scale.
		/// Formula = [F] = [K] x 9/5 - 459.67.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Farenheit.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Kelvin.
		/// </param>
		public static Double ConvertKelvinToFarenheit(Double temp) {
			return (((temp * 9) / 5) - 459.67);
		}

		/// <summary>
		/// Converts the temperature from Kelvin to Celcius scale.
		/// Formula = [C] = [K] - 273.15.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Celcius.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degress Kelvin.
		/// </param>
		public static Double ConvertKelvinToCelcius(Double temp) {
			return (temp + ABSOLUTE_ZERO_CELCIUS);
		}

		/// <summary>
		/// Converts the temperature from Celcius to Rankine scale.
		/// Formula = [R] = ([C] + 273.15) x 9/5.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Rankine.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Celcius.
		/// </param>
		public static Double ConvertCelciusToRankine(Double temp) {
			return (((temp - ABSOLUTE_ZERO_CELCIUS) * 9) / 5);
		}

		/// <summary>
		/// Converts the temperature from Celcius to Kelvin scale.
		/// Formula = [K] = [C] + 273.15.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Kelvin.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Celcius
		/// </param>
		public static Double ConvertCelciusToKelvin(Double temp) {
			return (temp - ABSOLUTE_ZERO_CELCIUS);
		}

		/// <summary>
		/// Converts the temperature from Celcius to Farenheit scale.
		/// Formula = [F] = [C] x 9/5 + 32.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Farenheit.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Celcius.
		/// </param>
		public static Double ConvertCelciusToFarenheit(Double temp) {
			return (((temp * 9) / 5) + 32);
		}

		/// <summary>
		/// Converts the temperature from Farenheit to Rankine scale.
		/// Formula = [R] = [F] + 459.67.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Rankine.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Farenheit.
		/// </param>
		public static Double ConvertFarenheitToRankine(Double temp) {
			return (temp + 459.67);
		}

		/// <summary>
		/// Converts the temperature from Farenheit to Kelvin scale.
		/// Formula = [K] = ([F] + 459.67) x 5/9.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Kelvin.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Farenheit.
		/// </param>
		public static Double ConvertFarenheitToKelvin(Double temp) {
			return (((temp + 459.67) * 5) / 9);
		}

		/// <summary>
		/// Converts the temperature from Farenheit to Celcius scale.
		/// Formula = [C] = ([F] - 32) x 5/9.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Celcius.
		/// </returns>
		/// <param name="temp">
		/// The temperature in degrees Farenheit.
		/// </param>
		public static Double ConvertFarenheitToCelcius(Double temp) {
			return ((temp - 32) * 5 / 9);
		}

		/// <summary>
		/// Converts the specified temperature value to Rankine scale.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Rankine.
		/// </returns>
		/// <param name="scale">
		/// The temperature scale to convert from.
		/// </param>
		/// <param name="temp">
		/// The temperature value to convert to Rankine scale.
		/// </param>
		public static Double ConvertToRankine(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertFarenheitToRankine(temp);
					break;
				case TemperatureScale.Celcius:
					val = ConvertCelciusToRankine(temp);
					break;
				case TemperatureScale.Kelvin:
					val = ConvertKelvinToRankine(temp);
					break;
				case TemperatureScale.Rankine:
					val = temp;
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature in Rankine to the specified scale.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert the specified Rankine value to.
		/// </param>
		/// <param name="temp">
		/// The temperature value in degrees Rankine.
		/// </param>
		public static Double ConvertFromRankine(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertRankineToFarenheit(temp);
					break;
				case TemperatureScale.Celcius:
					val = ConvertRankineToCelcius(temp);
					break;
				case TemperatureScale.Kelvin:
					val = ConvertKelvinToRankine(temp);
					break;
				case TemperatureScale.Rankine:
					val = temp;
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified value from the specified scale to degrees Kelvin.
		/// </summary>
		/// <returns>
		/// The temperature in degrees Kelvin.
		/// </returns>
		/// <param name="scale">
		/// The temperature scale to convert the value from.
		/// </param>
		/// <param name="temp">
		/// The temperature value to convert.
		/// </param>
		public static Double ConvertToKelvin(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertFarenheitToKelvin(temp);
					break;
				case TemperatureScale.Celcius:
					val = ConvertCelciusToKelvin(temp);
					break;
				case TemperatureScale.Kelvin:
					val = temp;
					break;
				case TemperatureScale.Rankine:
					val = ConvertRankineToKelvin(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature from Kelvin to the specified scale.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert to.
		/// </param>
		/// <param name="temp">
		/// The temperature in degrees Kelvin.
		/// </param>
		public static Double ConvertFromKelvin(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertKelvinToFarenheit(temp);
					break;
				case TemperatureScale.Celcius:
					val = ConvertKelvinToCelcius(temp);
					break;
				case TemperatureScale.Kelvin:
					val = temp;
					break;
				case TemperatureScale.Rankine:
					val = ConvertKelvinToRankine(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature to the specified Celcius scale.
		/// </summary>
		/// <returns>
		/// The temperature value in degrees Celcius.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert to Celcius.
		/// </param>
		/// <param name="temp">
		/// The temperature value.
		/// </param>
		public static Double ConvertToCelcius(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertFarenheitToCelcius(temp);
					break;
				case TemperatureScale.Celcius:
					val = temp;
					break;
				case TemperatureScale.Kelvin:
					val = ConvertKelvinToCelcius(temp);
					break;
				case TemperatureScale.Rankine:
					val = ConvertRankineToCelcius(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature value in degrees Celcius to the specified scale.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert the temperature value to.
		/// </param>
		/// <param name="temp">
		/// The temperature in degrees Celcius.
		/// </param>
		public static Double ConvertFromCelcius(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = ConvertCelciusToFarenheit(temp);
					break;
				case TemperatureScale.Celcius:
					val = temp;
					break;
				case TemperatureScale.Kelvin:
					val = ConvertCelciusToKelvin(temp);
					break;
				case TemperatureScale.Rankine:
					val = ConvertCelciusToRankine(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature value to Farenheit scale.
		/// </summary>
		/// <returns>
		/// The temperature in degrees Farenheit.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert the temperature from.
		/// </param>
		/// <param name="temp">
		/// The temperature value in the specified scale.
		/// </param>
		public static Double ConvertToFarenheit(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = temp;
					break;
				case TemperatureScale.Celcius:
					val = ConvertCelciusToFarenheit(temp);
					break;
				case TemperatureScale.Kelvin:
					val = ConvertKelvinToFarenheit(temp);
					break;
				case TemperatureScale.Rankine:
					val = ConvertRankineToFarenheit(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Convert the temperature in degrees Farenheit to the specified scale.
		/// </summary>
		/// <returns>
		/// The temperature value in the specified scale.
		/// </returns>
		/// <param name="scale">
		/// The scale to convert to.
		/// </param>
		/// <param name="temp">
		/// The temperature in degrees Farenheit.
		/// </param>
		public static Double ConvertFromFarenheit(TemperatureScale scale, Double temp) {
			Double val = 0;
			switch (scale) {
				case TemperatureScale.Farenheit:
					val = temp;
					break;
				case TemperatureScale.Celcius:
					val = ConvertFarenheitToCelcius(temp);
					break;
				case TemperatureScale.Kelvin:
					val = ConvertFarenheitToKelvin(temp);
					break;
				case TemperatureScale.Rankine:
					val = ConvertFarenheitToRankine(temp);
					break;
				default:
					break;
			}
			return val;
		}

		/// <summary>
		/// Converts the specified temperature from one scale to another.
		/// </summary>
		/// <param name="from">
		/// The scale to convert the temperature from.
		/// </param>
		/// <param name="to">
		/// The scale to convert the temperature to.
		/// </param>
		/// <param name="temp">
		/// The temperature value to convert.
		/// </param>
		public static Double Convert(TemperatureScale from, TemperatureScale to, Double temp) {
			Double val = 0;
			switch (from) {
				case TemperatureScale.Farenheit:
					val = ConvertFromFarenheit(to, temp);
					break;
				case TemperatureScale.Celcius:
					val = ConvertFromCelcius(to, temp);
					break;
				case TemperatureScale.Kelvin:
					val = ConvertFromKelvin(to, temp);
					break;
				case TemperatureScale.Rankine:
					val = ConvertFromRankine(to, temp);
					break;
				default:
					break;
			}
			return val;
		}
	}
}

