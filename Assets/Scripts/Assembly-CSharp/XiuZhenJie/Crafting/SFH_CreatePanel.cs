using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SFH_CreatePanel : MonoBehaviour
	{
		public Button LiKaiBtn;

		public GameObject TempleItem;

		public RectTransform HuaBuRect;

		public Painting PaintingImg;

		public RectTransform SuccessBg;

		public Button Success_QueDingBtn;

		public Button Success_CloseBtn;

		public Text Success_TitleText;

		public Text Success_MiddleText;

		public RectTransform FailBg;

		public Text FailJingLiText;

		public Button Fail_CloseBtn;

		public Button Fail_AgainBtn;

		public Button WeiHao;

		public UnityAction<Vector2> MouseDownPos;

		public UnityAction<Vector2> MouseUpPos;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (MouseDownPos != null)
				{
					MouseDownPos(Input.mousePosition);
				}
			}
			else if (Input.GetMouseButtonUp(0) && MouseUpPos != null)
			{
				MouseUpPos(Input.mousePosition);
			}
		}
	}
}
