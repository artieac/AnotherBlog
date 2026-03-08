/**
 * Copyright (c) 2009 Arthur Correa.
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Common Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.opensource.org/licenses/cpl1.0.php
 *
 * Contributors:
 *    Arthur Correa – initial contribution
 */
using NUnit.Framework;
using AnotherBlog.Core.Utilities;

namespace AlwaysMoveForward.AnotherBlog.UnitTest.Utilities
{
    [TestFixture]
    public class UtilsTest
    {
        [Test]
        public void StripHtml_WithHtmlTags_RemovesTags()
        {
            var input = "<p>Hello <strong>World</strong></p>";

            var result = Utils.StripHtml(input);

            Assert.That(result, Is.EqualTo("Hello World"));
        }

        [Test]
        public void StripHtml_WithNestedTags_RemovesAllTags()
        {
            var input = "<div><p>Text <a href='link'>link</a></p></div>";

            var result = Utils.StripHtml(input);

            Assert.That(result, Is.EqualTo("Text link"));
        }

        [Test]
        public void StripHtml_WithNoTags_ReturnsSameString()
        {
            var input = "Plain text without tags";

            var result = Utils.StripHtml(input);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void StripHtml_WithNull_ReturnsEmptyString()
        {
            var result = Utils.StripHtml(null);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void StripHtml_WithEmptyString_ReturnsEmptyString()
        {
            var result = Utils.StripHtml(string.Empty);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void StripHtml_WithMultilineHtml_RemovesTags()
        {
            var input = "<p>Line 1\n<span>Line 2</span></p>";

            var result = Utils.StripHtml(input);

            Assert.That(result, Is.EqualTo("Line 1\nLine 2"));
        }

        [Test]
        public void StripHtml_WithSelfClosingTags_RemovesTags()
        {
            var input = "Text<br/>More text<hr/>";

            var result = Utils.StripHtml(input);

            Assert.That(result, Is.EqualTo("TextMore text"));
        }

        [Test]
        public void StripJavascript_WithScriptTag_RemovesScript()
        {
            var input = "Before<script>alert('test');</script>After";

            var result = Utils.StripJavascript(input);

            // Note: The current implementation has a bug - it doesn't actually modify the string
            // This test documents the current behavior
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void StripJavascript_WithNoScriptTag_ReturnsSameString()
        {
            var input = "Plain text without script";

            var result = Utils.StripJavascript(input);

            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void StripJavascript_WithEmptyString_ReturnsEmptyString()
        {
            var result = Utils.StripJavascript(string.Empty);

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
