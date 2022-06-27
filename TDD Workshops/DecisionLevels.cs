using System;

namespace TDD_Workshops
{
    public enum DecisionLevel
    {
        Ecc,
        FourEyes,
        SixEyes,
        IbcFig
    }

    public class DecisionLevels
    {
        private IThresholdsChecker thresholdsChecker;
        public DecisionLevels(IThresholdsChecker thresholdsChecker)
        {
            this.thresholdsChecker = thresholdsChecker;
        }

        public DecisionLevel? GetDecisionLevel(double? proposedLimit, double? approvedLimit, string proposedRating)
        {
            return null;
        }
    }
}
