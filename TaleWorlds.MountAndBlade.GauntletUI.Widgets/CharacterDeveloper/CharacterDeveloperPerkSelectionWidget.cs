using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000180 RID: 384
	public class CharacterDeveloperPerkSelectionWidget : Widget
	{
		// Token: 0x060013EE RID: 5102 RVA: 0x00036534 File Offset: 0x00034734
		public CharacterDeveloperPerkSelectionWidget(UIContext context)
			: base(context)
		{
			base.IsVisible = false;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00036550 File Offset: 0x00034750
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsVisible && this._latestMouseUpWidgetWhenActivated != base.EventManager.LatestMouseUpWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget))
			{
				this.Deactivate();
			}
			this.UpdatePosition();
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000365A0 File Offset: 0x000347A0
		private void UpdatePosition()
		{
			if (base.IsVisible && this._latestMouseUpWidgetWhenActivated != null)
			{
				float num = this._latestMouseUpWidgetWhenActivated.GlobalPosition.X + this._latestMouseUpWidgetWhenActivated.Size.X + this._distBetweenPerkItemsMultiplier * 2f * base._scaleToUse;
				float num2 = 0f;
				if (base.GetChild(0).ChildCount > 1)
				{
					PerkItemButtonWidget perkItemButtonWidget;
					if ((perkItemButtonWidget = this._latestMouseUpWidgetWhenActivated as PerkItemButtonWidget) != null)
					{
						if (perkItemButtonWidget.AlternativeType == 1)
						{
							num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y + (this._latestMouseUpWidgetWhenActivated.Size.Y - 4f * base._scaleToUse) - base.Size.Y / 2f;
						}
						else if (perkItemButtonWidget.AlternativeType == 2)
						{
							num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y - base.Size.Y / 2f;
						}
					}
				}
				else
				{
					num2 = this._latestMouseUpWidgetWhenActivated.GlobalPosition.Y + this._latestMouseUpWidgetWhenActivated.Size.Y / 2f - base.Size.Y / 2f;
				}
				base.ScaledPositionXOffset = MathF.Clamp(num, 0f, base.EventManager.PageSize.X - base.Size.X);
				base.ScaledPositionYOffset = MathF.Clamp(num2, 0f, base.EventManager.PageSize.Y - base.Size.Y);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00036731 File Offset: 0x00034931
		private void Activate()
		{
			if (this._latestMouseUpWidgetWhenActivated == null)
			{
				this._latestMouseUpWidgetWhenActivated = base.EventManager.LatestMouseDownWidget;
			}
			base.IsVisible = true;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00036753 File Offset: 0x00034953
		private void Deactivate()
		{
			base.EventFired("Deactivate", Array.Empty<object>());
			base.IsVisible = false;
			this._latestMouseUpWidgetWhenActivated = null;
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x00036773 File Offset: 0x00034973
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x0003677B File Offset: 0x0003497B
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					if (this._isActive)
					{
						this.Activate();
						return;
					}
					this.Deactivate();
				}
			}
		}

		// Token: 0x04000909 RID: 2313
		private float _distBetweenPerkItemsMultiplier = 16f;

		// Token: 0x0400090A RID: 2314
		private Widget _latestMouseUpWidgetWhenActivated;

		// Token: 0x0400090B RID: 2315
		private bool _isActive;
	}
}
