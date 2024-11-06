using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

internal class NavigationHandler(MC.INavigation navigation, NavigationTarget target, bool animated) : IContainerElementHandler
{
    private readonly TaskCompletionSource _taskCompletionSource = new();
    private MC.Page _currentPage;
    private bool _firstAdd = true;

    public Task WaitForNavigation() => _taskCompletionSource.Task;
    public event Action PageClosed;

    public async Task AddChildAsync(MC.Page child)
    {
        _currentPage = child;

        if (target == NavigationTarget.Modal)
        {
            await navigation.PushModalAsync(child, _firstAdd && animated);
        }
        else
        {
            await navigation.PushAsync(child, _firstAdd && animated);
        }

        _taskCompletionSource.TrySetResult();
        _firstAdd = false;

        child.ParentChanged += ParentChanged;
    }

    public async Task RemoveChildAsync(MC.Page child)
    {
        child.ParentChanged -= ParentChanged;
        if (target == NavigationTarget.Modal)
        {
            if (navigation.ModalStack.LastOrDefault() == child)
            {
                await navigation.PopModalAsync(animated: false);
            }
        }
        else
        {
            if (navigation.NavigationStack.Contains(child))
                navigation.RemovePage(child);
        }
    }

    private void ParentChanged(object sender, EventArgs e)
    {
        var page = sender as MC.Page;

        if (page == _currentPage && page.Parent == null)
        {
            // Notify that the page is closed so that rootComponent could be removed from Blazor tree.
            PageClosed?.Invoke();
        }

        page.ParentChanged -= ParentChanged;
    }

    public async void RemoveChild(object child, int physicalSiblingIndex)
    {
        await RemoveChildAsync((MC.Page)child);
    }

    public async void AddChild(object child, int physicalSiblingIndex)
    {
        await AddChildAsync((MC.Page)child);
    }

    public object TargetElement => null;
}
