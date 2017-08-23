using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ContosoUniversity.Infrastructure.Tags
{
    [HtmlTargetElement("display-label-tag", TagStructure = TagStructure.WithoutEndTag)]
    public class DisplayLabelTagHelper : HtmlTagTagHelper
    {
        protected override string Category { get; } = nameof(TagConventions.DisplayLabels);
    }
}