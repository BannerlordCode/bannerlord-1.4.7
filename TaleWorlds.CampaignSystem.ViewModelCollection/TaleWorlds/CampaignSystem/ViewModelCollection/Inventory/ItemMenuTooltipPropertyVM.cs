using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000090 RID: 144
	public class ItemMenuTooltipPropertyVM : TooltipProperty
	{
		// Token: 0x06000C74 RID: 3188 RVA: 0x00032D92 File Offset: 0x00030F92
		public ItemMenuTooltipPropertyVM()
		{
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00032D9A File Offset: 0x00030F9A
		public ItemMenuTooltipPropertyVM(string definition, string value, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null, string modifierBonusText = null, bool isModifierBeneficial = false)
			: base(definition, value, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
			this.ModifierBonusText = modifierBonusText;
			this.HasModifierBonus = !string.IsNullOrEmpty(modifierBonusText);
			this.IsModifierBeneficial = isModifierBeneficial;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00032DD0 File Offset: 0x00030FD0
		public ItemMenuTooltipPropertyVM(string definition, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(definition, _valueFunc, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00032DE6 File Offset: 0x00030FE6
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00032DFC File Offset: 0x00030FFC
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, object[] valueArgs, int textHeight, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, valueArgs, textHeight, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00032E14 File Offset: 0x00031014
		public ItemMenuTooltipPropertyVM(string definition, string value, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null, TooltipProperty.TooltipPropertyFlags propertyFlags = TooltipProperty.TooltipPropertyFlags.None, string modifierBonusText = null, bool isModifierBeneficial = false)
			: base(definition, value, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
			this.ModifierBonusText = modifierBonusText;
			this.HasModifierBonus = !string.IsNullOrEmpty(modifierBonusText);
			this.IsModifierBeneficial = isModifierBeneficial;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00032E4C File Offset: 0x0003104C
		public ItemMenuTooltipPropertyVM(string definition, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(definition, _valueFunc, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00032E64 File Offset: 0x00031064
		public ItemMenuTooltipPropertyVM(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, HintViewModel propertyHint = null)
			: base(_definitionFunc, _valueFunc, textHeight, color, onlyShowWhenExtended, TooltipProperty.TooltipPropertyFlags.None)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00032E7C File Offset: 0x0003107C
		public ItemMenuTooltipPropertyVM(TooltipProperty property, HintViewModel propertyHint = null)
			: base(property)
		{
			this.PropertyHint = propertyHint;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00032E8C File Offset: 0x0003108C
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x00032E94 File Offset: 0x00031094
		[DataSourceProperty]
		public HintViewModel PropertyHint
		{
			get
			{
				return this._propertyHint;
			}
			set
			{
				if (value != this._propertyHint)
				{
					this._propertyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PropertyHint");
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00032EB2 File Offset: 0x000310B2
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00032EBA File Offset: 0x000310BA
		[DataSourceProperty]
		public bool HasModifierBonus
		{
			get
			{
				return this._hasModifierBonus;
			}
			set
			{
				if (value != this._hasModifierBonus)
				{
					this._hasModifierBonus = value;
					base.OnPropertyChangedWithValue(value, "HasModifierBonus");
				}
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00032ED8 File Offset: 0x000310D8
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x00032EE0 File Offset: 0x000310E0
		[DataSourceProperty]
		public bool IsModifierBeneficial
		{
			get
			{
				return this._isModifierBeneficial;
			}
			set
			{
				if (value != this._isModifierBeneficial)
				{
					this._isModifierBeneficial = value;
					base.OnPropertyChangedWithValue(value, "IsModifierBeneficial");
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00032EFE File Offset: 0x000310FE
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x00032F06 File Offset: 0x00031106
		[DataSourceProperty]
		public string ModifierBonusText
		{
			get
			{
				return this._modifierBonusText;
			}
			set
			{
				if (value != this._modifierBonusText)
				{
					this._modifierBonusText = value;
					base.OnPropertyChangedWithValue<string>(value, "ModifierBonusText");
				}
			}
		}

		// Token: 0x04000594 RID: 1428
		private HintViewModel _propertyHint;

		// Token: 0x04000595 RID: 1429
		private bool _hasModifierBonus;

		// Token: 0x04000596 RID: 1430
		private bool _isModifierBeneficial;

		// Token: 0x04000597 RID: 1431
		private string _modifierBonusText;
	}
}
