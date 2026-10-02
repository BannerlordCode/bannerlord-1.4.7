using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000314 RID: 788
	public interface IReadOnlyPerkObject
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002CDF RID: 11487
		TextObject Name { get; }

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002CE0 RID: 11488
		TextObject Description { get; }

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002CE1 RID: 11489
		List<string> GameModes { get; }

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002CE2 RID: 11490
		int PerkListIndex { get; }

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002CE3 RID: 11491
		string IconId { get; }

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002CE4 RID: 11492
		string HeroIdleAnimOverride { get; }

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002CE5 RID: 11493
		string HeroMountIdleAnimOverride { get; }

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002CE6 RID: 11494
		string TroopIdleAnimOverride { get; }

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002CE7 RID: 11495
		string TroopMountIdleAnimOverride { get; }

		// Token: 0x06002CE8 RID: 11496
		int GetExtraTroopCount(bool isWarmup);

		// Token: 0x06002CE9 RID: 11497
		List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isWarmup, bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAllEquipments = false);

		// Token: 0x06002CEA RID: 11498
		float GetDrivenPropertyBonusOnSpawn(bool isWarmup, bool isPlayer, DrivenProperty drivenProperty, float baseValue);

		// Token: 0x06002CEB RID: 11499
		float GetHitpoints(bool isWarmup, bool isPlayer);

		// Token: 0x06002CEC RID: 11500
		MPPerkObject Clone(MissionPeer peer);
	}
}
