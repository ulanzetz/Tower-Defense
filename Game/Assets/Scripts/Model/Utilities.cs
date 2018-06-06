using System;
using System.Collections.Generic;

public static class Utilities
{
    /// <summary>
    /// Возвращает числа в границах с опреленным шагом
    /// </summary>
    public static IEnumerable<float> Range(float a, float b, float step)
    {
        for (float i = Math.Min(a, b); i <= Math.Max(a, b); i += step)
            yield return i;
    }

    /// <summary>
    /// Применяет функцию для каждого элемента последовательности
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
            action(element);
    }
}