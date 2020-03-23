using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Swagger
{
    /// <summary>
    /// Adds correlation if parameter to all endpoints.
    /// </summary>
    public class CorrelationIdHeaderOperationFilter : IOperationFilter
    {
        private const string HeaderName = "correlation-id";

        /// <summary>
        /// Applies the filter to the operation.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.All(p => p.Name != HeaderName))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    // Make sure that the resulting info meets all requirements
                    // of the OpenAPI Specification (i.e. has schema or content field set)
                    Name = HeaderName,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = false,
                    AllowEmptyValue = false,
                    Example = new OpenApiString(Guid.NewGuid().ToString()),
                    Description = "Optional correlation id. Can be any string. Should be unique per call. If provided, all logging will use this id to be able to correlate requests spanning multiple services.",
                });
            }
        }
    }
}
