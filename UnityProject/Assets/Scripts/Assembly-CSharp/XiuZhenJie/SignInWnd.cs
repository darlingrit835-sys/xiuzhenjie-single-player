using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Google.Protobuf;
using LitJson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class SignInWnd : Window
	{
		public SignInPanel _panel;

		private RoleVO currentRoleVo;

		private SignInStatus signInStatus;

		private string m_ServerName = "";

		public override void Awake(params object[] paralist)
		{
			_panel = base.GameObject.GetComponent<SignInPanel>();
			AddButtonClickListener(_panel.SignInBtn, SignInBtnClick);
			AddButtonClickListener(_panel.SignUpBtn, SignUpBtnClick);
			AddButtonClickListener(_panel.GongGaoBtn, GongGaoBtnClick);
			AddButtonClickListener(_panel.ForgetPasBtn, ForgetPasWndShow);
			AddButtonClickListener(_panel.TongYiBtn, TongYiGongShi);
			AddButtonClickListener(_panel.JuJueBtn, JuJueGongShi);
			_panel.InputRect.gameObject.SetActive(value: true);
			string text = ObscuredPrefs.GetString("yonghuming");
			string text2 = ObscuredPrefs.GetString("mima");
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text))
			{
				_panel.UserNameFiled.text = text;
				_panel.PassWordFiled.text = text2;
				Singleton<UIManager>.Instance.m_Mono.StartCoroutine(webrequest(text, text2, isShowGongGao: true));
			}
		}

		private void ForgetPasWndShow()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ForgetPasWnd.prefab", true);
		}

		private void SignUpBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("SignUpWnd.prefab", true);
		}

		public void SignInBtnClick()
		{
			if (string.IsNullOrEmpty(_panel.UserNameFiled.text))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入手机号");
			}
			else if (string.IsNullOrEmpty(_panel.PassWordFiled.text))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入密码");
			}
			else
			{
				Singleton<UIManager>.Instance.m_Mono.StartCoroutine(webrequest(_panel.UserNameFiled.text, _panel.PassWordFiled.text, isShowGongGao: true));
			}
		}

		public void SignIn(string result)
		{
			if (!string.IsNullOrEmpty(result))
			{
				string[] array = result.Split(';');
				if (array.Length >= 3)
				{
					Singleton<UIManager>.Instance.m_Mono.StartCoroutine(webrequest(array[0].ToString(), array[1].ToString(), array[2].ToString()));
				}
			}
		}

		private void IsShowGongGao()
		{
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(IsGongGaoShow());
		}

		private void GongGaoBtnClick()
		{
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(GongGaoShow());
		}

		private IEnumerator GongGaoShow()
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("version", "1.3.5");
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/announcement/queryInfo", dictionary);
			yield return www.SendWebRequest();
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				switch (status)
				{
				case HttpsRequestStatus.ServerError:
					MonoSingleton<GameStart>.Instance.PopUpAttention(content);
					break;
				case HttpsRequestStatus.SuccessFul:
					Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, sm.msgBody.bizInfo.dataString);
					break;
				}
			});
		}

		private IEnumerator IsGongGaoShow()
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("version", "1.3.5");
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/announcement/isShow", dictionary);
			yield return www.SendWebRequest();
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				if (status != HttpsRequestStatus.ServerError && status == HttpsRequestStatus.SuccessFul && !string.IsNullOrEmpty(sm.msgBody.bizInfo.dataString) && sm.msgBody.bizInfo.dataString == "true")
				{
					GongGaoBtnClick();
				}
			});
		}

		private IEnumerator webrequest(string loginId, string sign, string time)
		{
			signInStatus = SignInStatus.SignInIng;
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("loginId", loginId);
			wWWForm.AddField("sign", sign);
			wWWForm.AddField("timestamp", time);
			wWWForm.AddField("type", 2);
			wWWForm.AddField("clientVersion", "1.3.5");
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/channel/login", wWWForm);
			yield return www.SendWebRequest();
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
				{
					switch (status)
					{
					case HttpsRequestStatus.ServerError:
						signInStatus = SignInStatus.None;
						MonoSingleton<GameStart>.Instance.PopUpAttention(content);
						break;
					case HttpsRequestStatus.SuccessFul:
						LoginSuccess(sm.msgBody.bizInfo.dataString);
						IsShowGongGao();
						break;
					default:
						signInStatus = SignInStatus.None;
						break;
					}
				});
			}
			else
			{
				signInStatus = SignInStatus.None;
				MonoSingleton<GameStart>.Instance.PopUpAttention("登陆失败,服务器没有响应");
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private IEnumerator webrequest(string userName, string password, bool isShowGongGao = false)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("mobilePhone", userName);
			wWWForm.AddField("password", password);
			wWWForm.AddField("clientVersion", "1.3.5");
			Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true, "正在登錄中");
			UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/login", wWWForm);
			if (Constants.UsesLocalServer)
			{
				www.timeout = 6;
			}
			yield return www.SendWebRequest();
			string text = www.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
				{
					switch (status)
					{
					case HttpsRequestStatus.ServerError:
						MonoSingleton<GameStart>.Instance.PopUpAttention(content);
						break;
					case HttpsRequestStatus.SuccessFul:
						LoginSuccess(sm.msgBody.bizInfo.dataString);
						ObscuredPrefs.SetString("yonghuming", userName);
						ObscuredPrefs.SetString("mima", password);
						if (isShowGongGao)
						{
							IsShowGongGao();
						}
						break;
					}
				});
			}
			else
			{
				string message = Constants.UsesLocalServer
					? $"无法连接本机服务 {Constants.LoginServerIP}:{Constants.LoginServerPort}。请启动本机服务后重试。"
					: "登陆失败,服务器没有响应";
				MonoSingleton<GameStart>.Instance.PopUpAttention(message);
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private void LoginSuccess(string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return;
			}
			LoginModel lm = JsonMapper.ToObject<LoginModel>(result);
			if (lm == null)
			{
				Debug.Log("登录失败");
			}
			object[] paralist = new object[3]
			{
				lm.gameServersInfoList,
				(Action<string, int, GameServerInfo>)((string ip, int port, GameServerInfo serverInfo) =>
				{
					currentRoleVo = null;
					currentRoleVo = serverInfo.currentRoleVO;
					Constants.PayIp = lm.payIp;
					Constants.PayPort = lm.payPort;
					Constants.SocketServerIP = ip;
					Constants.SocketServerPort = port;
					Constants.SocketServerName = ((serverInfo != null) ? serverInfo.name : "");
					if (serverInfo.currentRoleVO != null)
					{
						Singleton<RoleManager>.Instance.Init(lm.mobilePhone, lm.userId, serverInfo.gameServerId, serverInfo.gameServerIpManageId, lm.huoDongList, serverInfo.currentRoleVO.serversRoleId);
					}
					else
					{
						Singleton<RoleManager>.Instance.Init(lm.mobilePhone, lm.userId, serverInfo.gameServerId, serverInfo.gameServerIpManageId, lm.huoDongList);
					}
					Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true, "正在获取角色信息");
					NetManager instance = Singleton<NetManager>.Instance;
					instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
					NetManager instance2 = Singleton<NetManager>.Instance;
					instance2.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(instance2.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
					Singleton<NetManager>.Instance.StartFirstConnect(ip, port);
				}),
				lm.lastGameServersId
			};
			Singleton<UIManager>.Instance.PopUpWnd("ChooseServerWnd.prefab", bTop: true, paralist);
		}

		private void ConnectSuccess(ServerType serverType, bool isRestart)
		{
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			if (currentRoleVo != null)
			{
				GetRoleAndMapInFo();
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
			Singleton<UIManager>.Instance.PopUpWnd("CreateRoleWnd.prefab", true);
		}

		private void InitGameInfo(UnityAction callBack)
		{
		}

		private void GetRoleAndMapInFo()
		{
			JsonMapper.ToJson(new LoginInfoModel
			{
				gameServerIpManageId = Singleton<RoleManager>.Instance.gameServerIpManageId,
				userId = Singleton<RoleManager>.Instance.UserId
			});
			Singleton<AdvertisingManager>.Instance.LoadAd();
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
									BuglyAgent.SetUserId($"roleId:{roleInfo.Id},roleName{roleInfo.Name}");
									Singleton<RoleManager>.Instance.SetValue(roleInfo, list);
									Singleton<ChatManager>.Instance.Init();
									Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, CheckOfflineRequest);
								}
								else
								{
									string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
								}
							}
						});
					}
					else
					{
						Singleton<NetManager>.Instance.NetClose();
						MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleInfo.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention("登录失败，请尝试重新登录");
					}
					Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
				}
			});
		}

		private void CheckOfflineRequest()
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			if (role_Entity != null && role_Entity.PracticeGuideVal == 100)
			{
				Timer.Register(0.5f, OfflineRequest);
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
							OfflineClever = roleOfflineResp_.OfflineClever,
							OfflineVigor = roleOfflineResp_.OfflineVigor,
							IsTopValue = roleOfflineResp_.IsFullTime
						};
						Singleton<UIManager>.Instance.PopUpWnd("LiXianShouYiWnd.prefab", true, offlineEntity);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleOfflineResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public void TongYiGongShi()
		{
		}

		private void JuJueGongShi()
		{
			MonoSingleton<GameRoot>.Instance.ExitApplication();
		}

		public override void OnDisable()
		{
			signInStatus = SignInStatus.None;
			base.OnDisable();
		}
	}
}
