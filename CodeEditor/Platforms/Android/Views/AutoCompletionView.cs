using Android.Graphics.Drawables;
using Android.Widget;
using static Android.Widget.AdapterView;

namespace CodeEditor.Views.Droid;

internal class OnItemClickListener : Java.Lang.Object, IOnItemClickListener
{
    private readonly Action<int> callback;
    public OnItemClickListener(Action<int> callback)
    {
        this.callback = callback;
    }

    public void OnItemClick(AdapterView parent, View view, int position, long id)
    {
        this.callback?.Invoke(position);
    }
}

public class AutoCompletionWindow : ListPopupWindow
{
    private readonly Context context;
    private AutoCompletionAdapter adapter;
    private readonly int maxHeight = 195;
    private readonly Action onShowed;
    public AutoCompletionWindow(Context context, Action onShowed, Action<int> onItemClicked) : base(context)
    {
        this.context = context;
        this.onShowed = onShowed;
        this.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
        this.SetOnItemClickListener(new OnItemClickListener(onItemClicked));
        this.SetDropDownGravity(GravityFlags.Start);
    }

    public override void Show()
    {
        this.VerticalOffset = -this.Height;
        base.Show();
        this.onShowed.Invoke();
    }

    public void SetAdapter(List<CompletionItem> items, Color textColor, Func<CompletionItem, string> getDescriptionCallback)
    {
        this.adapter = new AutoCompletionAdapter(this.context, items, textColor, getDescriptionCallback);
        base.SetAdapter(this.adapter);
        this.Width = this.AnchorView.Width;
        var height = items.Count * 65;
        this.Height = Util.Dp2Px(Math.Min(height, this.maxHeight));
    }

    public AutoCompletionAdapter GetAdapter()
    {
        return this.adapter;
    }

    internal void SetOnItemClickListener(Action<int> callback)
    {
        base.SetOnItemClickListener(new OnItemClickListener(callback));
    }

    public CompletionItem GetItem(int position)
    {
        return this.adapter.GetItem(position);
    }

    private void SetOutsideTouchable(bool value)
    {
        var field = Java.Lang.Class.FromType(typeof(ListPopupWindow)).GetDeclaredField("mPopup");
        field.Accessible = true;
        var popup = (PopupWindow)field.Get(this);
        popup.OutsideTouchable = value;
    }
}
