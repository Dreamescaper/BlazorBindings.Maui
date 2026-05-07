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


    private readonly List<MC.View> _children = [];

    public override Task SetParametersAsync(ParameterView parameters)
    {
        foreach (var parameterValue in parameters)
        {
            switch (parameterValue.Name)
            {
                case nameof(Column):
                    var columnValue = CastParameter<int?>(parameterValue.Value, parameterValue.Name);
                    if (columnValue != Column)
                    {
                        Column = columnValue;
                        _children.ForEach(c => MC.Grid.SetColumn(c, Column ?? 0));
                    }
                    break;
                case nameof(Row):
                    var rowValue = CastParameter<int?>(parameterValue.Value, parameterValue.Name);
                    if (rowValue != Row)
                    {
                        Row = rowValue;
                        _children.ForEach(c => MC.Grid.SetRow(c, Row ?? 0));
                    }
                    break;
                case nameof(ColumnSpan):
                    var colSpanValue = CastParameter<int?>(parameterValue.Value, parameterValue.Name);
                    if (colSpanValue != ColumnSpan)
                    {
                        ColumnSpan = colSpanValue;
                        _children.ForEach(c => MC.Grid.SetColumnSpan(c, ColumnSpan ?? 1));
                    }
                    break;
                case nameof(RowSpan):
                    var rowSpanValue = CastParameter<int?>(parameterValue.Value, parameterValue.Name);
                    if (rowSpanValue != RowSpan)
                    {
                        RowSpan = rowSpanValue;
                        _children.ForEach(c => MC.Grid.SetRowSpan(c, RowSpan ?? 1));
                    }
                    break;
                case nameof(ChildContent):
                    {
                        ChildContent = CastParameter<RenderFragment>(parameterValue.Value, parameterValue.Name);
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

    public void RemoveChild(int physicalSiblingIndex)
    {
        _children.RemoveAt(physicalSiblingIndex);
    }

    object IElementHandler.TargetElement => null;
    void INonPhysicalChild.SetParent(object parentElement) { }
    void INonPhysicalChild.RemoveFromParent(object parentElement) { }
    bool INonPhysicalChild.ShouldAddChildrenToParent => true;
}
