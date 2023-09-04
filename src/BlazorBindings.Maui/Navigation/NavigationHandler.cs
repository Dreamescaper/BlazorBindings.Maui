using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui;

internal class NavigationHandler : IContainerElementHandler
{
    private readonly NavigationTarget _target;
    private readonly MC.INavigation _navigation;
    private readonly bool _animated;
    private readonly TaskCompletionSource _taskCompletionSource = new();
    private MC.Page _currentPage;
    private bool _firstAdd = true;

    public NavigationHandler(MC.INavigation navigation, NavigationTarget target, bool animated)
    {
        _target = target;
        _navigation = navigation;
        _animated = animated;
    }

    public Task WaitForNavigation() => _taskCompletionSource.Task;
    public event Action PageClosed;

    public async Task AddChildAsync(MC.Page child)
    {
        // The order of AddChild and RemoveChild is undetermined. We need to make sure that the previous page is removed.
        if (_currentPage != null)
            await RemoveChildAsync(_currentPage);

        _currentPage = child;

        if (_target == NavigationTarget.Modal)
        {
            await _navigation.PushModalAsync(child, _firstAdd && _animated);
        }
        else
        {
            await _navigation.PushAsync(child, _firstAdd && _animated);
        }

        _taskCompletionSource.TrySetResult();
        _firstAdd = false;

        child.ParentChanged += ParentChanged;
    }

    public async Task RemoveChildAsync(MC.Page child)
    {
        child.ParentChanged -= ParentChanged;
        if (_target == NavigationTarget.Modal)
        {
            if (_navigation.ModalStack.LastOrDefault() == child)
            {
                await _navigation.PopModalAsync(animated: false);
            }
        }
        else
        {
            if (_navigation.NavigationStack.Contains(child))
                _navigation.RemovePage(child);
        }
    }

    private void ParentChanged(object sender, EventArgs e)
    {
        var page = sender as MC.Page;

        if (page == _currentPage && page.Parent == null)
        {
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
