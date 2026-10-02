using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x02000041 RID: 65
	public class StoryModeEncounterGameMenuModel : EncounterGameMenuModel
	{
		// Token: 0x06000440 RID: 1088 RVA: 0x00018FA0 File Offset: 0x000171A0
		public override string GetEncounterMenu(PartyBase attackerParty, PartyBase defenderParty, out bool startBattle, out bool joinBattle)
		{
			Settlement settlement = MapEventHelper.GetEncounteredPartyBase(attackerParty, defenderParty).Settlement;
			string text;
			if (settlement != null && settlement.SettlementComponent is TrainingField)
			{
				text = "training_field_menu";
				startBattle = false;
				joinBattle = false;
			}
			else if (StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
			{
				text = "storymode_game_menu_blocker";
				startBattle = false;
				joinBattle = false;
			}
			else
			{
				text = base.BaseModel.GetEncounterMenu(attackerParty, defenderParty, out startBattle, out joinBattle);
			}
			return text;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001900B File Offset: 0x0001720B
		public override string GetGenericStateMenu()
		{
			return base.BaseModel.GetGenericStateMenu();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00019018 File Offset: 0x00017218
		public override string GetNewPartyJoinMenu(MobileParty newParty)
		{
			return base.BaseModel.GetNewPartyJoinMenu(newParty);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00019026 File Offset: 0x00017226
		public override string GetRaidCompleteMenu()
		{
			return base.BaseModel.GetRaidCompleteMenu();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00019033 File Offset: 0x00017233
		public override bool IsPlunderMenu(string menuId)
		{
			return base.BaseModel.IsPlunderMenu(menuId);
		}
	}
}
