using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Options
{
	// Token: 0x02000189 RID: 393
	public class CharacterCreationOptionsItemWidget : Widget
	{
		// Token: 0x0600145F RID: 5215 RVA: 0x000376EC File Offset: 0x000358EC
		public CharacterCreationOptionsItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000376FC File Offset: 0x000358FC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isDirty)
			{
				if (this.Type == 0)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = true;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = false;
				}
				else if (this.Type == 1)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = true;
				}
				else if (this.Type == 2)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = true;
					this.NumericOptionWidget.IsVisible = false;
				}
				else if (this.Type == 3)
				{
					this.ActionOptionWidget.IsVisible = true;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = false;
				}
				this.ResetNavigationIndices();
				this._isDirty = false;
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00037814 File Offset: 0x00035A14
		private void ResetNavigationIndices()
		{
			if (base.GamepadNavigationIndex == -1)
			{
				return;
			}
			bool flag = false;
			Widget booleanOptionWidget = this.BooleanOptionWidget;
			if (booleanOptionWidget != null && booleanOptionWidget.IsVisible)
			{
				this.BooleanOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
				flag = true;
			}
			else
			{
				Widget numericOptionWidget = this.NumericOptionWidget;
				if (numericOptionWidget != null && numericOptionWidget.IsVisible)
				{
					this.NumericOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
					flag = true;
				}
				else
				{
					Widget selectionOptionWidget = this.SelectionOptionWidget;
					if (selectionOptionWidget != null && selectionOptionWidget.IsVisible)
					{
						this.SelectionOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
						flag = true;
					}
					else
					{
						Widget actionOptionWidget = this.ActionOptionWidget;
						if (actionOptionWidget != null && actionOptionWidget.IsVisible)
						{
							this.ActionOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				base.GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000378D9 File Offset: 0x00035AD9
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			base.OnGamepadNavigationIndexUpdated(newIndex);
			this.ResetNavigationIndices();
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x000378E8 File Offset: 0x00035AE8
		// (set) Token: 0x06001464 RID: 5220 RVA: 0x000378F0 File Offset: 0x00035AF0
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x00037915 File Offset: 0x00035B15
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x0003791D File Offset: 0x00035B1D
		[Editor(false)]
		public Widget ActionOptionWidget
		{
			get
			{
				return this._actionOptionWidget;
			}
			set
			{
				if (this._actionOptionWidget != value)
				{
					this._actionOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "ActionOptionWidget");
				}
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0003793B File Offset: 0x00035B3B
		// (set) Token: 0x06001468 RID: 5224 RVA: 0x00037943 File Offset: 0x00035B43
		[Editor(false)]
		public Widget NumericOptionWidget
		{
			get
			{
				return this._numericOptionWidget;
			}
			set
			{
				if (this._numericOptionWidget != value)
				{
					this._numericOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "NumericOptionWidget");
				}
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x00037961 File Offset: 0x00035B61
		// (set) Token: 0x0600146A RID: 5226 RVA: 0x00037969 File Offset: 0x00035B69
		[Editor(false)]
		public Widget SelectionOptionWidget
		{
			get
			{
				return this._selectionOptionWidget;
			}
			set
			{
				if (this._selectionOptionWidget != value)
				{
					this._selectionOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectionOptionWidget");
				}
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x00037987 File Offset: 0x00035B87
		// (set) Token: 0x0600146C RID: 5228 RVA: 0x0003798F File Offset: 0x00035B8F
		[Editor(false)]
		public Widget BooleanOptionWidget
		{
			get
			{
				return this._booleanOptionWidget;
			}
			set
			{
				if (this._booleanOptionWidget != value)
				{
					this._booleanOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "BooleanOptionWidget");
				}
			}
		}

		// Token: 0x0400093D RID: 2365
		private bool _isDirty = true;

		// Token: 0x0400093E RID: 2366
		private int _type;

		// Token: 0x0400093F RID: 2367
		private Widget _actionOptionWidget;

		// Token: 0x04000940 RID: 2368
		private Widget _numericOptionWidget;

		// Token: 0x04000941 RID: 2369
		private Widget _selectionOptionWidget;

		// Token: 0x04000942 RID: 2370
		private Widget _booleanOptionWidget;
	}
}
