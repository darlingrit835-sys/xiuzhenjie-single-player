using System.Globalization;
using System.Text;

namespace XiuZhenJie
{
	public class FriendEntity
	{
		public string FromRoleId;

		public string Content;

		public string ReceiveTime;

		public long Timestamp;

		internal static string WrapContent(string content)
		{
			if (content.Length <= 14) return content;
			var text = new StringBuilder(content.Length + content.Length / 14);
			var elements = StringInfo.GetTextElementEnumerator(content);
			int column = 0;
			while (elements.MoveNext())
			{
				string glyph = elements.GetTextElement();
				if (column == 14 && glyph != "\n")
				{
					text.Append('\n');
					column = 0;
				}
				text.Append(glyph);
				column = glyph == "\n" ? 0 : column + 1;
			}
			return text.ToString();
		}
	}
}
