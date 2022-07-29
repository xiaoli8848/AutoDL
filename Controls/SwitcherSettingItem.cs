using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDL.Controls
{
    public class SwitcherSettingItem : SettingItem
    {
        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
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
            this.SettingContent = switcher;
        }
    }
}
