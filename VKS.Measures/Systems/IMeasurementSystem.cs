using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKS.Measures.Systems
{
    /// <summary>
    /// Declaring interface for supported measurement systems.
    /// </summary>
    public interface IMeasurementSystem
    {
        /// <summary>
        /// Gets or sets the name of measurement system.
        /// </summary>
        /// <value>
        /// The name of measurement system.
        /// </value>
        string Name { get; set; }

    }
}
