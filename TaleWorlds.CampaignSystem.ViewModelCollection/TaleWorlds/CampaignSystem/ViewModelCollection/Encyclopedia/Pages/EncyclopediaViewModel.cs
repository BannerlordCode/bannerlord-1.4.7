using System;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D7 RID: 215
	public class EncyclopediaViewModel : Attribute
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x00051FCE File Offset: 0x000501CE
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x00051FD6 File Offset: 0x000501D6
		public Type PageTargetType { get; private set; }

		// Token: 0x0600149B RID: 5275 RVA: 0x00051FDF File Offset: 0x000501DF
		public EncyclopediaViewModel(Type pageTargetType)
		{
			this.PageTargetType = pageTargetType;
		}
	}
}
