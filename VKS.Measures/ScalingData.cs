namespace VKS.Measures
{
	/// <summary>
	/// Information about scaling data for measure
	/// </summary>
	internal struct ScalingData<TPrimitive> where TPrimitive: struct
	{
		/// <summary>
		/// The scale level for measure: 0 - no scale, negative values - fractions, positive values - multiplications.
		/// </summary>
		public int ScaleLevel;

		/// <summary>
		/// The scaled value of measure.
		/// </summary>
		public TPrimitive ScaledValue;

		/// <summary>
		/// The name of the quantity considering scale level.
		/// </summary>
		public string Quantifier;

		/// <summary>
		/// The quantity abbreviation considering scale level.
		/// </summary>
		public string Abbreviation;
	}
}

