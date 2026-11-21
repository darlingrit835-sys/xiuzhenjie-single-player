using UnityEngine;
using UnityEngine.EventSystems;

namespace XiuZhenJie
{
	public class ZF_ButAnXiaItem : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		public int BenShenShu;

		public void OnPointerDown(PointerEventData eventData)
		{
			Singleton<UIManager>.Instance.FindWndByName<ZF_LianZhiWnd>("ZF_LianZhiWnd.prefab")?.ZhuanDoA(BenShenShu);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Singleton<UIManager>.Instance.FindWndByName<ZF_LianZhiWnd>("ZF_LianZhiWnd.prefab").Shu = 0;
		}
	}
}
