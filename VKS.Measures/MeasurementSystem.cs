using System;
using System.Diagnostics.Contracts;
using VKS.Measures.RC;

namespace VKS.Measures
{
	/// <summary>
	/// Measurement systems used for convertions.
	/// </summary>
	public enum MeasurementSystem
	{

		/// <summary>
		/// The international system of units (ISoU).
		/// </summary>
		InternationalSystemOfUnits
	}

	/// <summary>
	/// International system of units helper methods and routines.
	/// </summary>
	public static class InternationalSystemOfUnits
	{
		/// <summary>
		/// Gets the scale power for specified <paramref name="basicValue"/>.
		/// </summary>
		/// <returns>The scale power for specified value of measure.</returns>
		/// <param name="basicValue">The initial value of measure.</param>
		/// <remarks>
		/// For mass and other measures <paramref name="basicValue"/> should be multiplied by 1 000 due to
		/// usage of kilogram instead of gram.
		/// </remarks>
		public static int GetScalePower (double basicValue)
		{
			if (double.IsNaN(basicValue))
				throw new ArgumentOutOfRangeException(Exceptions.NotSupported_NANAsValue);

			if ((basicValue == 0)||(double.IsInfinity(basicValue)))
				return 0;
			var _Power = Math.Floor(Math.Log10(Math.Abs(basicValue)));

			return Convert.ToInt32(_Power);
		}

		/// <summary>
		/// Gets the scale prefix for specified scale level.
		/// </summary>
		/// <returns>The culture-dependent and translatable scale prefix.</returns>
		/// <param name="scale">Scale of measure as power of 10.</param>
		public static string GetScalePrefix (int scale)
		{

			if (scale == 0)
				return "";
			else
			{
				#region multipliers
				if (scale >= 24)
					return ISOUScales.Prefix_M24;
				if (scale >= 21)
					return ISOUScales.Prefix_M21;
				if (scale >= 18)
					return ISOUScales.Prefix_M18;
				if (scale >= 15)
					return ISOUScales.Prefix_M15;
				if (scale >= 12)
					return ISOUScales.Prefix_M12;
				if (scale >= 9)
					return ISOUScales.Prefix_M09;
				if (scale >= 6)
					return ISOUScales.Prefix_M06;
				if (scale >= 3)
					return ISOUScales.Prefix_M03;
				if (scale >= 2)
					return ISOUScales.Prefix_M02;
				if (scale >= 1)
					return ISOUScales.Prefix_M01;
				#endregion
				#region fractions
				if (scale <= -24)
					return ISOUScales.Prefix_F24;
				if (scale <= -21)
					return ISOUScales.Prefix_F21;
				if (scale <= -18)
					return ISOUScales.Prefix_F18;
				if (scale <= -15)
					return ISOUScales.Prefix_F15;
				if (scale <= -12)
					return ISOUScales.Prefix_F12;
				if (scale <= -9)
					return ISOUScales.Prefix_F09;
				if (scale <= -6)
					return ISOUScales.Prefix_F06;
				if (scale <= -3)
					return ISOUScales.Prefix_F03;
				if (scale <= -2)
					return ISOUScales.Prefix_F02;
				if (scale <= -1)
					return ISOUScales.Prefix_F01;
				#endregion

				return ""; //By default return null prefix if scale is -1...1
			}
		}

		/// <summary>
		/// Gets the scale prefix abbreviation for specified scale level.
		/// </summary>
		/// <returns>The culture-dependent and translatable scale prefix abbreviation.</returns>
		/// <param name="scale">Scale of measure as power of 10.</param>
		public static string GetScaleAbbreviation (int scale)
		{
			if (scale == 0)
				return "";
			else
			{
				#region multipliers
				if (scale >= 24)
					return ISOUScales.Abbr_M24;
				if (scale >= 21)
					return ISOUScales.Abbr_M21;
				if (scale >= 18)
					return ISOUScales.Abbr_M18;
				if (scale >= 15)
					return ISOUScales.Abbr_M15;
				if (scale >= 12)
					return ISOUScales.Abbr_M12;
				if (scale >= 9)
					return ISOUScales.Abbr_M09;
				if (scale >= 6)
					return ISOUScales.Abbr_M06;
				if (scale >= 3)
					return ISOUScales.Abbr_M03;
				if (scale >= 2)
					return ISOUScales.Abbr_M02;
				if (scale >= 1)
					return ISOUScales.Abbr_M01;
				#endregion
				#region fractions
				if (scale <= -24)
					return ISOUScales.Abbr_F24;
				if (scale <= -21)
					return ISOUScales.Abbr_F21;
				if (scale <= -18)
					return ISOUScales.Abbr_F18;
				if (scale <= -15)
					return ISOUScales.Abbr_F15;
				if (scale <= -12)
					return ISOUScales.Abbr_F12;
				if (scale <= -9)
					return ISOUScales.Abbr_F09;
				if (scale <= -6)
					return ISOUScales.Abbr_F06;
				if (scale <= -3)
					return ISOUScales.Abbr_F03;
				if (scale <= -2)
					return ISOUScales.Abbr_F02;
				if (scale <= -1)
					return ISOUScales.Abbr_F01;
				#endregion

				return ""; //By default return null prefix if scale is -1...1
			}
		}
	}
}

