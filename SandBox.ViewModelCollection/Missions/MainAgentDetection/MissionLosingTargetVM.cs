using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000044 RID: 68
	public class MissionLosingTargetVM : ViewModel
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x00011957 File Offset: 0x0000FB57
		public MissionLosingTargetVM()
		{
			this.RefreshValues();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011965 File Offset: 0x0000FB65
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LosingTargetWarningText = new TextObject("{=kXy4R7ca}You are about to lose the target.", null).ToString();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011983 File Offset: 0x0000FB83
		public void UpdateLosingTargetValues(bool isLosingTarget, float losingTargetTimer, float losingTargetTreshold)
		{
			this.IsLosingTarget = isLosingTarget;
			this.LosingTargetRatio = MathF.Clamp(losingTargetTimer / losingTargetTreshold * 100f, 0f, 100f);
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000119AA File Offset: 0x0000FBAA
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x000119B2 File Offset: 0x0000FBB2
		[DataSourceProperty]
		public bool IsLosingTarget
		{
			get
			{
				return this._isLosingTarget;
			}
			set
			{
				if (value != this._isLosingTarget)
				{
					this._isLosingTarget = value;
					base.OnPropertyChangedWithValue(value, "IsLosingTarget");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000119D0 File Offset: 0x0000FBD0
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x000119D8 File Offset: 0x0000FBD8
		[DataSourceProperty]
		public float LosingTargetRatio
		{
			get
			{
				return this._losingTargetRatio;
			}
			set
			{
				if (value != this._losingTargetRatio)
				{
					this._losingTargetRatio = value;
					base.OnPropertyChangedWithValue(value, "LosingTargetRatio");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000119F6 File Offset: 0x0000FBF6
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x000119FE File Offset: 0x0000FBFE
		[DataSourceProperty]
		public string LosingTargetWarningText
		{
			get
			{
				return this._losingTargetWarningText;
			}
			set
			{
				if (value != this._losingTargetWarningText)
				{
					this._losingTargetWarningText = value;
					base.OnPropertyChangedWithValue<string>(value, "LosingTargetWarningText");
				}
			}
		}

		// Token: 0x0400023B RID: 571
		private bool _isLosingTarget;

		// Token: 0x0400023C RID: 572
		private float _losingTargetRatio;

		// Token: 0x0400023D RID: 573
		private string _losingTargetWarningText;
	}
}
