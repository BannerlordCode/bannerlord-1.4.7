using System;
using TaleWorlds.CampaignSystem.GameMenus;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BB RID: 187
	public interface IGameMenuOverlayProvider
	{
		// Token: 0x06001296 RID: 4758
		GameMenuOverlay GetOverlay(GameMenu.MenuOverlayType menuOverlayType);
	}
}
