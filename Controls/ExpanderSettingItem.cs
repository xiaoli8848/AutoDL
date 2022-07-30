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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Controls
{
    [ContentProperty(Name = nameof(ExpanderSettingContent))]
    public class ExpanderSettingItem : Control
    {
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ExpanderSettingItem),
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
            typeof(ExpanderSettingItem),
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
            typeof(ExpanderSettingItem),
            new PropertyMetadata(default(string)));

        protected UIElement SettingContent
        {
            get => (UIElement)GetValue(SettingContentProperty);
            set => SetValue(SettingContentProperty, value);
        }

        protected DependencyProperty SettingContentProperty = DependencyProperty.Register(
            nameof(SettingContent),
            typeof(UIElement),
            typeof(ExpanderSettingItem),
            new PropertyMetadata(default(UIElement)));

        public UIElement ExpanderSettingContent
        {
            get => (UIElement)GetValue(ExpanderSettingContentProperty);
            set => SetValue(ExpanderSettingContentProperty, value);
        }

        public static readonly DependencyProperty ExpanderSettingContentProperty = DependencyProperty.Register(
            nameof(ExpanderSettingContent),
            typeof(UIElement),
            typeof(ExpanderSettingItem),
            new PropertyMetadata(default(UIElement)));

        public ExpanderSettingItem()
        {
            this.DefaultStyleKey = typeof(ExpanderSettingItem);
        }
    }
}
