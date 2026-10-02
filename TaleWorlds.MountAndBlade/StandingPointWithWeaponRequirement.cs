using System;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200035C RID: 860
	public class StandingPointWithWeaponRequirement : StandingPoint
	{
		// Token: 0x06003142 RID: 12610 RVA: 0x000C7FA6 File Offset: 0x000C61A6
		public StandingPointWithWeaponRequirement()
		{
			this.AutoSheathWeapons = false;
			this._requiredWeaponClasses = new WeaponClass[0];
			this._hasAlternative = base.HasAlternative();
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000C7FCD File Offset: 0x000C61CD
		protected internal override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000C7FD5 File Offset: 0x000C61D5
		public void InitRequiredWeaponClasses(WeaponClass[] requiredWeaponClasses)
		{
			this._requiredWeaponClasses = requiredWeaponClasses;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000C7FDE File Offset: 0x000C61DE
		public void InitRequiredWeapon(ItemObject weapon)
		{
			this._requiredWeapon = weapon;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000C7FE7 File Offset: 0x000C61E7
		public void InitGivenWeapon(ItemObject weapon)
		{
			this._givenWeapon = weapon;
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000C7FF0 File Offset: 0x000C61F0
		public override bool IsDisabledForAgent(Agent agent)
		{
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			if (this._requiredWeapon != null)
			{
				if (primaryWieldedItemIndex != EquipmentIndex.None && agent.Equipment[primaryWieldedItemIndex].Item == this._requiredWeapon)
				{
					return base.IsDisabledForAgent(agent);
				}
			}
			else if (this._givenWeapon != null)
			{
				if (primaryWieldedItemIndex == EquipmentIndex.None || agent.Equipment[primaryWieldedItemIndex].Item != this._givenWeapon)
				{
					return base.IsDisabledForAgent(agent);
				}
			}
			else if (!this._requiredWeaponClasses.IsEmpty<WeaponClass>())
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					if (!agent.Equipment[equipmentIndex].IsEmpty && this._requiredWeaponClasses.Contains(agent.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass) && (!agent.Equipment[equipmentIndex].CurrentUsageItem.IsConsumable || agent.Equipment[equipmentIndex].Amount < agent.Equipment[equipmentIndex].ModifiedMaxAmount || equipmentIndex == EquipmentIndex.ExtraWeaponSlot))
					{
						return base.IsDisabledForAgent(agent);
					}
				}
			}
			return true;
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000C811D File Offset: 0x000C631D
		public void SetHasAlternative(bool hasAlternative)
		{
			this._hasAlternative = hasAlternative;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000C8126 File Offset: 0x000C6326
		public override bool HasAlternative()
		{
			return this._hasAlternative;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000C812E File Offset: 0x000C632E
		public void SetUsingBattleSide(BattleSideEnum side)
		{
			this.StandingPointSide = side;
		}

		// Token: 0x040014A9 RID: 5289
		private ItemObject _requiredWeapon;

		// Token: 0x040014AA RID: 5290
		private ItemObject _givenWeapon;

		// Token: 0x040014AB RID: 5291
		private WeaponClass[] _requiredWeaponClasses;

		// Token: 0x040014AC RID: 5292
		private bool _hasAlternative;
	}
}
