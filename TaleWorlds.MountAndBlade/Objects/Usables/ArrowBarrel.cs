using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003A6 RID: 934
	public class ArrowBarrel : AmmoBarrelBase
	{
		// Token: 0x06003511 RID: 13585 RVA: 0x000DA38F File Offset: 0x000D858F
		protected override int GetSoundEvent()
		{
			return SoundEvent.GetEventIdFromString(this._pickupSoundEventString);
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000DA39C File Offset: 0x000D859C
		protected override WeaponClass[] GetRequiredWeaponClasses()
		{
			return new WeaponClass[]
			{
				WeaponClass.Arrow,
				WeaponClass.Bolt
			};
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000DA3AE File Offset: 0x000D85AE
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=bWi4aMO9}Arrow Barrel", null);
		}

		// Token: 0x04001687 RID: 5767
		private readonly string _pickupSoundEventString = "event:/mission/combat/pickup_arrows";
	}
}
