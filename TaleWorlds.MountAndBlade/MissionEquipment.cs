using System;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000256 RID: 598
	public class MissionEquipment
	{
		// Token: 0x060021CF RID: 8655 RVA: 0x0007684C File Offset: 0x00074A4C
		public MissionEquipment()
		{
			this._weaponSlots = new MissionWeapon[5];
			this._cache = default(MissionEquipment.MissionEquipmentCache);
			this._cache.Initialize();
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00076884 File Offset: 0x00074A84
		public MissionEquipment(Equipment spawnEquipment, Banner banner)
			: this()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this._weaponSlots[(int)equipmentIndex] = new MissionWeapon(spawnEquipment[equipmentIndex].Item, spawnEquipment[equipmentIndex].ItemModifier, banner);
			}
		}

		// Token: 0x170006CA RID: 1738
		public MissionWeapon this[int index]
		{
			get
			{
				return this._weaponSlots[index];
			}
			set
			{
				this._weaponSlots[index] = value;
				this._cache.InvalidateOnWeaponSlotUpdated();
				Action onWeaponSlotUpdated = this.OnWeaponSlotUpdated;
				if (onWeaponSlotUpdated == null)
				{
					return;
				}
				onWeaponSlotUpdated();
			}
		}

		// Token: 0x170006CB RID: 1739
		public MissionWeapon this[EquipmentIndex index]
		{
			get
			{
				return this._weaponSlots[(int)index];
			}
			set
			{
				this[(int)index] = value;
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00076924 File Offset: 0x00074B24
		public void FillFrom(MissionEquipment sourceEquipment)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this[equipmentIndex] = new MissionWeapon(sourceEquipment[equipmentIndex].Item, sourceEquipment[equipmentIndex].ItemModifier, null);
			}
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00076968 File Offset: 0x00074B68
		public void FillFrom(Equipment sourceEquipment, Banner banner)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this[equipmentIndex] = new MissionWeapon(sourceEquipment[equipmentIndex].Item, sourceEquipment[equipmentIndex].ItemModifier, banner);
			}
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000769AC File Offset: 0x00074BAC
		private float CalculateGetTotalWeightOfWeapons()
		{
			float num = 0f;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this[equipmentIndex];
				if (!missionWeapon.IsEmpty)
				{
					if (missionWeapon.CurrentUsageItem.IsShield)
					{
						if (missionWeapon.HitPoints > 0)
						{
							num += missionWeapon.GetWeight();
						}
					}
					else
					{
						num += missionWeapon.GetWeight();
					}
				}
			}
			return num;
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x00076A0C File Offset: 0x00074C0C
		public float GetTotalWeightOfWeapons()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			float value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons))
				{
					this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons, this.CalculateGetTotalWeightOfWeapons());
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat.TotalWeightOfWeapons);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00076AB4 File Offset: 0x00074CB4
		public static EquipmentIndex SelectWeaponPickUpSlot(Agent agentPickingUp, MissionWeapon weaponBeingPickedUp, bool isStuckMissile)
		{
			EquipmentIndex equipmentIndex = EquipmentIndex.None;
			if (weaponBeingPickedUp.Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnWeaponChange | ItemFlags.DropOnAnyAction))
			{
				equipmentIndex = EquipmentIndex.ExtraWeaponSlot;
			}
			else
			{
				bool flag = weaponBeingPickedUp.Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand);
				EquipmentIndex equipmentIndex2 = (flag ? agentPickingUp.GetOffhandWieldedItemIndex() : agentPickingUp.GetPrimaryWieldedItemIndex());
				MissionWeapon missionWeapon = ((equipmentIndex2 != EquipmentIndex.None) ? agentPickingUp.Equipment[equipmentIndex2] : MissionWeapon.Invalid);
				if (isStuckMissile)
				{
					bool flag2 = false;
					bool flag3 = false;
					bool isConsumable = weaponBeingPickedUp.Item.PrimaryWeapon.IsConsumable;
					if (isConsumable)
					{
						flag2 = !missionWeapon.IsEmpty && missionWeapon.IsEqualTo(weaponBeingPickedUp) && missionWeapon.HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount);
						flag3 = !missionWeapon.IsEmpty && missionWeapon.IsSameType(weaponBeingPickedUp) && missionWeapon.HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount);
					}
					EquipmentIndex equipmentIndex3 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex4 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex5 = EquipmentIndex.None;
					EquipmentIndex equipmentIndex6 = EquipmentIndex.WeaponItemBeginSlot;
					while (equipmentIndex6 < EquipmentIndex.ExtraWeaponSlot)
					{
						if (!isConsumable)
						{
							goto IL_019F;
						}
						if (equipmentIndex4 != EquipmentIndex.None && !agentPickingUp.Equipment[equipmentIndex6].IsEmpty && agentPickingUp.Equipment[equipmentIndex6].IsEqualTo(weaponBeingPickedUp) && agentPickingUp.Equipment[equipmentIndex6].HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount))
						{
							equipmentIndex4 = equipmentIndex6;
						}
						else
						{
							if (equipmentIndex5 != EquipmentIndex.None || agentPickingUp.Equipment[equipmentIndex6].IsEmpty || !agentPickingUp.Equipment[equipmentIndex6].IsSameType(weaponBeingPickedUp) || !agentPickingUp.Equipment[equipmentIndex6].HasEnoughSpaceForAmount((int)weaponBeingPickedUp.Amount))
							{
								goto IL_019F;
							}
							equipmentIndex5 = equipmentIndex6;
						}
						IL_01C0:
						equipmentIndex6++;
						continue;
						IL_019F:
						if (equipmentIndex3 == EquipmentIndex.None && agentPickingUp.Equipment[equipmentIndex6].IsEmpty)
						{
							equipmentIndex3 = equipmentIndex6;
							goto IL_01C0;
						}
						goto IL_01C0;
					}
					if (flag2)
					{
						equipmentIndex = equipmentIndex2;
					}
					else if (equipmentIndex4 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex5;
					}
					else if (flag3)
					{
						equipmentIndex = equipmentIndex2;
					}
					else if (equipmentIndex5 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex5;
					}
					else if (equipmentIndex3 != EquipmentIndex.None)
					{
						equipmentIndex = equipmentIndex3;
					}
				}
				else
				{
					bool isConsumable2 = weaponBeingPickedUp.Item.PrimaryWeapon.IsConsumable;
					if (isConsumable2 && weaponBeingPickedUp.Amount == 0)
					{
						equipmentIndex = EquipmentIndex.None;
					}
					else
					{
						if (flag && equipmentIndex2 != EquipmentIndex.None)
						{
							for (int i = 0; i < 4; i++)
							{
								if (i != (int)equipmentIndex2 && !agentPickingUp.Equipment[i].IsEmpty && agentPickingUp.Equipment[i].Item.ItemFlags.HasAnyFlag(ItemFlags.HeldInOffHand))
								{
									equipmentIndex = equipmentIndex2;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None && isConsumable2)
						{
							for (EquipmentIndex equipmentIndex7 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex7 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex7++)
							{
								if (!agentPickingUp.Equipment[equipmentIndex7].IsEmpty && agentPickingUp.Equipment[equipmentIndex7].IsSameType(weaponBeingPickedUp) && agentPickingUp.Equipment[equipmentIndex7].Amount < agentPickingUp.Equipment[equipmentIndex7].ModifiedMaxAmount)
								{
									equipmentIndex = equipmentIndex7;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							for (EquipmentIndex equipmentIndex8 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex8 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex8++)
							{
								if (agentPickingUp.Equipment[equipmentIndex8].IsEmpty)
								{
									equipmentIndex = equipmentIndex8;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							for (EquipmentIndex equipmentIndex9 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex9 < EquipmentIndex.ExtraWeaponSlot; equipmentIndex9++)
							{
								if (!agentPickingUp.Equipment[equipmentIndex9].IsEmpty && agentPickingUp.Equipment[equipmentIndex9].IsAnyConsumable() && agentPickingUp.Equipment[equipmentIndex9].Amount == 0)
								{
									equipmentIndex = equipmentIndex9;
									break;
								}
							}
						}
						if (equipmentIndex == EquipmentIndex.None && !missionWeapon.IsEmpty)
						{
							equipmentIndex = equipmentIndex2;
						}
						if (equipmentIndex == EquipmentIndex.None)
						{
							equipmentIndex = EquipmentIndex.WeaponItemBeginSlot;
						}
					}
				}
			}
			return equipmentIndex;
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00076E7C File Offset: 0x0007507C
		public bool HasAmmo(EquipmentIndex equipmentIndex, out int rangedUsageIndex, out bool hasLoadedAmmo, out bool noAmmoInThisSlot)
		{
			hasLoadedAmmo = false;
			noAmmoInThisSlot = false;
			MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex];
			rangedUsageIndex = missionWeapon.GetRangedUsageIndex();
			if (rangedUsageIndex >= 0)
			{
				if (missionWeapon.Ammo > 0)
				{
					hasLoadedAmmo = true;
					return true;
				}
				noAmmoInThisSlot = missionWeapon.IsAnyConsumable() && missionWeapon.Amount == 0;
				for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.NumAllWeaponSlots; equipmentIndex2++)
				{
					MissionWeapon missionWeapon2 = this[(int)equipmentIndex2];
					if (!missionWeapon2.IsEmpty && missionWeapon2.HasAnyUsageWithWeaponClass(missionWeapon.GetWeaponComponentDataForUsage(rangedUsageIndex).AmmoClass) && this[(int)equipmentIndex2].ModifiedMaxAmount > 1 && missionWeapon2.Amount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00076F28 File Offset: 0x00075128
		public int GetAmmoAmount(EquipmentIndex weaponIndex)
		{
			if (this[weaponIndex].IsAnyConsumable() && this[weaponIndex].ModifiedMaxAmount <= 1)
			{
				return (int)this[weaponIndex].ModifiedMaxAmount;
			}
			int num = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[(int)equipmentIndex].IsEmpty && this[(int)equipmentIndex].CurrentUsageItem.WeaponClass == this[weaponIndex].CurrentUsageItem.AmmoClass && this[(int)equipmentIndex].ModifiedMaxAmount > 1)
				{
					num += (int)this[(int)equipmentIndex].Amount;
				}
			}
			return num;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00076FD8 File Offset: 0x000751D8
		public int GetMaxAmmo(EquipmentIndex weaponIndex)
		{
			if (this[weaponIndex].IsAnyConsumable() && this[weaponIndex].ModifiedMaxAmount <= 1)
			{
				return (int)this[weaponIndex].ModifiedMaxAmount;
			}
			int num = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[(int)equipmentIndex].IsEmpty && this[(int)equipmentIndex].CurrentUsageItem.WeaponClass == this[weaponIndex].CurrentUsageItem.AmmoClass && this[(int)equipmentIndex].ModifiedMaxAmount > 1)
				{
					num += (int)this[(int)equipmentIndex].ModifiedMaxAmount;
				}
			}
			return num;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00077088 File Offset: 0x00075288
		public void GetAmmoCountAndIndexOfType(ItemObject.ItemTypeEnum itemType, out int ammoCount, out EquipmentIndex eIndex, EquipmentIndex equippedIndex = EquipmentIndex.None)
		{
			ItemObject.ItemTypeEnum ammoTypeForItemType = ItemObject.GetAmmoTypeForItemType(itemType);
			ItemObject itemObject;
			if (equippedIndex != EquipmentIndex.None)
			{
				itemObject = this[equippedIndex].Item;
				ammoCount = 0;
			}
			else
			{
				itemObject = null;
				ammoCount = -1;
			}
			eIndex = equippedIndex;
			if (ammoTypeForItemType != ItemObject.ItemTypeEnum.Invalid)
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.Weapon3; equipmentIndex >= EquipmentIndex.WeaponItemBeginSlot; equipmentIndex--)
				{
					if (!this[equipmentIndex].IsEmpty && this[equipmentIndex].Item.Type == ammoTypeForItemType)
					{
						int amount = (int)this[equipmentIndex].Amount;
						if (amount > 0)
						{
							if (itemObject == null)
							{
								eIndex = equipmentIndex;
								itemObject = this[equipmentIndex].Item;
								ammoCount = amount;
							}
							else if (itemObject.Id == this[equipmentIndex].Item.Id)
							{
								ammoCount += amount;
							}
						}
					}
				}
			}
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0007715C File Offset: 0x0007535C
		public static bool DoesWeaponFitToSlot(EquipmentIndex slotIndex, MissionWeapon weapon)
		{
			bool flag;
			if (weapon.IsEmpty)
			{
				flag = true;
			}
			else if (weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.DropOnWeaponChange | ItemFlags.DropOnAnyAction))
			{
				flag = slotIndex == EquipmentIndex.ExtraWeaponSlot;
			}
			else
			{
				flag = slotIndex >= EquipmentIndex.WeaponItemBeginSlot && slotIndex < EquipmentIndex.ExtraWeaponSlot;
			}
			return flag;
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000771A4 File Offset: 0x000753A4
		public void CheckLoadedAmmos()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!this[equipmentIndex].IsEmpty && this[equipmentIndex].Item.PrimaryWeapon.WeaponClass == WeaponClass.Crossbow)
				{
					int num;
					EquipmentIndex equipmentIndex2;
					this.GetAmmoCountAndIndexOfType(this[equipmentIndex].Item.Type, out num, out equipmentIndex2, EquipmentIndex.None);
					if (equipmentIndex2 != EquipmentIndex.None)
					{
						MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex2].Consume(MathF.Min(this[equipmentIndex].MaxAmmo, this._weaponSlots[(int)equipmentIndex2].Amount));
						this._weaponSlots[(int)equipmentIndex].ReloadAmmo(missionWeapon, this._weaponSlots[(int)equipmentIndex].ReloadPhaseCount);
					}
				}
			}
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0007727E File Offset: 0x0007547E
		public void SetUsageIndexOfSlot(EquipmentIndex slotIndex, int usageIndex)
		{
			this._weaponSlots[(int)slotIndex].CurrentUsageIndex = usageIndex;
			this._cache.InvalidateOnWeaponUsageIndexUpdated();
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0007729D File Offset: 0x0007549D
		public void SetReloadPhaseOfSlot(EquipmentIndex slotIndex, short reloadPhase)
		{
			this._weaponSlots[(int)slotIndex].ReloadPhase = reloadPhase;
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000772B4 File Offset: 0x000754B4
		public void SetAmountOfSlot(EquipmentIndex slotIndex, short dataValue, bool addOverflowToMaxAmount = false)
		{
			if (addOverflowToMaxAmount)
			{
				short num = dataValue - this._weaponSlots[(int)slotIndex].Amount;
				if (num > 0)
				{
					this._weaponSlots[(int)slotIndex].AddExtraModifiedMaxValue(num);
				}
			}
			short amount = this._weaponSlots[(int)slotIndex].Amount;
			this._weaponSlots[(int)slotIndex].Amount = dataValue;
			this._cache.InvalidateOnWeaponAmmoUpdated();
			if ((amount != 0 && dataValue == 0) || (amount == 0 && dataValue != 0))
			{
				this._cache.InvalidateOnWeaponAmmoAvailabilityChanged();
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00077338 File Offset: 0x00075538
		public void SetHitPointsOfSlot(EquipmentIndex slotIndex, short dataValue, bool addOverflowToMaxHitPoints = false)
		{
			if (addOverflowToMaxHitPoints)
			{
				short num = dataValue - this._weaponSlots[(int)slotIndex].HitPoints;
				if (num > 0)
				{
					this._weaponSlots[(int)slotIndex].AddExtraModifiedMaxValue(num);
				}
			}
			this._weaponSlots[(int)slotIndex].HitPoints = dataValue;
			this._cache.InvalidateOnWeaponHitPointsUpdated();
			if (dataValue == 0)
			{
				this._cache.InvalidateOnWeaponDestroyed();
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000773A0 File Offset: 0x000755A0
		public void SetReloadedAmmoOfSlot(EquipmentIndex slotIndex, EquipmentIndex ammoSlotIndex, short totalAmmo)
		{
			if (ammoSlotIndex == EquipmentIndex.None)
			{
				this._weaponSlots[(int)slotIndex].SetAmmo(MissionWeapon.Invalid);
			}
			else
			{
				MissionWeapon missionWeapon = this._weaponSlots[(int)ammoSlotIndex];
				missionWeapon.Amount = totalAmmo;
				this._weaponSlots[(int)slotIndex].SetAmmo(missionWeapon);
			}
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000773FB File Offset: 0x000755FB
		public void SetConsumedAmmoOfSlot(EquipmentIndex slotIndex, short count)
		{
			this._weaponSlots[(int)slotIndex].ConsumeAmmo(count);
			this._cache.InvalidateOnWeaponAmmoUpdated();
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0007741A File Offset: 0x0007561A
		public void AttachWeaponToWeaponInSlot(EquipmentIndex slotIndex, ref MissionWeapon weapon, ref MatrixFrame attachLocalFrame)
		{
			this._weaponSlots[(int)slotIndex].AttachWeapon(weapon, ref attachLocalFrame);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00077434 File Offset: 0x00075634
		public bool HasShield()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.IsShield)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00077470 File Offset: 0x00075670
		public bool HasAnyWeapon()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (this._weaponSlots[(int)equipmentIndex].CurrentUsageItem != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000774A0 File Offset: 0x000756A0
		public bool HasAnyWeaponWithFlags(WeaponFlags flags)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.WeaponFlags.HasAllFlags(flags))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000774E0 File Offset: 0x000756E0
		public bool HasAnyWeaponWithItemUsageSetFlags(ItemObject.ItemUsageSetFlags flags)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				MissionWeapon missionWeapon = this._weaponSlots[(int)equipmentIndex];
				if (missionWeapon.HasAnyUsageWithItemUsageSetFlags(flags))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x00077514 File Offset: 0x00075714
		public ItemObject GetBanner()
		{
			ItemObject itemObject = null;
			MissionWeapon missionWeapon = this._weaponSlots[4];
			ItemObject item = missionWeapon.Item;
			if (item != null && item.IsBannerItem && item.BannerComponent != null)
			{
				itemObject = item;
			}
			return itemObject;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00077550 File Offset: 0x00075750
		public bool HasRangedWeapon(WeaponClass requiredAmmoClass = WeaponClass.Undefined)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				WeaponComponentData currentUsageItem = this._weaponSlots[(int)equipmentIndex].CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.IsRangedWeapon && (requiredAmmoClass == WeaponClass.Undefined || currentUsageItem.AmmoClass == requiredAmmoClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00077598 File Offset: 0x00075798
		public bool ContainsNonConsumableRangedWeaponWithAmmo()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x00077634 File Offset: 0x00075834
		public bool ContainsMeleeWeapon()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000776D0 File Offset: 0x000758D0
		public bool ContainsShield()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0007776C File Offset: 0x0007596C
		public bool ContainsSpear()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00077808 File Offset: 0x00075A08
		public bool ContainsThrownWeapon()
		{
			this._cacheLock.EnterReadLock();
			try
			{
				if (this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon))
				{
					return this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon);
				}
			}
			finally
			{
				this._cacheLock.ExitReadLock();
			}
			this._cacheLock.EnterWriteLock();
			bool value;
			try
			{
				if (!this._cache.IsValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon))
				{
					this.GatherInformationAndUpdateCache();
				}
				value = this._cache.GetValue(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon);
			}
			finally
			{
				this._cacheLock.ExitWriteLock();
			}
			return value;
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000778A4 File Offset: 0x00075AA4
		private void GatherInformationAndUpdateCache()
		{
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			this.GatherInformation(out flag, out flag2, out flag3, out flag4, out flag5);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsMeleeWeapon, flag);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsShield, flag2);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsSpear, flag3);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsNonConsumableRangedWeaponWithAmmo, flag4);
			this._cache.UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool.ContainsThrownWeapon, flag5);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00077904 File Offset: 0x00075B04
		private void GatherInformation(out bool containsMeleeWeapon, out bool containsShield, out bool containsSpear, out bool containsNonConsumableRangedWeaponWithAmmo, out bool containsThrownWeapon)
		{
			containsMeleeWeapon = false;
			containsShield = false;
			containsSpear = false;
			containsNonConsumableRangedWeaponWithAmmo = false;
			containsThrownWeapon = false;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				bool flag;
				bool flag2;
				bool flag3;
				bool flag4;
				bool flag5;
				WeaponClass weaponClass;
				this._weaponSlots[(int)equipmentIndex].GatherInformationFromWeapon(out flag, out flag2, out flag3, out flag4, out flag5, out weaponClass);
				containsMeleeWeapon = containsMeleeWeapon || flag;
				containsShield = containsShield || flag2;
				containsSpear = containsSpear || flag3;
				containsThrownWeapon = containsThrownWeapon || flag5;
				if (flag4)
				{
					containsNonConsumableRangedWeaponWithAmmo = containsNonConsumableRangedWeaponWithAmmo || this.GetAmmoAmount(equipmentIndex) > 0;
				}
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00077980 File Offset: 0x00075B80
		public void SetGlossMultipliersOfWeaponsRandomly(int seed)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				this._weaponSlots[(int)equipmentIndex].SetRandomGlossMultiplier(seed);
			}
		}

		// Token: 0x04000D30 RID: 3376
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

		// Token: 0x04000D31 RID: 3377
		public Action OnWeaponSlotUpdated;

		// Token: 0x04000D32 RID: 3378
		private readonly MissionWeapon[] _weaponSlots;

		// Token: 0x04000D33 RID: 3379
		private MissionEquipment.MissionEquipmentCache _cache;

		// Token: 0x0200053E RID: 1342
		private struct MissionEquipmentCache
		{
			// Token: 0x06003C65 RID: 15461 RVA: 0x000F0C0B File Offset: 0x000EEE0B
			public void Initialize()
			{
				this._cachedBool = default(StackArray.StackArray5Bool);
				this._validity = default(StackArray.StackArray6Bool);
			}

			// Token: 0x06003C66 RID: 15462 RVA: 0x000F0C25 File Offset: 0x000EEE25
			public bool IsValid(MissionEquipment.MissionEquipmentCache.CachedBool queriedData)
			{
				return this._validity[(int)queriedData];
			}

			// Token: 0x06003C67 RID: 15463 RVA: 0x000F0C34 File Offset: 0x000EEE34
			public void UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedBool data, bool value)
			{
				this._cachedBool[(int)data] = value;
				this._validity[(int)data] = true;
			}

			// Token: 0x06003C68 RID: 15464 RVA: 0x000F0C5D File Offset: 0x000EEE5D
			public bool GetValue(MissionEquipment.MissionEquipmentCache.CachedBool data)
			{
				return this._cachedBool[(int)data];
			}

			// Token: 0x06003C69 RID: 15465 RVA: 0x000F0C6B File Offset: 0x000EEE6B
			public bool IsValid(MissionEquipment.MissionEquipmentCache.CachedFloat queriedData)
			{
				return this._validity[(int)(5 + queriedData)];
			}

			// Token: 0x06003C6A RID: 15466 RVA: 0x000F0C7C File Offset: 0x000EEE7C
			public void UpdateAndMarkValid(MissionEquipment.MissionEquipmentCache.CachedFloat data, float value)
			{
				this._cachedFloat = value;
				this._validity[(int)(5 + data)] = true;
			}

			// Token: 0x06003C6B RID: 15467 RVA: 0x000F0CA1 File Offset: 0x000EEEA1
			public float GetValue(MissionEquipment.MissionEquipmentCache.CachedFloat data)
			{
				return this._cachedFloat;
			}

			// Token: 0x06003C6C RID: 15468 RVA: 0x000F0CAC File Offset: 0x000EEEAC
			public void InvalidateOnWeaponSlotUpdated()
			{
				this._validity[0] = false;
				this._validity[1] = false;
				this._validity[2] = false;
				this._validity[3] = false;
				this._validity[4] = false;
				this._validity[5] = false;
			}

			// Token: 0x06003C6D RID: 15469 RVA: 0x000F0D07 File Offset: 0x000EEF07
			public void InvalidateOnWeaponUsageIndexUpdated()
			{
			}

			// Token: 0x06003C6E RID: 15470 RVA: 0x000F0D09 File Offset: 0x000EEF09
			public void InvalidateOnWeaponAmmoUpdated()
			{
				this._validity[5] = false;
			}

			// Token: 0x06003C6F RID: 15471 RVA: 0x000F0D18 File Offset: 0x000EEF18
			public void InvalidateOnWeaponAmmoAvailabilityChanged()
			{
				this._validity[3] = false;
			}

			// Token: 0x06003C70 RID: 15472 RVA: 0x000F0D27 File Offset: 0x000EEF27
			public void InvalidateOnWeaponHitPointsUpdated()
			{
				this._validity[5] = false;
			}

			// Token: 0x06003C71 RID: 15473 RVA: 0x000F0D36 File Offset: 0x000EEF36
			public void InvalidateOnWeaponDestroyed()
			{
				this._validity[1] = false;
			}

			// Token: 0x04001D7E RID: 7550
			private const int CachedBoolCount = 5;

			// Token: 0x04001D7F RID: 7551
			private const int CachedFloatCount = 1;

			// Token: 0x04001D80 RID: 7552
			private float _cachedFloat;

			// Token: 0x04001D81 RID: 7553
			private StackArray.StackArray5Bool _cachedBool;

			// Token: 0x04001D82 RID: 7554
			private StackArray.StackArray6Bool _validity;

			// Token: 0x020006B3 RID: 1715
			public enum CachedBool
			{
				// Token: 0x04002312 RID: 8978
				ContainsMeleeWeapon,
				// Token: 0x04002313 RID: 8979
				ContainsShield,
				// Token: 0x04002314 RID: 8980
				ContainsSpear,
				// Token: 0x04002315 RID: 8981
				ContainsNonConsumableRangedWeaponWithAmmo,
				// Token: 0x04002316 RID: 8982
				ContainsThrownWeapon,
				// Token: 0x04002317 RID: 8983
				Count
			}

			// Token: 0x020006B4 RID: 1716
			public enum CachedFloat
			{
				// Token: 0x04002319 RID: 8985
				TotalWeightOfWeapons,
				// Token: 0x0400231A RID: 8986
				Count
			}
		}
	}
}
