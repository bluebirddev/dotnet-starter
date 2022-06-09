using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Bluebird.Core.Starter.Domain.Contracts.Services;
using Bluebird.Core.Starter.Repository.PostgresSql;
using Bluebird.Core.Starter.Repository.PostgresSql.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using Bluebird.Core.Starter.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Bluebird.Core.Starter.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add logging (Can add different kinds of logging here)
builder.Logging.AddConsole();

#region Configuration Methods
void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(gen =>
    {
        gen.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Assembly.GetExecutingAssembly().GetName().Name}", Version = "v1" });
        gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        gen.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
        gen.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
        gen.MapType<decimal?>(() => new OpenApiSchema { Type = "number", Format = "decimal?" });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        gen.IncludeXmlComments(xmlPath);
    });
}
void ConfigureCORS(WebApplicationBuilder builder)
{
    var origin = builder.Configuration.GetSection("AllowedHosts").Get<string>();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder
                .WithOrigins(origin)
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
    });
}
void ConfigureDbConnections(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("Main");
    // Add Postgres Context
    builder.Services.AddDbContext<DotnetStarterContext>(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(typeof(DotnetStarterContext).Assembly.FullName)));
}
void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });


    builder.Services.AddAuthorization();
    builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
}
void ConfigureDependencyInjection(WebApplicationBuilder builder)
{
    #region Internal
    #endregion Internal

    #region Repository Layer
    builder.Services.AddScoped<IMovieRepository, MovieRepository>();
    #endregion Repository Layer

    #region Service Layer
    builder.Services.AddScoped<IMovieService, MovieService>();
    #endregion Service Layer

    #region Integration Layer
    #endregion Integration Layer

    #region Gateway Layer
    #endregion Gateway Layer
}
void ConfigureHttpClients(WebApplicationBuilder builder)
{ }
#endregion

#region Services
builder.Services.AddRouting(options => options.LowercaseUrls = true);

ConfigureSwagger(builder);
ConfigureCORS(builder);

builder.Services.AddMvc(o => o.EnableEndpointRouting = false)
    // .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DotnetStarterContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

ConfigureDbConnections(builder);
ConfigureAuthentication(builder);
ConfigureDependencyInjection(builder);
ConfigureHttpClients(builder);
#endregion

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRewriter(new RewriteOptions().AddRedirect("^$", "/swagger"));
// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.EnableDeepLinking();
    c.DisplayRequestDuration();
    c.DefaultModelExpandDepth(2);
    c.DefaultModelsExpandDepth(-1);
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseCors();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseMvc();

app.Run();
