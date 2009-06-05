using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StringMatching;

namespace TestProject
{
    [TestFixture]
    public class StringDiffTests
    {
        [Test]
        public void SimplestExecutionPlan()
        {
            var stringA = "a";
            var stringB = "a";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            Assert.AreEqual(0, executionPlan.Steps.Count);
        }

        [Test]
        public void SimplestDiff_Test()
        {
            var stringA = "a";
            var stringB = "b";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            Assert.AreEqual(1, executionPlan.Steps.Count);
        }

        [Test]
        public void MultiItem_Diff_Test()
        {
            var stringA = "abcba";
            var stringB = "bbcbb";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            Assert.AreEqual(2, executionPlan.Steps.Count);
        }

        [Test]
        public void MultiItem_Diff_Assymetrical_Test()
        {
            var stringA = "desk";
            var stringB = "pesticide";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            Assert.AreEqual(7, executionPlan.Steps.Count);
        }

        [Test]
        public void MultiItem_Diff_Horizontal_Assymetrical_Test()
        {
            var stringA = "abdbca";
            var stringB = "abc";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            Assert.AreEqual(3, executionPlan.Steps.Count);
        }

        [Test]
        public void MultiItem_Diff_Assymetrical_Test_With_Print()
        {
            var stringA = "desk";
            var stringB = "pesticide";

            var diff = new StringDiff();
            var executionPlan = diff.Between(stringA, stringB);

            var printedPlan = executionPlan.PrintAllSteps();
            Assert.IsNotNullOrEmpty(printedPlan);
        }
    }
}
