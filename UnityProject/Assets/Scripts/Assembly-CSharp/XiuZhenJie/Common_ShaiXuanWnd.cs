using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class Common_ShaiXuanWnd : Window
	{
		public Common_ShaiXuanPanel Panel;

		public UnityAction ReChooseAction;

		private RectTransform m_RectTransform;

		private Dictionary<string, List<ShaiXuanParamsModel>> m_ContentDic;

		private Dictionary<string, ShaiXuanParamsModel> m_ContentResult;

		private List<ShaiXuanItemType> UseShaiXuanType;

		private GameObject m_BigItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_ContentDic = new Dictionary<string, List<ShaiXuanParamsModel>>();
			m_ContentResult = new Dictionary<string, ShaiXuanParamsModel>();
			Panel = base.GameObject.GetComponent<Common_ShaiXuanPanel>();
			m_RectTransform = base.GameObject.GetComponent<RectTransform>();
			m_ContentDic = new Dictionary<string, List<ShaiXuanParamsModel>>();
			AddButtonClickListener(Panel.MaskBtn, PopPanel);
			AddButtonClickListener(Panel.QueDingBtn, PopPanel);
			if (paralist.Length == 0)
			{
				return;
			}
			UseShaiXuanType = paralist[0] as List<ShaiXuanItemType>;
			bool isContainAllType = paralist.Length == 2 && (bool)paralist[1];
			if (UseShaiXuanType == null)
			{
				return;
			}
			foreach (ShaiXuanItemType item in UseShaiXuanType)
			{
				switch (item)
				{
				case ShaiXuanItemType.CanWuZhuangTai:
					AddCanWuZhuangTai(isContainAllType);
					break;
				case ShaiXuanItemType.CanWuLeiBie:
					AddCanWuLeiBie(isContainAllType);
					break;
				case ShaiXuanItemType.WuXing:
					AddWuXing(isContainAllType);
					break;
				case ShaiXuanItemType.PinJie:
					AddPinJie(isContainAllType);
					break;
				case ShaiXuanItemType.DengJie:
					AddDengJie(isContainAllType);
					break;
				case ShaiXuanItemType.AllGoodsType:
					AddAllGoodsType(isContainAllType);
					break;
				case ShaiXuanItemType.FaShuType:
					AddFaShuType(isContainAllType);
					break;
				case ShaiXuanItemType.DanFangType:
					AddDanFangType(isContainAllType);
					break;
				case ShaiXuanItemType.GongFaType:
					AddGongFaType(isContainAllType);
					break;
				case ShaiXuanItemType.XuShenTaRank:
					AddXSTPinJie(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_DanYao:
					QZG_DanYaoTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_FaQi:
					QZG_FaQiTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_FaQiType:
					QZG_FaQiTypeTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_FaShu:
					QZG_FaShuTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_FuLu:
					QZG_FuLuTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_GongFa:
					QZG_GongFaTitle(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_QiTa:
					QZG_QiTaTypeTitle(isContainAllType);
					break;
				case ShaiXuanItemType.XSJ_ZhuCe:
					XSJ_ZhuCeState(isContainAllType);
					break;
				case ShaiXuanItemType.XSJ_FaQi:
					QZG_FaQiTypeTitle(isContainAllType);
					AddXSTPinJie(isContainAllType);
					break;
				case ShaiXuanItemType.XSJ_LingZhi:
					AddSiQiType(isContainAllType);
					AddWuWeiType(isContainAllType);
					AddBuXieType(isContainAllType);
					AddGuiJingType(isContainAllType);
					break;
				case ShaiXuanItemType.XSJ_ZhenFa:
					AddXSTPinJie(isContainAllType);
					AddZhenFaType(isContainAllType);
					break;
				case ShaiXuanItemType.QZG_ZhenFa:
					QZG_ZhenFaTypeTitle(isContainAllType);
					QZG_ZhenFaUseTitle(isContainAllType);
					break;
				}
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (m_BigItem == null)
			{
				m_BigItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_ShaiXuanBigItem.prefab");
			}
			float num = 0f;
			foreach (KeyValuePair<string, List<ShaiXuanParamsModel>> item in m_ContentDic)
			{
				Common_ShaiXuanBigItem component = Object.Instantiate(m_BigItem, Panel.ScrollContent).GetComponent<Common_ShaiXuanBigItem>();
				num += component.SetItem(this, item.Key, item.Value, (ShaiXuanParamsModel value) =>
				{
					if (m_ContentResult.ContainsKey(item.Key))
					{
						m_ContentResult[item.Key] = value;
					}
					else
					{
						m_ContentResult.Add(item.Key, value);
					}
				});
			}
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, num);
		}

		private void AddCanWuZhuangTai(bool isContainAllType = false)
		{
			string key = "状态";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "可参悟",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "已参悟",
				ItemTag = "2"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddAllGoodsType(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "功法",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "丹方",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "阵图",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "器图",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "符书",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法术",
				ItemTag = "6"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "动物",
				ItemTag = null
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "植物",
				ItemTag = "12"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "矿物",
				ItemTag = "13"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法器",
				ItemTag = null
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "丹药",
				ItemTag = "19"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "符箓",
				ItemTag = "21"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "工具",
				ItemTag = "25"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "秘境",
				ItemTag = "28"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "福地",
				ItemTag = "32"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddCanWuLeiBie(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "功法",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法术",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "丹方",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "器图",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "符书",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "阵图",
				ItemTag = "6"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddWuXing(bool isContainAllType = false)
		{
			string key = "五行";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "金",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "木",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "水",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "火",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "土",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "其它",
				ItemTag = "0"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddPinJie(bool isContainAllType = false)
		{
			string key = "品阶";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "黄",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "玄",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "地",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "天",
				ItemTag = "4"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddXSTPinJie(bool isContainAllType = false)
		{
			string key = "品阶";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = "0"
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "玄",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "地",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "天",
				ItemTag = "4"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddDengJie(bool isContainAllType = false)
		{
			string key = "等阶";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "练气",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "筑基",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "结丹",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "元婴",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "分神",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "炼虚",
				ItemTag = "6"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "合体",
				ItemTag = "7"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "渡劫",
				ItemTag = "8"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "大乘",
				ItemTag = "9"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddFaShuType(bool isContainAllType = false)
		{
			string key = "法术类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "锁定攻击",
				ItemTag = "10"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "持续攻击",
				ItemTag = "12"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "范围攻击",
				ItemTag = "14"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法术护盾",
				ItemTag = "20"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "伤害增益",
				ItemTag = "30"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "防御增益",
				ItemTag = "31"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "减低移速",
				ItemTag = "40"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "限制行动",
				ItemTag = "41"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "五行遁术",
				ItemTag = "50"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "御剑术",
				ItemTag = "51"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddDanFangType(bool isContainAllType = false)
		{
			string key = "药效";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "恢复体魄",
				ItemTag = "10"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "恢复神识",
				ItemTag = "11"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "恢复真元",
				ItemTag = "12"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "恢复精血",
				ItemTag = "13"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "恢复寿元",
				ItemTag = "14"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长体魄",
				ItemTag = "20"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长神识",
				ItemTag = "21"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长真元",
				ItemTag = "22"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长精血",
				ItemTag = "23"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长寿元",
				ItemTag = "24"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "增长经脉",
				ItemTag = "25"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddGongFaType(bool isContainAllType = false)
		{
			string key = "功法类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "神识",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "体魄",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "真元",
				ItemTag = "3"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddSiQiType(bool isContainAllType = false)
		{
			string key = "四气";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "温",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "热",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "凉",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "寒",
				ItemTag = "4"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddWuWeiType(bool isContainAllType = false)
		{
			string key = "五味";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "辛",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "甘",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "酸",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "苦",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "咸",
				ItemTag = "3"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddBuXieType(bool isContainAllType = false)
		{
			string key = "补泻";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "补",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "泻",
				ItemTag = "2"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddGuiJingType(bool isContainAllType = false)
		{
			string key = "归经";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手厥阴",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手少阴",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足少阳",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手少阳",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足太阳",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足太阴",
				ItemTag = "6"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手太阳",
				ItemTag = "7"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手阳明",
				ItemTag = "8"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "手太阴",
				ItemTag = "9"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足阳明",
				ItemTag = "10"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足厥阴",
				ItemTag = "11"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "足少阴",
				ItemTag = "12"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void AddZhenFaType(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "杀",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "困",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "幻",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "回蓝",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "回血",
				ItemTag = "5"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_GongFaTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "功法",
				ItemTag = "1"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_FaShuTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法术",
				ItemTag = "6"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_FuLuTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "符书",
				ItemTag = "5"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "符箓",
				ItemTag = "21"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_DanYaoTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "丹方",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "丹药",
				ItemTag = "19"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_FaQiTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "法器",
				ItemTag = 14.ToString()
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "器图",
				ItemTag = 4.ToString()
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_FaQiTypeTitle(bool isContainAllType = false)
		{
			string key = "功能类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "攻击法器",
				ItemTag = 15.ToString()
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "防御法器",
				ItemTag = 26.ToString()
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "防具",
				ItemTag = 16.ToString()
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "鞋",
				ItemTag = 29.ToString()
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_FaQiUseTitle(bool isContainAllType = false)
		{
			string key = "法器作用";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "可配置",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "可装备",
				ItemTag = "2"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_QiTaTypeTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "动物表皮",
				ItemTag = "8"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "动物血液",
				ItemTag = "9"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "动物骨骼",
				ItemTag = "10"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "动物妖丹",
				ItemTag = "11"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "植物",
				ItemTag = "12"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "矿物",
				ItemTag = "13"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "秘境地图",
				ItemTag = "28"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "洞天福地",
				ItemTag = "32"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void XSJ_ZhuCeState(bool isContainAllType = false)
		{
			string key = "状态";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = "-1"
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "可注册",
				ItemTag = "0"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "待审核",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "已注册",
				ItemTag = "2"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_ZhenFaTypeTitle(bool isContainAllType = false)
		{
			string key = "类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "阵盘",
				ItemTag = 20.ToString()
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "阵图",
				ItemTag = 3.ToString()
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		private void QZG_ZhenFaUseTitle(bool isContainAllType = false)
		{
			string key = "功能类别";
			List<ShaiXuanParamsModel> list = new List<ShaiXuanParamsModel>();
			if (isContainAllType)
			{
				list.Add(new ShaiXuanParamsModel
				{
					ItemName = "全部",
					ItemTag = null
				});
			}
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "杀阵",
				ItemTag = "1"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "缚灵阵",
				ItemTag = "2"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "幻阵",
				ItemTag = "3"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "复灵阵",
				ItemTag = "4"
			});
			list.Add(new ShaiXuanParamsModel
			{
				ItemName = "活血阵",
				ItemTag = "5"
			});
			if (!m_ContentDic.ContainsKey(key))
			{
				m_ContentDic.Add(key, list);
			}
		}

		public Dictionary<string, ShaiXuanParamsModel> GetResult()
		{
			return m_ContentResult;
		}

		private void PopPanel()
		{
			if (ReChooseAction != null)
			{
				ReChooseAction();
			}
			PopUpPanel(isPop: false);
		}

		public void PopUpPanel(bool isPop)
		{
			_ = m_RectTransform.sizeDelta;
			base.GameObject.transform.SetAsLastSibling();
			if (isPop)
			{
				DOTween.To(() => m_RectTransform.anchoredPosition, (Vector2 v) =>
				{
					m_RectTransform.anchoredPosition = v;
				}, new Vector2(0f, 0f), 0.5f);
				Panel.MaskBtn.gameObject.SetActive(value: true);
			}
			else
			{
				DOTween.To(() => m_RectTransform.anchoredPosition, (Vector2 v) =>
				{
					m_RectTransform.anchoredPosition = v;
				}, new Vector2(215f, 0f), 0.5f);
				Panel.MaskBtn.gameObject.SetActive(value: false);
			}
		}
	}
}
