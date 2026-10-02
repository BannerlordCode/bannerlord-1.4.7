using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200009D RID: 157
	[Flags]
	public enum EquipmentCategories : uint
	{
		// Token: 0x04000505 RID: 1285
		None = 0U,
		// Token: 0x04000506 RID: 1286
		IsFemaleTemplate = 1U,
		// Token: 0x04000507 RID: 1287
		IsLordTemplate = 2U,
		// Token: 0x04000508 RID: 1288
		IsChildEquipmentTemplate = 4U,
		// Token: 0x04000509 RID: 1289
		IsTeenagerEquipmentTemplate = 8U,
		// Token: 0x0400050A RID: 1290
		IsKingdomRulerTemplate = 16U
	}
}
