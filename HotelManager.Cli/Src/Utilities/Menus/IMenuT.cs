namespace HotelManager.Cli.Utilities.Menus;

internal interface IMenu<in T> : IMenu
{
    string IMenu.GetTitle(ApplicationContext application, object? context)
    {
        return GetTitle(application, (T?)context);
    }

    IReadOnlyList<IMenuItem> IMenu.GetItems(ApplicationContext application, object? context)
    {
        return GetItems(application, (T?)context);
    }

    string GetTitle(ApplicationContext application, T? context);
    IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, T? context);
}