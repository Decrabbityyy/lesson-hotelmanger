namespace HotelManager.Cli.Utilities.Menus;

internal interface IMenu
{
    string GetTitle(ApplicationContext application, object? context);

    IReadOnlyList<IMenuItem> GetItems(ApplicationContext application, object? context);

    public (IMenu? menu, object? context) Show(ApplicationContext application, object? context)
    {
        var title = GetTitle(application, context);
        var items = GetItems(application, context);
        Console.WriteLine();
        Console.WriteLine("+ ------------------");
        Console.WriteLine($"| {title}");
        Console.WriteLine("+ ------------------");
        for (var i = 0; i < items.Count; i++) Console.WriteLine($"| {i + 1}. {items[i].GetTitle(application, context)}");
        Console.WriteLine("+ ------------------");
        Console.Write("| 请输入序号: ");

        if (int.TryParse(Console.ReadLine(), out var choice) && choice > 0 && choice <= items.Count) return items[choice - 1].Run(application, context);
        Console.WriteLine("无效输入, 按任意键重试");
        return (this, context);
    }
}