using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000115 RID: 277
	public class DefaultEquipmentSelectionModel : EquipmentSelectionModel
	{
		// Token: 0x060017F6 RID: 6134 RVA: 0x00073468 File Offset: 0x00071668
		public override Equipment GetEquipmentForHeroComeOfAge(Hero hero, Equipment.EquipmentType equipmentType)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, equipmentType);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00073480 File Offset: 0x00071680
		public override Equipment GetEquipmentForHeroReachesTeenAge(Hero hero)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate | EquipmentCategories.IsTeenagerEquipmentTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0007349C File Offset: 0x0007169C
		public override Equipment GetEquipmentForDeliveredOffspring(Hero hero)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate | EquipmentCategories.IsChildEquipmentTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x000734B4 File Offset: 0x000716B4
		public override Equipment GetEquipmentForCompanionWhenTurningToLord(Hero companionHero, Equipment.EquipmentType equipmentType)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			return this.GetSuitableEquipmentSet(companionHero, equipmentCategories, equipmentType);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x000734CC File Offset: 0x000716CC
		public override Equipment GetEquipmentForInitialChildrenGeneration(Hero hero)
		{
			bool flag = hero.Age < (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge;
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			if (flag)
			{
				equipmentCategories |= EquipmentCategories.IsChildEquipmentTemplate;
			}
			else
			{
				equipmentCategories |= EquipmentCategories.IsTeenagerEquipmentTemplate;
			}
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00073510 File Offset: 0x00071710
		public override ValueTuple<Equipment, Equipment> GetEquipmentsForChangingRuler(Hero newRuler, Hero oldRuler, Equipment.EquipmentType equipmentType)
		{
			Equipment equipment = null;
			Equipment equipment2 = null;
			if (newRuler != Hero.MainHero)
			{
				equipment = this.GetSuitableEquipmentSet(newRuler, EquipmentCategories.IsKingdomRulerTemplate, equipmentType);
			}
			if (oldRuler != null && oldRuler.IsActive && oldRuler != Hero.MainHero)
			{
				equipment2 = this.GetSuitableEquipmentSet(oldRuler, EquipmentCategories.IsLordTemplate, equipmentType);
			}
			return new ValueTuple<Equipment, Equipment>(equipment, equipment2);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00073558 File Offset: 0x00071758
		private Equipment GetSuitableEquipmentSet(Hero hero, EquipmentCategories customFlags, Equipment.EquipmentType equipmentType)
		{
			MBList<Equipment> mblist = new MBList<Equipment>();
			if (hero.IsFemale)
			{
				customFlags |= EquipmentCategories.IsFemaleTemplate;
			}
			foreach (MBEquipmentRoster mbequipmentRoster in MBEquipmentRosterExtensions.All)
			{
				if (this.IsRosterAppropriateForHeroAsTemplate(mbequipmentRoster, hero.Culture, customFlags))
				{
					foreach (Equipment equipment in mbequipmentRoster.AllEquipments)
					{
						if (equipment.ItemEquipmentType == equipmentType)
						{
							mblist.Add(equipment);
						}
					}
				}
			}
			return mblist.GetRandomElement<Equipment>();
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0007361C File Offset: 0x0007181C
		private bool IsRosterAppropriateForHeroAsTemplate(MBEquipmentRoster equipmentRoster, CultureObject culture, EquipmentCategories customFlags)
		{
			bool flag = false;
			if (equipmentRoster.EquipmentCulture == culture && equipmentRoster.EquipmentCategories == customFlags)
			{
				flag = true;
			}
			return flag;
		}
	}
}
