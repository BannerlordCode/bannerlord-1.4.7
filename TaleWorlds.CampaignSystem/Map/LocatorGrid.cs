using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000222 RID: 546
	internal class LocatorGrid<T> where T : ILocatable<T>
	{
		// Token: 0x060020DE RID: 8414 RVA: 0x00091B19 File Offset: 0x0008FD19
		internal LocatorGrid(float gridNodeSize = 5f, int gridWidth = 32, int gridHeight = 32)
		{
			this._width = gridWidth;
			this._height = gridHeight;
			this._gridNodeSize = gridNodeSize;
			this._nodes = new T[this._width * this._height];
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00091B4E File Offset: 0x0008FD4E
		private int MapCoordinates(int x, int y)
		{
			x %= this._width;
			if (x < 0)
			{
				x += this._width;
			}
			y %= this._height;
			if (y < 0)
			{
				y += this._height;
			}
			return y * this._width + x;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00091B8C File Offset: 0x0008FD8C
		internal bool CheckWhetherPositionsAreInSameNode(Vec2 pos1, ILocatable<T> locatable)
		{
			int num = this.Pos2NodeIndex(pos1);
			int locatorNodeIndex = locatable.LocatorNodeIndex;
			return num == locatorNodeIndex;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00091BAC File Offset: 0x0008FDAC
		internal bool UpdateLocator(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			Vec2 getPosition2D = locatable2.GetPosition2D;
			int num = this.Pos2NodeIndex(getPosition2D);
			if (num != locatable2.LocatorNodeIndex)
			{
				if (locatable2.LocatorNodeIndex >= 0)
				{
					this.RemoveFromList(locatable2);
				}
				this.AddToList(num, locatable);
				locatable2.LocatorNodeIndex = num;
				return true;
			}
			return false;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00091BFC File Offset: 0x0008FDFC
		private void RemoveFromList(ILocatable<T> locatable)
		{
			if (this._nodes[locatable.LocatorNodeIndex] == locatable)
			{
				this._nodes[locatable.LocatorNodeIndex] = locatable.NextLocatable;
				locatable.NextLocatable = default(T);
				return;
			}
			ILocatable<T> locatable2;
			if ((locatable2 = this._nodes[locatable.LocatorNodeIndex]) != null)
			{
				while (locatable2.NextLocatable != null)
				{
					if (locatable2.NextLocatable == locatable)
					{
						locatable2.NextLocatable = locatable.NextLocatable;
						locatable.NextLocatable = default(T);
						return;
					}
					locatable2 = locatable2.NextLocatable;
				}
				Debug.FailedAssert("cannot remove party from MapLocator: " + locatable.ToString(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Map\\LocatorGrid.cs", "RemoveFromList", 134);
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00091CCC File Offset: 0x0008FECC
		private void AddToList(int nodeIndex, T locator)
		{
			T t = this._nodes[nodeIndex];
			this._nodes[nodeIndex] = locator;
			locator.NextLocatable = t;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00091D00 File Offset: 0x0008FF00
		private T FindLocatableOnNextNode(ref LocatableSearchData<T> data)
		{
			T t = default(T);
			do
			{
				data.CurrentY++;
				if (data.CurrentY > data.MaxYInclusive)
				{
					data.CurrentY = data.MinY;
					data.CurrentX++;
				}
				if (data.CurrentX <= data.MaxXInclusive)
				{
					int num = this.MapCoordinates(data.CurrentX, data.CurrentY);
					t = this._nodes[num];
				}
			}
			while (t == null && data.CurrentX <= data.MaxXInclusive);
			return t;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00091D8C File Offset: 0x0008FF8C
		internal T FindNextLocatable(ref LocatableSearchData<T> data)
		{
			if (data.CurrentLocatable != null)
			{
				data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				while (data.CurrentLocatable != null)
				{
					if (data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) < data.RadiusSquared)
					{
						break;
					}
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			while (data.CurrentLocatable == null && data.CurrentX <= data.MaxXInclusive)
			{
				data.CurrentLocatable = this.FindLocatableOnNextNode(ref data);
				while (data.CurrentLocatable != null && data.CurrentLocatable.GetPosition2D.DistanceSquared(data.Position) >= data.RadiusSquared)
				{
					data.CurrentLocatable = data.CurrentLocatable.NextLocatable;
				}
			}
			return (T)((object)data.CurrentLocatable);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00091E74 File Offset: 0x00090074
		internal LocatableSearchData<T> StartFindingLocatablesAroundPosition(Vec2 position, float radius)
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.GetBoundaries(position, radius, out num, out num2, out num3, out num4);
			return new LocatableSearchData<T>(position, radius, num, num2, num3, num4);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00091E9C File Offset: 0x0009009C
		internal void RemoveLocatable(T locatable)
		{
			ILocatable<T> locatable2 = locatable;
			if (locatable2.LocatorNodeIndex >= 0)
			{
				this.RemoveFromList(locatable2);
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00091EC0 File Offset: 0x000900C0
		private void GetBoundaries(Vec2 position, float radius, out int minX, out int minY, out int maxX, out int maxY)
		{
			Vec2 vec = new Vec2(radius, radius);
			this.GetGridIndices(position - vec, out minX, out minY);
			this.GetGridIndices(position + vec, out maxX, out maxY);
			int num = Math.Min(maxX - minX, this._width - 1);
			int num2 = Math.Min(maxY - minY, this._height - 1);
			minX %= this._width;
			minY %= this._height;
			maxX = minX + num;
			maxY = minY + num2;
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00091F47 File Offset: 0x00090147
		private void GetGridIndices(Vec2 position, out int x, out int y)
		{
			x = MathF.Floor(position.x / this._gridNodeSize);
			y = MathF.Floor(position.y / this._gridNodeSize);
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00091F74 File Offset: 0x00090174
		private int Pos2NodeIndex(Vec2 position)
		{
			int num;
			int num2;
			this.GetGridIndices(position, out num, out num2);
			return this.MapCoordinates(num, num2);
		}

		// Token: 0x040009AC RID: 2476
		private const float DefaultGridNodeSize = 5f;

		// Token: 0x040009AD RID: 2477
		private const int DefaultGridWidth = 32;

		// Token: 0x040009AE RID: 2478
		private const int DefaultGridHeight = 32;

		// Token: 0x040009AF RID: 2479
		private readonly T[] _nodes;

		// Token: 0x040009B0 RID: 2480
		private readonly float _gridNodeSize;

		// Token: 0x040009B1 RID: 2481
		private readonly int _width;

		// Token: 0x040009B2 RID: 2482
		private readonly int _height;
	}
}
