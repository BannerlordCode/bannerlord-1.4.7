using System;
using System.Collections.Generic;
using TaleWorlds.Core.SaveCompability;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000C9 RID: 201
	public class SaveableCoreTypeDefiner : SaveableTypeDefiner
	{
		// Token: 0x06000ADF RID: 2783 RVA: 0x000230C8 File Offset: 0x000212C8
		public SaveableCoreTypeDefiner()
			: base(10000)
		{
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000230D8 File Offset: 0x000212D8
		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(ArmorComponent), 2, null);
			base.AddClassDefinition(typeof(Banner), 3, null);
			base.AddClassDefinition(typeof(BannerData), 4, null);
			base.AddClassDefinition(typeof(BasicCharacterObject), 5, null);
			base.AddClassDefinition(typeof(CharacterAttribute), 6, null);
			base.AddClassDefinition(typeof(WeaponDesign), 9, null);
			base.AddClassDefinition(typeof(CraftingPiece), 10, null);
			base.AddClassDefinition(typeof(CraftingTemplate), 11, null);
			base.AddClassDefinition(typeof(EntitySystem<>), 15, null);
			base.AddClassDefinition(typeof(Equipment), 16, null);
			base.AddClassDefinition(typeof(TradeItemComponent), 18, null);
			base.AddClassDefinition(typeof(GameType), 26, null);
			base.AddClassDefinition(typeof(HorseComponent), 27, null);
			base.AddClassDefinition(typeof(ItemCategory), 28, null);
			base.AddClassDefinition(typeof(ItemComponent), 29, null);
			base.AddClassDefinition(typeof(ItemModifier), 30, null);
			base.AddClassDefinition(typeof(ItemModifierGroup), 31, null);
			base.AddClassDefinition(typeof(ItemObject), 32, null);
			base.AddClassDefinition(typeof(MissionResult), 36, null);
			base.AddClassDefinition(typeof(PropertyObject), 38, null);
			base.AddClassDefinition(typeof(SkillObject), 39, null);
			base.AddClassDefinition(typeof(PropertyOwner<>), 40, null);
			base.AddClassDefinition(typeof(PropertyOwnerF<>), 41, null);
			base.AddClassDefinition(typeof(SiegeEngineType), 42, null);
			base.AddClassDefinition(typeof(WeaponDesignElement), 44, null);
			base.AddClassDefinition(typeof(WeaponComponent), 45, null);
			base.AddClassDefinition(typeof(WeaponComponentData), 46, null);
			base.AddClassDefinition(typeof(InformationData), 50, null);
			base.AddClassDefinition(typeof(MBFastRandom), 52, null);
			base.AddClassDefinition(typeof(BannerComponent), 53, null);
			base.AddClassDefinition(typeof(ShipHull), 54, null);
			base.AddClassDefinition(typeof(ShipUpgradePiece), 55, null);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00023340 File Offset: 0x00021540
		protected override void DefineStructTypes()
		{
			base.AddStructDefinition(typeof(ItemRosterElement), 1004, null);
			base.AddStructDefinition(typeof(UniqueTroopDescriptor), 1006, null);
			base.AddStructDefinition(typeof(StaticBodyProperties), 1009, null);
			base.AddStructDefinition(typeof(EquipmentElement), 1011, null);
			base.AddStructDefinition(typeof(AgentSaveData), 1012, null);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000233BC File Offset: 0x000215BC
		protected override void DefineEnumTypes()
		{
			base.AddEnumDefinition(typeof(BattleSideEnum), 2001, null);
			base.AddEnumDefinition(typeof(Equipment.EquipmentType), 2006, null);
			base.AddEnumDefinition(typeof(WeaponFlags), 2007, null);
			base.AddEnumDefinition(typeof(FormationClass), 2008, null);
			base.AddEnumDefinition(typeof(BattleState), 2009, null);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00023437 File Offset: 0x00021637
		protected override void DefineInterfaceTypes()
		{
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00023439 File Offset: 0x00021639
		protected override void DefineConflictResolvers()
		{
			base.AddConflictResolver(8, new CharacterSkillsResolver());
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00023447 File Offset: 0x00021647
		protected override void DefineRootClassTypes()
		{
			base.AddRootClassDefinition(typeof(Game), 4001, null);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002345F File Offset: 0x0002165F
		protected override void DefineGenericClassDefinitions()
		{
			base.ConstructGenericClassDefinition(typeof(Tuple<int, int>));
			base.ConstructGenericClassDefinition(typeof(PropertyOwner<SkillObject>));
			base.ConstructGenericClassDefinition(typeof(PropertyOwner<CharacterAttribute>));
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00023491 File Offset: 0x00021691
		protected override void DefineGenericStructDefinitions()
		{
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00023494 File Offset: 0x00021694
		protected override void DefineContainerDefinitions()
		{
			base.ConstructContainerDefinition(typeof(ItemRosterElement[]));
			base.ConstructContainerDefinition(typeof(EquipmentElement[]));
			base.ConstructContainerDefinition(typeof(Equipment[]));
			base.ConstructContainerDefinition(typeof(WeaponDesignElement[]));
			base.ConstructContainerDefinition(typeof(List<ItemObject>));
			base.ConstructContainerDefinition(typeof(List<ItemComponent>));
			base.ConstructContainerDefinition(typeof(List<ItemModifier>));
			base.ConstructContainerDefinition(typeof(List<ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(List<CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(List<SkillObject>));
			base.ConstructContainerDefinition(typeof(List<ItemCategory>));
			base.ConstructContainerDefinition(typeof(List<CraftingPiece>));
			base.ConstructContainerDefinition(typeof(List<CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(List<SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(List<PropertyObject>));
			base.ConstructContainerDefinition(typeof(List<UniqueTroopDescriptor>));
			base.ConstructContainerDefinition(typeof(List<Equipment>));
			base.ConstructContainerDefinition(typeof(List<BannerData>));
			base.ConstructContainerDefinition(typeof(List<EquipmentElement>));
			base.ConstructContainerDefinition(typeof(List<WeaponDesign>));
			base.ConstructContainerDefinition(typeof(List<ItemRosterElement>));
			base.ConstructContainerDefinition(typeof(List<InformationData>));
			base.ConstructContainerDefinition(typeof(List<AgentSaveData>));
			base.ConstructContainerDefinition(typeof(List<BattleSideEnum>));
			base.ConstructContainerDefinition(typeof(List<ShipUpgradePiece>));
			base.ConstructContainerDefinition(typeof(List<ShipHull>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemCategory>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CraftingPiece>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, PropertyObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, SkillObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemComponent>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ItemModifier>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemCategory>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CraftingPiece>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CraftingTemplate>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, SiegeEngineType>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, PropertyObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, SkillObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, CharacterAttribute>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemModifierGroup>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemObject>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemComponent>));
			base.ConstructContainerDefinition(typeof(Dictionary<MBGUID, ItemModifier>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemCategory, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemCategory, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<SiegeEngineType, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<SkillObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<PropertyObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<PropertyObject, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<ItemObject, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<CharacterAttribute, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<CraftingTemplate, List<CraftingPiece>>));
			base.ConstructContainerDefinition(typeof(Dictionary<CraftingTemplate, float>));
			base.ConstructContainerDefinition(typeof(Dictionary<long, Dictionary<long, int>>));
			base.ConstructContainerDefinition(typeof(Dictionary<int, Tuple<int, int>>));
			base.ConstructContainerDefinition(typeof(Dictionary<EquipmentElement, int>));
			base.ConstructContainerDefinition(typeof(Dictionary<string, ShipUpgradePiece>));
		}
	}
}
