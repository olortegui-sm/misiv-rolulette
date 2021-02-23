using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Exceptions
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        IHostingEnvironment env;
        public HttpGlobalExceptionFilter(IHostingEnvironment _env)
        {
            env = _env;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(CustomException))
            {
                var json = new JsonErrorResponse
                {
                    Messages = ((CustomException)context.Exception).Messages
                };

                if (env.IsEnvironment("Local")
                || env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }

                context.Result = new ObjectResult(json);
                context.HttpContext.Response.StatusCode = ((CustomException)context.Exception).HttpStatusCode;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "An error occur.Try it again." }
                };

                if (env.IsEnvironment("Local")
                || env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception;
                }
                context.Result = new ObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        public class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}
