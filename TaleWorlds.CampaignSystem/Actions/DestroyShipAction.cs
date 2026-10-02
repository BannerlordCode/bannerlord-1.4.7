using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004AA RID: 1194
	public static class DestroyShipAction
	{
		// Token: 0x06004A70 RID: 19056 RVA: 0x00178CCC File Offset: 0x00176ECC
		private static void ApplyInternal(Ship ship, DestroyShipAction.ShipDestroyDetail detail)
		{
			PartyBase owner = ship.Owner;
			if (owner != null)
			{
				MobileParty mobileParty = owner.MobileParty;
				if (mobileParty != null)
				{
					mobileParty.SetNavalVisualAsDirty();
				}
			}
			ship.Owner = null;
			CampaignEventDispatcher.Instance.OnShipDestroyed(owner, ship, detail);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00178D08 File Offset: 0x00176F08
		public static void Apply(Ship ship)
		{
			DestroyShipAction.ApplyInternal(ship, DestroyShipAction.ShipDestroyDetail.ApplyDefault);
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x00178D11 File Offset: 0x00176F11
		public static void ApplyByDiscard(Ship ship)
		{
			DestroyShipAction.ApplyInternal(ship, DestroyShipAction.ShipDestroyDetail.ApplyByDiscard);
		}

		// Token: 0x02000895 RID: 2197
		public enum ShipDestroyDetail
		{
			// Token: 0x040024AE RID: 9390
			ApplyDefault,
			// Token: 0x040024AF RID: 9391
			ApplyByDiscard
		}
	}
}
