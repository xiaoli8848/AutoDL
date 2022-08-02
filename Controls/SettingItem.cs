namespace AutoDL.Controls;

public class SettingItem : Control
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(default(string))
    );

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(SettingItem),
        new PropertyMetadata(default(IconElement))
    );

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(default(string)));

    protected static readonly DependencyProperty SettingContentProperty = DependencyProperty.Register(
        nameof(SettingContent),
        typeof(UIElement),
        typeof(SettingItem),
        new PropertyMetadata(default(UIElement)));

    public SettingItem()
    {
        DefaultStyleKey = typeof(SettingItem);
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public IconElement Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    protected UIElement SettingContent
    {
        get => (UIElement)GetValue(SettingContentProperty);
        set => SetValue(SettingContentProperty, value);
    }
}