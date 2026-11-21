using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class YiCanWuItem : MonoBehaviour
	{
		public Button btn;

		public Button RemoveBtn;

		public Text ItemName;

		public Image LevelImg;

		public Image CanWuImg;

		public Text LevelText;

		private int m_CurrentType;

		public void SetValue(string _name, int level, int type, Sprite LevelTagSprite, Sprite wuXingSprite, UnityAction _btnAction, UnityAction<int> _yiWangAction)
		{
			m_CurrentType = type;
			ItemName.text = _name;
			if (LevelTagSprite != null)
			{
				LevelImg.sprite = LevelTagSprite;
			}
			if (wuXingSprite != null)
			{
				CanWuImg.sprite = wuXingSprite;
			}
			LevelText.text = level.ToString();
			btn.onClick.RemoveAllListeners();
			RemoveBtn.onClick.RemoveAllListeners();
			if (_btnAction != null)
			{
				btn.onClick.AddListener(() =>
				{
					if (_btnAction != null)
					{
						_btnAction();
					}
				});
			}
			if (_yiWangAction == null)
			{
				return;
			}
			RemoveBtn.onClick.AddListener(() =>
			{
				if (_yiWangAction != null)
				{
					_yiWangAction(m_CurrentType);
				}
			});
		}
	}
}
