using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000041 RID: 65
	public class MainAgentDetectionVM : ViewModel
	{
		// Token: 0x06000436 RID: 1078 RVA: 0x000113BA File Offset: 0x0000F5BA
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SuspicionFullText = new TextObject("{=KgTFCWG8}You are suspicious", null).ToString();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000113D8 File Offset: 0x0000F5D8
		public void UpdateDetectionValues(float minDetectionLevel, float maxDetectionLevel, float currentDetectionLevel)
		{
			this.MinimumDetectionLevel = minDetectionLevel;
			this.MaximumDetectionLevel = maxDetectionLevel;
			this.CurrentDetectionLevel = currentDetectionLevel;
			this.CurrentDetectionLevelRatio = MBMath.InverseLerp(this.MinimumDetectionLevel, this.MaximumDetectionLevel, this.CurrentDetectionLevel);
			this.HasDetection = this.CurrentDetectionLevel > 0f;
			this.HasReachedSuspicionTreshold = this.CurrentDetectionLevel >= this.MaximumDetectionLevel;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00011441 File Offset: 0x0000F641
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00011449 File Offset: 0x0000F649
		[DataSourceProperty]
		public bool HasDetection
		{
			get
			{
				return this._hasDetection;
			}
			set
			{
				if (value != this._hasDetection)
				{
					this._hasDetection = value;
					base.OnPropertyChangedWithValue(value, "HasDetection");
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00011467 File Offset: 0x0000F667
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0001146F File Offset: 0x0000F66F
		[DataSourceProperty]
		public bool HasReachedSuspicionTreshold
		{
			get
			{
				return this._hasReachedSuspicionTreshold;
			}
			set
			{
				if (value != this._hasReachedSuspicionTreshold)
				{
					this._hasReachedSuspicionTreshold = value;
					base.OnPropertyChangedWithValue(value, "HasReachedSuspicionTreshold");
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001148D File Offset: 0x0000F68D
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00011495 File Offset: 0x0000F695
		[DataSourceProperty]
		public float MinimumDetectionLevel
		{
			get
			{
				return this._minimumDetectionLevel;
			}
			set
			{
				if (value != this._minimumDetectionLevel)
				{
					this._minimumDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "MinimumDetectionLevel");
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000114B3 File Offset: 0x0000F6B3
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x000114BB File Offset: 0x0000F6BB
		[DataSourceProperty]
		public float MaximumDetectionLevel
		{
			get
			{
				return this._maximumDetectionLevel;
			}
			set
			{
				if (value != this._maximumDetectionLevel)
				{
					this._maximumDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "MaximumDetectionLevel");
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x000114D9 File Offset: 0x0000F6D9
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x000114E1 File Offset: 0x0000F6E1
		[DataSourceProperty]
		public float CurrentDetectionLevel
		{
			get
			{
				return this._currentDetectionLevel;
			}
			set
			{
				if (value != this._currentDetectionLevel)
				{
					this._currentDetectionLevel = value;
					base.OnPropertyChangedWithValue(value, "CurrentDetectionLevel");
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000114FF File Offset: 0x0000F6FF
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00011507 File Offset: 0x0000F707
		[DataSourceProperty]
		public float CurrentDetectionLevelRatio
		{
			get
			{
				return this._currentDetectionLevelRatio;
			}
			set
			{
				if (value != this._currentDetectionLevelRatio)
				{
					this._currentDetectionLevelRatio = value;
					base.OnPropertyChangedWithValue(value, "CurrentDetectionLevelRatio");
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00011525 File Offset: 0x0000F725
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0001152D File Offset: 0x0000F72D
		[DataSourceProperty]
		public string SuspicionFullText
		{
			get
			{
				return this._suspicionFullText;
			}
			set
			{
				if (value != this._suspicionFullText)
				{
					this._suspicionFullText = value;
					base.OnPropertyChangedWithValue<string>(value, "SuspicionFullText");
				}
			}
		}

		// Token: 0x04000225 RID: 549
		private bool _hasDetection;

		// Token: 0x04000226 RID: 550
		private bool _hasReachedSuspicionTreshold;

		// Token: 0x04000227 RID: 551
		private float _minimumDetectionLevel;

		// Token: 0x04000228 RID: 552
		private float _maximumDetectionLevel;

		// Token: 0x04000229 RID: 553
		private float _currentDetectionLevel;

		// Token: 0x0400022A RID: 554
		private float _currentDetectionLevelRatio;

		// Token: 0x0400022B RID: 555
		private string _suspicionFullText;
	}
}
