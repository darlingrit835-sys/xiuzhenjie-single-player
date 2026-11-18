using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GameStart : MonoSingleton<GameStart>
	{
		private GameObject m_obj;

		private string m_ServerMaintenanceText = "当前服务器维护中~!";

		private UIManager uiMag;

		public Text TimeText;

		public MonoBehaviour m_mono;

		public TextAsset SensitiveWordsText;

		public string[] SensitiveWords;

		public GameObject WaitRequestAni;

		[SerializeField] private Material[] recoveredBundleMaterials;

		private bool m_LoadCompelete;

		private GameObject m_VerticalRollingText;

		private Transform m_HorizontalRollingBg;

		private Text m_HorizontalRollingText;

		private bool m_IsHorizontalRolling;

		private RectTransform UIRoot;

		private Transform WndRoot;

		private Transform WndRootFont;

		private bool m_IsReconnecting;

		private bool m_IsBattleReconnecting;

		private bool m_IsChatReconnecting;

		private int m_ReConnectTimes;

		private int m_MaxReConnectTimes = 5;

		private int m_BattleReConnectTimes;

		private int m_BattleMaxReConnectTimes = 1;

		private int m_ChatServerReConnectTimes;

		private int m_ChatServerMaxReConnectTimes = 5;

		private Queue<string> RollingTextQue = new Queue<string>();

		private Queue<PopUpWndEntity> PopUpTextQue = new Queue<PopUpWndEntity>();

		private Queue<string> m_HorizontalRollingTextQue = new Queue<string>();

		private string m_WindowPath = "Assets/GameData/Prefabs/UIPrefabs/Window/";

		public bool IsCanShowLiXianShouYi;

		private float m_RollingTextQueEveryTime;

		private float m_PopTextQueEveryTime = 2f;

		private AttentionWnd wnd;

		protected override void Awake()
		{
			base.Awake();
			m_mono = this;
			string text = Resources.Load("SensitiveWords").ToString();
			SensitiveWords = text.Split(',');
			uiMag = Singleton<UIManager>.Instance;
			Singleton<ResourceManager>.Instance.Init(this);
			Singleton<ObjectManager>.Instance.Init(base.transform.Find("RecyclePoolTrs"), base.transform.Find("SceneTrs"));
			Singleton<HotPatchManager>.Instance.Init(this);
			UIRoot = base.transform.Find("UIRoot") as RectTransform;
			WndRoot = base.transform.Find("UIRoot/WndRoot");
			WndRootFont = base.transform.Find("UIRoot/WndRootFront");
			m_HorizontalRollingBg = base.transform.Find("UIRoot/WndRootFront/Title/ScrollText");
			m_HorizontalRollingText = m_HorizontalRollingBg.Find("Text").gameObject.GetComponent<Text>();
			uiMag.Init(UIRoot, WndRoot.gameObject.GetComponent<RectTransform>(), base.transform.Find("UIRoot/UICamera").GetComponent<Camera>(), base.transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>(), this);
			RegisterUI();
			TimeText.gameObject.SetActive(value: true);
			uiMag.PopUpWnd("StartLoadSceneWnd.prefab", true, true);
		}

		private void Update()
		{
			uiMag?.OnUpdate();
			Singleton<RoleManager>.Instance?.Update();
			if (m_LoadCompelete)
			{
				m_LoadCompelete = false;
			}
			if (RollingTextQue.Count > 0 && m_RollingTextQueEveryTime >= 0.5f)
			{
				m_RollingTextQueEveryTime = 0f;
				if (WndRootFont == null || m_VerticalRollingText == null)
				{
					return;
				}
				GameObject obj = UnityEngine.Object.Instantiate(m_VerticalRollingText, WndRootFont.transform);
				obj.GetComponentInChildren<Text>().text = RollingTextQue.Dequeue();
				obj.transform.SetAsLastSibling();
			}
			m_RollingTextQueEveryTime += Time.deltaTime;
			if (PopUpTextQue.Count > 0 && m_PopTextQueEveryTime >= 2f)
			{
				wnd = Singleton<UIManager>.Instance.FindWndByName<AttentionWnd>("AttentionWnd.prefab");
				if (wnd == null)
				{
					m_PopTextQueEveryTime = 0f;
					PopUpWndEntity popUpWndEntity = PopUpTextQue.Dequeue();
					Singleton<UIManager>.Instance.PopUpWnd("AttentionWnd.prefab", base.transform, true, popUpWndEntity.Content, popUpWndEntity.SureBtnAction, popUpWndEntity.CloseBtnAction);
				}
			}
			m_PopTextQueEveryTime += Time.deltaTime;
			if (!m_IsHorizontalRolling && m_HorizontalRollingTextQue.Count > 0)
			{
				m_IsHorizontalRolling = true;
				string text = m_HorizontalRollingTextQue.Dequeue();
				m_HorizontalRollingText.text = text;
				m_HorizontalRollingText.rectTransform.sizeDelta = new Vector2(text.Length * 14, m_HorizontalRollingText.rectTransform.sizeDelta.y);
				m_HorizontalRollingBg.gameObject.SetActive(value: true);
			}
			if (m_IsHorizontalRolling)
			{
				float num = m_HorizontalRollingText.rectTransform.anchoredPosition.x - Time.deltaTime * 30f;
				m_HorizontalRollingText.rectTransform.anchoredPosition = new Vector2(num, 0f);
				if (num <= 0f - (m_HorizontalRollingText.rectTransform.sizeDelta.x + UIRoot.sizeDelta.x))
				{
					m_IsHorizontalRolling = false;
					m_HorizontalRollingText.rectTransform.anchoredPosition = new Vector2(0f, 0f);
					if (m_HorizontalRollingTextQue.Count <= 0)
					{
						m_HorizontalRollingBg.gameObject.SetActive(value: false);
					}
				}
			}
			TimeTongBu();
		}

		private void FixedUpdate()
		{
			uiMag?.OnFixUpdate();
			Singleton<NetManager>.Instance?.FixUpdate();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		public void StartLogin()
		{
			m_VerticalRollingText = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ScrollingTextItem.prefab");
			Singleton<UIManager>.Instance.PopUpWnd("SignInWnd.prefab", false, true);
			Singleton<UIManager>.Instance.CloseWnd("StartLoadSceneWnd.prefab");
			IsCanShowLiXianShouYi = true;
		}

		private void Init()
		{
			Singleton<GuideManager>.Instance.Init();
			Singleton<NetManager>.Instance.Init(this);
			NetManager netManager = Singleton<NetManager>.Instance;
			netManager.NetErrorEvent = (Action<ServerType>)Delegate.Remove(netManager.NetErrorEvent, new Action<ServerType>(NetErrorCallBack));
			NetManager netManager2 = Singleton<NetManager>.Instance;
			netManager2.NetErrorEvent = (Action<ServerType>)Delegate.Combine(netManager2.NetErrorEvent, new Action<ServerType>(NetErrorCallBack));
			NetManager netManager3 = Singleton<NetManager>.Instance;
			netManager3.DisConnectEvent = (Action<ServerType>)Delegate.Remove(netManager3.DisConnectEvent, new Action<ServerType>(DisConnectCallBack));
			NetManager netManager4 = Singleton<NetManager>.Instance;
			netManager4.DisConnectEvent = (Action<ServerType>)Delegate.Combine(netManager4.DisConnectEvent, new Action<ServerType>(DisConnectCallBack));
			NetManager netManager5 = Singleton<NetManager>.Instance;
			netManager5.GameRadioAction = (Action<string>)Delegate.Remove(netManager5.GameRadioAction, new Action<string>(ReceiveRadioMsg));
			NetManager netManager6 = Singleton<NetManager>.Instance;
			netManager6.GameRadioAction = (Action<string>)Delegate.Combine(netManager6.GameRadioAction, new Action<string>(ReceiveRadioMsg));
			NetManager netManager7 = Singleton<NetManager>.Instance;
			netManager7.GameExceptionAction = (Action)Delegate.Remove(netManager7.GameExceptionAction, new Action(ReceiveGameException));
			NetManager netManager8 = Singleton<NetManager>.Instance;
			netManager8.GameExceptionAction = (Action)Delegate.Combine(netManager8.GameExceptionAction, new Action(ReceiveGameException));
		}

		public IEnumerator StartGame(Image image, Text text)
		{
			m_IsReconnecting = false;
			image.fillAmount = 0f;
			yield return null;
			text.text = "加载本地数据... ...";
			AssetBundleManager bundles = Singleton<AssetBundleManager>.Instance;
			if (bundles.LoadAssetBundleConfig())
			{
				RestoreBundleMaterialShaders(bundles);
			}
			Singleton<AdvertisingManager>.Instance.Init();
			Init();
			image.fillAmount = 0.4f;
			yield return null;
			text.text = "加载配置... ...";
			Singleton<FacadeManager>.Instance.Init();
			image.fillAmount = 0.5f;
			yield return null;
			text.text = "初始化场景... ...";
			Singleton<GameMapManager>.Instance.Init(this);
			Singleton<VoiceManager>.Instance.Init(MonoSingleton<GameRoot>.instance.gameObject.GetComponent<AudioSource>(), base.gameObject.GetComponent<AudioSource>());
			image.fillAmount = 0.8f;
			yield return null;
			image.fillAmount = 1f;
			StartLogin();
		}

		private void RestoreBundleMaterialShaders(AssetBundleManager bundles)
		{
			if (recoveredBundleMaterials == null || recoveredBundleMaterials.Length == 0)
			{
				return;
			}

			// Keep the shared materials alive for prefabs that load them later.
			uint guideMaterial = Crc32.GetCrc32(Constants.GUIDE_Material);
			AssetBundle materialBundle = bundles.LoadResouceAssetBundle(guideMaterial)?.m_AssetBundle;
			if (materialBundle == null)
			{
				return;
			}

			foreach (Material fallback in recoveredBundleMaterials)
			{
				Material material = materialBundle.LoadAsset<Material>(fallback.name);
				if (material != null && (material.shader == null || !material.shader.isSupported || material.shader.name == "Hidden/InternalErrorShader"))
				{
					material.shader = fallback.shader;
				}
			}
		}

		public IEnumerator GetGameInitInfo()
		{
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/client/data/queryClientDataModel", null);
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				PopUpAttention("初始化游戏数据失败:" + www.isHttpError + www.isNetworkError);
				Debug.Log(www.error);
				yield break;
			}
			HttpsRequest.DealwithShellModel(www.downloadHandler.text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				switch (status)
				{
				case HttpsRequestStatus.ServerError:
					PopUpAttention(content);
					Debug.Log(content);
					break;
				case HttpsRequestStatus.SuccessFul:
					try
					{
						JsonMapper.ToObject<GameInitInfoModel>(sm.msgBody.bizInfo.dataString);
						break;
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
						break;
					}
				}
			});
		}

		public void ReceiveRadioMsg(string msg)
		{
			if (msg.Equals("^_^踢人"))
			{
				PopUpAttention("服务器维护中!", ClearDataSource, ClearDataSource);
			}
			else
			{
				m_HorizontalRollingTextQue.Enqueue(msg);
			}
		}

		public void ReceiveGameException()
		{
			PopUpAttention("检测到游戏内出现异常现象！", () =>
			{
				MonoSingleton<GameRoot>.Instance.ExitApplication();
			});
			Timer.Register(3f, () =>
			{
				MonoSingleton<GameRoot>.Instance.ExitApplication();
			});
		}

		public void ShowWaitReauestWnd()
		{
			if (WaitRequestAni != null && !WaitRequestAni.gameObject.activeSelf)
			{
				WaitRequestAni.gameObject.SetActive(value: true);
			}
		}

		public void HideWaitReauestWnd()
		{
			if (WaitRequestAni != null && WaitRequestAni.gameObject.activeSelf)
			{
				WaitRequestAni.gameObject.SetActive(value: false);
			}
		}

		public string GetHintMsgByCode(int code)
		{
			if (code == 0 || code == 5000)
			{
				return null;
			}
			string value = "";
			Singleton<FacadeManager>.Instance.m_CodeMsg.TryGetValue(code, out value);
			return string.IsNullOrEmpty(value) ? ("Error Code : " + code) : value;
		}

		public void PopRollingTextShow(string msg)
		{
			RollingTextQue.Enqueue(msg);
		}

		public void PopUpAttention(string _textValue, Action _action = null, Action _closeAction = null)
		{
			if (_textValue != null)
			{
				AttentionWnd attentionWnd = Singleton<UIManager>.Instance.FindWndByName<AttentionWnd>("AttentionWnd.prefab");
				if (attentionWnd != null)
				{
					attentionWnd.ChangeValue(_textValue, _action, _closeAction);
					return;
				}
				Singleton<UIManager>.Instance.PopUpWnd("AttentionWnd.prefab", base.transform, true, _textValue, _action, _closeAction);
			}
		}

		public void XiaoWenHao(string _text, string _Name, int _ciShui = 1)
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", base.transform, true, _text, _Name);
		}

		public bool IsCanUseString(string words)
		{
			if (SensitiveWords != null)
			{
				string[] sensitiveWords = SensitiveWords;
				foreach (string value in sensitiveWords)
				{
					if (words.Contains(value) && !string.IsNullOrEmpty(value))
					{
						return false;
					}
				}
			}
			return true;
		}

		private void RegisterUI()
		{
			uiMag.Register<ForgetPasWnd>("ForgetPasWnd.prefab");
			uiMag.Register<JingShiWnd>("JingShiWnd.prefab");
			uiMag.Register<MaskWnd>("MaskWnd.prefab");
			uiMag.Register<StartLogoWnd>("StartLogoWnd.prefab");
			uiMag.Register<StartLoadSceneWnd>("StartLoadSceneWnd.prefab");
			uiMag.Register<GuideWnd>("GuideWnd.prefab");
			uiMag.Register<SignInWnd>("SignInWnd.prefab");
			uiMag.Register<SignUpWnd>("SignUpWnd.prefab");
			uiMag.Register<CreateRoleWnd>("CreateRoleWnd.prefab");
			uiMag.Register<CeHuaWnd>("CeHuaWnd.prefab");
			uiMag.Register<ChengZhuFuMainWnd>("ChengZhuFuWnd.prefab");
			uiMag.Register<ChuanSongDaZhenWnd>("ChuanSongDaZhenWnd.prefab");
			uiMag.Register<DiHuoMiShiWnd>("DiHuoMiShiWnd.prefab");
			uiMag.Register<DiHuoGeCaiLiaoChooseWnd>("DiHuoGeCaiLiaoChooseWnd.prefab");
			uiMag.Register<DiHuoGeCaiLiaoShaiXuanWnd>("DiHuoGeCaiLiaoShaiXuanWnd.prefab");
			uiMag.Register<DiHuoGeCaiLiaoShaiXuan_LK_Wnd>("DiHuoGeCaiLiaoShaiXuan_LK_Wnd.prefab");
			uiMag.Register<MainCityWnd>("MainCityWnd.prefab");
			uiMag.Register<MainTitleWnd>("MainTitleWnd.prefab");
			uiMag.Register<WaiChuLiLianWnd>("WaiChuLiLianWnd.prefab");
			uiMag.Register<ZhongJieFangShiWnd>("ZhongJieFangShiWnd.prefab");
			uiMag.Register<YouJianDongFuWnd>("YouJianDongFuWnd.prefab");
			uiMag.Register<ChengNeiMiJingWnd>("ChengNeiMiJingWnd.prefab");
			uiMag.Register<PopUpWnd>("PopUpWnd.prefab");
			uiMag.Register<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
			uiMag.Register<LoadingWnd>("LoadingWnd.prefab");
			uiMag.Register<AttentionWnd>("AttentionWnd.prefab");
			uiMag.Register<ChuanXinWnd>("ChuanXinWnd.prefab");
			uiMag.Register<DuiHuanLingYuWnd>("DuiHuanLingYuWnd.prefab");
			uiMag.Register<DuiHuanLingShiWnd>("DuiHuanLingShiWnd.prefab");
			uiMag.Register<OptionWnd>("OptionWnd.prefab");
			uiMag.Register<DongFuWnd>("DongFuWnd.prefab");
			uiMag.Register<DiHuoGeLianDanJieSuanWnd>("DiHuoGeLianDanJieSuanWnd.prefab");
			uiMag.Register<GengHuanGongFaWnd>("GengHuanGongFaWnd.prefab");
			uiMag.Register<KongLingFaZhenWnd>("KongLingFaZhenWnd.prefab");
			uiMag.Register<JuLingFaZhenWnd>("JuLingFaZhenWnd.prefab");
			uiMag.Register<FaZhenXiaoHaoLingShiWnd>("FaZhenXiaoHaoLingShiWnd.prefab");
			uiMag.Register<GengHuanFaZhenDengJielWnd>("GengHuanFaZhenDengJieWnd.prefab");
			uiMag.Register<DongFuChooseWnd>("DongFuChooseWnd.prefab");
			uiMag.Register<CanWuWnd>("CanWuWnd.prefab");
			uiMag.Register<CanWuZhongWnd>("CanWuZhongWnd.prefab");
			uiMag.Register<HuaFuWnd>("HuaFuWnd.prefab");
			uiMag.Register<HuaFuChoose_LK_Wnd>("HuaFuChoose_LK_Wnd.prefab");
			uiMag.Register<QiZhenGeWnd>("QiZhenGeWnd.prefab");
			uiMag.Register<QZGItemInformationWnd>("QZG_ItmeInformation.prefab");
			uiMag.Register<QiZhenGeGoodsInformationWnd>("QZG_GoodsInformationWnd.prefab");
			uiMag.Register<QiZhenGeShaiXuanWnd>("QZG_ShaiXuan.prefab");
			uiMag.Register<QiZhenGeChooseWnd>("QZG_ChooseWnd.prefab");
			uiMag.Register<QiZhenGeWoDeJingPaiWnd>("QZG_WoDeJingPaiWnd.prefab");
			uiMag.Register<QiZhenGeWoDePaiMaiWnd>("QZG_WoDePaiMaiWnd.prefab");
			uiMag.Register<JingMaiWnd>("JingMaiWnd.prefab");
			uiMag.Register<ChooseServerWnd>("ChooseServerWnd.prefab");
			uiMag.Register<NetMessageWnd>("NetMessageWnd.prefab");
			uiMag.Register<QiZhenGePaiMaiWnd>("QZG_PaiMaiWnd.prefab");
			uiMag.Register<QZGPaiMaiInformationWnd>("QZG_PaiMaiWuPinXinXiWnd.prefab");
			uiMag.Register<ShangPuWnd>("ShangPuWnd.prefab");
			uiMag.Register<ChengZhuFuChengWeiZhiWeiWnd>("ChengZhuFuChengWeiKeQingWnd.prefab");
			uiMag.Register<NaJieTipWnd>("NaJieTipWnd.prefab");
			uiMag.Register<DiHuoGeWnd>("DiHuoGeWnd.prefab");
			uiMag.Register<DiHuoGeLianZhiBuZhouSheZhiWnd>("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab");
			uiMag.Register<DiHuoGeKaiShiLianZhiWnd>("DiHuoGeLianZhiWnd.prefab");
			uiMag.Register<CaiLiaoShaiXuanWnd>("CaiLiaoXuanZeShaiXuanWnd.prefab");
			uiMag.Register<ZhanDouJieGuoWnd>("ZhaoDouJieGuo.prefab");
			uiMag.Register<ZhanDouDiaoLuoWnd>("GuaiWuDiaoLuo.prefab");
			uiMag.Register<QiZhenGePaiMaiWnd>("QZG_PaiMaiWnd.prefab");
			uiMag.Register<QZGPaiMaiInformationWnd>("QZG_PaiMaiWuPinXinXiWnd.prefab");
			uiMag.Register<ShangPuWnd>("ShangPuWnd.prefab");
			uiMag.Register<ShangPinXiangQingWnd>("ShangPuWuPinXiangQingWnd.prefab");
			uiMag.Register<XuShenDianWnd>("XuShenDianWnd.prefab");
			uiMag.Register<XuShenJieWnd>("XuShenJieWnd.prefab");
			uiMag.Register<XuShenTaWnd>("XuShenTaWnd.prefab");
			uiMag.Register<XuShenTaBangDanWnd>("XuShenTaBangDanWnd.prefab");
			uiMag.Register<DanShiTaWnd>("DanShiTaWnd.prefab");
			uiMag.Register<DST_RenZhengWnd>("DST_DanShiRenZhengWnd.prefab");
			uiMag.Register<DST_ZhuCeWnd>("DST_ZhuCeWnd.prefab");
			uiMag.Register<DST_DanFangDuiHuanWnd>("DST_DanFangDuiHuanWnd.prefab");
			uiMag.Register<DST_DanFangDuiHuanShaiXuanWnd>("DST_DanFangDuiHuanShaiXuanWnd.prefab");
			uiMag.Register<DST_ChuangTaWnd>("DST_ChuangTaWnd.prefab");
			uiMag.Register<ChuanSongTiShiWnd>("ChuanSongTiShiWnd.prefab");
			uiMag.Register<WorldMapWnd>("WorldMapWnd.prefab");
			uiMag.Register<LiXianShouYiWnd>("LiXianShouYiWnd.prefab");
			uiMag.Register<NaJieOneKeySoleWnd>("NaJieOneKeySoleWnd.prefab");
			uiMag.Register<KeyBoardWnd>("KeyBoardWnd.prefab");
			uiMag.Register<ChuWuXiangWnd>("ChuWuXiangWnd.prefab");
			uiMag.Register<ChongShenWnd>("ChongShengWnd.prefab");
			uiMag.Register<WorldCaiLiaoWnd>("WorldCaiLiaoWnd.prefab");
			uiMag.Register<DST_ZhuCeTipWnd>("DST_ZhuCeTipWnd.prefab");
			uiMag.Register<ChuangZhao_LianZhiDengJieWnd>("ChuangZhao_LianZhiDengJieWnd.prefab");
			uiMag.Register<DST_DanFangDuiHuanShaiXuanWnd>("DST_DanFangDuiHuanShaiXuanWnd.prefab");
			uiMag.Register<DST_DanFangDuiHuanTipWnd>("DST_DanFangDuiHuanTipWnd.prefab");
			uiMag.Register<TuPoDuJieWnd>("TuPoDuJieWnd.prefab");
			uiMag.Register<TuPoDuJieSuccessWnd>("TuPoDuJieSuccessWnd.prefab");
			uiMag.Register<TuPoDuJieFailWnd>("TuPoDuJieFailWnd.prefab");
			uiMag.Register<ChuangZhaoGongFaWnd>("ChuangZhaoGongFaWnd.prefab");
			uiMag.Register<ChuangZhaoGongFaJieGuoWnd>("ChuangZhaoGongFaJieGuoWnd.prefab");
			uiMag.Register<DaoJingShengDianWnd>("DaoJingShengDianWnd.prefab");
			uiMag.Register<DJSD_GongFaDuiHuanWnd>("DJSD_GongFaDuiHuanWnd.prefab");
			uiMag.Register<DJSD_GongFaDuiHuanShaiXuanWnd>("DJSD_GongFaDuiHuanShaiXuanWnd.prefab");
			uiMag.Register<DJSD_GongFaDuiHuanTipWnd>("DJSD_GongFaDuiHuanTipWnd.prefab");
			uiMag.Register<DJSD_YiZhuCeListWnd>("DJSD_YiZhuCeListWnd.prefab");
			uiMag.Register<WanFaGeWnd>("WanFaGeWnd.prefab");
			uiMag.Register<WFG_FaShuDuiHuanWnd>("WFG_FaShuDuiHuanWnd.prefab");
			uiMag.Register<WFG_FaShuDuiHuanShaiXuanWnd>("WFG_FaShuDuiHuanShaiXuanWnd.prefab");
			uiMag.Register<WFG_FaShuDuiHuanTipWnd>("WFG_FaShuDuiHuanTipWnd.prefab");
			uiMag.Register<ChuangZaoFaShuWnd>("ChuangZaoFaShuWnd.prefab");
			uiMag.Register<ChuangZhaoFaShuJieGuoWnd>("ChuangZhaoFaShuJieGuoWnd.prefab");
			uiMag.Register<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			uiMag.Register<DongFuZhiFuWnd>("DongFuZhiFuWnd.prefab");
			uiMag.Register<DongFuYongTuChooseWnd>("DongFuYongTuChooseWnd.prefab");
			uiMag.Register<Common_ShaiXuanWnd>("Common_ShaiXuanWnd.prefab");
			uiMag.Register<Common_ZhiFuWnd>("Common_ZhiFuWnd.prefab");
			uiMag.Register<Common_ChooseGoodsNumWnd>("Common_ChooseGoodsNumWnd.prefab");
			uiMag.Register<NeiShiWnd>("NeiShiWnd.prefab");
			uiMag.Register<ZongLanWnd>("ZongLanWnd.prefab");
			uiMag.Register<DongFuMiShiWnd>("DongFuMiShiWnd.prefab");
			uiMag.Register<RoleZhuangBeiWnd>("RoleZhuangBeiWnd.prefab");
			uiMag.Register<BattleWnd>("BattleWnd.prefab");
			uiMag.Register<GuideNewWnd>("GuideNewWnd.prefab");
			uiMag.Register<ZhanDouSettlementWnd>("ZhanDouSettlementWnd.prefab");
			uiMag.Register<ShaoDangWnd>("ShaoDangWnd.prefab");
			uiMag.Register<XuanTianLouWnd>("XuanTianLouWnd.prefab");
			uiMag.Register<XuanTianLouXiangQingWnd>("XuanTianLouXiangQingWnd.prefab");
			uiMag.Register<XuanTianLingWnd>("XuanTianLingWnd.prefab");
			uiMag.Register<GuangGaoWnd>("GuangGaoWnd.prefab");
			uiMag.Register<WuDaoTaiWnd>("WuDaoTaiWnd.prefab");
			uiMag.Register<WuDaoTaiItemWnd>("WuDaoTaiIXiangQingWnd.prefab");
			uiMag.Register<ZhanFaShiXIangQingWnd>("ZhanFaShiXiangQingWnd.prefab");
			uiMag.Register<ZhanFaShiWnd>("ZhanFaShiWnd.prefab");
			uiMag.Register<XiaoWenHaoWnd>("XiaoWenHaoWnd.prefab");
			uiMag.Register<ChooseChengHaoWnd>("ChooseChengHaoWnd.prefab");
			uiMag.Register<DongFuExmpleWnd>("DongFuExmpleWnd.prefab");
			uiMag.Register<TuPoDuJieExmpleWnd>("TuPoDuJieExmpleWnd.prefab");
			uiMag.Register<ZhongShangShowWnd>("ZhongShangShowWnd.prefab");
			uiMag.Register<DiHuoGeCaiLiaoJiaGeWnd>("DiHuoGeCaiLiaoJiaGeWnd.prefab");
			uiMag.Register<DJSD_ZhuCeWnd>("DJSD_ZhuCeWnd.prefab");
			uiMag.Register<DJSD_ZhuCeTipWnd>("DJSD_ZhuCeTipWnd.prefab");
			uiMag.Register<WFG_ZhuCeWnd>("WFG_ZhuCeWnd.prefab");
			uiMag.Register<WFG_ZhuCeTipWnd>("WFG_ZhuCeTipWnd.prefab");
			uiMag.Register<RoleZhuangBeiTipWnd>("RoleZhuangBeiTipWnd.prefab");
			uiMag.Register<Common_TipWnd>("Common_TipWnd.prefab");
			uiMag.Register<Common_QiRenWnd>("Common_QiRenWnd.prefab");
			uiMag.Register<HuaFuChooseWnd>("HuaFuChooseWnd.prefab");
			uiMag.Register<SFH_ShenFuHuiWnd>("SFH_ShenFuHuiWnd.prefab");
			uiMag.Register<SFH_FuShuDuiHuanWnd>("SFH_FuShuDuiHuanWnd.prefab");
			uiMag.Register<SFH_FuShuDuiHuanTipWnd>("SFH_FuShuDuiHuanTipWnd.prefab");
			uiMag.Register<SFH_ZhuCeTipWnd>("SFH_ZhuCeTipWnd.prefab");
			uiMag.Register<SFH_ZhuCeWnd>("SFH_ZhuCeWnd.prefab");
			uiMag.Register<SFH_CreateChooseWnd>("SFH_CreateChooseWnd.prefab");
			uiMag.Register<SFH_CreateWnd>("SFH_CreateWnd.prefab");
			uiMag.Register<SFH_CreateResultWnd>("SFH_CreateResultWnd.prefab");
			uiMag.Register<LingQuanJiuSiWnd>("LingQuanJiuSiWnd.prefab");
			uiMag.Register<DuiHuanLingShiNewWnd>("DuiHuanLingShiNewWnd.prefab");
			uiMag.Register<QianDaoWnd>("QianDaoWnd.prefab");
			uiMag.Register<LT_ZhongZiWnd>("LT_ZhongZiWnd.prefab");
			uiMag.Register<LT_LingTuWnd>("LT_LingTuWnd.prefab");
			uiMag.Register<LT_TuRangGenHuanWnd>("LT_TuRangGenHuanWnd.prefab");
			uiMag.Register<LT_ShoHuoWnd>("LT_ShoHuoWnd.prefab");
			uiMag.Register<LT_JiaoGuanWnd>("LT_JiaoGuanWnd.prefab");
			uiMag.Register<LT_LingYeWnd>("LT_LingYeWnd.prefab");
			uiMag.Register<LZ_ChuangZaoWnd>("LZ_ChuangZaoWnd.prefab");
			uiMag.Register<LZ_ZhuCeTipWnd>("LZ_ZhuCeTipWnd.prefab");
			uiMag.Register<LZ_ZhuCeWnd>("LZ_ZhuCeWnd.prefab");
			uiMag.Register<LZ_MianWnd>("LZ_MianWnd.prefab");
			uiMag.Register<LZ_LingZhiDuiHuanTipWnd>("LZ_LingZhiDuiHuanTipWnd.prefab");
			uiMag.Register<LZ_LingZhiDuiHuanWnd>("LZ_LingZhiDuiHuanWnd.prefab");
			uiMag.Register<LT_LingTianMainWnd>("LT_LingTianMainWnd.prefab");
			uiMag.Register<LZ_CaiLiaoShaiXuanWnd>("LZ_CaiLiaoShaiXuanWnd.prefab");
			uiMag.Register<LZ_WorldCaiLiaoWnd>("LZ_WorldCaiLiaoWnd.prefab");
			uiMag.Register<ZS_ZhuCeTipWnd>("ZS_ZhuCeTipWnd.prefab");
			uiMag.Register<ZS_ZhuCeWnd>("ZS_ZhuCeWnd.prefab");
			uiMag.Register<ZS_MianWnd>("ZS_MianWnd.prefab");
			uiMag.Register<ZS_DuiHuanTipWnd>("ZS_DuiHuanTipWnd.prefab");
			uiMag.Register<ZS_DuiHuanWnd>("ZS_DuiHuanWnd.prefab");
			uiMag.Register<ZS_ChuangZhaoWnd>("ZS_ChuangZhaoWnd.prefab");
			uiMag.Register<ZF_LianZhiWnd>("ZF_LianZhiWnd.prefab");
			uiMag.Register<ZF_Choose_LK_Wnd>("ZF_Choose_LK_Wnd.prefab");
			uiMag.Register<ZF_CreateChooseWnd>("ZF_CreateChooseWnd.prefab");
			uiMag.Register<AH_MainSceneWndl>("AH_MainSceneWnd.prefab");
			uiMag.Register<AH_WoDeGuaPaiWnd>("AH_WoDeGuaPaiWnd.prefab");
			uiMag.Register<AH_WoDePaiMaoWnd>("AH_WoDePaiMaoWnd.prefab");
			uiMag.Register<AH_PaiPinXiangQingWnd>("AH_PaiPinXiangQingWnd.prefab");
			uiMag.Register<AH_PaiMaiHuWnd>("AH_PaiMaiHuWnd.prefab");
			uiMag.Register<AH_PaiPinListWnd>("AH_PaiPinListWnd.prefab");
			uiMag.Register<AH_PaiPingXiangQingWnd>("AH_PaiPingXiangQingWnd.prefab");
			uiMag.Register<DaoYouWnd>("DaoYouWnd.prefab");
			uiMag.Register<DaoYouChatWnd>("DaoYouChatWnd.prefab");
			uiMag.Register<DaoYouChuanWuWnd>("DaoYouChuanWuWnd.prefab");
			uiMag.Register<SheLiZongMenWnd>("SheLiZongMenWnd.prefab");
			uiMag.Register<ZongMenChatWnd>("ZongMenChatWnd.prefab");
			uiMag.Register<ZongMenChengYuanWnd>("ZongMenChengYuanWnd.prefab");
			uiMag.Register<ZongMen_CangBaoGeWnd>("ZongMen_CangBaoGeWnd.prefab");
			uiMag.Register<ZongMenChouJiangEventWnd>("ZongMenChouJiangEventWnd.prefab");
			uiMag.Register<ZongMen_LingTianWnd>("ZongMen_LingTianWnd.prefab");
			uiMag.Register<ZongMen_NeiMenWnd>("ZongMen_NeiMenWnd.prefab");
			uiMag.Register<ZongMen_YeLianWnd>("ZongMen_YeLianWnd.prefab");
			uiMag.Register<ZongMen_LianDanWnd>("ZongMen_LianDanWnd.prefab");
			uiMag.Register<ZongMen_LianQiWnd>("ZongMen_LianQiWnd.prefab");
			uiMag.Register<ZongMen_ZhenFaWnd>("ZongMen_ZhenFaWnd.prefab");
			uiMag.Register<ZongMen_ZhiFuWnd>("ZongMen_ZhiFuWnd.prefab");
			uiMag.Register<ZongMen_GongFangWnd>("ZongMen_GongFangWnd.prefab");
			uiMag.Register<ZongMen_YuShouWnd>("ZongMen_YuShouWnd.prefab");
			uiMag.Register<ZongMen_SpeedUpWnd>("ZongMen_SpeedUpWnd.prefab");
			uiMag.Register<ZongMenWnd>("ZongMenWnd.prefab");
			uiMag.Register<BaiRuShanMenWnd>("BaiRuShanMenWnd.prefab");
			uiMag.Register<ZongMenShouChaoWnd>("ZongMenShouChaoWnd.prefab");
			uiMag.Register<ZongMen_DaDianWnd>("ZongMen_DaDianWnd.prefab");
			uiMag.Register<ZongMen_DaDianXieRenWnd>("ZongMen_DaDianXieRenWnd.prefab");
			uiMag.Register<ZongMen_HuZongDaZhenWnd>("ZongMen_HuZongDaZhenWnd.prefab");
			uiMag.Register<ZongMen_HuZongDaZhenUpgradeWnd>("ZongMen_HuZongDaZhenUpgradeWnd.prefab");
			uiMag.Register<ZongMen_KuangMaiWnd>("ZongMen_KuangMaiWnd.prefab");
			uiMag.Register<ZongMen_WaiWuDianWnd>("ZongMen_WaiWuDianWnd.prefab");
			uiMag.Register<ZongMen_WaiWuDianXiangQingWnd>("ZongMen_WaiWuDianXiangQingWnd.prefab");
			uiMag.Register<ZongMen_YanWuTaiWnd>("ZongMen_YanWuTaiWnd.prefab");
			uiMag.Register<ZongMen_ZhangLaoWnd>("ZongMen_ZhangLaoWnd.prefab");
			uiMag.Register<ZongMen_ZhangLaoRenMingWnd>("ZongMen_ZhangLaoRenMingWnd.prefab");
			uiMag.Register<ZongMen_NeiWuDianWnd>("ZongMen_NeiWuDianWnd.prefab");
			uiMag.Register<ZongMen_ZhiFaDianWnd>("ZongMen_ZhiFaDianWnd.prefab");
			uiMag.Register<TiShengPingJieWnd>("TiShengPingJieWnd.prefab");
			uiMag.Register<ZongMenZhuFengWnd>("ZongMenZhuFengWnd.prefab");
			uiMag.Register<ZongMen_ShiKongMiJingWnd>("ZongMen_ShiKongMiJingWnd.prefab");
			uiMag.Register<CanWuGongFaWnd>("CanWuGongFaWnd.prefab");
			uiMag.Register<ChengZhuFuMainWnd>("ChengZhuFuMainWnd.prefab");
			uiMag.Register<FaQiXiLianWnd>("FaQiXiLianWnd.prefab");
			uiMag.Register<ShiTuToZiWnd>("ShiTuToZiWnd.prefab");
			uiMag.Register<ShiTuWnd>("ShiTuWnd.prefab");
			uiMag.Register<CashGoodsWnd>("CashGoodsWnd.prefab");
			uiMag.Register<HeChengWnd>("HeChengWnd.prefab");
			uiMag.Register<Common_WebWnd>("Common_WebWnd.prefab");
			uiMag.Register<LevelRankWnd>("LevelRankWnd.prefab");
			uiMag.Register<HuiGuiDuiHuanWnd>("HuiGuiDuiHuanWnd.prefab");
			uiMag.Register<ChouJiangEventWnd>("ChouJiangEventWnd.prefab");
			uiMag.Register<JiFenDuiHuanWnd>("JiFenDuiHuanWnd.prefab");
			uiMag.Register<LianQiChooseWnd>("LianQiChooseWnd.prefab");
			uiMag.Register<LianQiCaiLiaoShaiXuanWnd>("LianQiCaiLiaoShaiXuanWnd.prefab");
			uiMag.Register<LianQiCaiLiaoShaiXuan_LK_Wnd>("LianQiCaiLiaoShaiXuan_LK_Wnd.prefab");
			uiMag.Register<LianQiJieSuan_ChengGongWnd>("LianQiJieSuan_ChengGongWnd.prefab");
			uiMag.Register<LianQiJieSuan_ShiBaiWnd>("LianQiJieSuan_ShiBaiWnd.prefab");
			uiMag.Register<LianQi_ChuangZaoMianWnd>("LianQi_ChuangZaoMianWnd.prefab");
			uiMag.Register<LianQi_DuiHuanWnd>("LianQi_DuiHuanWnd.prefab");
			uiMag.Register<LQ_QiTuDuiHuanTipWnd>("LQ_QiTuDuiHuanTipWnd.prefab");
			uiMag.Register<LianQi_ZhuCeWnd>("LianQi_ZhuCeWnd.prefab");
			uiMag.Register<LQ_ZhuCeTipWnd>("LQ_ZhuCeTipWnd.prefab");
			uiMag.Register<LianQiFangWnd>("LianQiFangWnd.prefab");
			uiMag.Register<LianQiCreateChooseStageWnd>("LianQiCreateChooseStageWnd.prefab");
			uiMag.Register<LianQiBuZhouSheZhiWnd>("LianQiBuZhouSheZhiWnd.prefab");
			uiMag.Register<LianQiCaiLiaoXuanZeShaiXuanWnd>("LianQiCaiLiaoXuanZeShaiXuanWnd.prefab");
			uiMag.Register<LianQiCreateResultWnd>("LianQiCreateResultWnd.prefab");
			uiMag.Register<XuShenJiTanWnd>("XuShenJiTanWnd.prefab");
			uiMag.Register<HeDaoTaiWnd>("HeDaoTaiWnd.prefab");
		}

		private void PreLoadWnd()
		{
			Singleton<ResourceManager>.Instance.PreloadRes(m_WindowPath + "WorldMapWnd.prefab");
		}

		private void NetErrorCallBack(ServerType serverType)
		{
			switch (serverType)
			{
			case ServerType.GamesNormalServer:
				ErrorDealWith();
				return;
			case ServerType.ChatServer:
				ChatSocketReConnect();
				return;
			}
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "FightingScene")
			{
				BattleSocketReConnect();
			}
		}

		private void DisConnectCallBack(ServerType serverType)
		{
			switch (serverType)
			{
			case ServerType.GamesNormalServer:
				ErrorDealWith();
				return;
			case ServerType.ChatServer:
				ChatSocketReConnect();
				return;
			}
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "FightingScene")
			{
				BattleSocketReConnect();
			}
		}

		private void ErrorDealWith()
		{
			GetServerStatusCallBack();
		}

		private void GetServerStatusCallBack()
		{
			HideWaitReauestWnd();
			if (m_IsReconnecting)
			{
				return;
			}
			m_IsReconnecting = true;
			if (string.IsNullOrEmpty(Singleton<GameMapManager>.Instance.CurrentMapName) || Singleton<GameMapManager>.Instance.CurrentMapName == "StartLoadScene" || Singleton<GameMapManager>.Instance.CurrentMapName == "LoginScene")
			{
				NetMessageWnd obj = Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true) as NetMessageWnd;
				obj.ChangeValue("无法连接到服务器");
				obj.AddCloseCallBack(() =>
				{
					m_IsReconnecting = false;
				});
				Singleton<NetManager>.Instance.NetClose();
				Singleton<RoleManager>.Instance.ClearData();
			}
			else
			{
				Timer.Register(1f, () =>
				{
					SocketReConnect();
				});
			}
		}

		private void SocketReConnect()
		{
			if (m_ReConnectTimes <= m_MaxReConnectTimes)
			{
				ReConnect();
			}
			else if (Singleton<GameMapManager>.Instance?.CurrentMapName == "FightingScene")
			{
				m_IsReconnecting = false;
				m_ReConnectTimes = 1;
				ReConnect();
			}
			else
			{
				ComeBackToLoginScene("很抱歉,无法连接服务器");
			}
		}

		private void BattleSocketReConnect()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "FightingScene" || m_IsBattleReconnecting)
			{
				return;
			}
			if (m_BattleReConnectTimes <= m_BattleMaxReConnectTimes)
			{
				BattleReConnect();
				return;
			}
			PopUpAttention("很抱歉,无法连接至战斗服务器,是否重连", () =>
			{
				m_IsBattleReconnecting = false;
				m_BattleReConnectTimes = 1;
				BattleReConnect();
			}, () =>
			{
				Singleton<BattleManager>.Instance.ExitBattle(isExitScene: false);
				ClearDataSource();
			});
		}

		private void ChatSocketReConnect()
		{
			if (!m_IsChatReconnecting)
			{
				if (m_ChatServerReConnectTimes <= m_ChatServerMaxReConnectTimes)
				{
					ChatReConnect();
					return;
				}
				m_IsChatReconnecting = false;
				m_ChatServerReConnectTimes = 1;
				Singleton<NetManager>.Instance.NetClose(ServerType.ChatServer);
			}
		}

		private void ReConnect()
		{
			Singleton<NetManager>.Instance.NetClose();
			NetManager netManager = Singleton<NetManager>.Instance;
			netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectSuccess));
			Singleton<NetManager>.Instance.ReConnect();
			m_ReConnectTimes++;
			Timer.Register(2f, () =>
			{
				NetSvc value = null;
				if (Singleton<NetManager>.Instance.ConnectedSocket.TryGetValue(ServerType.GamesNormalServer, out value))
				{
					if (value.IsConnect)
					{
						Debug.Log("已经重连上了");
					}
					else
					{
						SocketReConnect();
					}
				}
			});
		}

		private void BattleReConnect()
		{
			m_IsBattleReconnecting = true;
			Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
			NetManager netManager = Singleton<NetManager>.Instance;
			netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectBatlleSuccess));
			Singleton<NetManager>.Instance.ReConnect(ServerType.BattleServer);
			m_BattleReConnectTimes++;
			Timer.Register(2f, () =>
			{
				NetSvc value = null;
				m_IsBattleReconnecting = false;
				if (Singleton<NetManager>.Instance.ConnectedSocket.TryGetValue(ServerType.BattleServer, out value))
				{
					if (value.IsConnect)
					{
						Debug.Log("已经重连上了");
					}
					else
					{
						BattleSocketReConnect();
					}
				}
			});
		}

		private void ChatReConnect()
		{
			m_IsChatReconnecting = true;
			Singleton<NetManager>.Instance.NetClose(ServerType.ChatServer);
			NetManager netManager = Singleton<NetManager>.Instance;
			netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectChatSuccess));
			Singleton<NetManager>.Instance.ReConnect(ServerType.ChatServer);
			m_ChatServerReConnectTimes++;
			Timer.Register(2f, () =>
			{
				NetSvc value = null;
				m_IsChatReconnecting = false;
				if (Singleton<NetManager>.Instance.ConnectedSocket.TryGetValue(ServerType.ChatServer, out value))
				{
					if (value.IsConnect)
					{
						Debug.Log("CHAT SERVER已经重连上了");
					}
					else
					{
						ChatSocketReConnect();
					}
				}
			});
		}

		public void ComeBackToLoginScene(string content)
		{
			PopUpAttention(content, ClearDataSource, ClearDataSource);
		}

		public void ClearDataSource()
		{
			Singleton<NetManager>.Instance?.NetClose();
			Singleton<AdvertisingManager>.Instance?.CloseMgr();
			m_IsReconnecting = false;
			m_IsBattleReconnecting = false;
			RollingTextQue.Clear();
			PopUpTextQue.Clear();
			Singleton<GameMapManager>.Instance?.LoadScene("LoadingWnd.prefab", "LoginScene");
			Singleton<RoleManager>.Instance?.ClearData();
			Singleton<ChatManager>.Instance?.ClearData();
		}

		private void ReConnectSuccess(ServerType serverType, bool isRestart)
		{
			if (serverType != ServerType.GamesNormalServer)
			{
				return;
			}
			MonoSingleton<GameStart>.instance.HideWaitReauestWnd();
			m_IsReconnecting = false;
			NetManager netManager = Singleton<NetManager>.Instance;
			netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectSuccess));
			RoleLoginReq_1002002 data = new RoleLoginReq_1002002
			{
				GameServerIpManageId = Singleton<RoleManager>.Instance.gameServerIpManageId,
				RoleId = Singleton<RoleManager>.Instance.currentRoleId,
				LoginIp = CommonMethod.GetIP(ADDRESSFAM.IPv4),
				GameServerId = Singleton<RoleManager>.Instance.ServerId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LOGIN, data, (object _item) =>
			{
				ByteString byteString = _item as ByteString;
				if (!(byteString == null))
				{
					RoleLoginResp_1002002 roleInfo = new RoleLoginResp_1002002();
					roleInfo.MergeFrom(byteString);
					if (roleInfo.IsSuccess)
					{
						ChildrenMapReq_1007003 data2 = new ChildrenMapReq_1007003
						{
							MapId = roleInfo.MapId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data2, (object _obj) =>
						{
							ByteString byteString2 = _obj as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
								childrenMapResp_.MergeFrom(byteString2);
								if (childrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
									{
										MapEntity item = new MapEntity
										{
											Depth = item2.Level,
											FiveElement = item2.FiveElements,
											Id = item2.Id,
											MapName = item2.Name,
											MapType = item2.Type,
											MaxLevel = item2.MaxLevel,
											MinLevel = item2.MinLevel,
											ParentName = item2.ParentName
										};
										list.Add(item);
									}
									Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab");
									Singleton<NetManager>.Instance.SendStrandedMsg();
									Singleton<RoleManager>.Instance.SetValue(roleInfo, list);
									OfflineRequest();
								}
								else
								{
									string hintMsgByCode2 = GetHintMsgByCode(childrenMapResp_.Code);
									PopUpAttention(hintMsgByCode2);
								}
							}
						});
					}
					else
					{
						string hintMsgByCode = GetHintMsgByCode(roleInfo.Code);
						PopUpAttention(hintMsgByCode);
					}
					Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
				}
			});
		}

		private void ReConnectBatlleSuccess(ServerType serverType, bool isRestart)
		{
			if (serverType == ServerType.BattleServer)
			{
				NetManager netManager = Singleton<NetManager>.Instance;
				netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectBatlleSuccess));
				m_IsBattleReconnecting = false;
				m_BattleReConnectTimes = 0;
				if (Singleton<GameMapManager>.Instance.CurrentMapName == "FightingScene")
				{
					Singleton<BattleManager>.Instance.ReConnectBatlle();
				}
			}
		}

		private void ReConnectChatSuccess(ServerType serverType, bool isRestart)
		{
			if (serverType == ServerType.ChatServer)
			{
				NetManager netManager = Singleton<NetManager>.Instance;
				netManager.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(netManager.ConnectSuccessEvent, new Action<ServerType, bool>(ReConnectChatSuccess));
				m_IsChatReconnecting = false;
				m_ChatServerReConnectTimes = 0;
				Singleton<ChatManager>.Instance.JoinRoomChatServer();
			}
		}

		private void OfflineRequest()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.RoleOffline, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleOfflineResp_1002003 roleOfflineResp_ = new RoleOfflineResp_1002003();
					roleOfflineResp_.MergeFrom(byteString);
					if (roleOfflineResp_.IsSuccess)
					{
						OfflineEntity offlineEntity = new OfflineEntity
						{
							Age = roleOfflineResp_.Age,
							OfflineTime = roleOfflineResp_.OfflineTime,
							Perception = roleOfflineResp_.Perception,
							Physique = roleOfflineResp_.Physique,
							SuperAir = roleOfflineResp_.SuperAir,
							IsTopValue = roleOfflineResp_.IsFullTime,
							OfflineClever = roleOfflineResp_.OfflineClever,
							OfflineVigor = roleOfflineResp_.OfflineVigor
						};
						Singleton<UIManager>.Instance.PopUpWnd("LiXianShouYiWnd.prefab", true, offlineEntity);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleOfflineResp_.Code);
						PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public bool ReSetRoleInfo(object _obj)
		{
			if (_obj == null || _obj.ToString() == "false")
			{
				ReGetRoleInfo();
				return false;
			}
			return true;
		}

		public void ReGetRoleInfo()
		{
		}

		private void TimeTongBu()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			TimeText.text = dateTime.ToString("HH:mm");
		}

		public void ErrorLogUpdate(string content)
		{
			StartCoroutine(ErrorLogUp(content));
		}

		private IEnumerator ErrorLogUp(string content)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("errorString", (content == null) ? "" : content);
			wWWForm.AddField("version", "1.3.5".ToString());
			string value = "";
			if (Singleton<RoleManager>.Instance.Role_Entity != null)
			{
				value = Singleton<RoleManager>.Instance.Role_Entity.Id;
			}
			wWWForm.AddField("errorRoleId", value);
			UnityWebRequest unityWebRequest = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/client/log/error", wWWForm);
			yield return unityWebRequest.SendWebRequest();
		}

		private void OnApplicationQuit()
		{
		}
	}
}
