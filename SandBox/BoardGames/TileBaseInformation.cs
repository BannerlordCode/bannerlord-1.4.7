using System;
using SandBox.BoardGames.Pawns;

namespace SandBox.BoardGames
{
	// Token: 0x020000EA RID: 234
	public struct TileBaseInformation
	{
		// Token: 0x06000B60 RID: 2912 RVA: 0x00054DCB File Offset: 0x00052FCB
		public TileBaseInformation(ref PawnBase pawnOnTile)
		{
			this.PawnOnTile = pawnOnTile;
		}

		// Token: 0x040004F6 RID: 1270
		public PawnBase PawnOnTile;
	}
}
