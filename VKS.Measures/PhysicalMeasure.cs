using VKS.Measures.Systems;
using System.ComponentModel;

namespace VKS.Measures
{
    /// <summary>
    /// Abstract class that defines behavior for all physical measures in library (length, mass, force, pressure etc).
    /// </summary>
    public abstract class PhysicalMeasure: Measure<double>
    {
        /// <summary>
        /// Converts from primitive value in specified <see cref="IMeasurementSystem"/> to an inner representation of measure.
        /// </summary>
        /// <param name="sourceSystem">The source system of measurement (i.e. Avoirdupois, ISoU etc).</param>
        /// <param name="sourceValue">The value of primitive.</param>
        protected abstract void ConvertFromPrimitive(double sourceValue, IMeasurementSystem sourceSystem);

        /// <summary>
        /// Converts from inner representation to primitive value (<see cref="System.Double"/>).
        /// </summary>
        /// <param name="targetSystem">The target system of measurements.</param>
        /// <returns>The primitive value that contains the measure representation in specified measurement system.</returns>
        protected abstract double ConvertToPrimitive(IMeasurementSystem targetSystem);

    }
}
