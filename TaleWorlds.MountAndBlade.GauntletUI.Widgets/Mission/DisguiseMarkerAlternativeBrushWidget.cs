using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000DC RID: 220
	public class DisguiseMarkerAlternativeBrushWidget : BrushWidget
	{
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0001F7D9 File Offset: 0x0001D9D9
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0001F7E1 File Offset: 0x0001D9E1
		public Widget BackgroundGlowWidget { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0001F7EA File Offset: 0x0001D9EA
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x0001F7F2 File Offset: 0x0001D9F2
		public Widget FrameWidget { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0001F7FB File Offset: 0x0001D9FB
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0001F803 File Offset: 0x0001DA03
		public Widget FillBarWidget { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0001F80C File Offset: 0x0001DA0C
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0001F814 File Offset: 0x0001DA14
		public float AlarmedHeight { get; set; } = 40f;

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0001F81D File Offset: 0x0001DA1D
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0001F825 File Offset: 0x0001DA25
		public float DefaultHeight { get; set; } = 20f;

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0001F82E File Offset: 0x0001DA2E
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0001F836 File Offset: 0x0001DA36
		public Vec2 Position { get; set; }

		// Token: 0x06000B4E RID: 2894 RVA: 0x0001F83F File Offset: 0x0001DA3F
		public DisguiseMarkerAlternativeBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0001F860 File Offset: 0x0001DA60
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			bool flag = !string.IsNullOrEmpty(this.AlarmState) && (float)this.AlarmProgress > 0f;
			base.SuggestedHeight = MathF.Lerp(base.SuggestedHeight, flag ? this.AlarmedHeight : this.DefaultHeight, dt * 5f, 1E-05f);
			base.SuggestedWidth = MathF.Lerp(base.SuggestedWidth, (float)(flag ? 38 : 32), dt * 5f, 1E-05f);
			if (!string.IsNullOrEmpty(this.OffenseTypeIdentifier))
			{
				Widget backgroundGlowWidget = this.BackgroundGlowWidget;
				if (backgroundGlowWidget != null)
				{
					backgroundGlowWidget.SetState(this.OffenseTypeIdentifier);
				}
			}
			if (!string.IsNullOrEmpty(this.AlarmState))
			{
				Widget fillBarWidget = this.FillBarWidget;
				if (fillBarWidget == null)
				{
					return;
				}
				fillBarWidget.SetState(this.AlarmState);
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0001F975 File Offset: 0x0001DB75
		private void UpdateState()
		{
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0001F977 File Offset: 0x0001DB77
		private void UpdateAlarmState()
		{
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0001F979 File Offset: 0x0001DB79
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x0001F981 File Offset: 0x0001DB81
		public int AlarmProgress
		{
			get
			{
				return this._alarmProgress;
			}
			set
			{
				if (value != this._alarmProgress)
				{
					this._alarmProgress = value;
					base.OnPropertyChanged(value, "AlarmProgress");
				}
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0001F99F File Offset: 0x0001DB9F
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0001F9A7 File Offset: 0x0001DBA7
		public string AlarmState
		{
			get
			{
				return this._alarmState;
			}
			set
			{
				if (value != this._alarmState)
				{
					this._alarmState = value;
					base.OnPropertyChanged<string>(value, "AlarmState");
					this.UpdateAlarmState();
				}
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
		public string OffenseTypeIdentifier
		{
			get
			{
				return this._offenseTypeIdentifier;
			}
			set
			{
				if (value != this._offenseTypeIdentifier)
				{
					this._offenseTypeIdentifier = value;
					base.OnPropertyChanged<string>(value, "OffenseTypeIdentifier");
					this.UpdateState();
				}
			}
		}

		// Token: 0x0400051D RID: 1309
		private int _alarmProgress;

		// Token: 0x0400051E RID: 1310
		private string _alarmState;

		// Token: 0x0400051F RID: 1311
		private string _offenseTypeIdentifier;
	}
}
