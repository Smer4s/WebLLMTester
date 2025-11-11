namespace Uply.Domain.Common.Exstensions;

public static class DictionaryExtension
{
    public static T? TryGetValue<T>(this Dictionary<string, object> parameters, string key) =>
        parameters.TryGetValue(key, out var value) ? (T?)value : default;
}
