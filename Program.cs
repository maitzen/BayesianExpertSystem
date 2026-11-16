using System;
using System.IO;

namespace BayesianExpertSystem;

internal class Program
{
    private static void Main()
    {
        Console.Write("Введите путь к файлу: ");
        string basePath = Console.ReadLine() ?? throw new InvalidOperationException();
        if (!File.Exists(basePath))
        {
            Console.WriteLine($"Файл {basePath} не найден.");
            return;
        }
        try
        {
            var @base = KnowledgeBaseParser.Parse(basePath);
            var engine = new BayesianEngine(@base);
            var ui = new ConsoleInterface(@base, engine);
            ui.Run();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}.");
        }
        Console.Write("Нажмите любую клавишу...");
        Console.ReadKey(intercept: true);
    }
}
