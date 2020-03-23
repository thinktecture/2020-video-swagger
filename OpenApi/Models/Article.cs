using System.ComponentModel.DataAnnotations;

namespace OpenApi.Models
{
    /// <summary>
    /// Represents an article.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// The unique number of the article.
        /// </summary>
        /// <example>5</example>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// The name of the article.
        /// </summary>
        /// <example>Water</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// An optional description of the article.
        /// </summary>
        /// <example>A non-alcoholic beverage</example>
        public string Description { get; set; }

        /// <summary>
        /// The price of the article.
        /// </summary>
        /// <example>2</example>
        public float Price { get; set; }
    }
}
