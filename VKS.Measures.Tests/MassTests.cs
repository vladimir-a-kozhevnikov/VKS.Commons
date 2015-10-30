using NUnit.Framework;
using System;

using VKS.Measures;

namespace VKS.Measures.Tests
{
	[TestFixture ()]
	public class MassTests
	{
		[TestCase (235346.346564, "g")]
		[TestCase (236456123557.345, "G")]
		[TestCase (4.66756, "q::0")]
		[TestCase (4.3457547, "Q:0:-2")]
		[TestCase (34254366575673456.346456324557456, "s")]
		[TestCase (45345345452345.5635623453245, "f:0:3:a:0.000")]
		public void TestFormat (double massValue, string format)
		{
			var _mass = new Mass (MeasurementSystem.InternationalSystemOfUnits, massValue);
			Console.WriteLine ("{0}: {1}", format, _mass.ToString (format, null));
		}

		[TestCase (4454.354, 1000.34, 5454.694)]
		[TestCase (-2234, 0, -2234.0, ExpectedException = typeof(MeasureException))]
		public void Test_this (double massValue, double secondValue, double expectedValue)
		{
			var _mass = new Mass (MeasurementSystem.InternationalSystemOfUnits, massValue);
			var _second = new Mass (MeasurementSystem.InternationalSystemOfUnits, secondValue);
			var _sum = _mass + _second;
			Assert.AreEqual (expectedValue, _sum [MeasurementSystem.InternationalSystemOfUnits]);
		}

		[TestCase (237.0143d, 15, 2.370143E+017)]
		[TestCase (434.41835d, 18, 4.3441835E+020)]
		[TestCase (5.3723829d, -23, 5.3723829E-023)]
		[TestCase (3.3785357d, 17, 3.3785357E+017)]
		[TestCase (352.551717399986d, 0, 352.551717399986)]
		[TestCase (507.418900641385, -13, 0.0000000000507418900641385)]
		[TestCase (474.692846785342, -18, 4.74692846785342E-016)]
		[TestCase (112.931674786271, -24, 1.12931674786271E-022)]
		[TestCase (335.453591469686, 17, 3.35453591469686E+019)]
		[TestCase (401.70052891784, -8, 0.00000401070052891784)]
		[TestCase (65.0238457628966, -20, 6.50238457628966E-019)]
		[TestCase (147.555830623151, 23, 1.47555830623151E+025)]
		[TestCase (27.9104863406372, -10, 0.00000000279104863406372)]
		[TestCase (77.5769800986412, 9, 77576980098.6412)]
		[TestCase (138.098616475949, 5, 13809861.6475948)]
		[TestCase (422.752413284753, 19, 4.22752413284753E+021)]
		[TestCase (256.272127023692, 17, 2.56272127023692E+019)]
		[TestCase (218.363542448025, -23, 2.18363542448025E-021)]
		[TestCase (593.073599935247, 19, 5.93073599935247E+021)]
		[TestCase (552.514370933883, 13, 5.52514370933883E+015)]
		[TestCase (42.2587583721223, 6, 42258758.3721223)]
		[TestCase (344.608135585701, -8, 0.00000344608135585701)]
		public void Test_this_value (double massValue, int scale, double expectedValue)
		{
			var _mass = new Mass ();
			_mass [MeasurementSystem.InternationalSystemOfUnits, scale] = massValue;
			Assert.AreEqual (expectedValue, _mass [MeasurementSystem.InternationalSystemOfUnits]);
		}

	}
}

