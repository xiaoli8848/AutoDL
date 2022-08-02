namespace AutoDL.Controls;

public class SwitcherSettingItem : SettingItem
{
    public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
        nameof(IsOn),
        typeof(bool),
        typeof(SwitcherSettingItem),
        new PropertyMetadata(false));

    public SwitcherSettingItem()
    {
        var switcher = new ToggleSwitch();
        var binding = new Binding
        {
            Source = this,
            Path = new PropertyPath("IsOn"),
            Mode = BindingMode.TwoWay
        };
        switcher.SetBinding(ToggleSwitch.IsOnProperty, binding);
        
        SettingContent = switcher;
    }

    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }
}