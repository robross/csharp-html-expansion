using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Html.Expansion;
using System.Web.Mvc;

namespace Tests
{
	[TestClass]
	public class ExpansionTests
	{
		[TestMethod]
		public void Tag()
		{
			var expected = new TagBuilder("div");
			var actual = ExpandShorthand.Do("div").Item1;
			
			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithId()
		{
			var expected = new TagBuilder("div");
			expected.MergeAttribute("id", "MyDiv");
			
			var actual = ExpandShorthand.Do("div#MyDiv").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithClass()
		{
			var expected = new TagBuilder("div");
			expected.AddCssClass("fooClass");

			var actual = ExpandShorthand.Do("div.fooClass").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithIdAndClass()
		{
			var expected = new TagBuilder("div");
			expected.MergeAttribute("id", "MyDiv");
			expected.AddCssClass("fooClass");

			var actual = ExpandShorthand.Do("div#MyDiv.fooClass").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithIdAndMultipleClass()
		{
			var expected = new TagBuilder("div");
			expected.MergeAttribute("id", "MyDiv");
			expected.AddCssClass("barClass");
			expected.AddCssClass("fooClass");

			var actual = ExpandShorthand.Do("div#MyDiv.fooClass.barClass").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithHyphenatedClass()
		{
			var expected = new TagBuilder("div");
			expected.AddCssClass("bar-class");
			
			var actual = ExpandShorthand.Do("div.bar-class").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithAttribute()
		{
			var expected = new TagBuilder("a");
			expected.MergeAttribute("href", "#foo");

			var actual = ExpandShorthand.Do("a[href='#foo']").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithMultipleAttributes()
		{
			var expected = new TagBuilder("a");
			expected.MergeAttribute("href", "#foo");
			expected.MergeAttribute("toggle", "things");

			var actual = ExpandShorthand.Do("a[href='#foo'][toggle='things']").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithInnerTextAndAttributes()
		{
			var expected = new TagBuilder("a");
			expected.MergeAttribute("href", "#foo");
			expected.MergeAttribute("toggle", "things");
			expected.InnerHtml = "Do";

			var actual = ExpandShorthand.Do("a[href='#foo'][toggle='things'] 'Do'").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}

		[TestMethod]
		public void TagWithInnerTextWithWhtitespace()
		{
			var expected = new TagBuilder("a");
			expected.InnerHtml = "Do Stuff";

			var actual = ExpandShorthand.Do("a 'Do Stuff'").Item1;

			Assert.AreEqual(expected.ToString(), actual.Render());
		}
	}
}
