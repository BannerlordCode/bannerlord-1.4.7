using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200021D RID: 541
	public interface IMapPoint
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060020A7 RID: 8359
		TextObject Name { get; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060020A8 RID: 8360
		CampaignVec2 Position { get; }

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060020A9 RID: 8361
		PathFaceRecord CurrentNavigationFace { get; }

		// Token: 0x060020AA RID: 8362
		Vec3 GetPositionAsVec3();

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060020AB RID: 8363
		IFaction MapFaction { get; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060020AC RID: 8364
		bool IsInspected { get; }

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060020AD RID: 8365
		bool IsVisible { get; }

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060020AE RID: 8366
		// (set) Token: 0x060020AF RID: 8367
		bool IsActive { get; set; }
	}
}
