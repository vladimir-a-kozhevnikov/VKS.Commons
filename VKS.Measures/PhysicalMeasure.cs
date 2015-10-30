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
			@"^(?<token>[GgFfSsNnQq]{1})(\:{1}(?<system>\d+)?(\:{1}(?<scale>[-]?\d+)?(\:{1}(?<quantity>[fFaAnN])?(\:{1}(?<number>.+))?)?)?)?$";


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
		/// <param name="format">The format sequence.</param>
		/// <param name="formatProvider">The format provider for instance.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		/// <exception cref="FormatException">Thrown when specified format couldn't be recognized.</exception>
		/// <remarks>
		/// <b>Format sequence:</b> 
		/// 	<c>&lt;FormatOption&gt;[:&lt;TargetMeasurementSystem&gt;[:&lt;TargetScale&gt;[:&lt;QuantifierType&gt;[:&lt;NumericFormat&gt;]]]]</c>
		///	
		/// <b>Available format options:</b>
		/// <list type="table">
		/// 	<listheader>
		/// 		<term>Format identifier</term>
		/// 		<description>Description of target format string.</description>
		///		<item>
		/// 		<term>g</term>
		/// 		<description>General format, based on measurement system default format.</description>
		/// 	</item>
		///		<item>
		/// 		<term>G</term>
		/// 		<description>General format, based on measurement system default format with smart scaling.</description>
		/// 	</item>
		///		<item>
		/// 		<term>s or S</term>
		/// 		<description>Scientific format for output. No scaling used, value converted to exponent format if not overriden.</description>
		/// 	</item>
		///		<item>
		/// 		<term>f</term>
		/// 		<description>Full notation with scaling.</description>
		/// 	</item>
		///		<item>
		/// 		<term>F</term>
		/// 		<description>Full notation with smart scaling.</description>
		/// 	</item>
		///		<item>
		/// 		<term>n or N</term>
		/// 		<description>Numeric-only without scaling.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>q</term>
		/// 		<description>Abbreviated (shortened) description of quantifier considering scale level.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>Q</term>
		/// 		<description>Full quantifier name considering scale level.</description>
		/// 	</item>
		/// </list> 	
		/// 
		/// <b>Available measurement systems:</b>
		/// <list type="table">
		/// 	<listheader>
		/// 		<term>Code</term>
		/// 		<description>Associated measurement system</description>
		/// 	</listheader>
		/// 	<item>
		/// 		<term>0</term>
		/// 		<description>
		/// 			Metric aka International system of Units.
		/// 		</description>
		/// </list>
		///		
		///	<b>Target scale (ignored for "n" / "N" format options, required for "q" / "Q" format options):</b>
		///		Positive or negative integer or zero (for basic unit). If skipped automatic scaling performed.
		///				
		/// <b>Quantifier type definitions (ignored for "n"/"N" format options):</b>
		/// <list type="table">
		/// 	<listheader>
		/// 		<term>Identifier</term>
		/// 		<description>Target quantifier</description>
		/// 	</listheader>
		/// 	<item>
		/// 		<term>f or F</term>
		/// 		<description>Inserted full quantifier name (i.e. kilometer(s) instead of km).</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>a or A</term>
		/// 		<description>Inserted abbreviated quantifier name (km instead of kilometer, g instead gram etc).</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>n or N</term>
		/// 		<description>No quantifier name in output.</description>
		/// 	</item>
		/// </list>
		///		
		///	<b>Numeric format (ignored for "s" / "S" and "q" / "Q" format options):</b>
		///		Any supported format for double.ToString() method. By default general numerics format preferred (with one 
		///			exception - for scientific format - exponential format is default).
		///			
		/// </remarks>
		public override string ToString (string format, IFormatProvider formatProvider)
		{
			var _formatTokens = Regex.Matches (format, FormatPattern,
				                    RegexOptions.Singleline |
				                    RegexOptions.CultureInvariant |
				                    RegexOptions.IgnorePatternWhitespace,
				                    new TimeSpan (0, 0, 10));

			var _formatType = _formatTokens [0]?.Groups ["token"]?.Value ?? "g";
			var _sysToken = _formatTokens [0]?.Groups ["system"]?.Value;
			var _systemToken = _sysToken == "" ? 0 : Convert.ToInt32 (_sysToken);
			var _targetSystem = Enum.IsDefined (typeof(MeasurementSystem), _systemToken) ? (MeasurementSystem)_systemToken : 0;
			int? _targetScale;
			if (string.IsNullOrEmpty (_formatTokens [0]?.Groups ["scale"]?.Value))
				_targetScale = null;
			else
				_targetScale = Convert.ToInt32 (_formatTokens [0]?.Groups ["scale"]?.Value);
			var _quantifierType = _formatTokens [0]?.Groups ["quantity"]?.Value;
			var _numberFormat = _formatTokens [0]?.Groups ["number"]?.Value ?? "g";
			_numberFormat = string.IsNullOrWhiteSpace (_numberFormat) ? "g" : _numberFormat;

			var _genericNFI = new System.Globalization.NumberFormatInfo {
				NumberGroupSeparator = " ",
				NumberDecimalDigits = 3,
				NumberDecimalSeparator = ".",
				NegativeInfinitySymbol = "-∞",
				PositiveInfinitySymbol = "+∞",
				NaNSymbol = "?",
				NegativeSign = "-",
				PositiveSign = " "
			};

			switch (_targetSystem) {
			case MeasurementSystem.InternationalSystemOfUnits:
				var _metricValue = ConvertToPrimitive (MeasurementSystem.InternationalSystemOfUnits);
				var _scaledValue = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits, _targetScale);
				switch (_formatType) {
				case "q":
					//Abbreviated quantifier name considering scaling level.
					if (_targetScale == null)
						throw new FormatException ();
					return _scaledValue.Abbreviation;
				case "Q":
					//Full quantifier name considering scaling level.
					if (_targetScale == null)
						throw new FormatException ();
					return _scaledValue.Quantifier;
				case "g":
					//General formatting without smart scaling.
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						return string.Format (_genericNFI, "{0:#,##0.000} {1}", _scaledValue.ScaledValue, 
							_scaledValue.Quantifier);
					case "n":
						return string.Format (_genericNFI, "{0:#,##0.000}", _scaledValue.ScaledValue);
					default:
						return string.Format (_genericNFI, "{0:#,##0.000} {1}", _scaledValue.ScaledValue, 
							_scaledValue.Abbreviation);
					}
				case "G":
					//General formatting with smart scaling.
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue, true);
					_scaledValue = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits, _targetScale);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						return string.Format (_genericNFI, "{0:#,##0.000} {1}", _scaledValue.ScaledValue, 
							_scaledValue.Quantifier);
					case "n":
						return string.Format (_genericNFI, "{0:#,##0.000}", _scaledValue.ScaledValue);
					default:
						return string.Format (_genericNFI, "{0:#,##0.000} {1}", _scaledValue.ScaledValue, 
							_scaledValue.Abbreviation);
					}
				case "f":
					//Full formatting rules without smart-scaling.
					_quantifierType = string.IsNullOrWhiteSpace (_quantifierType) ? "n" : _quantifierType;
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						return string.Format (_genericNFI, "{0} {1}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider), 
							_scaledValue.Quantifier);
					case "a":
						return string.Format (_genericNFI, "{0} {1}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider), 
							_scaledValue.Abbreviation);
					case "n":
						return string.Format (_genericNFI, "{0}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider));
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
				case "F":
					//Full formatting rules with smart-scaling.
					_quantifierType = string.IsNullOrWhiteSpace (_quantifierType) ? "n" : _quantifierType;
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue, true);
					_scaledValue = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits, _targetScale);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						return string.Format (_genericNFI, "{0} {1}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider), 
							_scaledValue.Quantifier);
					case "a":
						return string.Format (_genericNFI, "{0} {1}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider), 
							_scaledValue.Abbreviation);
					case "n":
						return string.Format (_genericNFI, "{0}", _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider));
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
				case "N":
				case "n":
					//Numeric only (without quantifier and scaling):
					_scaledValue = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits, 0);
					return _scaledValue.ScaledValue.ToString (_numberFormat, formatProvider);
				case "S":
				case "s":
					//Scientific format (exponential):
					_scaledValue = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits, 0);
					return string.Format ("{0} {1}", 
						_scaledValue.ScaledValue.ToString ("e5", formatProvider),
						_scaledValue.Abbreviation);
				default:
					throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
				}

			//Left in case when new measurement system will be added.
			//This code is unreachable when everything is OK with versions of library and sources.
			default:
				throw new ArgumentOutOfRangeException ("format", Exceptions.NotSupported_MeasurementSystem);
			}
		}
	}
}
