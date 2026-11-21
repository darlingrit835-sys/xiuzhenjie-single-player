using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZF_LianZhiPanel : MonoBehaviour
	{
		public enum SlideVector
		{
			NullVector = 0,
			Up = 1,
			Down = 2,
			Left = 3,
			Right = 4
		}

		public Image Mask;

		public Button Likai;

		public Button WenHao;

		public Text ShengYu;

		public ZS_YuanPanButListItem A_1;

		public ZS_YuanPanButListItem B_1;

		public ZS_YuanPanButListItem C_1;

		public LongClickButton ZhuanDo_A;

		public LongClickButton ZhuanDo_B;

		public LongClickButton ZhuanDo_C;

		public RectTransform FailPanel;

		public Button FailOnceAgainBtn;

		public Text FailWuXingXiaoHao;

		public RectTransform SuccessBg;

		public Button Success_QueDingBtn;

		public Button Success_CloseBtn;

		public Text Success_TitleText;

		public Text Success_MiddleText;

		private Vector2 ModelPos = new Vector2(561f, 1064f);

		public Vector2 mousePos;

		public Vector2 premousePos;

		public Quaternion q;

		private float RotateAngle;

		public Vector3 localEluer;

		private bool IsSelect;

		private SlideVector m_CurrentVector;

		private float m_Timer;

		private float m_SlidingDistance = 1f;

		private bool m_MouseBtnInput;

		private float m_AddTime;

		public UnityAction<SlideVector> ZF_PointMoveHandler;

		public bool HuaDo;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && !HuaDo)
			{
				premousePos = (mousePos = Input.mousePosition);
			}
			if (Input.GetMouseButton(0) && HuaDo)
			{
				mousePos = Input.mousePosition;
				RotateAngle = Vector2.Angle(premousePos - ModelPos, mousePos - ModelPos);
				if (mousePos.x < 0f || mousePos.y < 0f || mousePos.x > (float)Screen.width || mousePos.y > (float)Screen.height)
				{
					m_CurrentVector = SlideVector.NullVector;
					ZF_PointMoveHandler?.Invoke(m_CurrentVector);
					HuaDo = false;
				}
				if (RotateAngle == 0f)
				{
					premousePos = mousePos;
				}
				else
				{
					q = Quaternion.FromToRotation(premousePos - ModelPos, mousePos - ModelPos);
					float num = ((q.z > 0f) ? 1 : (-1));
					localEluer.x += num * RotateAngle;
					premousePos = mousePos;
					m_CurrentVector = SlideVector.Down;
					ZF_PointMoveHandler?.Invoke(m_CurrentVector);
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				m_CurrentVector = SlideVector.NullVector;
				ZF_PointMoveHandler?.Invoke(m_CurrentVector);
				HuaDo = false;
			}
		}
	}
}
