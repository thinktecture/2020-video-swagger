using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenApi.Models;
using OpenApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OpenApi.Controllers
{
    /// <summary>
    /// Handles articles.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/>.
        /// </summary>
        /// <param name="articleService">The persistence service for articles.</param>
        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
        }

        /// <summary>
        /// Gets all articles available
        /// </summary>
        /// <returns>A list of all available articles.</returns>
        [HttpGet]
        [SwaggerOperation("Gets all articles.", "This operation will return all articles from the store.")]
        [SwaggerResponse(200, "A list of all available articles.", typeof(Article[]))]
        [SwaggerResponse(404, "No articles are available.")]
        public ActionResult<IEnumerable<Article>> Get()
        {
            if (_articleService.Articles.Count == 0)
                return NotFound();

            return Ok(GetPagedData(0, Int32.MaxValue).Entries);
        }

        /// <summary>
        /// Fetches an article based on its number.
        /// </summary>
        /// <param name="number">The article number.</param>
        /// <returns>The article if found; otherwise, 404.</returns>
        [HttpGet("{number}")]
        [SwaggerOperation("Gets a certain article.", "This operation will return a specific article from the store.")]
        [SwaggerResponse(200, "The requested article.", typeof(Article))]
        [SwaggerResponse(404, "No article found.")]
        public ActionResult<Article> GetByNumber([FromRoute][SwaggerParameter("The article number to fetch.")] int number)
        {
            var article = _articleService.Articles.FirstOrDefault(a => a.Number == number);

            if (article == null)
                return NotFound();

            return article;
        }

        /// <summary>
        /// Creates an article.
        /// </summary>
        /// <param name="article">The article to create.</param>
        /// <returns>201, when creation was successful; otherwise, 409</returns>
        [HttpPost]
        public IActionResult Add([FromBody] Article article)
        {
            try
            {
                _articleService.Add(article);
                return Created($"{Request.Path}/{article.Number}", article);
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="number">The number of the article to update.</param>
        /// <param name="article">The new article data to put instead of the current one.</param>
        /// <returns>Whether the article was successfully updated or not.</returns>
        [HttpPut("{number}")]
        public IActionResult Update([FromRoute] int number, [FromBody] Article article)
        {
            try
            {
                article.Number = number;
                _articleService.Update(article);

                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes an existing article.
        /// </summary>
        /// <param name="number">The number of the article to remove.</param>
        /// <returns>Whether deletion of the article was successful or not.</returns>
        [HttpDelete("{number}")]
        public IActionResult Delete([FromRoute] int number)
        {
            try
            {
                var article = new Article() { Number = number, };
                _articleService.Remove(article);

                Response.Headers.Add("x-deletion-id", Guid.NewGuid().ToString());
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        private PagedResult<Article> GetPagedData(int skip, int take)
        {
            return new PagedResult<Article>()
            {
                Entries = _articleService.Articles.Skip(skip).Take(take).ToArray(),
                StartIndex = skip,
                TotalAmount = _articleService.Articles.Count,
            };
        }
    }
}
