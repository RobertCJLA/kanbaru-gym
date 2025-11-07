using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace kanbarugym.Views.Controls
{
 public partial class FloatingMenu : ContentView
 {
 public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
 nameof(IsOpen), typeof(bool), typeof(FloatingMenu), false, propertyChanged: OnIsOpenChanged);

 public static readonly BindableProperty MenuMarginProperty = BindableProperty.Create(
 nameof(MenuMargin), typeof(Thickness), typeof(FloatingMenu), new Thickness(0));

 public static readonly BindableProperty EditCommandProperty = BindableProperty.Create(
 nameof(EditCommand), typeof(ICommand), typeof(FloatingMenu));

 public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(
 nameof(DeleteCommand), typeof(ICommand), typeof(FloatingMenu));

 public static readonly BindableProperty EditTextProperty = BindableProperty.Create(
 nameof(EditText), typeof(string), typeof(FloatingMenu), "Editar");

 public static readonly BindableProperty DeleteTextProperty = BindableProperty.Create(
 nameof(DeleteText), typeof(string), typeof(FloatingMenu), "Eliminar");

 public bool IsOpen
 {
 get => (bool)GetValue(IsOpenProperty);
 set => SetValue(IsOpenProperty, value);
 }

 public Thickness MenuMargin
 {
 get => (Thickness)GetValue(MenuMarginProperty);
 set => SetValue(MenuMarginProperty, value);
 }

 public ICommand? EditCommand
 {
 get => (ICommand?)GetValue(EditCommandProperty);
 set => SetValue(EditCommandProperty, value);
 }

 public ICommand? DeleteCommand
 {
 get => (ICommand?)GetValue(DeleteCommandProperty);
 set => SetValue(DeleteCommandProperty, value);
 }

 public string EditText
 {
 get => (string)GetValue(EditTextProperty);
 set => SetValue(EditTextProperty, value);
 }

 public string DeleteText
 {
 get => (string)GetValue(DeleteTextProperty);
 set => SetValue(DeleteTextProperty, value);
 }

 public FloatingMenu()
 {
 InitializeComponent();
 IsVisible = IsOpen;
 }

 private static async void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
 {
 if (bindable is FloatingMenu fm)
 {
 bool show = (bool)newValue;
 fm.IsVisible = show;
 if (show)
 {
 fm.OutsideTapCatcher.InputTransparent = false;
 await System.Threading.Tasks.Task.WhenAll(
 fm.MenuPanel.FadeTo(1.0,150, Easing.CubicOut),
 fm.MenuPanel.ScaleTo(1.0,150, Easing.CubicOut)
 );
 }
 else
 {
 fm.OutsideTapCatcher.InputTransparent = true;
 await System.Threading.Tasks.Task.WhenAll(
 fm.MenuPanel.FadeTo(0.0,120, Easing.CubicIn),
 fm.MenuPanel.ScaleTo(0.95,120, Easing.CubicIn)
 );
 fm.IsVisible = false;
 }
 }
 }

 private void OnOutsideTapped(object? sender, EventArgs e)
 {
 IsOpen = false;
 }

 private void OnEditTapped(object? sender, EventArgs e)
 {
 if (EditCommand?.CanExecute(BindingContext) == true)
 EditCommand.Execute(BindingContext);
 IsOpen = false;
 }

 private void OnDeleteTapped(object? sender, EventArgs e)
 {
 if (DeleteCommand?.CanExecute(BindingContext) == true)
 DeleteCommand.Execute(BindingContext);
 IsOpen = false;
 }
 }
}