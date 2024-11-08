using BlazorBindings.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Hosting;
using System.Runtime.ExceptionServices;
using System.Threading;
using MauiDispatching = Microsoft.Maui.Dispatching;

namespace BlazorBindings.UnitTests;

static class TestApplication
{
    class ApplicationFromTestAssembly : MC.Application;

    public static MC.Application Create() => Create<ApplicationFromTestAssembly>();

    public static T Create<T>() where T : MC.Application, new()
    {
        var mauiApp = TestServiceProvider
            .CreateMauiAppBuilder()
            .UseMauiApp<T>()
            .Build();

        var application = mauiApp.Services.GetRequiredService<IApplication>();

        application.Handler = new TestHandler
        {
            MauiContext = new MauiContext(mauiApp.Services),
            VirtualView = application
        };

        DependencyService.RegisterSingleton(new TestSystemResources());

        return (T)application;
    }

    class TestHandler : IElementHandler
    {
        public object PlatformView => null;
        public IElement VirtualView { get; set; }
        public IMauiContext MauiContext { get; set; }
        public void DisconnectHandler() { }
        public void Invoke(string command, object args = null) { }
        public void SetMauiContext(IMauiContext mauiContext) => MauiContext = mauiContext;
        public void SetVirtualView(IElement view) => VirtualView = view;
        public void UpdateValue(string property) { }
    }
}

#pragma warning disable CS0612 // Type or member is obsolete. Unfortunately, I need to register this, otherwise some tests fail.
class TestSystemResources : ISystemResourcesProvider
#pragma warning restore CS0612 // Type or member is obsolete
{
    public IResourceDictionary GetSystemResources() => new ResourceDictionary();
}

public static class TestServiceProvider
{
    public static MauiAppBuilder CreateMauiAppBuilder()
    {
        var builder = MauiApp.CreateBuilder();

        // Added this as a workaround to https://github.com/dotnet/maui/issues/24216
        DependencyService.RegisterSingleton(new TestSystemResources());

        builder.UseMauiBlazorBindings();
        builder.Services.AddSingleton<MauiBlazorBindingsRenderer, TestBlazorBindingsRenderer>();
        builder.Services.AddSingleton<MauiDispatching.IDispatcher, TestDispatcher>();
        return builder;
    }

    class TestDispatcher : MauiDispatching.IDispatcher
    {
        public bool IsDispatchRequired => false;
        public MauiDispatching.IDispatcherTimer CreateTimer() => null;
        public bool Dispatch(Action action)
        {
            action();
            return true;
        }

        public bool DispatchDelayed(TimeSpan delay, Action action)
        {
            Thread.Sleep(delay);
            action();
            return true;
        }
    }
}

internal class TestBlazorBindingsRenderer
    (MauiBlazorBindingsServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    : MauiBlazorBindingsRenderer(serviceProvider, loggerFactory)
{
    public bool ThrowExceptions { get; set; } = true;

    public List<Exception> Exceptions { get; } = [];

    protected override void HandleException(Exception exception)
    {
        Exceptions.Add(exception);

        if (ThrowExceptions)
            ExceptionDispatchInfo.Throw(exception);
    }

    public override Dispatcher Dispatcher => NullDispatcher.Instance;

    sealed class NullDispatcher : Dispatcher
    {
        public static readonly Dispatcher Instance = new NullDispatcher();

        private NullDispatcher()
        {
        }

        public override bool CheckAccess() => true;

        public override Task InvokeAsync(Action workItem)
        {
            workItem();
            return Task.CompletedTask;
        }

        public override Task InvokeAsync(Func<Task> workItem)
        {
            return workItem();
        }

        public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
        {
            return Task.FromResult(workItem());
        }

        public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
        {
            return workItem();
        }
    }
}
