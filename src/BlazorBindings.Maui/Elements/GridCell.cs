// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public class GridCell : NativeControlComponentBase, IContainerElementHandler, INonPhysicalChild
{
    [Parameter] public int? Column { get; set; }
    [Parameter] public int? ColumnSpan { get; set; }
    [Parameter] public int? Row { get; set; }
    [Parameter] public int? RowSpan { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }


    private readonly List<MC.View> _children = new();
    private MC.Grid _parentGrid;

    object IElementHandler.TargetElement => null;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameterValue in parameters)
        {
            switch (parameterValue.Name)
            {
                case nameof(Column):
                    var columnValue = (int)parameterValue.Value;
                    if (columnValue != Column)
                    {
                        Column = columnValue;
                        _children.ForEach(c => MC.Grid.SetColumn(c, Column ?? 0));
                    }
                    break;
                case nameof(Row):
                    var rowValue = (int)parameterValue.Value;
                    if (rowValue != Row)
                    {
                        Row = rowValue;
                        _children.ForEach(c => MC.Grid.SetRow(c, Row ?? 0));
                    }
                    break;
                case nameof(ColumnSpan):
                    var colSpanValue = (int)parameterValue.Value;
                    if (colSpanValue != ColumnSpan)
                    {
                        ColumnSpan = colSpanValue;
                        _children.ForEach(c => MC.Grid.SetColumnSpan(c, ColumnSpan ?? 1));
                    }
                    break;
                case nameof(RowSpan):
                    var rowSpanValue = (int)parameterValue.Value;
                    if (rowSpanValue != RowSpan)
                    {
                        RowSpan = rowSpanValue;
                        _children.ForEach(c => MC.Grid.SetRowSpan(c, RowSpan ?? 1));
                    }
                    break;
                case nameof(ChildContent):
                    {
                        ChildContent = (RenderFragment)parameterValue.Value;
                        break;
                    }
            }
        }

        return base.SetParametersAsync(ParameterView.Empty);
    }

    protected override RenderFragment GetChildContent() => ChildContent;

    public void AddChild(object child, int physicalSiblingIndex)
    {
        if (child is not MC.View childView)
        {
            throw new ArgumentException($"Expected parent to be of type {typeof(MC.View).FullName} but it is of type {child?.GetType().FullName}.", nameof(child));
        }

        MC.Grid.SetColumn(childView, Column ?? 0);
        MC.Grid.SetColumnSpan(childView, ColumnSpan ?? 1);
        MC.Grid.SetRow(childView, Row ?? 0);
        MC.Grid.SetRowSpan(childView, RowSpan ?? 1);

        _children.Add(childView);
        _parentGrid.Children.Add(childView);
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        if (child is not MC.View childView)
        {
            throw new ArgumentException($"Expected parent to be of type {typeof(MC.View).FullName} but it is of type {child?.GetType().FullName}.", nameof(child));
        }

        _children.Remove(childView);
        _parentGrid.Children.Remove(childView);
    }

    public int GetChildIndex(object child)
    {
        return child is MC.View childView
            ? _children.IndexOf(childView)
            : -1;
    }

    void INonPhysicalChild.SetParent(object parentElement)
    {
        _parentGrid = parentElement as MC.Grid
            ?? throw new ArgumentException($"Expected parent to be of type {typeof(MC.Grid).FullName} but it is of type {parentElement?.GetType().FullName}.", nameof(parentElement));
    }

    void INonPhysicalChild.RemoveFromParent(object parentElement)
    {
        if (_parentGrid != null)
        {
            foreach (var child in _children)
            {
                _parentGrid.Children.Remove(child);
            }

            _children.Clear();
            _parentGrid = null;
        }
    }
}
