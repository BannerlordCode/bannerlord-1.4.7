using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005D RID: 93
	public class QuestItemButtonWidget : ButtonWidget
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0000F818 File Offset: 0x0000DA18
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x0000F820 File Offset: 0x0000DA20
		public Brush MainStoryLineItemBrush { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000F829 File Offset: 0x0000DA29
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x0000F831 File Offset: 0x0000DA31
		public Brush NavalStorylineItemBrush { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000F83A File Offset: 0x0000DA3A
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x0000F842 File Offset: 0x0000DA42
		public Brush NormalItemBrush { get; set; }

		// Token: 0x0600050C RID: 1292 RVA: 0x0000F84B File Offset: 0x0000DA4B
		public QuestItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000F854 File Offset: 0x0000DA54
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				if (this.IsNavalStorylineQuest)
				{
					base.Brush = this.NavalStorylineItemBrush;
				}
				else if (this.IsMainStoryLineQuest)
				{
					base.Brush = this.MainStoryLineItemBrush;
				}
				else
				{
					base.Brush = this.NormalItemBrush;
				}
				this._initialized = true;
			}
			if (this.QuestNameText != null && this.QuestDateText != null)
			{
				if (base.CurrentState == "Pressed")
				{
					this.QuestNameText.PositionYOffset = (float)this.QuestNameYOffset;
					this.QuestNameText.PositionXOffset = (float)this.QuestNameXOffset;
					this.QuestDateText.PositionYOffset = (float)this.QuestDateYOffset;
					this.QuestDateText.PositionXOffset = (float)this.QuestDateXOffset;
				}
				else
				{
					this.QuestNameText.PositionYOffset = 0f;
					this.QuestNameText.PositionXOffset = 0f;
					this.QuestDateText.PositionYOffset = 0f;
					this.QuestDateText.PositionXOffset = 0f;
				}
			}
			if (this.QuestDateText != null)
			{
				if (this.IsCompleted)
				{
					this.QuestDateText.IsVisible = false;
					return;
				}
				this.QuestDateText.IsHidden = this.IsRemainingDaysHidden;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0000F98F File Offset: 0x0000DB8F
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x0000F997 File Offset: 0x0000DB97
		[Editor(false)]
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
			set
			{
				if (this._isCompleted != value)
				{
					this._isCompleted = value;
					base.OnPropertyChanged(value, "IsCompleted");
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0000F9B5 File Offset: 0x0000DBB5
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x0000F9BD File Offset: 0x0000DBBD
		[Editor(false)]
		public bool IsMainStoryLineQuest
		{
			get
			{
				return this._isMainStoryLineQuest;
			}
			set
			{
				if (this._isMainStoryLineQuest != value)
				{
					this._isMainStoryLineQuest = value;
					base.OnPropertyChanged(value, "IsMainStoryLineQuest");
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0000F9DB File Offset: 0x0000DBDB
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x0000F9E3 File Offset: 0x0000DBE3
		[Editor(false)]
		public bool IsNavalStorylineQuest
		{
			get
			{
				return this._isNavalStorylineQuest;
			}
			set
			{
				if (this._isNavalStorylineQuest != value)
				{
					this._isNavalStorylineQuest = value;
					base.OnPropertyChanged(value, "IsNavalStorylineQuest");
				}
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0000FA01 File Offset: 0x0000DC01
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0000FA09 File Offset: 0x0000DC09
		[Editor(false)]
		public bool IsRemainingDaysHidden
		{
			get
			{
				return this._isRemainingDaysHidden;
			}
			set
			{
				if (this._isRemainingDaysHidden != value)
				{
					this._isRemainingDaysHidden = value;
					base.OnPropertyChanged(value, "IsRemainingDaysHidden");
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000FA27 File Offset: 0x0000DC27
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0000FA2F File Offset: 0x0000DC2F
		[Editor(false)]
		public TextWidget QuestNameText
		{
			get
			{
				return this._questNameText;
			}
			set
			{
				if (this._questNameText != value)
				{
					this._questNameText = value;
					base.OnPropertyChanged<TextWidget>(value, "QuestNameText");
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000FA4D File Offset: 0x0000DC4D
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x0000FA55 File Offset: 0x0000DC55
		[Editor(false)]
		public TextWidget QuestDateText
		{
			get
			{
				return this._questDateText;
			}
			set
			{
				if (this._questDateText != value)
				{
					this._questDateText = value;
					base.OnPropertyChanged<TextWidget>(value, "QuestDateText");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000FA73 File Offset: 0x0000DC73
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x0000FA7B File Offset: 0x0000DC7B
		[Editor(false)]
		public int QuestNameYOffset
		{
			get
			{
				return this._questNameYOffset;
			}
			set
			{
				if (this._questNameYOffset != value)
				{
					this._questNameYOffset = value;
					base.OnPropertyChanged(value, "QuestNameYOffset");
				}
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000FA99 File Offset: 0x0000DC99
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x0000FAA1 File Offset: 0x0000DCA1
		[Editor(false)]
		public int QuestNameXOffset
		{
			get
			{
				return this._questNameXOffset;
			}
			set
			{
				if (this._questNameXOffset != value)
				{
					this._questNameXOffset = value;
					base.OnPropertyChanged(value, "QuestNameXOffset");
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0000FABF File Offset: 0x0000DCBF
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x0000FAC7 File Offset: 0x0000DCC7
		[Editor(false)]
		public int QuestDateYOffset
		{
			get
			{
				return this._questDateYOffset;
			}
			set
			{
				if (this._questDateYOffset != value)
				{
					this._questDateYOffset = value;
					base.OnPropertyChanged(value, "QuestDateYOffset");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0000FAE5 File Offset: 0x0000DCE5
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x0000FAED File Offset: 0x0000DCED
		[Editor(false)]
		public int QuestDateXOffset
		{
			get
			{
				return this._questDateXOffset;
			}
			set
			{
				if (this._questDateXOffset != value)
				{
					this._questDateXOffset = value;
					base.OnPropertyChanged(value, "QuestDateXOffset");
				}
			}
		}

		// Token: 0x04000227 RID: 551
		private bool _initialized;

		// Token: 0x0400022B RID: 555
		private TextWidget _questNameText;

		// Token: 0x0400022C RID: 556
		private TextWidget _questDateText;

		// Token: 0x0400022D RID: 557
		private int _questNameYOffset;

		// Token: 0x0400022E RID: 558
		private int _questNameXOffset;

		// Token: 0x0400022F RID: 559
		private int _questDateYOffset;

		// Token: 0x04000230 RID: 560
		private int _questDateXOffset;

		// Token: 0x04000231 RID: 561
		private bool _isCompleted;

		// Token: 0x04000232 RID: 562
		private bool _isRemainingDaysHidden;

		// Token: 0x04000233 RID: 563
		private bool _isMainStoryLineQuest;

		// Token: 0x04000234 RID: 564
		private bool _isNavalStorylineQuest;
	}
}
