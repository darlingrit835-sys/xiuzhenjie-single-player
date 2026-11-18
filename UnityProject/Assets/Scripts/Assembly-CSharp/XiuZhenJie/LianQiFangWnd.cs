using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class LianQiFangWnd : Window
	{
		public LianQiFangPanel Panel;

		private bool m_KuoBtnIsPressed;

		private bool m_SuoBtnIsPressed;

		private ObscuredInt m_MaxProgressValue;

		private ObscuredInt m_BestProgressValue;

		private ObscuredInt m_ChuiDaTimes;

		private ObscuredInt m_MaxChuiDaTimes;

		private QiTuEntity m_DataSource;

		private ObscuredFloat m_ChuiDaProgressValue = 0f;

		private Dictionary<int, Sprite> LianQiTypeDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiFangPanel>();
			LianQiTypeDic = new Dictionary<int, Sprite>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.WenHaoBtn, HelpMsgShow);
			Panel.KuoBtn.OnLongButtonDown.AddListener(KuoBtnPressDown);
			Panel.KuoBtn.OnLongButtonDoUp.AddListener(KuoBtnPressUp);
			Panel.SuoBtn.OnLongButtonDown.AddListener(SuoBtnPressDown);
			Panel.SuoBtn.OnLongButtonDoUp.AddListener(SuoBtnPressUp);
			AddButtonClickListener(Panel.ShiQuBtn, ShiQuBtn);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as QiTuEntity;
			}
			if (m_DataSource != null)
			{
				m_MaxProgressValue = UnityEngine.Random.Range(140, 200);
				m_BestProgressValue = 100;
				m_MaxChuiDaTimes = m_DataSource.MaxChuiLianTimes;
				m_ChuiDaTimes = 0;
				m_ChuiDaProgressValue = 0.25f;
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.TitleText.text = $"已锻打次数：{m_ChuiDaTimes}次 ，可锻打次数：{(int)m_MaxChuiDaTimes - (int)m_ChuiDaTimes}";
			ResourceLoad();
			Singleton<VoiceManager>.Instance.SwitchBgMusic(BackGroundMusicType.LianQi);
		}

		private void ResourceLoad()
		{
			Sprite sprite = null;
			sprite = (FaQiTypeEnum)m_DataSource.Icon switch
			{
				FaQiTypeEnum.JIAN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Jian.png"), 
				FaQiTypeEnum.GONG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Gong.png"), 
				FaQiTypeEnum.CHI => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Chi.png"), 
				FaQiTypeEnum.LING_DANG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_LingDang.png"), 
				FaQiTypeEnum.YUE => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Yue.png"), 
				FaQiTypeEnum.MAO => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Mao.png"), 
				FaQiTypeEnum.BANG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Bang.png"), 
				FaQiTypeEnum.YIN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Yin.png"), 
				FaQiTypeEnum.QIN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Qin.png"), 
				FaQiTypeEnum.ZHUA => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Zhua.png"), 
				FaQiTypeEnum.DAO => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Dao.png"), 
				FaQiTypeEnum.QIANG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Qiang.png"), 
				FaQiTypeEnum.CHUI => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Chui.png"), 
				FaQiTypeEnum.JI => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Ji.png"), 
				FaQiTypeEnum.ZHONG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Zhong.png"), 
				FaQiTypeEnum.DING => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Ding.png"), 
				FaQiTypeEnum.FAN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Fan.png"), 
				FaQiTypeEnum.LING => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Ling.png"), 
				FaQiTypeEnum.ZHU => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Zhu.png"), 
				FaQiTypeEnum.QI => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Qi.png"), 
				FaQiTypeEnum.ZHEN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Zhen.png"), 
				FaQiTypeEnum.FU => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Fu.png"), 
				FaQiTypeEnum.DUN => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Dun.png"), 
				FaQiTypeEnum.ZHUO => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Zhuo.png"), 
				FaQiTypeEnum.SUO => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Suo.png"), 
				FaQiTypeEnum.YI => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Yi.png"), 
				FaQiTypeEnum.SHANG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Shang.png"), 
				FaQiTypeEnum.XUE => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Xue.png"), 
				FaQiTypeEnum.JIA => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Jia.png"), 
				FaQiTypeEnum.CHANG => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Chang.png"), 
				_ => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LianQi/LQ_Yi.png"), 
			};
			Panel.ContentBigImg.sprite = sprite;
			Panel.ContentSmallImg.sprite = sprite;
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_KuoBtnIsPressed || m_SuoBtnIsPressed)
			{
				Panel.ChuiZiImg.fillAmount += Time.fixedDeltaTime * 0.1f;
			}
		}

		private void KuoBtnPressDown()
		{
			m_KuoBtnIsPressed = true;
		}

		private void KuoBtnPressUp()
		{
			m_KuoBtnIsPressed = false;
			KuoShowAni();
		}

		private void SuoBtnPressDown()
		{
			m_SuoBtnIsPressed = true;
		}

		private void SuoBtnPressUp()
		{
			m_SuoBtnIsPressed = false;
			SuoShowAni();
		}

		private void KuoShowAni()
		{
			Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianQiVoice);
			float num = Panel.ChuiZiImg.fillAmount * (float)(int)m_MaxProgressValue;
			float x = Panel.ContentSmallImg.rectTransform.localScale.x;
			float x2 = Panel.ContentBigImg.rectTransform.localScale.x;
			float num2 = x + x2 * (num / 100f);
			if (num2 > x2 * 1.6f)
			{
				num2 = x2 * 1.6f;
			}
			float num3 = num2 / x2;
			if (num3 > 1f)
			{
				num3 = 1f - (num3 - 1f);
			}
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(Change(num3, Panel.ProgressImg.fillAmount, 20));
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = Panel.ContentSmallImg.rectTransform.DOScale(new Vector3(num2, num2, 1f), 2f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(AniCompelete));
			Panel.ProgressImg.DOFillAmount(num3, 2f);
			Panel.KuoBtn.gameObject.SetActive(value: false);
			Panel.SuoBtn.gameObject.SetActive(value: false);
			Panel.ShiQuBtn.gameObject.SetActive(value: false);
			Panel.LiKaiBtn.interactable = false;
			Panel.WenHaoBtn.interactable = false;
		}

		private void SuoShowAni()
		{
			Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianQiVoice);
			float num = Panel.ChuiZiImg.fillAmount * (float)(int)m_MaxProgressValue;
			float x = Panel.ContentSmallImg.rectTransform.localScale.x;
			float x2 = Panel.ContentBigImg.rectTransform.localScale.x;
			float num2 = x - x2 * (num / 100f);
			if (num2 < x2 * 0.25f)
			{
				num2 = x2 * 0.25f;
			}
			float num3 = num2 / x2;
			if (num3 > 1f)
			{
				num3 = 1f - (num3 - 1f);
			}
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(Change(num3, Panel.ProgressImg.fillAmount, 20));
			Panel.ProgressImg.DOFillAmount(num3, 2f);
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = Panel.ContentSmallImg.rectTransform.DOScale(new Vector3(num2, num2, 1f), 2f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(AniCompelete));
			Panel.KuoBtn.gameObject.SetActive(value: false);
			Panel.SuoBtn.gameObject.SetActive(value: false);
			Panel.ShiQuBtn.gameObject.SetActive(value: false);
			Panel.LiKaiBtn.interactable = false;
			Panel.WenHaoBtn.interactable = false;
		}

		private IEnumerator Change(float target, float start, int change_speed)
		{
			float delta = (target - start) / 20f;
			m_ChuiDaProgressValue = start;
			for (int i = 0; i < 20; i++)
			{
				m_ChuiDaProgressValue = (float)m_ChuiDaProgressValue + delta;
				Panel.ProgressText.text = ((float)m_ChuiDaProgressValue * 100f).ToString("0") + "%";
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void AniCompelete()
		{
			ChuiDaRecord();
			++m_ChuiDaTimes;
			Panel.TitleText.text = $"已锻打次数：{m_ChuiDaTimes}次 ，可锻打次数：{(int)m_MaxChuiDaTimes - (int)m_ChuiDaTimes}";
			Panel.ChuiZiImg.fillAmount = 0f;
			Panel.KuoBtn.gameObject.SetActive(value: true);
			Panel.SuoBtn.gameObject.SetActive(value: true);
			Panel.LiKaiBtn.interactable = true;
			Panel.WenHaoBtn.interactable = true;
			if ((float)m_ChuiDaProgressValue >= 0.75f)
			{
				Panel.ShiQuBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.ShiQuBtn.gameObject.SetActive(value: false);
			}
			if ((int)m_ChuiDaTimes > (int)m_MaxChuiDaTimes)
			{
				LianQiFail();
			}
		}

		private void ChuiDaRecord()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.RefiningEquipmentRecord, null, null);
		}

		private void ShiQuBtn()
		{
			if ((float)m_ChuiDaProgressValue < 0.75f)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("完整度75%以下会炼制失败，请继续锤炼！");
				return;
			}
			RefiningEquipmentReq_1005049 data = new RefiningEquipmentReq_1005049
			{
				FractionOfCoverage = int.Parse(((float)m_ChuiDaProgressValue * 100f).ToString("0")),
				IsRefiningSuccess = true,
				MethodEquipmentId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFINING_EQUIPMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RefiningEquipmentResp_1005049 refiningEquipmentResp_ = new RefiningEquipmentResp_1005049();
					refiningEquipmentResp_.MergeFrom(byteString);
					if (refiningEquipmentResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningEquipmentResp_.CurrentVigor;
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(refiningEquipmentResp_.LiveLevel, refiningEquipmentResp_.LiveProficiency, RoleFuZhiType.LianQi);
						BaseNaJieGoodEntity baseNaJieGoodEntity = new BaseNaJieGoodEntity();
						baseNaJieGoodEntity = new BaseNaJieGoodEntity
						{
							Rank = refiningEquipmentResp_.Rank,
							Name = m_DataSource.Name,
							Type = m_DataSource.Type
						};
						Singleton<UIManager>.Instance.PopUpWnd("LianQiJieSuan_ChengGongWnd.prefab", true, m_DataSource.CaiLiaoList, baseNaJieGoodEntity);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refiningEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LianQiFail()
		{
			RefiningEquipmentReq_1005049 data = new RefiningEquipmentReq_1005049
			{
				FractionOfCoverage = int.Parse(((float)m_ChuiDaProgressValue * 100f).ToString("0")),
				IsRefiningSuccess = false,
				MethodEquipmentId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFINING_EQUIPMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RefiningEquipmentResp_1005049 refiningEquipmentResp_ = new RefiningEquipmentResp_1005049();
					refiningEquipmentResp_.MergeFrom(byteString);
					if (refiningEquipmentResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningEquipmentResp_.CurrentVigor;
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(refiningEquipmentResp_.LiveLevel, refiningEquipmentResp_.LiveProficiency, RoleFuZhiType.LianQi);
						Singleton<UIManager>.Instance.PopUpWnd("LianQiJieSuan_ShiBaiWnd.prefab", true, m_DataSource.CaiLiaoList);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refiningEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void HelpMsgShow()
		{
			string text = $"1、每件法器的可锻打次数最大为5次。 2、当匹配度达到75%以上可拾取。 3、匹配度越高锻打次数越少则越接近最大品阶 4、按住扩或缩后蓄力条会开始蓄力，在蓄力合适的时候松开按钮即可。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "法器炼制");
		}

		public override void OnClose()
		{
			base.OnClose();
			Singleton<VoiceManager>.Instance.SwitchBgMusic(BackGroundMusicType.Main);
		}
	}
}
