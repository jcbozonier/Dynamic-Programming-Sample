using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StringMatching;

namespace TestProject
{
    [TestFixture]
    public class Simple_strings
    {
        [Test]
        public void Simplest_strings_both_match()
        {
            Assert.IsTrue(StringMatcher.IsMatch("a", "a"));
        }

        [Test]
        public void Simplest_strings_neither_match()
        {
            Assert.IsFalse(StringMatcher.IsMatch("a", "z"));
        }

        [Test]
        public void Simplest_strings_simplest_pattern_matches()
        {
            Assert.IsTrue(StringMatcher.IsMatch(".", "z"));
        }

        [Test]
        public void Simplest_strings_simplest_pattern_asterisk_matches()
        {
            Assert.IsTrue(StringMatcher.IsMatch("*", "z"));
        }
    }

    [TestFixture]
    public class Off_by_one_matchers
    {
        [Test]
        public void String_off_by_one_case_1_matches()
        {
            Assert.IsTrue(StringMatcher.IsMatch("a.c.e", "abcde"));
        }

        [Test]
        public void String_off_by_one_case_2_no_match()
        {
            Assert.IsFalse(StringMatcher.IsMatch("a.c.", "abcde"));
        }

    }

    [TestFixture]
    public class Off_by_many_matchers
    {
        [Test]
        public void String_off_by_many_case_1_matches()
        {
            Assert.IsTrue(StringMatcher.IsMatch(".a*.j*", "cadeajmn"));
        }

        [Test]
        public void String_off_by_many_case_2_no_match()
        {
            Assert.IsFalse(StringMatcher.IsMatch("a.d*", "abcde"));
        }

    }
}
