using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000091 RID: 145
	public class CapturePointVM : CompassTargetVM
	{
		// Token: 0x06000DFD RID: 3581 RVA: 0x0002B060 File Offset: 0x00029260
		public CapturePointVM(FlagCapturePoint target, TargetIconType iconType)
			: base(iconType, 0U, 0U, null, false, false)
		{
			this.Target = target;
			foreach (string text in this.Target.GameEntity.Tags)
			{
				if (text.StartsWith("enable_") || text.StartsWith("disable_"))
				{
					this.IsSpawnAffectorFlag = true;
				}
			}
			if (this.Target.GameEntity.HasTag("keep_capture_point"))
			{
				this.IsKeepFlag = true;
			}
			this.ResetFlag();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002B0F5 File Offset: 0x000292F5
		public override void Refresh(float circleX, float x, float distance)
		{
			base.Refresh(circleX, x, distance);
			this.FlagProgress = this.Target.GetFlagProgress();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002B114 File Offset: 0x00029314
		public void OnOwnerChanged(Team newTeam)
		{
			uint num = ((newTeam != null) ? newTeam.Color : 4284111450U);
			uint num2 = ((newTeam != null) ? newTeam.Color2 : uint.MaxValue);
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0002B147 File Offset: 0x00029347
		public void ResetFlag()
		{
			this.OnOwnerChanged(null);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002B150 File Offset: 0x00029350
		internal void OnRemainingMoraleChanged(int remainingMorale)
		{
			if (this.RemainingRemovalTime != remainingMorale && remainingMorale != 90)
			{
				this.RemainingRemovalTime = (int)((float)remainingMorale / 1f);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0002B16F File Offset: 0x0002936F
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0002B177 File Offset: 0x00029377
		[DataSourceProperty]
		public float FlagProgress
		{
			get
			{
				return this._flagProgress;
			}
			set
			{
				if (value != this._flagProgress)
				{
					this._flagProgress = value;
					base.OnPropertyChangedWithValue(value, "FlagProgress");
				}
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0002B195 File Offset: 0x00029395
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0002B19D File Offset: 0x0002939D
		[DataSourceProperty]
		public bool IsSpawnAffectorFlag
		{
			get
			{
				return this._isSpawnAffectorFlag;
			}
			set
			{
				if (value != this._isSpawnAffectorFlag)
				{
					this._isSpawnAffectorFlag = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnAffectorFlag");
				}
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0002B1BB File Offset: 0x000293BB
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x0002B1C3 File Offset: 0x000293C3
		[DataSourceProperty]
		public bool IsKeepFlag
		{
			get
			{
				return this._isKeepFlag;
			}
			set
			{
				if (value != this._isKeepFlag)
				{
					this._isKeepFlag = value;
					base.OnPropertyChangedWithValue(value, "IsKeepFlag");
				}
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0002B1E1 File Offset: 0x000293E1
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0002B1E9 File Offset: 0x000293E9
		[DataSourceProperty]
		public int RemainingRemovalTime
		{
			get
			{
				return this._remainingRemovalTime;
			}
			set
			{
				if (value != this._remainingRemovalTime)
				{
					this._remainingRemovalTime = value;
					base.OnPropertyChangedWithValue(value, "RemainingRemovalTime");
				}
			}
		}

		// Token: 0x04000669 RID: 1641
		public readonly FlagCapturePoint Target;

		// Token: 0x0400066A RID: 1642
		private float _flagProgress;

		// Token: 0x0400066B RID: 1643
		private int _remainingRemovalTime = -1;

		// Token: 0x0400066C RID: 1644
		private bool _isKeepFlag;

		// Token: 0x0400066D RID: 1645
		private bool _isSpawnAffectorFlag;
	}
}
