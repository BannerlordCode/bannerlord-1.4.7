using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Menu.Overlay
{
	// Token: 0x02000124 RID: 292
	public class ArmyOverlayCohesionFillBarWidget : FillBarWidget
	{
		// Token: 0x06000F6F RID: 3951 RVA: 0x0002AB72 File Offset: 0x00028D72
		public ArmyOverlayCohesionFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0002AB82 File Offset: 0x00028D82
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isWarningDirty)
			{
				this.DetermineBarAnimState();
				this._isWarningDirty = false;
			}
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0002ABA0 File Offset: 0x00028DA0
		private void DetermineBarAnimState()
		{
			BrushWidget brushWidget;
			if (base.FillWidget != null && (brushWidget = base.FillWidget as BrushWidget) != null)
			{
				brushWidget.RegisterBrushStatesOfWidget();
				if (this.IsCohesionWarningEnabled)
				{
					if (brushWidget.CurrentState == "WarningLeader")
					{
						brushWidget.BrushRenderer.RestartAnimation();
						return;
					}
					if (this.IsArmyLeader)
					{
						brushWidget.SetState("WarningLeader");
						return;
					}
					brushWidget.SetState("WarningNormal");
					return;
				}
				else
				{
					if (brushWidget.CurrentState == "Default")
					{
						brushWidget.BrushRenderer.RestartAnimation();
						return;
					}
					brushWidget.SetState("Default");
				}
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0002AC3C File Offset: 0x00028E3C
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0002AC44 File Offset: 0x00028E44
		[Editor(false)]
		public bool IsCohesionWarningEnabled
		{
			get
			{
				return this._isCohesionWarningEnabled;
			}
			set
			{
				if (value != this._isCohesionWarningEnabled)
				{
					this._isCohesionWarningEnabled = value;
					base.OnPropertyChanged(value, "IsCohesionWarningEnabled");
					this.DetermineBarAnimState();
					this._isWarningDirty = true;
				}
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002AC6F File Offset: 0x00028E6F
		// (set) Token: 0x06000F75 RID: 3957 RVA: 0x0002AC77 File Offset: 0x00028E77
		[Editor(false)]
		public bool IsArmyLeader
		{
			get
			{
				return this._isArmyLeader;
			}
			set
			{
				if (value != this._isArmyLeader)
				{
					this._isArmyLeader = value;
					base.OnPropertyChanged(value, "IsArmyLeader");
					this.DetermineBarAnimState();
					this._isWarningDirty = true;
				}
			}
		}

		// Token: 0x04000707 RID: 1799
		private bool _isWarningDirty = true;

		// Token: 0x04000708 RID: 1800
		private bool _isCohesionWarningEnabled;

		// Token: 0x04000709 RID: 1801
		private bool _isArmyLeader;
	}
}
