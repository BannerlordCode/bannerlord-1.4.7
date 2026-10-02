using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000207 RID: 519
	public static class CosmeticsManagerHelper
	{
		// Token: 0x06001E17 RID: 7703 RVA: 0x00067AEC File Offset: 0x00065CEC
		public static Dictionary<int, List<int>> GetUsedIndicesFromIds(Dictionary<string, List<string>> usedCosmetics)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in usedCosmetics)
			{
				int num = -1;
				for (int i = 0; i < objectTypeList.Count; i++)
				{
					if (objectTypeList[i].StringId == keyValuePair.Key)
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					List<int> list = new List<int>();
					foreach (string text in keyValuePair.Value)
					{
						int num2 = -1;
						for (int j = 0; j < CosmeticsManager.CosmeticElementsList.Count; j++)
						{
							if (CosmeticsManager.CosmeticElementsList[j].Id == text)
							{
								num2 = j;
								break;
							}
						}
						if (num2 >= 0)
						{
							list.Add(num2);
						}
					}
					if (list.Count > 0)
					{
						dictionary.Add(num, list);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x00067C30 File Offset: 0x00065E30
		public static ActionIndexCache GetSuitableTauntAction(Agent agent, int tauntIndex)
		{
			if (agent.Equipment == null)
			{
				return ActionIndexCache.act_none;
			}
			WeaponComponentData currentUsageItem = agent.WieldedWeapon.CurrentUsageItem;
			WeaponComponentData currentUsageItem2 = agent.WieldedOffhandWeapon.CurrentUsageItem;
			return ActionIndexCache.Create(TauntUsageManager.Instance.GetAction(tauntIndex, agent.GetIsLeftStance(), !agent.HasMount, currentUsageItem, currentUsageItem2));
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x00067C8C File Offset: 0x00065E8C
		public static TauntUsageManager.TauntUsage.TauntUsageFlag GetActionNotUsableReason(Agent agent, int tauntIndex)
		{
			WeaponComponentData currentUsageItem = agent.WieldedWeapon.CurrentUsageItem;
			WeaponComponentData currentUsageItem2 = agent.WieldedOffhandWeapon.CurrentUsageItem;
			return TauntUsageManager.Instance.GetIsActionNotSuitableReason(tauntIndex, agent.GetIsLeftStance(), !agent.HasMount, currentUsageItem, currentUsageItem2);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x00067CD4 File Offset: 0x00065ED4
		public static string GetSuitableTauntActionForEquipment(Equipment equipment, TauntCosmeticElement taunt)
		{
			if (equipment == null)
			{
				return null;
			}
			EquipmentIndex equipmentIndex;
			EquipmentIndex equipmentIndex2;
			bool flag;
			equipment.GetInitialWeaponIndicesToEquip(out equipmentIndex, out equipmentIndex2, out flag, Equipment.InitialWeaponEquipPreference.Any);
			WeaponComponentData weaponComponentData;
			if (equipmentIndex == EquipmentIndex.None)
			{
				weaponComponentData = null;
			}
			else
			{
				ItemObject item = equipment[equipmentIndex].Item;
				weaponComponentData = ((item != null) ? item.PrimaryWeapon : null);
			}
			WeaponComponentData weaponComponentData2 = weaponComponentData;
			WeaponComponentData weaponComponentData3;
			if (equipmentIndex2 == EquipmentIndex.None)
			{
				weaponComponentData3 = null;
			}
			else
			{
				ItemObject item2 = equipment[equipmentIndex2].Item;
				weaponComponentData3 = ((item2 != null) ? item2.PrimaryWeapon : null);
			}
			WeaponComponentData weaponComponentData4 = weaponComponentData3;
			return TauntUsageManager.Instance.GetAction(TauntUsageManager.Instance.GetIndexOfAction(taunt.Id), false, true, weaponComponentData2, weaponComponentData4);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x00067D5B File Offset: 0x00065F5B
		public static bool IsWeaponClassOneHanded(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.OneHandedAxe || weaponClass == WeaponClass.OneHandedPolearm || weaponClass == WeaponClass.OneHandedSword;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x00067D6C File Offset: 0x00065F6C
		public static bool IsWeaponClassTwoHanded(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.TwoHandedAxe || weaponClass == WeaponClass.TwoHandedMace || weaponClass == WeaponClass.TwoHandedPolearm || weaponClass == WeaponClass.TwoHandedSword;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00067D81 File Offset: 0x00065F81
		public static bool IsWeaponClassShield(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.LargeShield || weaponClass == WeaponClass.SmallShield;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x00067D8F File Offset: 0x00065F8F
		public static bool IsWeaponClassBow(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.Bow;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x00067D96 File Offset: 0x00065F96
		public static bool IsWeaponClassCrossbow(WeaponClass weaponClass)
		{
			return weaponClass == WeaponClass.Crossbow;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00067DA0 File Offset: 0x00065FA0
		public static WeaponClass[] GetComplimentaryWeaponClasses(WeaponClass weaponClass)
		{
			switch (weaponClass)
			{
			case WeaponClass.OneHandedSword:
			case WeaponClass.OneHandedAxe:
			case WeaponClass.Mace:
			case WeaponClass.Pick:
			case WeaponClass.OneHandedPolearm:
			case WeaponClass.LowGripPolearm:
			case WeaponClass.Stone:
			case WeaponClass.ThrowingAxe:
			case WeaponClass.ThrowingKnife:
			case WeaponClass.Javelin:
			case WeaponClass.BallistaStone:
				return new WeaponClass[]
				{
					WeaponClass.SmallShield,
					WeaponClass.LargeShield
				};
			case WeaponClass.Arrow:
				return new WeaponClass[] { WeaponClass.Bow };
			case WeaponClass.Bolt:
				return new WeaponClass[] { WeaponClass.Crossbow };
			case WeaponClass.SlingStone:
				return new WeaponClass[] { WeaponClass.Sling };
			case WeaponClass.Bow:
				return new WeaponClass[] { WeaponClass.Arrow };
			case WeaponClass.Crossbow:
				return new WeaponClass[] { WeaponClass.Bolt };
			case WeaponClass.Sling:
				return new WeaponClass[] { WeaponClass.SlingStone };
			case WeaponClass.SmallShield:
			case WeaponClass.LargeShield:
				return new WeaponClass[]
				{
					WeaponClass.OneHandedAxe,
					WeaponClass.OneHandedSword,
					WeaponClass.OneHandedPolearm,
					WeaponClass.Mace
				};
			}
			return new WeaponClass[0];
		}
	}
}
