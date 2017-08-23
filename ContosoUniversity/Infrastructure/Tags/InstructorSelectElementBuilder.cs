using ContosoUniversity.Models;

namespace ContosoUniversity.Infrastructure.Tags
{
    public class InstructorSelectElementBuilder : EntitySelectElementBuilder<Instructor>
    {
        protected override int GetValue(Instructor instance)
        {
            return instance.Id;
        }

        protected override string GetDisplayValue(Instructor instance)
        {
            return instance.FullName;
        }
    }
}