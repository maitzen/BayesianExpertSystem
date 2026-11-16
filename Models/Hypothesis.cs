using System.Collections.Generic;

namespace BayesianExpertSystem.Models;

public class Hypothesis
{
    public string Text { get; set; } = string.Empty;

    public double PriorProbability { get; set; } = -1.0;

    public double CurrentProbability { get; set; } = -1.0;

    public Dictionary<int, (double Py, double Pn)> Conditionals { get; set; } = [];
}
