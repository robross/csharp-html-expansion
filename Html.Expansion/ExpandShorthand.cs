using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Html.Expansion
{
	public static class ExpandShorthand
	{
		static Regex _parser = new Regex(@"(?<tag>[^#\.[\s]+)(?<id>#[^\.[\s]+)*(?<class>\.[^\.[\s]+)*(?<attr>\[[\w-]+\s?=\s?'[^]]+'\])*", RegexOptions.IgnoreCase);

		public static Tuple<HtmlTag, HtmlTag> Do(string shorthand)
		{
			HtmlTag head = null;
			HtmlTag current = null;

			var matchedTags = _parser.Matches(shorthand);
			foreach (Match tagMatch in matchedTags)
			{
				var tagName = tagMatch.Groups["tag"].Value;

				if (tagName.IndexOf('\'') == 0)
					AddLiteal(head, current, tagName);
				else
					AddTag(ref head, ref current, tagMatch, tagName);
			}

			return new Tuple<HtmlTag, HtmlTag>(head, current);
		}

		private static void AddTag(ref HtmlTag head, ref HtmlTag current, Match tagMatch, string tagName)
		{
			var tag = new HtmlTag(tagName);

			string id = tagMatch.Groups["id"].Value;
			if (!string.IsNullOrWhiteSpace(id))
				tag.id(id.TrimStart('#'));

			tagMatch.Groups["class"]
				.Captures.Cast<Capture>().ToList()
				.ForEach(c => tag.addClass(c.Value.Trim('.')));

			tagMatch.Groups["attr"]
				.Captures.Cast<Capture>().ToList()
				.ForEach(c =>
				{
					var attr = c.Value.Trim('[', ']');
					var eqIndex = attr.IndexOf('=');
					var name = attr.Substring(0, eqIndex);
					var value = attr.Substring(eqIndex + 1, attr.Length - eqIndex - 1).Trim('\'');
					tag.attr(name, value);
				});

			if (head == null)
			{
				current = tag;
				head = tag;
			}
			else
			{
				current.appendTag(tag);
				current = tag;
			}
		}

		private static void AddLiteal(HtmlTag head, HtmlTag current, string tagName)
		{
			if (head == null)
				throw new ArgumentException("cannot start with a literal");

			current.append(tagName.Trim('\''));
		}
	}
}
