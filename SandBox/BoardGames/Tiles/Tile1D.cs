using System;
using SandBox.BoardGames.Objects;
using TaleWorlds.Engine;

namespace SandBox.BoardGames.Tiles
{
	// Token: 0x020000F3 RID: 243
	public class Tile1D : TileBase
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0005D1D3 File Offset: 0x0005B3D3
		public int X { get; }

		// Token: 0x06000C5D RID: 3165 RVA: 0x0005D1DB File Offset: 0x0005B3DB
		public Tile1D(GameEntity entity, BoardGameDecal decal, int x)
			: base(entity, decal)
		{
			this.X = x;
		}
	}
}
