using BlazorBindings.Core;
using BlazorBindings.Maui.Extensions;
using System.Collections.Immutable;

namespace BlazorBindings.UnitTests.Components;

public class TestContainerComponent : NativeControlComponentBase, IElementHandler, IContainerElementHandler
{
    private TestTargetElement _targetElement = new();

    [Parameter] public int X { get; set; }
    [Parameter] public int Y { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        _targetElement.X = X;
        _targetElement.Y = Y;
    }

    public object TargetElement => _targetElement;

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        _targetElement.Children = _targetElement.Children.Insert(physicalSiblingIndex, child.Cast<TestTargetElement>());
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        if (!Equals(_targetElement.Children[physicalSiblingIndex], child))
            throw new InvalidOperationException("Unexpected child to remove.");

        _targetElement.Children = _targetElement.Children.RemoveAt(physicalSiblingIndex);
    }

    void IContainerElementHandler.ReplaceChild(int physicalSiblingIndex, object oldChild, object newChild)
    {
        if (!Equals(_targetElement.Children[physicalSiblingIndex], oldChild))
            throw new InvalidOperationException("Unexpected child to remove.");

        _targetElement.Children = _targetElement.Children.SetItem(physicalSiblingIndex, newChild.Cast<TestTargetElement>());
    }

    // We want to track element state when it is added to parent, therefore mutable struct is used.
    public record struct TestTargetElement
    {
        public TestTargetElement() : this(0, 0) { }

        public TestTargetElement(int x, int y)
        {
            X = x;
            Y = y;
            Children = ImmutableList.Create<TestTargetElement>();
        }

        public int X { get; set; }
        public int Y { get; set; }

        public ImmutableList<TestTargetElement> Children { get; set; }
    }
}
