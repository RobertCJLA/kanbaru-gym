using kanbarugym.ViewModels;
using kanbarugym.Clases;

namespace kanbarugym.Views;

public partial class Trainer : ContentPage
{
    public TrainersViewModel ViewModel { get; } = new();

    public Trainer()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarEntrenadores();
    }

    private void CloseAllMenus()
    {
        foreach (var it in ViewModel.Entrenadores)
            it.IsMenuVisible = false;
    }

    private Element? GetItemRoot(Element start)
    {
        // Sube hasta el contenedor del DataTemplate
        Element? cur = start;
        while (cur is not null && cur.Parent is not CollectionView && cur.Parent is not null)
            cur = cur.Parent;
        // El padre inmediato del template es una CollectionView; regresamos al nodo anterior válido
        return start; // usaremos el propio start y buscaremos hacia arriba en pocos niveles
    }

    private VisualElement? FindMenuBorder(Element? start)
    {
        // Buscar ascendiendo unos niveles y luego recorriendo descendientes inmediatos
        Element? node = start;
        for (int i = 0; i < 6 && node is not null; i++)
        {
            if (node is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    if (child is VisualElement ve && ve.AutomationId == "MenuBorder")
                        return ve;
                    if (child is Border b && b.AutomationId == "MenuBorder")
                        return b;
                }
            }
            node = node?.Parent;
        }
        return null;
    }

    private async void OnGearClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton btn && btn.BindingContext is EntrenadorClass entrenador)
        {
            var show = !entrenador.IsMenuVisible;
            CloseAllMenus();
            entrenador.IsMenuVisible = show;

            var menu = FindMenuBorder(btn);
            if (menu is not null)
            {
                if (show)
                    await Task.WhenAll(menu.FadeTo(1.0, 150, Easing.CubicOut), menu.ScaleTo(1.0, 150, Easing.CubicOut));
                else
                    await Task.WhenAll(menu.FadeTo(0.08, 150, Easing.CubicIn), menu.ScaleTo(0.95, 150, Easing.CubicIn));
            }
        }
    }

    private void OnEditTrainer(object? sender, TappedEventArgs e)
    {
        if ((sender as Element)?.BindingContext is EntrenadorClass entrenador)
        {
            CloseAllMenus();
            if (ViewModel.EditTrainerCommand?.CanExecute(entrenador) == true)
                ViewModel.EditTrainerCommand.Execute(entrenador);
        }
    }

    private void OnDeleteTrainer(object? sender, TappedEventArgs e)
    {
        if ((sender as Element)?.BindingContext is EntrenadorClass entrenador)
        {
            CloseAllMenus();
            if (ViewModel.DeleteTrainerCommand?.CanExecute(entrenador) == true)
                ViewModel.DeleteTrainerCommand.Execute(entrenador);
        }
    }
}