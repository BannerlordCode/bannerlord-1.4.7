using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000258 RID: 600
	public abstract class MissionObject : ScriptComponentBehavior
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x00077BBE File Offset: 0x00075DBE
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00077BC5 File Offset: 0x00075DC5
		// (set) Token: 0x0600221D RID: 8733 RVA: 0x00077BCD File Offset: 0x00075DCD
		public MissionObjectId Id { get; set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x00077BD6 File Offset: 0x00075DD6
		// (set) Token: 0x0600221F RID: 8735 RVA: 0x00077BDE File Offset: 0x00075DDE
		public bool IsDisabled { get; private set; }

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x00077BE7 File Offset: 0x00075DE7
		public virtual TextObject HitObjectName { get; }

		// Token: 0x06002221 RID: 8737 RVA: 0x00077BF0 File Offset: 0x00075DF0
		public MissionObject()
		{
			MissionObjectId missionObjectId = new MissionObjectId(-1, false);
			this.Id = missionObjectId;
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x00077C20 File Offset: 0x00075E20
		public virtual void SetAbilityOfFaces(bool enabled)
		{
			if (this.DynamicNavmeshIdStart > 0)
			{
				for (int i = this.DynamicNavmeshIdStart; i < this.DynamicNavmeshIdStart + 10; i++)
				{
					base.GameEntity.Scene.SetAbilityOfFacesWithId(i, enabled);
				}
			}
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00077C68 File Offset: 0x00075E68
		protected void SetAbilityOfConditionalFaces(bool enabled)
		{
			if (this.DynamicNavmeshIdStart > 0)
			{
				base.GameEntity.Scene.SetAbilityOfFacesWithId(this.DynamicNavmeshIdStart + 8, enabled);
			}
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00077C9C File Offset: 0x00075E9C
		protected internal override void OnInit()
		{
			base.OnInit();
			if (!GameNetwork.IsClientOrReplay)
			{
				this.AttachDynamicNavmeshToEntity();
				this.SetAbilityOfFaces(base.GameEntity.IsValid && base.GameEntity.IsVisibleIncludeParents());
				this.SetAbilityOfConditionalFaces(base.GameEntity.IsValid && base.GameEntity.IsVisibleIncludeParents());
			}
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00077D0C File Offset: 0x00075F0C
		protected virtual void AttachDynamicNavmeshToEntity()
		{
			if (this.NavMeshPrefabName.Length > 0)
			{
				this.DynamicNavmeshIdStart = Mission.Current.GetNextDynamicNavMeshIdStart();
				base.GameEntity.Scene.ImportNavigationMeshPrefab(this.NavMeshPrefabName, this.DynamicNavmeshIdStart);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 1, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 2, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 3, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 4, false, true, false, false, false);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 8, false, true, false, true, true);
				this.SetAbilityOfFaces(base.GameEntity.IsValid && base.GameEntity.GetPhysicsState());
			}
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x00077E04 File Offset: 0x00076004
		protected virtual WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return base.GameEntity;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00077E0C File Offset: 0x0007600C
		protected internal override bool OnCheckForProblems()
		{
			base.OnCheckForProblems();
			bool flag = false;
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			list.Add(base.GameEntity);
			base.GameEntity.GetChildrenRecursive(ref list);
			bool flag2 = false;
			foreach (WeakGameEntity weakGameEntity in list)
			{
				flag2 = flag2 || (weakGameEntity.HasPhysicsDefinitionWithoutFlags(1) && !weakGameEntity.PhysicsDescBodyFlag.HasAnyFlag(BodyFlags.CommonCollisionExcludeFlagsForMissile));
			}
			Vec3 scaleVector = base.GameEntity.GetGlobalFrame().rotation.GetScaleVector();
			bool flag3 = MathF.Abs(scaleVector.x - scaleVector.y) >= 0.01f || MathF.Abs(scaleVector.x - scaleVector.z) >= 0.01f;
			if (flag2 && flag3)
			{
				MBEditor.AddEntityWarning(base.GameEntity, "Mission object has non-uniform scale and physics object. This is not supported because any attached focusable item to this mesh will not work within this configuration.");
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x00077F1C File Offset: 0x0007611C
		protected internal override void OnPreInit()
		{
			base.OnPreInit();
			if (this.Mission != null)
			{
				int num = -1;
				bool flag;
				if (this.Mission.IsLoadingFinished)
				{
					flag = true;
					if (!GameNetwork.IsClientOrReplay)
					{
						num = this.Mission.GetFreeRuntimeMissionObjectId();
					}
				}
				else
				{
					flag = false;
					num = this.Mission.GetFreeSceneMissionObjectId();
				}
				this.Id = new MissionObjectId(num, flag);
				this.Mission.AddActiveMissionObject(this);
			}
			base.GameEntity.SetAsReplayEntity();
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("batched_physics_entity");
			if (firstChildEntityWithTag != WeakGameEntity.Invalid)
			{
				firstChildEntityWithTag.CreateVariableRatePhysics(true);
			}
			string name = base.GameEntity.Name;
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				BodyFlags bodyFlag = weakGameEntity.BodyFlag;
				if (weakGameEntity != firstChildEntityWithTag)
				{
					weakGameEntity.CreateVariableRatePhysics(true);
				}
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0007802C File Offset: 0x0007622C
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x0007804D File Offset: 0x0007624D
		protected internal virtual void OnMissionReset()
		{
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x0007804F File Offset: 0x0007624F
		public virtual void AfterMissionStart()
		{
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00078051 File Offset: 0x00076251
		public virtual void OnMissionEnded()
		{
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00078053 File Offset: 0x00076253
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00078055 File Offset: 0x00076255
		protected internal virtual bool OnHit(Agent attackerAgent, int damage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ScriptComponentBehavior attackerScriptComponentBehavior, out bool reportDamage, out float finalDamage, out float fireDamage, out float modifiedFireDamage)
		{
			reportDamage = false;
			finalDamage = (float)damage;
			fireDamage = -1f;
			modifiedFireDamage = -1f;
			return false;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x00078074 File Offset: 0x00076274
		public void SetEnabled(bool isParentObject = false)
		{
			if (this.IsDisabled)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.SetAbilityOfFaces(true);
				}
				if (isParentObject && base.GameEntity != null)
				{
					List<WeakGameEntity> list = new List<WeakGameEntity>();
					base.GameEntity.GetChildrenRecursive(ref list);
					foreach (MissionObject missionObject in from sc in list.SelectMany<WeakGameEntity, ScriptComponentBehavior>((WeakGameEntity ac) => ac.GetScriptComponents())
						where sc is MissionObject
						select sc as MissionObject)
					{
						missionObject.SetEnabled(false);
					}
				}
				Mission.Current.ActivateMissionObject(this);
				this.IsDisabled = false;
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00078180 File Offset: 0x00076380
		public void SetEnabledAndMakeVisible(bool isParentObject = false, bool enableFaces = false)
		{
			this.SetEnabledAndMakeVisibleAux(isParentObject, enableFaces);
			base.SetScriptComponentToTick(this.GetTickRequirement());
			if (base.GameEntity != null)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (WeakGameEntity weakGameEntity in list)
				{
					int scriptCount = weakGameEntity.GetScriptCount();
					for (int i = 0; i < scriptCount; i++)
					{
						ScriptComponentBehavior scriptAtIndex = weakGameEntity.GetScriptAtIndex(i);
						if (scriptAtIndex != null)
						{
							scriptAtIndex.SetScriptComponentToTick(scriptAtIndex.GetTickRequirement());
						}
					}
				}
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00078238 File Offset: 0x00076438
		private void SetEnabledAndMakeVisibleAux(bool isParentObject, bool enableFaces)
		{
			if (enableFaces && !GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(true);
			}
			if (isParentObject && base.GameEntity != null)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
				{
					missionObject.SetEnabledAndMakeVisibleAux(false, enableFaces);
				}
			}
			Mission.Current.ActivateMissionObject(this);
			this.IsDisabled = false;
			if (base.GameEntity != null)
			{
				base.GameEntity.SetVisibilityExcludeParents(true);
				base.GameEntity.SetPhysicsState(true, false);
			}
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00078318 File Offset: 0x00076518
		public void SetDisabled(bool isParentObject = false)
		{
			if (!this.IsDisabled)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.SetAbilityOfFaces(false);
				}
				if (isParentObject && base.GameEntity.IsValid)
				{
					List<WeakGameEntity> list = new List<WeakGameEntity>();
					base.GameEntity.GetChildrenRecursive(ref list);
					foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
					{
						missionObject.SetDisabled(false);
					}
				}
				Mission.Current.DeactivateMissionObject(this);
				this.IsDisabled = true;
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000783D8 File Offset: 0x000765D8
		public void SetDisabledAndMakeInvisible(bool isParentObject = false, bool disableFaces = false)
		{
			if (disableFaces && !GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(false);
			}
			if (isParentObject && base.GameEntity.IsValid)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				foreach (MissionObject missionObject in list.SelectMany<WeakGameEntity, MissionObject>((WeakGameEntity ac) => ac.GetScriptComponents<MissionObject>()))
				{
					missionObject.SetDisabledAndMakeInvisible(false, disableFaces);
				}
			}
			Mission.Current.DeactivateMissionObject(this);
			this.IsDisabled = true;
			if (base.GameEntity.IsValid)
			{
				base.GameEntity.SetVisibilityExcludeParents(false);
				base.GameEntity.SetPhysicsState(false, false);
				base.SetScriptComponentToTick(this.GetTickRequirement());
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000784C8 File Offset: 0x000766C8
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			if (!GameNetwork.IsClientOrReplay)
			{
				this.SetAbilityOfFaces(false);
			}
			if (this.Mission != null)
			{
				this.Mission.OnMissionObjectRemoved(this, removeReason);
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000784F5 File Offset: 0x000766F5
		public virtual void OnEndMission()
		{
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x000784F7 File Offset: 0x000766F7
		public bool CreatedAtRuntime
		{
			get
			{
				return this.Id.CreatedAtRuntime;
			}
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00078504 File Offset: 0x00076704
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x00078508 File Offset: 0x00076708
		public virtual void AddStuckMissile(GameEntity missileEntity)
		{
			base.GameEntity.AddChild(missileEntity.WeakEntity, false);
		}

		// Token: 0x04000D45 RID: 3397
		public const int MaxNavMeshPerDynamicObject = 50;

		// Token: 0x04000D48 RID: 3400
		[EditableScriptComponentVariable(true, "")]
		protected string NavMeshPrefabName = "";

		// Token: 0x04000D49 RID: 3401
		protected int DynamicNavmeshIdStart;

		// Token: 0x0200053F RID: 1343
		protected enum DynamicNavmeshLocalIds
		{
			// Token: 0x04001D84 RID: 7556
			Inside = 1,
			// Token: 0x04001D85 RID: 7557
			Enter,
			// Token: 0x04001D86 RID: 7558
			Exit,
			// Token: 0x04001D87 RID: 7559
			Blocker,
			// Token: 0x04001D88 RID: 7560
			Extra1,
			// Token: 0x04001D89 RID: 7561
			Extra2,
			// Token: 0x04001D8A RID: 7562
			Extra3,
			// Token: 0x04001D8B RID: 7563
			ConditionalBlocker,
			// Token: 0x04001D8C RID: 7564
			Reserved1,
			// Token: 0x04001D8D RID: 7565
			Count
		}
	}
}
