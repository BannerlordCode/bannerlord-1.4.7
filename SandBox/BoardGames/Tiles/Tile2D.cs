using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F4 RID: 244
	public class Tile2D : TileBase
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0005D1EC File Offset: 0x0005B3EC
		public int X { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0005D1F4 File Offset: 0x0005B3F4
		public int Y { get; }

		// Token: 0x06000C60 RID: 3168 RVA: 0x0005D1FC File Offset: 0x0005B3FC
		public Tile2D(GameEntity entity, BoardGameDecal decal, int x, int y)
			: base(entity, decal)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
