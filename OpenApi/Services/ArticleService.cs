using System;
using System.Collections.Generic;
using System.Linq;
using OpenApi.Models;

namespace OpenApi.Services
{
#pragma warning disable 1591 // Missing XML Doc
    public class ArticleService
    {
        private readonly List<Article> _articles = new List<Article>()
        {
            new Article() { Number = 1, Name = "Test Article", Description = "", Price = (float) 42.42, },
            new Article() { Number = 2, Name = "Thinktecture Pen", Description = "A nice pen", Price = (float) 2.50, },
            new Article() { Number = 3, Name = "Cup of Coffee", Description = "Makes you awake", Price = (float) 2.50, },
        };

        public IReadOnlyList<Article> Articles => _articles;

        public void Add(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));

            if (_articles.Any(a => a.Number == article.Number))
                throw new InvalidOperationException($"An article with number {article.Number} already exists.");

            _articles.Add(article);
        }

        public void Update(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));

            var current = _articles.FirstOrDefault(a => a.Number == article.Number);
            if (current == null)
                throw new InvalidOperationException($"An article with number {article.Number} does not exists.");

            current.Name = article.Name;
            current.Description = article.Description;
            current.Price = article.Price;
        }

        public void Remove(Article article)
        {
            if (article == null) throw new ArgumentNullException(nameof(article));

            var current = _articles.FirstOrDefault(a => a.Number == article.Number);
            if (current == null)
                throw new InvalidOperationException($"An article with number {article.Number} does not exists.");

            _articles.Remove(current);
        }
    }
#pragma warning restore 1591
}
