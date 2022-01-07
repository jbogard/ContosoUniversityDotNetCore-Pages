using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Infrastructure;

public class EntityModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var original = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (original != ValueProviderResult.None)
        {
            var originalValue = original.FirstValue;
            if (int.TryParse(originalValue, out var id))
            {
                var dbContext = bindingContext.HttpContext.RequestServices.GetRequiredService<SchoolContext>();

                var entity = await dbContext.FindAsync(bindingContext.ModelType, id);

                bindingContext.Result = entity != null ? ModelBindingResult.Success(entity) : bindingContext.Result;
            }
        }

    }
}