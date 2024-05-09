using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazingTailwind.Components;

public class MyInputText : InputText
{
    private const string BaseClass =
        "appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var attributes = AdditionalAttributes?.ToDictionary() ?? new Dictionary<string, object>();
        if (attributes.TryGetValue("class", out var existingClass))
        {
            attributes["class"] = $"{BaseClass} {existingClass}";
        }
        else
        {
            attributes.Add("class", BaseClass);
        }

        AdditionalAttributes = attributes;
        base.BuildRenderTree(builder);
    }
}