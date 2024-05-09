# Reusable Blazor Components

I made this project to test out how create reusable Blazor components in a way that give you the same experience as using the underlying component with full support for intellisense.

For simple html attributes then you can use [attribute splatting](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/splat-attributes-and-arbitrary-parameters?view=aspnetcore-8.0). Unfortunately this does not work on directives like `@bind-value`. If you manually take care of all the directives then you can splat the rest, but I didn't want to do that just to perhaps style something (simpler scenarios).

As a test-case I used the `InputText` component and used something simple like styling. This way I can easily verify that passing `@bind-value` works and I can override the attribute from the caller.

## The current working solution
1. Inherit the component you wish to wrap
2. Override `BuildRenderTree` and modify the class attribute.

This was the first version that seems to work fine. There might be issues or limits with this that I haven't seen yet. There might be other ways of doing it as well that I haven't discovered.

### Usage
```C#
public class MyInputText : InputText
{
    private const string BaseClass = "my-base-classes";

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        AdditionalAttributes = ComponentStyleHelper.ApplyBaseClass(BaseClass, AdditionalAttributes);
        base.BuildRenderTree(builder);
    }
}
```

This allows to create easily styled components that act like the original component and also use directives with few lines of code. This is made simple by creating this utility function that you can reuse.

```C#
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