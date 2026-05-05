// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class Grid
{
    static partial void RegisterAdditionalHandlers()
    {
        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Grid.Column",
            (element, value) => MC.Grid.SetColumn(element, AttributeHelper.GetInt(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Grid.ColumnSpan",
            (element, value) => MC.Grid.SetColumnSpan(element, AttributeHelper.GetInt(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Grid.Row",
            (element, value) => MC.Grid.SetRow(element, AttributeHelper.GetInt(value)));

        AttachedPropertyRegistry.RegisterAttachedPropertyHandler("Grid.RowSpan",
            (element, value) => MC.Grid.SetRowSpan(element, AttributeHelper.GetInt(value)));
    }

    private static readonly MC.ColumnDefinitionCollectionTypeConverter ColumnDefinitionConverter = new();
    private static readonly MC.RowDefinitionCollectionTypeConverter RowDefinitionConverter = new();

    /// <summary>
    /// A comma-separated list of column definitions. A column definition can be:
    /// Auto-sized with the <c>Auto</c> keyword; A numeric size, such as <c>80.5</c>; Or a relative size, such as <c>*</c>, <c>2*</c>, or <c>3.5*</c>.
    /// </summary>
    [Parameter] public string ColumnDefinitions { get; set; }
    /// <summary>
    /// A comma-separated list of row definitions. A row definition can be:
    /// Auto-sized with the <c>Auto</c> keyword; A numeric size, such as <c>80.5</c>; Or a relative size, such as <c>*</c>, <c>2*</c>, or <c>3.5*</c>.
    /// </summary>
    [Parameter] public string RowDefinitions { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        switch (name)
        {
            case nameof(ColumnDefinitions):
                if (!Equals(ColumnDefinitions, value))
                {
                    var columnDefinitionsText = CastParameter<string>(value, name);
                    var columnDefinitions = (MC.ColumnDefinitionCollection)ColumnDefinitionConverter.ConvertFromInvariantString(columnDefinitionsText);
                    NativeControl.ColumnDefinitions = columnDefinitions;
                    ColumnDefinitions = columnDefinitionsText;
                }
                return true;
            case nameof(RowDefinitions):
                if (!Equals(RowDefinitions, value))
                {
                    var rowDefinitionsText = CastParameter<string>(value, name);
                    var rowDefinitions = (MC.RowDefinitionCollection)RowDefinitionConverter.ConvertFromInvariantString(rowDefinitionsText);
                    NativeControl.RowDefinitions = rowDefinitions;
                    RowDefinitions = rowDefinitionsText;
                }
                return true;
            default:
                return base.HandleAdditionalParameter(name, value);
        }
    }
}
