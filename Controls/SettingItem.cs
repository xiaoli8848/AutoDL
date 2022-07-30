namespace AutoDL.Controls;

public class SettingItem : Control
{
    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(default(string))
        );

    public IconElement Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(SettingItem),
        new PropertyMetadata(default(IconElement))
    );

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(default(string)));

    protected UIElement SettingContent
    {
        get => (UIElement)GetValue(SettingContentProperty);
        set => SetValue(SettingContentProperty, value);
    }

    protected DependencyProperty SettingContentProperty = DependencyProperty.Register(
        nameof(SettingContent),
        typeof(UIElement),
        typeof(SettingItem),
        new PropertyMetadata(default(UIElement)));

    public SettingItem()
    {
        DefaultStyleKey = typeof(SettingItem);
    }
}