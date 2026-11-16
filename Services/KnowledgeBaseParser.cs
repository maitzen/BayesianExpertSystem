using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace BayesianExpertSystem.Services;

public static partial class KnowledgeBaseParser
{
    public static KnowledgeBase Parse(string filePath)
    {
        var @base = new KnowledgeBase();
        var lines = File.ReadAllLines(filePath);
        var regex = EvidenceRegex();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            var match = regex.Match(line);
            if (match.Success)
            {
                var id = int.Parse(match.Groups[1].Value);
                var text = match.Groups[2].Value.Trim();
                @base.Evidences.Add(new Evidence(id, text));
                continue;
            }
            if (TryParseHypothesis(line, out var hypothesis))
            {
                @base.Hypotheses.Add(hypothesis);
            }
            else if (string.IsNullOrEmpty(@base.Title))
            {
                @base.Title = line;
            }
        }
        return @base;
    }

    private static bool TryParseHypothesis(string line, out Hypothesis hypothesis)
    {
        hypothesis = new Hypothesis();
        var parts = line.Split(',');
        if (parts.Length < 5)
        {
            return false;
        }
        try
        {
            hypothesis.Text = parts[0].Trim();
            hypothesis.PriorProbability = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            hypothesis.CurrentProbability = hypothesis.PriorProbability;
            for (int i = 2; i < parts.Length - 2; i += 3)
            {
                int k = int.Parse(parts[i].Trim());
                double py = double.Parse(parts[i + 1].Trim(), CultureInfo.InvariantCulture);
                double pn = double.Parse(parts[i + 2].Trim(), CultureInfo.InvariantCulture);
                hypothesis.Conditionals[k] = (py, pn);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    [GeneratedRegex(@"^(\d+)\.\s+(.+)$")]
    private static partial Regex EvidenceRegex();
}
