using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZaoFaShuJieYinPanel : MonoBehaviour
	{
		private Dictionary<string, Sprite> m_JieYinSpriteDic = new Dictionary<string, Sprite>();

		private Dictionary<string, string> m_JieYinSpritePathDic;

		private void init()
		{
			m_JieYinSpriteDic = new Dictionary<string, Sprite>();
			m_JieYinSpritePathDic = new Dictionary<string, string>();
			m_JieYinSpritePathDic.Add("乾天印", "ShouYin_1");
			m_JieYinSpritePathDic.Add("坎水印", "ShouYin_2");
			m_JieYinSpritePathDic.Add("艮山印", "ShouYin_3");
			m_JieYinSpritePathDic.Add("震雷印", "ShouYin_4");
			m_JieYinSpritePathDic.Add("巽风印", "ShouYin_5");
			m_JieYinSpritePathDic.Add("离火印", "ShouYin_6");
			m_JieYinSpritePathDic.Add("坤地印", "ShouYin_7");
			m_JieYinSpritePathDic.Add("兑泽印", "ShouYin_8");
			m_JieYinSpritePathDic.Add("地风印", "ShouYin_9");
			m_JieYinSpritePathDic.Add("水风印", "ShouYin_10");
			m_JieYinSpritePathDic.Add("天风印", "ShouYin_11");
			m_JieYinSpritePathDic.Add("天山印", "ShouYin_12");
			m_JieYinSpritePathDic.Add("天地印", "ShouYin_13");
			m_JieYinSpritePathDic.Add("风地印", "ShouYin_14");
			m_JieYinSpritePathDic.Add("山地印", "ShouYin_15");
			m_JieYinSpritePathDic.Add("火地印", "ShouYin_16");
			m_JieYinSpritePathDic.Add("火天印", "ShouYin_17");
			m_JieYinSpritePathDic.Add("水泽印", "ShouYin_18");
			m_JieYinSpritePathDic.Add("水雷印", "ShouYin_19");
			m_JieYinSpritePathDic.Add("水火印", "ShouYin_20");
			m_JieYinSpritePathDic.Add("泽火印", "ShouYin_21");
			m_JieYinSpritePathDic.Add("雷火印", "ShouYin_22");
			m_JieYinSpritePathDic.Add("地火印", "ShouYin_23");
			m_JieYinSpritePathDic.Add("地水印", "ShouYin_24");
			m_JieYinSpritePathDic.Add("山火印", "ShouYin_25");
			m_JieYinSpritePathDic.Add("山泽印", "ShouYin_26");
			m_JieYinSpritePathDic.Add("天泽印", "ShouYin_27");
			m_JieYinSpritePathDic.Add("风泽印", "ShouYin_28");
			m_JieYinSpritePathDic.Add("风山印", "ShouYin_29");
			m_JieYinSpritePathDic.Add("雷地印", "ShouYin_30");
			m_JieYinSpritePathDic.Add("雷水印", "ShouYin_31");
			m_JieYinSpritePathDic.Add("雷风印", "ShouYin_32");
		}

		public void ShowImg(Image img, string name, Dictionary<string, string> jieYinSpritePathDic)
		{
			if (m_JieYinSpritePathDic == null)
			{
				m_JieYinSpritePathDic = jieYinSpritePathDic;
			}
			Sprite value = null;
			if (m_JieYinSpriteDic.TryGetValue(name, out value))
			{
				img.sprite = value;
				if (!img.gameObject.activeSelf)
				{
					img.gameObject.SetActive(value: true);
				}
				return;
			}
			string value2 = string.Empty;
			if (m_JieYinSpritePathDic.TryGetValue(name, out value2))
			{
				string path = "Assets/GameData/Textures/Yin/" + value2 + ".png";
				Sprite sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>(path);
				if (!m_JieYinSpriteDic.ContainsKey(name))
				{
					m_JieYinSpriteDic.Add(name, sprite);
				}
				else
				{
					m_JieYinSpriteDic[name] = sprite;
				}
				img.sprite = sprite;
				if (!img.gameObject.activeSelf)
				{
					img.gameObject.SetActive(value: true);
				}
			}
			else
			{
				img.sprite = null;
			}
		}
	}
}
