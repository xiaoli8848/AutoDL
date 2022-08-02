using Microsoft.UI.Xaml.Markup;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Controls;

[ContentProperty(Name = nameof(ExpanderSettingContent))]
public class ExpanderSettingItem : SettingItem
{
    public static readonly DependencyProperty ExpanderSettingContentProperty = DependencyProperty.Register(
        nameof(ExpanderSettingContent),
        typeof(UIElement),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(UIElement)));

    public ExpanderSettingItem()
    {
        DefaultStyleKey = typeof(ExpanderSettingItem);
    }

    public UIElement ExpanderSettingContent
    {
        get => (UIElement)GetValue(ExpanderSettingContentProperty);
        set => SetValue(ExpanderSettingContentProperty, value);
    }
}