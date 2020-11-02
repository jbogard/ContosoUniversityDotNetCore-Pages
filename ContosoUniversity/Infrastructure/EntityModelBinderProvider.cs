using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContosoUniversity.Infrastructure
{
    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context) 
            => typeof(IEntity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
    }
}