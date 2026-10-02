using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x0200003F RID: 63
	public class MissionStealthFailCounterVM : ViewModel
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x00011227 File Offset: 0x0000F427
		public MissionStealthFailCounterVM()
		{
			this._countDownTextObject = new TextObject("{=pY8lnL11}Mission will fail in: {SEC}", null);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00011240 File Offset: 0x0000F440
		public void UpdateFailCounter(float failCounterElapsedTime, float failCounterMaxTime, bool isStealthFailCounterMissionLogicActive)
		{
			this.IsCounterActive = !BannerlordConfig.HideBattleUI && !MBCommon.IsPaused && isStealthFailCounterMissionLogicActive && failCounterElapsedTime > 0f;
			this.FailCounterMaxTime = failCounterMaxTime;
			if (this.IsCounterActive)
			{
				this.FailCounterElapsedTime = this.FailCounterMaxTime - failCounterElapsedTime;
				this._countDownTextObject.SetTextVariable("SEC", MathF.Ceiling(this.FailCounterElapsedTime));
				this.CountDownText = this._countDownTextObject.ToString();
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x000112BE File Offset: 0x0000F4BE
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x000112C6 File Offset: 0x0000F4C6
		[DataSourceProperty]
		public string CountDownText
		{
			get
			{
				return this._countDownText;
			}
			set
			{
				if (value != this._countDownText)
				{
					this._countDownText = value;
					base.OnPropertyChangedWithValue<string>(value, "CountDownText");
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000112E9 File Offset: 0x0000F4E9
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x000112F1 File Offset: 0x0000F4F1
		[DataSourceProperty]
		public float FailCounterElapsedTime
		{
			get
			{
				return this._failCounterElapsedTime;
			}
			set
			{
				if (value != this._failCounterElapsedTime)
				{
					this._failCounterElapsedTime = value;
					base.OnPropertyChangedWithValue(value, "FailCounterElapsedTime");
				}
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001130F File Offset: 0x0000F50F
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00011317 File Offset: 0x0000F517
		[DataSourceProperty]
		public float FailCounterMaxTime
		{
			get
			{
				return this._failCounterMaxTime;
			}
			set
			{
				if (value != this._failCounterMaxTime)
				{
					this._failCounterMaxTime = value;
					base.OnPropertyChangedWithValue(value, "FailCounterMaxTime");
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00011335 File Offset: 0x0000F535
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0001133D File Offset: 0x0000F53D
		[DataSourceProperty]
		public bool IsCounterActive
		{
			get
			{
				return this._isCounterActive;
			}
			set
			{
				if (value != this._isCounterActive)
				{
					this._isCounterActive = value;
					base.OnPropertyChangedWithValue(value, "IsCounterActive");
				}
			}
		}

		// Token: 0x04000220 RID: 544
		private TextObject _countDownTextObject;

		// Token: 0x04000221 RID: 545
		private float _failCounterElapsedTime;

		// Token: 0x04000222 RID: 546
		private string _countDownText;

		// Token: 0x04000223 RID: 547
		private float _failCounterMaxTime;

		// Token: 0x04000224 RID: 548
		private bool _isCounterActive;
	}
}
