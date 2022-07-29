namespace AutoDL.Controls;

public class SettingItem : Control
{
    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(SettingItem),
        new PropertyMetadata(default(string))
        );

    public SymbolIconSource Symbol
    {
        get => (SymbolIconSource)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
        nameof(Symbol),
        typeof(SymbolIconSource),
        typeof(SettingItem),
        new PropertyMetadata(default(SymbolIconSource))
    );

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    readonly DependencyProperty LabelProperty = DependencyProperty.Register(
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