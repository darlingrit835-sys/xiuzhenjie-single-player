using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		[SerializeField]
		private Color32 topColor = Color.white;

		[SerializeField]
		private Color32 bottomColor = Color.black;

		public override void ModifyMesh(Mesh mesh)
		{
			if (!IsActive())
			{
				return;
			}
			Vector3[] vertices = mesh.vertices;
			int vertexCount = mesh.vertexCount;
			if (vertexCount <= 0)
			{
				return;
			}
			float num = vertices[0].y;
			float num2 = vertices[0].y;
			for (int i = 1; i < vertexCount; i++)
			{
				float y = vertices[i].y;
				if (y > num2)
				{
					num2 = y;
				}
				else if (y < num)
				{
					num = y;
				}
			}
			List<Color32> list = new List<Color32>();
			float num3 = num2 - num;
			for (int j = 0; j < vertexCount; j++)
			{
				list.Add(Color32.Lerp(bottomColor, topColor, (vertices[j].y - num) / num3));
			}
			mesh.SetColors(list);
		}

		public override void ModifyMesh(VertexHelper vh)
		{
		}
	}
}
