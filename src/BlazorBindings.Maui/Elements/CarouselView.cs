using MC = Microsoft.Maui.Controls;

namespace BlazorBindings.Maui.Elements;

public partial class CarouselView<T> : IHandleAfterRender
{
    private MC.IndicatorView _indicatorView;

    [Parameter] public Func<IndicatorView> IndicatorView { get; set; }

    protected override bool HandleAdditionalParameter(string name, object value)
    {
        if (name == nameof(IndicatorView))
        {
            IndicatorView = CastParameter<Func<IndicatorView>>(value, name);
            return true;
        }

        return base.HandleAdditionalParameter(name, value);
    }

    Task IHandleAfterRender.OnAfterRenderAsync()
    {
        if (IndicatorView != null)
        {
            var newIndicatorView = IndicatorView()?.NativeControl;
            if (!Equals(newIndicatorView, _indicatorView))
            {
                _indicatorView = newIndicatorView;
                NativeControl.IndicatorView = newIndicatorView;
            }
        }
        return Task.CompletedTask;
    }
}
