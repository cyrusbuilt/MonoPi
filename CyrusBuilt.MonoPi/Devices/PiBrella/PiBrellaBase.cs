//
//  PiBrellaBase.cs
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
using System.Collections.Generic;
using CyrusBuilt.MonoPi.Components.Button;
using CyrusBuilt.MonoPi.Components.Buzzer;
using CyrusBuilt.MonoPi.Components.Lights;
using CyrusBuilt.MonoPi.IO;

namespace CyrusBuilt.MonoPi.Devices.PiBrella
{
	/// <summary>
	/// Base class for PiBrella device abstractions.
	/// </summary>
	public abstract class PiBrellaBase : DeviceBase, IPiBrella
	{
		#region Fields
		private IBuzzer _buzzer = null;
		private IButton _button = null;
		private List<ILED> _leds = null;
		private List<IRaspiGpio> _inputs = null;
		private List<IRaspiGpio> _outputs = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.PiBrella.PiBrellaBase"/>
		/// class. This is the default constructor.
		/// </summary>
		protected PiBrellaBase()
			: base() {
			this._inputs = new List<IRaspiGpio>();
			this._inputs.Add(PiBrellaInput.A);
			this._inputs.Add(PiBrellaInput.B);
			this._inputs.Add(PiBrellaInput.C);
			this._inputs.Add(PiBrellaInput.D);
			this._inputs.Add(PiBrellaInput.BUTTON);

			this._inputs[0].Name = "INPUT A";
			this._inputs[1].Name = "INPUT B";
			this._inputs[2].Name = "INPUT C";
			this._inputs[3].Name = "INPUT D";
			this._inputs[4].Name = "BUTTON";
			foreach (IRaspiGpio input in this._inputs) {
				input.Provision();
			}

			this._outputs = new List<IRaspiGpio>();
			this._outputs.Add(PiBrellaOutput.E);
			this._outputs.Add(PiBrellaOutput.F);
			this._outputs.Add(PiBrellaOutput.G);
			this._outputs.Add(PiBrellaOutput.H);
			this._outputs.Add(PiBrellaOutput.LED_RED);
			this._outputs.Add(PiBrellaOutput.LED_YELLOW);
			this._outputs.Add(PiBrellaOutput.LED_GREEN);

			this._outputs[0].Name = "OUTPUT E";
			this._outputs[1].Name = "OUTPUT F";
			this._outputs[2].Name = "OUTPUT G";
			this._outputs[3].Name = "OUTPUT H";
			this._outputs[4].Name = "RED LED";
			this._outputs[5].Name = "YELLOW LED";
			this._outputs[6].Name = "GREEN LED";
			foreach (IRaspiGpio output in this._outputs) {
				output.Provision();
			}

			this._leds = new List<ILED>();
			this._leds.Add(new LEDComponent(this._outputs[4]));
			this._leds.Add(new LEDComponent(this._outputs[5]));
			this._leds.Add(new LEDComponent(this._outputs[6]));

			this._button = new ButtonComponent(this._inputs[4]);

			this._buzzer = new BuzzerComponent(PiBrellaOutput.BUZZER);
			this._buzzer.Name = "PIBRELLA BUZZER";
			this._buzzer.Stop();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiBrella.PiBrellaBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiBrella.PiBrellaBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="CyrusBuilt.MonoPi.Devices.PiBrella.PiBrellaBase"/> so the garbage collector can reclaim the memory that
		/// the <see cref="CyrusBuilt.MonoPi.Devices.PiBrella.PiBrellaBase"/> was occupying.</remarks>
		public override void Dispose() {
			if (base.IsDisposed) {
				return;
			}

			this._button = null;
			if (this._buzzer != null) {
				this._buzzer.Stop();
				this._buzzer = null;
			}

			if (this._leds != null) {
				this._leds.Clear();
				this._leds = null;
			}

			if (this._inputs != null) {
				this._leds.Clear();
				this._leds = null;
			}

			if (this._outputs != null) {
				this._outputs.Clear();
				this._outputs = null;
			}
			base.Dispose();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the red LED.
		/// </summary>
		/// <value>
		/// The red LED output component.
		/// </value>
		public ILED RedLED {
			get { return this._leds[0]; }
		}

		/// <summary>
		/// Gets the yellow LED.
		/// </summary>
		/// <value>
		/// The yellow LED output component.
		/// </value>
		public ILED YellowLED {
			get { return this._leds[1]; }
		}

		/// <summary>
		/// Gets the green LED.
		/// </summary>
		/// <value>
		/// The green LED output component.
		/// </value>
		public ILED GreenLED {
			get { return this._leds[2]; }
		}

		/// <summary>
		/// Gets the LEDs.
		/// </summary>
		/// <value>
		/// The LEDs.
		/// </value>
		public ILED[] LEDs {
			get { return this._leds.ToArray(); }
		}

		/// <summary>
		/// Gets the button.
		/// </summary>
		/// <value>
		/// The PiBrella button input.
		/// </value>
		public IButton Button {
			get { return this._button; }
		}

		/// <summary>
		/// Gets the buzzer.
		/// </summary>
		/// <value>
		/// The buzzer output component.
		/// </value>
		public IBuzzer Buzzer {
			get { return this._buzzer; }
		}

		/// <summary>
		/// Gets PiBrella input A.
		/// </summary>
		/// <value>
		/// Input A.
		/// </value>
		public IRaspiGpio InputA {
			get { return this._inputs[0]; }
		}

		/// <summary>
		/// Gets PiBrella input B.
		/// </summary>
		/// <value>
		/// Input B.
		/// </value>
		public IRaspiGpio InputB {
			get { return this._inputs[1]; }
		}

		/// <summary>
		/// Gets PiBrella input C.
		/// </summary>
		/// <value>
		/// Input C.
		/// </value>
		public IRaspiGpio InputC {
			get { return this._inputs[2]; }
		}

		/// <summary>
		/// Gets PiBrella input D.
		/// </summary>
		/// <value>
		/// Input D.
		/// </value>
		public IRaspiGpio InputD {
			get { return this._inputs[3]; }
		}

		/// <summary>
		/// Gets all the PiBrella inputs.
		/// </summary>
		/// <value>
		/// The inputs.
		/// </value>
		public IRaspiGpio[] Inputs {
			get { return this._inputs.ToArray(); }
		}

		/// <summary>
		/// Gets PiBrella output E.
		/// </summary>
		/// <value>
		/// Output E.
		/// </value>
		public IRaspiGpio OutputE {
			get { return this._outputs[0]; }
		}

		/// <summary>
		/// Gets PiBrella output F.
		/// </summary>
		/// <value>
		/// Output F.
		/// </value>
		public IRaspiGpio OutputF {
			get { return this._outputs[1]; }
		}

		/// <summary>
		/// Gets PiBrella output G.
		/// </summary>
		/// <value>
		/// Output G.
		/// </value>
		public IRaspiGpio OutputG {
			get { return this._outputs[2]; }
		}

		/// <summary>
		/// Gets PiBrella output H.
		/// </summary>
		/// <value>
		/// Output H.
		/// </value>
		public IRaspiGpio OutputH {
			get { return this._outputs[3]; }
		}
			
		/// <summary>
		/// Gets all the PiBrella outputs.
		/// </summary>
		/// <value>The outputs.</value>
		public IRaspiGpio[] Outputs {
			get { return this._outputs.ToArray(); }
		}
		#endregion
	}
}

