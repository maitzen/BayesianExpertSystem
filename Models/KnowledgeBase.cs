using System.Collections.Generic;

namespace BayesianExpertSystem.Models;

public class KnowledgeBase
{
    public string Title { get; set; } = string.Empty;

    public List<Evidence> Evidences { get; set; } = [];

    public List<Hypothesis> Hypotheses { get; set; } = [];
}
