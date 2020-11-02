using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ContosoUniversity.Infrastructure
{
    public class ValidatorPageFilter : IPageFilter
    {
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request.Method == "GET")
                {
                    var result = new BadRequestResult();
                    context.Result = result;
                }
                else
                {
                    var result = new ContentResult();
                    var content = JsonConvert.SerializeObject(context.ModelState,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    result.Content = content;
                    result.ContentType = "application/json";

                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = result;
                }
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }
    }
}