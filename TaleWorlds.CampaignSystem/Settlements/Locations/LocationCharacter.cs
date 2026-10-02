using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Settlements.Locations
{
	// Token: 0x020003C7 RID: 967
	public class LocationCharacter
	{
		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x000EC97D File Offset: 0x000EAB7D
		public CharacterObject Character
		{
			get
			{
				return (CharacterObject)this.AgentData.AgentCharacter;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060039DA RID: 14810 RVA: 0x000EC98F File Offset: 0x000EAB8F
		public IAgentOriginBase AgentOrigin
		{
			get
			{
				return this.AgentData.AgentOrigin;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x000EC99C File Offset: 0x000EAB9C
		public AgentData AgentData { get; }

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060039DC RID: 14812 RVA: 0x000EC9A4 File Offset: 0x000EABA4
		public bool UseCivilianEquipment { get; }

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x000EC9AC File Offset: 0x000EABAC
		public string ActionSetCode { get; }

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060039DE RID: 14814 RVA: 0x000EC9B4 File Offset: 0x000EABB4
		public string AlarmedActionSetCode { get; }

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x000EC9BC File Offset: 0x000EABBC
		// (set) Token: 0x060039E0 RID: 14816 RVA: 0x000EC9C4 File Offset: 0x000EABC4
		public string SpecialTargetTag { get; set; }

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x000EC9CD File Offset: 0x000EABCD
		// (set) Token: 0x060039E2 RID: 14818 RVA: 0x000EC9D5 File Offset: 0x000EABD5
		public bool ForceSpawnInSpecialTargetTag { get; set; }

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x000EC9DE File Offset: 0x000EABDE
		public LocationCharacter.AddBehaviorsDelegate AddBehaviors { get; }

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x000EC9E6 File Offset: 0x000EABE6
		public LocationCharacter.AfterAgentCreatedDelegate AfterAgentCreated { get; }

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000EC9EE File Offset: 0x000EABEE
		public bool FixedLocation { get; }

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000EC9F6 File Offset: 0x000EABF6
		// (set) Token: 0x060039E7 RID: 14823 RVA: 0x000EC9FE File Offset: 0x000EABFE
		public Alley MemberOfAlley { get; private set; }

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x000ECA07 File Offset: 0x000EAC07
		public ItemObject SpecialItem { get; }

		// Token: 0x060039E9 RID: 14825 RVA: 0x000ECA10 File Offset: 0x000EAC10
		public LocationCharacter(AgentData agentData, LocationCharacter.AddBehaviorsDelegate addBehaviorsDelegate, string spawnTag, bool fixedLocation, LocationCharacter.CharacterRelations characterRelation, string actionSetCode, bool useCivilianEquipment, bool isFixedCharacter = false, ItemObject specialItem = null, bool isHidden = false, bool isVisualTracked = false, bool overrideBodyProperties = true, LocationCharacter.AfterAgentCreatedDelegate afterAgentCreated = null, bool forceSpawnOnSpecialTargetTag = false)
		{
			this.AgentData = agentData;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				int num = -2;
				if (overrideBodyProperties)
				{
					num = (isFixedCharacter ? (Settlement.CurrentSettlement.StringId + "_" + this.Character.StringId).GetDeterministicHashCode() : agentData.AgentEquipmentSeed);
				}
				this.AgentData.BodyProperties(this.Character.GetBodyProperties(this.Character.Equipment, num));
			}
			this.AddBehaviors = addBehaviorsDelegate;
			this.SpecialTargetTag = spawnTag;
			this.FixedLocation = fixedLocation;
			this.ActionSetCode = actionSetCode ?? TaleWorlds.Core.ActionSetCode.GenerateActionSetNameWithSuffix(this.AgentData.AgentMonster, this.AgentData.AgentCharacter.IsFemale, "_villager");
			this.AlarmedActionSetCode = TaleWorlds.Core.ActionSetCode.GenerateActionSetNameWithSuffix(this.AgentData.AgentMonster, this.AgentData.AgentIsFemale, "_villager");
			this.PrefabNamesForBones = new Dictionary<sbyte, string>();
			this.CharacterRelation = characterRelation;
			this.SpecialItem = specialItem;
			this.UseCivilianEquipment = useCivilianEquipment;
			this.AfterAgentCreated = afterAgentCreated;
			this.IsVisualTracked = isVisualTracked;
			if (forceSpawnOnSpecialTargetTag)
			{
				this.ForceSpawnInSpecialTargetTag = true;
			}
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x000ECB3D File Offset: 0x000EAD3D
		public void SetAlleyOfCharacter(Alley alley)
		{
			this.MemberOfAlley = alley;
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x000ECB48 File Offset: 0x000EAD48
		public static LocationCharacter CreateBodyguardHero(Hero hero, MobileParty party, LocationCharacter.AddBehaviorsDelegate addBehaviorsDelegate)
		{
			UniqueTroopDescriptor uniqueTroopDescriptor = new UniqueTroopDescriptor(FlattenedTroopRoster.GenerateUniqueNoFromParty(party, 0));
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
			return new LocationCharacter(new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, uniqueTroopDescriptor, false, false)).Monster(monsterWithSuffix).NoHorses(true), addBehaviorsDelegate, null, false, LocationCharacter.CharacterRelations.Friendly, null, !PlayerEncounter.LocationEncounter.Settlement.IsVillage, false, null, false, false, true, null, false);
		}

		// Token: 0x040011D6 RID: 4566
		public bool IsVisualTracked;

		// Token: 0x040011DF RID: 4575
		public Dictionary<sbyte, string> PrefabNamesForBones;

		// Token: 0x040011E1 RID: 4577
		public LocationCharacter.CharacterRelations CharacterRelation;

		// Token: 0x0200079B RID: 1947
		// (Invoke) Token: 0x060062F1 RID: 25329
		public delegate void AddBehaviorsDelegate(IAgent agent);

		// Token: 0x0200079C RID: 1948
		// (Invoke) Token: 0x060062F5 RID: 25333
		public delegate void AfterAgentCreatedDelegate(IAgent agent);

		// Token: 0x0200079D RID: 1949
		public enum CharacterRelations
		{
			// Token: 0x04001EFA RID: 7930
			Neutral,
			// Token: 0x04001EFB RID: 7931
			Friendly,
			// Token: 0x04001EFC RID: 7932
			Enemy
		}
	}
}
