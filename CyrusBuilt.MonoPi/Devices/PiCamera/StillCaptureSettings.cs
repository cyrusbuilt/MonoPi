//
//  StillCaptureSettings.cs
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
using System.Drawing;
using System.IO;

namespace CyrusBuilt.MonoPi.Devices.PiCamera
{
	/// <summary>
	/// Still capture camera settings.
	/// </summary>
	public class StillCaptureSettings
	{
		#region Constants
		/// <summary>
		/// The default image size width (640).
		/// </summary>
		public const Int32 DEFAULT_IMAGE_SIZE_W = 640;

		/// <summary>
		/// The default image size height (480).
		/// </summary>
		public const Int32 DEFAULT_IMAGE_SIZE_H = 480;

		/// <summary>
		/// The minimum quality value (0).
		/// </summary>
		public const Int32 QUALITY_MIN = 0;

		/// <summary>
		/// The maximum quality value (100). This value is almost
		/// completely uncompressed.
		/// </summary>
		public const Int32 QUALITY_MAX = 100;

		/// <summary>
		/// The default quality value (75). This provides the best
		/// all-around quality vs. performance value.
		/// </summary>
		public const Int32 QUALITY_DEFAULT = 75;

		/// <summary>
		/// The default timeout value (5 seconds).
		/// </summary>
		public const Int32 TIMEOUT_DEFAULT = 5000;
		#endregion

		#region Fields
		private Size _imageSize = new Size(DEFAULT_IMAGE_SIZE_W, DEFAULT_IMAGE_SIZE_H);
		private Int32 _quality = QUALITY_DEFAULT;
		private Int32 _timeout = TIMEOUT_DEFAULT;
		private Int32 _timeLapse = 0;
		private FileInfo _output = null;
		private Boolean _verbose = false;
		private Boolean _raw = false;
		private Boolean _fullPreview = false;
		private ImageEncoding _encoding = ImageEncoding.JPEG;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CyrusBuilt.MonoPi.Devices.PiCamera.StillCaptureSettings"/>
		/// class. This is the default constructor.
		/// </summary>
		public StillCaptureSettings() {
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the size of the image. Default of 640 x 480.
		/// </summary>
		public Size ImageSize {
			get { return this._imageSize; }
			set { this._imageSize = value; }
		}

		/// <summary>
		/// Gets or sets the image quality.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Quality value must be between <see cref="QUALITY_MIN"/> and
		/// <see cref="QUALITY_MAX"/>. Default is <see cref="QUALITY_DEFAULT"/>.
		/// </exception>
		public Int32 ImageQuality {
			get { return this._quality; }
			set {
				if ((value < QUALITY_MIN) || (value > QUALITY_MAX)) {
					throw new ArgumentOutOfRangeException("StillCaptureSettings.ImageQuality",
						"Quality value out of range");
				}
				this._quality = value;
			}
		}

		/// <summary>
		/// Gets or sets the timeout in milliseconds. This is the time
		/// to elapse before capture and shutdown. Default is
		/// <see cref="TIMEOUT_DEFAULT"/>.
		/// </summary>
		public Int32 Timeout {
			get { return this._timeout; }
			set { this._timeout = value; }
		}

		/// <summary>
		/// Gets or sets the output file the image will be captured to.
		/// </summary>
		public FileInfo OutputFile {
			get { return this._output; }
			set { this._output = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not the still capture will
		/// produce verbose output.
		/// </summary>
		public Boolean Verbose {
			get { return this._verbose; }
			set { this._verbose = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not to add raw Bayer data
		/// to JPEG metadata. This option inserts the raw Bayer data into the
		/// JPEG metadata if <see cref="Encoding"/> is <see cref="ImageEncoding.JPEG"/>.
		/// </summary>
		public Boolean Raw {
			get { return this._raw; }
			set { this._raw = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not to run in full preview mode.
		/// This runs the preview windows using the full resolution capture mode.
		/// Maximum frames-per-second in this mode is 15fps and the preview will have
		/// the same field of view as the capture. Captures should happen more quickly
		/// as no mode change should be required.
		/// </summary>
		public Boolean FullPreview {
			get { return this._fullPreview; }
			set { this._fullPreview = value; }
		}

		/// <summary>
		/// Gets or sets the time lapse interval in milliseconds. Set to zero to disable
		/// time-lapse capture.
		/// </summary>
		public Int32 TimeLapseInterval {
			get { return this._timeLapse; }
			set { this._timeLapse = value; }
		}

		/// <summary>
		/// Gets or sets the still image capture encoding.
		/// </summary>
		public ImageEncoding Encoding {
			get { return this._encoding; }
			set { this._encoding = value; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Converts this instance to a string of arguments that
		/// can be passed to raspistill.
		/// </summary>
		/// <returns>
		/// A string of arguments for the raspistill utility.
		/// </returns>
		public String ToArgumentString() {
			String fname = String.Empty;
			if (this._output != null) {
				fname = Path.GetFileNameWithoutExtension(this._output.FullName);
			}

			String args = String.Empty;
			if (this._imageSize != Size.Empty) {
				args += " --width " + this._imageSize.Width.ToString();
				args += " --height " + this._imageSize.Height.ToString();
			}

			args += " --quality " + this._quality.ToString();
			if (this._timeout > 0) {
				args += " --timeout " + this._timeout.ToString();
			}

			if (this._timeLapse > 0) {
				args += " --timelapse " + this._timeLapse.ToString();
				fname += "_%04d";
			}

			fname += "." + CaptureUtils.GetEncodingFileExtension(this._encoding);
			fname = Path.Combine(this._output.Directory.FullName, fname);
			this._output = new FileInfo(fname);
			args += " --output " + this._output.FullName;

			if (this._verbose) {
				args += " --verbose";
			}

			args += " --encoding " + this._output.Extension;
			if (this._fullPreview) {
				args += " --fullpreview";
			}
			return args;
		}
		#endregion
	}
}

