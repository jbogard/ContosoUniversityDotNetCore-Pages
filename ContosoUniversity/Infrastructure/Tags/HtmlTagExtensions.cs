using HtmlTags;

namespace ContosoUniversity.Infrastructure.Tags;

public static class HtmlTagExtensions
{
    public static HtmlTag AddPlaceholder(this HtmlTag tag, string placeholder) 
        => tag.Attr("placeholder", placeholder);

    public static HtmlTag AddPattern(this HtmlTag tag, string pattern) 
        => tag.Data("pattern", pattern);

    public static HtmlTag AutoCapitalize(this HtmlTag tag) 
        => tag.Data("autocapitalize", "true");
}