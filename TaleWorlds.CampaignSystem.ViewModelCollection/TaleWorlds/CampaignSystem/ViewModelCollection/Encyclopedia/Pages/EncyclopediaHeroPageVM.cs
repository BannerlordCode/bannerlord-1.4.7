using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000D3 RID: 211
	[EncyclopediaViewModel(typeof(Hero))]
	public class EncyclopediaHeroPageVM : EncyclopediaContentPageVM
	{
		// Token: 0x0600141B RID: 5147 RVA: 0x0005095C File Offset: 0x0004EB5C
		public EncyclopediaHeroPageVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this._hero = base.Obj as Hero;
			this._relationAscendingComparer = new HeroRelationComparer(this._hero, true, true);
			this._relationDescendingComparer = new HeroRelationComparer(this._hero, false, true);
			TextObject textObject;
			this.IsInformationHidden = CampaignUIHelper.IsHeroInformationHidden(this._hero, out textObject);
			this._infoHiddenReasonText = textObject;
			this._allRelatedHeroes = new List<Hero>
			{
				this._hero.Father,
				this._hero.Mother,
				this._hero.Spouse
			};
			this._allRelatedHeroes.AddRange(this._hero.Siblings);
			this._allRelatedHeroes.AddRange(this._hero.ExSpouses);
			this._allRelatedHeroes.AddRange(CampaignUIHelper.GetChildrenAndGrandchildrenOfHero(this._hero));
			StringHelpers.SetCharacterProperties("NPC", this._hero.CharacterObject, null, false);
			this.Settlements = new MBBindingList<EncyclopediaSettlementVM>();
			this.Dwellings = new MBBindingList<EncyclopediaDwellingVM>();
			this.Allies = new MBBindingList<HeroVM>();
			this.AdditionalAllies = new MBBindingList<HeroVM>();
			this.Enemies = new MBBindingList<HeroVM>();
			this.AdditionalEnemies = new MBBindingList<HeroVM>();
			this.Family = new MBBindingList<EncyclopediaFamilyMemberVM>();
			this.Companions = new MBBindingList<HeroVM>();
			this.History = new MBBindingList<EncyclopediaHistoryEventVM>();
			this.Skills = new MBBindingList<EncyclopediaSkillVM>();
			this.Stats = new MBBindingList<StringPairItemVM>();
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.HeroCharacter = new HeroViewModel(CharacterViewModel.StanceTypes.EmphasizeFace);
			base.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(this._hero);
			this.Faction = new EncyclopediaFactionVM(this._hero.Clan);
			this.RefreshValues();
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00050B28 File Offset: 0x0004ED28
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.AlliesText = GameTexts.FindText("str_friends", null).ToString();
			this.EnemiesText = GameTexts.FindText("str_enemies", null).ToString();
			this.FamilyText = GameTexts.FindText("str_family_group", null).ToString();
			this.CompanionsText = GameTexts.FindText("str_companions", null).ToString();
			this.DwellingsText = GameTexts.FindText("str_dwellings", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_settlements", null).ToString();
			this.DeceasedText = GameTexts.FindText("str_encyclopedia_deceased", null).ToString();
			this.TraitsText = GameTexts.FindText("str_traits_group", null).ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.InfoText = GameTexts.FindText("str_info", null).ToString();
			this.PregnantHint = new HintViewModel(GameTexts.FindText("str_pregnant", null), null);
			base.UpdateBookmarkHintText();
			this.UpdateInformationText();
			this.Refresh();
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x00050C58 File Offset: 0x0004EE58
		public override void Refresh()
		{
			base.IsLoadingOver = false;
			this.Settlements.Clear();
			this.Dwellings.Clear();
			this.Allies.Clear();
			this.Enemies.Clear();
			this.AdditionalAllies.Clear();
			this.AdditionalEnemies.Clear();
			this.Companions.Clear();
			this.Family.Clear();
			this.History.Clear();
			this.Skills.Clear();
			this.Stats.Clear();
			this.Traits.Clear();
			this.NameText = this._hero.Name.ToString();
			string text = GameTexts.FindText("str_missing_info_indicator", null).ToString();
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			this.HasNeutralClan = this._hero.Clan == null;
			if (!this.IsInformationHidden)
			{
				List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
				list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
				foreach (SkillObject skillObject in list)
				{
					if (this._hero.GetSkillValue(skillObject) >= 50)
					{
						this.Skills.Add(new EncyclopediaSkillVM(skillObject, this._hero.GetSkillValue(skillObject)));
					}
				}
				foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
				{
					if (this._hero.GetTraitLevel(traitObject) != 0)
					{
						this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this._hero));
					}
				}
				if (this._hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
				{
					for (int i = 0; i < Hero.AllAliveHeroes.Count; i++)
					{
						this.AddHeroToRelatedVMList(Hero.AllAliveHeroes[i]);
					}
					for (int j = 0; j < Hero.DeadOrDisabledHeroes.Count; j++)
					{
						this.AddHeroToRelatedVMList(Hero.DeadOrDisabledHeroes[j]);
					}
					this.Allies.Sort(this._relationDescendingComparer);
					this.Enemies.Sort(this._relationAscendingComparer);
					while (this.Allies.Count > 13)
					{
						HeroVM heroVM = this.Allies[13];
						this.Allies.Remove(heroVM);
						this.AdditionalAllies.Add(heroVM);
					}
					while (this.Enemies.Count > 13)
					{
						HeroVM heroVM2 = this.Enemies[13];
						this.Enemies.Remove(heroVM2);
						this.AdditionalEnemies.Add(heroVM2);
					}
					this.OnAdditionalListsUpdated();
				}
				if (this._hero.Clan != null && this._hero == this._hero.Clan.Leader)
				{
					for (int k = 0; k < this._hero.Clan.Companions.Count; k++)
					{
						Hero hero = this._hero.Clan.Companions[k];
						this.Companions.Add(new HeroVM(hero, false));
					}
				}
				for (int l = 0; l < this._allRelatedHeroes.Count; l++)
				{
					Hero hero2 = this._allRelatedHeroes[l];
					if (hero2 != null && pageOf.IsValidEncyclopediaItem(hero2))
					{
						this.Family.Add(new EncyclopediaFamilyMemberVM(hero2, this._hero));
					}
				}
				for (int m = 0; m < this._hero.OwnedWorkshops.Count; m++)
				{
					this.Dwellings.Add(new EncyclopediaDwellingVM(this._hero.OwnedWorkshops[m].WorkshopType));
				}
				EncyclopediaPage pageOf2 = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Settlement));
				for (int n = 0; n < Settlement.All.Count; n++)
				{
					Settlement settlement = Settlement.All[n];
					if (settlement.OwnerClan != null && settlement.OwnerClan.Leader == this._hero && pageOf2.IsValidEncyclopediaItem(settlement))
					{
						this.Settlements.Add(new EncyclopediaSettlementVM(settlement));
					}
				}
			}
			this.HasAnySkills = this.Skills.Count > 0;
			if (this._hero.Culture != null)
			{
				string text2 = GameTexts.FindText("str_enc_sf_culture", null).ToString();
				this.Stats.Add(new StringPairItemVM(text2, this._hero.Culture.Name.ToString(), null));
			}
			string text3 = GameTexts.FindText("str_enc_sf_age", null).ToString();
			this.Stats.Add(new StringPairItemVM(text3, this.IsInformationHidden ? text : ((int)this._hero.Age).ToString(), null));
			MBObjectBase hero3 = this._hero;
			for (int num = Campaign.Current.LogEntryHistory.GameActionLogs.Count - 1; num >= 0; num--)
			{
				IEncyclopediaLog encyclopediaLog;
				if ((encyclopediaLog = Campaign.Current.LogEntryHistory.GameActionLogs[num] as IEncyclopediaLog) != null && encyclopediaLog.IsVisibleInEncyclopediaPageOf(hero3))
				{
					this.History.Add(new EncyclopediaHistoryEventVM(encyclopediaLog));
				}
			}
			if (!this._hero.IsNotable && !this._hero.IsWanderer)
			{
				Clan clan = this._hero.Clan;
				if (((clan != null) ? clan.Kingdom : null) != null)
				{
					this.KingdomRankText = CampaignUIHelper.GetHeroKingdomRank(this._hero);
				}
			}
			string heroOccupationName = CampaignUIHelper.GetHeroOccupationName(this._hero);
			if (!string.IsNullOrEmpty(heroOccupationName))
			{
				string text4 = GameTexts.FindText("str_enc_sf_occupation", null).ToString();
				this.Stats.Add(new StringPairItemVM(text4, this.IsInformationHidden ? text : heroOccupationName, null));
			}
			if (this._hero != Hero.MainHero)
			{
				string text5 = GameTexts.FindText("str_enc_sf_relation", null).ToString();
				this.Stats.Add(new StringPairItemVM(text5, this.IsInformationHidden ? text : this._hero.GetRelationWithPlayer().ToString(), null));
			}
			this.LastSeenText = ((this._hero == Hero.MainHero) ? "" : HeroHelper.GetLastSeenText(this._hero).ToString());
			this.HeroCharacter.FillFrom(this._hero, -1, this._hero.IsNotable, true);
			this.HeroCharacter.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			this.IsCompanion = this._hero.CompanionOf != null;
			if (this.IsCompanion)
			{
				this.MasterText = GameTexts.FindText("str_companion_of", null).ToString();
				Clan companionOf = this._hero.CompanionOf;
				this.Master = new HeroVM((companionOf != null) ? companionOf.Leader : null, false);
			}
			this.IsPregnant = this._hero.IsPregnant;
			this.IsDead = !this._hero.IsAlive;
			base.IsLoadingOver = true;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000513C0 File Offset: 0x0004F5C0
		private void AddHeroToRelatedVMList(Hero hero)
		{
			if (!Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(hero) || hero.IsNotable)
			{
				return;
			}
			if (hero != this._hero && hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._allRelatedHeroes.Contains(hero))
			{
				if (this._hero.IsFriend(hero))
				{
					this.Allies.Add(new HeroVM(hero, false));
					return;
				}
				if (this._hero.IsEnemy(hero))
				{
					this.Enemies.Add(new HeroVM(hero, false));
				}
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00051474 File Offset: 0x0004F674
		public override string GetName()
		{
			return this._hero.Name.ToString();
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00051488 File Offset: 0x0004F688
		public override string GetNavigationBarURL()
		{
			return HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ " + HyperlinkTexts.GetGenericHyperlinkText("ListPage-Heroes", GameTexts.FindText("str_encyclopedia_heroes", null).ToString()) + " \\ " + this.GetName();
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x000514ED File Offset: 0x0004F6ED
		public void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00051500 File Offset: 0x0004F700
		public override void ExecuteSwitchBookmarkedState()
		{
			base.ExecuteSwitchBookmarkedState();
			if (base.IsBookmarked)
			{
				Campaign.Current.EncyclopediaManager.ViewDataTracker.AddEncyclopediaBookmarkToItem(this._hero);
				return;
			}
			Campaign.Current.EncyclopediaManager.ViewDataTracker.RemoveEncyclopediaBookmarkFromItem(this._hero);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00051550 File Offset: 0x0004F750
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroCharacter.OnFinalize();
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00051564 File Offset: 0x0004F764
		private void UpdateInformationText()
		{
			this.InformationText = "";
			if (!TextObject.IsNullOrEmpty(this._hero.EncyclopediaText))
			{
				this.InformationText = this._hero.EncyclopediaText.ToString();
				return;
			}
			if (this._hero.CharacterObject.Occupation == Occupation.Lord)
			{
				this.InformationText = Hero.SetHeroEncyclopediaTextAndLinks(this._hero).ToString();
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x000515D0 File Offset: 0x0004F7D0
		private void OnAdditionalListsUpdated()
		{
			this.AnyAdditionalAllies = this.AdditionalAllies.Count > 0;
			this.AnyAdditionalEnemies = this.AdditionalEnemies.Count > 0;
			this.AdditionalAlliesString = (this.AnyAdditionalAllies ? new TextObject("{=!}+{REMAINING}", null).SetTextVariable("REMAINING", this.AdditionalAllies.Count).ToString() : string.Empty);
			this.AdditionalEnemiesString = (this.AnyAdditionalEnemies ? new TextObject("{=!}+{REMAINING}", null).SetTextVariable("REMAINING", this.AdditionalEnemies.Count).ToString() : string.Empty);
			this.AdditionalAlliesHint = new BasicTooltipViewModel(() => this.GetOverflowTooltip(this.AdditionalAllies));
			this.AdditionalEnemiesHint = new BasicTooltipViewModel(() => this.GetOverflowTooltip(this.AdditionalEnemies));
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x000516A8 File Offset: 0x0004F8A8
		private List<TooltipProperty> GetOverflowTooltip(MBBindingList<HeroVM> overflowList)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			foreach (HeroVM heroVM in overflowList)
			{
				list.Add(new TooltipProperty(string.Empty, heroVM.NameText, 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0005170C File Offset: 0x0004F90C
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x00051714 File Offset: 0x0004F914
		[DataSourceProperty]
		public EncyclopediaFactionVM Faction
		{
			get
			{
				return this._faction;
			}
			set
			{
				if (value != this._faction)
				{
					this._faction = value;
					base.OnPropertyChangedWithValue<EncyclopediaFactionVM>(value, "Faction");
				}
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x00051732 File Offset: 0x0004F932
		// (set) Token: 0x0600142A RID: 5162 RVA: 0x0005173A File Offset: 0x0004F93A
		[DataSourceProperty]
		public bool IsCompanion
		{
			get
			{
				return this._isCompanion;
			}
			set
			{
				if (value != this._isCompanion)
				{
					this._isCompanion = value;
					base.OnPropertyChangedWithValue(value, "IsCompanion");
				}
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x00051758 File Offset: 0x0004F958
		// (set) Token: 0x0600142C RID: 5164 RVA: 0x00051760 File Offset: 0x0004F960
		[DataSourceProperty]
		public bool IsPregnant
		{
			get
			{
				return this._isPregnant;
			}
			set
			{
				if (value != this._isPregnant)
				{
					this._isPregnant = value;
					base.OnPropertyChangedWithValue(value, "IsPregnant");
				}
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0005177E File Offset: 0x0004F97E
		// (set) Token: 0x0600142E RID: 5166 RVA: 0x00051786 File Offset: 0x0004F986
		[DataSourceProperty]
		public HeroVM Master
		{
			get
			{
				return this._master;
			}
			set
			{
				if (value != this._master)
				{
					this._master = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Master");
				}
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x000517A4 File Offset: 0x0004F9A4
		// (set) Token: 0x06001430 RID: 5168 RVA: 0x000517AC File Offset: 0x0004F9AC
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x000517CF File Offset: 0x0004F9CF
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x000517D7 File Offset: 0x0004F9D7
		[DataSourceProperty]
		public string InfoText
		{
			get
			{
				return this._infoText;
			}
			set
			{
				if (value != this._infoText)
				{
					this._infoText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfoText");
				}
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x000517FA File Offset: 0x0004F9FA
		// (set) Token: 0x06001434 RID: 5172 RVA: 0x00051802 File Offset: 0x0004FA02
		[DataSourceProperty]
		public string TraitsText
		{
			get
			{
				return this._traitsText;
			}
			set
			{
				if (value != this._traitsText)
				{
					this._traitsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitsText");
				}
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00051825 File Offset: 0x0004FA25
		// (set) Token: 0x06001436 RID: 5174 RVA: 0x0005182D File Offset: 0x0004FA2D
		[DataSourceProperty]
		public string MasterText
		{
			get
			{
				return this._masterText;
			}
			set
			{
				if (value != this._masterText)
				{
					this._masterText = value;
					base.OnPropertyChangedWithValue<string>(value, "MasterText");
				}
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00051850 File Offset: 0x0004FA50
		// (set) Token: 0x06001438 RID: 5176 RVA: 0x00051858 File Offset: 0x0004FA58
		[DataSourceProperty]
		public string KingdomRankText
		{
			get
			{
				return this._kingdomRankText;
			}
			set
			{
				if (value != this._kingdomRankText)
				{
					this._kingdomRankText = value;
					base.OnPropertyChangedWithValue<string>(value, "KingdomRankText");
				}
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0005187B File Offset: 0x0004FA7B
		[DataSourceProperty]
		public string InfoHiddenReasonText
		{
			get
			{
				return this._infoHiddenReasonText.ToString();
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00051888 File Offset: 0x0004FA88
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x00051890 File Offset: 0x0004FA90
		[DataSourceProperty]
		public string SkillsText
		{
			get
			{
				return this._skillsText;
			}
			set
			{
				if (value != this._skillsText)
				{
					this._skillsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillsText");
				}
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x000518B3 File Offset: 0x0004FAB3
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x000518BB File Offset: 0x0004FABB
		[DataSourceProperty]
		public HeroViewModel HeroCharacter
		{
			get
			{
				return this._heroCharacter;
			}
			set
			{
				if (value != this._heroCharacter)
				{
					this._heroCharacter = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "HeroCharacter");
				}
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x000518D9 File Offset: 0x0004FAD9
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x000518E1 File Offset: 0x0004FAE1
		[DataSourceProperty]
		public string LastSeenText
		{
			get
			{
				return this._lastSeenText;
			}
			set
			{
				if (value != this._lastSeenText)
				{
					this._lastSeenText = value;
					base.OnPropertyChangedWithValue<string>(value, "LastSeenText");
				}
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00051904 File Offset: 0x0004FB04
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x0005190C File Offset: 0x0004FB0C
		[DataSourceProperty]
		public string DeceasedText
		{
			get
			{
				return this._deceasedText;
			}
			set
			{
				if (value != this._deceasedText)
				{
					this._deceasedText = value;
					base.OnPropertyChangedWithValue<string>(value, "DeceasedText");
				}
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0005192F File Offset: 0x0004FB2F
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00051937 File Offset: 0x0004FB37
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0005195A File Offset: 0x0004FB5A
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x00051962 File Offset: 0x0004FB62
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x00051985 File Offset: 0x0004FB85
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0005198D File Offset: 0x0004FB8D
		[DataSourceProperty]
		public string DwellingsText
		{
			get
			{
				return this._dwellingsText;
			}
			set
			{
				if (value != this._dwellingsText)
				{
					this._dwellingsText = value;
					base.OnPropertyChangedWithValue<string>(value, "DwellingsText");
				}
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x000519B0 File Offset: 0x0004FBB0
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x000519B8 File Offset: 0x0004FBB8
		[DataSourceProperty]
		public string CompanionsText
		{
			get
			{
				return this._companionsText;
			}
			set
			{
				if (value != this._companionsText)
				{
					this._companionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompanionsText");
				}
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x000519DB File Offset: 0x0004FBDB
		// (set) Token: 0x0600144B RID: 5195 RVA: 0x000519E3 File Offset: 0x0004FBE3
		[DataSourceProperty]
		public string AlliesText
		{
			get
			{
				return this._alliesText;
			}
			set
			{
				if (value != this._alliesText)
				{
					this._alliesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlliesText");
				}
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00051A06 File Offset: 0x0004FC06
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x00051A0E File Offset: 0x0004FC0E
		[DataSourceProperty]
		public string EnemiesText
		{
			get
			{
				return this._enemiesText;
			}
			set
			{
				if (value != this._enemiesText)
				{
					this._enemiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemiesText");
				}
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00051A31 File Offset: 0x0004FC31
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x00051A39 File Offset: 0x0004FC39
		[DataSourceProperty]
		public string FamilyText
		{
			get
			{
				return this._familyText;
			}
			set
			{
				if (value != this._familyText)
				{
					this._familyText = value;
					base.OnPropertyChangedWithValue<string>(value, "FamilyText");
				}
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x00051A5C File Offset: 0x0004FC5C
		// (set) Token: 0x06001451 RID: 5201 RVA: 0x00051A64 File Offset: 0x0004FC64
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> Stats
		{
			get
			{
				return this._stats;
			}
			set
			{
				if (value != this._stats)
				{
					this._stats = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "Stats");
				}
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x00051A82 File Offset: 0x0004FC82
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x00051A8A File Offset: 0x0004FC8A
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTraitItemVM> Traits
		{
			get
			{
				return this._traits;
			}
			set
			{
				if (value != this._traits)
				{
					this._traits = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTraitItemVM>>(value, "Traits");
				}
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00051AA8 File Offset: 0x0004FCA8
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x00051AB0 File Offset: 0x0004FCB0
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> Skills
		{
			get
			{
				return this._skills;
			}
			set
			{
				if (value != this._skills)
				{
					this._skills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00051ACE File Offset: 0x0004FCCE
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x00051AD6 File Offset: 0x0004FCD6
		[DataSourceProperty]
		public MBBindingList<EncyclopediaDwellingVM> Dwellings
		{
			get
			{
				return this._dwellings;
			}
			set
			{
				if (value != this._dwellings)
				{
					this._dwellings = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaDwellingVM>>(value, "Dwellings");
				}
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x00051AF4 File Offset: 0x0004FCF4
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x00051AFC File Offset: 0x0004FCFC
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00051B1A File Offset: 0x0004FD1A
		// (set) Token: 0x0600145B RID: 5211 RVA: 0x00051B22 File Offset: 0x0004FD22
		[DataSourceProperty]
		public MBBindingList<EncyclopediaFamilyMemberVM> Family
		{
			get
			{
				return this._family;
			}
			set
			{
				if (value != this._family)
				{
					this._family = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFamilyMemberVM>>(value, "Family");
				}
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x00051B40 File Offset: 0x0004FD40
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x00051B48 File Offset: 0x0004FD48
		[DataSourceProperty]
		public MBBindingList<HeroVM> Companions
		{
			get
			{
				return this._companions;
			}
			set
			{
				if (value != this._companions)
				{
					this._companions = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Companions");
				}
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x00051B66 File Offset: 0x0004FD66
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x00051B6E File Offset: 0x0004FD6E
		[DataSourceProperty]
		public MBBindingList<HeroVM> Enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				if (value != this._enemies)
				{
					this._enemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x00051B8C File Offset: 0x0004FD8C
		// (set) Token: 0x06001461 RID: 5217 RVA: 0x00051B94 File Offset: 0x0004FD94
		[DataSourceProperty]
		public MBBindingList<HeroVM> Allies
		{
			get
			{
				return this._allies;
			}
			set
			{
				if (value != this._allies)
				{
					this._allies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Allies");
				}
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x00051BB2 File Offset: 0x0004FDB2
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x00051BBA File Offset: 0x0004FDBA
		[DataSourceProperty]
		public MBBindingList<EncyclopediaHistoryEventVM> History
		{
			get
			{
				return this._history;
			}
			set
			{
				if (value != this._history)
				{
					this._history = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaHistoryEventVM>>(value, "History");
				}
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x00051BD8 File Offset: 0x0004FDD8
		// (set) Token: 0x06001465 RID: 5221 RVA: 0x00051BE0 File Offset: 0x0004FDE0
		[DataSourceProperty]
		public bool HasNeutralClan
		{
			get
			{
				return this._hasNeutralClan;
			}
			set
			{
				if (value != this._hasNeutralClan)
				{
					this._hasNeutralClan = value;
					base.OnPropertyChangedWithValue(value, "HasNeutralClan");
				}
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x00051BFE File Offset: 0x0004FDFE
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x00051C06 File Offset: 0x0004FE06
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (value != this._isDead)
				{
					this._isDead = value;
					base.OnPropertyChanged("IsAlive");
					base.OnPropertyChangedWithValue(value, "IsDead");
				}
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x00051C2F File Offset: 0x0004FE2F
		// (set) Token: 0x06001469 RID: 5225 RVA: 0x00051C37 File Offset: 0x0004FE37
		[DataSourceProperty]
		public bool IsInformationHidden
		{
			get
			{
				return this._isInformationHidden;
			}
			set
			{
				if (value != this._isInformationHidden)
				{
					this._isInformationHidden = value;
					base.OnPropertyChangedWithValue(value, "IsInformationHidden");
				}
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00051C55 File Offset: 0x0004FE55
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x00051C5D File Offset: 0x0004FE5D
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x00051C80 File Offset: 0x0004FE80
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x00051C88 File Offset: 0x0004FE88
		[DataSourceProperty]
		public HintViewModel PregnantHint
		{
			get
			{
				return this._pregnantHint;
			}
			set
			{
				if (value != this._pregnantHint)
				{
					this._pregnantHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PregnantHint");
				}
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x00051CA6 File Offset: 0x0004FEA6
		// (set) Token: 0x0600146F RID: 5231 RVA: 0x00051CAE File Offset: 0x0004FEAE
		[DataSourceProperty]
		public bool HasAnySkills
		{
			get
			{
				return this._hasAnySkills;
			}
			set
			{
				if (value != this._hasAnySkills)
				{
					this._hasAnySkills = value;
					base.OnPropertyChangedWithValue(value, "HasAnySkills");
				}
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x00051CCC File Offset: 0x0004FECC
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00051CD4 File Offset: 0x0004FED4
		[DataSourceProperty]
		public MBBindingList<HeroVM> AdditionalEnemies
		{
			get
			{
				return this._additionalEnemies;
			}
			set
			{
				if (value != this._additionalEnemies)
				{
					this._additionalEnemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "AdditionalEnemies");
				}
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00051CF2 File Offset: 0x0004FEF2
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00051CFA File Offset: 0x0004FEFA
		[DataSourceProperty]
		public MBBindingList<HeroVM> AdditionalAllies
		{
			get
			{
				return this._additionalAllies;
			}
			set
			{
				if (value != this._additionalAllies)
				{
					this._additionalAllies = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "AdditionalAllies");
				}
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00051D18 File Offset: 0x0004FF18
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x00051D20 File Offset: 0x0004FF20
		[DataSourceProperty]
		public bool AnyAdditionalAllies
		{
			get
			{
				return this._anyAdditionalAllies;
			}
			set
			{
				if (value != this._anyAdditionalAllies)
				{
					this._anyAdditionalAllies = value;
					base.OnPropertyChangedWithValue(value, "AnyAdditionalAllies");
				}
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x00051D3E File Offset: 0x0004FF3E
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x00051D46 File Offset: 0x0004FF46
		[DataSourceProperty]
		public bool AnyAdditionalEnemies
		{
			get
			{
				return this._anyAdditionalEnemies;
			}
			set
			{
				if (value != this._anyAdditionalEnemies)
				{
					this._anyAdditionalEnemies = value;
					base.OnPropertyChangedWithValue(value, "AnyAdditionalEnemies");
				}
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x00051D64 File Offset: 0x0004FF64
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x00051D6C File Offset: 0x0004FF6C
		[DataSourceProperty]
		public string AdditionalAlliesString
		{
			get
			{
				return this._additionalAlliesString;
			}
			set
			{
				if (value != this._additionalAlliesString)
				{
					this._additionalAlliesString = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalAlliesString");
				}
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x00051D8F File Offset: 0x0004FF8F
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x00051D97 File Offset: 0x0004FF97
		[DataSourceProperty]
		public string AdditionalEnemiesString
		{
			get
			{
				return this._additionalEnemiesString;
			}
			set
			{
				if (value != this._additionalEnemiesString)
				{
					this._additionalEnemiesString = value;
					base.OnPropertyChangedWithValue<string>(value, "AdditionalEnemiesString");
				}
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x00051DBA File Offset: 0x0004FFBA
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x00051DC2 File Offset: 0x0004FFC2
		[DataSourceProperty]
		public BasicTooltipViewModel AdditionalAlliesHint
		{
			get
			{
				return this._additionalAlliesHint;
			}
			set
			{
				if (value != this._additionalAlliesHint)
				{
					this._additionalAlliesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AdditionalAlliesHint");
				}
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00051DE0 File Offset: 0x0004FFE0
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x00051DE8 File Offset: 0x0004FFE8
		[DataSourceProperty]
		public BasicTooltipViewModel AdditionalEnemiesHint
		{
			get
			{
				return this._additionalEnemiesHint;
			}
			set
			{
				if (value != this._additionalEnemiesHint)
				{
					this._additionalEnemiesHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AdditionalEnemiesHint");
				}
			}
		}

		// Token: 0x04000933 RID: 2355
		private readonly Hero _hero;

		// Token: 0x04000934 RID: 2356
		private readonly TextObject _infoHiddenReasonText;

		// Token: 0x04000935 RID: 2357
		private List<Hero> _allRelatedHeroes;

		// Token: 0x04000936 RID: 2358
		private readonly HeroRelationComparer _relationAscendingComparer;

		// Token: 0x04000937 RID: 2359
		private readonly HeroRelationComparer _relationDescendingComparer;

		// Token: 0x04000938 RID: 2360
		private const int _alliesEnemiesCapacity = 13;

		// Token: 0x04000939 RID: 2361
		private MBBindingList<HeroVM> _enemies;

		// Token: 0x0400093A RID: 2362
		private MBBindingList<HeroVM> _allies;

		// Token: 0x0400093B RID: 2363
		private MBBindingList<EncyclopediaFamilyMemberVM> _family;

		// Token: 0x0400093C RID: 2364
		private MBBindingList<HeroVM> _companions;

		// Token: 0x0400093D RID: 2365
		private MBBindingList<EncyclopediaSettlementVM> _settlements;

		// Token: 0x0400093E RID: 2366
		private MBBindingList<EncyclopediaDwellingVM> _dwellings;

		// Token: 0x0400093F RID: 2367
		private MBBindingList<EncyclopediaHistoryEventVM> _history;

		// Token: 0x04000940 RID: 2368
		private MBBindingList<EncyclopediaSkillVM> _skills;

		// Token: 0x04000941 RID: 2369
		private MBBindingList<StringPairItemVM> _stats;

		// Token: 0x04000942 RID: 2370
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000943 RID: 2371
		private string _clanText;

		// Token: 0x04000944 RID: 2372
		private string _settlementsText;

		// Token: 0x04000945 RID: 2373
		private string _dwellingsText;

		// Token: 0x04000946 RID: 2374
		private string _alliesText;

		// Token: 0x04000947 RID: 2375
		private string _enemiesText;

		// Token: 0x04000948 RID: 2376
		private string _companionsText;

		// Token: 0x04000949 RID: 2377
		private string _lastSeenText;

		// Token: 0x0400094A RID: 2378
		private string _nameText;

		// Token: 0x0400094B RID: 2379
		private string _informationText;

		// Token: 0x0400094C RID: 2380
		private string _deceasedText;

		// Token: 0x0400094D RID: 2381
		private string _traitsText;

		// Token: 0x0400094E RID: 2382
		private string _skillsText;

		// Token: 0x0400094F RID: 2383
		private string _infoText;

		// Token: 0x04000950 RID: 2384
		private string _kingdomRankText;

		// Token: 0x04000951 RID: 2385
		private string _familyText;

		// Token: 0x04000952 RID: 2386
		private HeroViewModel _heroCharacter;

		// Token: 0x04000953 RID: 2387
		private bool _isCompanion;

		// Token: 0x04000954 RID: 2388
		private bool _isPregnant;

		// Token: 0x04000955 RID: 2389
		private bool _hasNeutralClan;

		// Token: 0x04000956 RID: 2390
		private bool _isDead;

		// Token: 0x04000957 RID: 2391
		private bool _isInformationHidden;

		// Token: 0x04000958 RID: 2392
		private HeroVM _master;

		// Token: 0x04000959 RID: 2393
		private EncyclopediaFactionVM _faction;

		// Token: 0x0400095A RID: 2394
		private string _masterText;

		// Token: 0x0400095B RID: 2395
		private HintViewModel _pregnantHint;

		// Token: 0x0400095C RID: 2396
		private bool _hasAnySkills;

		// Token: 0x0400095D RID: 2397
		private MBBindingList<HeroVM> _additionalAllies;

		// Token: 0x0400095E RID: 2398
		private MBBindingList<HeroVM> _additionalEnemies;

		// Token: 0x0400095F RID: 2399
		private bool _anyAdditionalAllies;

		// Token: 0x04000960 RID: 2400
		private bool _anyAdditionalEnemies;

		// Token: 0x04000961 RID: 2401
		private string _additionalAlliesString;

		// Token: 0x04000962 RID: 2402
		private string _additionalEnemiesString;

		// Token: 0x04000963 RID: 2403
		private BasicTooltipViewModel _additionalAlliesHint;

		// Token: 0x04000964 RID: 2404
		private BasicTooltipViewModel _additionalEnemiesHint;
	}
}
