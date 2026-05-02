using BlazorBindings.Core;
using BlazorBindings.Maui.Extensions;

namespace BlazorBindings.UnitTests.Components;

public class RecordingContainerComponent : NativeControlComponentBase, IElementHandler, IContainerElementHandler
{
    private readonly RecordingContainerTarget _targetElement = new();

    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    public object TargetElement => _targetElement;

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        _targetElement.Operations.Add(new("AddChild", physicalSiblingIndex, 1, [child.Cast<RecordingChildTarget>().Id]));
        _targetElement.Children.Insert(physicalSiblingIndex, child.Cast<RecordingChildTarget>());
    }

    void IContainerElementHandler.RemoveChild(int physicalSiblingIndex)
    {
        _targetElement.Operations.Add(new("RemoveChild", physicalSiblingIndex, 1, []));
        _targetElement.Children.RemoveAt(physicalSiblingIndex);
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object newChild)
    {
        _targetElement.Operations.Add(new("ReplaceChild", physicalSiblingIndex, 1, [newChild.Cast<RecordingChildTarget>().Id]));
        _targetElement.Children[physicalSiblingIndex] = newChild.Cast<RecordingChildTarget>();
    }

    void IContainerElementHandler.AddRange(int index, IReadOnlyList<object> children)
    {
        _targetElement.Operations.Add(new("AddRange", index, children.Count, children.Select(child => child.Cast<RecordingChildTarget>().Id).ToArray()));

        for (var i = 0; i < children.Count; i++)
            _targetElement.Children.Insert(index + i, children[i].Cast<RecordingChildTarget>());
    }

    void IContainerElementHandler.RemoveRange(int index, int count)
    {
        _targetElement.Operations.Add(new("RemoveRange", index, count, []));

        for (var i = 0; i < count; i++)
            _targetElement.Children.RemoveAt(index);
    }

    void IContainerElementHandler.ReplaceRange(int index, int count, IReadOnlyList<object> newChildren)
    {
        _targetElement.Operations.Add(new("ReplaceRange", index, count, newChildren.Select(child => child.Cast<RecordingChildTarget>().Id).ToArray()));

        var replacementsCount = Math.Min(count, newChildren.Count);

        for (var i = 0; i < replacementsCount; i++)
            _targetElement.Children[index + i] = newChildren[i].Cast<RecordingChildTarget>();

        for (var i = replacementsCount; i < count; i++)
            _targetElement.Children.RemoveAt(index + replacementsCount);

        for (var i = replacementsCount; i < newChildren.Count; i++)
            _targetElement.Children.Insert(index + i, newChildren[i].Cast<RecordingChildTarget>());
    }

    public class RecordingContainerTarget
    {
        public List<RecordingChildTarget> Children { get; } = [];
        public List<RecordingOperation> Operations { get; } = [];
    }

    public record RecordingOperation(string Name, int Index, int Count, int[] NewChildIds);
}

public class RecordingChildComponent : NativeControlComponentBase, IElementHandler
{
    private readonly RecordingChildTarget _targetElement = new();

    [Parameter] public int Id { get; set; }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _targetElement.Id = Id;
    }

    public object TargetElement => _targetElement;
}

public class AlternateRecordingChildComponent : RecordingChildComponent;

public class RecordingChildTarget
{
    public int Id { get; set; }
}
