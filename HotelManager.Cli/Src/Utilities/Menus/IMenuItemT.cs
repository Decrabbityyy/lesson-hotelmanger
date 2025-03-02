namespace HotelManager.Cli.Utilities.Menus;

internal interface IMenuItem<in T> : IMenuItem
{
    string IMenuItem.GetTitle(ApplicationContext application, object? context)
    {
        return GetTitle(application, (T?)context);
    }

    (IMenu?, object?) IMenuItem.Run(ApplicationContext application, object? context)
    {
        return Run(application, (T?)context);
    }

    string GetTitle(ApplicationContext application, T? context);

    (IMenu?, object?) Run(ApplicationContext application, T? context);
}