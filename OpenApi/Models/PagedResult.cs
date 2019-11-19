using System.Collections.Generic;

namespace OpenApi.Models
{
    /// <summary>
    /// Contains a paged result of different data entries.
    /// </summary>
    /// <typeparam name="T">The type of the data entries.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The actual entries in this paged result.
        /// </summary>
        public IEnumerable<T> Entries { get; set; }

        /// <summary>
        /// The starting index of entries returned.
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// The total amount of available entries.
        /// </summary>
        public int TotalAmount { get; set; }
    }
}
