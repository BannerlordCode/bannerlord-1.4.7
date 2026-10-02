using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameMenus;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000B9 RID: 185
	public static class GameMenuOverlayFactory
	{
		// Token: 0x0600124D RID: 4685 RVA: 0x0004A29D File Offset: 0x0004849D
		public static void RegisterProvider(IGameMenuOverlayProvider provider)
		{
			GameMenuOverlayFactory._providers.Add(provider);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004A2AA File Offset: 0x000484AA
		public static void UnregisterProvider(IGameMenuOverlayProvider provider)
		{
			GameMenuOverlayFactory._providers.Remove(provider);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0004A2B8 File Offset: 0x000484B8
		public static GameMenuOverlay GetOverlay(GameMenu.MenuOverlayType menuOverlayType)
		{
			for (int i = GameMenuOverlayFactory._providers.Count - 1; i >= 0; i--)
			{
				GameMenuOverlay overlay = GameMenuOverlayFactory._providers[i].GetOverlay(menuOverlayType);
				if (overlay != null)
				{
					return overlay;
				}
			}
			return null;
		}

		// Token: 0x0400085A RID: 2138
		private static List<IGameMenuOverlayProvider> _providers = new List<IGameMenuOverlayProvider>();
	}
}
