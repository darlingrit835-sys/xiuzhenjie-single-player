using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class TouZhiCaiLiaoItem : MonoBehaviour
	{
		public Dictionary<TouZhiCaiLiaoTypeEnum, Sprite> CaiLiaoSprite;

		public void Awake()
		{
			CaiLiaoSprite = new Dictionary<TouZhiCaiLiaoTypeEnum, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/TouZhi_JingXue.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/TouZhi_YaoDan.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/TouZhi_CaoYao.png");
			CaiLiaoSprite.Add(TouZhiCaiLiaoTypeEnum.JingXue, value);
			CaiLiaoSprite.Add(TouZhiCaiLiaoTypeEnum.YaoDan, value2);
			CaiLiaoSprite.Add(TouZhiCaiLiaoTypeEnum.ZhiWu, value3);
		}

		public void SetImgAndRun(TouZhiCaiLiaoTypeEnum type)
		{
			Sprite value = null;
			if (CaiLiaoSprite.TryGetValue(type, out value))
			{
				base.gameObject.GetComponent<Image>().sprite = value;
			}
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = base.gameObject.GetComponent<RectTransform>().DOLocalMove(new Vector2(0f, 50f), 1f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}));
		}
	}
}
