using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CliniFlow.API.Helper; // Agregamos el namespace para el mensaje CA1050

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Corregimos el aviso CS8602 con chequeo de nulidad (?)
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true ||
                           context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        if (hasAuthorize)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });

            // Usamos la nueva sintaxis de .NET 10 para evitar CS0117 y CS0246
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                }
            };
        }
    }
}