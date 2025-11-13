using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

#if ANDROID
using AView = Android.Views.View;
#elif IOS || MACCATALYST
using UIKit;
using CoreGraphics;
#elif WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#endif

namespace kanbarugym.Views.Controls
{
 public partial class FloatingMenu : ContentView
 {
 public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
 nameof(IsOpen), typeof(bool), typeof(FloatingMenu), false, propertyChanged: OnIsOpenChanged);

 public static readonly BindableProperty EditCommandProperty = BindableProperty.Create(
 nameof(EditCommand), typeof(ICommand), typeof(FloatingMenu));

 public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(
 nameof(DeleteCommand), typeof(ICommand), typeof(FloatingMenu));

 public static readonly BindableProperty EditTextProperty = BindableProperty.Create(
 nameof(EditText), typeof(string), typeof(FloatingMenu), "Editar");

 public static readonly BindableProperty DeleteTextProperty = BindableProperty.Create(
 nameof(DeleteText), typeof(string), typeof(FloatingMenu), "Eliminar");

 private VisualElement? _anchor;
 private VisualElement? _root;
 private ItemsView? _itemsView;
 private double _verticalOffset;

 public bool IsOpen
 {
 get => (bool)GetValue(IsOpenProperty);
 set => SetValue(IsOpenProperty, value);
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

 public void ShowFor(VisualElement anchor, VisualElement coordinateRoot, ItemsView? scroller = null)
 {
 Detach();
 _anchor = anchor;
 _root = coordinateRoot;
 _itemsView = scroller;
 _verticalOffset =0;

 if (_root is not null)
 _root.SizeChanged += OnRootSizeChanged;
 if (_anchor is not null)
 _anchor.SizeChanged += OnAnchorSizeChanged;
 if (_itemsView is not null)
 _itemsView.Scrolled += OnItemsViewScrolled;

 Reposition();
 IsOpen = true;
 }

 public void Hide()
 {
 IsOpen = false;
 }

 private void OnItemsViewScrolled(object? sender, ItemsViewScrolledEventArgs e)
 {
 _verticalOffset = e.VerticalOffset;
 Reposition();
 }
 private void OnRootSizeChanged(object? sender, EventArgs e) => Reposition();
 private void OnAnchorSizeChanged(object? sender, EventArgs e) => Reposition();

 private void Detach()
 {
 if (_root is not null)
 _root.SizeChanged -= OnRootSizeChanged;
 if (_anchor is not null)
 _anchor.SizeChanged -= OnAnchorSizeChanged;
 if (_itemsView is not null)
 _itemsView.Scrolled -= OnItemsViewScrolled;
 _root = null; _anchor = null; _itemsView = null; _verticalOffset =0;
 }

 private void Reposition()
 {
 if (_anchor is null || _root is null)
 return;

 var rect = GetBoundsRelativeToRoot(_anchor, _root);
 const double gap =4;
 double x = rect.X - gap; // anchor left X, we will align menu AnchorX=1 (top-right) after translate
 double y = rect.Y + rect.Height + gap - _verticalOffset;

 // Translate the menu so its top-right sits at (x, y)
 double targetX = x - MenuPanel.Width; // because AnchorX=1, top-right corner is at local X=Width
 double targetY = y; // AnchorY=0 => top edge
 MenuPanel.TranslationX = targetX;
 MenuPanel.TranslationY = targetY;
 }

 private static Rect GetBoundsRelativeToRoot(VisualElement element, VisualElement root)
 {
#if ANDROID
 var v = element.Handler?.PlatformView as AView;
 var r = root.Handler?.PlatformView as AView;
 if (v is not null && r is not null)
 {
 var locA = new int[2];
 var locR = new int[2];
 v.GetLocationOnScreen(locA);
 r.GetLocationOnScreen(locR);
 double x = locA[0] - locR[0];
 double y = locA[1] - locR[1];
 return new Rect(x, y, element.Width, element.Height);
 }
#elif IOS || MACCATALYST
 var v = element.Handler?.PlatformView as UIView;
 var r = root.Handler?.PlatformView as UIView;
 if (v is not null && r is not null)
 {
 var p = v.ConvertPointToView(CGPoint.Empty, r);
 return new Rect(p.X, p.Y, element.Width, element.Height);
 }
#elif WINDOWS
 var v = element.Handler?.PlatformView as FrameworkElement;
 var r = root.Handler?.PlatformView as FrameworkElement;
 if (v is not null && r is not null)
 {
 GeneralTransform t = v.TransformToVisual(r);
 var p = t.TransformPoint(new Windows.Foundation.Point(0,0));
 return new Rect(p.X, p.Y, element.Width, element.Height);
 }
#endif
 double fx =0, fy =0;
 Element? current = element;
 while (current is VisualElement ve && current != root)
 {
 fx += ve.X;
 fy += ve.Y;
 current = ve.Parent;
 }
 return new Rect(fx, fy, element.Width, element.Height);
 }

 private static async void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
 {
 if (bindable is FloatingMenu fm)
 {
 bool show = (bool)newValue;
 fm.IsVisible = show;
 fm.OutsideTapCatcher.InputTransparent = !show;
 if (show)
 {
 await System.Threading.Tasks.Task.WhenAll(
 fm.MenuPanel.FadeTo(1.0,150, Easing.CubicOut),
 fm.MenuPanel.ScaleTo(1.0,150, Easing.CubicOut)
 );
 }
 else
 {
 await System.Threading.Tasks.Task.WhenAll(
 fm.MenuPanel.FadeTo(0.0,120, Easing.CubicIn),
 fm.MenuPanel.ScaleTo(0.95,120, Easing.CubicIn)
 );
 fm.IsVisible = false;
 fm.Detach();
 }
 }
 }

 private void OnOutsideTapped(object? sender, EventArgs e) => Hide();
 private void OnEditTapped(object? sender, EventArgs e) { if (EditCommand?.CanExecute(BindingContext) == true) EditCommand.Execute(BindingContext); Hide(); }
 private void OnDeleteTapped(object? sender, EventArgs e) { if (DeleteCommand?.CanExecute(BindingContext) == true) DeleteCommand.Execute(BindingContext); Hide(); }
 }
}