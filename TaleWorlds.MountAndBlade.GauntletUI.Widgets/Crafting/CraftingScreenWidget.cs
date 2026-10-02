using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016A RID: 362
	public class CraftingScreenWidget : Widget
	{
		// Token: 0x06001312 RID: 4882 RVA: 0x00034011 File Offset: 0x00032211
		public CraftingScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0003401C File Offset: 0x0003221C
		private void OnMainAction(Widget widget)
		{
			if (this.IsInCraftingMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/craft_success");
				return;
			}
			if (this.IsInRefinementMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/refine_success");
				return;
			}
			if (this.IsInSmeltingMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/smelt_success");
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00034082 File Offset: 0x00032282
		private void OnFinalAction(Widget widget)
		{
			if (this.NewCraftedWeaponPopupWidget != null && this.IsInCraftingMode)
			{
				bool isVisible = this.NewCraftedWeaponPopupWidget.IsVisible;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x000340A0 File Offset: 0x000322A0
		// (set) Token: 0x06001316 RID: 4886 RVA: 0x000340A8 File Offset: 0x000322A8
		[Editor(false)]
		public bool IsInCraftingMode
		{
			get
			{
				return this._isInCraftingMode;
			}
			set
			{
				if (this._isInCraftingMode != value)
				{
					this._isInCraftingMode = value;
					base.OnPropertyChanged(value, "IsInCraftingMode");
				}
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x000340C6 File Offset: 0x000322C6
		// (set) Token: 0x06001318 RID: 4888 RVA: 0x000340CE File Offset: 0x000322CE
		[Editor(false)]
		public bool IsInRefinementMode
		{
			get
			{
				return this._isInRefinementMode;
			}
			set
			{
				if (this._isInRefinementMode != value)
				{
					this._isInRefinementMode = value;
					base.OnPropertyChanged(value, "IsInRefinementMode");
				}
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x000340EC File Offset: 0x000322EC
		// (set) Token: 0x0600131A RID: 4890 RVA: 0x000340F4 File Offset: 0x000322F4
		[Editor(false)]
		public bool IsInSmeltingMode
		{
			get
			{
				return this._isInSmeltingMode;
			}
			set
			{
				if (this._isInSmeltingMode != value)
				{
					this._isInSmeltingMode = value;
					base.OnPropertyChanged(value, "IsInSmeltingMode");
				}
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x00034112 File Offset: 0x00032312
		// (set) Token: 0x0600131C RID: 4892 RVA: 0x0003411C File Offset: 0x0003231C
		[Editor(false)]
		public ButtonWidget MainActionButtonWidget
		{
			get
			{
				return this._mainActionButtonWidget;
			}
			set
			{
				if (this._mainActionButtonWidget != value)
				{
					this._mainActionButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "MainActionButtonWidget");
					if (!value.ClickEventHandlers.Contains(new Action<Widget>(this.OnMainAction)))
					{
						value.ClickEventHandlers.Add(new Action<Widget>(this.OnMainAction));
					}
				}
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00034175 File Offset: 0x00032375
		// (set) Token: 0x0600131E RID: 4894 RVA: 0x00034180 File Offset: 0x00032380
		[Editor(false)]
		public ButtonWidget FinalCraftButtonWidget
		{
			get
			{
				return this._mainActionButtonWidget;
			}
			set
			{
				if (this._finalCraftButtonWidget != value)
				{
					this._finalCraftButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FinalCraftButtonWidget");
					if (!value.ClickEventHandlers.Contains(new Action<Widget>(this.OnFinalAction)))
					{
						value.ClickEventHandlers.Add(new Action<Widget>(this.OnFinalAction));
					}
				}
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x000341D9 File Offset: 0x000323D9
		// (set) Token: 0x06001320 RID: 4896 RVA: 0x000341E1 File Offset: 0x000323E1
		[Editor(false)]
		public Widget NewCraftedWeaponPopupWidget
		{
			get
			{
				return this._newCraftedWeaponPopupWidget;
			}
			set
			{
				if (this._newCraftedWeaponPopupWidget != value)
				{
					this._newCraftedWeaponPopupWidget = value;
					base.OnPropertyChanged<Widget>(value, "NewCraftedWeaponPopupWidget");
				}
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x000341FF File Offset: 0x000323FF
		// (set) Token: 0x06001322 RID: 4898 RVA: 0x00034207 File Offset: 0x00032407
		[Editor(false)]
		public Widget CraftingOrderPopupWidget
		{
			get
			{
				return this._craftingOrdersPopupWidget;
			}
			set
			{
				if (this._craftingOrdersPopupWidget != value)
				{
					this._craftingOrdersPopupWidget = value;
					base.OnPropertyChanged<Widget>(value, "CraftingOrderPopupWidget");
				}
			}
		}

		// Token: 0x040008A6 RID: 2214
		private ButtonWidget _mainActionButtonWidget;

		// Token: 0x040008A7 RID: 2215
		private ButtonWidget _finalCraftButtonWidget;

		// Token: 0x040008A8 RID: 2216
		private bool _isInCraftingMode;

		// Token: 0x040008A9 RID: 2217
		private bool _isInRefinementMode;

		// Token: 0x040008AA RID: 2218
		private bool _isInSmeltingMode;

		// Token: 0x040008AB RID: 2219
		private Widget _newCraftedWeaponPopupWidget;

		// Token: 0x040008AC RID: 2220
		private Widget _craftingOrdersPopupWidget;
	}
}
