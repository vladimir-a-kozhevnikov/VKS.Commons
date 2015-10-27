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
		[TestCase(double.NaN, 0, ExpectedException = typeof(ArgumentOutOfRangeException))]
		[TestCase(double.NegativeInfinity, 0)]
		[TestCase(double.PositiveInfinity, 0)]
		[TestCase(1D, 0)]
		[TestCase(1350.2345323434534D, 3)]
		[TestCase(0.024D, -2)]
		[TestCase(0.0000000345D, -8)]
		public void Test_GetScalePower(double value, int expectedResult)
		{
			Assert.AreEqual(expectedResult, InternationalSystemOfUnits.GetScalePower(value));
		}

		[TestCase(1, "en-US")]
		[TestCase(5, "ru-RU")]
		[TestCase(5, "fr-FR")]
		public void Test_GetScalePrefix(int scale, string cultureCode = "en-US")
		{
			Console.WriteLine(InternationalSystemOfUnits.GetScalePrefix(scale));
		}
	}
}
