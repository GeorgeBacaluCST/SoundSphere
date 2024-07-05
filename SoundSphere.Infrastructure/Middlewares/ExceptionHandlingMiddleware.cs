﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundSphere.Infrastructure.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace SoundSphere.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try { await next(context); }
            catch (Exception exception)
            {
                ProblemDetails problem = exception switch
                {
                    ResourceNotFoundException ex => new ProblemDetails { Title = "Resource not found", Status = StatusCodes.Status404NotFound, Detail = ex.Message },
                    InvalidRequestException ex => new ProblemDetails { Title = "Invalid request", Status = StatusCodes.Status400BadRequest, Detail = ex.Message },
                    _ => new ProblemDetails { Title = "Server Error", Status = StatusCodes.Status500InternalServerError, Detail = exception.Message }
                };
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}