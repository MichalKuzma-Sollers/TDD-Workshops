using Microsoft.VisualStudio.TestTools.UnitTesting;
using TDD_Workshops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace TDD_Workshops.Tests
{
    [TestClass()]
    public class DecisionLevelsTests
    {
        private DecisionLevels decisionLevels;
        private Mock<IThresholdsChecker> thresholdsChecker;

        [TestInitialize]
        public void Init()
        {
            thresholdsChecker = new Mock<IThresholdsChecker>();
            decisionLevels = new DecisionLevels(thresholdsChecker.Object);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenProposedLimitIsNull()
        {
            Assert.ThrowsException<WarningMessageException>(() =>
            {
                decisionLevels.GetDecisionLevel(null, 10.0, "1");
            });
        }

        [DataRow(100.0, "3-")]
        [DataRow(100.0, "5")]
        [DataRow(100.0, "7+")]
        [DataRow(90.0, "5")]
        [DataTestMethod]
        public void ShouldCheckThresholdsSixEyesFourEyesWhenNoIncreaseAndRatingFrom3mTo7p(double proposedLimit, string proposedRating)
        {
            VerifyThresholdsChecked(proposedLimit, proposedRating, new HashSet<DecisionLevel> { DecisionLevel.SixEyes, DecisionLevel.FourEyes });
        }

        [DataRow(100.0, "1")]
        [DataRow(100.0, "2+")]
        [DataRow(100.0, "8-")]
        [DataRow(100.0, "10")]
        [DataRow(90.0, "10")]
        [DataTestMethod]
        public void ShouldCheckThresholdsAllWhenNoIncreaseAndRatingLowerThan3mOrHigherThan7p(double proposedLimit, string proposedRating)
        {

            VerifyThresholdsChecked(proposedLimit, proposedRating, new HashSet<DecisionLevel> { DecisionLevel.SixEyes, DecisionLevel.FourEyes, DecisionLevel.Ecc, DecisionLevel.IbcFig });
        }

        [TestMethod]
        public void ShouldReturnSixEyesIfNewCustomer()
        {
            Assert.AreEqual(DecisionLevel.SixEyes, decisionLevels.GetDecisionLevel(100.0, null, "3"));
            Assert.AreEqual(DecisionLevel.SixEyes, decisionLevels.GetDecisionLevel(100.0, 0.0, "3"));
        }

        [DataRow(150.0)]
        [DataRow(111.0)]
        [DataTestMethod]
        public void ShouldReturnFourEyesIfIncreaseOver10p(double proposedLimit)
        {
            Assert.AreEqual(DecisionLevel.FourEyes, decisionLevels.GetDecisionLevel(proposedLimit, 100.0, "4"));
        }

        [DataRow(109.0, "4-")]
        [DataRow(105.0, "5")]
        [DataRow(101.0, "7+")]
        [DataTestMethod]
        public void ShouldCheckThresholdsSixEyesFourEyesWhenIncreasePositiveButNoMoreThan10pAndRatingFrom4mTo7p(double proposedLimit, string proposedRating)
        {
            VerifyThresholdsChecked(proposedLimit, proposedRating, new HashSet<DecisionLevel> { DecisionLevel.SixEyes, DecisionLevel.FourEyes });
        }

        [DataRow(109.0, "1")]
        [DataRow(105.0, "3+")]
        [DataRow(104.0, "8-")]
        [DataRow(101.0, "10")]
        [DataTestMethod]
        public void ShouldCheckThresholdsAllWhenIncreasePositiveButNoMoreThan10pAndRatingLowerThan4mOrHigherThan7p(double proposedLimit, string proposedRating)
        {
            VerifyThresholdsChecked(proposedLimit, proposedRating, new HashSet<DecisionLevel> { DecisionLevel.SixEyes, DecisionLevel.FourEyes, DecisionLevel.Ecc, DecisionLevel.IbcFig });
        }

        private void VerifyThresholdsChecked(double proposedLimit, string proposedRating, HashSet<DecisionLevel> availableLevels)
        {
            thresholdsChecker.Setup(mock => mock.CheckThresholds(It.Is<ISet<DecisionLevel>>(arg => arg != null
                   && arg.SetEquals(availableLevels))));

            decisionLevels.GetDecisionLevel(proposedLimit, 100.0, proposedRating);

            thresholdsChecker.VerifyAll();
        }
    }
}