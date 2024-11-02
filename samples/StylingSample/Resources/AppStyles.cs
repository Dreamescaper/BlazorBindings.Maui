using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

namespace StylingSample.Resources;

public static class AppStyles
{
    public static ResourceDictionary Scoped { get; } = new()
    {
        new Style<Button>()
            .Add(Button.HorizontalOptionsProperty, LayoutOptions.Start)
            .Add(Button.BackgroundColorProperty, Colors.Transparent)
            .AddAppThemeBinding(Button.TextColorProperty, Colors.Black, Colors.White)
    };

    public static ResourceDictionary Default => new()
    {
        ActivityIndicatorStyle,
        IndicatorViewStyle,
        BorderStyle,
        BoxViewStyle,
        LabelStyle,
        ButtonStyle,
        PageStyle,
        NavigationPageStyle,
        ShellStyle
    };

    public static Style ActivityIndicatorStyle { get; } = new Style<ActivityIndicator>()
        .AddAppThemeBinding(ActivityIndicator.ColorProperty, Colors.Primary, Colors.White);

    public static Style IndicatorViewStyle { get; } = new Style<IndicatorView>()
        .AddAppThemeBinding(IndicatorView.IndicatorColorProperty, Colors.Gray200, Colors.Gray500)
        .AddAppThemeBinding(IndicatorView.SelectedIndicatorColorProperty, Colors.Gray950, Colors.Gray100);

    public static Style BorderStyle { get; } = new Style<Border>()
        .AddAppThemeBinding(Border.StrokeProperty, Colors.Gray200, Colors.Gray500)
        .Add(Border.StrokeShapeProperty, new Rectangle())
        .Add(Border.StrokeThicknessProperty, 1);

    public static Style BoxViewStyle { get; } = new Style<BoxView>()
        .AddAppThemeBinding(BoxView.BackgroundColorProperty, Colors.Gray950, Colors.Gray200);

    public static Style PageStyle { get; } = new Style<Page>()
        .Add(Page.PaddingProperty, 0)
        .AddAppThemeBinding(Page.BackgroundColorProperty, Colors.White, Colors.OffBlack)
        .ApplyToDerivedTypes(true);

    public static Style NavigationPageStyle { get; } = new Style<NavigationPage>()
        .AddAppThemeBinding(NavigationPage.BarTextColorProperty, Colors.Gray200, Colors.White)
        .AddAppThemeBinding(NavigationPage.BarBackgroundColorProperty, Colors.White, Colors.OffBlack);

    public static Style ShellStyle { get; } = new Style<Shell>()
        .AddAppThemeBinding(Shell.NavBarHasShadowProperty, true, true)
        .AddAppThemeBinding(Shell.TitleColorProperty, Colors.Black, Colors.SecondaryDarkText)
        .AddAppThemeBinding(Shell.DisabledColorProperty, Colors.Black, Colors.White)
        .AddAppThemeBinding(Shell.UnselectedColorProperty, Colors.Black, Colors.White)
        .AddAppThemeBinding(Shell.ForegroundColorProperty, Colors.Black, Colors.SecondaryDarkText)
        .AddAppThemeBinding(Shell.BackgroundColorProperty, Colors.White, Colors.Black);

    public static Style LabelStyle { get; } = new Style<Label>()
        .AddAppThemeBinding(Label.TextColorProperty, Colors.Black, Colors.White)
        .Add(Label.BackgroundColorProperty, Colors.Transparent)
        .Add(Label.FontSizeProperty, 14);

    public static Style ButtonStyle { get; } = new Style<Button>()
        .AddAppThemeBinding(Button.TextColorProperty, Colors.White, Colors.PrimaryDarkText)
        .AddAppThemeBinding(Button.BackgroundColorProperty, Colors.Primary, Colors.PrimaryDark)
        .Add(Button.FontSizeProperty, 14)
        .Add(Button.BorderWidthProperty, 0)
        .Add(Button.CornerRadiusProperty, 8)
        .Add(Button.PaddingProperty, new Thickness(14, 10))
        .Add(Button.MinimumHeightRequestProperty, 44)
        .Add(Button.MinimumWidthRequestProperty, 44);
}
