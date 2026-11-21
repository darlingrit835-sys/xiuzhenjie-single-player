using System;
using UnityEngine;

namespace XiuZhenJie
{
	public readonly struct BattleLandmark
	{
		public readonly int Cell, Kind;
		public BattleLandmark(int cell, int kind) { Cell = cell; Kind = kind; }
	}

	public sealed class BattleFieldLayout
	{
		public const float CellSize = 2f;
		public readonly int Width, Height;
		public readonly Vector2 Origin;
		public readonly byte[] Tiles, Explored;
		public readonly string RegionId;
		public readonly BattleLandmark[] Landmarks;

		// Area effects subscribe for this dungeon's lifetime and identify occupants by ObjectIndex.
		public event Action<EntityBase, int, int> CellChanged;

		private BattleFieldLayout(BattleRoleEnterResp_8001001 entry, int width, int height)
		{
			Origin = new Vector2((float)entry.MapMinX, (float)entry.MapMinY);
			Width = width;
			Height = height;
			RegionId = entry.RegionId;
			Tiles = entry.WorldTiles.ToByteArray();
			Explored = entry.Explored.ToByteArray();
			var packed = entry.Landmarks;
			if (packed.Length % 3 != 0 || packed.Length / 3 > Tiles.Length)
				throw new FormatException("战场地标数据长度错误");
			var sites = new System.Collections.Generic.List<BattleLandmark>(packed.Length / 3);
			for (int i = 0; i < packed.Length; i += 3)
			{
				int cell = packed[i] | (packed[i + 1] << 8), kind = packed[i + 2];
				if (cell >= Tiles.Length) throw new FormatException("战场地标越界");
				if (kind >= 1 && kind <= 6) sites.Add(new BattleLandmark(cell, kind));
			}
			Landmarks = sites.ToArray();
		}

		public static BattleFieldLayout Read(BattleRoleEnterResp_8001001 entry)
		{
			if (entry.WorldTiles.IsEmpty) return null;
			double width = (entry.MapMaxX - entry.MapMinX) / CellSize;
			double height = (entry.MapMaxY - entry.MapMinY) / CellSize;
			if (!(width >= 1 && height >= 1 && width <= SystemInfo.maxTextureSize / 3 && height <= SystemInfo.maxTextureSize / 3 && width == Math.Floor(width) && height == Math.Floor(height)
				&& width * height == entry.WorldTiles.Length && entry.Explored.Length == (entry.WorldTiles.Length + 7L) / 8))
				throw new FormatException("战场格数与地图边界或探索记录不符");
			var layout = new BattleFieldLayout(entry, (int)width, (int)height);
			foreach (byte tile in layout.Tiles)
				if (tile > 6) throw new FormatException("未识别的战场地形");
			if (!layout.IsWalkable(new Vector2((float)entry.PosX, (float)entry.PosY)))
				throw new FormatException("战场出生点不在可通行地形");
			return layout;
		}

		public bool NearLandmark(int cell, int radius)
		{
			int x = cell % Width, y = cell / Width;
			foreach (var site in Landmarks)
			{
				int dx = x - site.Cell % Width, dy = y - site.Cell / Width;
				if (dx * dx + dy * dy <= radius * radius) return true;
			}
			return false;
		}

		public int Index(Vector2 world)
		{
			int x = Mathf.FloorToInt((world.x - Origin.x) / CellSize);
			int y = Mathf.FloorToInt((world.y - Origin.y) / CellSize);
			return x < 0 || y < 0 || x >= Width || y >= Height ? -1 : y * Width + x;
		}

		public Vector2 Center(int index)
		{
			return Origin + new Vector2(index % Width + .5f, index / Width + .5f) * CellSize;
		}

		public bool IsWalkable(Vector2 world)
		{
			int cell = Index(world);
			return cell >= 0 && Tiles[cell] != 0 && Tiles[cell] != 4 && Tiles[cell] <= 6;
		}

		// Visit the cells crossed by a straight interaction ray, including both sides of a corner.
		public bool CanCross(Vector2 from, Vector2 to)
		{
			if (!IsWalkable(from) || !IsWalkable(to)) return false;
			int start = Index(from), end = Index(to);
			int x = start % Width, y = start / Width, tx = end % Width, ty = end / Width;
			Vector2 delta = to - from;
			int sx = Math.Sign(delta.x), sy = Math.Sign(delta.y);
			double dx = sx == 0 ? double.PositiveInfinity : CellSize / Math.Abs((double)delta.x);
			double dy = sy == 0 ? double.PositiveInfinity : CellSize / Math.Abs((double)delta.y);
			double nx = sx == 0 ? double.PositiveInfinity : (Origin.x + (x + (sx > 0 ? 1 : 0))*CellSize - from.x) / (double)delta.x;
			double ny = sy == 0 ? double.PositiveInfinity : (Origin.y + (y + (sy > 0 ? 1 : 0))*CellSize - from.y) / (double)delta.y;
			while (x != tx || y != ty)
			{
				if (x == tx || (y != ty && ny < nx)) { y += sy; ny += dy; }
				else if (y == ty || nx < ny) { x += sx; nx += dx; }
				else
				{
					if (!IsWalkable(Center(y * Width + x + sx)) || !IsWalkable(Center((y + sy) * Width + x))) return false;
					x += sx; y += sy; nx += dx; ny += dy;
				}
				byte surface = Tiles[y * Width + x];
				if (surface == 0 || surface == 4) return false;
			}
			return true;
		}

		public void NotifyMovement(EntityBase actor, Vector2 from, Vector2 to)
		{
			if (CellChanged == null) return;
			int before = Index(from), after = Index(to);
			if (before != after) CellChanged(actor, before, after);
		}
	}
}
