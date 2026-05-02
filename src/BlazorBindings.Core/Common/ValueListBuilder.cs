// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Taken from https://source.dot.net/#System.Text.RegularExpressions/src/libraries/Common/src/System/Collections/Generic/ValueListBuilder.cs
 
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
 
namespace System.Collections.Generic;

[InlineArray(Capacity)]
internal struct InlineBuffer<T>
{
    public const int Capacity = 8;
    private T _element0;
}

internal ref partial struct ValueListBuilder<T>
{
    private T[]? _arrayFromPool;
    private int _pos;
    private InlineBuffer<T> inlineBuffer;

    private Span<T> Buffer => _arrayFromPool is not null
        ? _arrayFromPool.AsSpan()
        : MemoryMarshal.CreateSpan(ref Unsafe.As<InlineBuffer<T>, T>(ref inlineBuffer), InlineBuffer<T>.Capacity);

    public int Length
    {
        get => _pos;
        set
        {
            Debug.Assert(value >= 0);
            Debug.Assert(value <= _pos);
            _pos = value;
        }
    }

    public ref T this[int index]
    {
        get
        {
            Debug.Assert(index < _pos);
            return ref Buffer[index];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(T item)
    {
        int pos = _pos;

        // Workaround for https://github.com/dotnet/runtime/issues/72004
        Span<T> span = Buffer;
        if ((uint)pos < (uint)span.Length)
        {
            span[pos] = item;
            _pos = pos + 1;
        }
        else
        {
            AddWithResize(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(scoped ReadOnlySpan<T> source)
    {
        int pos = _pos;
        Span<T> span = Buffer;
        if (source.Length == 1 && (uint)pos < (uint)span.Length)
        {
            span[pos] = source[0];
            _pos = pos + 1;
        }
        else
        {
            AppendMultiChar(source);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AppendMultiChar(scoped ReadOnlySpan<T> source)
    {
        if ((uint)(_pos + source.Length) > (uint)Buffer.Length)
        {
            Grow(source.Length);
        }

        source.CopyTo(Buffer.Slice(_pos));
        _pos += source.Length;
    }

    public void Insert(int index, scoped ReadOnlySpan<T> source)
    {
        Debug.Assert(index == 0, "Implementation currently only supports index == 0");

        if ((uint)(_pos + source.Length) > (uint)Buffer.Length)
        {
            Grow(source.Length);
        }

        Buffer.Slice(0, _pos).CopyTo(Buffer.Slice(source.Length));
        source.CopyTo(Buffer);
        _pos += source.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AppendSpan(int length)
    {
        Debug.Assert(length >= 0);

        int pos = _pos;
        Span<T> span = Buffer;
        if ((uint)(pos + length) <= (uint)span.Length)
        {
            _pos = pos + length;
            return span.Slice(pos, length);
        }
        else
        {
            return AppendSpanWithGrow(length);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Span<T> AppendSpanWithGrow(int length)
    {
        int pos = _pos;
        Grow(length);
        _pos += length;
        return Buffer.Slice(pos, length);
    }

    // Hide uncommon path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(T item)
    {
        Debug.Assert(_pos == Buffer.Length);
        int pos = _pos;
        Grow(1);
        Buffer[pos] = item;
        _pos = pos + 1;
    }

    public ReadOnlySpan<T> AsSpan()
    {
        return Buffer.Slice(0, _pos);
    }

    public bool TryCopyTo(Span<T> destination, out int itemsWritten)
    {
        if (Buffer.Slice(0, _pos).TryCopyTo(destination))
        {
            itemsWritten = _pos;
            return true;
        }

        itemsWritten = 0;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        int pos = _pos;
        T[]? toReturn = _arrayFromPool;

        this = default;

        if (toReturn != null)
        {
            if (!typeof(T).IsPrimitive)
            {
                Array.Clear(toReturn, 0, pos);
            }

            ArrayPool<T>.Shared.Return(toReturn);
        }
    }

    /// <summary>
    /// Resize the internal buffer either by doubling current buffer size or
    /// by adding <paramref name="additionalCapacityBeyondPos"/> to
    /// <see cref="_pos"/> whichever is greater.
    /// </summary>
    /// <param name="additionalCapacityBeyondPos">
    /// Number of chars requested beyond current position.
    /// </param>
    private void Grow(int additionalCapacityBeyondPos)
    {
        Debug.Assert(additionalCapacityBeyondPos > 0);

        int currentLength = Buffer.Length;

        Debug.Assert(_pos > currentLength - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

        const int ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

        // Double the size of the buffer, starting from the inline capacity.
        int nextCapacity = Math.Max(currentLength * 2, _pos + additionalCapacityBeyondPos);

        if ((uint)nextCapacity > ArrayMaxLength)
        {
            nextCapacity = Math.Max(Math.Max(currentLength + 1, ArrayMaxLength), currentLength);
        }

        T[] array = ArrayPool<T>.Shared.Rent(nextCapacity);
        Buffer.CopyTo(array);

        T[]? toReturn = _arrayFromPool;
        _arrayFromPool = array;
        if (toReturn != null)
        {
            if (!typeof(T).IsPrimitive)
            {
                Array.Clear(toReturn, 0, _pos);
            }

            ArrayPool<T>.Shared.Return(toReturn);
        }
    }
}