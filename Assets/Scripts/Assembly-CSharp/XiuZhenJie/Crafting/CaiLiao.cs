using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CaiLiao : MonoBehaviour
	{
		public Image CaiLiaoImage;

		public Text CaiLiaoText;

		public Button ShiQuBtn;

		public CaiLiaoType type;

		private XzjWorldGoodsMaterialsMineralSpaceVO KuangWuSource;

		private XzjWorldGoodsMaterialsPlantsSpaceVO CaoYaoSource;

		private ZhanDouWnd zhanDouWnd;

		private MonsterModel dataSource;

		public void InstanceCaiLiao(MonsterModel dataSource, CaiLiaoType caiLiaoType, Sprite _imageSprite, ZhanDouWnd zhanDouWnd)
		{
		}

		private void InstanceCaoYao(XzjWorldGoodsMaterialsPlantsSpaceVO _source, Sprite sprite)
		{
			CaiLiaoImage.sprite = sprite;
			CaiLiaoText.text = _source.e;
			ShiQuBtn.onClick.AddListener(ShiQuClickEvent);
		}

		private void InstanceKuangWu(XzjWorldGoodsMaterialsMineralSpaceVO _source, Sprite sprite)
		{
			CaiLiaoImage.sprite = sprite;
			CaiLiaoText.text = _source.e;
			ShiQuBtn.onClick.AddListener(ShiQuClickEvent);
		}

		private void ShiQuClickEvent()
		{
		}
	}
}
