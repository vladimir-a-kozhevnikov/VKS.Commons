using System;
using System.ComponentModel;


namespace VKS.Measures
{
	/// <summary>
	/// Abstract class that defines basic requirements and rules for all measures.
	/// </summary>
	/// <typeparam name="TPrimitive">The type of the measure's underlying primitive.</typeparam>
	public abstract class Measure<TPrimitive> : INotifyPropertyChanged, IFormattable,
		IComparable where TPrimitive : struct, IComparable
	{
		/// <summary>
		/// The back-storage field that contains actual value of measure.
		/// </summary>
		private TPrimitive FInnerValue;

		/// <summary>
		/// Gets or sets the inner representation of the measure value.
		/// </summary>
		/// <value>
		/// The inner value of the measure.
		/// </value>
		protected virtual TPrimitive InnerValue {
			get { return FInnerValue; }
			set {
				if (!FInnerValue.Equals (value)) {
					OnPropertyChanged ();
					FInnerValue = value;
				}
			}
		}

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The specified format sequence.</param>
		/// <param name="formatProvider">The format provider for instance.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public abstract string ToString (string format, IFormatProvider formatProvider);

		/// <summary>
		/// Gets the <see cref="VKS.Measures.Measure &lt; TPrimitive &gt;"/> value converted to the specified <paramref name="targetMeasurementSystem"/>.
		/// </summary>
		/// <param name="targetMeasurementSystem">Target measurement system.</param>
		public abstract TPrimitive this [MeasurementSystem targetMeasurementSystem] {
			get;

		}

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
		public abstract TPrimitive this [MeasurementSystem measurementSystem, int scale] {
			get;
			set;
		}

		/// <summary>
		/// Compares this instance to another object.
		/// </summary>
		/// <returns>The comparison result.</returns>
		/// <param name="obj">An object to compare.</param>
		public abstract int CompareTo (object obj);

		/// <summary>
		/// Fires the <see cref="PropertyChanged"/> event on this instance.
		/// </summary>
		/// <param name="propertyName">Name of the changed property.</param>
		protected virtual void OnPropertyChanged ([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
		{
			var _Handler = PropertyChanged;
			if (_Handler != null)
				_Handler (this, new PropertyChangedEventArgs (propertyName));
		}

		/// <summary>
		/// Converts from primitive value in specified <see cref="MeasurementSystem"/> to an inner representation of measure.
		/// </summary>
		/// <param name="sourceSystem">The source system of measurement (i.e. Avoirdupois, ISoU etc).</param>
		/// <param name="sourceValue">The value of primitive.</param>
		protected abstract void ConvertFromPrimitive (TPrimitive sourceValue, MeasurementSystem sourceSystem);

		/// <summary>
		/// Converts from inner representation to primitive value (<see cref="System.Double"/>).
		/// </summary>
		/// <param name="targetSystem">The target system of measurements.</param>
		/// <returns>The primitive value that contains the measure representation in specified measurement system.</returns>
		protected abstract TPrimitive ConvertToPrimitive (MeasurementSystem targetSystem);

		/// <summary>
		/// Scales the measure to a target scale passed in <paramref name="targetScale"/> parameter.
		/// </summary>
		/// <returns>The <see cref="ScalingData&lt;TPrimitive&gt;"/> structure that contains information about scaled measure.</returns>
		/// <param name="targetSystem">Target measurement system.</param>
		/// <param name="targetScale">Target scale level. If this parameter set to null, then system tries to scale
		/// passed value automatically to the best fitted scale level.</param>
		internal abstract ScalingData<TPrimitive> ScaleMeasure (MeasurementSystem targetSystem, int? targetScale = null);

	}
}
