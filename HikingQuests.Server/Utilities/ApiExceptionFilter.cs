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
              
            
            switch (context.Exception)
            {
                case ArgumentException argEx:
                    result = new BadRequestObjectResult(new { message = argEx.Message });
                    break;

                case KeyNotFoundException keyEx:
                    result = new NotFoundObjectResult(new { message = keyEx.Message });
                    break;

                case InvalidOperationException invOpEx:
                    result = new ConflictObjectResult(new { message = invOpEx.Message });
                    break;
                default:
                    _logger.LogError(context.Exception, "Unhandled exception occurred");
                    var response = new
                    {
                        message = "An unexpected error occurred",
                        details = _webHostEnvironment.IsDevelopment() ? context.Exception.ToString() : null
                    };
                    result = new ObjectResult(response) { StatusCode = 500 };
                    break;
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
