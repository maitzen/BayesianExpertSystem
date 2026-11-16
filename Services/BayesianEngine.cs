namespace BayesianExpertSystem.Services;

public class BayesianEngine(KnowledgeBase @base)
{
    private readonly KnowledgeBase _base = @base;

    public void UpdateProbabilities(int evidenceId, double evidenceProbability)
    {
        foreach (var h in _base.Hypotheses)
        {
            if (!h.Conditionals.ContainsKey(evidenceId))
            {
                continue;
            }
            var (py, pn) = h.Conditionals[evidenceId];
            double pCurrent = h.CurrentProbability;
            double pE = pCurrent * py + (1.0 - pCurrent) * pn;
            double pNotE = pCurrent * (1.0 - py) + (1.0 - pCurrent) * (1.0 - pn);
            if (pE == 0.0 || pNotE == 0.0)
            {
                continue;
            }
            double pH_Given_E = (pCurrent * py) / pE;
            double pH_Given_NotE = (pCurrent * (1.0 - py)) / pNotE;
            h.CurrentProbability = pH_Given_E * evidenceProbability + pH_Given_NotE * (1.0 - evidenceProbability);
        }
    }
}
