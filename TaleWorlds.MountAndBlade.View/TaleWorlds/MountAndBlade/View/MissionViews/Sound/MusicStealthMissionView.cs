using System;
using System.Collections.Generic;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000088 RID: 136
	public class MusicStealthMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00026142 File Offset: 0x00024342
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00026148 File Offset: 0x00024348
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._cautiousAgents = new List<Agent>();
			this._patrollingCautiousAgents = new List<Agent>();
			this._detectedAgents = new List<Agent>();
			this._combatAgents = new List<Agent>();
			this._stealthNotificationSoundEvents = new Dictionary<Agent, SoundEvent>();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
			this._warningSoundState = MusicStealthMissionView.WarningSoundState.None;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000261B8 File Offset: 0x000243B8
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000261CE File Offset: 0x000243CE
		void IMusicHandler.OnUpdated(float dt)
		{
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000261D0 File Offset: 0x000243D0
		public override void AfterStart()
		{
			base.AfterStart();
			MBMusicManager.Current.StartTheme(MusicTheme.StealthA, 0.25f, false);
			PsaiCore.Instance.HoldCurrentIntensity(true);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000261F8 File Offset: 0x000243F8
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			object lockObject = MusicStealthMissionView._lockObject;
			lock (lockObject)
			{
				this._cautiousAgents.Remove(agent);
				this._patrollingCautiousAgents.Remove(agent);
				this._detectedAgents.Remove(agent);
				switch (flag)
				{
				case Agent.AIStateFlag.Cautious:
					this._cautiousAgents.Add(agent);
					break;
				case Agent.AIStateFlag.PatrollingCautious:
					this._patrollingCautiousAgents.Add(agent);
					break;
				case Agent.AIStateFlag.Alarmed:
					this._detectedAgents.Add(agent);
					break;
				}
				this.CheckIntensityChange();
				this.CheckWarningSoundStateChange(agent);
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000262A8 File Offset: 0x000244A8
		public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
			base.OnAgentHit(affectedAgent, affectorAgent, in affectorWeapon, in blow, in attackCollisionData);
			if (affectedAgent.IsMainAgent && affectorAgent != null && affectorAgent != affectedAgent && affectorAgent.IsEnemyOf(Agent.Main) && !this._combatAgents.Contains(affectorAgent))
			{
				this._combatAgents.Add(affectorAgent);
				this.CheckIntensityChange();
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00026300 File Offset: 0x00024500
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			this._cautiousAgents.Remove(affectedAgent);
			this._patrollingCautiousAgents.Remove(affectedAgent);
			this._detectedAgents.Remove(affectedAgent);
			this._combatAgents.Remove(affectedAgent);
			this.CheckIntensityChange();
			this.CheckWarningSoundStateChange(affectedAgent);
			if (this._stealthNotificationSoundEvents.ContainsKey(affectedAgent) && !affectedAgent.IsActive())
			{
				this._stealthNotificationSoundEvents.Remove(affectedAgent);
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0002637C File Offset: 0x0002457C
		private void CheckIntensityChange()
		{
			float num;
			if (this._combatAgents.Count > 0)
			{
				num = 1f;
			}
			else if (this._detectedAgents.Count > 0)
			{
				num = 0.75f;
			}
			else if (this._cautiousAgents.Count > 0 || this._patrollingCautiousAgents.Count > 0)
			{
				num = 0.5f;
			}
			else
			{
				num = 0.25f;
			}
			float num2 = num - PsaiCore.Instance.GetCurrentIntensity();
			if (Math.Abs(num2) > 1E-05f)
			{
				PsaiCore.Instance.HoldCurrentIntensity(false);
				PsaiCore.Instance.AddToCurrentIntensity(num2);
				PsaiCore.Instance.HoldCurrentIntensity(true);
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00026420 File Offset: 0x00024620
		private void CheckWarningSoundStateChange(Agent relatedAgent)
		{
			MusicStealthMissionView.WarningSoundState intendedWarningSoundState = this.GetIntendedWarningSoundState();
			if (intendedWarningSoundState != this._warningSoundState)
			{
				this.ChangeWarningSoundState(intendedWarningSoundState, relatedAgent);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00026445 File Offset: 0x00024645
		private MusicStealthMissionView.WarningSoundState GetIntendedWarningSoundState()
		{
			if (this._detectedAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.Alarmed;
			}
			if (this._patrollingCautiousAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.PatrollingCautious;
			}
			if (this._cautiousAgents.Count > 0)
			{
				return MusicStealthMissionView.WarningSoundState.Cautious;
			}
			return MusicStealthMissionView.WarningSoundState.Neutral;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00026478 File Offset: 0x00024678
		private void ChangeWarningSoundState(MusicStealthMissionView.WarningSoundState newState, Agent relatedAgent)
		{
			SoundEvent soundEvent;
			if (!this._stealthNotificationSoundEvents.TryGetValue(relatedAgent, out soundEvent))
			{
				soundEvent = SoundEvent.CreateEventFromString("event:/ui/stealth/stealth_notification_b", base.Mission.Scene);
				this._stealthNotificationSoundEvents.Add(relatedAgent, soundEvent);
			}
			this._warningSoundState = newState;
			soundEvent.SetPosition(relatedAgent.Position);
			float num = (float)this._warningSoundState;
			soundEvent.SetParameter("agent_state", num);
			if (!soundEvent.IsPlaying())
			{
				soundEvent.Play();
			}
		}

		// Token: 0x040002E1 RID: 737
		private const string StealthNotificationSoundEventId = "event:/ui/stealth/stealth_notification_b";

		// Token: 0x040002E2 RID: 738
		private const string AgentStateParameterName = "agent_state";

		// Token: 0x040002E3 RID: 739
		private static object _lockObject = new object();

		// Token: 0x040002E4 RID: 740
		private List<Agent> _cautiousAgents;

		// Token: 0x040002E5 RID: 741
		private List<Agent> _patrollingCautiousAgents;

		// Token: 0x040002E6 RID: 742
		private List<Agent> _detectedAgents;

		// Token: 0x040002E7 RID: 743
		private List<Agent> _combatAgents;

		// Token: 0x040002E8 RID: 744
		private Dictionary<Agent, SoundEvent> _stealthNotificationSoundEvents;

		// Token: 0x040002E9 RID: 745
		private MusicStealthMissionView.WarningSoundState _warningSoundState;

		// Token: 0x020000E7 RID: 231
		private enum WarningSoundState
		{
			// Token: 0x040003F2 RID: 1010
			None,
			// Token: 0x040003F3 RID: 1011
			Neutral,
			// Token: 0x040003F4 RID: 1012
			Cautious,
			// Token: 0x040003F5 RID: 1013
			PatrollingCautious,
			// Token: 0x040003F6 RID: 1014
			Alarmed
		}
	}
}
