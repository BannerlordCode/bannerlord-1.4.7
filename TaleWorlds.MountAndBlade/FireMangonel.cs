using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000346 RID: 838
	public class FireMangonel : Mangonel
	{
		// Token: 0x06002F47 RID: 12103 RVA: 0x000B8E01 File Offset: 0x000B7001
		public override SiegeEngineType GetSiegeEngineType()
		{
			if (this.DefaultSide != BattleSideEnum.Attacker)
			{
				return DefaultSiegeEngineTypes.FireCatapult;
			}
			return DefaultSiegeEngineTypes.FireOnager;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000B8E18 File Offset: 0x000B7018
		public override float ProcessTargetValue(float baseValue, TargetFlags flags)
		{
			if (flags.HasAnyFlag(TargetFlags.NotAThreat))
			{
				return -1000f;
			}
			if (flags.HasAllFlags(TargetFlags.IsFlammable | TargetFlags.IsSiegeEngine))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeEngine))
			{
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsStructure))
			{
				baseValue *= 1.5f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSmall))
			{
				baseValue *= 8f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsMoving))
			{
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.DebugThreat))
			{
				baseValue *= 10f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsSiegeTower))
			{
				baseValue *= 12f;
			}
			if (flags.HasAnyFlag(TargetFlags.IsFlammable))
			{
				baseValue *= 100f;
			}
			return baseValue;
		}
	}
}
