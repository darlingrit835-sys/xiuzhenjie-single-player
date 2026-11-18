using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GuideMask : MonoBehaviour, ICanvasRaycastFilter
	{
		private Image _targetImage;

		public void SetTargetImage(Image target)
		{
			_targetImage = target;
		}

		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			if (_targetImage == null)
			{
				return true;
			}
			return !RectTransformUtility.RectangleContainsScreenPoint(_targetImage.rectTransform, sp, eventCamera);
		}
	}
}
