using System;
using System.ComponentModel;

namespace VKS.Measures
{
	/// <summary>
	/// Abstract class that defines behavior for all physical measures in library (length, mass, force, pressure etc).
	/// </summary>
	public abstract class PhysicalMeasure: Measure<double>
	{

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
		public override string ToString (string format, System.IFormatProvider formatProvider)
		{
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
			throw new NotImplementedException ();
		}
	}
}
