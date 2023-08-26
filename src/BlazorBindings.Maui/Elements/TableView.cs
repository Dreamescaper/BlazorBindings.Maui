using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class TableView : IContainerElementHandler
{
    /// <summary>
    /// Gets or sets the root of the table.
    /// </summary>
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override RenderFragment GetChildContent() => ChildContent;

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(ChildContent))
        {
            ChildContent = (RenderFragment)value;
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    void IContainerElementHandler.AddChild(object child, int physicalSiblingIndex)
    {
        switch (child)
        {
            case MC.TableRoot root:
                NativeControl.Root = root;
                break;
            case MC.TableSection section:
                (NativeControl.Root ??= new()).Insert(0, section);
                break;
            default:
                throw new ArgumentException($"TableView does not allow {child?.GetType().Name} as a child, it only allows TableRoot or TableSection.", nameof(child));
        }
    }

    void IContainerElementHandler.RemoveChild(object child, int physicalSiblingIndex)
    {
        if (child is MC.TableRoot)
        {
            NativeControl.Root = null;
            return;
        }

        if (child is MC.TableSection section)
        {
            NativeControl.Root?.Remove(section);
            return;
        }
    }
}
