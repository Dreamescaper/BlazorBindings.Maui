using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Reflection;

namespace BlazorBindings.UnitTests.Extensions;

public static class ControlsExtensions
{
    public static T GetTemplateContent<T>(this TemplatedView view) where T : View
    {
        var child = ((IVisualTreeElement)view).GetVisualChildren()[0];
        return (T)((ContentView)child).Content;
    }

    public static T GetTemplateContent<T>(this object templateRoot) where T : View
    {
        return (T)((ContentView)templateRoot).Content;
    }

    public static void RaiseEvent<T>(this BindableObject bindableObject, string eventName, T args)
    {
        var declaringType = bindableObject.GetType().GetEvent(eventName).DeclaringType;
        var backingField = declaringType.GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        var delegateInstance = (EventHandler<T>)backingField.GetValue(bindableObject);
        delegateInstance?.Invoke(null, args);
    }
}
