using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails
{
	// Token: 0x02000047 RID: 71
	public class CraftingPieceCreationData : ThumbnailCreationData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000FFAA File Offset: 0x0000E1AA
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000FFB2 File Offset: 0x0000E1B2
		public CraftingPiece CraftingPiece { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000FFBB File Offset: 0x0000E1BB
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000FFC3 File Offset: 0x0000E1C3
		public string Type { get; private set; }

		// Token: 0x06000260 RID: 608 RVA: 0x0000FFCC File Offset: 0x0000E1CC
		public CraftingPieceCreationData(CraftingPiece craftingPiece, string type, Action<Texture> setAction, Action cancelAction)
			: base(craftingPiece.StringId + "$" + type, setAction, cancelAction)
		{
			this.CraftingPiece = craftingPiece;
			this.Type = type;
		}
	}
}
