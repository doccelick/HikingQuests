using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HikingQuests.Server.Utilities
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            ObjectResult result;

            if (context.Exception is ArgumentException ||
                context.Exception is ArgumentNullException)
            {
                result = new BadRequestObjectResult(new { message = context.Exception.Message });
            }
            else
            {
                _logger.LogError(context.Exception, "Unhandled exception occured");

                var response = new
                {
                    message = "An unexpected error occured",
                    details = _webHostEnvironment.IsDevelopment() ? context.Exception.Message : null
                };


                result = new ObjectResult(response)
                {
                    StatusCode = 500
                };
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
