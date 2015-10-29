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
		/// Gets the quantifier name for the specified targetMeasurementSystem.
		/// </summary>
		/// <param name="targetMeasurementSystem">Target measurement system.</param>
		/// <param name="isAbbreviation">If set to <c>true</c> then abbreviated quantifier name returned.</param>
		public abstract string this [MeasurementSystem targetMeasurementSystem, bool isAbbreviation] {
			get;
		}

		/// <summary>
		/// Gets or sets the <see cref="VKS.Measures.PhysicalMeasure"/> value considering <paramref name="measurementSystem"/> and
		/// <paramref name="scale"/> level.
		/// </summary>
		/// <param name="measurementSystem">Measurement system used for convertions.</param>
		/// <param name="scale">Scale level used for convertions.</param>
		public abstract double this [MeasurementSystem measurementSystem, int scale] {
			get;
			set;
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
		///	<b>Numeric format (ignored for "q" / "Q" format options):</b>
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
			var _systemToken = Convert.ToInt32 (_formatTokens [0]?.Groups ["system"]?.Value ?? "0");
			var _targetSystem = Enum.IsDefined (typeof(MeasurementSystem), _systemToken) ? (MeasurementSystem)_systemToken : 0;
			int? _targetScale;
			if (string.IsNullOrEmpty (_formatTokens [0]?.Groups ["scale"]?.Value))
				_targetScale = null;
			else
				_targetScale = Convert.ToInt32 (_formatTokens [0]?.Groups ["scale"]?.Value);
			var _quantifierType = _formatTokens [0]?.Groups ["quantity"]?.Value ?? "n";
			var _numberFormat = _formatTokens [0]?.Groups ["number"]?.Value ?? "g";
			string _quantifier;
			switch (_quantifierType.ToLowerInvariant ()) {
			case "f":
				_quantifier = this [MeasurementSystem.InternationalSystemOfUnits, false];
				break;
			case "a":
				_quantifier = this [MeasurementSystem.InternationalSystemOfUnits, true];
				break;
			case "n":
				_quantifier = "";
				break;
			default:
				throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
			}

			string _prefix; //Define local variable for quantity name prefix.
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
				switch (_formatType) {
				case "q":
					//Abbreviated quantifier name considering scaling level.
					if (_targetScale == null)
						throw new FormatException ();
					return string.Format ("{0}{1}", InternationalSystemOfUnits.GetScalePrefix (_targetScale.Value),
						this [MeasurementSystem.InternationalSystemOfUnits, true]);
				case "Q":
					//Full quantifier name considering scaling level.
					if (_targetScale == null)
						throw new FormatException ();
					return string.Format ("{0}{1}", InternationalSystemOfUnits.GetScalePrefix (_targetScale.Value),
						this [MeasurementSystem.InternationalSystemOfUnits, false]);
				case "g":
					//General formatting without smart scaling.
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						_prefix = InternationalSystemOfUnits.GetScalePrefix (_targetScale ?? 0);
						break;
					case "a":
						_prefix = InternationalSystemOfUnits.GetScaleAbbreviation (_targetScale ?? 0);
						break;
					case "n":
						_prefix = "";
						break;
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
					return string.Format (_genericNFI, "{0:#,##0.000} {1}{2}", _metricValue / Math.Pow (10, _targetScale ?? 0),
						_prefix, _quantifier);
				case "G":
					//General formatting with smart scaling.
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue, true);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						_prefix = InternationalSystemOfUnits.GetScalePrefix (_targetScale ?? 0);
						break;
					case "a":
						_prefix = InternationalSystemOfUnits.GetScaleAbbreviation (_targetScale ?? 0);
						break;
					case "n":
						_prefix = "";
						break;
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
					return string.Format (_genericNFI, "{0:#,##0.000} {1}{2}", _metricValue / Math.Pow (10, _targetScale ?? 0),
						_prefix, _quantifier);
				case "f":
					//TODO: Full formatting rules without smart-scaling.
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue, false);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						_prefix = InternationalSystemOfUnits.GetScalePrefix (_targetScale ?? 0);
						break;
					case "a":
						_prefix = InternationalSystemOfUnits.GetScaleAbbreviation (_targetScale ?? 0);
						break;
					case "n":
						_prefix = "";
						break;
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
					return string.Format ("{0}{1}{2}", (_metricValue / Math.Pow (10, _targetScale ?? 0)).ToString (_numberFormat, formatProvider),
						_prefix, _quantifier);
				case "F":
					//Full formatting rules with smart-scaling.
					_targetScale = _targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue, true);
					switch (_quantifierType.ToLowerInvariant ()) {
					case "f":
						_prefix = InternationalSystemOfUnits.GetScalePrefix (_targetScale ?? 0);
						break;
					case "a":
						_prefix = InternationalSystemOfUnits.GetScaleAbbreviation (_targetScale ?? 0);
						break;
					case "n":
						_prefix = "";
						break;
					default:
						throw new FormatException (string.Format (Exceptions.NotSupported_FormatSequence, format));
					}
					return string.Format ("{0}{1}{2}", (_metricValue / Math.Pow (10, _targetScale ?? 0)).ToString (_numberFormat, formatProvider),
						_prefix, _quantifier);
				case "N":
				case "n":
					//Numeric only (without quantifier and scaling):
					return _metricValue.ToString (_numberFormat, formatProvider);
				case "S":
				case "s":
					//Scientific format (exponential):
					_numberFormat = _formatTokens [0]?.Groups ["number"]?.Value ?? "e10";
					return string.Format ("{0} {1}", 
						_metricValue.ToString (_numberFormat, formatProvider), _quantifier).TrimEnd ();
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
