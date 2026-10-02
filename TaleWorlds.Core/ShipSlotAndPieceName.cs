using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000092 RID: 146
	public struct ShipSlotAndPieceName
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x0001D05D File Offset: 0x0001B25D
		public ShipSlotAndPieceName(string slotName, string pieceName)
		{
			this.SlotName = slotName;
			this.PieceName = pieceName;
		}

		// Token: 0x04000463 RID: 1123
		public string SlotName;

		// Token: 0x04000464 RID: 1124
		public string PieceName;
	}
}
