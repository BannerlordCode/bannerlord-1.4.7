using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000C0 RID: 192
	public class HeroExecutionSceneNotificationData : SceneNotificationData
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0005CD53 File Offset: 0x0005AF53
		public Hero Executer { get; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0005CD5B File Offset: 0x0005AF5B
		public Hero Victim { get; }

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x0005CD63 File Offset: 0x0005AF63
		public override bool IsNegativeOptionShown { get; }

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x0005CD6B File Offset: 0x0005AF6B
		public override string SceneID
		{
			get
			{
				return "scn_execution_notification";
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x0005CD72 File Offset: 0x0005AF72
		public override TextObject NegativeText
		{
			get
			{
				return GameTexts.FindText("str_execution_negative_action", null);
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0005CD7F File Offset: 0x0005AF7F
		public override bool IsAffirmativeOptionShown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0005CD82 File Offset: 0x0005AF82
		public override TextObject TitleText { get; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x0005CD8A File Offset: 0x0005AF8A
		public override TextObject AffirmativeText { get; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060013FC RID: 5116 RVA: 0x0005CD92 File Offset: 0x0005AF92
		public override TextObject AffirmativeTitleText { get; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0005CD9A File Offset: 0x0005AF9A
		public override TextObject AffirmativeHintText { get; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0005CDA2 File Offset: 0x0005AFA2
		public override TextObject AffirmativeHintTextExtended { get; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x0005CDAA File Offset: 0x0005AFAA
		public override TextObject AffirmativeDescriptionText { get; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x0005CDB2 File Offset: 0x0005AFB2
		public override SceneNotificationData.RelevantContextType RelevantContext { get; }

		// Token: 0x06001401 RID: 5121 RVA: 0x0005CDBC File Offset: 0x0005AFBC
		public override SceneNotificationData.SceneNotificationCharacter[] GetSceneNotificationCharacters()
		{
			Equipment equipment = this.Victim.BattleEquipment.Clone(true);
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			ItemObject itemObject = Items.All.FirstOrDefault<ItemObject>((ItemObject i) => i.StringId == "execution_axe");
			Equipment equipment2 = this.Executer.BattleEquipment.Clone(true);
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(itemObject, null, null, false));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon3, default(EquipmentElement));
			equipment2.AddEquipmentToSlotWithoutAgent(EquipmentIndex.ExtraWeaponSlot, default(EquipmentElement));
			return new SceneNotificationData.SceneNotificationCharacter[]
			{
				CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Victim, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false),
				CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.Executer, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false)
			};
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0005CF10 File Offset: 0x0005B110
		private HeroExecutionSceneNotificationData(Hero executingHero, Hero dyingHero, TextObject titleText, TextObject affirmativeTitleText, TextObject affirmativeActionText, TextObject affirmativeActionDescriptionText, TextObject affirmativeActionHintText, TextObject affirmativeActionHintExtendedText, bool isNegativeOptionShown, Action onAffirmativeAction, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any)
		{
			this.Executer = executingHero;
			this.Victim = dyingHero;
			this.TitleText = titleText;
			this.AffirmativeTitleText = affirmativeTitleText;
			this.AffirmativeText = affirmativeActionText;
			this.AffirmativeDescriptionText = affirmativeActionDescriptionText;
			this.AffirmativeHintText = affirmativeActionHintText;
			this.AffirmativeHintTextExtended = affirmativeActionHintExtendedText;
			this.IsNegativeOptionShown = isNegativeOptionShown;
			this.RelevantContext = relevantContextType;
			this._onAffirmativeAction = onAffirmativeAction;
			this._runAffirmativeActionAtClose = false;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0005CF7F File Offset: 0x0005B17F
		public override void OnCloseAction()
		{
			this.PostponedAffirmativeAction();
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0005CF87 File Offset: 0x0005B187
		public override void OnAffirmativeAction()
		{
			base.OnAffirmativeAction();
			this._runAffirmativeActionAtClose = true;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0005CF98 File Offset: 0x0005B198
		private void PostponedAffirmativeAction()
		{
			if (this._runAffirmativeActionAtClose)
			{
				if (this._onAffirmativeAction != null)
				{
					this._onAffirmativeAction();
				}
				else if (this.Victim != Hero.MainHero)
				{
					if (MobileParty.MainParty.MapEvent != null)
					{
						KillCharacterAction.ApplyByExecutionAfterMapEvent(this.Victim, this.Executer, true, true);
					}
					else
					{
						KillCharacterAction.ApplyByExecution(this.Victim, this.Executer, true, true);
					}
				}
			}
			this._runAffirmativeActionAtClose = false;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0005D00C File Offset: 0x0005B20C
		public static HeroExecutionSceneNotificationData CreateForPlayerExecutingHero(Hero dyingHero, Action onAffirmativeAction, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any, bool showNegativeOption = true)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
			GameTexts.SetVariable("YEAR", CampaignTime.Now.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			TextObject textObject = GameTexts.FindText("str_execution_positive_action", null);
			textObject.SetCharacterProperties("DYING_HERO", dyingHero.CharacterObject, false);
			return new HeroExecutionSceneNotificationData(Hero.MainHero, dyingHero, GameTexts.FindText("str_executing_prisoner", null), GameTexts.FindText("str_executed_prisoner", null), textObject, GameTexts.FindText("str_execute_prisoner_desc", null), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, false), HeroExecutionSceneNotificationData.GetExecuteTroopHintText(dyingHero, true), showNegativeOption, onAffirmativeAction, relevantContextType);
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0005D0B0 File Offset: 0x0005B2B0
		public static HeroExecutionSceneNotificationData CreateForInformingPlayer(Hero executingHero, Hero dyingHero, SceneNotificationData.RelevantContextType relevantContextType = SceneNotificationData.RelevantContextType.Any, Action onClose = null)
		{
			GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
			GameTexts.SetVariable("YEAR", CampaignTime.Now.GetYear);
			GameTexts.SetVariable("NAME", dyingHero.Name);
			TextObject textObject = new TextObject("{=uYjEknNX}{VICTIM.NAME}'s execution by {EXECUTER.NAME}", null);
			textObject.SetCharacterProperties("VICTIM", dyingHero.CharacterObject, false);
			textObject.SetCharacterProperties("EXECUTER", executingHero.CharacterObject, false);
			return new HeroExecutionSceneNotificationData(executingHero, dyingHero, textObject, GameTexts.FindText("str_executed_prisoner", null), GameTexts.FindText("str_proceed", null), null, null, null, false, onClose, relevantContextType);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0005D14C File Offset: 0x0005B34C
		private static TextObject GetExecuteTroopHintText(Hero dyingHero, bool showAll)
		{
			Dictionary<Clan, int> dictionary = new Dictionary<Clan, int>();
			GameTexts.SetVariable("LEFT", new TextObject("{=jxypVgl2}Relation Changes", null));
			string text = GameTexts.FindText("str_LEFT_colon", null).ToString();
			if (dyingHero.Clan != null)
			{
				foreach (Clan clan in Clan.All)
				{
					foreach (Hero hero in clan.Heroes)
					{
						if (!hero.IsHumanPlayerCharacter && hero.IsAlive && hero != dyingHero && (!hero.IsLord || hero.Clan.Leader == hero))
						{
							bool flag;
							int relationChangeForExecutingHero = Campaign.Current.Models.ExecutionRelationModel.GetRelationChangeForExecutingHero(dyingHero, hero, out flag);
							if (relationChangeForExecutingHero != 0)
							{
								if (dictionary.ContainsKey(clan))
								{
									if (relationChangeForExecutingHero < dictionary[clan])
									{
										dictionary[clan] = relationChangeForExecutingHero;
									}
								}
								else
								{
									dictionary.Add(clan, relationChangeForExecutingHero);
								}
							}
						}
					}
				}
				GameTexts.SetVariable("newline", "\n");
				List<KeyValuePair<Clan, int>> list = dictionary.OrderBy<KeyValuePair<Clan, int>, int>((KeyValuePair<Clan, int> change) => change.Value).ToList<KeyValuePair<Clan, int>>();
				int num = 0;
				foreach (KeyValuePair<Clan, int> keyValuePair in list)
				{
					Clan key = keyValuePair.Key;
					int value = keyValuePair.Value;
					GameTexts.SetVariable("LEFT", key.Name);
					GameTexts.SetVariable("RIGHT", value);
					string text2 = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
					GameTexts.SetVariable("STR1", text);
					GameTexts.SetVariable("STR2", text2);
					text = GameTexts.FindText("str_string_newline_string", null).ToString();
					num++;
					if (!showAll && num == HeroExecutionSceneNotificationData.MaxShownRelationChanges)
					{
						TextObject textObject = new TextObject("{=DPTPuyip}And {NUMBER} more...", null);
						GameTexts.SetVariable("NUMBER", dictionary.Count - num);
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", textObject);
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
						TextObject textObject2 = new TextObject("{=u12ocP9f}Hold '{EXTEND_KEY}' for more info.", null);
						textObject2.SetTextVariable("EXTEND_KEY", GameTexts.FindText("str_game_key_text", "anyalt"));
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", textObject2);
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
						break;
					}
				}
				return new TextObject("{=!}" + text, null);
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x040006A0 RID: 1696
		private bool _runAffirmativeActionAtClose;

		// Token: 0x040006A1 RID: 1697
		private readonly Action _onAffirmativeAction;

		// Token: 0x040006A2 RID: 1698
		protected static int MaxShownRelationChanges = 8;
	}
}
