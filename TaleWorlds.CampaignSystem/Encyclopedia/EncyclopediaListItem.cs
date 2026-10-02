using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000175 RID: 373
	public struct EncyclopediaListItem
	{
		// Token: 0x06001B58 RID: 7000 RVA: 0x0008D9CD File Offset: 0x0008BBCD
		public EncyclopediaListItem(object obj, string name, string description, string id, string typeName, bool playerCanSeeValues, Action onShowTooltip = null)
		{
			this.Object = obj;
			this.Name = name;
			this.Description = description;
			this.Id = id;
			this.TypeName = typeName;
			this.PlayerCanSeeValues = playerCanSeeValues;
			this.OnShowTooltip = onShowTooltip;
		}

		// Token: 0x04000936 RID: 2358
		public readonly object Object;

		// Token: 0x04000937 RID: 2359
		public readonly string Name;

		// Token: 0x04000938 RID: 2360
		public readonly string Description;

		// Token: 0x04000939 RID: 2361
		public readonly string Id;

		// Token: 0x0400093A RID: 2362
		public readonly string TypeName;

		// Token: 0x0400093B RID: 2363
		public readonly bool PlayerCanSeeValues;

		// Token: 0x0400093C RID: 2364
		public readonly Action OnShowTooltip;
	}
}
