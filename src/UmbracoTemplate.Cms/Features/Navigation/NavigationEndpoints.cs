using UmbracoTemplate.Cms.Features.Navigation.GetNavigation;

namespace UmbracoTemplate.Cms.Features.Navigation;

public static class NavigationEndpoints
{
    public static RouteGroupBuilder MapNavigationEndpoints(this RouteGroupBuilder groupBuilder)
    {

        var navGroup = groupBuilder.MapGroup("navigation");
            
        navGroup.MapGet("", GetNavigationEndpoint.GetNavigation).WithTags("Navigation");
        
        return groupBuilder;
    }
}