using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019D RID: 413
	public abstract class FleetManagementModel : MBGameModel<FleetManagementModel>
	{
		// Token: 0x06001C88 RID: 7304
		public abstract bool CanTroopsReturn();

		// Token: 0x06001C89 RID: 7305
		public abstract CampaignTime GetReturnTimeForTroops(Ship ship);

		// Token: 0x06001C8A RID: 7306
		public abstract bool CanSendShipToPlayerClan(Ship ship, int playerShipsCount, int troopsCountToSend, out TextObject hint);

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001C8B RID: 7307
		public abstract int MinimumTroopCountRequiredToSendShips { get; }
	}
}
