using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Extensions
{
    public class ErrorHandler
    {
        private  RequestDelegate _next;

        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            await response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StackTrace = exception.StackTrace, // or custom one
                Message = exception.Message, // or custom one
                Description = exception.InnerException?.Message
            }));
        }
    }
}