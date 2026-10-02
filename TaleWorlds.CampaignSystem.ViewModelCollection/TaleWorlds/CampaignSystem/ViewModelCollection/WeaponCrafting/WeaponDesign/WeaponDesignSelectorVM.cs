using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010D RID: 269
	public class WeaponDesignSelectorVM : ViewModel
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x0005BAB7 File Offset: 0x00059CB7
		public WeaponDesign Design { get; }

		// Token: 0x0600180C RID: 6156 RVA: 0x0005BAC0 File Offset: 0x00059CC0
		public WeaponDesignSelectorVM(WeaponDesign design, Action<WeaponDesignSelectorVM> onSelection)
		{
			this._onSelection = onSelection;
			this.Design = design;
			TextObject textObject = new TextObject("{=uZhHh7pm}Crafted {CURR_TEMPLATE_NAME}", null);
			textObject.SetTextVariable("CURR_TEMPLATE_NAME", design.Template.TemplateName);
			TextObject textObject2 = design.WeaponName ?? textObject;
			this.Name = textObject2.ToString();
			Crafting.GenerateItem(design, textObject2, Hero.MainHero.Culture, design.Template.ItemModifierGroup, ref this._generatedVisualItem, design.HashedCode);
			MBObjectManager.Instance.RegisterObject<ItemObject>(this._generatedVisualItem);
			this.Visual = new ItemImageIdentifierVM(this._generatedVisualItem, "");
			this.WeaponTypeCode = design.Template.StringId;
			this.Hint = new BasicTooltipViewModel(() => this.GetHint());
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0005BB94 File Offset: 0x00059D94
		private List<TooltipProperty> GetHint()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", this._generatedVisualItem.Name.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (CraftingStatData craftingStatData in Crafting.GetStatDatasFromTemplate(0, this._generatedVisualItem, this.Design.Template))
			{
				if (craftingStatData.IsValid && craftingStatData.CurValue > 0f && craftingStatData.MaxValue > 0f)
				{
					list.Add(new TooltipProperty(craftingStatData.DescriptionText.ToString(), craftingStatData.CurValue.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0005BC64 File Offset: 0x00059E64
		public void ExecuteSelect()
		{
			Action<WeaponDesignSelectorVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0005BC77 File Offset: 0x00059E77
		public override void OnFinalize()
		{
			base.OnFinalize();
			MBObjectManager.Instance.UnregisterObject(this._generatedVisualItem);
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x0005BC8F File Offset: 0x00059E8F
		// (set) Token: 0x06001811 RID: 6161 RVA: 0x0005BC97 File Offset: 0x00059E97
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

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x0005BCB5 File Offset: 0x00059EB5
		// (set) Token: 0x06001813 RID: 6163 RVA: 0x0005BCBD File Offset: 0x00059EBD
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0005BCE0 File Offset: 0x00059EE0
		// (set) Token: 0x06001815 RID: 6165 RVA: 0x0005BCE8 File Offset: 0x00059EE8
		[DataSourceProperty]
		public string WeaponTypeCode
		{
			get
			{
				return this._weaponTypeCode;
			}
			set
			{
				if (value != this._weaponTypeCode)
				{
					this._weaponTypeCode = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponTypeCode");
				}
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0005BD0B File Offset: 0x00059F0B
		// (set) Token: 0x06001817 RID: 6167 RVA: 0x0005BD13 File Offset: 0x00059F13
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x0005BD31 File Offset: 0x00059F31
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x0005BD39 File Offset: 0x00059F39
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

		// Token: 0x04000B09 RID: 2825
		private readonly Action<WeaponDesignSelectorVM> _onSelection;

		// Token: 0x04000B0A RID: 2826
		private readonly ItemObject _generatedVisualItem;

		// Token: 0x04000B0B RID: 2827
		private bool _isSelected;

		// Token: 0x04000B0C RID: 2828
		private string _name;

		// Token: 0x04000B0D RID: 2829
		private string _weaponTypeCode;

		// Token: 0x04000B0E RID: 2830
		private ItemImageIdentifierVM _visual;

		// Token: 0x04000B0F RID: 2831
		private BasicTooltipViewModel _hint;
	}
}
