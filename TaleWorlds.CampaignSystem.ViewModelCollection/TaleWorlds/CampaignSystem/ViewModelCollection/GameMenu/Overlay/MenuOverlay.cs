using System;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BC RID: 188
	public class MenuOverlay : Attribute
	{
		// Token: 0x06001297 RID: 4759 RVA: 0x0004B5D3 File Offset: 0x000497D3
		public MenuOverlay(string typeId)
		{
			this.TypeId = typeId;
		}

		// Token: 0x0400087A RID: 2170
		public new string TypeId;
	}
}
