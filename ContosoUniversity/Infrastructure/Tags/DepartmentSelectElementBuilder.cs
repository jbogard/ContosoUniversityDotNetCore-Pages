using ContosoUniversity.Models;

namespace ContosoUniversity.Infrastructure.Tags;

public class DepartmentSelectElementBuilder : EntitySelectElementBuilder<Department>
{
    protected override int GetValue(Department instance) => instance.Id;

    protected override string GetDisplayValue(Department instance) => instance.Name;
}