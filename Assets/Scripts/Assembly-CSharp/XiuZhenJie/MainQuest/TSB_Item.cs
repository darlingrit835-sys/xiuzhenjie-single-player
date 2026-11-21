using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class TSB_Item : MonoBehaviour
	{
		public Text TitleText;

		public Text ContentTitleText;

		public Text ContentText;

		public Button LingQuBtn;

		private int id;

		private int type;

		private int currentProgress;

		public void SetItem(int id, int type, string title, string contentTitle, int currentProgress, int maxProgress, bool isCompelete, UnityAction<int, int, TSB_Item> LingQuAction)
		{
			this.id = id;
			this.type = type;
			this.currentProgress = currentProgress;
			TitleText.text = "【" + title + "】";
			ContentTitleText.text = string.Format(contentTitle, maxProgress);
			ContentText.text = currentProgress + "/" + maxProgress;
			LingQuBtn.onClick.RemoveAllListeners();
			LingQuBtn.interactable = isCompelete;
			LingQuBtn.onClick.AddListener(() =>
			{
				LingQuAction?.Invoke(this.id, this.type, this);
			});
		}

		public void ResetItem(int id, int status, string contentTitle, int maxProgress)
		{
			if (status == 2)
			{
				ContentTitleText.text = "";
				ContentText.text = "";
				LingQuBtn.interactable = false;
			}
			this.id = id;
			ContentTitleText.text = string.Format(contentTitle, maxProgress);
			ContentText.text = currentProgress + "/" + maxProgress;
			LingQuBtn.interactable = status == 1;
		}
	}
}
