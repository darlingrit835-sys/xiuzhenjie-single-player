using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class BaseMapPanel : MonoBehaviour
	{
		public virtual void SetValue(MapType _mapType, List<MapEntity> _list, List<string> _parentIds, MapEnterType _enterType)
		{
		}

		public virtual void ClearValue()
		{
		}
	}
}
