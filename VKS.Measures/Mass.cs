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
	public sealed class Mass: PhysicalMeasure, IComparable<Mass>
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
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
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


		#region implemented abstract members of Measure

		/// <summary>
		/// Compares this instance of <see cref="Mass"/> to another object.
		/// </summary>
		/// <returns>The comparison result.</returns>
		/// <param name="obj">An comparison target.</param>
		public override int CompareTo (object obj)
		{
			return this.CompareTo (obj as Mass);
		}

		protected override void ConvertFromPrimitive (double sourceValue, MeasurementSystem sourceSystem)
		{
			throw new NotImplementedException ();
		}

		protected override double ConvertToPrimitive (MeasurementSystem targetSystem)
		{
			throw new NotImplementedException ();
		}

		internal override ScalingData<double> ScaleMeasure (MeasurementSystem targetSystem, int? targetScale = null)
		{
			throw new NotImplementedException ();
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
		/// Initializes a new instance of the <see cref="VKS.Measures.Mass"/> class.
		/// </summary>
		/// <param name="sourceSystem">Source measurement system.</param>
		/// <param name="value">Initial value of measure.</param>
		public Mass (MeasurementSystem sourceSystem, double value)
		{
			ConvertFromPrimitive (value, sourceSystem);
		}

		///<summary>
		/// 	Sum operator
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		public static Mass operator + (Mass first, Mass second)
		{
			if ((first == null) || (second == null))
				return null;
			
			return new Mass () {
				InnerValue = first.InnerValue + second.InnerValue
			};


		}

		public static Mass operator - (Mass first, Mass second)
		{
			if ((first == null) || (second == null))
				return null;
			
			var _result = first.InnerValue - second.InnerValue;
			if (_result < 0)
				throw new ArgumentOutOfRangeException ("second",
					string.Format (Exceptions.NegativeValuesNotAllowed, MeasuresName.Mass));
			return new Mass {
				InnerValue = _result
			};
		}

		public static Mass operator * (Mass first, double second)
		{
			if (first == null)
				return null;
			
			if (second < 0)
				throw new ArgumentOutOfRangeException ("second",
					string.Format (Exceptions.NegativeValuesNotAllowed, MeasuresName.Mass));

			return new Mass {
				InnerValue = first.InnerValue * second
			};
		}

		public static Mass operator * (double first, Mass second)
		{
			return second * first;
		}

		public static bool operator == (Mass first, Mass second)
		{
			return Math.Abs (first.InnerValue - second.InnerValue) <= double.Epsilon;
		}

		public static bool operator != (Mass first, Mass second)
		{
			return Math.Abs (first.InnerValue - second.InnerValue) > double.Epsilon;
		}

	}
}

