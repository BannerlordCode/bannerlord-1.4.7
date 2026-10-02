using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000046 RID: 70
	public struct CraftingStatData
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00015582 File Offset: 0x00013782
		public bool IsValid
		{
			get
			{
				return this.MaxValue >= 0f;
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00015594 File Offset: 0x00013794
		public CraftingStatData(TextObject descriptionText, float curValue, float maxValue, CraftingTemplate.CraftingStatTypes type, DamageTypes damageType = DamageTypes.Invalid)
		{
			this.DescriptionText = descriptionText;
			this.CurValue = curValue;
			this.MaxValue = maxValue;
			this.Type = type;
			this.DamageType = damageType;
		}

		// Token: 0x040002C8 RID: 712
		public readonly TextObject DescriptionText;

		// Token: 0x040002C9 RID: 713
		public readonly float CurValue;

		// Token: 0x040002CA RID: 714
		public readonly float MaxValue;

		// Token: 0x040002CB RID: 715
		public readonly CraftingTemplate.CraftingStatTypes Type;

		// Token: 0x040002CC RID: 716
		public readonly DamageTypes DamageType;
	}
}
