using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000067 RID: 103
	public class PartyManageTroopPopupWidget : Widget
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001066D File Offset: 0x0000E86D
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00010675 File Offset: 0x0000E875
		public Widget PrimaryInputKeyVisualParent { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001067E File Offset: 0x0000E87E
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00010686 File Offset: 0x0000E886
		public Widget SecondaryInputKeyVisualParent { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001068F File Offset: 0x0000E88F
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00010697 File Offset: 0x0000E897
		public Widget TertiaryInputKeyVisualParent { get; set; }

		// Token: 0x06000579 RID: 1401 RVA: 0x000106A0 File Offset: 0x0000E8A0
		public PartyManageTroopPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000106AC File Offset: 0x0000E8AC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible)
			{
				Widget hoveredWidget = base.EventManager.HoveredWidget;
				if (hoveredWidget != null && this.PrimaryInputKeyVisualParent != null && this.SecondaryInputKeyVisualParent != null && this.TertiaryInputKeyVisualParent != null)
				{
					PartyTroopManagementItemButtonWidget firstParentTupleOfWidget = this.GetFirstParentTupleOfWidget(hoveredWidget);
					if (firstParentTupleOfWidget != null)
					{
						Widget actionButtonAtIndex = firstParentTupleOfWidget.GetActionButtonAtIndex(0);
						if (this.IsPrimaryActionAvailable && actionButtonAtIndex != null)
						{
							this.PrimaryInputKeyVisualParent.IsVisible = true;
							this.PrimaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex.GlobalPosition.X;
							this.PrimaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex.GlobalPosition.Y - 10f;
						}
						else
						{
							this.PrimaryInputKeyVisualParent.IsVisible = false;
						}
						Widget actionButtonAtIndex2 = firstParentTupleOfWidget.GetActionButtonAtIndex(1);
						if (this.IsSecondaryActionAvailable && actionButtonAtIndex2 != null)
						{
							this.SecondaryInputKeyVisualParent.IsVisible = true;
							this.SecondaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex2.GlobalPosition.X;
							this.SecondaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex2.GlobalPosition.Y - 10f;
						}
						else
						{
							this.SecondaryInputKeyVisualParent.IsVisible = false;
						}
						Widget actionButtonAtIndex3 = firstParentTupleOfWidget.GetActionButtonAtIndex(2);
						if (this.IsTertiaryActionAvailable && actionButtonAtIndex3 != null)
						{
							this.TertiaryInputKeyVisualParent.IsVisible = true;
							this.TertiaryInputKeyVisualParent.ScaledPositionXOffset = actionButtonAtIndex3.GlobalPosition.X;
							this.TertiaryInputKeyVisualParent.ScaledPositionYOffset = actionButtonAtIndex3.GlobalPosition.Y - 10f;
							return;
						}
						this.TertiaryInputKeyVisualParent.IsVisible = false;
						return;
					}
					else
					{
						this.PrimaryInputKeyVisualParent.IsVisible = false;
						this.SecondaryInputKeyVisualParent.IsVisible = false;
						this.TertiaryInputKeyVisualParent.IsVisible = false;
					}
				}
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00010854 File Offset: 0x0000EA54
		private PartyTroopManagementItemButtonWidget GetFirstParentTupleOfWidget(Widget widget)
		{
			for (Widget widget2 = widget; widget2 != null; widget2 = widget2.ParentWidget)
			{
				PartyTroopManagementItemButtonWidget partyTroopManagementItemButtonWidget;
				if ((partyTroopManagementItemButtonWidget = widget2 as PartyTroopManagementItemButtonWidget) != null)
				{
					return partyTroopManagementItemButtonWidget;
				}
			}
			return null;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001087C File Offset: 0x0000EA7C
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x00010884 File Offset: 0x0000EA84
		public bool IsPrimaryActionAvailable
		{
			get
			{
				return this._isPrimaryActionAvailable;
			}
			set
			{
				if (value != this._isPrimaryActionAvailable)
				{
					this._isPrimaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsPrimaryActionAvailable");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x000108A2 File Offset: 0x0000EAA2
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x000108AA File Offset: 0x0000EAAA
		public bool IsSecondaryActionAvailable
		{
			get
			{
				return this._isSecondaryActionAvailable;
			}
			set
			{
				if (value != this._isSecondaryActionAvailable)
				{
					this._isSecondaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsSecondaryActionAvailable");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000108C8 File Offset: 0x0000EAC8
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x000108D0 File Offset: 0x0000EAD0
		public bool IsTertiaryActionAvailable
		{
			get
			{
				return this._isTertiaryActionAvailable;
			}
			set
			{
				if (value != this._isTertiaryActionAvailable)
				{
					this._isTertiaryActionAvailable = value;
					base.OnPropertyChanged(value, "IsTertiaryActionAvailable");
				}
			}
		}

		// Token: 0x0400025B RID: 603
		private bool _isPrimaryActionAvailable;

		// Token: 0x0400025C RID: 604
		private bool _isSecondaryActionAvailable;

		// Token: 0x0400025D RID: 605
		private bool _isTertiaryActionAvailable;
	}
}
