using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

using VKS.Measures;
using VKS.Measures.RC;

namespace VKS.Measures.Tests
{
	[TestFixture]
	public class ISoURoutinesTests
	{
		[TestCase (double.NaN, false, 0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[TestCase (double.NegativeInfinity, false, 0)]
		[TestCase (double.PositiveInfinity, false, 0)]
		[TestCase (1D, false, 0)]
		[TestCase (1350.2345323434534D, false, 3)]
		[TestCase (0.024D, false, -2)]
		[TestCase (0.0000000345D, false, -9)]
		[TestCase (0.0000000543564576756834645D, false, -9)]
		[TestCase (12434.546345236453425564356, true, 3)]
		[TestCase (124345.46345236453425564356, true, 3)]
		[TestCase (-19434546.345236453425564356, true, 6)]
		//		[TestCase (12434.546345236453425564356, true, 3)]
//		[TestCase (12434.546345236453425564356, true, 3)]
//		[TestCase (12434.546345236453425564356, true, 3)]
//		[TestCase (12434.546345236453425564356, true, 3)]
		public void Test_GetScalePower (double value, bool useSmartScaling, int expectedResult)
		{
			Assert.AreEqual (expectedResult, InternationalSystemOfUnits.GetScalePower (value, useSmartScaling));
		}

		[TestCase (1, "en-US")]
		[TestCase (5, "ru-RU")]
		[TestCase (5, "fr-FR")]
		[TestCase (-124, "en-US")]
		[TestCase (-5, "ru-RU")]
		public void Test_GetScalePrefix (int scale, string cultureCode = "en-US")
		{
			ISOUScales.Culture = System.Globalization.CultureInfo.GetCultureInfo (cultureCode);
			Console.WriteLine (InternationalSystemOfUnits.GetScalePrefix (scale));
		}

		[TestCase (1, "en-US")]
		[TestCase (5, "ru-RU")]
		[TestCase (5, "fr-FR")]
		[TestCase (-124, "en-US")]
		[TestCase (-5, "ru-RU")]
		public void Test_GetScaleAbbreviation (int scale, string cultureCode = "en-US")
		{
			ISOUScales.Culture = System.Globalization.CultureInfo.GetCultureInfo (cultureCode);
			Console.WriteLine (InternationalSystemOfUnits.GetScaleAbbreviation (scale));
		}

	}
}
