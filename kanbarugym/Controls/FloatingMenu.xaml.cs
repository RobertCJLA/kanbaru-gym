using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using kanbarugym.ViewModels;
using kanbarugym.Clases;

#if ANDROID
using AView = Android.Views.View;
using Android.App;
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
        public static readonly BindableProperty TargetTrainerProperty = BindableProperty.Create(
            nameof(TargetTrainer), typeof(EntrenadorClass), typeof(FloatingMenu));

        private VisualElement? _anchor;
        private VisualElement? _root;
        private ItemsView? _itemsView;
        private double _verticalOffset;
        private double _horizontalOffset;

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

        public EntrenadorClass? TargetTrainer
        {
            get => (EntrenadorClass?)GetValue(TargetTrainerProperty);
            set => SetValue(TargetTrainerProperty, value);
        }

        public FloatingMenu()
        {
            InitializeComponent();
            IsVisible = false;
            // Quitar cambio de BindingContext para que herede el de la página (TrainersViewModel)
            // this.BindingContext = this; // REMOVIDO: evita que EditCommand/DeleteCommand queden en null
        }

        /// <summary>
        /// Muestra el menú anclado al elemento visual especificado
        /// </summary>
        /// <param name="anchor">El elemento visual (botón de engranaje) al que se anclará el menú</param>
        /// <param name="root">El elemento raíz (Grid principal) para calcular posiciones relativas</param>
        /// <param name="scroller">El CollectionView o ScrollView para manejar el desplazamiento</param>
        public void ShowFor(VisualElement anchor, VisualElement root, ItemsView? scroller = null)
        {
            Detach();
            _anchor = anchor;
            // Usar OverlayRoot como referencia para el cálculo de coordenadas
            _root = OverlayRoot;
            _itemsView = scroller;
            _verticalOffset = 0;
            _horizontalOffset = 0;

            if (root is not null)
                root.SizeChanged += OnRootSizeChanged;

            if (_anchor is not null)
                _anchor.SizeChanged += OnAnchorSizeChanged;

            if (_itemsView is not null)
                _itemsView.Scrolled += OnItemsViewScrolled;

            // Abrir primero y luego reposicionar para asegurar que el layout y handlers existan
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                IsOpen = true; // hace visible el overlay y crea/asegura handlers

                // Esperar a que el layout se estabilice
                await Task.Delay(50);
                Reposition();
                await Task.Delay(50);
                Reposition();
            });
        }

        public void Hide() => IsOpen = false;

        private void OnItemsViewScrolled(object? s, ItemsViewScrolledEventArgs e)
        {
            _verticalOffset = e.VerticalOffset;
            _horizontalOffset = e.HorizontalOffset;

            // Reposicionar en el hilo principal con un pequeño delay para suavizar
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Reposition();
            });
        }

        private void OnRootSizeChanged(object? s, EventArgs e) => Reposition();

        private void OnAnchorSizeChanged(object? s, EventArgs e) => Reposition();

        private void Detach()
        {
            // Buscar el root original (el que se pasó como parámetro) para desuscribirse
            var page = this.Parent;
            while (page is not null && page is not Page)
                page = page.Parent;

            if (page is Page p)
            {
                var rootGrid = p.FindByName<VisualElement>("RootGrid");
                if (rootGrid is not null)
                    rootGrid.SizeChanged -= OnRootSizeChanged;
            }

            if (_anchor is not null)
                _anchor.SizeChanged -= OnAnchorSizeChanged;

            if (_itemsView is not null)
                _itemsView.Scrolled -= OnItemsViewScrolled;

            _root = null;
            _anchor = null;
            _itemsView = null;
        }

        private void Reposition()
        {
            // Limpieza de condiciones duplicadas y ajuste de posición
            if (_anchor is null || OverlayRoot is null || MenuPanel is null)
                return;

            // Asegurar que existan handlers/plataforma antes de calcular coordenadas
            if (_anchor.Handler is null || OverlayRoot.Handler is null || MenuPanel.Handler is null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(16);
                    Reposition();
                });
                return;
            }

            // Asegurar que tengamos medidas válidas del overlay para evitar posiciones absolutas en el primer render
            if (OverlayRoot.Width <= 0 || OverlayRoot.Height <= 0)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(16);
                    Reposition();
                });
                return;
            }

            try
            {
                if (_anchor.Width <= 0 || _anchor.Height <= 0)
                    return;

                var rect = GetBoundsRelativeToRoot(_anchor, OverlayRoot);
                if (double.IsNaN(rect.X) || double.IsNaN(rect.Y) ||
                    double.IsInfinity(rect.X) || double.IsInfinity(rect.Y) ||
                    rect.Width <= 0 || rect.Height <= 0)
                    return;

                double menuWidth = MenuPanel.Width > 0 ? MenuPanel.Width : 150;
                double menuHeight = MenuPanel.Height > 0 ? MenuPanel.Height : 100;

                const double extraUpShift = 20; // subida adicional
                const double extraRightShift = 8; // desplazamiento a la derecha

                double x = rect.X - menuWidth + extraRightShift - _horizontalOffset; // mover a la derecha
                double y = rect.Y + rect.Height - _verticalOffset - extraUpShift; // subir

                double rootWidth = OverlayRoot.Width > 0 ? OverlayRoot.Width : 400;
                double rootHeight = OverlayRoot.Height > 0 ? OverlayRoot.Height : 800;

                if (x < 0) x = 0;
                if (y + menuHeight > rootHeight)
                {
                    y = rect.Y - menuHeight - _verticalOffset - extraUpShift;
                    if (y < 0) y = 0;
                }
                if (y < 0) y = 0;

                MenuPanel.TranslationX = x;
                MenuPanel.TranslationY = y;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al reposicionar el menú: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los límites del elemento relativo al elemento raíz
        /// </summary>
        private static Rect GetBoundsRelativeToRoot(VisualElement element, VisualElement root)
        {
#if ANDROID
            var v = element.Handler?.PlatformView as AView;
            var r = root.Handler?.PlatformView as AView;
            if (v is not null && r is not null)
            {
                var locA = new int[2];
                v.GetLocationOnScreen(locA);
                var locR = new int[2];
                r.GetLocationOnScreen(locR);

                // Convertir píxeles a unidades lógicas usando la densidad de pantalla
                var activity = Platform.CurrentActivity;
                var density = activity?.Resources?.DisplayMetrics?.Density ?? 1.0f;

                double x = (locA[0] - locR[0]) / density;
                double y = (locA[1] - locR[1]) / density;

                // Usar las dimensiones reales del elemento (ya están en unidades lógicas)
                double width = element.Width > 0 && !double.IsNaN(element.Width) ? element.Width : (v.Width / density);
                double height = element.Height > 0 && !double.IsNaN(element.Height) ? element.Height : (v.Height / density);

                return new Rect(x, y, width, height);
            }
#elif IOS || MACCATALYST
            var v = element.Handler?.PlatformView as UIView;
            var r = root.Handler?.PlatformView as UIView;
            if (v is not null && r is not null)
            {
                var p = v.ConvertPointToView(CGPoint.Empty, r);
                double width = v.Bounds.Width > 0 ? v.Bounds.Width : element.Width;
                double height = v.Bounds.Height > 0 ? v.Bounds.Height : element.Height;
                return new Rect(p.X, p.Y, width, height);
            }
#elif WINDOWS
            var v = element.Handler?.PlatformView as FrameworkElement;
            var r = root.Handler?.PlatformView as FrameworkElement;
            if (v is not null && r is not null)
            {
                GeneralTransform t = v.TransformToVisual(r);
                var p = t.TransformPoint(new Windows.Foundation.Point(0, 0));
                double width = v.ActualWidth > 0 ? v.ActualWidth : element.Width;
                double height = v.ActualHeight > 0 ? v.ActualHeight : element.Height;
                return new Rect(p.X, p.Y, width, height);
            }
#endif
            // Fallback: calcular sumando offsets a través del árbol visual
            double fx = 0, fy = 0;
            Element? current = element;

            while (current is VisualElement ve && current != root)
            {
                fx += ve.X;
                fy += ve.Y;
                current = ve.Parent;
            }

            double fallbackWidth = element.Width > 0 ? element.Width : 24;
            double fallbackHeight = element.Height > 0 ? element.Height : 24;

            return new Rect(fx, fy, fallbackWidth, fallbackHeight);
        }

        private static async void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not FloatingMenu fm)
                return;

            bool show = (bool)newValue;

            if (show)
            {
                // Mostrar el overlay y el menú
                fm.IsVisible = true;
                fm.OutsideTapCatcher.InputTransparent = false;

                // Reposicionar antes de animar
                fm.Reposition();

                // Animar la aparición
                fm.MenuPanel.Opacity = 0;
                fm.MenuPanel.Scale = 0.90;

                await System.Threading.Tasks.Task.WhenAll(
                    fm.MenuPanel.FadeTo(1.0, 150, Easing.CubicOut),
                    fm.MenuPanel.ScaleTo(1.0, 150, Easing.CubicOut));
            }
            else
            {
                // Animar la desaparición
                await System.Threading.Tasks.Task.WhenAll(
                    fm.MenuPanel.FadeTo(0.0, 120, Easing.CubicIn),
                    fm.MenuPanel.ScaleTo(0.90, 120, Easing.CubicIn));

                // Ocultar después de la animación
                fm.IsVisible = false;
                fm.OutsideTapCatcher.InputTransparent = true;
                fm.Detach();
            }
        }

        private void OnOutsideTapped(object? sender, EventArgs e)
        {
            // Cerrar el menú cuando se toca fuera
            Hide();
        }

        private void OnEditTapped(object? sender, EventArgs e)
        {
            // Ejecutar solo con el entrenador objetivo
            if (TargetTrainer is not null && EditCommand?.CanExecute(TargetTrainer) == true)
                EditCommand.Execute(TargetTrainer);
            Hide();
        }

        private void OnDeleteTapped(object? sender, EventArgs e)
        {
            if (TargetTrainer is not null && DeleteCommand?.CanExecute(TargetTrainer) == true)
                DeleteCommand.Execute(TargetTrainer);
            Hide();
        }
    }
}