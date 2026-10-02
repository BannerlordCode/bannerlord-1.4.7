using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.Order
{
	// Token: 0x0200010F RID: 271
	public class CraftingOrderItemVM : ViewModel
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0005F13A File Offset: 0x0005D33A
		public CraftingOrder CraftingOrder { get; }

		// Token: 0x060018D5 RID: 6357 RVA: 0x0005F144 File Offset: 0x0005D344
		public CraftingOrderItemVM(CraftingOrder order, Action<CraftingOrderItemVM> onSelection, Func<CraftingAvailableHeroItemVM> getCurrentCraftingHero, List<CraftingStatData> orderStatDatas, CampaignUIHelper.IssueQuestFlags questFlags = CampaignUIHelper.IssueQuestFlags.None)
		{
			this.CraftingOrder = order;
			this._orderOwner = order.OrderOwner;
			this._getCurrentCraftingHero = getCurrentCraftingHero;
			this._orderStatDatas = orderStatDatas;
			this._onSelection = onSelection;
			this.WeaponAttributes = new MBBindingList<WeaponAttributeVM>();
			this.OrderOwnerData = new HeroVM(this._orderOwner, false);
			this._weaponTemplate = order.PreCraftedWeaponDesignItem.WeaponDesign.Template;
			this.OrderWeaponTypeCode = this._weaponTemplate.StringId;
			this.Quests = this.GetQuestMarkers(questFlags);
			this.IsQuestOrder = this.Quests.Count > 0;
			this.RefreshValues();
			this.RefreshStats();
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0005F204 File Offset: 0x0005D404
		private MBBindingList<QuestMarkerVM> GetQuestMarkers(CampaignUIHelper.IssueQuestFlags flags)
		{
			MBBindingList<QuestMarkerVM> mbbindingList = new MBBindingList<QuestMarkerVM>();
			if ((flags & CampaignUIHelper.IssueQuestFlags.ActiveIssue) != CampaignUIHelper.IssueQuestFlags.None)
			{
				mbbindingList.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveIssue, null, null));
			}
			if ((flags & CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest) != CampaignUIHelper.IssueQuestFlags.None)
			{
				mbbindingList.Add(new QuestMarkerVM(CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest, null, null));
			}
			return mbbindingList;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0005F240 File Offset: 0x0005D440
		public void RefreshStats()
		{
			this.WeaponAttributes.Clear();
			ItemObject preCraftedWeaponDesignItem = this.CraftingOrder.PreCraftedWeaponDesignItem;
			if (((preCraftedWeaponDesignItem != null) ? preCraftedWeaponDesignItem.Weapons : null) == null)
			{
				Debug.FailedAssert("Crafting order does not contain any valid weapons", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Crafting\\WeaponDesign\\Order\\CraftingOrderItemVM.cs", "RefreshStats", 71);
				return;
			}
			this.CraftingOrder.GetStatWeapon();
			foreach (CraftingStatData craftingStatData in this._orderStatDatas)
			{
				if (craftingStatData.IsValid)
				{
					this.WeaponAttributes.Add(new WeaponAttributeVM(craftingStatData.Type, craftingStatData.DamageType, craftingStatData.DescriptionText.ToString(), craftingStatData.CurValue));
				}
			}
			IEnumerable<Hero> enumerable = from x in CraftingHelper.GetAvailableHeroesForCrafting()
				where this.CraftingOrder.IsOrderAvailableForHero(x)
				select x;
			this.HasAvailableHeroes = enumerable.Any<Hero>();
			this.OrderPrice = this.CraftingOrder.BaseGoldReward;
			this.RefreshDifficulty();
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0005F344 File Offset: 0x0005D544
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OrderNumberText = GameTexts.FindText("str_crafting_order_header", null).ToString();
			this.OrderWeaponType = this._weaponTemplate.TemplateName.ToString();
			this.OrderDifficultyLabelText = this._difficultyText.ToString();
			this.OrderDifficultyValueText = MathF.Round(this.CraftingOrder.OrderDifficulty).ToString();
			this.DisabledReasonHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetCraftingOrderDisabledReasonTooltip(this._getCurrentCraftingHero().Hero, this.CraftingOrder));
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0005F3CC File Offset: 0x0005D5CC
		private void RefreshDifficulty()
		{
			Hero hero = this._getCurrentCraftingHero().Hero;
			int skillValue = hero.GetSkillValue(DefaultSkills.Crafting);
			this.IsEnabled = this.CraftingOrder.IsOrderAvailableForHero(hero);
			this.IsDifficultySuitableForHero = this.CraftingOrder.OrderDifficulty < (float)skillValue;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0005F41D File Offset: 0x0005D61D
		public void ExecuteSelectOrder()
		{
			Action<CraftingOrderItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x0005F430 File Offset: 0x0005D630
		// (set) Token: 0x060018DC RID: 6364 RVA: 0x0005F438 File Offset: 0x0005D638
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x0005F456 File Offset: 0x0005D656
		// (set) Token: 0x060018DE RID: 6366 RVA: 0x0005F45E File Offset: 0x0005D65E
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

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x0005F47C File Offset: 0x0005D67C
		// (set) Token: 0x060018E0 RID: 6368 RVA: 0x0005F484 File Offset: 0x0005D684
		[DataSourceProperty]
		public bool HasAvailableHeroes
		{
			get
			{
				return this._hasAvailableHeroes;
			}
			set
			{
				if (value != this._hasAvailableHeroes)
				{
					this._hasAvailableHeroes = value;
					base.OnPropertyChangedWithValue(value, "HasAvailableHeroes");
				}
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x0005F4A2 File Offset: 0x0005D6A2
		// (set) Token: 0x060018E2 RID: 6370 RVA: 0x0005F4AA File Offset: 0x0005D6AA
		[DataSourceProperty]
		public bool IsDifficultySuitableForHero
		{
			get
			{
				return this._isDifficultySuitableForHero;
			}
			set
			{
				if (value != this._isDifficultySuitableForHero)
				{
					this._isDifficultySuitableForHero = value;
					base.OnPropertyChangedWithValue(value, "IsDifficultySuitableForHero");
				}
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0005F4C8 File Offset: 0x0005D6C8
		// (set) Token: 0x060018E4 RID: 6372 RVA: 0x0005F4D0 File Offset: 0x0005D6D0
		[DataSourceProperty]
		public bool IsQuestOrder
		{
			get
			{
				return this._isQuestOrder;
			}
			set
			{
				if (value != this._isQuestOrder)
				{
					this._isQuestOrder = value;
					base.OnPropertyChangedWithValue(value, "IsQuestOrder");
				}
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0005F4EE File Offset: 0x0005D6EE
		// (set) Token: 0x060018E6 RID: 6374 RVA: 0x0005F4F6 File Offset: 0x0005D6F6
		[DataSourceProperty]
		public int OrderPrice
		{
			get
			{
				return this._orderPrice;
			}
			set
			{
				if (value != this._orderPrice)
				{
					this._orderPrice = value;
					base.OnPropertyChangedWithValue(value, "OrderPrice");
				}
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0005F514 File Offset: 0x0005D714
		// (set) Token: 0x060018E8 RID: 6376 RVA: 0x0005F51C File Offset: 0x0005D71C
		[DataSourceProperty]
		public string OrderDifficultyLabelText
		{
			get
			{
				return this._orderDifficultyLabelText;
			}
			set
			{
				if (value != this._orderDifficultyLabelText)
				{
					this._orderDifficultyLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderDifficultyLabelText");
				}
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x0005F53F File Offset: 0x0005D73F
		// (set) Token: 0x060018EA RID: 6378 RVA: 0x0005F547 File Offset: 0x0005D747
		[DataSourceProperty]
		public string OrderDifficultyValueText
		{
			get
			{
				return this._orderDifficultyValueText;
			}
			set
			{
				if (value != this._orderDifficultyValueText)
				{
					this._orderDifficultyValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderDifficultyValueText");
				}
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x0005F56A File Offset: 0x0005D76A
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x0005F572 File Offset: 0x0005D772
		[DataSourceProperty]
		public string OrderNumberText
		{
			get
			{
				return this._orderNumberText;
			}
			set
			{
				if (value != this._orderNumberText)
				{
					this._orderNumberText = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderNumberText");
				}
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x0005F595 File Offset: 0x0005D795
		// (set) Token: 0x060018EE RID: 6382 RVA: 0x0005F59D File Offset: 0x0005D79D
		[DataSourceProperty]
		public string OrderWeaponType
		{
			get
			{
				return this._orderWeaponType;
			}
			set
			{
				if (value != this._orderWeaponType)
				{
					this._orderWeaponType = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderWeaponType");
				}
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x0005F5C0 File Offset: 0x0005D7C0
		// (set) Token: 0x060018F0 RID: 6384 RVA: 0x0005F5C8 File Offset: 0x0005D7C8
		[DataSourceProperty]
		public string OrderWeaponTypeCode
		{
			get
			{
				return this._orderWeaponTypeCode;
			}
			set
			{
				if (value != this._orderWeaponTypeCode)
				{
					this._orderWeaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderWeaponTypeCode");
				}
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x0005F5EB File Offset: 0x0005D7EB
		// (set) Token: 0x060018F2 RID: 6386 RVA: 0x0005F5F3 File Offset: 0x0005D7F3
		[DataSourceProperty]
		public HeroVM OrderOwnerData
		{
			get
			{
				return this._orderOwnerData;
			}
			set
			{
				if (value != this._orderOwnerData)
				{
					this._orderOwnerData = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "OrderOwnerData");
				}
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x0005F611 File Offset: 0x0005D811
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x0005F619 File Offset: 0x0005D819
		[DataSourceProperty]
		public BasicTooltipViewModel DisabledReasonHint
		{
			get
			{
				return this._disabledReasonHint;
			}
			set
			{
				if (value != this._disabledReasonHint)
				{
					this._disabledReasonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "DisabledReasonHint");
				}
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x0005F637 File Offset: 0x0005D837
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x0005F63F File Offset: 0x0005D83F
		[DataSourceProperty]
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x0005F65D File Offset: 0x0005D85D
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x0005F665 File Offset: 0x0005D865
		[DataSourceProperty]
		public MBBindingList<WeaponAttributeVM> WeaponAttributes
		{
			get
			{
				return this._weaponAttributes;
			}
			set
			{
				if (value != this._weaponAttributes)
				{
					this._weaponAttributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponAttributeVM>>(value, "WeaponAttributes");
				}
			}
		}

		// Token: 0x04000B63 RID: 2915
		private Hero _orderOwner;

		// Token: 0x04000B64 RID: 2916
		private Action<CraftingOrderItemVM> _onSelection;

		// Token: 0x04000B65 RID: 2917
		private Func<CraftingAvailableHeroItemVM> _getCurrentCraftingHero;

		// Token: 0x04000B66 RID: 2918
		private CraftingTemplate _weaponTemplate;

		// Token: 0x04000B67 RID: 2919
		private TextObject _difficultyText = new TextObject("{=udPWHmOm}Difficulty:", null);

		// Token: 0x04000B68 RID: 2920
		private List<CraftingStatData> _orderStatDatas;

		// Token: 0x04000B69 RID: 2921
		private bool _isEnabled;

		// Token: 0x04000B6A RID: 2922
		private bool _isSelected;

		// Token: 0x04000B6B RID: 2923
		private bool _hasAvailableHeroes;

		// Token: 0x04000B6C RID: 2924
		private bool _isDifficultySuitableForHero;

		// Token: 0x04000B6D RID: 2925
		private bool _isQuestOrder;

		// Token: 0x04000B6E RID: 2926
		private int _orderPrice;

		// Token: 0x04000B6F RID: 2927
		private string _orderDifficultyLabelText;

		// Token: 0x04000B70 RID: 2928
		private string _orderDifficultyValueText;

		// Token: 0x04000B71 RID: 2929
		private string _orderNumberText;

		// Token: 0x04000B72 RID: 2930
		private string _orderWeaponType;

		// Token: 0x04000B73 RID: 2931
		private string _orderWeaponTypeCode;

		// Token: 0x04000B74 RID: 2932
		private HeroVM _orderOwnerData;

		// Token: 0x04000B75 RID: 2933
		private BasicTooltipViewModel _disabledReasonHint;

		// Token: 0x04000B76 RID: 2934
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000B77 RID: 2935
		private MBBindingList<WeaponAttributeVM> _weaponAttributes;
	}
}
