using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002E RID: 46
	public class MissionQuestBarVM : ViewModel
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x0000FCE0 File Offset: 0x0000DEE0
		public void UpdateQuestValues(float minDetectionLevel, float maxDetectionLevel, float currentDetectionLevel)
		{
			this.MinimumQuestLevel = minDetectionLevel;
			this.MaximumQuestLevel = maxDetectionLevel;
			this.CurrentQuestLevel = currentDetectionLevel;
			this.CurrentQuestLevelRatio = MBMath.InverseLerp(this.MinimumQuestLevel, this.MaximumQuestLevel, this.CurrentQuestLevel);
			this.HasQuestLevel = this.CurrentQuestLevel > 0f;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000FD32 File Offset: 0x0000DF32
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000FD3A File Offset: 0x0000DF3A
		[DataSourceProperty]
		public bool HasQuestLevel
		{
			get
			{
				return this._hasQuestLevel;
			}
			set
			{
				if (value != this._hasQuestLevel)
				{
					this._hasQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "HasQuestLevel");
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000FD58 File Offset: 0x0000DF58
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000FD60 File Offset: 0x0000DF60
		[DataSourceProperty]
		public float MinimumQuestLevel
		{
			get
			{
				return this._minimumQuestLevel;
			}
			set
			{
				if (value != this._minimumQuestLevel)
				{
					this._minimumQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "MinimumQuestLevel");
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000FD7E File Offset: 0x0000DF7E
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0000FD86 File Offset: 0x0000DF86
		[DataSourceProperty]
		public float MaximumQuestLevel
		{
			get
			{
				return this._maximumQuestLevel;
			}
			set
			{
				if (value != this._maximumQuestLevel)
				{
					this._maximumQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "MaximumQuestLevel");
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000FDAC File Offset: 0x0000DFAC
		[DataSourceProperty]
		public float CurrentQuestLevel
		{
			get
			{
				return this._currentQuestLevel;
			}
			set
			{
				if (value != this._currentQuestLevel)
				{
					this._currentQuestLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentQuestLevel");
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000FDCA File Offset: 0x0000DFCA
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0000FDD2 File Offset: 0x0000DFD2
		[DataSourceProperty]
		public float CurrentQuestLevelRatio
		{
			get
			{
				return this._currentQuestLevelRatio;
			}
			set
			{
				if (value != this._currentQuestLevelRatio)
				{
					this._currentQuestLevelRatio = value;
					base.OnPropertyChangedWithValue(value, "CurrentQuestLevelRatio");
				}
			}
		}

		// Token: 0x040001E3 RID: 483
		private bool _hasQuestLevel;

		// Token: 0x040001E4 RID: 484
		private float _minimumQuestLevel;

		// Token: 0x040001E5 RID: 485
		private float _maximumQuestLevel;

		// Token: 0x040001E6 RID: 486
		private float _currentQuestLevel;

		// Token: 0x040001E7 RID: 487
		private float _currentQuestLevelRatio;
	}
}
