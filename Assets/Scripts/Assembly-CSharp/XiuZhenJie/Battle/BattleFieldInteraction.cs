using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public sealed class BattleFieldInteraction
	{
		struct Node
		{
			public DiaoLuoItem Item;
			public string Label;
		}

		readonly BattleSceneManager scene;
		readonly Dictionary<int, List<Node>> cells = new Dictionary<int, List<Node>>();
		Node nearby;
		Vector2 lastPosition;
		bool dirty = true;

		public BattleFieldInteraction(BattleSceneManager scene) { this.scene = scene; }

		public void Register(GameObject root, string label)
		{
			int index = scene.Layout.Index(root.transform.position);
			if (index < 0) return;
			if (!cells.TryGetValue(index, out var bucket)) cells[index] = bucket = new List<Node>(2);
			bucket.Add(new Node { Item = root.GetComponent<DiaoLuoItem>(), Label = label });
			dirty = true;
		}

		public string NearbyLabel
		{
			get { Refresh(); return nearby.Item && nearby.Item.isActiveAndEnabled ? nearby.Label : null; }
		}

		public void Collect()
		{
			dirty = true;
			Refresh();
			var item = nearby.Item;
			if (item && item.isActiveAndEnabled && scene.SelfPlayerEntity.CurrentAniState != AniState.Die)
				scene.SelfPlayerEntity.PickUpGoods(item.GoodsId, item.ObjectIndex, item.Name, item.gameObject, item.CompareTag("BattleDiaoLuoLingShi") ? item.Count : 0);
		}

		void Refresh()
		{
			var layout = scene.Layout;
			Vector2 position = scene.SelfPlayerEntity.GetPos();
			if (!dirty && (position - lastPosition).sqrMagnitude < .0001f && (nearby.Label == null || nearby.Item && nearby.Item.isActiveAndEnabled)) return;
			lastPosition = position;
			dirty = false;
			nearby = default;
			int index = layout.Index(position);
			if (index < 0) return;
			int cx = index % layout.Width, cy = index / layout.Width;
			float nearest = 2.8f * 2.8f;
			for (int y = Mathf.Max(0,cy-2); y <= Mathf.Min(layout.Height-1,cy+2); y++)
			for (int x = Mathf.Max(0,cx-2); x <= Mathf.Min(layout.Width-1,cx+2); x++)
			{
				int cell = y * layout.Width + x;
				if (!cells.TryGetValue(cell, out var bucket)) continue;
				for (int i = bucket.Count - 1; i >= 0; i--)
				{
					var node = bucket[i];
					if (!node.Item) { bucket.RemoveAt(i); continue; }
					if (!node.Item.isActiveAndEnabled) continue;
					Vector2 point = node.Item.transform.position;
					float distance = (point - position).sqrMagnitude;
					if (distance > nearest || !scene.FieldView.IsVisible(point) || !layout.CanCross(position, point)) continue;
					nearest = distance;
					nearby = node;
				}
			}
		}
	}
}
