// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BlazorBindings.Maui.Extensions;
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
        var childView = child.Cast<MC.View>();

        MC.Grid.SetColumn(childView, Column ?? 0);
        MC.Grid.SetColumnSpan(childView, ColumnSpan ?? 1);
        MC.Grid.SetRow(childView, Row ?? 0);
        MC.Grid.SetRowSpan(childView, RowSpan ?? 1);

        _children.Add(childView);
    }

    public void RemoveChild(object child, int physicalSiblingIndex)
    {
        var childView = child.Cast<MC.View>();
        _children.Remove(childView);
    }

    object IElementHandler.TargetElement => null;
    void INonPhysicalChild.SetParent(object parentElement) { }
    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    bool INonPhysicalChild.ShouldAddChildrenToParent => true;
}
