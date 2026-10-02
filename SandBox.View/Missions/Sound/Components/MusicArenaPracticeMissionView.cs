using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions.Sound.Components
{
	// Token: 0x0200002E RID: 46
	public class MusicArenaPracticeMissionView : MissionView, IMusicHandler
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0001204B File Offset: 0x0001024B
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001204E File Offset: 0x0001024E
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00012075 File Offset: 0x00010275
		public override void EarlyStart()
		{
			this._allOneShotSoundEventsAreDisabled = false;
			this._arenaSoundEntity = base.Mission.Scene.FindEntityWithTag("arena_sound");
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000120A8 File Offset: 0x000102A8
		public override void OnMissionScreenFinalize()
		{
			SoundManager.SetGlobalParameter("ArenaIntensity", 0f);
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000120D0 File Offset: 0x000102D0
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			if (affectedAgent != null && affectorAgent != null && affectorAgent.IsMainAgent && (agentState == AgentState.Killed || agentState == AgentState.Unconscious))
			{
				if (affectedAgent.Team != affectorAgent.Team)
				{
					if (affectedAgent.IsHuman)
					{
						this._currentTournamentIntensity++;
						if (affectedAgent.HasMount)
						{
							this._currentTournamentIntensity++;
						}
						if (killingBlow.OverrideKillInfo == Agent.KillInfo.Headshot)
						{
							this._currentTournamentIntensity += 3;
						}
						if (killingBlow.IsMissile)
						{
							this._currentTournamentIntensity++;
						}
						else
						{
							this._currentTournamentIntensity += 2;
						}
					}
					else if (affectedAgent.RiderAgent != null)
					{
						this._currentTournamentIntensity += 3;
					}
				}
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00012198 File Offset: 0x00010398
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001219C File Offset: 0x0001039C
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && affectedAgent != null && affectorAgent.IsMainAgent && affectedAgent.IsHuman && affectedAgent.Position.Distance(affectorAgent.Position) >= 15f && (blow.VictimBodyPart == BoneBodyPartType.Head || blow.VictimBodyPart == BoneBodyPartType.Neck))
			{
				this._currentTournamentIntensity += 3;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00012201 File Offset: 0x00010401
		public override void OnMissileHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this._currentTournamentIntensity += 2;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00012239 File Offset: 0x00010439
		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (!isCanceled && attacker != null && victim != null && attacker.IsMainAgent && victim.IsHuman && collisionData.IsShieldBroken)
			{
				this._currentTournamentIntensity += 2;
				this.UpdateAudienceIntensity();
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00012274 File Offset: 0x00010474
		private void UpdateAudienceIntensity()
		{
			bool flag = false;
			if (this._currentTournamentIntensity > 60)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.High;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.High;
			}
			else if (this._currentTournamentIntensity > 30)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.Mid;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.Mid;
			}
			else if (this._currentTournamentIntensity <= 30)
			{
				flag = this._currentArenaIntensityLevel != MusicArenaPracticeMissionView.ArenaIntensityLevel.Low;
				this._currentArenaIntensityLevel = MusicArenaPracticeMissionView.ArenaIntensityLevel.Low;
			}
			if (flag)
			{
				SoundManager.SetGlobalParameter("ArenaIntensity", (float)this._currentArenaIntensityLevel);
			}
			if (!this._allOneShotSoundEventsAreDisabled)
			{
				this.Cheer();
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00012304 File Offset: 0x00010504
		private void Cheer()
		{
			string text = "event:/mission/ambient/arena/reaction";
			Vec3 globalPosition = this._arenaSoundEntity.GlobalPosition;
			SoundManager.StartOneShotEvent(text, in globalPosition);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001232A File Offset: 0x0001052A
		public void OnUpdated(float dt)
		{
		}

		// Token: 0x040000CB RID: 203
		private const string ArenaSoundTag = "arena_sound";

		// Token: 0x040000CC RID: 204
		private const string ArenaIntensityParameterId = "ArenaIntensity";

		// Token: 0x040000CD RID: 205
		private const string ArenaPositiveReactionsSoundId = "event:/mission/ambient/arena/reaction";

		// Token: 0x040000CE RID: 206
		private const int MainAgentKnocksDownAnOpponentBaseIntensityChange = 1;

		// Token: 0x040000CF RID: 207
		private const int MainAgentKnocksDownAnOpponentHeadShotIntensityChange = 3;

		// Token: 0x040000D0 RID: 208
		private const int MainAgentKnocksDownAnOpponentMountedTargetIntensityChange = 1;

		// Token: 0x040000D1 RID: 209
		private const int MainAgentKnocksDownAnOpponentRangedHitIntensityChange = 1;

		// Token: 0x040000D2 RID: 210
		private const int MainAgentKnocksDownAnOpponentMeleeHitIntensityChange = 2;

		// Token: 0x040000D3 RID: 211
		private const int MainAgentHeadShotFrom15MetersRangeIntensityChange = 3;

		// Token: 0x040000D4 RID: 212
		private const int MainAgentDismountsAnOpponentIntensityChange = 3;

		// Token: 0x040000D5 RID: 213
		private const int MainAgentBreaksAShieldIntensityChange = 2;

		// Token: 0x040000D6 RID: 214
		private int _currentTournamentIntensity;

		// Token: 0x040000D7 RID: 215
		private MusicArenaPracticeMissionView.ArenaIntensityLevel _currentArenaIntensityLevel;

		// Token: 0x040000D8 RID: 216
		private bool _allOneShotSoundEventsAreDisabled;

		// Token: 0x040000D9 RID: 217
		private GameEntity _arenaSoundEntity;

		// Token: 0x02000097 RID: 151
		private enum ArenaIntensityLevel
		{
			// Token: 0x040002F8 RID: 760
			None,
			// Token: 0x040002F9 RID: 761
			Low,
			// Token: 0x040002FA RID: 762
			Mid,
			// Token: 0x040002FB RID: 763
			High
		}
	}
}
