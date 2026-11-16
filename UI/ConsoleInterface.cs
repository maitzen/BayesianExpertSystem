using System;

namespace BayesianExpertSystem.UI;

public class ConsoleInterface(KnowledgeBase @base, BayesianEngine engine)
{
    private readonly KnowledgeBase _base = @base;

    private readonly BayesianEngine _engine = engine;

    public void Run()
    {
        Console.WriteLine($"{_base.Title}\n");
        PrintHypotheses();
        foreach (var evidence in _base.Evidences)
        {
            Console.WriteLine($"\nВопрос {evidence.Id}: {evidence.Text}");
            Console.WriteLine("Введите значение с -5 ('точно нет') по 5 ('точно да'). 0 - 'не знаю'.");
            int userInput = GetValidInput();
            if (userInput == 0)
            {
                Console.WriteLine("Пропуск вопроса.");
                continue;
            }
            double pEvidence = (userInput + 5) / 10.0;
            _engine.UpdateProbabilities(evidence.Id, pEvidence);
            PrintHypotheses();
        }
        Console.WriteLine("\nКонсультация завершена.");
    }

    private void PrintHypotheses()
    {
        Console.WriteLine("\nТекущие вероятности гипотез");
        foreach (var h in _base.Hypotheses)
        {
            Console.WriteLine($"{h.Text,-20}{h.CurrentProbability:P2}");
        }
    }

    private static int GetValidInput()
    {
        while (true)
        {
            Console.Write("Ваш ответ: ");
            if (int.TryParse(Console.ReadLine(), out int value) && value >= -5 && value <= 5)
            {
                return value;
            }
            Console.WriteLine("Ошибка. Введите целое число от -5 до 5.");
        }
    }
}
