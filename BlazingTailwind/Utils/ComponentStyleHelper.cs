namespace BlazingTailwind.Utils;

public static class ComponentStyleHelper
{
    public static Dictionary<string, object> ApplyBaseClass(string baseClass, IReadOnlyDictionary<string, object>? originalAttributes)
    {
        var attributes = new Dictionary<string, object>(originalAttributes ?? new Dictionary<string, object>());
        if (attributes.TryGetValue("class", out var existingClass))
        {
            attributes["class"] = $"{baseClass} {existingClass}";
        }
        else
        {
            attributes.Add("class", baseClass);
        }

        return attributes;
    }
}