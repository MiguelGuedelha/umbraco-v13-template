using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace UmbracoTemplate.Cms.Features.Navigation.GetNavigation;

public static class GetNavigationEndpoint
{
    public static IResult GetNavigation(IUmbracoContextFactory contextFactory)
    {
        using var context = contextFactory.EnsureUmbracoContext();
        
        var root = context.UmbracoContext.Content?.GetAtRoot().FirstOrDefault();

        if (root == null)
        {
            return TypedResults.NotFound();
        }

        var navItems = root.Children
            .Where(x => x.Value<bool>("showInNavigation"))
            .Select(x => GetChildrenNavItems(x, 0, 2))
            .ToList();
        
        navItems.Insert(0, new
        {
            Title = root.Value<string>("navigationTitle") ?? root.Value<string>("title") ?? root.Name,
            Url = root.Url(),
            Children = Enumerable.Empty<object>()
        });
        
        return TypedResults.Ok(navItems);
    }

    private static object GetChildrenNavItems(IPublishedContent parent, int parentLevel, int maxLevel)
    {
        if (parentLevel == maxLevel)
        {
            return new
            {
                Title = parent.Value<string>("navigationTitle") ?? parent.Value<string>("title") ?? parent.Name,
                Url = parent.Url(),
                Children = Enumerable.Empty<object>()
            };
        }

        return new
        {
            Title = parent.Value<string>("navigationTitle") ?? parent.Value<string>("title") ?? parent.Name,
            Url = parent.Url(),
            Children = parent.Children
                .Where(x => x.Value<bool>("showInNavigation"))
                .Select(x => GetChildrenNavItems(x, parentLevel + 1, maxLevel))
        };
    }
}