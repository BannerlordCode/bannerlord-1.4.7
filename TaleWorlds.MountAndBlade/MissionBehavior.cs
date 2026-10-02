using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000267 RID: 615
	public abstract class MissionBehavior : IMissionBehavior
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x00078B8D File Offset: 0x00076D8D
		// (set) Token: 0x0600226F RID: 8815 RVA: 0x00078B95 File Offset: 0x00076D95
		public Mission Mission { get; internal set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x00078B9E File Offset: 0x00076D9E
		public IInputContext DebugInput
		{
			get
			{
				return Input.DebugInput;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002271 RID: 8817
		public abstract MissionBehaviorType BehaviorType { get; }

		// Token: 0x06002272 RID: 8818 RVA: 0x00078BA5 File Offset: 0x00076DA5
		public virtual void OnAfterMissionCreated()
		{
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00078BA7 File Offset: 0x00076DA7
		public virtual void OnBehaviorInitialize()
		{
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00078BA9 File Offset: 0x00076DA9
		public virtual void OnCreated()
		{
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00078BAB File Offset: 0x00076DAB
		public virtual void EarlyStart()
		{
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00078BAD File Offset: 0x00076DAD
		public virtual void AfterStart()
		{
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00078BAF File Offset: 0x00076DAF
		public virtual void OnMissileHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00078BB1 File Offset: 0x00076DB1
		public virtual void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00078BB3 File Offset: 0x00076DB3
		public virtual void OnMissileCollisionReaction(Mission.MissileCollisionReaction collisionReaction, Agent attackerAgent, Agent attachedAgent, sbyte attachedBoneIndex)
		{
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00078BB5 File Offset: 0x00076DB5
		public virtual void OnMissionScreenPreLoad()
		{
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00078BB7 File Offset: 0x00076DB7
		public virtual void OnAgentCreated(Agent agent)
		{
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00078BB9 File Offset: 0x00076DB9
		public virtual void OnAgentBuild(Agent agent, Banner banner)
		{
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x00078BBB File Offset: 0x00076DBB
		public virtual void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00078BBD File Offset: 0x00076DBD
		public virtual void OnAgentControllerSetToPlayer(Agent agent)
		{
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00078BBF File Offset: 0x00076DBF
		public virtual void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
		{
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x00078BC1 File Offset: 0x00076DC1
		public virtual void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x00078BC3 File Offset: 0x00076DC3
		public virtual void OnEarlyAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x00078BC5 File Offset: 0x00076DC5
		public virtual void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00078BC7 File Offset: 0x00076DC7
		public virtual void OnAgentDeleted(Agent affectedAgent)
		{
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00078BC9 File Offset: 0x00076DC9
		public virtual void OnAgentFleeing(Agent affectedAgent)
		{
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00078BCB File Offset: 0x00076DCB
		public virtual void OnAgentPanicked(Agent affectedAgent)
		{
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00078BCD File Offset: 0x00076DCD
		public virtual void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00078BCF File Offset: 0x00076DCF
		public virtual void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00078BD1 File Offset: 0x00076DD1
		public virtual void OnAddTeam(Team team)
		{
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00078BD3 File Offset: 0x00076DD3
		public virtual void AfterAddTeam(Team team)
		{
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00078BD5 File Offset: 0x00076DD5
		public virtual void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00078BD7 File Offset: 0x00076DD7
		public virtual void OnClearScene()
		{
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00078BD9 File Offset: 0x00076DD9
		public virtual void OnEndMissionInternal()
		{
			this.OnEndMission();
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00078BE1 File Offset: 0x00076DE1
		protected virtual void OnEndMission()
		{
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00078BE3 File Offset: 0x00076DE3
		public virtual void OnRemoveBehavior()
		{
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00078BE5 File Offset: 0x00076DE5
		public virtual void OnFixedMissionTick(float fixedDt)
		{
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00078BE7 File Offset: 0x00076DE7
		public virtual void OnPreMissionTick(float dt)
		{
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00078BE9 File Offset: 0x00076DE9
		public virtual void OnPreDisplayMissionTick(float dt)
		{
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x00078BEB File Offset: 0x00076DEB
		public virtual void OnMissionTick(float dt)
		{
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x00078BED File Offset: 0x00076DED
		public virtual void OnAgentMount(Agent agent)
		{
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00078BEF File Offset: 0x00076DEF
		public virtual void OnAgentDismount(Agent agent)
		{
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00078BF1 File Offset: 0x00076DF1
		public virtual bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return false;
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x00078BF4 File Offset: 0x00076DF4
		public virtual void OnEntityRemoved(GameEntity entity)
		{
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00078BF6 File Offset: 0x00076DF6
		public virtual void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x00078BF8 File Offset: 0x00076DF8
		public virtual void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usedObject)
		{
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x00078BFA File Offset: 0x00076DFA
		public virtual void OnRenderingStarted()
		{
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x00078BFC File Offset: 0x00076DFC
		public virtual void OnMissionStateActivated()
		{
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00078BFE File Offset: 0x00076DFE
		public virtual void OnMissionStateFinalized()
		{
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00078C00 File Offset: 0x00076E00
		public virtual void OnMissionStateDeactivated()
		{
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00078C02 File Offset: 0x00076E02
		public virtual List<CompassItemUpdateParams> GetCompassTargets()
		{
			return null;
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00078C05 File Offset: 0x00076E05
		public virtual void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00078C07 File Offset: 0x00076E07
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00078C09 File Offset: 0x00076E09
		public virtual void OnAfterDeploymentFinished()
		{
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x00078C0B File Offset: 0x00076E0B
		public virtual void OnTeamDeployed(Team team)
		{
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00078C0D File Offset: 0x00076E0D
		public virtual void OnBattleSideDeployed(BattleSideEnum side)
		{
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00078C0F File Offset: 0x00076E0F
		protected internal virtual void OnGetAgentState(Agent agent, bool usedSurgery)
		{
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00078C11 File Offset: 0x00076E11
		public virtual void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x00078C13 File Offset: 0x00076E13
		protected internal virtual void OnObjectDisabled(DestructableComponent destructionComponent)
		{
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00078C15 File Offset: 0x00076E15
		public virtual void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x00078C17 File Offset: 0x00076E17
		protected internal virtual void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00078C19 File Offset: 0x00076E19
		public virtual void OnRegisterBlow(Agent attacker, Agent victim, WeakGameEntity realHitEntity, Blow b, ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon)
		{
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x00078C1B File Offset: 0x00076E1B
		public virtual void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
		{
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00078C1D File Offset: 0x00076E1D
		public virtual void OnMissileRemoved(int MissileIndex)
		{
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x00078C1F File Offset: 0x00076E1F
		public virtual void OnTutorialCompleted(string completedTutorialIdentifier)
		{
		}
	}
}
