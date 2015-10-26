using System;
using System.ComponentModel;

namespace VKS.Measures
{
    /// <summary>
    /// Abstract class that defines basic requirements and rules for all measures.
    /// </summary>
    /// <typeparam name="TPrimitive">The type of the measure's underlying primitive.</typeparam>
    public abstract class Measure<TPrimitive> : INotifyPropertyChanged, IFormattable where TPrimitive : struct, IComparable
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
        protected virtual TPrimitive InnerValue
        {
            get { return FInnerValue; }
            set
            {
                if(FInnerValue.CompareTo(value) != 0)
                {
                    OnPropertyChanged();
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
        public abstract string ToString(string format, IFormatProvider formatProvider);

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event on this instance.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            var _Handler = PropertyChanged;
            if (_Handler != null)
                _Handler(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
