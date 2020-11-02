using System.Linq;
using System.Text.RegularExpressions;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;

namespace ContosoUniversity.Infrastructure.Tags
{
    public class DefaultDisplayLabelBuilder : IElementBuilder
    {
        public bool Matches(ElementRequest subject)
        {
            return true;
        }

        public HtmlTag Build(ElementRequest request)
        {
            return new HtmlTag("").NoTag().Text(BreakUpCamelCase(request.Accessor.Name));
        }

        public static string BreakUpCamelCase(string fieldName)
        {
            var patterns = new[]
            {
                "([a-z])([A-Z])",
                "([0-9])([a-zA-Z])",
                "([a-zA-Z])([0-9])"
            };
            var output = patterns.Aggregate(fieldName,
                static (current, pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));
            return output.Replace('_', ' ');
        }
    }
}