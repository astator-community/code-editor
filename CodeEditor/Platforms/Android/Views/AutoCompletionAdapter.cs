using Android.Widget;

namespace CodeEditor.Views.Droid;
public class AutoCompletionAdapter : ArrayAdapter
{
    private static readonly Context appContext = Android.App.Application.Context;
    private static readonly int completionItemViewId = appContext.Resources.GetIdentifier("completion_item_view", "layout", appContext.PackageName);
    private static readonly int completionItemImageViewId = appContext.Resources.GetIdentifier("completion_image", "id", appContext.PackageName);
    private static readonly int completionItemTextViewId = appContext.Resources.GetIdentifier("completion_text", "id", appContext.PackageName);
    private static readonly int completionDescViewId = appContext.Resources.GetIdentifier("completion_desc", "id", appContext.PackageName);
    private static readonly int namespaceImageId = appContext.Resources.GetIdentifier("namespace", "drawable", appContext.PackageName);
    private static readonly int classPublicImageId = appContext.Resources.GetIdentifier("class_public", "drawable", appContext.PackageName);
    private static readonly int classPrivateImageId = appContext.Resources.GetIdentifier("class_private", "drawable", appContext.PackageName);
    private static readonly int structPublicImageId = appContext.Resources.GetIdentifier("struct_public", "drawable", appContext.PackageName);
    private static readonly int structPrivateImageId = appContext.Resources.GetIdentifier("struct_private", "drawable", appContext.PackageName);
    private static readonly int interfacePublicImageId = appContext.Resources.GetIdentifier("interface_public", "drawable", appContext.PackageName);
    private static readonly int interfacePrivateImageId = appContext.Resources.GetIdentifier("interface_private", "drawable", appContext.PackageName);
    private static readonly int delegatePublicImageId = appContext.Resources.GetIdentifier("delegate_public", "drawable", appContext.PackageName);
    private static readonly int delegatePrivateImageId = appContext.Resources.GetIdentifier("delegate_private", "drawable", appContext.PackageName);
    private static readonly int enumPublicImageId = appContext.Resources.GetIdentifier("enum_public", "drawable", appContext.PackageName);
    private static readonly int enumPrivateImageId = appContext.Resources.GetIdentifier("enum_private", "drawable", appContext.PackageName);
    private static readonly int localVariableImageId = appContext.Resources.GetIdentifier("local_variable", "drawable", appContext.PackageName);
    private static readonly int propertyPublicImageId = appContext.Resources.GetIdentifier("property_public", "drawable", appContext.PackageName);
    private static readonly int propertyPrivateImageId = appContext.Resources.GetIdentifier("property_private", "drawable", appContext.PackageName);
    private static readonly int fieldPublicImageId = appContext.Resources.GetIdentifier("field_public", "drawable", appContext.PackageName);
    private static readonly int fieldPrivateImageId = appContext.Resources.GetIdentifier("field_private", "drawable", appContext.PackageName);
    private static readonly int methodPublicImageId = appContext.Resources.GetIdentifier("method_public", "drawable", appContext.PackageName);
    private static readonly int methodPrivateImageId = appContext.Resources.GetIdentifier("method_private", "drawable", appContext.PackageName);
    private static readonly int extensionMethodImageId = appContext.Resources.GetIdentifier("extension_method", "drawable", appContext.PackageName);
    private static readonly int constPublicImageId = appContext.Resources.GetIdentifier("const_public", "drawable", appContext.PackageName);
    private static readonly int constPrivateImageId = appContext.Resources.GetIdentifier("const_private", "drawable", appContext.PackageName);
    private static readonly int keywordImageId = appContext.Resources.GetIdentifier("keyword", "drawable", appContext.PackageName);

    private static int GetTagImageResourceId(CompletionItem item)
    {
        var symbolKind = item.Tags[0];
        var isPrivate = item.Tags.Length > 1 && item.Tags[1] is "Private";
        return symbolKind switch
        {
            "Namespace" => namespaceImageId,
            "Class" => !isPrivate ? classPublicImageId : classPrivateImageId,
            "Structure" => !isPrivate ? structPublicImageId : structPrivateImageId,
            "Interface" => !isPrivate ? interfacePublicImageId : interfacePrivateImageId,
            "Delegate" => !isPrivate ? delegatePublicImageId : delegatePrivateImageId,
            "Enum" => !isPrivate ? enumPublicImageId : enumPrivateImageId,
            "Local" or "Parameter" => localVariableImageId,
            "Property" => !isPrivate ? propertyPublicImageId : propertyPrivateImageId,
            "Field" => !isPrivate ? fieldPublicImageId : fieldPrivateImageId,
            "Method" => !isPrivate ? methodPublicImageId : methodPrivateImageId,
            "ExtensionMethod" => extensionMethodImageId,
            "Constant" => !isPrivate ? constPublicImageId : constPrivateImageId,
            "Keyword" => keywordImageId,
            _ => keywordImageId,
        };
    }


    private readonly List<CompletionItem> items;
    private readonly string[] descriptions;
    private readonly Color textColor;
    private readonly Func<CompletionItem, string> getDescriptionCallback;

    public AutoCompletionAdapter(Context context, List<CompletionItem> items, Color textColor, Func<CompletionItem, string> getDescriptionAction) : base(context, 0, items)
    {
        this.items = items;
        this.descriptions = new string[items.Count];
        this.textColor = textColor;
        this.getDescriptionCallback = getDescriptionAction;
    }

    public new CompletionItem GetItem(int position)
    {
        return this.items[position];
    }

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        var item = this.items[position];
        convertView ??= LayoutInflater.From(this.Context).Inflate(completionItemViewId, parent, false);

        var imgView = convertView.FindViewById<ImageView>(completionItemImageViewId);
        var textView = convertView.FindViewById<TextView>(completionItemTextViewId);
        var descView = convertView.FindViewById<TextView>(completionDescViewId);

        imgView.SetImageResource(GetTagImageResourceId(item));
        textView.Text = item.DisplayText;
        textView.SetTextColor(this.textColor);
        this.descriptions[position] ??= this.getDescriptionCallback.Invoke(this.items[position]);
        descView.Text = string.IsNullOrEmpty(this.descriptions[position]) ? "..." : this.descriptions[position];
        descView.SetTextColor(this.textColor);

        return convertView;
    }
}
