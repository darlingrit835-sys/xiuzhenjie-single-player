using UnityEngine;

namespace XiuZhenJie
{
	public class GuideMaskNewItem : MonoBehaviour
	{
		private Vector3 corners;

		private Material material;

		public float currentOffsetX;

		public float currentOffsetY;

		private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out var localPoint);
			return localPoint;
		}

		private void Awake()
		{
			Vector4 value = new Vector4(0f, 0f, 0f, 0f);
			material.SetVector("_Center", value);
			material.SetFloat("_SliderX", currentOffsetX);
			material.SetFloat("_SliderY", currentOffsetY);
		}
	}
}
