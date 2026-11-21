using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace XiuZhenJie
{
	public class JingMaiItem : MonoBehaviour
	{
		public string XueWeiId;

		public SpriteRenderer ItemImg;

		public SpriteRenderer MaskImg;

		public TextMesh ItemText;

		public List<string> Childs = new List<string>();

		public Tweener tw1;

		public Tweener tw2;

		public Animator JianBianAnimator;

		public XueWeiStatus CurrentStatus;

		private bool isShanSuo;

		private void Awake()
		{
			CurrentStatus = XueWeiStatus.Disable;
		}

		public void ChangeStatus(XueWeiStatus _status)
		{
			switch (_status)
			{
			case XueWeiStatus.Usable:
				MaskImg.gameObject.SetActive(value: true);
				JianBianAnimator.enabled = true;
				break;
			case XueWeiStatus.Disable:
				MaskImg.gameObject.SetActive(value: true);
				JianBianAnimator.enabled = false;
				break;
			case XueWeiStatus.Used:
				JianBianAnimator.enabled = false;
				MaskImg.gameObject.SetActive(value: false);
				break;
			}
			CurrentStatus = _status;
		}
	}
}
