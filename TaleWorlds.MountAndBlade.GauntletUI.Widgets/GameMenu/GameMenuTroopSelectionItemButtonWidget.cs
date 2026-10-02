using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000152 RID: 338
	public class GameMenuTroopSelectionItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000318B5 File Offset: 0x0002FAB5
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x000318BD File Offset: 0x0002FABD
		public ButtonWidget AddButtonWidget { get; set; }

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x000318C6 File Offset: 0x0002FAC6
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x000318CE File Offset: 0x0002FACE
		public ButtonWidget RemoveButtonWidget { get; set; }

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000318D7 File Offset: 0x0002FAD7
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x000318DF File Offset: 0x0002FADF
		public Widget CheckmarkVisualWidget { get; set; }

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000318E8 File Offset: 0x0002FAE8
		// (set) Token: 0x060011E3 RID: 4579 RVA: 0x000318F0 File Offset: 0x0002FAF0
		public Widget AddRemoveControls { get; set; }

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000318F9 File Offset: 0x0002FAF9
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00031901 File Offset: 0x0002FB01
		public Widget HeroHealthParent { get; set; }

		// Token: 0x060011E6 RID: 4582 RVA: 0x0003190A File Offset: 0x0002FB0A
		public GameMenuTroopSelectionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0003191C File Offset: 0x0002FB1C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.AddButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnAdd));
				this.RemoveButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnRemove));
				this._initialized = true;
			}
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00031985 File Offset: 0x0002FB85
		private void OnRemove(Widget obj)
		{
			base.EventFired("Remove", Array.Empty<object>());
			this.Refresh();
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0003199D File Offset: 0x0002FB9D
		private void OnAdd(Widget obj)
		{
			base.EventFired("Add", Array.Empty<object>());
			this.Refresh();
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000319B5 File Offset: 0x0002FBB5
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.CurrentAmount == 0)
			{
				base.EventFired("Add", Array.Empty<object>());
			}
			else
			{
				base.EventFired("Remove", Array.Empty<object>());
			}
			this.Refresh();
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000319F0 File Offset: 0x0002FBF0
		private void Refresh()
		{
			if (this.CheckmarkVisualWidget == null || this.AddRemoveControls == null || this.AddButtonWidget == null || this.RemoveButtonWidget == null)
			{
				return;
			}
			if (this.MaxAmount == 0)
			{
				base.DoNotAcceptEvents = false;
				base.DoNotPassEventsToChildren = true;
				this.CheckmarkVisualWidget.IsHidden = this.CurrentAmount == 0;
				this.AddRemoveControls.IsHidden = true;
				this.AddButtonWidget.IsHidden = true;
				this.RemoveButtonWidget.IsHidden = true;
				base.IsDisabled = true;
				base.DominantSelectedState = this.IsLocked;
				this.HeroHealthParent.IsHidden = !this.IsTroopHero;
				if (this.IsLocked)
				{
					base.IsDisabled = this.CurrentAmount <= 0;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			else if (this.MaxAmount == 1)
			{
				base.DoNotAcceptEvents = false;
				base.DoNotPassEventsToChildren = true;
				this.CheckmarkVisualWidget.IsHidden = this.CurrentAmount == 0;
				this.AddRemoveControls.IsHidden = true;
				this.AddButtonWidget.IsHidden = true;
				this.RemoveButtonWidget.IsHidden = true;
				base.IsDisabled = (this.IsRosterFull && this.CurrentAmount <= 0) || this.IsLocked;
				base.DominantSelectedState = this.IsLocked;
				this.HeroHealthParent.IsHidden = !this.IsTroopHero;
				if (this.IsLocked)
				{
					base.IsDisabled = this.CurrentAmount <= 0;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			else
			{
				base.DoNotAcceptEvents = true;
				base.DoNotPassEventsToChildren = false;
				this.CheckmarkVisualWidget.IsHidden = true;
				this.AddRemoveControls.IsHidden = false;
				this.HeroHealthParent.IsHidden = true;
				this.AddButtonWidget.IsHidden = false;
				this.RemoveButtonWidget.IsHidden = false;
				this.AddButtonWidget.IsDisabled = this.IsRosterFull || this.CurrentAmount >= this.MaxAmount;
				this.RemoveButtonWidget.IsDisabled = this.CurrentAmount <= 0;
				if (this.IsLocked)
				{
					base.IsDisabled = false;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			base.GamepadNavigationIndex = (this.AddRemoveControls.IsVisible ? (-1) : 0);
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00031C44 File Offset: 0x0002FE44
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x00031C4C File Offset: 0x0002FE4C
		public bool IsRosterFull
		{
			get
			{
				return this._isRosterFull;
			}
			set
			{
				if (this._isRosterFull != value)
				{
					this._isRosterFull = value;
					base.OnPropertyChanged(value, "IsRosterFull");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x00031C71 File Offset: 0x0002FE71
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x00031C79 File Offset: 0x0002FE79
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (this._isLocked != value)
				{
					this._isLocked = value;
					base.OnPropertyChanged(value, "IsLocked");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00031C9E File Offset: 0x0002FE9E
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00031CA6 File Offset: 0x0002FEA6
		public bool IsTroopHero
		{
			get
			{
				return this._isTroopHero;
			}
			set
			{
				if (this._isTroopHero != value)
				{
					this._isTroopHero = value;
					base.OnPropertyChanged(value, "IsTroopHero");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00031CCB File Offset: 0x0002FECB
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00031CD3 File Offset: 0x0002FED3
		public int CurrentAmount
		{
			get
			{
				return this._currentAmount;
			}
			set
			{
				if (this._currentAmount != value)
				{
					this._currentAmount = value;
					base.OnPropertyChanged(value, "CurrentAmount");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00031CF8 File Offset: 0x0002FEF8
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00031D00 File Offset: 0x0002FF00
		public int MaxAmount
		{
			get
			{
				return this._maxAmount;
			}
			set
			{
				if (this._maxAmount != value)
				{
					this._maxAmount = value;
					base.OnPropertyChanged(value, "MaxAmount");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x04000828 RID: 2088
		private bool _initialized;

		// Token: 0x04000829 RID: 2089
		private bool _isDirty = true;

		// Token: 0x0400082A RID: 2090
		private int _maxAmount;

		// Token: 0x0400082B RID: 2091
		private int _currentAmount;

		// Token: 0x0400082C RID: 2092
		private bool _isRosterFull;

		// Token: 0x0400082D RID: 2093
		private bool _isLocked;

		// Token: 0x0400082E RID: 2094
		private bool _isTroopHero;
	}
}
