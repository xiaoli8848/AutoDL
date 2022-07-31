﻿using Microsoft.UI.Xaml.Markup;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AutoDL.Controls;

[ContentProperty(Name = nameof(ExpanderSettingContent))]
public class ExpanderSettingItem : Control
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(string))
    );

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(IconElement))
    );

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(string)));

    public static readonly DependencyProperty ExpanderSettingContentProperty = DependencyProperty.Register(
        nameof(ExpanderSettingContent),
        typeof(UIElement),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(UIElement)));

    protected static readonly DependencyProperty SettingContentProperty = DependencyProperty.Register(
        nameof(SettingContent),
        typeof(UIElement),
        typeof(ExpanderSettingItem),
        new PropertyMetadata(default(UIElement)));

    public ExpanderSettingItem()
    {
        DefaultStyleKey = typeof(ExpanderSettingItem);
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

    public UIElement ExpanderSettingContent
    {
        get => (UIElement)GetValue(ExpanderSettingContentProperty);
        set => SetValue(ExpanderSettingContentProperty, value);
    }
}