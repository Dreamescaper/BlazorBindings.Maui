namespace BlazorBindings.Maui;

public interface INavigation
{
    /// <summary>
    /// Performs URI-based Shell navigation. This method is only available for Shell-based applications.
    /// </summary>
    /// <param name="uri">URI to navigate to.</param>
    /// <param name="parameters">Additional parameters to set for component.</param>
    Task NavigateToAsync(string uri, Dictionary<string, object> parameters = null);

    Task PopAsync(bool animated = true);

    Task PopModalAsync(bool animated = true);

    /// <summary>
    /// Push page component from the <paramref name="renderFragment"/> to the Navigation Stack.
    /// </summary>
    /// <remarks>Experimental API, subject to change.</remarks>
    Task PushAsync(RenderFragment renderFragment, bool animated = true);

    /// <summary>
    /// Push page component <typeparamref name="T"/> to the Navigation Stack.
    /// </summary>
    Task PushAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        Dictionary<string, object> arguments = null, bool animated = true)
        where T : IComponent;

    /// <summary>
    /// Push page component from the <paramref name="renderFragment"/> to the Modal Stack.
    /// </summary>
    /// <remarks>Experimental API, subject to change.</remarks>
    Task PushModalAsync(RenderFragment renderFragment, bool animated = true);

    /// <summary>
    /// Push page component <typeparamref name="T"/> to the Modal Stack.
    /// </summary>
    Task PushModalAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(
        Dictionary<string, object> arguments = null, bool animated = true)
        where T : IComponent;
}