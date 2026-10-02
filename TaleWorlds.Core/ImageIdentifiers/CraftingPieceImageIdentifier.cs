using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E5 RID: 229
	public class CraftingPieceImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x000255D5 File Offset: 0x000237D5
		public CraftingPieceImageIdentifier(CraftingPiece craftingPiece, string pieceUsageId)
		{
			base.Id = ((craftingPiece != null) ? (craftingPiece.StringId + "$" + pieceUsageId) : "");
			base.AdditionalArgs = "";
			base.TextureProviderName = "CraftingPieceImageTextureProvider";
		}
	}
}
