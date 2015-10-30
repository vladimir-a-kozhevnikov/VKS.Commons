using System;

namespace VKS.Measures
{
	public class MeasureException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureException"/> class
		/// </summary>
		public MeasureException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public MeasureException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public MeasureException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

