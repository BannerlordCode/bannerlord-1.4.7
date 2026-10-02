using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006C RID: 108
	public class PartyUpgradeButtonWidget : ButtonWidget
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x00011827 File Offset: 0x0000FA27
		public PartyUpgradeButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00011830 File Offset: 0x0000FA30
		private void UpdateVisual()
		{
			if (this.ImageIdentifierWidget == null || this.UnavailableBrush == null || this.InsufficientBrush == null)
			{
				return;
			}
			if (!this.IsAvailable)
			{
				this.ImageIdentifierWidget.Brush.GlobalColor = new Color(1f, 1f, 1f, 1f);
				this.ImageIdentifierWidget.Brush.SaturationFactor = -100f;
				this._marinerTroopBrush.SetState("Disabled");
				base.UpdateChildrenStates = false;
				base.IsEnabled = true;
				base.Brush = this.UnavailableBrush;
				return;
			}
			if (this.IsAvailable && this.IsInsufficient)
			{
				this.ImageIdentifierWidget.Brush.GlobalColor = new Color(0.9f, 0.5f, 0.5f, 1f);
				this.ImageIdentifierWidget.Brush.SaturationFactor = -150f;
				this._marinerTroopBrush.SetState("Disabled");
				base.UpdateChildrenStates = false;
				base.IsEnabled = true;
				base.Brush = this.InsufficientBrush;
				return;
			}
			this.ImageIdentifierWidget.Brush.GlobalColor = new Color(1f, 1f, 1f, 1f);
			this.ImageIdentifierWidget.Brush.SaturationFactor = 0f;
			this._marinerTroopBrush.SetState("Default");
			base.UpdateChildrenStates = true;
			base.IsEnabled = true;
			base.Brush = this.DefaultBrush;
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000119A8 File Offset: 0x0000FBA8
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000119B0 File Offset: 0x0000FBB0
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifierWidget
		{
			get
			{
				return this._imageIdentifierWidget;
			}
			set
			{
				if (this._imageIdentifierWidget != value)
				{
					this._imageIdentifierWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifierWidget");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000119CE File Offset: 0x0000FBCE
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x000119D6 File Offset: 0x0000FBD6
		[Editor(false)]
		public Brush DefaultBrush
		{
			get
			{
				return this._defaultBrush;
			}
			set
			{
				if (this._defaultBrush != value)
				{
					this._defaultBrush = value;
					base.OnPropertyChanged<Brush>(value, "DefaultBrush");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000119F4 File Offset: 0x0000FBF4
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x000119FC File Offset: 0x0000FBFC
		[Editor(false)]
		public BrushWidget MarinerTroopBrush
		{
			get
			{
				return this._marinerTroopBrush;
			}
			set
			{
				if (this._marinerTroopBrush != value)
				{
					this._marinerTroopBrush = value;
					base.OnPropertyChanged<BrushWidget>(value, "MarinerTroopBrush");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00011A1A File Offset: 0x0000FC1A
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00011A22 File Offset: 0x0000FC22
		[Editor(false)]
		public Brush UnavailableBrush
		{
			get
			{
				return this._unavailableBrush;
			}
			set
			{
				if (this._unavailableBrush != value)
				{
					this._unavailableBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnavailableBrush");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00011A40 File Offset: 0x0000FC40
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00011A48 File Offset: 0x0000FC48
		[Editor(false)]
		public Brush InsufficientBrush
		{
			get
			{
				return this._insufficientBrush;
			}
			set
			{
				if (this._insufficientBrush != value)
				{
					this._insufficientBrush = value;
					base.OnPropertyChanged<Brush>(value, "InsufficientBrush");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00011A66 File Offset: 0x0000FC66
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x00011A6E File Offset: 0x0000FC6E
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (this._isAvailable != value)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
				}
				this.UpdateVisual();
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00011A92 File Offset: 0x0000FC92
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00011A9A File Offset: 0x0000FC9A
		[Editor(false)]
		public bool IsInsufficient
		{
			get
			{
				return this._isInsufficient;
			}
			set
			{
				if (this._isInsufficient != value)
				{
					this._isInsufficient = value;
					base.OnPropertyChanged(value, "IsInsufficient");
				}
				this.UpdateVisual();
			}
		}

		// Token: 0x04000286 RID: 646
		private ImageIdentifierWidget _imageIdentifierWidget;

		// Token: 0x04000287 RID: 647
		private Brush _defaultBrush;

		// Token: 0x04000288 RID: 648
		private Brush _unavailableBrush;

		// Token: 0x04000289 RID: 649
		private Brush _insufficientBrush;

		// Token: 0x0400028A RID: 650
		private BrushWidget _marinerTroopBrush;

		// Token: 0x0400028B RID: 651
		private bool _isAvailable;

		// Token: 0x0400028C RID: 652
		private bool _isInsufficient;
	}
}
