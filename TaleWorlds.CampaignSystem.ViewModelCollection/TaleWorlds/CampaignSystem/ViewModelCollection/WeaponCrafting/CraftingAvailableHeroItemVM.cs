using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000F8 RID: 248
	public class CraftingAvailableHeroItemVM : ViewModel
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x00057A71 File Offset: 0x00055C71
		public Hero Hero { get; }

		// Token: 0x0600166E RID: 5742 RVA: 0x00057A7C File Offset: 0x00055C7C
		public CraftingAvailableHeroItemVM(Hero hero, Action<CraftingAvailableHeroItemVM> onSelection)
		{
			this._onSelection = onSelection;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this.Hero = hero;
			this.HeroData = new HeroVM(this.Hero, false);
			this.Hint = new BasicTooltipViewModel(() => CampaignUIHelper.GetCraftingHeroTooltip(this.Hero, this._craftingOrder));
			this.CraftingPerks = new MBBindingList<CraftingPerkVM>();
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00057AE1 File Offset: 0x00055CE1
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HeroData.RefreshValues();
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00057AF4 File Offset: 0x00055CF4
		public void RefreshStamina()
		{
			this.CurrentStamina = (float)this._craftingBehavior.GetHeroCraftingStamina(this.Hero);
			this.MaxStamina = this._craftingBehavior.GetMaxHeroCraftingStamina(this.Hero);
			int num = (int)(this.CurrentStamina / (float)this.MaxStamina * 100f);
			GameTexts.SetVariable("NUMBER", num);
			this.StaminaPercentage = GameTexts.FindText("str_NUMBER_percent", null).ToString();
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00057B67 File Offset: 0x00055D67
		public void RefreshOrderAvailability(CraftingOrder order)
		{
			this._craftingOrder = order;
			if (order != null)
			{
				this.IsDisabled = !order.IsOrderAvailableForHero(this.Hero);
				return;
			}
			this.IsDisabled = false;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00057B90 File Offset: 0x00055D90
		public void RefreshSkills()
		{
			this.SmithySkillLevel = this.Hero.GetSkillValue(DefaultSkills.Crafting);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00057BA8 File Offset: 0x00055DA8
		public void RefreshPerks()
		{
			this.CraftingPerks.Clear();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.Skill == DefaultSkills.Crafting && this.Hero.GetPerkValue(perkObject))
				{
					this.CraftingPerks.Add(new CraftingPerkVM(perkObject));
				}
			}
			this.PerksText = ((this.CraftingPerks.Count > 0) ? new TextObject("{=8lCWWK9G}Smithing Perks", null).ToString() : new TextObject("{=WHRq5Dp0}No Smithing Perks", null).ToString());
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00057C60 File Offset: 0x00055E60
		public void ExecuteSelection()
		{
			Action<CraftingAvailableHeroItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x00057C73 File Offset: 0x00055E73
		// (set) Token: 0x06001676 RID: 5750 RVA: 0x00057C7B File Offset: 0x00055E7B
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x00057C99 File Offset: 0x00055E99
		// (set) Token: 0x06001678 RID: 5752 RVA: 0x00057CA1 File Offset: 0x00055EA1
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x00057CBF File Offset: 0x00055EBF
		// (set) Token: 0x0600167A RID: 5754 RVA: 0x00057CC7 File Offset: 0x00055EC7
		[DataSourceProperty]
		public HeroVM HeroData
		{
			get
			{
				return this._heroData;
			}
			set
			{
				if (value != this._heroData)
				{
					this._heroData = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "HeroData");
				}
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x00057CE5 File Offset: 0x00055EE5
		// (set) Token: 0x0600167C RID: 5756 RVA: 0x00057CED File Offset: 0x00055EED
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600167D RID: 5757 RVA: 0x00057D0B File Offset: 0x00055F0B
		// (set) Token: 0x0600167E RID: 5758 RVA: 0x00057D13 File Offset: 0x00055F13
		[DataSourceProperty]
		public float CurrentStamina
		{
			get
			{
				return this._currentStamina;
			}
			set
			{
				if (value != this._currentStamina)
				{
					this._currentStamina = value;
					base.OnPropertyChangedWithValue(value, "CurrentStamina");
				}
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x00057D31 File Offset: 0x00055F31
		// (set) Token: 0x06001680 RID: 5760 RVA: 0x00057D39 File Offset: 0x00055F39
		[DataSourceProperty]
		public int MaxStamina
		{
			get
			{
				return this._maxStamina;
			}
			set
			{
				if (value != this._maxStamina)
				{
					this._maxStamina = value;
					base.OnPropertyChangedWithValue(value, "MaxStamina");
				}
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x00057D57 File Offset: 0x00055F57
		// (set) Token: 0x06001682 RID: 5762 RVA: 0x00057D5F File Offset: 0x00055F5F
		[DataSourceProperty]
		public string StaminaPercentage
		{
			get
			{
				return this._staminaPercentage;
			}
			set
			{
				if (value != this._staminaPercentage)
				{
					this._staminaPercentage = value;
					base.OnPropertyChangedWithValue<string>(value, "StaminaPercentage");
				}
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00057D82 File Offset: 0x00055F82
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x00057D8A File Offset: 0x00055F8A
		[DataSourceProperty]
		public int SmithySkillLevel
		{
			get
			{
				return this._smithySkillLevel;
			}
			set
			{
				if (value != this._smithySkillLevel)
				{
					this._smithySkillLevel = value;
					base.OnPropertyChangedWithValue(value, "SmithySkillLevel");
				}
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00057DA8 File Offset: 0x00055FA8
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x00057DB0 File Offset: 0x00055FB0
		[DataSourceProperty]
		public MBBindingList<CraftingPerkVM> CraftingPerks
		{
			get
			{
				return this._craftingPerks;
			}
			set
			{
				if (value != this._craftingPerks)
				{
					this._craftingPerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingPerkVM>>(value, "CraftingPerks");
				}
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00057DCE File Offset: 0x00055FCE
		// (set) Token: 0x06001688 RID: 5768 RVA: 0x00057DD6 File Offset: 0x00055FD6
		[DataSourceProperty]
		public string PerksText
		{
			get
			{
				return this._perksText;
			}
			set
			{
				if (value != this._perksText)
				{
					this._perksText = value;
					base.OnPropertyChangedWithValue<string>(value, "PerksText");
				}
			}
		}

		// Token: 0x04000A41 RID: 2625
		private readonly Action<CraftingAvailableHeroItemVM> _onSelection;

		// Token: 0x04000A42 RID: 2626
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000A43 RID: 2627
		private CraftingOrder _craftingOrder;

		// Token: 0x04000A44 RID: 2628
		private HeroVM _heroData;

		// Token: 0x04000A45 RID: 2629
		private BasicTooltipViewModel _hint;

		// Token: 0x04000A46 RID: 2630
		private float _currentStamina;

		// Token: 0x04000A47 RID: 2631
		private int _maxStamina;

		// Token: 0x04000A48 RID: 2632
		private string _staminaPercentage;

		// Token: 0x04000A49 RID: 2633
		private bool _isDisabled;

		// Token: 0x04000A4A RID: 2634
		private bool _isSelected;

		// Token: 0x04000A4B RID: 2635
		private int _smithySkillLevel;

		// Token: 0x04000A4C RID: 2636
		private MBBindingList<CraftingPerkVM> _craftingPerks;

		// Token: 0x04000A4D RID: 2637
		private string _perksText;
	}
}
