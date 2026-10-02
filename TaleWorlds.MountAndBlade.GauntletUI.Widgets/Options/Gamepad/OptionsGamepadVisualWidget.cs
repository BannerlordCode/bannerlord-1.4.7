using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007D RID: 125
	public class OptionsGamepadVisualWidget : Widget
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00013B37 File Offset: 0x00011D37
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00013B3F File Offset: 0x00011D3F
		public Widget ParentAreaWidget { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00013B48 File Offset: 0x00011D48
		private float _verticalMarginBetweenKeys
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00013B4F File Offset: 0x00011D4F
		public OptionsGamepadVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00013B70 File Offset: 0x00011D70
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				using (List<Widget>.Enumerator enumerator = base.ParentWidget.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						OptionsGamepadKeyLocationWidget optionsGamepadKeyLocationWidget;
						if ((optionsGamepadKeyLocationWidget = enumerator.Current as OptionsGamepadKeyLocationWidget) != null)
						{
							this._allKeyLocations.Add(optionsGamepadKeyLocationWidget);
						}
					}
				}
				this._initalized = true;
			}
			if (this._isKeysDirty)
			{
				this._allKeyLocations.ForEach(delegate(OptionsGamepadKeyLocationWidget k)
				{
					k.SetKeyProperties(string.Empty, this.ParentAreaWidget);
				});
				foreach (Widget widget in base.Children)
				{
					OptionsGamepadOptionItemListPanel optionItem;
					if ((optionItem = widget as OptionsGamepadOptionItemListPanel) != null)
					{
						OptionsGamepadKeyLocationWidget optionsGamepadKeyLocationWidget2 = this._allKeyLocations.Find((OptionsGamepadKeyLocationWidget l) => l.KeyID == optionItem.KeyId);
						if (optionsGamepadKeyLocationWidget2 != null)
						{
							optionItem.SetKeyProperties(optionsGamepadKeyLocationWidget2, this.ParentAreaWidget);
						}
						else
						{
							optionItem.IsVisible = false;
						}
					}
				}
				this._isKeysDirty = false;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00013CA8 File Offset: 0x00011EA8
		private void OnActionTextChanged()
		{
			this._isKeysDirty = true;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00013CB4 File Offset: 0x00011EB4
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this._isKeysDirty = true;
			OptionsGamepadOptionItemListPanel optionsGamepadOptionItemListPanel;
			if ((optionsGamepadOptionItemListPanel = child as OptionsGamepadOptionItemListPanel) != null && !this._allChildKeyItems.Contains(optionsGamepadOptionItemListPanel))
			{
				this._allChildKeyItems.Add(optionsGamepadOptionItemListPanel);
				optionsGamepadOptionItemListPanel.OnActionTextChanged += this.OnActionTextChanged;
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00013D08 File Offset: 0x00011F08
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			this._isKeysDirty = true;
			OptionsGamepadOptionItemListPanel optionsGamepadOptionItemListPanel;
			if ((optionsGamepadOptionItemListPanel = child as OptionsGamepadOptionItemListPanel) != null && this._allChildKeyItems.Contains(optionsGamepadOptionItemListPanel))
			{
				this._allChildKeyItems.Remove(optionsGamepadOptionItemListPanel);
				optionsGamepadOptionItemListPanel.OnActionTextChanged -= this.OnActionTextChanged;
			}
		}

		// Token: 0x040002F1 RID: 753
		private List<OptionsGamepadKeyLocationWidget> _allKeyLocations = new List<OptionsGamepadKeyLocationWidget>();

		// Token: 0x040002F2 RID: 754
		private List<OptionsGamepadOptionItemListPanel> _allChildKeyItems = new List<OptionsGamepadOptionItemListPanel>();

		// Token: 0x040002F4 RID: 756
		private bool _initalized;

		// Token: 0x040002F5 RID: 757
		private bool _isKeysDirty;
	}
}
