namespace Html.Expansion
{
	/// <summary>
	/// A piece of text.
	/// </summary>
	internal class Literal : IRenderable
	{
		readonly string _value;

		public Literal(string value)
		{
			_value = value;
		}

		public string Render()
		{
			return _value;
		}
	}
}
