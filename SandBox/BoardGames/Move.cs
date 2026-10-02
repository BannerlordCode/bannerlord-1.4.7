using System;
using SandBox.BoardGames.Pawns;
using SandBox.BoardGames.Tiles;

namespace SandBox.BoardGames
{
	// Token: 0x020000EB RID: 235
	public struct Move
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00054DD5 File Offset: 0x00052FD5
		public bool IsValid
		{
			get
			{
				return this.Unit != null && this.GoalTile != null;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00054DEA File Offset: 0x00052FEA
		public Move(PawnBase unit, TileBase goalTile)
		{
			this.Unit = unit;
			this.GoalTile = goalTile;
		}

		// Token: 0x040004F7 RID: 1271
		public static readonly Move Invalid = new Move
		{
			Unit = null,
			GoalTile = null
		};

		// Token: 0x040004F8 RID: 1272
		public PawnBase Unit;

		// Token: 0x040004F9 RID: 1273
		public TileBase GoalTile;
	}
}
