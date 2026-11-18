using UnityEngine;

public class DiaoLuoItem : MonoBehaviour
{
	private int m_ObjectIndex;

	private string m_Name;

	private string m_GoodsId;

	private int m_Count;

	public int ObjectIndex
	{
		get
		{
			return m_ObjectIndex;
		}
		private set
		{
			m_ObjectIndex = value;
		}
	}

	public string Name
	{
		get
		{
			return m_Name;
		}
		private set
		{
			m_Name = value;
		}
	}

	public string GoodsId
	{
		get
		{
			return m_GoodsId;
		}
		private set
		{
			m_GoodsId = value;
		}
	}

	public int Count
	{
		get
		{
			return m_Count;
		}
		private set
		{
			m_Count = value;
		}
	}

	public void Init(int objectIndex, string name, string goodsId, int count = 1)
	{
		ObjectIndex = objectIndex;
		Name = name;
		GoodsId = goodsId;
		Count = count;
	}
}
