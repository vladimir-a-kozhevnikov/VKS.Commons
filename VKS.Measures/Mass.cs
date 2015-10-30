using System;

using VKS.Measures.RC;

namespace VKS.Measures
{
	/// <summary>
	/// Mass physical measure.
	/// </summary>
	/// <remarks>
	/// Negative mass potentially could exists but not in common business logic. So, to prevent errors in calculations
	/// negative mass marked as invalid values. Nonetheless, you could compile library with $(NEGATIVE_MASS) condition.
	/// </remarks>
	public sealed class Mass: PhysicalMeasure, IComparable<Mass>, IEquatable<Mass>
	{

		#region IComparable implementation

		/// <summary>
		/// Compares this instance of <see cref="Mass"/> to another instance.
		/// </summary>
		/// <returns>The comparison result.</returns>
		/// <param name="other">An comparison target.</param>
		public int CompareTo (Mass other)
		{
			if (other == null)
				return -1;
			return this.InnerValue.CompareTo (other.InnerValue);
		}

		/// <summary>
		/// Compares this instance of <see cref="Mass"/> to another object.
		/// </summary>
		/// <returns>The comparison result.</returns>
		/// <param name="obj">An comparison target.</param>
		public override int CompareTo (object obj)
		{
			return CompareTo (obj as Mass);
		}

		#endregion

		/// <summary>
		/// Gets the <see cref="VKS.Measures.Mass"/> with the specified targetMeasurementSystem.
		/// </summary>
		/// <param name="targetMeasurementSystem">Target measurement system.</param>
		public override double this [MeasurementSystem targetMeasurementSystem] {
			get {
				return this.ConvertToPrimitive (targetMeasurementSystem);
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="VKS.Measures.Mass"/> value considering <paramref name="measurementSystem"/> and
		/// <paramref name="scale"/> level.
		/// </summary>
		/// <param name="measurementSystem">Measurement system used for convertions.</param>
		/// <param name="scale">Scale level used for convertions.</param>
		public override double this [MeasurementSystem measurementSystem, int scale] {
			get {
				var _scaled = ScaleMeasure (measurementSystem, scale);
				return _scaled.ScaledValue;
			}
			set {
				
				double _newValue;

				switch (measurementSystem) {
				case MeasurementSystem.InternationalSystemOfUnits:
					_newValue = value * Math.Pow (10, scale);
					break;
				default:
					throw new MeasureSystemException (Exceptions.NotSupported_MeasurementSystem);
				}

				ConvertFromPrimitive (_newValue, measurementSystem);


			}
		}

		/// <summary>
		/// Gets the quantifier name for the specified targetMeasurementSystem.
		/// </summary>
		/// <param name="targetMeasurementSystem">Target measurement system.</param>
		/// <param name="isAbbreviation">If set to <c>true</c> is abbreviation.</param>
		public override string this [MeasurementSystem targetMeasurementSystem, bool isAbbreviation] {
			get {
				switch (targetMeasurementSystem) {
				case MeasurementSystem.InternationalSystemOfUnits:
					return isAbbreviation ? ISOUQuantifiers.A_Mass : ISOUQuantifiers.F_Mass;
				default:
					throw new ArgumentOutOfRangeException ("targetMeasurementSystem", Exceptions.NotSupported_MeasurementSystem);
				}
			}
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="VKS.Measures.Mass"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return InnerValue.GetHashCode ();
		}

		protected override void ConvertFromPrimitive (double sourceValue, MeasurementSystem sourceSystem)
		{
			if (sourceValue < 0)
				throw new MeasureException (string.Format (Exceptions.NegativeValuesNotAllowed, MeasuresName.Mass));
			switch (sourceSystem) {
			case MeasurementSystem.InternationalSystemOfUnits:
				InnerValue = sourceValue;
				break;
			default:
				throw new MeasureSystemException (Exceptions.NotSupported_MeasurementSystem);
			}
		}

		protected override double ConvertToPrimitive (MeasurementSystem targetSystem)
		{
			switch (targetSystem) {
			case MeasurementSystem.InternationalSystemOfUnits:
				return InnerValue;
			default:
				throw new MeasureSystemException (Exceptions.NotSupported_MeasurementSystem);
			}
		}

		internal override ScalingData<double> ScaleMeasure (MeasurementSystem targetSystem, int? targetScale = null)
		{
			switch (targetSystem) {
			case MeasurementSystem.InternationalSystemOfUnits:
				var _metricValue = ConvertToPrimitive (MeasurementSystem.InternationalSystemOfUnits);
				var _scale = (targetScale ?? InternationalSystemOfUnits.GetScalePower (_metricValue)) + 3;
				return new ScalingData<double> {
					ScaleLevel = _scale - 3,
					ScaledValue = _metricValue / Math.Pow (10, _scale - 3),
					Quantifier = (_scale == 6) ? ISOUQuantifiers.F_Mass_Ton : string.Format ("{0}{1}", InternationalSystemOfUnits.GetScalePrefix (_scale),
						this [targetSystem, false]),
					Abbreviation = (_scale == 6) ? ISOUQuantifiers.A_Mass_Ton : string.Format ("{0}{1}", InternationalSystemOfUnits.GetScaleAbbreviation (_scale),
						this [targetSystem, true])
				};
			default:
				throw new MeasureSystemException (Exceptions.NotSupported_MeasurementSystem);
			}
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="VKS.Measures.Mass"/> is equal to the current <see cref="VKS.Measures.Mass"/>.
		/// </summary>
		/// <param name="other">The <see cref="VKS.Measures.Mass"/> to compare with the current <see cref="VKS.Measures.Mass"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="VKS.Measures.Mass"/> is equal to the current
		/// <see cref="VKS.Measures.Mass"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (Mass other)
		{
			if (other == null)
				return false;

			var _thisScale = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits);
			var _otherScale = ScaleMeasure (MeasurementSystem.InternationalSystemOfUnits);

			if (_thisScale.ScaleLevel != _otherScale.ScaleLevel)
				return false;

			var _scaledDelta = Math.Abs (_thisScale.ScaledValue - _otherScale.ScaledValue);

			return (_scaledDelta <= 0.00000001d);
		
		}


		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="VKS.Measures.Mass"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="VKS.Measures.Mass"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="VKS.Measures.Mass"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			var _other = obj as Mass;
			return Equals (_other);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="VKS.Measures.Mass"/> class.
		/// </summary>
		public Mass ()
		{
			InnerValue = 0D;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VKS.Measures.Mass"/> class and populates it with
		/// specified value.
		/// </summary>
		/// <param name="sourceSystem">Source measurement system.</param>
		/// <param name="value">Initial value of measure.</param>
		public Mass (MeasurementSystem sourceSystem, double value)
		{
			ConvertFromPrimitive (value, sourceSystem);
		}

		/// <param name="first">The first summand.</param>
		/// <param name="second">The second summand.</param>
		public static Mass operator + (Mass first, Mass second)
		{
			if ((first == null) || (second == null))
				return null;
			
			return new Mass {
				InnerValue = first.InnerValue + second.InnerValue
			};


		}

		#region Operators for Mass

		/// <param name="first">The minuend.</param>
		/// <param name="second">The subtrahend.</param>
		public static Mass operator - (Mass first, Mass second)
		{
			if ((first == null) || (second == null))
				return null;
			
			var _result = first.InnerValue - second.InnerValue;
			if (_result < 0)
				throw new MeasureException (string.Format (Exceptions.NegativeValuesNotAllowed, MeasuresName.Mass));
			return new Mass {
				InnerValue = _result
			};
		}

		/// <param name="first">The first multiplier.</param>
		/// <param name="second">The second multiplier.</param>
		public static Mass operator * (Mass first, double second)
		{
			if (first == null)
				return null;
			
			if (second < 0)
				throw new MeasureException (string.Format (Exceptions.NegativeValuesNotAllowed, MeasuresName.Mass));

			return new Mass {
				InnerValue = first.InnerValue * second
			};
		}

		/// <param name="first">The first multiplier.</param>
		/// <param name="second">The second multiplier.</param>
		public static Mass operator * (double first, Mass second)
		{
			return second * first;
		}

		/// <param name="first">The divident.</param>
		/// <param name="second">The divider.</param>
		public static Mass operator / (Mass first, double second)
		{
			if (first == null)
				return null;
			return new Mass {
				InnerValue = first.InnerValue / second
			};
		}

		/// <param name="first">The divident.</param>
		/// <param name="second">The divider.</param>
		public static double operator / (Mass first, Mass second)
		{
			if ((first == null) || (second == null))
				throw new ArithmeticException ();

			return first.InnerValue / second.InnerValue;
				
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator == (Mass first, Mass second)
		{
			if (((object)first == null) && ((object)second == null))
				return true;
			if (((object)first == null) || ((object)second == null))
				return false;
			return first.Equals (second);
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator != (Mass first, Mass second)
		{
			if (((object)first == null) && ((object)second == null))
				return false;
			if (((object)first == null) || ((object)second == null))
				return true;
			return !first.Equals (second);
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator > (Mass first, Mass second)
		{
			if (first == second)
				return false;
			if (first != null)
				return first.CompareTo (second) > 0;
			else
				return false;
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator < (Mass first, Mass second)
		{
			if (first == second)
				return false;
			if (first != null)
				return first.CompareTo (second) < 0;
			else
				return true;
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator >= (Mass first, Mass second)
		{
			if (first == second)
				return false;
			if (first != null)
				return first.CompareTo (second) >= 0;
			else
				return false;
		}

		/// <param name="first">The first comparable.</param>
		/// <param name="second">The second comparable.</param>
		public static bool operator <= (Mass first, Mass second)
		{
			if (first == second)
				return false;
			if (first != null)
				return first.CompareTo (second) <= 0;
			else
				return true;
		}

		#endregion
	}
}

