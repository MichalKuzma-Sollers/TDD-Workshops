using System;
using System.Collections.Generic;

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
        private IList<String> ratingScale = new List<String>
        {
            "1-", "1", "1+", "2-", "2", "2+", "3-", "3", "3+", "4-", "4", "4+", "5-", "5", "5+", "6-", "6", "6+", "7-", "7", "7+", "8-", "8", "8+", "9-", "9", "9+", "10-", "10", "10+"
        };

        private IThresholdsChecker thresholdsChecker;
        public DecisionLevels(IThresholdsChecker thresholdsChecker)
        {
            this.thresholdsChecker = thresholdsChecker;
        }

        public DecisionLevel? GetDecisionLevel(double? proposedLimit, double? approvedLimit, string proposedRating)
        {
            if (approvedLimit == null || approvedLimit == 0.0)
                return DecisionLevel.SixEyes;
            if (proposedLimit == null)
                throw new WarningMessageException();

            double limitIncrease = (double)((proposedLimit - approvedLimit) / approvedLimit);
            
            if (limitIncrease >= 0.1)
                return DecisionLevel.FourEyes;

            int proposedRatingIndex = ratingScale.IndexOf(proposedRating);
            bool proposedRatingBetween4mAnd7p = proposedRatingIndex >= ratingScale.IndexOf("4-") && proposedRatingIndex <= ratingScale.IndexOf("7+");
            bool proposedRatingBetween3mAnd7p = proposedRatingIndex >= ratingScale.IndexOf("3-") && proposedRatingIndex <= ratingScale.IndexOf("7+");

            if ((limitIncrease > 0.0 && !proposedRatingBetween4mAnd7p) || (limitIncrease <= 0.0 && !proposedRatingBetween3mAnd7p))
            {
                return thresholdsChecker.CheckThresholds(new HashSet<DecisionLevel> { DecisionLevel.Ecc, DecisionLevel.FourEyes, DecisionLevel.IbcFig, DecisionLevel.SixEyes });
            }

            return thresholdsChecker.CheckThresholds(new HashSet<DecisionLevel> { DecisionLevel.FourEyes, DecisionLevel.SixEyes });
        }
    }
}
