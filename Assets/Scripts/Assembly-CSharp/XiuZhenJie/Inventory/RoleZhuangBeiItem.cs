using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiItem : MonoBehaviour
	{
		public Button ItemBtn;

		public Text ItemNameText;

		public Image ItemLevelImg;

		public Text ItemLevelText;

		public Image ItemBgImg;

		public BaseNaJieGoods GoodsDataSource;

		public void SetItem(BaseNaJieGoods dataSource, Dictionary<int, Sprite> LevelSpriteDic, Dictionary<WuXing, Sprite> WuXingSpriteDic, UnityAction<int, object> UseBtnClick)
		{
			GoodsDataSource = dataSource;
			if (dataSource != null)
			{
				Sprite value = null;
				WuXingSpriteDic.TryGetValue((WuXing)dataSource.p, out value);
				Sprite value2 = null;
				int levelByRank = WorldSetting.GetLevelByRank(dataSource.c);
				LevelSpriteDic.TryGetValue(levelByRank, out value2);
				ItemBgImg.sprite = value;
				ItemLevelImg.sprite = value2;
				ItemLevelText.text = dataSource.level.ToString();
				ItemNameText.text = dataSource.e;
				InitPanel(UseBtnClick);
			}
		}

		private void InitPanel(UnityAction<int, object> UseBtnClick)
		{
			NaJieTipModel _tipeModel = new NaJieTipModel();
			string wuPinIsCanUse = WorldSetting.GetWuPinIsCanUse(GoodsDataSource.k);
			if (wuPinIsCanUse == "法器")
			{
				_tipeModel.MiddleBtnText = "穿戴";
				UseBtnClick = (int _type, object obj) =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请在配置中设置法器");
				};
			}
			else
			{
				_tipeModel.MiddleBtnText = wuPinIsCanUse;
			}
			if ((wuPinIsCanUse == "法器" || wuPinIsCanUse == "穿戴") && GoodsDataSource != null && string.IsNullOrEmpty(GoodsDataSource.l))
			{
				UseBtnClick = (int _type, object obj) =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请先在纳戒中滴血认主");
				};
			}
			_tipeModel.JiaGe = (int)GoodsDataSource.g;
			_tipeModel.LeftBtnText = "贩卖";
			_tipeModel.LeftBtnClickEvent = null;
			_tipeModel.MiddleBtnClickEvent = UseBtnClick;
			_tipeModel.Name = GoodsDataSource.e;
			_tipeModel.ContentList = GoodsDataSource.q;
			_tipeModel.type = GoodsDataSource.k;
			_tipeModel.dataSource = GoodsDataSource;
			ItemBtn.onClick.RemoveAllListeners();
			ItemBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, _tipeModel);
			});
		}
	}
}
