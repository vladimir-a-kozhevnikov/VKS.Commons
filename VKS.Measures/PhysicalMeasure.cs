using System;
using System.Text.RegularExpressions;

using VKS.Measures.RC;

namespace VKS.Measures
{
	/// <summary>
	/// Abstract class that defines behavior for all physical measures in library (length, mass, force, pressure etc).
	/// </summary>
	public abstract class PhysicalMeasure: Measure<double>
	{

		/// <summary>
		/// Pattern for format sequence parsing.
		/// </summary>
		private readonly string FormatPattern =
			@"^(?<token>[GgFfSs]{1})(\:{1}(?<system>\d+)?(\:{1}(?<scale>[-]?\d+)?(\:{1}(?<quantity>[fan])?(\:{1}(?<number>.+))?)?)?)?$";

		/// <summary>
		/// Gets the name of the quantity.
		/// </summary>
		/// <value>The name of the quantity.</value>
		public abstract string QuantityName {
			get;
		}

		/// <summary>
		/// Gets the quantity name abbreviation.
		/// </summary>
		/// <value>The quantity name abbreviation.</value>
		public abstract string QuantityAbbreviation {
			get;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="VKS.Measures.PhysicalMeasure"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return InnerValue.GetHashCode ();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="VKS.Measures.PhysicalMeasure"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="VKS.Measures.PhysicalMeasure"/>.</returns>
		public override string ToString ()
		{
			return ToString ("g", null);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The specified format sequence.</param>
		/// <param name="formatProvider">The format provider for instance.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		/// <exception cref="FormatException">Thrown when specified format couldn't be recognized.</exception>
		public override string ToString (string format, System.IFormatProvider formatProvider)
		{
			/*  
			 * 
			 *  Format sequence rules:
			 * 		{1:g}
			 * 
			 *  Avaliable format options:
			 * 		g - general format, based on measurement system basic format.
			 * 		
			 */

			var _formatTokens = Regex.Matches (format, FormatPattern,
				                    RegexOptions.Singleline |
				                    RegexOptions.CultureInvariant |
				                    RegexOptions.IgnorePatternWhitespace,
				                    new TimeSpan (0, 0, 10));

			var _formatType = _formatTokens [0]?.Groups ["token"]?.Value ?? "g";
			var _systemToken = Convert.ToInt32 (_formatTokens [0]?.Groups ["system"]?.Value ?? "0");
			var _targetSystem = Enum.IsDefined (typeof(MeasurementSystem), _systemToken) ? (MeasurementSystem)_systemToken : 0;
			int? _targetScale;
			if (string.IsNullOrEmpty (_formatTokens [0]?.Groups ["scale"]?.Value))
				_targetScale = null;
			else
				_targetScale = Convert.ToInt32 (_formatTokens [0]?.Groups ["scale"]?.Value);
			var _quantifierType = _formatTokens [0]?.Groups ["quantity"]?.Value ?? "n";
			string _quantifier;
			switch (_quantifierType.ToLowerInvariant ()) {
			case "f":
				_quantifier = QuantityName;
				break;
			case "a":
				_quantifier = QuantityAbbreviation;
				break;
			default:
				_quantifier = "";
				break;
			}
			var _numberFormat = _formatTokens [0]?.Groups ["number"]?.Value ?? "g";

			switch (_targetSystem) {
			case MeasurementSystem.InternationalSystemOfUnits:
				switch (_formatType) {
				case "g":
					
				case "f":

				case "S":
				case "s":
					_numberFormat = _formatTokens [0]?.Groups ["number"]?.Value ?? "e10";
					return string.Format ("{0} {1}", 
						InnerValue.ToString (_numberFormat, formatProvider), _quantifier);
				default:
					throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
				}
			default:
				throw new ArgumentOutOfRangeException ("format", Exceptions.NotSupported_MeasurementSystem);
			}

			throw new NotImplementedException ();
		}

		/// <summary>
		/// Scales the measure to a target scale passed in <paramref name="targetScale"/> parameter considering
		/// that target measurement system is International System of Units.
		/// </summary>
		/// <returns>The <see cref="ScalingData&lt;TPrimitive&gt;"/> structure that contains information about scaled measure.</returns>
		/// <param name="targetScale">Target scale level. If this parameter set to null, then system tries to scale
		/// passed value automatically to the best fitted scale level.</param>
		internal ScalingData<double> ScaleISoU (int? targetScale = null)
		{
			var _Scale = targetScale ?? InternationalSystemOfUnits.GetScalePower (InnerValue);
			return new ScalingData<double> {
				ScaleAbbreviation = InternationalSystemOfUnits.GetScaleAbbreviation (_Scale),
				ScalePrefix = InternationalSystemOfUnits.GetScalePrefix (_Scale),
				ScaleLevel = _Scale,
				ScaledValue = InnerValue / Math.Pow (10, _Scale)
			};
		}
	}
}
