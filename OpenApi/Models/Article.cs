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
        public int Number { get; set; }

        /// <summary>
        /// The name of the article.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An optional description of the article.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the article.
        /// </summary>
        public float Price { get; set; }
    }
}
