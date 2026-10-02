using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x02000129 RID: 297
	public class MapBarGatherArmyBrushWidget : BrushWidget
	{
		// Token: 0x06000F93 RID: 3987 RVA: 0x0002B05A File Offset: 0x0002925A
		public MapBarGatherArmyBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0002B063 File Offset: 0x00029263
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.UpdateVisualState();
				this._initialized = true;
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0002B084 File Offset: 0x00029284
		private void UpdateVisualState()
		{
			base.IsEnabled = this.IsGatherArmyVisible;
			if (!this.IsGatherArmyVisible)
			{
				this.SetState("Disabled");
				return;
			}
			if (this._isInfoBarExtended)
			{
				this.SetState("Extended");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0002B0D0 File Offset: 0x000292D0
		private void OnMapInfoBarExtendStateChange(bool newState)
		{
			this._isInfoBarExtended = newState;
			this.UpdateVisualState();
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0002B0DF File Offset: 0x000292DF
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x0002B0E7 File Offset: 0x000292E7
		public MapInfoBarWidget InfoBarWidget
		{
			get
			{
				return this._infoBarWidget;
			}
			set
			{
				if (this._infoBarWidget != value)
				{
					this._infoBarWidget = value;
					this._infoBarWidget.OnMapInfoBarExtendStateChange += this.OnMapInfoBarExtendStateChange;
				}
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0002B110 File Offset: 0x00029310
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x0002B118 File Offset: 0x00029318
		public bool IsGatherArmyEnabled
		{
			get
			{
				return this._isGatherArmyEnabled;
			}
			set
			{
				if (this._isGatherArmyEnabled != value)
				{
					this._isGatherArmyEnabled = value;
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0002B130 File Offset: 0x00029330
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x0002B138 File Offset: 0x00029338
		public bool IsGatherArmyVisible
		{
			get
			{
				return this._isGatherArmyVisible;
			}
			set
			{
				if (this._isGatherArmyVisible != value)
				{
					this._isGatherArmyVisible = value;
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x04000713 RID: 1811
		private bool _isInfoBarExtended;

		// Token: 0x04000714 RID: 1812
		private bool _initialized;

		// Token: 0x04000715 RID: 1813
		private MapInfoBarWidget _infoBarWidget;

		// Token: 0x04000716 RID: 1814
		private bool _isGatherArmyEnabled;

		// Token: 0x04000717 RID: 1815
		private bool _isGatherArmyVisible;
	}
}
