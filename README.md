# Reusable Blazor Components

I made this project to test out how create reusable Blazor components in a way that give you the same experience as using the underlying component with full support for intellisense.

For simple html attributes then you can use [attribute splatting](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/splat-attributes-and-arbitrary-parameters?view=aspnetcore-8.0). Unfortunately this does not work on directives like `@bind-value`. If you manually take care of all the directives then you can splat the rest, but I didn't want to do that just to perhaps style something (simpler scenarios).

As a test-case I used the `InputText` component and used something simple like styling. This way I can easily verify that passing `@bind-value` works and I can override the attribute from the caller.

## The current working solution
1. Inherit the component you wish to wrap
2. Override `BuildRenderTree` and modify the class attribute.

This was the first version that seems to work fine. There might be issues or limits with this that I haven't seen yet. There might be other ways of doing it as well that I haven't discovered. If you go this route I would definitely extract this to a utility class or an extension method.

```C#
public class MyCustomComponent : OriginalComponent
{
    private const string BaseClass = "your-classes-here";

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
```

## React equivalent
If you are familiar with React, this is basically what I'm trying to achieve but in Blazor and C#.

```JavaScript
import { FC, InputHTMLAttributes } from "react";

export const CustomInput: FC<InputHTMLAttributes<HTMLInputElement>> = (className, ...props) => {
    const baseClass = "custom-input";
    const combinedClasses = `${baseClass} ${className || ""}`;

    return <input className={combinedClasses} {...props} />;
};
```