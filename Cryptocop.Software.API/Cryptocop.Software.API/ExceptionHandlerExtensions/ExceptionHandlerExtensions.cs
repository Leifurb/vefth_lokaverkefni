using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Cryptocop.Software.API.Models.Exceptions;
using System.Net;
using System;

namespace Cryptocop.Software.API.ExceptionHandlerExtensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(
                error => {
                    error.Run(
                        async context => {
                            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if(exceptionHandlerFeature == null) { return; }
                            var exception = exceptionHandlerFeature.Error;
                            var statusCode = (int) HttpStatusCode.InternalServerError;

                            if(exception is ResourceNotFoundException) {
                                statusCode = (int) HttpStatusCode.NotFound;
                            } else if(exception is ModelFormatException) {
                                statusCode = (int) HttpStatusCode.PreconditionFailed;
                            } else if(exception is ArgumentOutOfRangeException) {
                                statusCode = (int) HttpStatusCode.BadRequest;
                            } else if(exception is IdentityException) {
                                statusCode = (int) HttpStatusCode.BadRequest;
                            } else if(exception is ResourceExistsException) {
                                statusCode = (int) HttpStatusCode.BadRequest;
                            } 

                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = statusCode;

                            ExceptionModel e =  new ExceptionModel{
                                StatusCode = statusCode,
                                ExceptionMessage = exception.Message
                            };
                            await context.Response.WriteAsync(e.ToString());
                        }
                    ); 
                }
            );

        }
    }
}