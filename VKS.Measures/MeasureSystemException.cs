using System;

namespace VKS.Measures
{
	/// <summary>
	/// Thrown when exception occurs within measure system.
	/// </summary>
	public class MeasureSystemException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureSystemException"/> class
		/// </summary>
		public MeasureSystemException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureSystemException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public MeasureSystemException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MeasureSystemException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public MeasureSystemException (string message, Exception inner) : base (message, inner)
		{
		}

	}
}

