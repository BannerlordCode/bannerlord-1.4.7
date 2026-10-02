using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000113 RID: 275
	public class PowerLevelComparerWidget : Widget
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x00028501 File Offset: 0x00026701
		public PowerLevelComparerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002850C File Offset: 0x0002670C
		protected override void OnLateUpdate(float dt)
		{
			if (this.AttackerPowerWidget != null)
			{
				this.AttackerPowerWidget.AlphaFactor = 0.7f;
				this.AttackerPowerWidget.ValueFactor = -70f;
			}
			if (this.DefenderPowerWidget != null)
			{
				this.DefenderPowerWidget.AlphaFactor = 0.7f;
				this.DefenderPowerWidget.ValueFactor = -70f;
			}
			if (this._powerListPanel != null)
			{
				if (this._defenderSideInitialPowerLevelDescription == null)
				{
					this._defenderSideInitialPowerLevelDescription = new ContainerItemDescription();
					this._defenderSideInitialPowerLevelDescription.WidgetId = "DefenderSideInitialPowerLevel";
					this._powerListPanel.AddItemDescription(this._defenderSideInitialPowerLevelDescription);
				}
				if (this._attackerSideInitialPowerLevelDescription == null)
				{
					this._attackerSideInitialPowerLevelDescription = new ContainerItemDescription();
					this._attackerSideInitialPowerLevelDescription.WidgetId = "AttackerSideInitialPowerLevel";
					this._powerListPanel.AddItemDescription(this._attackerSideInitialPowerLevelDescription);
				}
			}
			if (this._defenderPowerListPanel != null)
			{
				if (this._defenderSidePowerLevelDescription == null)
				{
					this._defenderSidePowerLevelDescription = new ContainerItemDescription();
					this._defenderSidePowerLevelDescription.WidgetId = "DefenderSidePowerLevel";
					this._defenderPowerListPanel.AddItemDescription(this._defenderSidePowerLevelDescription);
				}
				if (this._defenderSideEmptyPowerLevelDescription == null)
				{
					this._defenderSideEmptyPowerLevelDescription = new ContainerItemDescription();
					this._defenderSideEmptyPowerLevelDescription.WidgetId = "DefenderSideEmptyPowerLevel";
					this._defenderPowerListPanel.AddItemDescription(this._defenderSideEmptyPowerLevelDescription);
				}
			}
			if (this._attackerPowerListPanel != null)
			{
				if (this._attackerSidePowerLevelDescription == null)
				{
					this._attackerSidePowerLevelDescription = new ContainerItemDescription();
					this._attackerSidePowerLevelDescription.WidgetId = "AttackerSidePowerLevel";
					this._attackerPowerListPanel.AddItemDescription(this._attackerSidePowerLevelDescription);
				}
				if (this._attackerSideEmptyPowerLevelDescription == null)
				{
					this._attackerSideEmptyPowerLevelDescription = new ContainerItemDescription();
					this._attackerSideEmptyPowerLevelDescription.WidgetId = "AttackerSideEmptyPowerLevel";
					this._attackerPowerListPanel.AddItemDescription(this._attackerSideEmptyPowerLevelDescription);
				}
			}
			if (this._defenderSideInitialPowerLevelDescription != null && this._attackerSideInitialPowerLevelDescription != null)
			{
				float num = (float)this.InitialDefenderBattlePower / (float)(this.InitialAttackerBattlePower + this.InitialDefenderBattlePower);
				float num2 = (float)this.InitialAttackerBattlePower / (float)(this.InitialAttackerBattlePower + this.InitialDefenderBattlePower);
				if (this._defenderSideInitialPowerLevelDescription.WidthStretchRatio != num || this._attackerSideInitialPowerLevelDescription.WidthStretchRatio != num2)
				{
					this._defenderSideInitialPowerLevelDescription.WidthStretchRatio = num;
					this._attackerSideInitialPowerLevelDescription.WidthStretchRatio = num2;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this._defenderSidePowerLevelDescription != null && this._defenderSideEmptyPowerLevelDescription != null)
			{
				float num3 = 1f - (float)this.DefenderPower / (float)this.InitialDefenderBattlePower;
				float num4 = (float)this.DefenderPower / (float)this.InitialDefenderBattlePower;
				if (this._defenderSideEmptyPowerLevelDescription.WidthStretchRatio != num3 || this._defenderSidePowerLevelDescription.WidthStretchRatio != num4)
				{
					this._defenderSidePowerLevelDescription.WidthStretchRatio = num4;
					this._defenderSideEmptyPowerLevelDescription.WidthStretchRatio = num3;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this._attackerSidePowerLevelDescription != null && this._attackerSideEmptyPowerLevelDescription != null)
			{
				float num5 = 1f - (float)this.AttackerPower / (float)this.InitialAttackerBattlePower;
				float num6 = (float)this.AttackerPower / (float)this.InitialAttackerBattlePower;
				if (this._attackerSidePowerLevelDescription.WidthStretchRatio != num6 || this._attackerSideEmptyPowerLevelDescription.WidthStretchRatio != num5)
				{
					this._attackerSidePowerLevelDescription.WidthStretchRatio = num6;
					this._attackerSideEmptyPowerLevelDescription.WidthStretchRatio = num5;
					base.SetMeasureAndLayoutDirty();
				}
			}
			if (this.IsCenterSeperatorEnabled && this.CenterSeperatorWidget != null)
			{
				this.CenterSeperatorWidget.ScaledPositionXOffset = this.AttackerPowerWidget.Size.X - (this.CenterSeperatorWidget.Size.X - this.CenterSpace) / 2f;
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x00028867 File Offset: 0x00026A67
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x0002886F File Offset: 0x00026A6F
		[Editor(false)]
		public bool IsCenterSeperatorEnabled
		{
			get
			{
				return this._isCenterSeperatorEnabled;
			}
			set
			{
				if (this._isCenterSeperatorEnabled != value)
				{
					this._isCenterSeperatorEnabled = value;
					base.OnPropertyChanged(value, "IsCenterSeperatorEnabled");
				}
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0002888D File Offset: 0x00026A8D
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x00028895 File Offset: 0x00026A95
		[Editor(false)]
		public float CenterSpace
		{
			get
			{
				return this._centerSpace;
			}
			set
			{
				if (this._centerSpace != value)
				{
					this._centerSpace = value;
					base.OnPropertyChanged(value, "CenterSpace");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x000288B3 File Offset: 0x00026AB3
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x000288BB File Offset: 0x00026ABB
		[Editor(false)]
		public double DefenderPower
		{
			get
			{
				return this._defenderPower;
			}
			set
			{
				if (this._defenderPower != value && !double.IsNaN(value))
				{
					this._defenderPower = value;
					base.OnPropertyChanged(value, "DefenderPower");
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x000288E1 File Offset: 0x00026AE1
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x000288E9 File Offset: 0x00026AE9
		[Editor(false)]
		public double AttackerPower
		{
			get
			{
				return this._attackerPower;
			}
			set
			{
				if (this._attackerPower != value && !double.IsNaN(value))
				{
					this._attackerPower = value;
					base.OnPropertyChanged(value, "AttackerPower");
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0002890F File Offset: 0x00026B0F
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x00028917 File Offset: 0x00026B17
		[Editor(false)]
		public double InitialAttackerBattlePower
		{
			get
			{
				return this._initialAttackerBattlePower;
			}
			set
			{
				if (this._initialAttackerBattlePower != value && !double.IsNaN(value))
				{
					this._initialAttackerBattlePower = value;
					base.OnPropertyChanged(value, "InitialAttackerBattlePower");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0002893D File Offset: 0x00026B3D
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x00028945 File Offset: 0x00026B45
		[Editor(false)]
		public double InitialDefenderBattlePower
		{
			get
			{
				return this._initialDefenderBattlePower;
			}
			set
			{
				if (this._initialDefenderBattlePower != value && !double.IsNaN(value))
				{
					this._initialDefenderBattlePower = value;
					base.OnPropertyChanged(value, "InitialDefenderBattlePower");
				}
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0002896B File Offset: 0x00026B6B
		// (set) Token: 0x06000EB9 RID: 3769 RVA: 0x00028973 File Offset: 0x00026B73
		[Editor(false)]
		public Widget AttackerPowerWidget
		{
			get
			{
				return this._attackerPowerWidget;
			}
			set
			{
				if (this._attackerPowerWidget != value)
				{
					this._attackerPowerWidget = value;
					base.OnPropertyChanged<Widget>(value, "AttackerPowerWidget");
				}
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00028991 File Offset: 0x00026B91
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00028999 File Offset: 0x00026B99
		[Editor(false)]
		public Widget DefenderPowerWidget
		{
			get
			{
				return this._defenderPowerWidget;
			}
			set
			{
				if (this._defenderPowerWidget != value)
				{
					this._defenderPowerWidget = value;
					base.OnPropertyChanged<Widget>(value, "DefenderPowerWidget");
				}
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x000289B7 File Offset: 0x00026BB7
		// (set) Token: 0x06000EBD RID: 3773 RVA: 0x000289BF File Offset: 0x00026BBF
		[Editor(false)]
		public ListPanel PowerListPanel
		{
			get
			{
				return this._powerListPanel;
			}
			set
			{
				if (this._powerListPanel != value)
				{
					this._powerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "PowerListPanel");
				}
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06000EBE RID: 3774 RVA: 0x000289DD File Offset: 0x00026BDD
		// (set) Token: 0x06000EBF RID: 3775 RVA: 0x000289E5 File Offset: 0x00026BE5
		[Editor(false)]
		public ListPanel AttackerPowerListPanel
		{
			get
			{
				return this._attackerPowerListPanel;
			}
			set
			{
				if (this._attackerPowerListPanel != value)
				{
					this._attackerPowerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "AttackerPowerListPanel");
				}
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x00028A03 File Offset: 0x00026C03
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x00028A0B File Offset: 0x00026C0B
		[Editor(false)]
		public ListPanel DefenderPowerListPanel
		{
			get
			{
				return this._defenderPowerListPanel;
			}
			set
			{
				if (this._defenderPowerListPanel != value)
				{
					this._defenderPowerListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "DefenderPowerListPanel");
				}
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x00028A29 File Offset: 0x00026C29
		// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x00028A31 File Offset: 0x00026C31
		[Editor(false)]
		public Widget CenterSeperatorWidget
		{
			get
			{
				return this._centerSeperatorWidget;
			}
			set
			{
				if (this._centerSeperatorWidget != value)
				{
					this._centerSeperatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "CenterSeperatorWidget");
				}
			}
		}

		// Token: 0x040006A7 RID: 1703
		private Widget _centerSeperatorWidget;

		// Token: 0x040006A8 RID: 1704
		private bool _isCenterSeperatorEnabled;

		// Token: 0x040006A9 RID: 1705
		private float _centerSpace;

		// Token: 0x040006AA RID: 1706
		private double _defenderPower;

		// Token: 0x040006AB RID: 1707
		private double _attackerPower;

		// Token: 0x040006AC RID: 1708
		private double _initialAttackerBattlePower;

		// Token: 0x040006AD RID: 1709
		private double _initialDefenderBattlePower;

		// Token: 0x040006AE RID: 1710
		private Widget _defenderPowerWidget;

		// Token: 0x040006AF RID: 1711
		private Widget _attackerPowerWidget;

		// Token: 0x040006B0 RID: 1712
		private ListPanel _powerListPanel;

		// Token: 0x040006B1 RID: 1713
		private ListPanel _defenderPowerListPanel;

		// Token: 0x040006B2 RID: 1714
		private ListPanel _attackerPowerListPanel;

		// Token: 0x040006B3 RID: 1715
		private ContainerItemDescription _defenderSideInitialPowerLevelDescription;

		// Token: 0x040006B4 RID: 1716
		private ContainerItemDescription _attackerSideInitialPowerLevelDescription;

		// Token: 0x040006B5 RID: 1717
		private ContainerItemDescription _defenderSidePowerLevelDescription;

		// Token: 0x040006B6 RID: 1718
		private ContainerItemDescription _defenderSideEmptyPowerLevelDescription;

		// Token: 0x040006B7 RID: 1719
		private ContainerItemDescription _attackerSidePowerLevelDescription;

		// Token: 0x040006B8 RID: 1720
		private ContainerItemDescription _attackerSideEmptyPowerLevelDescription;
	}
}
