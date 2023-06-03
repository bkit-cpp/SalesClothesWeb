using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Extensions
{
    public class SwaggerHeaderOperationFilter : IOperationFilter
    {
        private readonly string _headerName;
        private readonly string _headerValue;

        public SwaggerHeaderOperationFilter(string headerName, string headerValue)
        {
            _headerName = headerName;
            _headerValue = headerValue;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            var headerParameter = new OpenApiParameter()
            {
                Name = _headerName,
                In = ParameterLocation.Header,
                Description = $"Token - {_headerName}",
                Required = true,
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Default = new OpenApiString(_headerValue)
                }
            };

            operation.Parameters.Add(headerParameter);
        }
    }
}
