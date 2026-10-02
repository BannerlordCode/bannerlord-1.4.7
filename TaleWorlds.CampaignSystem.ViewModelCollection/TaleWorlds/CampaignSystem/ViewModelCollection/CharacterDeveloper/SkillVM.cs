using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000145 RID: 325
	public class SkillVM : ViewModel
	{
		// Token: 0x06001F16 RID: 7958 RVA: 0x0007278C File Offset: 0x0007098C
		public SkillVM(SkillObject skill, CharacterDeveloperHeroItemVM heroItem, Action<PerkVM> onStartPerkSelection)
		{
			SkillVM <>4__this = this;
			this._heroItem = heroItem;
			this.Skill = skill;
			this.MaxLevel = 300;
			this.SkillId = skill.StringId;
			this._onStartPerkSelection = onStartPerkSelection;
			this.IsInspected = false;
			this.SkillEffects = new MBBindingList<BindingListStringItem>();
			this.Perks = new MBBindingList<PerkVM>();
			this.AddFocusHint = new HintViewModel();
			this.LearningRateTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetLearningRateTooltip(<>4__this._heroItem.CharacterAttributes, <>4__this.CurrentFocusLevel, heroItem.Hero.GetSkillValue(skill), <>4__this.Skill));
			this.LearningLimitTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetLearningLimitTooltip(<>4__this._heroItem.CharacterAttributes, <>4__this.CurrentFocusLevel, <>4__this.Skill));
			this.InitializeValues();
			this._focusConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_skill_focus");
			this._skillConceptObj = Concept.All.SingleOrDefault<Concept>((Concept c) => c.StringId == "str_game_objects_skills");
			this.RefreshValues();
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000728BC File Offset: 0x00070ABC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AddFocusText = GameTexts.FindText("str_add_focus", null).ToString();
			this.HowToLearnText = this.Skill.HowToLearnSkillText.ToString();
			this.HowToLearnTitle = GameTexts.FindText("str_how_to_learn", null).ToString();
			this.DescriptionText = this.Skill.Description.ToString();
			this.NameText = this.Skill.Name.ToString();
			this.AttributesText = GameTexts.GameTextHelper.MergeTextObjectsWithComma(this.Skill.Attributes.Select<CharacterAttribute, TextObject>((CharacterAttribute x) => x.Abbreviation).ToList<TextObject>(), false).ToString();
			this.InitializeValues();
			this.RefreshWithCurrentValues();
			this.SkillEffects.ApplyActionOnAllItems(delegate(BindingListStringItem x)
			{
				x.RefreshValues();
			});
			this.Perks.ApplyActionOnAllItems(delegate(PerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000729E4 File Offset: 0x00070BE4
		public void InitializeValues()
		{
			if (this._heroItem.HeroDeveloper == null)
			{
				this.Level = 0;
			}
			else
			{
				this.Level = this._heroItem.HeroDeveloper.Hero.GetSkillValue(this.Skill);
				this.NextLevel = this.Level + 1;
				this.CurrentSkillXP = this._heroItem.HeroDeveloper.GetSkillXpProgress(this.Skill);
				this.XpRequiredForNextLevel = Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(this.Level + 1) - Campaign.Current.Models.CharacterDevelopmentModel.GetXpRequiredForSkillLevel(this.Level);
				this.ProgressPercentage = 100.0 * (double)this._currentSkillXP / (double)this.XpRequiredForNextLevel;
				this.ProgressHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("CURRENT_XP", this.CurrentSkillXP.ToString());
					GameTexts.SetVariable("LEVEL_MAX_XP", this.XpRequiredForNextLevel.ToString());
					return GameTexts.FindText("str_current_xp_over_max", null).ToString();
				});
				GameTexts.SetVariable("CURRENT_XP", this.CurrentSkillXP.ToString());
				GameTexts.SetVariable("LEVEL_MAX_XP", this.XpRequiredForNextLevel.ToString());
				this.ProgressText = GameTexts.FindText("str_current_xp_over_max", null).ToString();
				this.SkillXPHint = new BasicTooltipViewModel(delegate
				{
					GameTexts.SetVariable("REQUIRED_XP_FOR_NEXT_LEVEL", this.XpRequiredForNextLevel - this.CurrentSkillXP);
					return GameTexts.FindText("str_skill_xp_hint", null).ToString();
				});
			}
			this._orgFocusAmount = this._heroItem.HeroDeveloper.GetFocus(this.Skill);
			this.CurrentFocusLevel = this._orgFocusAmount;
			this.CreateLists();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x00072B54 File Offset: 0x00070D54
		public void RefreshWithCurrentValues()
		{
			float resultNumber = Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningRate(this._heroItem.CharacterAttributes, this.CurrentFocusLevel, this._heroItem.Hero.GetSkillValue(this.Skill), this.Skill, false).ResultNumber;
			GameTexts.SetVariable("COUNT", resultNumber.ToString("0.00"));
			this.CurrentLearningRateText = GameTexts.FindText("str_learning_rate_COUNT", null).ToString();
			this.CanLearnSkill = Math.Round((double)resultNumber, 2) > 0.0;
			this.LearningRate = resultNumber;
			this.FullLearningRateLevel = MathF.Round(Campaign.Current.Models.CharacterDevelopmentModel.CalculateLearningLimit(this._heroItem.CharacterAttributes, this.CurrentFocusLevel, this.Skill, false).ResultNumber);
			int requiredFocusPointsToAddFocusWithCurrentFocus = this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
			GameTexts.SetVariable("COSTAMOUNT", requiredFocusPointsToAddFocusWithCurrentFocus);
			this.FocusCostText = requiredFocusPointsToAddFocusWithCurrentFocus.ToString();
			GameTexts.SetVariable("COUNT", requiredFocusPointsToAddFocusWithCurrentFocus);
			GameTexts.SetVariable("RIGHT", "");
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_cost_COUNT", null));
			MBTextManager.SetTextVariable("FOCUS_ICON", "{=!}<img src=\"CharacterDeveloper\\cp_icon\">", false);
			this.NextLevelCostText = GameTexts.FindText("str_sf_text_with_focus_icon", null).ToString();
			this.RefreshCanAddFocus();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00072CBC File Offset: 0x00070EBC
		public void CreateLists()
		{
			this.SkillEffects.Clear();
			this.Perks.Clear();
			int skillValue = this._heroItem.HeroDeveloper.Hero.GetSkillValue(this.Skill);
			foreach (SkillEffect skillEffect in SkillEffect.All.Where<SkillEffect>((SkillEffect x) => x.EffectedSkill == this.Skill))
			{
				this.SkillEffects.Add(new BindingListStringItem(CampaignUIHelper.GetSkillEffectText(skillEffect, skillValue)));
			}
			foreach (PerkObject perkObject in from p in PerkObject.All
				where p.Skill == this.Skill
				orderby p.RequiredSkillValue
				select p)
			{
				PerkVM.PerkAlternativeType perkAlternativeType = ((perkObject.AlternativePerk == null) ? PerkVM.PerkAlternativeType.NoAlternative : ((perkObject.StringId.CompareTo(perkObject.AlternativePerk.StringId) < 0) ? PerkVM.PerkAlternativeType.FirstAlternative : PerkVM.PerkAlternativeType.SecondAlternative));
				PerkVM perkVM = new PerkVM(perkObject, this.IsPerkAvailable(perkObject), perkAlternativeType, new Action<PerkVM>(this.OnStartPerkSelection), new Action<PerkVM>(this.OnPerkSelectionOver), new Func<PerkObject, bool>(this.IsPerkSelected), new Func<PerkObject, bool>(this.IsPreviousPerkSelected));
				this.Perks.Add(perkVM);
			}
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x00072E50 File Offset: 0x00071050
		public void RefreshLists(SkillObject skill = null)
		{
			if (skill != null && skill != this.Skill)
			{
				return;
			}
			foreach (PerkVM perkVM in this.Perks)
			{
				perkVM.RefreshState();
			}
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x00072EB0 File Offset: 0x000710B0
		private void RefreshNumOfUnopenedPerks()
		{
			int num = 0;
			foreach (PerkVM perkVM in this.Perks)
			{
				if ((perkVM.CurrentState == PerkVM.PerkStates.EarnedButNotSelected || perkVM.CurrentState == PerkVM.PerkStates.EarnedPreviousPerkNotSelected) && (perkVM.AlternativeType == 1 || perkVM.AlternativeType == 0))
				{
					num++;
				}
			}
			this.NumOfUnopenedPerks = num;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00072F28 File Offset: 0x00071128
		private bool IsPerkSelected(PerkObject perk)
		{
			return this._heroItem.HeroDeveloper.GetPerkValue(perk) || this._heroItem.PerkSelection.IsPerkSelected(perk);
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x00072F50 File Offset: 0x00071150
		private bool IsPreviousPerkSelected(PerkObject perk)
		{
			IEnumerable<PerkObject> enumerable = PerkObject.All.Where<PerkObject>((PerkObject p) => p.Skill == perk.Skill && p.RequiredSkillValue < perk.RequiredSkillValue);
			if (!enumerable.Any<PerkObject>())
			{
				return true;
			}
			PerkObject perkObject = enumerable.MaxBy<PerkObject, float>((PerkObject p) => p.RequiredSkillValue - perk.RequiredSkillValue);
			return this.IsPerkSelected(perkObject) || (perkObject.AlternativePerk != null && this.IsPerkSelected(perkObject.AlternativePerk));
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00072FBF File Offset: 0x000711BF
		private bool IsPerkAvailable(PerkObject perk)
		{
			return perk.RequiredSkillValue <= (float)this.Level;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00072FD4 File Offset: 0x000711D4
		public void RefreshCanAddFocus()
		{
			bool flag = this._heroItem.UnspentCharacterPoints >= this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
			bool flag2 = this._currentFocusLevel >= Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill;
			string addFocusHintString = CampaignUIHelper.GetAddFocusHintString(flag, flag2, this.CurrentFocusLevel);
			this.AddFocusHint.HintText = (string.IsNullOrEmpty(addFocusHintString) ? TextObject.GetEmpty() : new TextObject("{=!}" + addFocusHintString, null));
			this.CanAddFocus = this._heroItem.CanAddFocusToSkillWithFocusAmount(this._currentFocusLevel);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00073074 File Offset: 0x00071274
		public void ExecuteAddFocus()
		{
			if (this.CanAddFocus)
			{
				this._heroItem.UnspentCharacterPoints -= this._heroItem.GetRequiredFocusPointsToAddFocusWithCurrentFocus(this.Skill);
				int currentFocusLevel = this.CurrentFocusLevel;
				this.CurrentFocusLevel = currentFocusLevel + 1;
				this._heroItem.RefreshCharacterValues();
				this.RefreshWithCurrentValues();
				MBInformationManager.HideInformations();
				Game.Current.EventManager.TriggerEvent<FocusAddedByPlayerEvent>(new FocusAddedByPlayerEvent(this._heroItem.Hero, this.Skill));
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000730F7 File Offset: 0x000712F7
		public void ExecuteShowFocusConcept()
		{
			if (this._focusConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._focusConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Focus encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\SkillVM.cs", "ExecuteShowFocusConcept", 253);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00073135 File Offset: 0x00071335
		public void ExecuteShowSkillConcept()
		{
			if (this._focusConceptObj != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this._skillConceptObj.EncyclopediaLink);
				return;
			}
			Debug.FailedAssert("Couldn't find Focus encyclopedia page", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\CharacterDeveloper\\SkillVM.cs", "ExecuteShowSkillConcept", 265);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00073173 File Offset: 0x00071373
		public void ExecuteInspect()
		{
			this._heroItem.SetCurrentSkill(this);
			this.RefreshCanAddFocus();
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00073187 File Offset: 0x00071387
		public void ResetChanges()
		{
			this.CurrentFocusLevel = this._orgFocusAmount;
			this.Perks.ApplyActionOnAllItems(delegate(PerkVM p)
			{
				p.RefreshState();
			});
			this.RefreshNumOfUnopenedPerks();
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000731C5 File Offset: 0x000713C5
		public bool IsThereAnyChanges()
		{
			return this.CurrentFocusLevel != this._orgFocusAmount;
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000731D8 File Offset: 0x000713D8
		public void ApplyChanges()
		{
			for (int i = 0; i < this.CurrentFocusLevel - this._orgFocusAmount; i++)
			{
				this._heroItem.HeroDeveloper.AddFocus(this.Skill, 1, true);
			}
			this._orgFocusAmount = this.CurrentFocusLevel;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00073224 File Offset: 0x00071424
		private void OnStartPerkSelection(PerkVM perk)
		{
			this._onStartPerkSelection(perk);
			if (perk.AlternativeType != 0)
			{
				this.Perks.SingleOrDefault<PerkVM>((PerkVM p) => p.Perk == perk.Perk.AlternativePerk);
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x00073274 File Offset: 0x00071474
		private void OnPerkSelectionOver(PerkVM perk)
		{
			if (perk.AlternativeType != 0)
			{
				this.Perks.SingleOrDefault<PerkVM>((PerkVM p) => p.Perk == perk.Perk.AlternativePerk);
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06001F2A RID: 7978 RVA: 0x000732B3 File Offset: 0x000714B3
		// (set) Token: 0x06001F2B RID: 7979 RVA: 0x000732BB File Offset: 0x000714BB
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000732DE File Offset: 0x000714DE
		// (set) Token: 0x06001F2D RID: 7981 RVA: 0x000732E6 File Offset: 0x000714E6
		[DataSourceProperty]
		public string HowToLearnText
		{
			get
			{
				return this._howToLearnText;
			}
			set
			{
				if (value != this._howToLearnText)
				{
					this._howToLearnText = value;
					base.OnPropertyChangedWithValue<string>(value, "HowToLearnText");
				}
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06001F2E RID: 7982 RVA: 0x00073309 File Offset: 0x00071509
		// (set) Token: 0x06001F2F RID: 7983 RVA: 0x00073311 File Offset: 0x00071511
		[DataSourceProperty]
		public string HowToLearnTitle
		{
			get
			{
				return this._howToLearnTitle;
			}
			set
			{
				if (value != this._howToLearnTitle)
				{
					this._howToLearnTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "HowToLearnTitle");
				}
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x00073334 File Offset: 0x00071534
		// (set) Token: 0x06001F31 RID: 7985 RVA: 0x0007333C File Offset: 0x0007153C
		[DataSourceProperty]
		public string AttributesText
		{
			get
			{
				return this._attributesText;
			}
			set
			{
				if (value != this._attributesText)
				{
					this._attributesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributesText");
				}
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x0007335F File Offset: 0x0007155F
		// (set) Token: 0x06001F33 RID: 7987 RVA: 0x00073367 File Offset: 0x00071567
		[DataSourceProperty]
		public bool CanAddFocus
		{
			get
			{
				return this._canAddFocus;
			}
			set
			{
				if (value != this._canAddFocus)
				{
					this._canAddFocus = value;
					base.OnPropertyChangedWithValue(value, "CanAddFocus");
				}
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x00073385 File Offset: 0x00071585
		// (set) Token: 0x06001F35 RID: 7989 RVA: 0x0007338D File Offset: 0x0007158D
		[DataSourceProperty]
		public bool CanLearnSkill
		{
			get
			{
				return this._canLearnSkill;
			}
			set
			{
				if (value != this._canLearnSkill)
				{
					this._canLearnSkill = value;
					base.OnPropertyChangedWithValue(value, "CanLearnSkill");
				}
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x000733AB File Offset: 0x000715AB
		// (set) Token: 0x06001F37 RID: 7991 RVA: 0x000733B3 File Offset: 0x000715B3
		[DataSourceProperty]
		public string NextLevelLearningRateText
		{
			get
			{
				return this._nextLevelLearningRateText;
			}
			set
			{
				if (value != this._nextLevelLearningRateText)
				{
					this._nextLevelLearningRateText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextLevelLearningRateText");
				}
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x000733D6 File Offset: 0x000715D6
		// (set) Token: 0x06001F39 RID: 7993 RVA: 0x000733DE File Offset: 0x000715DE
		[DataSourceProperty]
		public string NextLevelCostText
		{
			get
			{
				return this._nextLevelCostText;
			}
			set
			{
				if (value != this._nextLevelCostText)
				{
					this._nextLevelCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextLevelCostText");
				}
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x00073401 File Offset: 0x00071601
		// (set) Token: 0x06001F3B RID: 7995 RVA: 0x00073409 File Offset: 0x00071609
		[DataSourceProperty]
		public BasicTooltipViewModel ProgressHint
		{
			get
			{
				return this._progressHint;
			}
			set
			{
				if (value != this._progressHint)
				{
					this._progressHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProgressHint");
				}
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x00073427 File Offset: 0x00071627
		// (set) Token: 0x06001F3D RID: 7997 RVA: 0x0007342F File Offset: 0x0007162F
		[DataSourceProperty]
		public BasicTooltipViewModel SkillXPHint
		{
			get
			{
				return this._skillXPHint;
			}
			set
			{
				if (value != this._skillXPHint)
				{
					this._skillXPHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "SkillXPHint");
				}
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0007344D File Offset: 0x0007164D
		// (set) Token: 0x06001F3F RID: 7999 RVA: 0x00073455 File Offset: 0x00071655
		[DataSourceProperty]
		public HintViewModel AddFocusHint
		{
			get
			{
				return this._addFocusHint;
			}
			set
			{
				if (value != this._addFocusHint)
				{
					this._addFocusHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddFocusHint");
				}
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x00073473 File Offset: 0x00071673
		// (set) Token: 0x06001F41 RID: 8001 RVA: 0x0007347B File Offset: 0x0007167B
		[DataSourceProperty]
		public BasicTooltipViewModel LearningLimitTooltip
		{
			get
			{
				return this._learningLimitTooltip;
			}
			set
			{
				if (value != this._learningLimitTooltip)
				{
					this._learningLimitTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LearningLimitTooltip");
				}
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x00073499 File Offset: 0x00071699
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x000734A1 File Offset: 0x000716A1
		[DataSourceProperty]
		public BasicTooltipViewModel LearningRateTooltip
		{
			get
			{
				return this._learningRateTooltip;
			}
			set
			{
				if (value != this._learningRateTooltip)
				{
					this._learningRateTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "LearningRateTooltip");
				}
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x000734BF File Offset: 0x000716BF
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x000734C7 File Offset: 0x000716C7
		[DataSourceProperty]
		public double ProgressPercentage
		{
			get
			{
				return this._progressPercentage;
			}
			set
			{
				if (value != this._progressPercentage)
				{
					this._progressPercentage = value;
					base.OnPropertyChangedWithValue(value, "ProgressPercentage");
				}
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x000734E5 File Offset: 0x000716E5
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x000734ED File Offset: 0x000716ED
		[DataSourceProperty]
		public float LearningRate
		{
			get
			{
				return this._learningRate;
			}
			set
			{
				if (value != this._learningRate)
				{
					this._learningRate = value;
					base.OnPropertyChangedWithValue(value, "LearningRate");
				}
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x0007350B File Offset: 0x0007170B
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x00073513 File Offset: 0x00071713
		[DataSourceProperty]
		public int CurrentSkillXP
		{
			get
			{
				return this._currentSkillXP;
			}
			set
			{
				if (value != this._currentSkillXP)
				{
					this._currentSkillXP = value;
					base.OnPropertyChangedWithValue(value, "CurrentSkillXP");
				}
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x00073531 File Offset: 0x00071731
		// (set) Token: 0x06001F4B RID: 8011 RVA: 0x00073539 File Offset: 0x00071739
		[DataSourceProperty]
		public int NextLevel
		{
			get
			{
				return this._nextLevel;
			}
			set
			{
				if (value != this._nextLevel)
				{
					this._nextLevel = value;
					base.OnPropertyChangedWithValue(value, "NextLevel");
				}
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x00073557 File Offset: 0x00071757
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x0007355F File Offset: 0x0007175F
		[DataSourceProperty]
		public int FullLearningRateLevel
		{
			get
			{
				return this._fullLearningRateLevel;
			}
			set
			{
				if (value != this._fullLearningRateLevel)
				{
					this._fullLearningRateLevel = value;
					base.OnPropertyChangedWithValue(value, "FullLearningRateLevel");
				}
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x0007357D File Offset: 0x0007177D
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x00073585 File Offset: 0x00071785
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

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000735A3 File Offset: 0x000717A3
		// (set) Token: 0x06001F51 RID: 8017 RVA: 0x000735AB File Offset: 0x000717AB
		[DataSourceProperty]
		public int NumOfUnopenedPerks
		{
			get
			{
				return this._numOfUnopenedPerks;
			}
			set
			{
				if (value != this._numOfUnopenedPerks)
				{
					this._numOfUnopenedPerks = value;
					base.OnPropertyChangedWithValue(value, "NumOfUnopenedPerks");
				}
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000735C9 File Offset: 0x000717C9
		// (set) Token: 0x06001F53 RID: 8019 RVA: 0x000735D1 File Offset: 0x000717D1
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (value != this._progressText)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000735F4 File Offset: 0x000717F4
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x000735FC File Offset: 0x000717FC
		[DataSourceProperty]
		public string FocusCostText
		{
			get
			{
				return this._focusCostText;
			}
			set
			{
				if (value != this._focusCostText)
				{
					this._focusCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "FocusCostText");
				}
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x0007361F File Offset: 0x0007181F
		// (set) Token: 0x06001F57 RID: 8023 RVA: 0x00073627 File Offset: 0x00071827
		[DataSourceProperty]
		public MBBindingList<PerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<PerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x00073645 File Offset: 0x00071845
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x0007364D File Offset: 0x0007184D
		[DataSourceProperty]
		public MBBindingList<BindingListStringItem> SkillEffects
		{
			get
			{
				return this._skillEffects;
			}
			set
			{
				if (value != this._skillEffects)
				{
					this._skillEffects = value;
					base.OnPropertyChangedWithValue<MBBindingList<BindingListStringItem>>(value, "SkillEffects");
				}
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x0007366B File Offset: 0x0007186B
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x00073673 File Offset: 0x00071873
		[DataSourceProperty]
		public int MaxLevel
		{
			get
			{
				return this._maxLevel;
			}
			set
			{
				if (value != this._maxLevel)
				{
					this._maxLevel = value;
					base.OnPropertyChangedWithValue(value, "MaxLevel");
				}
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x00073691 File Offset: 0x00071891
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x00073699 File Offset: 0x00071899
		[DataSourceProperty]
		public string CurrentLearningRateText
		{
			get
			{
				return this._currentLearningRateText;
			}
			set
			{
				if (value != this._currentLearningRateText)
				{
					this._currentLearningRateText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentLearningRateText");
				}
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x000736BC File Offset: 0x000718BC
		// (set) Token: 0x06001F5F RID: 8031 RVA: 0x000736C4 File Offset: 0x000718C4
		[DataSourceProperty]
		public int CurrentFocusLevel
		{
			get
			{
				return this._currentFocusLevel;
			}
			set
			{
				if (value != this._currentFocusLevel)
				{
					this._currentFocusLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentFocusLevel");
				}
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x000736E2 File Offset: 0x000718E2
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x000736EA File Offset: 0x000718EA
		[DataSourceProperty]
		public string AddFocusText
		{
			get
			{
				return this._addFocusText;
			}
			set
			{
				if (value != this._addFocusText)
				{
					this._addFocusText = value;
					base.OnPropertyChangedWithValue<string>(value, "AddFocusText");
				}
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x0007370D File Offset: 0x0007190D
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x00073715 File Offset: 0x00071915
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (value != this._skillId)
				{
					this._skillId = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x00073738 File Offset: 0x00071938
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x00073740 File Offset: 0x00071940
		[DataSourceProperty]
		public bool IsInspected
		{
			get
			{
				return this._isInspected;
			}
			set
			{
				if (value != this._isInspected)
				{
					this._isInspected = value;
					base.OnPropertyChangedWithValue(value, "IsInspected");
				}
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x0007375E File Offset: 0x0007195E
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x00073766 File Offset: 0x00071966
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

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x00073789 File Offset: 0x00071989
		// (set) Token: 0x06001F69 RID: 8041 RVA: 0x00073791 File Offset: 0x00071991
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x04000E84 RID: 3716
		public const int MAX_SKILL_LEVEL = 300;

		// Token: 0x04000E85 RID: 3717
		public readonly SkillObject Skill;

		// Token: 0x04000E86 RID: 3718
		private readonly CharacterDeveloperHeroItemVM _heroItem;

		// Token: 0x04000E87 RID: 3719
		private readonly Concept _focusConceptObj;

		// Token: 0x04000E88 RID: 3720
		private readonly Concept _skillConceptObj;

		// Token: 0x04000E89 RID: 3721
		private readonly Action<PerkVM> _onStartPerkSelection;

		// Token: 0x04000E8A RID: 3722
		private int _orgFocusAmount;

		// Token: 0x04000E8B RID: 3723
		private MBBindingList<BindingListStringItem> _skillEffects;

		// Token: 0x04000E8C RID: 3724
		private MBBindingList<PerkVM> _perks;

		// Token: 0x04000E8D RID: 3725
		private BasicTooltipViewModel _progressHint;

		// Token: 0x04000E8E RID: 3726
		private HintViewModel _addFocusHint;

		// Token: 0x04000E8F RID: 3727
		private BasicTooltipViewModel _skillXPHint;

		// Token: 0x04000E90 RID: 3728
		private BasicTooltipViewModel _learningLimitTooltip;

		// Token: 0x04000E91 RID: 3729
		private BasicTooltipViewModel _learningRateTooltip;

		// Token: 0x04000E92 RID: 3730
		private string _nameText;

		// Token: 0x04000E93 RID: 3731
		private string _skillId;

		// Token: 0x04000E94 RID: 3732
		private string _addFocusText;

		// Token: 0x04000E95 RID: 3733
		private string _focusCostText;

		// Token: 0x04000E96 RID: 3734
		private string _currentLearningRateText;

		// Token: 0x04000E97 RID: 3735
		private string _nextLevelLearningRateText;

		// Token: 0x04000E98 RID: 3736
		private string _nextLevelCostText;

		// Token: 0x04000E99 RID: 3737
		private string _howToLearnText;

		// Token: 0x04000E9A RID: 3738
		private string _howToLearnTitle;

		// Token: 0x04000E9B RID: 3739
		private string _progressText;

		// Token: 0x04000E9C RID: 3740
		private string _descriptionText;

		// Token: 0x04000E9D RID: 3741
		private string _attributesText;

		// Token: 0x04000E9E RID: 3742
		private int _level = -1;

		// Token: 0x04000E9F RID: 3743
		private int _maxLevel;

		// Token: 0x04000EA0 RID: 3744
		private int _currentFocusLevel;

		// Token: 0x04000EA1 RID: 3745
		private int _currentSkillXP;

		// Token: 0x04000EA2 RID: 3746
		private int _xpRequiredForNextLevel;

		// Token: 0x04000EA3 RID: 3747
		private int _nextLevel;

		// Token: 0x04000EA4 RID: 3748
		private int _fullLearningRateLevel;

		// Token: 0x04000EA5 RID: 3749
		private int _numOfUnopenedPerks;

		// Token: 0x04000EA6 RID: 3750
		private bool _isInspected;

		// Token: 0x04000EA7 RID: 3751
		private bool _canAddFocus;

		// Token: 0x04000EA8 RID: 3752
		private bool _canLearnSkill;

		// Token: 0x04000EA9 RID: 3753
		private float _learningRate;

		// Token: 0x04000EAA RID: 3754
		private double _progressPercentage;

		// Token: 0x020002D3 RID: 723
		private enum SkillType
		{
			// Token: 0x040013BC RID: 5052
			Default,
			// Token: 0x040013BD RID: 5053
			Party,
			// Token: 0x040013BE RID: 5054
			Leader
		}
	}
}
