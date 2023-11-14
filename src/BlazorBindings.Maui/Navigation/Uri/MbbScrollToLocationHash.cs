using Microsoft.AspNetCore.Components.Routing;

namespace BlazorBindings.Maui.UriNavigation;

internal class MbbScrollToLocationHash : IScrollToLocationHash
{
    public Task RefreshScrollPositionForHash(string locationAbsolute) => Task.CompletedTask;
}
