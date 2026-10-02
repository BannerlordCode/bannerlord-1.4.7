using System;
using SandBox.Missions.AgentBehaviors;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002C RID: 44
	public class MissionAgentAlarmTargetVM : ViewModel
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000F738 File Offset: 0x0000D938
		public bool HasCautiousness
		{
			get
			{
				return this.TargetAgent.AIStateFlags.HasAnyFlag(Agent.AIStateFlag.Alarmed) || this.AlarmedBehaviorGroup.AlarmFactor > 0f;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000F761 File Offset: 0x0000D961
		public AlarmedBehaviorGroup AlarmedBehaviorGroup
		{
			get
			{
				if (this._alarmedBehaviorGroupCache == null)
				{
					AgentNavigator agentNavigator = this.TargetAgent.GetComponent<CampaignAgentComponent>().AgentNavigator;
					this._alarmedBehaviorGroupCache = ((agentNavigator != null) ? agentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>() : null);
				}
				return this._alarmedBehaviorGroupCache;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000F793 File Offset: 0x0000D993
		public MissionAgentAlarmTargetVM(Agent agent, Action<MissionAgentAlarmTargetVM> onRemove)
		{
			this.TargetAgent = agent;
			this._onRemove = onRemove;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public void UpdateValues()
		{
			string agentAlarmState = MissionAgentAlarmTargetVM.GetAgentAlarmState(this.TargetAgent.AIStateFlags);
			AlarmedBehaviorGroup alarmedBehaviorGroup = this.AlarmedBehaviorGroup;
			float num = ((alarmedBehaviorGroup != null) ? alarmedBehaviorGroup.AlarmFactor : 0f);
			if (num > 1f)
			{
				num = MathF.Min(num, 2f);
				num -= 1f;
				num = MathF.Lerp(0.3f, 1f, num, 1E-05f);
			}
			if (!this.IsInVision || !this.IsStealthModeEnabled || ((float)this.AlarmProgress <= 0f && !this.IsMainAgentInVisibilityRange))
			{
				this.AlarmProgress = 0;
				this.AlarmState = MissionAgentAlarmTargetVM.AlarmStateEnum.Invalid.ToString();
				return;
			}
			this.AlarmState = agentAlarmState;
			this.AlarmProgress = (int)(num * 100f);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000F874 File Offset: 0x0000DA74
		private static string GetAgentAlarmState(Agent.AIStateFlag stateFlag)
		{
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Alarmed)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.Alarmed.ToString();
			}
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Cautious)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.Cautious.ToString();
			}
			if ((stateFlag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.PatrollingCautious)
			{
				return MissionAgentAlarmTargetVM.AlarmStateEnum.PatrollingCautious.ToString();
			}
			return MissionAgentAlarmTargetVM.AlarmStateEnum.None.ToString();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
		public void UpdateScreenPosition(Camera missionCamera)
		{
			Vec3 position = this.TargetAgent.Position;
			position.z += this.TargetAgent.GetEyeGlobalHeight() + 0.35f;
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(missionCamera, position, ref this._latestX, ref this._latestY, ref this._latestW);
			this._wPosAfterPositionCalculation = ((this._latestW < 0f) ? (-1f) : 1.1f);
			this.WSign = (int)this._wPosAfterPositionCalculation;
			this.ScreenPosition = new Vec2(this._latestX, this._latestY);
			int wsign = this.WSign;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000F990 File Offset: 0x0000DB90
		public void ExecuteRemove()
		{
			Action<MissionAgentAlarmTargetVM> onRemove = this._onRemove;
			if (onRemove == null)
			{
				return;
			}
			onRemove(this);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000F9A3 File Offset: 0x0000DBA3
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000F9AB File Offset: 0x0000DBAB
		[DataSourceProperty]
		public bool IsStealthModeEnabled
		{
			get
			{
				return this._isStealthModeEnabled;
			}
			set
			{
				if (value != this._isStealthModeEnabled)
				{
					this._isStealthModeEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsStealthModeEnabled");
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000F9C9 File Offset: 0x0000DBC9
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000F9D1 File Offset: 0x0000DBD1
		[DataSourceProperty]
		public bool IsMainAgentInVisibilityRange
		{
			get
			{
				return this._isMainAgentInVisibilityRange;
			}
			set
			{
				if (value != this._isMainAgentInVisibilityRange)
				{
					this._isMainAgentInVisibilityRange = value;
					base.OnPropertyChangedWithValue(value, "IsMainAgentInVisibilityRange");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000F9EF File Offset: 0x0000DBEF
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000F9F7 File Offset: 0x0000DBF7
		[DataSourceProperty]
		public bool IsInVision
		{
			get
			{
				return this._isInVision;
			}
			set
			{
				if (value != this._isInVision)
				{
					this._isInVision = value;
					base.OnPropertyChangedWithValue(value, "IsInVision");
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000FA15 File Offset: 0x0000DC15
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000FA1D File Offset: 0x0000DC1D
		[DataSourceProperty]
		public bool IsSuspected
		{
			get
			{
				return this._isSuspected;
			}
			set
			{
				if (value != this._isSuspected)
				{
					this._isSuspected = value;
					base.OnPropertyChangedWithValue(value, "IsSuspected");
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000FA3B File Offset: 0x0000DC3B
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000FA43 File Offset: 0x0000DC43
		[DataSourceProperty]
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
					base.OnPropertyChangedWithValue(value, "AlarmProgress");
				}
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000FA61 File Offset: 0x0000DC61
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000FA69 File Offset: 0x0000DC69
		[DataSourceProperty]
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
					base.OnPropertyChangedWithValue<string>(value, "AlarmState");
				}
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000FA8C File Offset: 0x0000DC8C
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000FA94 File Offset: 0x0000DC94
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000FAB2 File Offset: 0x0000DCB2
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000FABA File Offset: 0x0000DCBA
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x040001CE RID: 462
		public readonly Agent TargetAgent;

		// Token: 0x040001CF RID: 463
		private readonly Action<MissionAgentAlarmTargetVM> _onRemove;

		// Token: 0x040001D0 RID: 464
		private float _latestX;

		// Token: 0x040001D1 RID: 465
		private float _latestY;

		// Token: 0x040001D2 RID: 466
		private float _latestW;

		// Token: 0x040001D3 RID: 467
		private float _wPosAfterPositionCalculation;

		// Token: 0x040001D4 RID: 468
		private AlarmedBehaviorGroup _alarmedBehaviorGroupCache;

		// Token: 0x040001D5 RID: 469
		private bool _isStealthModeEnabled;

		// Token: 0x040001D6 RID: 470
		private bool _isMainAgentInVisibilityRange;

		// Token: 0x040001D7 RID: 471
		private bool _isInVision;

		// Token: 0x040001D8 RID: 472
		private bool _isSuspected;

		// Token: 0x040001D9 RID: 473
		private string _alarmState;

		// Token: 0x040001DA RID: 474
		private int _wSign;

		// Token: 0x040001DB RID: 475
		private int _alarmProgress;

		// Token: 0x040001DC RID: 476
		private Vec2 _screenPosition;

		// Token: 0x0200009A RID: 154
		private enum AlarmStateEnum
		{
			// Token: 0x040003AB RID: 939
			Invalid = -1,
			// Token: 0x040003AC RID: 940
			None,
			// Token: 0x040003AD RID: 941
			Default,
			// Token: 0x040003AE RID: 942
			Cautious,
			// Token: 0x040003AF RID: 943
			PatrollingCautious,
			// Token: 0x040003B0 RID: 944
			Alarmed
		}
	}
}
