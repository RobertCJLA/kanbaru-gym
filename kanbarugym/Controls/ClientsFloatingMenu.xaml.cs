using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using kanbarugym.Clases;

#if ANDROID
using AView = Android.Views.View;
#elif IOS || MACCATALYST
using UIKit;
using CoreGraphics;
#elif WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#endif

namespace kanbarugym.Views.Controls;

public partial class ClientsFloatingMenu : ContentView
{
 public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
 nameof(IsOpen), typeof(bool), typeof(ClientsFloatingMenu), false, propertyChanged: OnIsOpenChanged);
 public static readonly BindableProperty PayCommandProperty = BindableProperty.Create(
 nameof(PayCommand), typeof(ICommand), typeof(ClientsFloatingMenu));
 public static readonly BindableProperty ViewCommandProperty = BindableProperty.Create(
 nameof(ViewCommand), typeof(ICommand), typeof(ClientsFloatingMenu));
 public static readonly BindableProperty EditCommandProperty = BindableProperty.Create(
 nameof(EditCommand), typeof(ICommand), typeof(ClientsFloatingMenu));
 public static readonly BindableProperty PayTextProperty = BindableProperty.Create(
 nameof(PayText), typeof(string), typeof(ClientsFloatingMenu), "Pagar membresía");
 public static readonly BindableProperty ViewTextProperty = BindableProperty.Create(
 nameof(ViewText), typeof(string), typeof(ClientsFloatingMenu), "Ver membresía");
 public static readonly BindableProperty EditTextProperty = BindableProperty.Create(
 nameof(EditText), typeof(string), typeof(ClientsFloatingMenu), "Editar");
 public static readonly BindableProperty TargetClientProperty = BindableProperty.Create(
 nameof(TargetClient), typeof(ClientesClass), typeof(ClientsFloatingMenu));

 VisualElement? _anchor;
 VisualElement? _root;
 ItemsView? _itemsView;
 double _verticalOffset;
 double _horizontalOffset;

 public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
 public ICommand? PayCommand { get => (ICommand?)GetValue(PayCommandProperty); set => SetValue(PayCommandProperty, value); }
 public ICommand? ViewCommand { get => (ICommand?)GetValue(ViewCommandProperty); set => SetValue(ViewCommandProperty, value); }
 public ICommand? EditCommand { get => (ICommand?)GetValue(EditCommandProperty); set => SetValue(EditCommandProperty, value); }
 public string PayText { get => (string)GetValue(PayTextProperty); set => SetValue(PayTextProperty, value); }
 public string ViewText { get => (string)GetValue(ViewTextProperty); set => SetValue(ViewTextProperty, value); }
 public string EditText { get => (string)GetValue(EditTextProperty); set => SetValue(EditTextProperty, value); }
 public ClientesClass? TargetClient { get => (ClientesClass?)GetValue(TargetClientProperty); set => SetValue(TargetClientProperty, value); }

 public ClientsFloatingMenu()
 {
 InitializeComponent();
 IsVisible = false;
 }

 public void ShowFor(VisualElement anchor, VisualElement root, ItemsView? scroller = null)
 {
 Detach();
 _anchor = anchor;
 _root = OverlayRoot;
 _itemsView = scroller;
 _verticalOffset =0;
 _horizontalOffset =0;

 // Prevenir destello: asegurar que el panel esté invisible antes de abrir
 MenuPanel.Opacity =0;
 MenuPanel.Scale =0.9;

 if (root is not null) root.SizeChanged += OnRootSizeChanged;
 if (_anchor is not null) _anchor.SizeChanged += OnAnchorSizeChanged;
 if (_itemsView is not null) _itemsView.Scrolled += OnItemsViewScrolled;

 MainThread.BeginInvokeOnMainThread(async () =>
 {
 IsOpen = true;
 await Task.Delay(50); Reposition();
 await Task.Delay(50); Reposition();
 });
 }

 public void Hide() => IsOpen = false;

 void OnOutsideTapped(object? sender, EventArgs e) => Hide();

 void OnPayTapped(object? sender, EventArgs e)
 {
 if (TargetClient != null && PayCommand?.CanExecute(TargetClient) == true) PayCommand.Execute(TargetClient);
 Hide();
 }
 void OnViewTapped(object? sender, EventArgs e)
 {
 if (TargetClient != null && ViewCommand?.CanExecute(TargetClient) == true) ViewCommand.Execute(TargetClient);
 Hide();
 }
 void OnEditTapped(object? sender, EventArgs e)
 {
 if (TargetClient != null && EditCommand?.CanExecute(TargetClient) == true) EditCommand.Execute(TargetClient);
 Hide();
 }

 void OnItemsViewScrolled(object? sender, ItemsViewScrolledEventArgs e)
 {
 _verticalOffset = e.VerticalOffset;
 _horizontalOffset = e.HorizontalOffset;
 MainThread.BeginInvokeOnMainThread(Reposition);
 }
 void OnRootSizeChanged(object? sender, EventArgs e) => Reposition();
 void OnAnchorSizeChanged(object? sender, EventArgs e) => Reposition();

 void Detach()
 {
 if (_root != null) _root.SizeChanged -= OnRootSizeChanged;
 if (_anchor != null) _anchor.SizeChanged -= OnAnchorSizeChanged;
 if (_itemsView != null) _itemsView.Scrolled -= OnItemsViewScrolled;
 _anchor = null; _root = null; _itemsView = null;
 }

 void Reposition()
 {
 if (_anchor == null || OverlayRoot == null || MenuPanel == null) return;
 if (_anchor.Handler == null || OverlayRoot.Handler == null || MenuPanel.Handler == null || OverlayRoot.Width <=0 || OverlayRoot.Height <=0)
 {
 _ = Task.Run(async () => { await Task.Delay(16); MainThread.BeginInvokeOnMainThread(Reposition); });
 return;
 }
 if (_anchor.Width <=0 || _anchor.Height <=0) return;
 var rect = GetBoundsRelativeToRoot(_anchor, OverlayRoot);
 if (rect.Width <=0 || rect.Height <=0) return;
 double menuWidth = MenuPanel.Width >0 ? MenuPanel.Width :170;
 double menuHeight = MenuPanel.Height >0 ? MenuPanel.Height :140;
 const double extraUpShift =20;
 const double extraRightShift =8;
 double x = rect.X - menuWidth + extraRightShift - _horizontalOffset;
 double y = rect.Y + rect.Height - _verticalOffset - extraUpShift;
 if (x <0) x =0;
 if (y + menuHeight > OverlayRoot.Height)
 {
 y = rect.Y - menuHeight - _verticalOffset - extraUpShift;
 if (y <0) y =0;
 }
 if (y <0) y =0;
 MenuPanel.TranslationX = x;
 MenuPanel.TranslationY = y;
 }

 static Rect GetBoundsRelativeToRoot(VisualElement anchor, VisualElement root)
 {
#if ANDROID
 if (anchor.Handler?.PlatformView is AView aView && root.Handler?.PlatformView is AView rView)
 {
 int[] locA = new int[2]; aView.GetLocationOnScreen(locA);
 int[] locR = new int[2]; rView.GetLocationOnScreen(locR);
 float density = aView.Context?.Resources?.DisplayMetrics?.Density ??1f;
 double x = (locA[0] - locR[0]) / density; double y = (locA[1] - locR[1]) / density;
 return new Rect(x, y, anchor.Width, anchor.Height);
 }
#elif IOS || MACCATALYST
 if (anchor.Handler?.PlatformView is UIKit.UIView av && root.Handler?.PlatformView is UIKit.UIView rv)
 {
 var point = av.ConvertPointToView(CoreGraphics.CGPoint.Empty, rv);
 return new Rect(point.X, point.Y, anchor.Width, anchor.Height);
 }
#elif WINDOWS
 if (anchor.Handler?.PlatformView is FrameworkElement fe && root.Handler?.PlatformView is FrameworkElement re)
 {
 var transform = fe.TransformToVisual(re);
 var p = transform.TransformPoint(new Windows.Foundation.Point(0,0));
 return new Rect(p.X, p.Y, anchor.Width, anchor.Height);
 }
#endif
 double accX =0; double accY =0; Element? current = anchor;
 while (current is VisualElement ve && current != root)
 { accX += ve.X; accY += ve.Y; current = ve.Parent; }
 return new Rect(accX, accY, anchor.Width, anchor.Height);
 }

 static async void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
 {
 if (bindable is not ClientsFloatingMenu self) return;
 bool open = (bool)newValue;
 if (open)
 {
 // Prevenir destello: preparar estado antes de hacer visible
 self.MenuPanel.Opacity =0;
 self.MenuPanel.Scale =0.9;
 self.Reposition();
 self.IsVisible = true;
 self.OutsideTapCatcher.InputTransparent = false;
 await Task.WhenAll(self.MenuPanel.FadeTo(1,180,Easing.CubicOut), self.MenuPanel.ScaleTo(1,180,Easing.CubicOut));
 }
 else
 {
 await Task.WhenAll(self.MenuPanel.FadeTo(0,140,Easing.CubicIn), self.MenuPanel.ScaleTo(0.9,140,Easing.CubicIn));
 self.IsVisible = false;
 self.OutsideTapCatcher.InputTransparent = true;
 self.Detach();
 }
 }
}
