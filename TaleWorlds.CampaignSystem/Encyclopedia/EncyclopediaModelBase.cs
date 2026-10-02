using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200017B RID: 379
	public abstract class EncyclopediaModelBase : Attribute
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0008E14D File Offset: 0x0008C34D
		// (set) Token: 0x06001B89 RID: 7049 RVA: 0x0008E155 File Offset: 0x0008C355
		public Type[] PageTargetTypes { get; private set; }

		// Token: 0x06001B8A RID: 7050 RVA: 0x0008E15E File Offset: 0x0008C35E
		public EncyclopediaModelBase(Type[] pageTargetTypes)
		{
			this.PageTargetTypes = pageTargetTypes;
		}
	}
}
