using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Painting : MonoBehaviour
	{
		public RenderTexture texRender;

		public InputDirection inputDirection;

		public bool activeInput;

		public PatingStatus patingStatus;

		public bool isStart;

		private Vector3 mousePos;

		public GameObject PointItem;

		public Material mat;

		public Texture brushTypeTexture;

		public Camera mainCamera;

		public float brushScale = 0.5f;

		public Color brushColor = Color.red;

		public RawImage raw;

		public float lastDistance;

		public Vector3[] PositionArray = new Vector3[3];

		public int a;

		public Vector3[] PositionArray1 = new Vector3[4];

		public int b;

		public float[] speedArray = new float[4];

		public int s;

		public int num = 50;

		private float[] Ypoints = new float[5] { 0.1f, 0.4f, 0.6f, 0.8f, 0.2f };

		private float[] Xpoints = new float[5] { 50f, 100f, 200f, 50f, 250f };

		private float[] Y_points = new float[5];

		private RectTransform[] _items;

		private InputDirection[] dirs;

		private int buzou;

		private int maxBuZou;

		private Vector2 rawMousePosition;

		private float rawWidth;

		private float rawHeight;

		private Vector3 startPosition = Vector3.zero;

		private Vector3 endPosition = Vector3.zero;

		public Action<bool> HuaFuEndAction;

		public void Awake()
		{
			inputDirection = InputDirection.NULL;
			patingStatus = PatingStatus.Compelete;
		}

		private void Start()
		{
			rawWidth = raw.rectTransform.sizeDelta.x;
			rawHeight = raw.rectTransform.sizeDelta.y;
			Vector2 vector = new Vector2(raw.rectTransform.anchoredPosition.x - raw.rectTransform.sizeDelta.x / 2f, raw.rectTransform.anchoredPosition.y - raw.rectTransform.sizeDelta.y / 2f);
			rawMousePosition = vector + new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			texRender = new RenderTexture((int)rawWidth, (int)rawHeight, 24, RenderTextureFormat.ARGB32);
			Clear(texRender);
		}

		private void Update()
		{
			if (isStart)
			{
				GetInputDirection();
				if (inputDirection != InputDirection.NULL && patingStatus == PatingStatus.Compelete && dirs[buzou] == inputDirection && buzou <= maxBuZou)
				{
					patingStatus = PatingStatus.Running;
					StartCoroutine(PointToPoint(new Vector2(_items[buzou].anchoredPosition.x, _items[buzou].anchoredPosition.y), new Vector2(_items[buzou + 1].anchoredPosition.x, _items[buzou + 1].anchoredPosition.y), buzou));
				}
				DrawImage();
			}
		}

		public void StartPaiting()
		{
			isStart = true;
			inputDirection = InputDirection.NULL;
			patingStatus = PatingStatus.Compelete;
		}

		private IEnumerator PointToPoint(Vector2 FromPoint, Vector2 TargetPoint, int index)
		{
			Vector2 lerq = TargetPoint - FromPoint;
			bool isFar = false;
			Vector2 pos = FromPoint;
			while (isStart && index == buzou)
			{
				pos += lerq / 100f;
				OnMouseMove(new Vector3(pos.x / rawWidth, pos.y / rawHeight, 0f));
				if (Vector2.Distance(pos, TargetPoint) < 5f)
				{
					if (!isFar)
					{
						inputDirection = InputDirection.wait;
					}
					if (dirs[index + 1] == inputDirection)
					{
						patingStatus = PatingStatus.Compelete;
						buzou++;
						break;
					}
					isFar = true;
				}
				else
				{
					if (dirs[index] != inputDirection && inputDirection != InputDirection.wait)
					{
						Debug.Log("失败");
						patingStatus = PatingStatus.Fail;
						inputDirection = InputDirection.NULL;
						isStart = false;
						OnMouseUp();
						if (HuaFuEndAction != null)
						{
							HuaFuEndAction(obj: false);
						}
						break;
					}
					if (isFar)
					{
						if (index == maxBuZou - 1)
						{
							Debug.Log("所有线段绘制完成");
							patingStatus = PatingStatus.Compelete;
							inputDirection = InputDirection.NULL;
							isStart = false;
							OnMouseUp();
							if (HuaFuEndAction != null)
							{
								HuaFuEndAction(obj: true);
							}
						}
						else
						{
							Debug.Log("失敗");
							patingStatus = PatingStatus.Fail;
							inputDirection = InputDirection.NULL;
							isStart = false;
							OnMouseUp();
							if (HuaFuEndAction != null)
							{
								HuaFuEndAction(obj: false);
							}
						}
						break;
					}
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public void CreatePointImg(List<Vector2> points)
		{
			_items = new RectTransform[points.Count];
			dirs = new InputDirection[points.Count];
			for (int i = 0; i < points.Count; i++)
			{
				GameObject obj = UnityEngine.Object.Instantiate(PointItem);
				obj.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = (i + 1).ToString();
				obj.name = i.ToString();
				RectTransform component = obj.GetComponent<RectTransform>();
				_items[i] = component;
				component.SetParent(base.transform);
				component.anchoredPosition = new Vector2(points[i].x, points[i].y);
				if (i - 1 >= 0)
				{
					dirs[i - 1] = PointChangeEnum(new Vector2(points[i].x, points[i].y), new Vector2(points[i - 1].x, points[i - 1].y));
				}
				component.localScale = Vector3.one;
			}
			buzou = 0;
			maxBuZou = points.Count - 1;
		}

		public void ClearPointImg()
		{
			foreach (Transform item in base.gameObject.transform)
			{
				if (!(item.gameObject.name == "Bg"))
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
		}

		private void OnMouseUp()
		{
			startPosition = Vector3.zero;
			a = 0;
			b = 0;
			s = 0;
		}

		private float SetScale(float distance)
		{
			float num = 0f;
			num = ((!(distance < 100f)) ? (0.425f - 0.00125f * distance) : (0.8f - 0.005f * distance));
			if (num <= 0.05f)
			{
				num = 0.05f;
			}
			return num;
		}

		private void OnMouseMove(Vector3 pos)
		{
			if (startPosition == Vector3.zero)
			{
				startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			}
			endPosition = pos;
			float num = Vector3.Distance(startPosition, endPosition);
			brushScale = SetScale(num);
			float num2 = 1f;
			_ = num / num2;
			ThreeOrderBézierCurse(pos, num, 2.5f);
			startPosition = endPosition;
			lastDistance = num;
		}

		private void OnMouseMove2(Vector3 pos)
		{
			if (startPosition == Vector3.zero)
			{
				startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			}
			endPosition = pos;
			float scale = Vector3.Distance(startPosition, endPosition);
			brushScale = SetScale(scale);
			DrawBrushTwo(texRender, brushColor, pos);
			startPosition = endPosition;
			lastDistance = scale;
		}

		public void Clear(RenderTexture destTexture)
		{
			Graphics.SetRenderTarget(destTexture);
			GL.PushMatrix();
			GL.Clear(clearDepth: true, clearColor: true, Color.white);
			GL.PopMatrix();
		}

		public void Clear()
		{
			Graphics.SetRenderTarget(texRender);
			GL.PushMatrix();
			GL.Clear(clearDepth: true, clearColor: true, Color.white);
			GL.PopMatrix();
		}

		private void DrawBrush(RenderTexture destTexture, int x, int y, Texture sourceTexture, Color color, float scale)
		{
			DrawBrush(destTexture, new Rect(x, y, sourceTexture.width, sourceTexture.height), sourceTexture, color, scale);
		}

		private void DrawBrush(RenderTexture destTexture, Rect destRect, Texture sourceTexture, Color color, float scale)
		{
			float num = (destRect.xMin - rawMousePosition.x) * (float)Screen.width / rawWidth - destRect.width * 0.1f / 2f;
			float num2 = (destRect.xMin - rawMousePosition.x) * (float)Screen.width / rawWidth + destRect.width * 0.1f / 2f;
			float num3 = (destRect.yMin - rawMousePosition.y) * (float)Screen.height / rawHeight - destRect.height * 0.1f / 2f;
			float num4 = (destRect.yMin - rawMousePosition.y) * (float)Screen.height / rawHeight + destRect.height * 0.1f / 2f;
			Graphics.SetRenderTarget(destTexture);
			GL.PushMatrix();
			GL.LoadOrtho();
			mat.SetTexture("_MainTex", brushTypeTexture);
			mat.SetColor("_Color", color);
			mat.SetPass(0);
			GL.Begin(7);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(num / rawWidth, num3 / rawHeight, 0f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(num2 / rawWidth, num3 / rawHeight, 0f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(num2 / rawWidth, num4 / rawHeight, 0f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(num / rawWidth, num4 / rawHeight, 0f);
			GL.End();
			GL.PopMatrix();
		}

		private void DrawBrushTwo(RenderTexture destTexture, Color color, Vector2 CenterPos)
		{
			Vector2 vector = new Vector2(CenterPos.x - 0.02f, CenterPos.y - 0.02f);
			Vector2 vector2 = new Vector2(CenterPos.x + 0.02f, CenterPos.y - 0.02f);
			Vector2 vector3 = new Vector2(CenterPos.x + 0.02f, CenterPos.y + 0.02f);
			Vector2 vector4 = new Vector2(CenterPos.x - 0.02f, CenterPos.y + 0.02f);
			Graphics.SetRenderTarget(destTexture);
			GL.PushMatrix();
			GL.LoadOrtho();
			mat.SetTexture("_MainTex", brushTypeTexture);
			mat.SetColor("_Color", color);
			mat.SetPass(0);
			GL.Begin(7);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(vector.x, vector.y, 0f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(vector2.x, vector2.y, 0f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(vector3.x, vector3.y, 0f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(vector4.x, vector4.y, 0f);
			GL.End();
			GL.PopMatrix();
		}

		private void DrawImage()
		{
			raw.texture = texRender;
		}

		public void OnClickClear()
		{
			Clear(texRender);
		}

		public void TwoOrderBézierCurse(Vector3 pos, float distance)
		{
			PositionArray[a] = pos;
			a++;
			if (a == 3)
			{
				for (int i = 0; i < this.num; i++)
				{
					Vector3 vector = (PositionArray[0] + PositionArray[2]) / 2f;
					PositionArray[1] = (PositionArray[1] - vector) / 2f + vector;
					float num = 1f / (float)this.num * (float)i / 2f;
					Vector3 vector2 = Mathf.Pow(1f - num, 2f) * PositionArray[0] + 2f * (1f - num) * num * PositionArray[1] + Mathf.Pow(num, 2f) * PositionArray[2];
					float num2 = (distance - lastDistance) / (float)this.num;
					DrawBrush(texRender, (int)vector2.x, (int)vector2.y, brushTypeTexture, brushColor, SetScale(lastDistance + num2 * (float)i));
				}
				PositionArray[0] = PositionArray[1];
				PositionArray[1] = PositionArray[2];
				a = 2;
			}
			else
			{
				DrawBrush(texRender, (int)endPosition.x, (int)endPosition.y, brushTypeTexture, brushColor, brushScale);
			}
		}

		public void ThreeOrderBézierCurse(Vector3 pos, float distance, float targetPosOffset)
		{
			PositionArray1[b] = pos;
			b++;
			speedArray[s] = distance;
			s++;
			if (b == 4)
			{
				Vector3 vector = PositionArray1[1];
				Vector3 vector2 = PositionArray1[2];
				Vector3 vector3 = (PositionArray1[0] + PositionArray1[2]) / 2f;
				PositionArray1[1] = (PositionArray1[1] - vector3) * 1.5f + vector3;
				vector3 = (vector + PositionArray1[3]) / 2f;
				PositionArray1[2] = (PositionArray1[2] - vector3) * 2.1f + vector3;
				for (int i = 0; (float)i < (float)this.num / 1.5f; i++)
				{
					float num = 1f / (float)this.num * (float)i;
					_ = Mathf.Pow(1f - num, 3f) * PositionArray1[0] + 3f * PositionArray1[1] * num * Mathf.Pow(1f - num, 2f) + 3f * PositionArray1[2] * num * num * (1f - num) + PositionArray1[3] * Mathf.Pow(num, 3f);
					_ = (speedArray[3] - speedArray[0]) / (float)this.num;
					UnityEngine.Random.Range(0f - targetPosOffset, targetPosOffset);
					DrawBrushTwo(texRender, brushColor, pos);
				}
				PositionArray1[0] = vector;
				PositionArray1[1] = vector2;
				PositionArray1[2] = PositionArray1[3];
				speedArray[0] = speedArray[1];
				speedArray[1] = speedArray[2];
				speedArray[2] = speedArray[3];
				b = 3;
				s = 3;
			}
			else
			{
				DrawBrushTwo(texRender, brushColor, pos);
			}
		}

		private void GetInputDirection()
		{
			if (Input.GetMouseButtonDown(0))
			{
				activeInput = true;
				mousePos = Input.mousePosition;
			}
			if (!Input.GetMouseButton(0) || !activeInput)
			{
				return;
			}
			Vector3 vector = Input.mousePosition - mousePos;
			if (vector.magnitude > 20f)
			{
				float num = Mathf.Acos(Vector3.Dot(Vector3.up, vector.normalized)) * 57.29578f;
				float num2 = Mathf.Acos(Vector3.Dot(Vector3.right, vector.normalized)) * 57.29578f;
				if (num <= 22.5f)
				{
					inputDirection = InputDirection.Up;
				}
				else if (num >= 157.5f)
				{
					inputDirection = InputDirection.Down;
				}
				else if (num2 <= 22.5f)
				{
					inputDirection = InputDirection.Right;
				}
				else if (num2 >= 157.5f)
				{
					inputDirection = InputDirection.Left;
				}
				else if (num2 > 22.5f && num2 < 90f && num > 22.5f && num < 67.5f)
				{
					inputDirection = InputDirection.UpRght;
				}
				else if (num2 > 22.5f && num2 < 90f && num >= 112.5f && num <= 157.5f)
				{
					inputDirection = InputDirection.DownRight;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num > 22.5f && num < 67.5f)
				{
					inputDirection = InputDirection.UpLeft;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num >= 112.5f && num <= 157.5f)
				{
					inputDirection = InputDirection.DownLeft;
				}
				activeInput = false;
				Debug.Log(inputDirection);
			}
		}

		private InputDirection PointChangeEnum(Vector2 OnePoint, Vector2 TwoPoint)
		{
			InputDirection result = InputDirection.NULL;
			Vector3 vector = OnePoint - TwoPoint;
			if (vector.magnitude > 0f)
			{
				float num = Mathf.Acos(Vector3.Dot(Vector3.up, vector.normalized)) * 57.29578f;
				float num2 = Mathf.Acos(Vector3.Dot(Vector3.right, vector.normalized)) * 57.29578f;
				if (num <= 22.5f)
				{
					result = InputDirection.Up;
				}
				else if (num >= 157.5f)
				{
					result = InputDirection.Down;
				}
				else if (num2 <= 22.5f)
				{
					result = InputDirection.Right;
				}
				else if (num2 >= 157.5f)
				{
					result = InputDirection.Left;
				}
				else if (num2 > 22.5f && num2 < 90f && num > 22.5f && num < 67.5f)
				{
					result = InputDirection.UpRght;
				}
				else if (num2 > 22.5f && num2 < 90f && num >= 112.5f && num <= 157.5f)
				{
					result = InputDirection.DownRight;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num > 22.5f && num < 67.5f)
				{
					result = InputDirection.UpLeft;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num >= 112.5f && num <= 157.5f)
				{
					result = InputDirection.DownLeft;
				}
			}
			return result;
		}
	}
}
