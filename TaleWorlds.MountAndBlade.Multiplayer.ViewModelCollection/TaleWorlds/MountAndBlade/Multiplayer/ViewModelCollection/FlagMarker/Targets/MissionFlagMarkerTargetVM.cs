using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x02000098 RID: 152
	public class MissionFlagMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0002E303 File Offset: 0x0002C503
		// (set) Token: 0x06000EF5 RID: 3829 RVA: 0x0002E30B File Offset: 0x0002C50B
		public FlagCapturePoint TargetFlag { get; private set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0002E314 File Offset: 0x0002C514
		public override Vec3 WorldPosition
		{
			get
			{
				if (this.TargetFlag != null)
				{
					return this.TargetFlag.Position;
				}
				Debug.FailedAssert("No target found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\FlagMarker\\Targets\\MissionFlagMarkerTargetVM.cs", "WorldPosition", 24);
				return Vec3.One;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0002E345 File Offset: 0x0002C545
		protected override float HeightOffset
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0002E34C File Offset: 0x0002C54C
		public MissionFlagMarkerTargetVM(FlagCapturePoint flag)
			: base(MissionMarkerType.Flag)
		{
			this.TargetFlag = flag;
			base.Name = Convert.ToChar(flag.FlagChar).ToString();
			foreach (string text in this.TargetFlag.GameEntity.Tags)
			{
				if (text.StartsWith("enable_") || text.StartsWith("disable_"))
				{
					this.IsSpawnAffectorFlag = true;
				}
			}
			if (this.TargetFlag.GameEntity.HasTag("keep_capture_point"))
			{
				this.IsKeepFlag = true;
			}
			this.OnOwnerChanged(null);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0002E3FC File Offset: 0x0002C5FC
		private Vec3 Vector3Maxamize(Vec3 vector)
		{
			float num = 0f;
			num = ((vector.x > num) ? vector.x : num);
			num = ((vector.y > num) ? vector.y : num);
			num = ((vector.z > num) ? vector.z : num);
			return vector / num;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0002E450 File Offset: 0x0002C650
		public override void UpdateScreenPosition(Camera missionCamera)
		{
			Vec3 worldPosition = this.WorldPosition;
			worldPosition.z += this.HeightOffset;
			Vec3 vec = missionCamera.WorldPointToViewPortPoint(ref worldPosition);
			vec.y = 1f - vec.y;
			if (vec.z < 0f)
			{
				vec.x = 1f - vec.x;
				vec.y = 1f - vec.y;
				vec.z = 0f;
				vec = this.Vector3Maxamize(vec);
			}
			if (float.IsPositiveInfinity(vec.x))
			{
				vec.x = 1f;
			}
			else if (float.IsNegativeInfinity(vec.x))
			{
				vec.x = 0f;
			}
			if (float.IsPositiveInfinity(vec.y))
			{
				vec.y = 1f;
			}
			else if (float.IsNegativeInfinity(vec.y))
			{
				vec.y = 0f;
			}
			vec.x = MathF.Clamp(vec.x, 0f, 1f) * Screen.RealScreenResolutionWidth;
			vec.y = MathF.Clamp(vec.y, 0f, 1f) * Screen.RealScreenResolutionHeight;
			base.ScreenPosition = new Vec2(vec.x, vec.y);
			this.FlagProgress = this.TargetFlag.GetFlagProgress();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0002E5AC File Offset: 0x0002C7AC
		public void OnOwnerChanged(Team team)
		{
			bool flag = team == null || team.TeamIndex == -1;
			uint num = (flag ? 4284111450U : team.Color);
			uint num2 = (flag ? uint.MaxValue : team.Color2);
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0002E5ED File Offset: 0x0002C7ED
		public void OnRemainingMoraleChanged(int remainingMorale)
		{
			if (this.RemainingRemovalTime != remainingMorale && remainingMorale != 90)
			{
				this.RemainingRemovalTime = (int)((float)remainingMorale / 1f);
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0002E60C File Offset: 0x0002C80C
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x0002E614 File Offset: 0x0002C814
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

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002E632 File Offset: 0x0002C832
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0002E63A File Offset: 0x0002C83A
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

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002E658 File Offset: 0x0002C858
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0002E660 File Offset: 0x0002C860
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

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002E67E File Offset: 0x0002C87E
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0002E686 File Offset: 0x0002C886
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

		// Token: 0x040006E6 RID: 1766
		private bool _isKeepFlag;

		// Token: 0x040006E7 RID: 1767
		private bool _isSpawnAffectorFlag;

		// Token: 0x040006E8 RID: 1768
		private float _flagProgress;

		// Token: 0x040006E9 RID: 1769
		private int _remainingRemovalTime = -1;
	}
}
