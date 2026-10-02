using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000142 RID: 322
	public class CharacterDeveloperHeroItemVM : ViewModel
	{
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0007056F File Offset: 0x0006E76F
		public HeroDeveloper HeroDeveloper
		{
			get
			{
				return this.Hero.HeroDeveloper;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0007057C File Offset: 0x0006E77C
		// (set) Token: 0x06001E64 RID: 7780 RVA: 0x00070584 File Offset: 0x0006E784
		public Hero Hero { get; private set; }

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0007058D File Offset: 0x0006E78D
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x00070595 File Offset: 0x0006E795
		public int OrgUnspentFocusPoints { get; private set; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0007059E File Offset: 0x0006E79E
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x000705A6 File Offset: 0x0006E7A6
		public int OrgUnspentAttributePoints { get; private set; }

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000705AF File Offset: 0x0006E7AF
		public IReadOnlyPropertyOwner<CharacterAttribute> CharacterAttributes
		{
			get
			{
				return this._characterAttributes;
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000705B8 File Offset: 0x0006E7B8
		public CharacterDeveloperHeroItemVM(Hero hero, Action onPerkSelection)
		{
			this.LevelHint = new HintViewModel();
			this.Hero = hero;
			this.OrgUnspentFocusPoints = this.HeroDeveloper.UnspentFocusPoints;
			this.UnspentCharacterPoints = this.OrgUnspentFocusPoints;
			this.OrgUnspentAttributePoints = this.HeroDeveloper.UnspentAttributePoints;
			this.UnspentAttributePoints = this.OrgUnspentAttributePoints;
			this.Attributes = new MBBindingList<CharacterAttributeItemVM>();
			this._characterAttributes = new PropertyOwner<CharacterAttribute>();
			this.PerkSelection = new PerkSelectionVM(this.HeroDeveloper, new Action<SkillObject>(this.RefreshPerksOfSkill), onPerkSelection);
			this.InitializeCharacter();
			this.RefreshValues();
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x00070658 File Offset: 0x0006E858
		public override void RefreshValues()
		{
			base.RefreshValues();
			StringHelpers.SetCharacterProperties("HERO", this.Hero.CharacterObject, null, false);
			this.HeroNameText = this.Hero.CharacterObject.Name.ToString();
			MBTextManager.SetTextVariable("LEVEL", this.Hero.CharacterObject.Level + 1);
			this.HeroNextLevelText = GameTexts.FindText("str_level_with_value", null).ToString();
			this.HeroInfoText = GameTexts.FindText("str_hero_name_level", null).ToString();
			this.FocusPointsText = GameTexts.FindText("str_focus_points", null).ToString();
			this.InitializeCharacter();
			this.Skills.ApplyActionOnAllItems(delegate(SkillVM x)
			{
				x.RefreshValues();
			});
			this.CurrentSkill.RefreshValues();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x00070738 File Offset: 0x0006E938
		private void InitializeCharacter()
		{
			this.HeroCharacter = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.Skills = new MBBindingList<SkillVM>();
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.Attributes.Clear();
			this.HeroCharacter.FillFrom(this.Hero, -1, false, false);
			this.HeroCharacter.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.HeroCharacter.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
			List<CharacterAttribute> list = TaleWorlds.CampaignSystem.Extensions.Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this._characterAttributes.SetPropertyValue(characterAttribute, this.Hero.GetAttributeValue(characterAttribute));
				this.Attributes.Add(new CharacterAttributeItemVM(this.Hero, characterAttribute, this, new Action<CharacterAttributeItemVM>(this.OnInspectAttribute), new Action<CharacterAttributeItemVM>(this.OnAddAttributePoint)));
			}
			List<SkillObject> list2 = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			foreach (SkillObject skillObject in list2)
			{
				this.Skills.Add(new SkillVM(skillObject, this, new Action<PerkVM>(this.OnStartPerkSelection)));
			}
			this.HasExtraSkills = this.Skills.Count > 18;
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.RefreshWithCurrentValues();
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.RefreshWithCurrentValues();
			}
			this.SetCurrentSkill(this.Skills[0]);
			this.RefreshCharacterValues();
			this.CharacterStats = new MBBindingList<StringPairItemVM>();
			if (this.Hero.GovernorOf != null)
			{
				GameTexts.SetVariable("SETTLEMENT_NAME", this.Hero.GovernorOf.Name.ToString());
				this.CharacterStats.Add(new StringPairItemVM(GameTexts.FindText("str_governor_of_label", null).ToString(), "", null));
			}
			if (MobileParty.MainParty.GetHeroPartyRoles(this.Hero).Count > 0)
			{
				this.CharacterStats.Add(new StringPairItemVM(CampaignUIHelper.GetHeroClanRoleText(this.Hero, Clan.PlayerClan), "", null));
			}
			foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
			{
				if (this.Hero.GetTraitLevel(traitObject) != 0)
				{
					this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this.Hero));
				}
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x00070A6C File Offset: 0x0006EC6C
		private void OnInspectAttribute(CharacterAttributeItemVM att)
		{
			this.CurrentInspectedAttribute = att;
			this.IsInspectingAnAttribute = true;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x00070A7C File Offset: 0x0006EC7C
		private void OnAddAttributePoint(CharacterAttributeItemVM att)
		{
			int unspentAttributePoints = this.UnspentAttributePoints;
			this.UnspentAttributePoints = unspentAttributePoints - 1;
			this._characterAttributes.SetPropertyValue(att.AttributeType, this._characterAttributes.GetPropertyValue(att.AttributeType) + 1);
			this.RefreshCharacterValues();
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x00070AC3 File Offset: 0x0006ECC3
		public void ExecuteStopInspectingCurrentAttribute()
		{
			this.IsInspectingAnAttribute = false;
			this.CurrentInspectedAttribute = null;
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x00070AD4 File Offset: 0x0006ECD4
		public void RefreshCharacterValues()
		{
			this.CurrentCharacterLevelLbl = this.Hero.Level.ToString();
			this.CurrentTotalXp = this.HeroDeveloper.TotalXp;
			this.XpRequiredForNextLevel = Campaign.Current.Models.CharacterDevelopmentModel.SkillsRequiredForLevel(this.Hero.Level + 1);
			GameTexts.SetVariable("CURRENTAMOUNT", this.CurrentTotalXp);
			GameTexts.SetVariable("TARGETAMOUNT", this.XpRequiredForNextLevel);
			this.LevelProgressText = GameTexts.FindText("str_character_skillpoint_progress", null).ToString();
			GameTexts.SetVariable("newline", "\n");
			GameTexts.SetVariable("CURRENT_SKILL_POINTS", this.CurrentTotalXp);
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_total_skill_points", null));
			GameTexts.SetVariable("NEXT_SKILL_POINTS", this.XpRequiredForNextLevel);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_next_level_at", null));
			string text = GameTexts.FindText("str_string_newline_string", null).ToString();
			GameTexts.SetVariable("SKILL_LEVEL_FOR_LEVEL_UP", this.XpRequiredForNextLevel - this.CurrentTotalXp);
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_how_to_level_up_character", null));
			string text2 = GameTexts.FindText("str_string_newline_string", null).ToString();
			this.LevelHint.HintText = new TextObject("{=!}" + text2, null);
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.RefreshWithCurrentValues();
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.RefreshWithCurrentValues();
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00070CAC File Offset: 0x0006EEAC
		public void RefreshPerksOfSkill(SkillObject skill)
		{
			SkillVM skillVM = this.Skills.SingleOrDefault<SkillVM>((SkillVM s) => s.Skill == skill);
			if (skillVM == null)
			{
				return;
			}
			skillVM.RefreshLists(null);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00070CE8 File Offset: 0x0006EEE8
		public void ResetChanges(bool isCancel)
		{
			this.PerkSelection.ResetSelectedPerks();
			foreach (CharacterAttribute characterAttribute in TaleWorlds.CampaignSystem.Extensions.Attributes.All)
			{
				this._characterAttributes.SetPropertyValue(characterAttribute, this.Hero.GetAttributeValue(characterAttribute));
			}
			if (!isCancel)
			{
				this.UnspentCharacterPoints = this.OrgUnspentFocusPoints;
				this.UnspentAttributePoints = this.OrgUnspentAttributePoints;
			}
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.Reset();
			}
			if (!isCancel)
			{
				foreach (CharacterAttributeItemVM characterAttributeItemVM2 in this.Attributes)
				{
					characterAttributeItemVM2.RefreshWithCurrentValues();
				}
			}
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.ResetChanges();
			}
			if (!isCancel)
			{
				foreach (SkillVM skillVM2 in this.Skills)
				{
					skillVM2.RefreshWithCurrentValues();
				}
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00070E5C File Offset: 0x0006F05C
		public void ApplyChanges()
		{
			this.PerkSelection.ApplySelectedPerks();
			foreach (CharacterAttributeItemVM characterAttributeItemVM in this.Attributes)
			{
				characterAttributeItemVM.Commit();
			}
			foreach (SkillVM skillVM in this.Skills)
			{
				skillVM.ApplyChanges();
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x00070EEC File Offset: 0x0006F0EC
		public void SetCurrentSkill(SkillVM skill)
		{
			if (this.CurrentSkill != null)
			{
				this.CurrentSkill.IsInspected = false;
			}
			this.CurrentSkill = skill;
			this.CurrentSkill.IsInspected = true;
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x00070F18 File Offset: 0x0006F118
		public bool IsThereAnyChanges()
		{
			bool flag = this.Skills.Any<SkillVM>((SkillVM s) => s.IsThereAnyChanges());
			return this.UnspentCharacterPoints != this.OrgUnspentFocusPoints || this.UnspentAttributePoints != this.OrgUnspentAttributePoints || this.PerkSelection.IsAnyPerkSelected() || flag;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00070F7C File Offset: 0x0006F17C
		public int GetRequiredFocusPointsToAddFocusWithCurrentFocus(SkillObject skill)
		{
			return this.Hero.HeroDeveloper.GetRequiredFocusPointsToAddFocus(skill);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00070F8F File Offset: 0x0006F18F
		public bool CanAddFocusToSkillWithFocusAmount(int currentFocusAmount)
		{
			return currentFocusAmount < Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill && this.UnspentCharacterPoints > 0;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x00070FB4 File Offset: 0x0006F1B4
		public bool IsSkillMaxAmongOtherSkills(SkillVM skill)
		{
			if (this.Skills.Count > 0)
			{
				int currentFocusLevel = skill.CurrentFocusLevel;
				return this.Skills.Max<SkillVM>((SkillVM s) => s.CurrentFocusLevel) <= currentFocusLevel;
			}
			return false;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00071008 File Offset: 0x0006F208
		public string GetNameWithNumOfUnopenedPerks()
		{
			if (this.Skills.Sum<SkillVM>((SkillVM s) => s.NumOfUnopenedPerks) == 0)
			{
				return this.HeroNameText;
			}
			GameTexts.SetVariable("STR1", this.HeroNameText);
			GameTexts.SetVariable("STR2", "{=!}<img src=\"CharacterDeveloper\\UnselectedPerksIcon\" extend=\"2\">");
			return GameTexts.FindText("str_STR1_space_STR2", null).ToString();
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x00071077 File Offset: 0x0006F277
		private void OnStartPerkSelection(PerkVM perk)
		{
			this.PerkSelection.SetCurrentSelectionPerk(perk);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x00071085 File Offset: 0x0006F285
		public int GetNumberOfUnselectedPerks()
		{
			return this.Skills.Sum<SkillVM>((SkillVM s) => s.NumOfUnopenedPerks);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000710B1 File Offset: 0x0006F2B1
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.HeroCharacter.OnFinalize();
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x000710C4 File Offset: 0x0006F2C4
		// (set) Token: 0x06001E7E RID: 7806 RVA: 0x000710CC File Offset: 0x0006F2CC
		[DataSourceProperty]
		public MBBindingList<SkillVM> Skills
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
					base.OnPropertyChangedWithValue<MBBindingList<SkillVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x000710EA File Offset: 0x0006F2EA
		// (set) Token: 0x06001E80 RID: 7808 RVA: 0x000710F2 File Offset: 0x0006F2F2
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> CharacterStats
		{
			get
			{
				return this._characterStats;
			}
			set
			{
				if (value != this._characterStats)
				{
					this._characterStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "CharacterStats");
				}
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x00071110 File Offset: 0x0006F310
		// (set) Token: 0x06001E82 RID: 7810 RVA: 0x00071118 File Offset: 0x0006F318
		[DataSourceProperty]
		public MBBindingList<CharacterAttributeItemVM> Attributes
		{
			get
			{
				return this._attributes;
			}
			set
			{
				if (value != this._attributes)
				{
					this._attributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterAttributeItemVM>>(value, "Attributes");
				}
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x00071136 File Offset: 0x0006F336
		// (set) Token: 0x06001E84 RID: 7812 RVA: 0x0007113E File Offset: 0x0006F33E
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

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x0007115C File Offset: 0x0006F35C
		// (set) Token: 0x06001E86 RID: 7814 RVA: 0x00071164 File Offset: 0x0006F364
		[DataSourceProperty]
		public PerkSelectionVM PerkSelection
		{
			get
			{
				return this._perkSelection;
			}
			set
			{
				if (value != this._perkSelection)
				{
					this._perkSelection = value;
					base.OnPropertyChangedWithValue<PerkSelectionVM>(value, "PerkSelection");
				}
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x00071182 File Offset: 0x0006F382
		// (set) Token: 0x06001E88 RID: 7816 RVA: 0x0007118A File Offset: 0x0006F38A
		[DataSourceProperty]
		public SkillVM CurrentSkill
		{
			get
			{
				return this._currentSkill;
			}
			set
			{
				if (value != this._currentSkill)
				{
					this._currentSkill = value;
					base.OnPropertyChangedWithValue<SkillVM>(value, "CurrentSkill");
				}
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000711A8 File Offset: 0x0006F3A8
		// (set) Token: 0x06001E8A RID: 7818 RVA: 0x000711B0 File Offset: 0x0006F3B0
		[DataSourceProperty]
		public CharacterAttributeItemVM CurrentInspectedAttribute
		{
			get
			{
				return this._currentInspectedAttribute;
			}
			set
			{
				if (value != this._currentInspectedAttribute)
				{
					this._currentInspectedAttribute = value;
					base.OnPropertyChangedWithValue<CharacterAttributeItemVM>(value, "CurrentInspectedAttribute");
				}
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000711CE File Offset: 0x0006F3CE
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x000711D6 File Offset: 0x0006F3D6
		[DataSourceProperty]
		public string FocusPointsText
		{
			get
			{
				return this._focusPointsText;
			}
			set
			{
				if (value != this._focusPointsText)
				{
					this._focusPointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusPointsText");
				}
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000711F9 File Offset: 0x0006F3F9
		// (set) Token: 0x06001E8E RID: 7822 RVA: 0x00071201 File Offset: 0x0006F401
		[DataSourceProperty]
		public string CurrentCharacterLevelLbl
		{
			get
			{
				return this._currentCharacterLevelLbl;
			}
			set
			{
				if (value != this._currentCharacterLevelLbl)
				{
					this._currentCharacterLevelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterLevelLbl");
				}
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x00071224 File Offset: 0x0006F424
		// (set) Token: 0x06001E90 RID: 7824 RVA: 0x0007122C File Offset: 0x0006F42C
		[DataSourceProperty]
		public string LevelProgressText
		{
			get
			{
				return this._levelProgressText;
			}
			set
			{
				if (value != this._levelProgressText)
				{
					this._levelProgressText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelProgressText");
				}
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x0007124F File Offset: 0x0006F44F
		// (set) Token: 0x06001E92 RID: 7826 RVA: 0x00071257 File Offset: 0x0006F457
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

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x00071275 File Offset: 0x0006F475
		// (set) Token: 0x06001E94 RID: 7828 RVA: 0x0007127D File Offset: 0x0006F47D
		[DataSourceProperty]
		public bool IsInspectingAnAttribute
		{
			get
			{
				return this._isInspectingAnAttribute;
			}
			set
			{
				if (value != this._isInspectingAnAttribute)
				{
					this._isInspectingAnAttribute = value;
					base.OnPropertyChangedWithValue(value, "IsInspectingAnAttribute");
				}
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0007129B File Offset: 0x0006F49B
		// (set) Token: 0x06001E96 RID: 7830 RVA: 0x000712A3 File Offset: 0x0006F4A3
		[DataSourceProperty]
		public int LevelProgressPercentage
		{
			get
			{
				return this._levelProgressPercentage;
			}
			set
			{
				if (value != this._levelProgressPercentage)
				{
					this._levelProgressPercentage = value;
					base.OnPropertyChangedWithValue(value, "LevelProgressPercentage");
				}
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x000712C1 File Offset: 0x0006F4C1
		// (set) Token: 0x06001E98 RID: 7832 RVA: 0x000712C9 File Offset: 0x0006F4C9
		[DataSourceProperty]
		public int CurrentTotalXp
		{
			get
			{
				return this._currentTotalXp;
			}
			set
			{
				if (value != this._currentTotalXp)
				{
					this._currentTotalXp = value;
					base.OnPropertyChangedWithValue(value, "CurrentTotalXp");
				}
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x000712E7 File Offset: 0x0006F4E7
		// (set) Token: 0x06001E9A RID: 7834 RVA: 0x000712EF File Offset: 0x0006F4EF
		[DataSourceProperty]
		public int XpRequiredForNextLevel
		{
			get
			{
				return this._xpRequiredForNextLevel;
			}
			set
			{
				if (value != this._xpRequiredForNextLevel)
				{
					this._xpRequiredForNextLevel = value;
					base.OnPropertyChangedWithValue(value, "XpRequiredForNextLevel");
				}
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0007130D File Offset: 0x0006F50D
		// (set) Token: 0x06001E9C RID: 7836 RVA: 0x00071315 File Offset: 0x0006F515
		[DataSourceProperty]
		public int UnspentCharacterPoints
		{
			get
			{
				return this._unspentCharacterPoints;
			}
			set
			{
				if (value != this._unspentCharacterPoints)
				{
					this._unspentCharacterPoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentCharacterPoints");
				}
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x00071333 File Offset: 0x0006F533
		// (set) Token: 0x06001E9E RID: 7838 RVA: 0x0007133B File Offset: 0x0006F53B
		[DataSourceProperty]
		public int UnspentAttributePoints
		{
			get
			{
				return this._unspentAttributePoints;
			}
			set
			{
				if (value != this._unspentAttributePoints)
				{
					this._unspentAttributePoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentAttributePoints");
				}
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x00071359 File Offset: 0x0006F559
		// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x00071361 File Offset: 0x0006F561
		[DataSourceProperty]
		public HintViewModel LevelHint
		{
			get
			{
				return this._levelHint;
			}
			set
			{
				if (value != this._levelHint)
				{
					this._levelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LevelHint");
				}
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x0007137F File Offset: 0x0006F57F
		// (set) Token: 0x06001EA2 RID: 7842 RVA: 0x00071387 File Offset: 0x0006F587
		[DataSourceProperty]
		public string HeroNameText
		{
			get
			{
				return this._heroNameText;
			}
			set
			{
				if (value != this._heroNameText)
				{
					this._heroNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroNameText");
				}
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x000713AA File Offset: 0x0006F5AA
		// (set) Token: 0x06001EA4 RID: 7844 RVA: 0x000713B2 File Offset: 0x0006F5B2
		[DataSourceProperty]
		public string HeroInfoText
		{
			get
			{
				return this._heroInfoText;
			}
			set
			{
				if (value != this._heroInfoText)
				{
					this._heroInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroInfoText");
				}
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x000713D5 File Offset: 0x0006F5D5
		// (set) Token: 0x06001EA6 RID: 7846 RVA: 0x000713DD File Offset: 0x0006F5DD
		[DataSourceProperty]
		public string HeroNextLevelText
		{
			get
			{
				return this._heroNextLevelText;
			}
			set
			{
				if (value != this._heroNextLevelText)
				{
					this._heroNextLevelText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroNextLevelText");
				}
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x00071400 File Offset: 0x0006F600
		// (set) Token: 0x06001EA8 RID: 7848 RVA: 0x00071408 File Offset: 0x0006F608
		[DataSourceProperty]
		public bool HasExtraSkills
		{
			get
			{
				return this._hasExtraSkills;
			}
			set
			{
				if (value != this._hasExtraSkills)
				{
					this._hasExtraSkills = value;
					base.OnPropertyChangedWithValue(value, "HasExtraSkills");
				}
			}
		}

		// Token: 0x04000E37 RID: 3639
		private readonly PropertyOwner<CharacterAttribute> _characterAttributes;

		// Token: 0x04000E38 RID: 3640
		private MBBindingList<SkillVM> _skills;

		// Token: 0x04000E39 RID: 3641
		private PerkSelectionVM _perkSelection;

		// Token: 0x04000E3A RID: 3642
		private HeroViewModel _heroCharacter;

		// Token: 0x04000E3B RID: 3643
		private int _xpRequiredForNextLevel;

		// Token: 0x04000E3C RID: 3644
		private int _currentTotalXp;

		// Token: 0x04000E3D RID: 3645
		private int _levelProgressPercentage;

		// Token: 0x04000E3E RID: 3646
		private int _unspentCharacterPoints;

		// Token: 0x04000E3F RID: 3647
		private int _unspentAttributePoints;

		// Token: 0x04000E40 RID: 3648
		private string _currentCharacterLevelLbl;

		// Token: 0x04000E41 RID: 3649
		private string _levelProgressText;

		// Token: 0x04000E42 RID: 3650
		private string _heroNameText;

		// Token: 0x04000E43 RID: 3651
		private string _heroInfoText;

		// Token: 0x04000E44 RID: 3652
		private bool _isInspectingAnAttribute;

		// Token: 0x04000E45 RID: 3653
		private HintViewModel _levelHint;

		// Token: 0x04000E46 RID: 3654
		private SkillVM _currentSkill;

		// Token: 0x04000E47 RID: 3655
		private CharacterAttributeItemVM _currentInspectedAttribute;

		// Token: 0x04000E48 RID: 3656
		private string _heroNextLevelText;

		// Token: 0x04000E49 RID: 3657
		private string _focusPointsText;

		// Token: 0x04000E4A RID: 3658
		private MBBindingList<StringPairItemVM> _characterStats;

		// Token: 0x04000E4B RID: 3659
		private MBBindingList<CharacterAttributeItemVM> _attributes;

		// Token: 0x04000E4C RID: 3660
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000E4D RID: 3661
		private bool _hasExtraSkills;
	}
}
