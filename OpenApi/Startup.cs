using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenApi.Services;
using OpenApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OpenApi
{
#pragma warning disable 1591 // Missing XML Doc
    public class Startup
    {
        private static readonly int[] ApiVersions = new[] { 1, 2, 3, };
        private static readonly int DefaultApiVersion = ApiVersions.Last();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ArticleService>();
            services.AddControllers();

            services.AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(DefaultApiVersion, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options => {
                    options.Authority = "https://demo.identityserver.io";
                    options.ApiName = "api";
                });

            services
                .AddAuthorization(config => {
                    config.AddPolicy("ApiPolicy", builder => {
                        builder.RequireAuthenticatedUser();
                        builder.RequireScope("api");
                    });
                });

            services.AddVersionedApiExplorer(options =>
            {
                // Swagger uses GroupName to sort endpoints into certain documents, so make sure ApiExplorer
                // sets a group name that corresponds to the version url part
                options.GroupNameFormat = "'v'VVV";

                // Make sure we do not have the {version} part of the Url as parameter in the swagger urls
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c => {
                foreach (var version in ApiVersions)
                {
                    c.SwaggerDoc($"v{version}", new OpenApiInfo() {
                        Title = "My API Name",
                        Version = $"v{version}",
                        Description = "Sample API for our video series",
                        Contact = new OpenApiContact() {
                            Name = "Sebastian Gingter",
                            Email = "sebastian.gingter@thinktecture.com",
                            Url = new Uri("https://thinktecture.com"),
                        },
                        License = new OpenApiLicense() {
                            Name = "Licensed under the Apache 2.0 License",
                            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html"),
                        },
                    });
                }

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Name = "Authentication",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    OpenIdConnectUrl = new Uri("https://demo.identityserver.io"),
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("https://demo.identityserver.io/connect/authorize"),
                            TokenUrl = new Uri("https://demo.identityserver.io/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "api", "Scope to access the API" },
                            },
                        },
                    },
                });

                c.EnableAnnotations();
                c.IncludeXmlComments("./OpenApi.xml");

                c.DocumentFilter<ApiInfoDocumentFilter>();
                c.OperationFilter<CorrelationIdHeaderOperationFilter>();
                c.OperationFilter<AddSecurityRequirementOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c => {
                c.RouteTemplate = "openapi/{documentName}/openapi.json"; // default: "swagger/{documentName}/swagger.json"
            });

            app.UseSwaggerUI(c => {
                c.RoutePrefix = "documentation"; // default: "Swagger"

                foreach (var version in ApiVersions)
                {
                    c.SwaggerEndpoint($"/openapi/v{version}/openapi.json", $"OpenAPI Sample v{version}");
                }

                c.OAuthConfigObject = new OAuthConfigObject()
                {
                    ClientId = "interactive.public",
                    ClientSecret = "secret",
                    UsePkceWithAuthorizationCodeGrant = true,
                };

                c.InjectStylesheet("/customizations.css");

                c.EnableFilter();
                c.ConfigObject.DocExpansion = DocExpansion.List;
                c.ConfigObject.DisplayRequestDuration = true;
            });

            app.UseStaticFiles();
        }
    }
#pragma warning restore 1591
}
