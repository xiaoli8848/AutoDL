using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml.Markup;

namespace AutoDL.Controls
{
    public sealed class SwitcherExpanderSettingItem : ExpanderSettingItem
    {
        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }

        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
            nameof(IsOn),
            typeof(bool),
            typeof(SwitcherExpanderSettingItem),
            new PropertyMetadata(false));

        public SwitcherExpanderSettingItem()
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
