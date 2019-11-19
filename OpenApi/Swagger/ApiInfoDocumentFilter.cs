using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Swagger
{
    /// <summary>
    /// Adds information to the OpenApi Info document
    /// </summary>
	public class ApiInfoDocumentFilter : IDocumentFilter
	{
        /// <summary>
        /// Applies the filter to the document.
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="context">The context.</param>
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			if (string.IsNullOrEmpty(swaggerDoc.Info.Description) || !swaggerDoc.Info.Description.Contains("automatically generated"))
            {
                if (!string.IsNullOrEmpty(swaggerDoc.Info.Description))
                {
                    swaggerDoc.Info.Description += "</br>";
                }

                swaggerDoc.Info.Description += $"This document was automatically generated as of {DateTime.UtcNow}.";
            }
		}
	}
}
