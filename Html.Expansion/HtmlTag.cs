using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Html.Expansion
{
	/// <summary>
	/// A nieve implementation of an html tag that uses TagBuilder to render.
	/// </summary>
	public class HtmlTag : IRenderable
	{
		readonly string _name;
		string _id;
		List<string> _classes = new List<string>();
		Dictionary<string, string> _attributes = new Dictionary<string, string>();
		List<IRenderable> _children = new List<IRenderable>();
		HtmlTag _parent;

		public HtmlTag(string name)
		{
			_name = name;
		}

		public HtmlTag(string name, HtmlTag parent)
		{
			_name = name;
			_parent = parent;
		}

		public HtmlTag addClass(string className)
		{
			_classes.Add(className);
			return this;
		}

		public HtmlTag id(string id)
		{
			_id = id;
			return this;
		}

		public HtmlTag attr(string attrName, string attrValue)
		{
			if (_attributes.ContainsKey(attrName))
				_attributes[attrName] = attrValue;
			else
				_attributes.Add(attrName, attrValue);

			return this;
		}

		public HtmlTag data(string attrName, string attrValue)
		{
			return attr("data-" + attrName, attrValue);
		}

		public HtmlTag append(string value)
		{
			_children.Add(new Literal(value));
			return this;
		}

		public HtmlTag appendTag(HtmlTag tag)
		{
			_children.Add(tag);
			return this;
		}

		public HtmlTag appendTag(string shorthand)
		{
			var tags = ExpandShorthand.Do(shorthand);
			_children.Add(tags.Item1);
			return this;
		}

		public HtmlTag begin(string tagName)
		{
			var tag = new HtmlTag(tagName, this);
			appendTag(tag);
			return tag;
		}

		public HtmlTag end()
		{
			return _parent;
		}

		public string tagName()
		{
			return _name;
		}

		public string Render()
		{
			var tag = new TagBuilder(_name);

			if (!string.IsNullOrWhiteSpace(_id))
				tag.Attributes.Add("id", _id);

			if (_classes.Any())
				tag.AddCssClass(string.Join(" ", _classes.Distinct().ToArray()));

			_attributes.Keys.ToList().ForEach(k => tag.Attributes.Add(k, _attributes[k]));

			if (_children.Any())
				tag.InnerHtml = string.Join(" ", _children.Select(c => c.Render()));

			return tag.ToString();
		}
	}
}