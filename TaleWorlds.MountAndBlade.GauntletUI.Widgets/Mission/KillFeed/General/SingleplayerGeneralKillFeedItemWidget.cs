using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.General
{
	// Token: 0x020000FD RID: 253
	public class SingleplayerGeneralKillFeedItemWidget : Widget
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00024D5C File Offset: 0x00022F5C
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x00024D64 File Offset: 0x00022F64
		public Brush TroopTypeIconBrush { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00024D6D File Offset: 0x00022F6D
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x00024D75 File Offset: 0x00022F75
		public Widget MurdererTypeWidget { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00024D7E File Offset: 0x00022F7E
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x00024D86 File Offset: 0x00022F86
		public Widget VictimTypeWidget { get; set; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00024D8F File Offset: 0x00022F8F
		// (set) Token: 0x06000D78 RID: 3448 RVA: 0x00024D97 File Offset: 0x00022F97
		public Widget ActionIconWidget { get; set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00024DA0 File Offset: 0x00022FA0
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00024DA8 File Offset: 0x00022FA8
		public TextWidget VictimNameWidget { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00024DB1 File Offset: 0x00022FB1
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x00024DB9 File Offset: 0x00022FB9
		public TextWidget MurdererNameWidget { get; set; }

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00024DC2 File Offset: 0x00022FC2
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x00024DCA File Offset: 0x00022FCA
		public float FadeInTime { get; set; } = 0.7f;

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00024DD3 File Offset: 0x00022FD3
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x00024DDB File Offset: 0x00022FDB
		public float StayTime { get; set; } = 3f;

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00024DE4 File Offset: 0x00022FE4
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00024DEC File Offset: 0x00022FEC
		public float FadeOutTime { get; set; } = 0.7f;

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00024DF5 File Offset: 0x00022FF5
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x00024DFD File Offset: 0x00022FFD
		public float TimeSinceCreation { get; private set; }

		// Token: 0x06000D85 RID: 3461 RVA: 0x00024E06 File Offset: 0x00023006
		public SingleplayerGeneralKillFeedItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00024E3C File Offset: 0x0002303C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._initialized)
			{
				this.Initialize();
				this._initialized = true;
			}
			if (!this.IsPaused)
			{
				this.TimeSinceCreation += dt * this._speedModifier;
			}
			if (this.TimeSinceCreation <= this.FadeInTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0.5f, this.TimeSinceCreation / this.FadeInTime));
				return;
			}
			if (this.TimeSinceCreation - this.FadeInTime <= this.StayTime)
			{
				this.SetGlobalAlphaRecursively(0.5f);
				return;
			}
			if (this.TimeSinceCreation - (this.FadeInTime + this.StayTime) <= this.FadeOutTime)
			{
				this.SetGlobalAlphaRecursively(Mathf.Lerp(base.AlphaFactor, 0f, (this.TimeSinceCreation - (this.FadeInTime + this.StayTime)) / this.FadeOutTime));
				if (base.AlphaFactor <= 0.1f)
				{
					base.EventFired("OnRemove", Array.Empty<object>());
					return;
				}
			}
			else
			{
				base.EventFired("OnRemove", Array.Empty<object>());
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00024F50 File Offset: 0x00023150
		private void Initialize()
		{
			this.SetGlobalAlphaRecursively(0f);
			Widget murdererTypeWidget = this.MurdererTypeWidget;
			Brush troopTypeIconBrush = this.TroopTypeIconBrush;
			Sprite sprite;
			if (troopTypeIconBrush == null)
			{
				sprite = null;
			}
			else
			{
				BrushLayer layer = troopTypeIconBrush.GetLayer(this.MurdererType);
				sprite = ((layer != null) ? layer.Sprite : null);
			}
			murdererTypeWidget.Sprite = sprite;
			Widget victimTypeWidget = this.VictimTypeWidget;
			Brush troopTypeIconBrush2 = this.TroopTypeIconBrush;
			Sprite sprite2;
			if (troopTypeIconBrush2 == null)
			{
				sprite2 = null;
			}
			else
			{
				BrushLayer layer2 = troopTypeIconBrush2.GetLayer(this.VictimType);
				sprite2 = ((layer2 != null) ? layer2.Sprite : null);
			}
			victimTypeWidget.Sprite = sprite2;
			this.ActionIconWidget.Sprite = this.ActionIconWidget.Context.SpriteData.GetSprite("General\\Mission\\PersonalKillfeed\\" + this.GetSpriteName());
			this.ActionIconWidget.Color = (this.IsUnconscious ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 0f, 0f, 1f));
			if (string.IsNullOrEmpty(this.VictimName))
			{
				this.VictimNameWidget.IsVisible = false;
				this.ActionIconWidget.MarginRight = 0f;
				this.VictimTypeWidget.MarginLeft = 5f;
			}
			if (string.IsNullOrEmpty(this.MurdererName))
			{
				this.MurdererNameWidget.IsVisible = false;
				this.ActionIconWidget.MarginLeft = 0f;
				this.MurdererTypeWidget.MarginRight = 5f;
			}
			if (this.IsSuicide)
			{
				this.MurdererNameWidget.IsVisible = false;
				this.MurdererTypeWidget.IsVisible = false;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000250CF File Offset: 0x000232CF
		private string GetSpriteName()
		{
			if (this.IsDrowning)
			{
				return "drowning_kill_icon";
			}
			if (this.IsHeadshot)
			{
				return "headshot_kill_icon";
			}
			return "kill_feed_skull";
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000250F2 File Offset: 0x000232F2
		public void SetSpeedModifier(float newSpeed)
		{
			if (newSpeed > this._speedModifier)
			{
				this._speedModifier = newSpeed;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00025104 File Offset: 0x00023304
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0002510C File Offset: 0x0002330C
		[Editor(false)]
		public string MurdererName
		{
			get
			{
				return this._murdererName;
			}
			set
			{
				if (value != this._murdererName)
				{
					this._murdererName = value;
					base.OnPropertyChanged<string>(value, "MurdererName");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0002512F File Offset: 0x0002332F
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00025137 File Offset: 0x00023337
		[Editor(false)]
		public string MurdererType
		{
			get
			{
				return this._murdererType;
			}
			set
			{
				if (value != this._murdererType)
				{
					this._murdererType = value;
					base.OnPropertyChanged<string>(value, "MurdererType");
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002515A File Offset: 0x0002335A
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00025162 File Offset: 0x00023362
		[Editor(false)]
		public string VictimName
		{
			get
			{
				return this._victimName;
			}
			set
			{
				if (value != this._victimName)
				{
					this._victimName = value;
					base.OnPropertyChanged<string>(value, "VictimName");
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00025185 File Offset: 0x00023385
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0002518D File Offset: 0x0002338D
		[Editor(false)]
		public string VictimType
		{
			get
			{
				return this._victimType;
			}
			set
			{
				if (value != this._victimType)
				{
					this._victimType = value;
					base.OnPropertyChanged<string>(value, "VictimType");
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x000251B0 File Offset: 0x000233B0
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x000251B8 File Offset: 0x000233B8
		[Editor(false)]
		public bool IsUnconscious
		{
			get
			{
				return this._isUnconscious;
			}
			set
			{
				if (value != this._isUnconscious)
				{
					this._isUnconscious = value;
					base.OnPropertyChanged(value, "IsUnconscious");
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x000251D6 File Offset: 0x000233D6
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x000251DE File Offset: 0x000233DE
		[Editor(false)]
		public bool IsHeadshot
		{
			get
			{
				return this._isHeadshot;
			}
			set
			{
				if (value != this._isHeadshot)
				{
					this._isHeadshot = value;
					base.OnPropertyChanged(value, "IsHeadshot");
				}
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x000251FC File Offset: 0x000233FC
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00025204 File Offset: 0x00023404
		[Editor(false)]
		public bool IsSuicide
		{
			get
			{
				return this._isSuicide;
			}
			set
			{
				if (value != this._isSuicide)
				{
					this._isSuicide = value;
					base.OnPropertyChanged(value, "IsSuicide");
				}
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00025222 File Offset: 0x00023422
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x0002522A File Offset: 0x0002342A
		[Editor(false)]
		public bool IsDrowning
		{
			get
			{
				return this._isDrowning;
			}
			set
			{
				if (value != this._isDrowning)
				{
					this._isDrowning = value;
					base.OnPropertyChanged(value, "IsDrowning");
				}
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00025248 File Offset: 0x00023448
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x00025250 File Offset: 0x00023450
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
			set
			{
				if (value != this._isPaused)
				{
					this._isPaused = value;
					base.OnPropertyChanged(value, "IsPaused");
				}
			}
		}

		// Token: 0x0400061C RID: 1564
		private float _speedModifier = 1f;

		// Token: 0x0400061E RID: 1566
		private bool _initialized;

		// Token: 0x0400061F RID: 1567
		private string _murdererName;

		// Token: 0x04000620 RID: 1568
		private string _murdererType;

		// Token: 0x04000621 RID: 1569
		private string _victimName;

		// Token: 0x04000622 RID: 1570
		private string _victimType;

		// Token: 0x04000623 RID: 1571
		private bool _isUnconscious;

		// Token: 0x04000624 RID: 1572
		private bool _isHeadshot;

		// Token: 0x04000625 RID: 1573
		private bool _isSuicide;

		// Token: 0x04000626 RID: 1574
		private bool _isDrowning;

		// Token: 0x04000627 RID: 1575
		private bool _isPaused;
	}
}
