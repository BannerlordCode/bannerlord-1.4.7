using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000177 RID: 375
	public class ClanPartyRoleSelectionToggleWidget : ButtonWidget
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x000350F0 File Offset: 0x000332F0
		public ClanPartyRoleSelectionToggleWidget(UIContext context)
			: base(context)
		{
			this.ClickEventHandlers.Add(new Action<Widget>(this.OnClick));
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00035114 File Offset: 0x00033314
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			ClanPartyRoleSelectionPopupWidget popup = this.Popup;
			if (((popup != null) ? popup.ActiveToggleWidget : null) == this && MathF.Abs(this.Popup.Size.Y - this._lastPopupSizeY) > 1E-05f)
			{
				this.UpdatePopupPosition();
				this._lastPopupSizeY = this.Popup.Size.Y;
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0003517C File Offset: 0x0003337C
		protected virtual void OnClick(Widget widget)
		{
			if (this.Popup != null)
			{
				if (this.Popup.ActiveToggleWidget == this)
				{
					this.ClosePopup();
					return;
				}
				this.OpenPopup();
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000351A1 File Offset: 0x000333A1
		private void OpenPopup()
		{
			this.Popup.ActiveToggleWidget = this;
			this.Popup.IsVisible = true;
			this.UpdatePopupPosition();
			this._lastPopupSizeY = this.Popup.Size.Y;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000351D7 File Offset: 0x000333D7
		private void ClosePopup()
		{
			this.Popup.ActiveToggleWidget = null;
			this.Popup.IsVisible = false;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x000351F4 File Offset: 0x000333F4
		private void UpdatePopupPosition()
		{
			this.Popup.ScaledPositionYOffset += base.GlobalPosition.Y - this.Popup.GlobalPosition.Y - this.Popup.Size.Y + 47f * base._scaleToUse;
			this.Popup.ScaledPositionXOffset += base.GlobalPosition.X - this.Popup.GlobalPosition.X + 80f * base._scaleToUse;
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00035288 File Offset: 0x00033488
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x00035290 File Offset: 0x00033490
		[Editor(false)]
		public ClanPartyRoleSelectionPopupWidget Popup
		{
			get
			{
				return this._popup;
			}
			set
			{
				if (this._popup != value)
				{
					this._popup = value;
					base.OnPropertyChanged<ClanPartyRoleSelectionPopupWidget>(value, "Popup");
					this._popup.AddToggleWidget(this);
				}
			}
		}

		// Token: 0x040008D8 RID: 2264
		private float _lastPopupSizeY;

		// Token: 0x040008D9 RID: 2265
		private ClanPartyRoleSelectionPopupWidget _popup;
	}
}
