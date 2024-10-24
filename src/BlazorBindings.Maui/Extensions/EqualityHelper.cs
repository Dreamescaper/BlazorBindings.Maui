using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorBindings.Maui.Extensions;

public static class EqualityHelper
{
    // ConcurrentDictionary to store compiled field accessors for faster lookups.
    // This caches the accessors for fields (like Delegate and Receiver) across multiple calls.
    private static readonly ConcurrentDictionary<Key, Func<object, object>> _fieldAccessors = new();

    // Added to temporarily solve the issue https://github.com/dotnet/aspnetcore/issues/53361
    // This method compares an EventCallback to an object. If the object is not an EventCallback,
    // it returns false. Otherwise, it checks if they are equal using AreEqual method.
    public static bool IsEqual(EventCallback callback, object obj)
    {
        // Check if the object is an EventCallback. If not, return false.
        if (obj is not EventCallback other)
        {
            return false;
        }

        // Compare the two EventCallbacks using the AreEqual method.
        return AreEqual(callback, other);
    }

    // Overload of IsEqual for generic EventCallback<T>. It works the same way as the above method.
    public static bool IsEqual<T>(EventCallback<T> callback, object obj)
    {
        // Check if the object is an EventCallback<T>. If not, return false.
        if (obj is not EventCallback<T> other)
        {
            return false;
        }

        // Compare the two generic EventCallbacks using the AreEqual method.
        return AreEqual(callback, other);
    }

    // Compares two EventCallback objects by checking if their delegates and receivers are equal.
    private static bool AreEqual(object callback1, object callback2)
    {
        // Retrieve the delegate (method) of both callbacks.
        var delegate1 = GetDelegate(callback1);
        var delegate2 = GetDelegate(callback2);

        // Retrieve the receiver (the object the delegate is bound to) of both callbacks.
        var receiver1 = GetReceiver(callback1);
        var receiver2 = GetReceiver(callback2);

        // Compare both the receiver and the delegate. They must both be equal for the callbacks to be considered equal.
        return ReferenceEquals(receiver1, receiver2) && Equals(delegate1, delegate2);
    }

    // Retrieves the delegate (MulticastDelegate) field from the EventCallback object.
    private static MulticastDelegate GetDelegate(object callback)
    {
        // Use reflection to access the "Delegate" field of the callback object.
        return (MulticastDelegate)GetField(callback.GetType(), "Delegate")(callback);
    }

    // Creates and caches a lambda-based accessor for a given field. This avoids the performance hit of reflection on subsequent calls.
    // The lambda is compiled into a Func<object, object> to provide access to the field value.
    private static Func<object, object> GetField(Type type, string fieldName)
    {
        // Check the cache or add a new field accessor if not already cached.
        return _fieldAccessors.GetOrAdd(new(type, fieldName), static key =>
        {
            // Use reflection to find the field on the given type.
            var fieldInfo = key.Type.GetField(key.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException($"Field '{key.FieldName}' not found in type '{key.Type}'.");

            // Create an expression that represents accessing the field.
            var param = Expression.Parameter(typeof(object), "instance"); // Parameter for the instance
            var fieldAccess = Expression.Field(Expression.Convert(param, key.Type), fieldInfo); // Access the field on the casted instance
            var lambda = Expression.Lambda<Func<object, object>>(Expression.Convert(fieldAccess, typeof(object)), param); // Convert the result to object

            // Compile the expression into a delegate and return it.
            return lambda.Compile();
        });
    }

    // Retrieves the receiver (IHandleEvent) field from the EventCallback object.
    private static IHandleEvent GetReceiver(object callback)
    {
        // Use reflection to access the "Receiver" field of the callback object.
        return (IHandleEvent)GetField(callback.GetType(), "Receiver")(callback);
    }

    // Represents the unique key used to cache field accessors in the ConcurrentDictionary.
    // Each key consists of the type and the field name.
    private sealed record Key(Type Type, string FieldName);
}
