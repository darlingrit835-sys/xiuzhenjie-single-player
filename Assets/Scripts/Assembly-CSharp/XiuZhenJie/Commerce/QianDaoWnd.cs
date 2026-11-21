using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class QianDaoWnd : Window
	{
		public QianDaoPanl Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<QianDaoPanl>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			ResourceLoad();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				SearchSignInAwardResp_1002018 searchSignInAwardResp_ = paralist[0] as SearchSignInAwardResp_1002018;
				GetQianDaoData(searchSignInAwardResp_);
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void GetQianDaoData(SearchSignInAwardResp_1002018 searchSignInAwardResp_1002018)
		{
			RefreshSignInState(searchSignInAwardResp_1002018);
			CreateDataList(searchSignInAwardResp_1002018);
			if (!Singleton<RoleManager>.Instance.Role_Entity.IsGetSignIn && Singleton<RoleManager>.Instance.Role_Entity.SignInDay <= 6)
			{
				LingQuQianDaoShouYi();
			}
		}

		// The trial server includes current progress when the window opens. Older
		// servers still supply the same state in the original role login reply.
		private void RefreshSignInState(SearchSignInAwardResp_1002018 awards)
		{
			int days = -1;
			bool? claimedToday = null;
			var input = awards.ToByteString().CreateCodedInput();
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				if (tag == 32) days = input.ReadInt32();
				else if (tag == 40) claimedToday = input.ReadBool();
				else input.SkipLastField();
			}
			if (days >= 0 && days <= 7 && claimedToday.HasValue)
			{
				var role = Singleton<RoleManager>.Instance.Role_Entity;
				role.SignInDay = days;
				role.IsGetSignIn = claimedToday.Value;
			}
		}

		private void CreateDataList(SearchSignInAwardResp_1002018 searchSignInAwardResp_1002018)
		{
			for (int i = 0; i < searchSignInAwardResp_1002018.SignInAward.Count; i++)
			{
				List<QianDaoItemGoodEntity> list = new List<QianDaoItemGoodEntity>();
				if (i < 6)
				{
					if (searchSignInAwardResp_1002018.SignInAward[i].Award.Count >= 1)
					{
						QianDaoItemGoodEntity item = CreateItem(i, searchSignInAwardResp_1002018.SignInAward[i].Award[0]);
						list.Add(item);
					}
				}
				else
				{
					foreach (SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019.Types.AwardResp_1002020 item3 in searchSignInAwardResp_1002018.SignInAward[i].Award)
					{
						QianDaoItemGoodEntity item2 = CreateItem(i, item3);
						list.Add(item2);
					}
				}
				SetItemsValue(i, list, Panel.QianDaoItems[i]);
			}
		}

		private void SetItemsValue(int index, List<QianDaoItemGoodEntity> dataList, QianDaoItem qianDaoItem)
		{
			qianDaoItem.SetValue(index + 1, index < Singleton<RoleManager>.Instance.Role_Entity.SignInDay, dataList);
		}

		private QianDaoItemGoodEntity CreateItem(int i, SearchSignInAwardResp_1002018.Types.SignInAwardResp_1002019.Types.AwardResp_1002020 Award)
		{
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(Award.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(Award.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			return new QianDaoItemGoodEntity
			{
				GoodsCount = Award.Num,
				GoodsName = Award.GoodsName,
				GoodsRank = levelByRank,
				GoodsRankSprite = value2,
				GoodsType = Award.Type,
				GoodsIcon = value
			};
		}

		private void LingQuQianDaoShouYi()
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			int rewardIndex = role.SignInDay;
			Singleton<NetManager>.Instance.SendMsg(MsgType.GetSignInAward, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GetSignInAwardResp_1002021 getSignInAwardResp_ = new GetSignInAwardResp_1002021();
					getSignInAwardResp_.MergeFrom(byteString);
					if (getSignInAwardResp_.IsSuccess)
					{
						role.IsGetSignIn = true;
						role.SignInDay = Mathf.Max(role.SignInDay, rewardIndex + 1);
						if (Panel != null && Panel.gameObject.activeInHierarchy && rewardIndex >= 0 && rewardIndex < Panel.QianDaoItems.Count)
							Panel.QianDaoItems[rewardIndex].StartAnimation();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(getSignInAwardResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
