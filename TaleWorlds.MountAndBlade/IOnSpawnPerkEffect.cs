using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030E RID: 782
	public interface IOnSpawnPerkEffect
	{
		// Token: 0x06002CA9 RID: 11433
		int GetExtraTroopCount();

		// Token: 0x06002CAA RID: 11434
		List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAll = false);

		// Token: 0x06002CAB RID: 11435
		float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue);

		// Token: 0x06002CAC RID: 11436
		float GetHitpoints(bool isPlayer);
	}
}
