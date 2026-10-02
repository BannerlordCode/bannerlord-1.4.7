using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000CF RID: 207
	public class BarberCampaignBehavior : CampaignBehaviorBase, IFacegenCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x00040CDE File Offset: 0x0003EEDE
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00040D0E File Offset: 0x0003EF0E
		public override void SyncData(IDataStore store)
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00040D10 File Offset: 0x0003EF10
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00040D1C File Offset: 0x0003EF1C
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("barber_start_talk_beggar", "start", "close_window", "{=pWzdxd7O}May the Heavens bless you, my poor {?PLAYER.GENDER}lady{?}fellow{\\?}, but I can't spare a coin right now.", new ConversationSentence.OnConditionDelegate(this.InDisguiseSpeakingToBarber), new ConversationSentence.OnConsequenceDelegate(this.InitializeBarberConversation), 100, null);
			campaignGameStarter.AddDialogLine("barber_start_talk", "start", "barber_question1", "{=2aXYYNBG}Come to have your hair cut, {?PLAYER.GENDER}my lady{?}my lord{\\?}? A new look for a new day?", new ConversationSentence.OnConditionDelegate(this.IsConversationAgentBarber), new ConversationSentence.OnConsequenceDelegate(this.InitializeBarberConversation), 100, null);
			campaignGameStarter.AddPlayerLine("player_accept_haircut", "barber_question1", "start_cut_token", "{=Q7wBRXtR}Yes, I have. ({GOLD_COST} {GOLD_ICON})", new ConversationSentence.OnConditionDelegate(this.GivePlayerAHaircutCondition), new ConversationSentence.OnConsequenceDelegate(this.GivePlayerAHaircut), 100, new ConversationSentence.OnClickableConditionDelegate(this.DoesPlayerHaveEnoughGold), null);
			campaignGameStarter.AddPlayerLine("player_refuse_haircut", "barber_question1", "no_haircut_conversation_token", "{=xPAAZAaI}My hair is fine as it is, thank you.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("barber_ask_if_done", "start_cut_token", "finish_cut_token", "{=M3K8wUOO}So... Does this please you, {?PLAYER.GENDER}my lady{?}my lord{\\?}?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("player_done_with_haircut", "finish_cut_token", "finish_barber", "{=zTF4bJm0}Yes, it's fine.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("player_not_done_with_haircut", "finish_cut_token", "start_cut_token", "{=BnoSOi3r}Actually...", new ConversationSentence.OnConditionDelegate(this.GivePlayerAHaircutCondition), new ConversationSentence.OnConsequenceDelegate(this.GivePlayerAHaircut), 100, new ConversationSentence.OnClickableConditionDelegate(this.DoesPlayerHaveEnoughGold), null);
			campaignGameStarter.AddDialogLine("barber_no_haircut_talk", "no_haircut_conversation_token", "close_window", "{=BusYGTrN}Excellent! Have a good day, then, {?PLAYER.GENDER}my lady{?}my lord{\\?}.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("barber_haircut_finished", "finish_barber", "player_had_a_haircut_token", "{=akqJbZpH}Marvellous! You cut a splendid appearance, {?PLAYER.GENDER}my lady{?}my lord{\\?}, if you don't mind my saying. Most splendid.", new ConversationSentence.OnConditionDelegate(this.DidPlayerHaveAHaircut), new ConversationSentence.OnConsequenceDelegate(this.ChargeThePlayer), 100, null);
			campaignGameStarter.AddDialogLine("barber_haircut_no_change", "finish_barber", "player_did_not_cut_token", "{=yLIZlaS1}Very well. Do come back when you're ready, {?PLAYER.GENDER}my lady{?}my lord{\\?}.", new ConversationSentence.OnConditionDelegate(this.DidPlayerNotHaveAHaircut), null, 100, null);
			campaignGameStarter.AddPlayerLine("player_no_haircut_finish_talk", "player_did_not_cut_token", "close_window", "{=oPUVNuhN}I'll keep you in mind", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("player_haircut_finish_talk", "player_had_a_haircut_token", "close_window", "{=F9Xjbchh}Thank you.", null, null, 100, null, null);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00040F3E File Offset: 0x0003F13E
		private bool InDisguiseSpeakingToBarber()
		{
			return this.IsConversationAgentBarber() && Campaign.Current.IsMainHeroDisguised;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00040F54 File Offset: 0x0003F154
		private bool DoesPlayerHaveEnoughGold(out TextObject explanation)
		{
			if (Hero.MainHero.Gold < 100)
			{
				explanation = new TextObject("{=RYJdU43V}Not Enough Gold", null);
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00040F77 File Offset: 0x0003F177
		private void ChargeThePlayer()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 100, false);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00040F87 File Offset: 0x0003F187
		private bool DidPlayerNotHaveAHaircut()
		{
			return !this.DidPlayerHaveAHaircut();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00040F94 File Offset: 0x0003F194
		private bool DidPlayerHaveAHaircut()
		{
			return Hero.MainHero.BodyProperties.StaticProperties != this._previousBodyProperties;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00040FBE File Offset: 0x0003F1BE
		private bool IsConversationAgentBarber()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return ((currentSettlement != null) ? currentSettlement.Culture.Barber : null) == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00040FDD File Offset: 0x0003F1DD
		private bool GivePlayerAHaircutCondition()
		{
			MBTextManager.SetTextVariable("GOLD_COST", 100);
			return true;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00040FEC File Offset: 0x0003F1EC
		private void GivePlayerAHaircut()
		{
			this._isOpenedFromBarberDialogue = true;
			BarberState barberState = Game.Current.GameStateManager.CreateState<BarberState>(new object[]
			{
				Hero.MainHero.CharacterObject,
				this.GetFaceGenFilter()
			});
			this._isOpenedFromBarberDialogue = false;
			GameStateManager.Current.PushState(barberState, 0);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00041040 File Offset: 0x0003F240
		private void InitializeBarberConversation()
		{
			this._previousBodyProperties = Hero.MainHero.BodyProperties.StaticProperties;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00041068 File Offset: 0x0003F268
		private LocationCharacter CreateBarber(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject barber = culture.Barber;
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(barber, out num, out num2, "Barber");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(barber, -1, null, default(UniqueTroopDescriptor))).Monster(FaceGen.GetMonsterWithSuffix(barber.Race, "_settlement_slow")).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_barber", true, relation, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x000410FC File Offset: 0x0003F2FC
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("center");
			int num;
			if (CampaignMission.Current.Location == locationWithId && Campaign.Current.IsDay && unusedUsablePointCount.TryGetValue("sp_merchant_notary", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateBarber), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00041164 File Offset: 0x0003F364
		public IFaceGeneratorCustomFilter GetFaceGenFilter()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			if (Settlement.CurrentSettlement != null)
			{
				list.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetHairIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, Settlement.CurrentSettlement.Culture));
				list2.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetBeardIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, Settlement.CurrentSettlement.Culture));
			}
			else
			{
				foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
				{
					list.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetHairIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, cultureObject));
					list2.AddRange(Campaign.Current.Models.BodyPropertiesModel.GetBeardIndicesForCulture(Hero.MainHero.CharacterObject.Race, Hero.MainHero.IsFemale ? 1 : 0, Hero.MainHero.Age, cultureObject));
				}
			}
			return new BarberCampaignBehavior.BarberFaceGeneratorCustomFilter(!this._isOpenedFromBarberDialogue, list.Distinct<int>().ToArray<int>(), list2.Distinct<int>().ToArray<int>());
		}

		// Token: 0x0400046C RID: 1132
		private const int BarberCost = 100;

		// Token: 0x0400046D RID: 1133
		private bool _isOpenedFromBarberDialogue;

		// Token: 0x0400046E RID: 1134
		private StaticBodyProperties _previousBodyProperties;

		// Token: 0x020001F2 RID: 498
		private class BarberFaceGeneratorCustomFilter : IFaceGeneratorCustomFilter
		{
			// Token: 0x06001390 RID: 5008 RVA: 0x000784BC File Offset: 0x000766BC
			public BarberFaceGeneratorCustomFilter(bool useDefaultStages, int[] haircutIndices, int[] faircutIndices)
			{
				this._haircutIndices = haircutIndices;
				this._facialHairIndices = faircutIndices;
				this._defaultStages = useDefaultStages;
			}

			// Token: 0x06001391 RID: 5009 RVA: 0x000784D9 File Offset: 0x000766D9
			public int[] GetHaircutIndices(BasicCharacterObject character)
			{
				return this._haircutIndices;
			}

			// Token: 0x06001392 RID: 5010 RVA: 0x000784E1 File Offset: 0x000766E1
			public int[] GetFacialHairIndices(BasicCharacterObject character)
			{
				return this._facialHairIndices;
			}

			// Token: 0x06001393 RID: 5011 RVA: 0x000784E9 File Offset: 0x000766E9
			public FaceGeneratorStage[] GetAvailableStages()
			{
				if (this._defaultStages)
				{
					return new FaceGeneratorStage[]
					{
						FaceGeneratorStage.Body,
						FaceGeneratorStage.Face,
						FaceGeneratorStage.Eyes,
						FaceGeneratorStage.Nose,
						FaceGeneratorStage.Mouth,
						FaceGeneratorStage.Hair,
						FaceGeneratorStage.Taint
					};
				}
				return new FaceGeneratorStage[] { FaceGeneratorStage.Hair };
			}

			// Token: 0x0400092F RID: 2351
			private readonly int[] _haircutIndices;

			// Token: 0x04000930 RID: 2352
			private readonly int[] _facialHairIndices;

			// Token: 0x04000931 RID: 2353
			private readonly bool _defaultStages;
		}
	}
}
