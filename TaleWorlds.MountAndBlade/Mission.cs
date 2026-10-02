using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.MountAndBlade.Network;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C1 RID: 449
	public sealed class Mission : DotNetObject, IMission
	{
		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x0005362B File Offset: 0x0005182B
		// (set) Token: 0x0600192A RID: 6442 RVA: 0x00053633 File Offset: 0x00051833
		internal UIntPtr Pointer { get; private set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x0005363C File Offset: 0x0005183C
		public bool IsFinalized
		{
			get
			{
				return this.Pointer == UIntPtr.Zero;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x0005364E File Offset: 0x0005184E
		// (set) Token: 0x0600192D RID: 6445 RVA: 0x0005365B File Offset: 0x0005185B
		public static Mission Current
		{
			get
			{
				Mission current = Mission._current;
				return Mission._current;
			}
			private set
			{
				if (value == null)
				{
					Mission current = Mission._current;
				}
				Mission._current = value;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0005366C File Offset: 0x0005186C
		// (set) Token: 0x0600192F RID: 6447 RVA: 0x00053674 File Offset: 0x00051874
		private MissionInitializerRecord InitializerRecord { get; set; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001930 RID: 6448 RVA: 0x0005367D File Offset: 0x0005187D
		public string SceneName
		{
			get
			{
				return this.InitializerRecord.SceneName;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0005368A File Offset: 0x0005188A
		public string SceneLevels
		{
			get
			{
				return this.InitializerRecord.SceneLevels;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x00053697 File Offset: 0x00051897
		public float DamageToPlayerMultiplier
		{
			get
			{
				return BannerlordConfig.GetDamageToPlayerMultiplier();
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001933 RID: 6451 RVA: 0x0005369E File Offset: 0x0005189E
		public float DamageToFriendsMultiplier
		{
			get
			{
				return this.InitializerRecord.DamageToFriendsMultiplier;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x000536AB File Offset: 0x000518AB
		public float DamageFromPlayerToFriendsMultiplier
		{
			get
			{
				return this.InitializerRecord.DamageFromPlayerToFriendsMultiplier;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001935 RID: 6453 RVA: 0x000536B8 File Offset: 0x000518B8
		public bool HasValidTerrainType
		{
			get
			{
				return this.InitializerRecord.TerrainType >= 0;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001936 RID: 6454 RVA: 0x000536CB File Offset: 0x000518CB
		public TerrainType TerrainType
		{
			get
			{
				if (!this.HasValidTerrainType)
				{
					return TerrainType.Water;
				}
				return (TerrainType)this.InitializerRecord.TerrainType;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x000536E3 File Offset: 0x000518E3
		// (set) Token: 0x06001938 RID: 6456 RVA: 0x000536EB File Offset: 0x000518EB
		public Scene Scene { get; private set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x000536F4 File Offset: 0x000518F4
		// (set) Token: 0x0600193A RID: 6458 RVA: 0x000536FC File Offset: 0x000518FC
		public Vec3 CustomCameraTargetLocalOffset { get; private set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x00053705 File Offset: 0x00051905
		// (set) Token: 0x0600193C RID: 6460 RVA: 0x0005370D File Offset: 0x0005190D
		public Vec3 CustomCameraLocalOffset { get; private set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00053716 File Offset: 0x00051916
		// (set) Token: 0x0600193E RID: 6462 RVA: 0x0005371E File Offset: 0x0005191E
		public Vec3 CustomCameraLocalOffset2 { get; private set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00053727 File Offset: 0x00051927
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x0005372F File Offset: 0x0005192F
		public Vec3 CustomCameraGlobalOffset { get; private set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x00053738 File Offset: 0x00051938
		// (set) Token: 0x06001942 RID: 6466 RVA: 0x00053740 File Offset: 0x00051940
		public Vec3 CustomCameraLocalRotationalOffset { get; private set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x00053749 File Offset: 0x00051949
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x00053751 File Offset: 0x00051951
		public bool CustomCameraIgnoreCollision { get; private set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x0005375A File Offset: 0x0005195A
		// (set) Token: 0x06001946 RID: 6470 RVA: 0x00053762 File Offset: 0x00051962
		public float CustomCameraFovMultiplier { get; private set; } = 1f;

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x0005376B File Offset: 0x0005196B
		// (set) Token: 0x06001948 RID: 6472 RVA: 0x00053773 File Offset: 0x00051973
		public float CustomCameraFixedDistance { get; private set; } = float.MinValue;

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x0005377C File Offset: 0x0005197C
		// (set) Token: 0x0600194A RID: 6474 RVA: 0x00053784 File Offset: 0x00051984
		public float ListenerAndAttenuationPosBlendFactor { get; private set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x0005378D File Offset: 0x0005198D
		// (set) Token: 0x0600194C RID: 6476 RVA: 0x00053795 File Offset: 0x00051995
		public GameEntity IgnoredEntityForCamera { get; private set; }

		// Token: 0x0600194D RID: 6477 RVA: 0x000537A0 File Offset: 0x000519A0
		public IEnumerable<WeakGameEntity> GetActiveEntitiesWithScriptComponentOfType<T>()
		{
			return from amo in this._activeMissionObjects
				where amo is T
				select amo.GameEntity;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x000537FB File Offset: 0x000519FB
		public void AddActiveMissionObject(MissionObject missionObject)
		{
			this._missionObjects.Add(missionObject);
			this._activeMissionObjects.Add(missionObject);
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00053815 File Offset: 0x00051A15
		public void ActivateMissionObject(MissionObject missionObject)
		{
			this._activeMissionObjects.Add(missionObject);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00053823 File Offset: 0x00051A23
		public void DeactivateMissionObject(MissionObject missionObject)
		{
			this._activeMissionObjects.Remove(missionObject);
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x00053832 File Offset: 0x00051A32
		public MBReadOnlyList<MissionObject> ActiveMissionObjects
		{
			get
			{
				return this._activeMissionObjects;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x0005383A File Offset: 0x00051A3A
		public MBReadOnlyList<MissionObject> MissionObjects
		{
			get
			{
				return this._missionObjects;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x00053842 File Offset: 0x00051A42
		public MBReadOnlyList<Mission.DynamicallyCreatedEntity> AddedEntitiesInfo
		{
			get
			{
				return this._addedEntitiesInfo;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x0005384A File Offset: 0x00051A4A
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00053852 File Offset: 0x00051A52
		public Mission.MBBoundaryCollection Boundaries { get; private set; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0005385C File Offset: 0x00051A5C
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x000538B8 File Offset: 0x00051AB8
		public bool IsMainAgentObjectInteractionEnabled
		{
			get
			{
				switch (this._missionMode)
				{
				case MissionMode.Conversation:
				case MissionMode.Barter:
				case MissionMode.Deployment:
				case MissionMode.Replay:
				case MissionMode.CutScene:
					return false;
				}
				return (this.IsNavalBattle || !this.MissionEnded) && this._isMainAgentObjectInteractionEnabled;
			}
			set
			{
				this._isMainAgentObjectInteractionEnabled = value;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x000538C4 File Offset: 0x00051AC4
		// (set) Token: 0x06001959 RID: 6489 RVA: 0x0005390E File Offset: 0x00051B0E
		public bool IsMainAgentItemInteractionEnabled
		{
			get
			{
				switch (this._missionMode)
				{
				case MissionMode.Conversation:
				case MissionMode.Barter:
				case MissionMode.Deployment:
				case MissionMode.Replay:
				case MissionMode.CutScene:
					return false;
				}
				return this._isMainAgentItemInteractionEnabled;
			}
			set
			{
				this._isMainAgentItemInteractionEnabled = value;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x00053917 File Offset: 0x00051B17
		// (set) Token: 0x0600195B RID: 6491 RVA: 0x0005391F File Offset: 0x00051B1F
		public bool IsTeleportingAgents { get; set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00053928 File Offset: 0x00051B28
		// (set) Token: 0x0600195D RID: 6493 RVA: 0x00053930 File Offset: 0x00051B30
		public bool ForceTickOccasionally { get; set; }

		// Token: 0x0600195E RID: 6494 RVA: 0x00053939 File Offset: 0x00051B39
		private void FinalizeMission()
		{
			TeamAISiegeComponent.OnMissionFinalize();
			MBAPI.IMBMission.FinalizeMission(this.Pointer);
			this.Pointer = UIntPtr.Zero;
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0005395B File Offset: 0x00051B5B
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x0005396D File Offset: 0x00051B6D
		public Mission.MissionCombatType CombatType
		{
			get
			{
				return (Mission.MissionCombatType)MBAPI.IMBMission.GetCombatType(this.Pointer);
			}
			set
			{
				MBAPI.IMBMission.SetCombatType(this.Pointer, (int)value);
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00053980 File Offset: 0x00051B80
		public void SetMissionCombatType(Mission.MissionCombatType missionCombatType)
		{
			MBAPI.IMBMission.SetCombatType(this.Pointer, (int)missionCombatType);
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x00053993 File Offset: 0x00051B93
		public MissionMode Mode
		{
			get
			{
				return this._missionMode;
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0005399C File Offset: 0x00051B9C
		public void ConversationCharacterChanged()
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnConversationCharacterChanged();
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x000539EC File Offset: 0x00051BEC
		public void SetMissionMode(MissionMode newMode, bool atStart)
		{
			if (this._missionMode != newMode)
			{
				MissionMode missionMode = this._missionMode;
				this._missionMode = newMode;
				if (this.CurrentState != Mission.State.Over)
				{
					for (int i = 0; i < this.MissionBehaviors.Count; i++)
					{
						this.MissionBehaviors[i].OnMissionModeChange(missionMode, atStart);
					}
					foreach (IMissionListener missionListener in this._listeners)
					{
						missionListener.OnMissionModeChange(missionMode, atStart);
					}
				}
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00053A88 File Offset: 0x00051C88
		private Mission.AgentCreationResult CreateAgentInternal(AgentFlag agentFlags, int forcedAgentIndex, bool isFemale, ref AgentSpawnData spawnData, ref AgentCapsuleData capsuleData, ref AnimationSystemData animationSystemData, int instanceNo)
		{
			return MBAPI.IMBMission.CreateAgent(this.Pointer, (ulong)agentFlags, forcedAgentIndex, isFemale, ref spawnData, ref capsuleData.BodyCap, ref capsuleData.CrouchedBodyCap, ref animationSystemData, instanceNo);
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x00053ABD File Offset: 0x00051CBD
		public float CurrentTime
		{
			get
			{
				return this._cachedMissionTime;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00053AC5 File Offset: 0x00051CC5
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x00053AD7 File Offset: 0x00051CD7
		public bool PauseAITick
		{
			get
			{
				return MBAPI.IMBMission.GetPauseAITick(this.Pointer);
			}
			set
			{
				MBAPI.IMBMission.SetPauseAITick(this.Pointer, value);
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00053AEA File Offset: 0x00051CEA
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void UpdateMissionTimeCache(float curTime)
		{
			this._cachedMissionTime = curTime;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00053AF3 File Offset: 0x00051CF3
		public float GetAverageFps()
		{
			return MBAPI.IMBMission.GetAverageFps(this.Pointer);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00053B05 File Offset: 0x00051D05
		public bool GetFallAvoidSystemActive()
		{
			return MBAPI.IMBMission.GetFallAvoidSystemActive(this.Pointer);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00053B17 File Offset: 0x00051D17
		public void SetFallAvoidSystemActive(bool fallAvoidActive)
		{
			MBAPI.IMBMission.SetFallAvoidSystemActive(this.Pointer, fallAvoidActive);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00053B2A File Offset: 0x00051D2A
		public bool IsPositionInsideBoundaries(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideBoundaries(this.Pointer, position);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00053B3D File Offset: 0x00051D3D
		public bool IsPositionInsideHardBoundaries(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideHardBoundaries(this.Pointer, position);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00053B50 File Offset: 0x00051D50
		public bool IsPositionInsideAnyBlockerNavMeshFace2D(Vec2 position)
		{
			return MBAPI.IMBMission.IsPositionInsideAnyBlockerNavMeshFace2D(this.Pointer, position);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00053B63 File Offset: 0x00051D63
		public bool IsPositionOnAnyBlockerNavMeshFace(Vec3 position)
		{
			return MBAPI.IMBMission.IsPositionOnAnyBlockerNavMeshFace(this.Pointer, position);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00053B78 File Offset: 0x00051D78
		private bool IsFormationUnitPositionAvailableAuxMT(ref WorldPosition formationPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance)
		{
			bool flag;
			using (new TWSharedMutexReadLock(Scene.PhysicsAndRayCastLock))
			{
				flag = MBAPI.IMBMission.IsFormationUnitPositionAvailable(this.Pointer, ref formationPosition, ref unitPosition, ref nearestAvailableUnitPosition, manhattanDistance);
			}
			return flag;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00053BC8 File Offset: 0x00051DC8
		public Agent RayCastForClosestAgent(Vec3 sourcePoint, Vec3 targetPoint, int excludedAgentIndex, float rayThickness, out float collisionDistance)
		{
			return MBAPI.IMBMission.RayCastForClosestAgent(this.Pointer, sourcePoint, targetPoint, excludedAgentIndex, rayThickness, out collisionDistance);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00053BE1 File Offset: 0x00051DE1
		public Agent RayCastForClosestAgentsLimbs(Vec3 sourcePoint, Vec3 targetPoint, int excludedAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex)
		{
			return MBAPI.IMBMission.RayCastForClosestAgentsLimbs(this.Pointer, sourcePoint, targetPoint, excludedAgentIndex, rayThickness, out collisionDistance, out boneIndex);
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00053BFC File Offset: 0x00051DFC
		public bool RayCastForGivenAgentsLimbs(Vec3 sourcePoint, Vec3 rayFinishPoint, int givenAgentIndex, float rayThickness, out float collisionDistance, out sbyte boneIndex)
		{
			return MBAPI.IMBMission.RayCastForGivenAgentsLimbs(this.Pointer, sourcePoint, rayFinishPoint, givenAgentIndex, rayThickness, out collisionDistance, out boneIndex);
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00053C17 File Offset: 0x00051E17
		internal AgentProximityMap.ProximityMapSearchStructInternal ProximityMapBeginSearch(Vec2 searchPos, float searchRadius)
		{
			return MBAPI.IMBMission.ProximityMapBeginSearch(this.Pointer, searchPos, searchRadius);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00053C2B File Offset: 0x00051E2B
		internal float ProximityMapMaxSearchRadius()
		{
			return MBAPI.IMBMission.ProximityMapMaxSearchRadius(this.Pointer);
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00053C3D File Offset: 0x00051E3D
		public float GetBiggestAgentCollisionPadding()
		{
			return MBAPI.IMBMission.GetBiggestAgentCollisionPadding(this.Pointer);
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00053C4F File Offset: 0x00051E4F
		public void SetMissionCorpseFadeOutTimeInSeconds(float corpseFadeOutTimeInSeconds)
		{
			MBAPI.IMBMission.SetMissionCorpseFadeOutTimeInSeconds(this.Pointer, corpseFadeOutTimeInSeconds);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00053C62 File Offset: 0x00051E62
		public void SetOverrideCorpseCount(int overrideCorpseCount)
		{
			MBAPI.IMBMission.SetOverrideCorpseCount(this.Pointer, overrideCorpseCount);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00053C75 File Offset: 0x00051E75
		public void SetReportStuckAgentsMode(bool value)
		{
			MBAPI.IMBMission.SetReportStuckAgentsMode(this.Pointer, value);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00053C88 File Offset: 0x00051E88
		internal void BatchFormationUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, WorldPosition orderPosition, Vec2 direction, int fileCount, int rankCount, bool fastCheckWithSameFaceGroupIdDigit)
		{
			MBAPI.IMBMission.BatchFormationUnitPositions(this.Pointer, orderedPositionIndices.RawArray, orderedLocalPositions.RawArray, availabilityTable.RawArray, globalPositionTable.RawArray, orderPosition, direction, fileCount, rankCount, fastCheckWithSameFaceGroupIdDigit);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00053CC8 File Offset: 0x00051EC8
		internal void ProximityMapFindNext(ref AgentProximityMap.ProximityMapSearchStructInternal searchStruct)
		{
			MBAPI.IMBMission.ProximityMapFindNext(this.Pointer, ref searchStruct);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00053CDC File Offset: 0x00051EDC
		[UsedImplicitly]
		[MBCallback(null, false)]
		public void ResetMission()
		{
			IMissionListener[] array = this._listeners.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnResetMission();
			}
			foreach (Agent agent in this._activeAgents)
			{
				agent.OnRemove();
			}
			foreach (Agent agent2 in this._allAgents)
			{
				agent2.OnDelete();
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnClearScene();
			}
			this.NumOfFormationsSpawnedTeamOne = 0;
			this.NumOfFormationsSpawnedTeamTwo = 0;
			foreach (Team team in this.Teams)
			{
				team.Reset();
			}
			MBAPI.IMBMission.ClearScene(this.Pointer);
			this._activeAgents.Clear();
			this._allAgents.Clear();
			this._mountsWithoutRiders.Clear();
			this.MainAgent = null;
			this.ClearMissiles();
			this._missilesList.Clear();
			this._missilesDictionary.Clear();
			this._agentCount = 0;
			for (int j = 0; j < 2; j++)
			{
				this._initialAgentCountPerSide[j] = 0;
				this._removedAgentCountPerSide[j] = 0;
			}
			this.ResetMissionObjects();
			this.RemoveSpawnedMissionObjects();
			this._activeMissionObjects.Clear();
			this._activeMissionObjects.AddRange(this.MissionObjects);
			this._tickActions.Clear();
			this.Scene.ClearDecals();
			PropertyChangedEventHandler onMissionReset = this.OnMissionReset;
			if (onMissionReset == null)
			{
				return;
			}
			onMissionReset(this, null);
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600197E RID: 6526 RVA: 0x00053EEC File Offset: 0x000520EC
		// (remove) Token: 0x0600197F RID: 6527 RVA: 0x00053F24 File Offset: 0x00052124
		public event PropertyChangedEventHandler OnMissionReset;

		// Token: 0x06001980 RID: 6528 RVA: 0x00053F5C File Offset: 0x0005215C
		public void Initialize()
		{
			Mission.Current = this;
			this.CurrentState = Mission.State.Initializing;
			this._deploymentPlan = this.GetMissionBehavior<MissionDeploymentPlanningLogic>();
			if (this._deploymentPlan == null)
			{
				this._deploymentPlan = new DefaultMissionDeploymentPlan(this);
			}
			MissionInitializerRecord initializerRecord = this.InitializerRecord;
			MBAPI.IMBMission.InitializeMission(this.Pointer, ref initializerRecord);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00053FAF File Offset: 0x000521AF
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnSceneCreated(Scene scene)
		{
			this.Scene = scene;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00053FB8 File Offset: 0x000521B8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void TickAgentsAndTeams(float dt, bool tickPaused)
		{
			this.TickAgentsAndTeamsImp(dt, tickPaused);
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00053FC2 File Offset: 0x000521C2
		public void TickAgentsAndTeamsAsync(float dt)
		{
			MBAPI.IMBMission.TickAgentsAndTeamsAsync(this.Pointer, dt);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00053FD5 File Offset: 0x000521D5
		internal void Tick(float dt)
		{
			MBAPI.IMBMission.Tick(this.Pointer, dt);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00053FE8 File Offset: 0x000521E8
		internal void IdleTick(float dt)
		{
			MBAPI.IMBMission.IdleTick(this.Pointer, dt);
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00053FFB File Offset: 0x000521FB
		public void MakeSound(int soundIndex, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2)
		{
			MBAPI.IMBMission.MakeSound(this.Pointer, soundIndex, position, soundCanBePredicted, isReliable, relatedAgent1, relatedAgent2);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00054018 File Offset: 0x00052218
		public void MakeSound(int soundIndex, Vec3 position, bool soundCanBePredicted, bool isReliable, int relatedAgent1, int relatedAgent2, ref SoundEventParameter parameter)
		{
			MBAPI.IMBMission.MakeSoundWithParameter(this.Pointer, soundIndex, position, soundCanBePredicted, isReliable, relatedAgent1, relatedAgent2, parameter);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00054045 File Offset: 0x00052245
		public void MakeSoundOnlyOnRelatedPeer(int soundIndex, Vec3 position, int relatedAgent)
		{
			MBAPI.IMBMission.MakeSoundOnlyOnRelatedPeer(this.Pointer, soundIndex, position, relatedAgent);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0005405A File Offset: 0x0005225A
		public void AddDynamicallySpawnedMissionObjectInfo(Mission.DynamicallyCreatedEntity entityInfo)
		{
			this._addedEntitiesInfo.Add(entityInfo);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00054068 File Offset: 0x00052268
		private void RemoveDynamicallySpawnedMissionObjectInfo(MissionObjectId id)
		{
			Mission.DynamicallyCreatedEntity dynamicallyCreatedEntity = this._addedEntitiesInfo.FirstOrDefault<Mission.DynamicallyCreatedEntity>((Mission.DynamicallyCreatedEntity x) => x.ObjectId == id);
			if (dynamicallyCreatedEntity != null)
			{
				this._addedEntitiesInfo.Remove(dynamicallyCreatedEntity);
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000540AC File Offset: 0x000522AC
		private int AddMissileAux(int forcedMissileIndex, bool isPrediction, Agent shooterAgent, in WeaponData weaponData, WeaponStatsData[] weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, WeakGameEntity gameEntityToIgnore, bool isPrimaryWeaponShot, out GameEntity missileEntity)
		{
			UIntPtr uintPtr;
			int num = MBAPI.IMBMission.AddMissile(this.Pointer, isPrediction, shooterAgent.Index, in weaponData, weaponStatsData, weaponStatsData.Length, damageBonus, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, gameEntityToIgnore.Pointer, forcedMissileIndex, isPrimaryWeaponShot, out uintPtr);
			missileEntity = (isPrediction ? null : new GameEntity(uintPtr));
			return num;
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00054104 File Offset: 0x00052304
		private int AddMissileSingleUsageAux(int forcedMissileIndex, bool isPrediction, Agent shooterAgent, in WeaponData weaponData, in WeaponStatsData weaponStatsData, float damageBonus, ref Vec3 position, ref Vec3 direction, ref Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, WeakGameEntity gameEntityToIgnore, bool isPrimaryWeaponShot, out GameEntity missileEntity)
		{
			UIntPtr uintPtr;
			int num = MBAPI.IMBMission.AddMissileSingleUsage(this.Pointer, isPrediction, shooterAgent.Index, in weaponData, in weaponStatsData, damageBonus, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, gameEntityToIgnore.Pointer, forcedMissileIndex, isPrimaryWeaponShot, out uintPtr);
			missileEntity = (isPrediction ? null : new GameEntity(uintPtr));
			return num;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00054155 File Offset: 0x00052355
		public Vec3 GetMissileCollisionPoint(Vec3 missileStartingPosition, Vec3 missileDirection, float missileSpeed, in WeaponData weaponData)
		{
			return MBAPI.IMBMission.GetMissileCollisionPoint(this.Pointer, missileStartingPosition, missileDirection, missileSpeed, in weaponData);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0005416C File Offset: 0x0005236C
		public void RemoveMissileAsClient(int missileIndex)
		{
			MBAPI.IMBMission.RemoveMissile(this.Pointer, missileIndex);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0005417F File Offset: 0x0005237F
		public static float GetMissileVerticalAimCorrection(Vec3 vecToTarget, float missileStartingSpeed, ref WeaponStatsData weaponStatsData, float airFrictionConstant)
		{
			return MBAPI.IMBMission.GetMissileVerticalAimCorrection(vecToTarget, missileStartingSpeed, ref weaponStatsData, airFrictionConstant);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0005418F File Offset: 0x0005238F
		public static float GetMissileRange(float missileStartingSpeed, float heightDifference)
		{
			return MBAPI.IMBMission.GetMissileRange(missileStartingSpeed, heightDifference);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0005419D File Offset: 0x0005239D
		public void PrepareMissileWeaponForDrop(int missileIndex)
		{
			MBAPI.IMBMission.PrepareMissileWeaponForDrop(this.Pointer, missileIndex);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000541B0 File Offset: 0x000523B0
		public void AddParticleSystemBurstByName(string particleSystem, MatrixFrame frame, bool synchThroughNetwork)
		{
			MBAPI.IMBMission.AddParticleSystemBurstByName(this.Pointer, particleSystem, ref frame, synchThroughNetwork);
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x000541C6 File Offset: 0x000523C6
		public bool IsLoadingFinished
		{
			get
			{
				return MBAPI.IMBMission.GetIsLoadingFinished(this.Pointer);
			}
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000541D8 File Offset: 0x000523D8
		public Vec2 GetClosestBoundaryPosition(Vec2 position)
		{
			return MBAPI.IMBMission.GetClosestBoundaryPosition(this.Pointer, position);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000541EC File Offset: 0x000523EC
		private void ResetMissionObjects()
		{
			for (int i = this._dynamicEntities.Count - 1; i >= 0; i--)
			{
				Mission.DynamicEntityInfo dynamicEntityInfo = this._dynamicEntities[i];
				dynamicEntityInfo.Entity.RemoveEnginePhysics();
				dynamicEntityInfo.Entity.Remove(74);
				this._dynamicEntities.RemoveAt(i);
			}
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				if (missionObject.CreatedAtRuntime)
				{
					break;
				}
				missionObject.OnMissionReset();
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00054290 File Offset: 0x00052490
		private void RemoveSpawnedMissionObjects()
		{
			MissionObject[] array = this._missionObjects.ToArray();
			for (int i = array.Length - 1; i >= 0; i--)
			{
				MissionObject missionObject = array[i];
				if (!missionObject.CreatedAtRuntime)
				{
					break;
				}
				if (missionObject.GameEntity.IsValid)
				{
					missionObject.GameEntity.RemoveAllChildren();
					missionObject.GameEntity.Remove(75);
				}
			}
			this._spawnedItemEntitiesCreatedAtRuntime.Clear();
			this._lastRuntimeMissionObjectIdCount = 0;
			this._emptyRuntimeMissionObjectIds.Clear();
			this._addedEntitiesInfo.Clear();
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0005431C File Offset: 0x0005251C
		public int GetFreeRuntimeMissionObjectId()
		{
			float totalMissionTime = MBCommon.GetTotalMissionTime();
			int num = -1;
			if (this._emptyRuntimeMissionObjectIds.Count > 0)
			{
				if (totalMissionTime - this._emptyRuntimeMissionObjectIds.Peek().Item2 > 30f || this._lastRuntimeMissionObjectIdCount >= 8191)
				{
					num = this._emptyRuntimeMissionObjectIds.Pop().Item1;
				}
				else
				{
					num = this._lastRuntimeMissionObjectIdCount;
					this._lastRuntimeMissionObjectIdCount++;
				}
			}
			else if (this._lastRuntimeMissionObjectIdCount < 8191)
			{
				num = this._lastRuntimeMissionObjectIdCount;
				this._lastRuntimeMissionObjectIdCount++;
			}
			return num;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000543B2 File Offset: 0x000525B2
		private void ReturnRuntimeMissionObjectId(int id)
		{
			this._emptyRuntimeMissionObjectIds.Push(new ValueTuple<int, float>(id, MBCommon.GetTotalMissionTime()));
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x000543CA File Offset: 0x000525CA
		public int GetFreeSceneMissionObjectId()
		{
			int lastSceneMissionObjectIdCount = this._lastSceneMissionObjectIdCount;
			this._lastSceneMissionObjectIdCount++;
			return lastSceneMissionObjectIdCount;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x000543E0 File Offset: 0x000525E0
		public void SetCameraFrame(ref MatrixFrame cameraFrame, float zoomFactor)
		{
			this.SetCameraFrame(ref cameraFrame, zoomFactor, ref cameraFrame.origin);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x000543F0 File Offset: 0x000525F0
		public void SetCameraFrame(ref MatrixFrame cameraFrame, float zoomFactor, ref Vec3 attenuationPosition)
		{
			cameraFrame.Fill();
			MBAPI.IMBMission.SetCameraFrame(this.Pointer, ref cameraFrame, zoomFactor, ref attenuationPosition);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0005440B File Offset: 0x0005260B
		public MatrixFrame GetCameraFrame()
		{
			return MBAPI.IMBMission.GetCameraFrame(this.Pointer);
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x0005441D File Offset: 0x0005261D
		// (set) Token: 0x0600199E RID: 6558 RVA: 0x00054424 File Offset: 0x00052624
		public bool CameraIsFirstPerson
		{
			get
			{
				return Mission._isCameraFirstPerson;
			}
			set
			{
				if (Mission._isCameraFirstPerson != value)
				{
					Mission._isCameraFirstPerson = value;
					MBAPI.IMBMission.SetCameraIsFirstPerson(value);
					this.ResetFirstThirdPersonView();
				}
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x00054445 File Offset: 0x00052645
		// (set) Token: 0x060019A0 RID: 6560 RVA: 0x0005444C File Offset: 0x0005264C
		public static float CameraAddedDistance
		{
			get
			{
				return BannerlordConfig.CombatCameraDistance;
			}
			set
			{
				if (value != BannerlordConfig.CombatCameraDistance)
				{
					BannerlordConfig.CombatCameraDistance = value;
				}
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060019A1 RID: 6561 RVA: 0x0005445C File Offset: 0x0005265C
		public float ClearSceneTimerElapsedTime
		{
			get
			{
				return MBAPI.IMBMission.GetClearSceneTimerElapsedTime(this.Pointer);
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0005446E File Offset: 0x0005266E
		public void ResetFirstThirdPersonView()
		{
			MBAPI.IMBMission.ResetFirstThirdPersonView(this.Pointer);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x00054480 File Offset: 0x00052680
		public void SetCustomCameraLocalOffset(Vec3 newCameraOffset)
		{
			this.CustomCameraLocalOffset = newCameraOffset;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00054489 File Offset: 0x00052689
		public void SetCustomCameraTargetLocalOffset(Vec3 newTargetLocalOffset)
		{
			this.CustomCameraTargetLocalOffset = newTargetLocalOffset;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00054492 File Offset: 0x00052692
		public void SetCustomCameraLocalOffset2(Vec3 newCameraOffset)
		{
			this.CustomCameraLocalOffset2 = newCameraOffset;
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0005449B File Offset: 0x0005269B
		public void SetCustomCameraLocalRotationalOffset(Vec3 newCameraRotationalOffset)
		{
			this.CustomCameraLocalRotationalOffset = newCameraRotationalOffset;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x000544A4 File Offset: 0x000526A4
		public void SetCustomCameraGlobalOffset(Vec3 newCameraOffset)
		{
			this.CustomCameraGlobalOffset = newCameraOffset;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x000544AD File Offset: 0x000526AD
		public void SetCustomCameraFovMultiplier(float newFovMultiplier)
		{
			this.CustomCameraFovMultiplier = newFovMultiplier;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x000544B6 File Offset: 0x000526B6
		public void SetCustomCameraFixedDistance(float distance)
		{
			this.CustomCameraFixedDistance = distance;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x000544BF File Offset: 0x000526BF
		public void SetIgnoredEntityForCamera(GameEntity ignoredEntity)
		{
			this.IgnoredEntityForCamera = ignoredEntity;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000544C8 File Offset: 0x000526C8
		public void SetCustomCameraIgnoreCollision(bool ignoreCollision)
		{
			this.CustomCameraIgnoreCollision = ignoreCollision;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x000544D1 File Offset: 0x000526D1
		public void SetListenerAndAttenuationPosBlendFactor(float factor)
		{
			this.ListenerAndAttenuationPosBlendFactor = factor;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x000544DC File Offset: 0x000526DC
		internal void UpdateSceneTimeSpeed()
		{
			if (this.Scene != null)
			{
				float num = 1f;
				int num2 = -1;
				for (int i = 0; i < this._timeSpeedRequests.Count; i++)
				{
					if (this._timeSpeedRequests[i].RequestedTimeSpeed < num)
					{
						num = this._timeSpeedRequests[i].RequestedTimeSpeed;
						num2 = this._timeSpeedRequests[i].RequestID;
					}
				}
				if (!this.Scene.TimeSpeed.ApproximatelyEqualsTo(num, 1E-05f))
				{
					if (num2 != -1)
					{
						Debug.Print(string.Format("Updated mission time speed with request ID:{0}, time speed{1}", num2, num), 0, Debug.DebugColor.White, 17592186044416UL);
					}
					else
					{
						Debug.Print(string.Format("Reverted time speed back to default({0})", num), 0, Debug.DebugColor.White, 17592186044416UL);
					}
					this.Scene.TimeSpeed = num;
				}
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x000545CB File Offset: 0x000527CB
		public void AddTimeSpeedRequest(Mission.TimeSpeedRequest request)
		{
			this._timeSpeedRequests.Add(request);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x000545DC File Offset: 0x000527DC
		[Conditional("_RGL_KEEP_ASSERTS")]
		private void AssertTimeSpeedRequestDoesNotExist(Mission.TimeSpeedRequest request)
		{
			for (int i = 0; i < this._timeSpeedRequests.Count; i++)
			{
				int requestID = this._timeSpeedRequests[i].RequestID;
				int requestID2 = request.RequestID;
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0005461C File Offset: 0x0005281C
		public void RemoveTimeSpeedRequest(int timeSpeedRequestID)
		{
			int num = -1;
			for (int i = 0; i < this._timeSpeedRequests.Count; i++)
			{
				if (this._timeSpeedRequests[i].RequestID == timeSpeedRequestID)
				{
					num = i;
				}
			}
			this._timeSpeedRequests.RemoveAt(num);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00054668 File Offset: 0x00052868
		public bool GetRequestedTimeSpeed(int timeSpeedRequestID, out float requestedTime)
		{
			foreach (Mission.TimeSpeedRequest timeSpeedRequest in this._timeSpeedRequests)
			{
				if (timeSpeedRequest.RequestID == timeSpeedRequestID)
				{
					requestedTime = timeSpeedRequest.RequestedTimeSpeed;
					return true;
				}
			}
			requestedTime = 0f;
			return false;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x000546D8 File Offset: 0x000528D8
		public void ClearAgentActions()
		{
			MBAPI.IMBMission.ClearAgentActions(this.Pointer);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x000546EA File Offset: 0x000528EA
		public void ClearMissiles()
		{
			MBAPI.IMBMission.ClearMissiles(this.Pointer);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000546FC File Offset: 0x000528FC
		public void ClearCorpses(bool isMissionReset)
		{
			MBAPI.IMBMission.ClearCorpses(this.Pointer, isMissionReset);
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005470F File Offset: 0x0005290F
		private Agent FindAgentWithIndexAux(int index)
		{
			if (index >= 0)
			{
				return MBAPI.IMBMission.FindAgentWithIndex(this.Pointer, index);
			}
			return null;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00054728 File Offset: 0x00052928
		private Agent GetClosestEnemyAgent(MBTeam team, Vec3 position, float radius)
		{
			return MBAPI.IMBMission.GetClosestEnemy(this.Pointer, team.Index, position, radius);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00054742 File Offset: 0x00052942
		private Agent GetClosestAllyAgent(MBTeam team, Vec3 position, float radius)
		{
			return MBAPI.IMBMission.GetClosestAlly(this.Pointer, team.Index, position, radius);
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0005475C File Offset: 0x0005295C
		private int GetNearbyEnemyAgentCount(MBTeam team, Vec2 position, float radius)
		{
			int num = 0;
			int num2 = 0;
			MBAPI.IMBMission.GetAgentCountAroundPosition(this.Pointer, team.Index, position, radius, ref num, ref num2);
			return num2;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0005478A File Offset: 0x0005298A
		public bool IsAgentInProximityMap(Agent agent)
		{
			return MBAPI.IMBMission.IsAgentInProximityMap(this.Pointer, agent.Index);
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000547A4 File Offset: 0x000529A4
		public void OnMissionStateActivate()
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissionStateActivated();
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000547F4 File Offset: 0x000529F4
		public void OnMissionStateDeactivate()
		{
			if (this.MissionBehaviors != null)
			{
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnMissionStateDeactivated();
				}
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0005484C File Offset: 0x00052A4C
		public void OnMissionStateFinalize(bool forceClearGPUResources)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissionStateFinalized();
			}
			if (GameNetwork.IsSessionActive && this.GetMissionBehavior<MissionNetworkComponent>() != null)
			{
				this.RemoveMissionBehavior(this.GetMissionBehavior<MissionNetworkComponent>());
			}
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.RemoveMissionBehavior(this.MissionBehaviors[i]);
			}
			this._deploymentPlan = null;
			this.MissionLogics.Clear();
			this.Scene = null;
			Mission.Current = null;
			this.ClearUnreferencedResources(forceClearGPUResources);
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00054908 File Offset: 0x00052B08
		public void ClearUnreferencedResources(bool forceClearGPUResources)
		{
			Common.MemoryCleanupGC(false);
			if (forceClearGPUResources)
			{
				MBAPI.IMBMission.ClearResources(this.Pointer);
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00054924 File Offset: 0x00052B24
		internal void OnEntityHit(WeakGameEntity entity, Agent attackerAgent, AttackCollisionData collisionData, int inflictedDamage, DamageTypes damageType, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, int affectorWeaponSlotOrMissileIndex, ref CombatLogData combatLog)
		{
			bool flag = false;
			float num = (float)inflictedDamage;
			float num2 = -1f;
			float num3 = -1f;
			MissionObject missionObject = null;
			while (entity.IsValid)
			{
				int scriptCount = entity.GetScriptCount();
				for (int i = 0; i < scriptCount; i++)
				{
					MissionObject missionObject2;
					if ((missionObject2 = entity.GetScriptAtIndex(i) as MissionObject) != null)
					{
						bool flag2;
						if (missionObject2.OnHit(attackerAgent, inflictedDamage, impactPosition, impactDirection, in weapon, affectorWeaponSlotOrMissileIndex, null, out flag2, out num, out num2, out num3))
						{
							missionObject = missionObject2;
						}
						flag = flag || flag2;
					}
				}
				if (missionObject != null)
				{
					break;
				}
				entity = entity.Parent;
			}
			combatLog.MissionObjectHit = missionObject;
			if (flag && attackerAgent != null && !attackerAgent.IsMount && !attackerAgent.IsAIControlled)
			{
				combatLog.DamageType = damageType;
				combatLog.InflictedDamage = inflictedDamage;
				combatLog.ModifiedDamage = MathF.Round(num - (float)inflictedDamage);
				if (num2 > 0f)
				{
					combatLog.InflictedFireDamage = (int)num2;
					combatLog.ModifiedFireDamage = (int)num3;
				}
				this.AddCombatLogSafe(attackerAgent, null, combatLog);
			}
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00054A1C File Offset: 0x00052C1C
		public float GetMainAgentMaxCameraZoom()
		{
			if (this.MainAgent != null)
			{
				return MissionGameModels.Current.AgentStatCalculateModel.GetMaxCameraZoom(this.MainAgent);
			}
			return 1f;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00054A41 File Offset: 0x00052C41
		public WorldPosition GetBestSlopeTowardsDirection(ref WorldPosition centerPosition, float halfSize, ref WorldPosition referencePosition)
		{
			return MBAPI.IMBMission.GetBestSlopeTowardsDirection(this.Pointer, ref centerPosition, halfSize, ref referencePosition);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00054A58 File Offset: 0x00052C58
		public WorldPosition GetBestSlopeAngleHeightPosForDefending(WorldPosition enemyPosition, WorldPosition defendingPosition, int sampleSize, float distanceRatioAllowedFromDefendedPos, float distanceSqrdAllowedFromBoundary, float cosinusOfBestSlope, float cosinusOfMaxAcceptedSlope, float minSlopeScore, float maxSlopeScore, float excessiveSlopePenalty, float nearConeCenterRatio, float nearConeCenterBonus, float heightDifferenceCeiling, float maxDisplacementPenalty)
		{
			return MBAPI.IMBMission.GetBestSlopeAngleHeightPosForDefending(this.Pointer, enemyPosition, defendingPosition, sampleSize, distanceRatioAllowedFromDefendedPos, distanceSqrdAllowedFromBoundary, cosinusOfBestSlope, cosinusOfMaxAcceptedSlope, minSlopeScore, maxSlopeScore, excessiveSlopePenalty, nearConeCenterRatio, nearConeCenterBonus, heightDifferenceCeiling, maxDisplacementPenalty);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00054A90 File Offset: 0x00052C90
		public Vec2 GetAveragePositionOfAgents(List<Agent> agents)
		{
			int num = 0;
			Vec2 vec = Vec2.Zero;
			foreach (Agent agent in agents)
			{
				num++;
				vec += agent.Position.AsVec2;
			}
			if (num == 0)
			{
				return Vec2.Invalid;
			}
			return vec * (1f / (float)num);
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00054B10 File Offset: 0x00052D10
		private void GetNearbyAgentsAux(Vec2 center, float radius, MBTeam team, Mission.GetNearbyAgentsAuxType type, MBList<Agent> resultList)
		{
			EngineStackArray.StackArray40Int stackArray40Int = default(EngineStackArray.StackArray40Int);
			object getNearbyAgentsAuxLock = Mission.GetNearbyAgentsAuxLock;
			lock (getNearbyAgentsAuxLock)
			{
				int num = 0;
				for (;;)
				{
					int num2 = -1;
					MBAPI.IMBMission.GetNearbyAgentsAux(this.Pointer, center, radius, team.Index, (int)type, num, ref stackArray40Int, ref num2);
					for (int i = 0; i < num2; i++)
					{
						Agent agent = DotNetObject.GetManagedObjectWithId(stackArray40Int[i]) as Agent;
						resultList.Add(agent);
					}
					if (num2 < 40)
					{
						break;
					}
					num += 40;
				}
			}
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00054BB4 File Offset: 0x00052DB4
		private int GetNearbyAgentsCountAux(Vec2 center, float radius, MBTeam team, Mission.GetNearbyAgentsAuxType type)
		{
			int num = 0;
			EngineStackArray.StackArray40Int stackArray40Int = default(EngineStackArray.StackArray40Int);
			object getNearbyAgentsAuxLock = Mission.GetNearbyAgentsAuxLock;
			lock (getNearbyAgentsAuxLock)
			{
				int num2 = 0;
				for (;;)
				{
					int num3 = -1;
					MBAPI.IMBMission.GetNearbyAgentsAux(this.Pointer, center, radius, team.Index, (int)type, num2, ref stackArray40Int, ref num3);
					num += num3;
					if (num3 < 40)
					{
						break;
					}
					num2 += 40;
				}
			}
			return num;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00054C34 File Offset: 0x00052E34
		public void SetRandomDecideTimeOfAgentsWithIndices(int[] agentIndices, float? minAIReactionTime = null, float? maxAIReactionTime = null)
		{
			if (minAIReactionTime == null || maxAIReactionTime == null)
			{
				maxAIReactionTime = new float?((float)(-1));
				minAIReactionTime = maxAIReactionTime;
			}
			MBAPI.IMBMission.SetRandomDecideTimeOfAgents(this.Pointer, agentIndices.Length, agentIndices, minAIReactionTime.Value, maxAIReactionTime.Value);
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00054C81 File Offset: 0x00052E81
		public void SetBowMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetBowMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00054C94 File Offset: 0x00052E94
		public void SetCrossbowMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetCrossbowMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00054CA7 File Offset: 0x00052EA7
		public void SetThrowingMissileSpeedModifier(float modifier)
		{
			MBAPI.IMBMission.SetThrowingMissileSpeedModifier(this.Pointer, modifier);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00054CBA File Offset: 0x00052EBA
		public void SetMissileRangeModifier(float modifier)
		{
			MBAPI.IMBMission.SetMissileRangeModifier(this.Pointer, modifier);
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00054CCD File Offset: 0x00052ECD
		public void SetLastMovementKeyPressed(Agent.MovementControlFlag lastMovementKeyPressed)
		{
			MBAPI.IMBMission.SetLastMovementKeyPressed(this.Pointer, lastMovementKeyPressed);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x00054CE0 File Offset: 0x00052EE0
		public Vec2 GetWeightedPointOfEnemies(Agent agent, Vec2 basePoint)
		{
			return MBAPI.IMBMission.GetWeightedPointOfEnemies(this.Pointer, agent.Index, basePoint);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00054CF9 File Offset: 0x00052EF9
		public bool GetPathBetweenPositions(ref NavigationData navData)
		{
			return MBAPI.IMBMission.GetNavigationPoints(this.Pointer, ref navData);
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00054D0C File Offset: 0x00052F0C
		public void SetNavigationFaceCostWithIdAroundPosition(int navigationFaceId, Vec3 position, float cost)
		{
			MBAPI.IMBMission.SetNavigationFaceCostWithIdAroundPosition(this.Pointer, navigationFaceId, position, cost);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00054D21 File Offset: 0x00052F21
		public WorldPosition GetStraightPathToTarget(Vec2 targetPosition, WorldPosition startingPosition, float samplingDistance = 1f, bool stopAtObstacle = true)
		{
			return MBAPI.IMBMission.GetStraightPathToTarget(this.Pointer, targetPosition, startingPosition, samplingDistance, stopAtObstacle);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00054D38 File Offset: 0x00052F38
		public void SkipForwardMissionReplay(float startTime, float endTime)
		{
			MBAPI.IMBMission.SkipForwardMissionReplay(this.Pointer, startTime, endTime);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00054D4C File Offset: 0x00052F4C
		public int GetDebugAgent()
		{
			return MBAPI.IMBMission.GetDebugAgent(this.Pointer);
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00054D5E File Offset: 0x00052F5E
		public void AddAiDebugText(string str)
		{
			MBAPI.IMBMission.AddAiDebugText(this.Pointer, str);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00054D71 File Offset: 0x00052F71
		public void SetDebugAgent(int index)
		{
			MBAPI.IMBMission.SetDebugAgent(this.Pointer, index);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00054D84 File Offset: 0x00052F84
		public static float GetFirstPersonFov()
		{
			return BannerlordConfig.FirstPersonFov;
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00054D8B File Offset: 0x00052F8B
		public float GetWaterLevelAtPosition(Vec2 position, bool useWaterRenderer)
		{
			return MBAPI.IMBMission.GetWaterLevelAtPosition(this.Pointer, position, useWaterRenderer);
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00054D9F File Offset: 0x00052F9F
		public float GetWaterLevelAtPositionMT(Vec2 position, bool useWaterRenderer)
		{
			return MBAPI.IMBMission.GetWaterLevelAtPosition(this.Pointer, position, useWaterRenderer);
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00054DB4 File Offset: 0x00052FB4
		[UsedImplicitly]
		[MBCallback(null, true)]
		public bool CanPhysicsCollideBetweenTwoEntities(UIntPtr entity0Ptr, UIntPtr entity1Ptr)
		{
			WeakGameEntity weakGameEntity = new WeakGameEntity(entity0Ptr);
			WeakGameEntity weakGameEntity2 = new WeakGameEntity(entity1Ptr);
			BodyFlags bodyFlags = (weakGameEntity.IsValid ? weakGameEntity.BodyFlag : BodyFlags.None);
			BodyFlags bodyFlags2 = (weakGameEntity2.IsValid ? weakGameEntity2.BodyFlag : BodyFlags.None);
			WeakGameEntity weakGameEntity3 = weakGameEntity;
			while (weakGameEntity3.IsValid)
			{
				int scriptCount = weakGameEntity3.GetScriptCount();
				for (int i = 0; i < scriptCount; i++)
				{
					ScriptComponentBehavior scriptAtIndex = weakGameEntity3.GetScriptAtIndex(i);
					if (scriptAtIndex != null && !(weakGameEntity == weakGameEntity2) && !scriptAtIndex.CanPhysicsCollideBetweenTwoEntities(weakGameEntity, bodyFlags, weakGameEntity2, bodyFlags2))
					{
						return false;
					}
				}
				weakGameEntity3 = weakGameEntity3.Parent;
			}
			weakGameEntity3 = weakGameEntity2;
			while (weakGameEntity3.IsValid)
			{
				int scriptCount2 = weakGameEntity3.GetScriptCount();
				for (int j = 0; j < scriptCount2; j++)
				{
					ScriptComponentBehavior scriptAtIndex2 = weakGameEntity3.GetScriptAtIndex(j);
					if (scriptAtIndex2 != null && !(weakGameEntity == weakGameEntity2) && !scriptAtIndex2.CanPhysicsCollideBetweenTwoEntities(weakGameEntity2, bodyFlags2, weakGameEntity, bodyFlags))
					{
						return false;
					}
				}
				weakGameEntity3 = weakGameEntity3.Parent;
			}
			return true;
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060019D7 RID: 6615 RVA: 0x00054EAC File Offset: 0x000530AC
		// (remove) Token: 0x060019D8 RID: 6616 RVA: 0x00054EE4 File Offset: 0x000530E4
		public event Mission.OnBeforeAgentRemovedDelegate OnBeforeAgentRemoved;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060019D9 RID: 6617 RVA: 0x00054F1C File Offset: 0x0005311C
		// (remove) Token: 0x060019DA RID: 6618 RVA: 0x00054F54 File Offset: 0x00053154
		public event Func<WorldPosition, Team, bool> IsFormationUnitPositionAvailable_AdditionalCondition;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060019DB RID: 6619 RVA: 0x00054F8C File Offset: 0x0005318C
		// (remove) Token: 0x060019DC RID: 6620 RVA: 0x00054FC4 File Offset: 0x000531C4
		public event Func<Agent, bool> CanAgentRout_AdditionalCondition;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060019DD RID: 6621 RVA: 0x00054FFC File Offset: 0x000531FC
		// (remove) Token: 0x060019DE RID: 6622 RVA: 0x00055034 File Offset: 0x00053234
		public event Mission.OnAddSoundAlarmFactorToAgentsDelegate OnAddSoundAlarmFactorToAgents;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060019DF RID: 6623 RVA: 0x0005506C File Offset: 0x0005326C
		// (remove) Token: 0x060019E0 RID: 6624 RVA: 0x000550A4 File Offset: 0x000532A4
		public event Func<bool> IsAgentInteractionAllowed_AdditionalCondition;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060019E1 RID: 6625 RVA: 0x000550DC File Offset: 0x000532DC
		// (remove) Token: 0x060019E2 RID: 6626 RVA: 0x00055114 File Offset: 0x00053314
		public event Mission.OnMainAgentChangedDelegate OnMainAgentChanged;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060019E3 RID: 6627 RVA: 0x0005514C File Offset: 0x0005334C
		// (remove) Token: 0x060019E4 RID: 6628 RVA: 0x00055184 File Offset: 0x00053384
		public event Mission.OnCameraShakeTriggeredDelegate OnCameraShakeTriggered;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060019E5 RID: 6629 RVA: 0x000551BC File Offset: 0x000533BC
		// (remove) Token: 0x060019E6 RID: 6630 RVA: 0x000551F4 File Offset: 0x000533F4
		public event Mission.ComputeTroopBodyPropertiesDelegate OnComputeTroopBodyProperties;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060019E7 RID: 6631 RVA: 0x0005522C File Offset: 0x0005342C
		// (remove) Token: 0x060019E8 RID: 6632 RVA: 0x00055264 File Offset: 0x00053464
		public event Func<BattleSideEnum, BasicCharacterObject, FormationClass> GetAgentTroopClass_Override;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060019E9 RID: 6633 RVA: 0x0005529C File Offset: 0x0005349C
		// (remove) Token: 0x060019EA RID: 6634 RVA: 0x000552D4 File Offset: 0x000534D4
		public event Action DeploymentFinishedEvent;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060019EB RID: 6635 RVA: 0x0005530C File Offset: 0x0005350C
		// (remove) Token: 0x060019EC RID: 6636 RVA: 0x00055344 File Offset: 0x00053544
		public event Action<Agent, SpawnedItemEntity> OnItemPickUp;

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x00055379 File Offset: 0x00053579
		public MBReadOnlyList<Mission.Missile> MissilesList
		{
			get
			{
				return this._missilesList;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060019EE RID: 6638 RVA: 0x00055384 File Offset: 0x00053584
		// (remove) Token: 0x060019EF RID: 6639 RVA: 0x000553BC File Offset: 0x000535BC
		public event Action<Agent, SpawnedItemEntity> OnItemDrop;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060019F0 RID: 6640 RVA: 0x000553F4 File Offset: 0x000535F4
		// (remove) Token: 0x060019F1 RID: 6641 RVA: 0x0005542C File Offset: 0x0005362C
		public event Action<Formation> FormationCaptainChanged;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060019F2 RID: 6642 RVA: 0x00055464 File Offset: 0x00053664
		// (remove) Token: 0x060019F3 RID: 6643 RVA: 0x0005549C File Offset: 0x0005369C
		public event Func<Agent, WorldPosition?> GetOverriddenFleePositionForAgent;

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000554D1 File Offset: 0x000536D1
		// (set) Token: 0x060019F5 RID: 6645 RVA: 0x000554DC File Offset: 0x000536DC
		public bool MissionEnded
		{
			get
			{
				return this._missionEnded;
			}
			private set
			{
				if (!this._missionEnded && value)
				{
					this.MissionIsEnding = true;
					foreach (MissionObject missionObject in this.MissionObjects)
					{
						missionObject.OnMissionEnded();
					}
					this.MissionIsEnding = false;
				}
				this._missionEnded = value;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x00055550 File Offset: 0x00053750
		public MBReadOnlyList<KeyValuePair<Agent, MissionTime>> MountsWithoutRiders
		{
			get
			{
				return this._mountsWithoutRiders;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060019F7 RID: 6647 RVA: 0x00055558 File Offset: 0x00053758
		// (remove) Token: 0x060019F8 RID: 6648 RVA: 0x00055590 File Offset: 0x00053790
		public event Func<bool> AreOrderGesturesEnabled_AdditionalCondition;

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x000555C5 File Offset: 0x000537C5
		// (set) Token: 0x060019FA RID: 6650 RVA: 0x000555CD File Offset: 0x000537CD
		public bool MissionIsEnding { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x000555D6 File Offset: 0x000537D6
		// (set) Token: 0x060019FC RID: 6652 RVA: 0x000555DE File Offset: 0x000537DE
		public bool IsDeploymentFinished { get; private set; }

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060019FD RID: 6653 RVA: 0x000555E8 File Offset: 0x000537E8
		// (remove) Token: 0x060019FE RID: 6654 RVA: 0x00055620 File Offset: 0x00053820
		public event Func<bool> IsBattleInRetreatEvent;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060019FF RID: 6655 RVA: 0x00055658 File Offset: 0x00053858
		// (remove) Token: 0x06001A00 RID: 6656 RVA: 0x00055690 File Offset: 0x00053890
		public event Action<int> OnMissileRemovedEvent;

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x000556C5 File Offset: 0x000538C5
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x000556CD File Offset: 0x000538CD
		public BattleSideEnum RetreatSide { get; private set; } = BattleSideEnum.None;

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x000556D6 File Offset: 0x000538D6
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x000556DE File Offset: 0x000538DE
		public bool IsFastForward
		{
			get
			{
				return this._isFastForward;
			}
			private set
			{
				this._isFastForward = value;
				MBAPI.IMBMission.OnFastForwardStateChanged(this.Pointer, this._isFastForward);
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x000556FD File Offset: 0x000538FD
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x00055705 File Offset: 0x00053905
		public bool FixedDeltaTimeMode { get; set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0005570E File Offset: 0x0005390E
		// (set) Token: 0x06001A08 RID: 6664 RVA: 0x00055716 File Offset: 0x00053916
		public float FixedDeltaTime { get; set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x0005571F File Offset: 0x0005391F
		// (set) Token: 0x06001A0A RID: 6666 RVA: 0x00055727 File Offset: 0x00053927
		public Mission.State CurrentState { get; private set; }

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x00055730 File Offset: 0x00053930
		// (set) Token: 0x06001A0C RID: 6668 RVA: 0x00055738 File Offset: 0x00053938
		public Mission.TeamCollection Teams { get; private set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x00055741 File Offset: 0x00053941
		public Team AttackerTeam
		{
			get
			{
				return this.Teams.Attacker;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0005574E File Offset: 0x0005394E
		public Team DefenderTeam
		{
			get
			{
				return this.Teams.Defender;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0005575B File Offset: 0x0005395B
		public Team AttackerAllyTeam
		{
			get
			{
				return this.Teams.AttackerAlly;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001A10 RID: 6672 RVA: 0x00055768 File Offset: 0x00053968
		public Team DefenderAllyTeam
		{
			get
			{
				return this.Teams.DefenderAlly;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x00055775 File Offset: 0x00053975
		// (set) Token: 0x06001A12 RID: 6674 RVA: 0x00055782 File Offset: 0x00053982
		public Team PlayerTeam
		{
			get
			{
				return this.Teams.Player;
			}
			set
			{
				this.Teams.Player = value;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x00055790 File Offset: 0x00053990
		public Team PlayerEnemyTeam
		{
			get
			{
				return this.Teams.PlayerEnemy;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001A14 RID: 6676 RVA: 0x0005579D File Offset: 0x0005399D
		public Team PlayerAllyTeam
		{
			get
			{
				return this.Teams.PlayerAlly;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000557AA File Offset: 0x000539AA
		// (set) Token: 0x06001A16 RID: 6678 RVA: 0x000557B2 File Offset: 0x000539B2
		public Team SpectatorTeam { get; set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x000557BB File Offset: 0x000539BB
		IMissionTeam IMission.PlayerTeam
		{
			get
			{
				return this.PlayerTeam;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x000557C3 File Offset: 0x000539C3
		public bool IsMissionEnding
		{
			get
			{
				return this.CurrentState != Mission.State.Over && this.MissionEnded;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x000557D6 File Offset: 0x000539D6
		public List<MissionLogic> MissionLogics { get; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001A1A RID: 6682 RVA: 0x000557DE File Offset: 0x000539DE
		public List<MissionBehavior> MissionBehaviors { get; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x000557E6 File Offset: 0x000539E6
		// (set) Token: 0x06001A1C RID: 6684 RVA: 0x000557EE File Offset: 0x000539EE
		public IInputContext InputManager { get; set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000557F7 File Offset: 0x000539F7
		// (set) Token: 0x06001A1E RID: 6686 RVA: 0x000557FF File Offset: 0x000539FF
		public bool NeedsMemoryCleanup { get; private set; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x00055808 File Offset: 0x00053A08
		public Agent InitialPlayerAgent
		{
			get
			{
				return this._initialPlayerAgent;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x00055810 File Offset: 0x00053A10
		// (set) Token: 0x06001A21 RID: 6689 RVA: 0x00055818 File Offset: 0x00053A18
		public Agent MainAgent
		{
			get
			{
				return this._mainAgent;
			}
			set
			{
				Agent mainAgent = this._mainAgent;
				this._mainAgent = value;
				Mission.OnMainAgentChangedDelegate onMainAgentChanged = this.OnMainAgentChanged;
				if (onMainAgentChanged != null)
				{
					onMainAgentChanged(mainAgent);
				}
				if (!GameNetwork.IsClient)
				{
					this.MainAgentServer = this._mainAgent;
				}
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0005585A File Offset: 0x00053A5A
		public IMissionDeploymentPlan DeploymentPlan
		{
			get
			{
				return this._deploymentPlan;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00055862 File Offset: 0x00053A62
		public bool IsBattleSpawnPathSelectorInitialized
		{
			get
			{
				return this._battleSpawnPathSelector != null && this._battleSpawnPathSelector.IsInitialized;
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0005587C File Offset: 0x00053A7C
		public bool GetDeploymentPlan<T>(out T deploymentPlan) where T : IMissionDeploymentPlan
		{
			deploymentPlan = default(T);
			IMissionDeploymentPlan deploymentPlan2;
			if (this._deploymentPlan != null && (deploymentPlan2 = this._deploymentPlan) is T)
			{
				T t = (T)((object)deploymentPlan2);
				deploymentPlan = t;
			}
			return deploymentPlan != null;
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x000558C4 File Offset: 0x00053AC4
		public float GetRemovedAgentRatioForSide(BattleSideEnum side)
		{
			float num = 0f;
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Cannot get removed agent count for side. Invalid battle side passed!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetRemovedAgentRatioForSide", 711);
			}
			float num2 = (float)this._initialAgentCountPerSide[(int)side];
			if (num2 > 0f && this._agentCount > 0)
			{
				num = MathF.Min((float)this._removedAgentCountPerSide[(int)side] / num2, 1f);
			}
			return num;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x00055926 File Offset: 0x00053B26
		// (set) Token: 0x06001A27 RID: 6695 RVA: 0x0005592E File Offset: 0x00053B2E
		public Agent MainAgentServer { get; set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00055937 File Offset: 0x00053B37
		public bool HasSpawnPath
		{
			get
			{
				return this._battleSpawnPathSelector.IsInitialized;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00055944 File Offset: 0x00053B44
		public bool IsFieldBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.FieldBattle;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x0005594F File Offset: 0x00053B4F
		public bool IsSiegeBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x0005595A File Offset: 0x00053B5A
		public bool IsSallyOutBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00055965 File Offset: 0x00053B65
		public bool IsNavalBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.NavalBattle;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x00055970 File Offset: 0x00053B70
		public bool IsNavalRaidBattle
		{
			get
			{
				return this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.NavalRaid;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x0005597B File Offset: 0x00053B7B
		public AgentReadOnlyList AllAgents
		{
			get
			{
				return this._allAgents;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x00055983 File Offset: 0x00053B83
		public AgentReadOnlyList Agents
		{
			get
			{
				return this._activeAgents;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0005598B File Offset: 0x00053B8B
		public bool IsInventoryAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsInventoryAccessibleAtMission || this._isScreenAccessAllowed) && this.IsInventoryAccessible;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x000559AE File Offset: 0x00053BAE
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x000559B6 File Offset: 0x00053BB6
		public bool IsInventoryAccessible { private get; set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x000559BF File Offset: 0x00053BBF
		// (set) Token: 0x06001A34 RID: 6708 RVA: 0x000559C7 File Offset: 0x00053BC7
		public MissionResult MissionResult { get; private set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x000559D0 File Offset: 0x00053BD0
		// (set) Token: 0x06001A36 RID: 6710 RVA: 0x000559D8 File Offset: 0x00053BD8
		public MissionFocusableObjectInformationProvider FocusableObjectInformationProvider { get; private set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x000559E1 File Offset: 0x00053BE1
		// (set) Token: 0x06001A38 RID: 6712 RVA: 0x000559E9 File Offset: 0x00053BE9
		public bool IsQuestScreenAccessible { private get; set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x000559F2 File Offset: 0x00053BF2
		private bool _isScreenAccessAllowed
		{
			get
			{
				return this.Mode != MissionMode.Battle && this.Mode != MissionMode.Deployment && this.Mode != MissionMode.Duel && this.Mode != MissionMode.CutScene;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x00055A1E File Offset: 0x00053C1E
		public bool IsQuestScreenAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsQuestScreenAccessibleAtMission || this._isScreenAccessAllowed) && this.IsQuestScreenAccessible;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x00055A41 File Offset: 0x00053C41
		// (set) Token: 0x06001A3C RID: 6716 RVA: 0x00055A49 File Offset: 0x00053C49
		public bool IsCharacterWindowAccessible { private get; set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x00055A52 File Offset: 0x00053C52
		public bool IsCharacterWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsCharacterWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsCharacterWindowAccessible;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x00055A75 File Offset: 0x00053C75
		// (set) Token: 0x06001A3F RID: 6719 RVA: 0x00055A7D File Offset: 0x00053C7D
		public bool IsPartyWindowAccessible { private get; set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x00055A86 File Offset: 0x00053C86
		public bool IsPartyWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsPartyWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsPartyWindowAccessible;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00055AA9 File Offset: 0x00053CA9
		// (set) Token: 0x06001A42 RID: 6722 RVA: 0x00055AB1 File Offset: 0x00053CB1
		public bool IsKingdomWindowAccessible { private get; set; }

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x00055ABA File Offset: 0x00053CBA
		public bool IsKingdomWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsKingdomWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsKingdomWindowAccessible;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x00055ADD File Offset: 0x00053CDD
		// (set) Token: 0x06001A45 RID: 6725 RVA: 0x00055AE5 File Offset: 0x00053CE5
		public bool IsClanWindowAccessible { private get; set; }

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x00055AEE File Offset: 0x00053CEE
		public bool IsClanWindowAccessAllowed
		{
			get
			{
				return Game.Current.GameType.IsClanWindowAccessibleAtMission && this._isScreenAccessAllowed && this.IsClanWindowAccessible;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x00055B11 File Offset: 0x00053D11
		// (set) Token: 0x06001A48 RID: 6728 RVA: 0x00055B19 File Offset: 0x00053D19
		public bool IsEncyclopediaWindowAccessible { private get; set; }

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00055B22 File Offset: 0x00053D22
		public bool IsEncyclopediaWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsEncyclopediaWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsEncyclopediaWindowAccessible;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00055B45 File Offset: 0x00053D45
		// (set) Token: 0x06001A4B RID: 6731 RVA: 0x00055B4D File Offset: 0x00053D4D
		public bool IsBannerWindowAccessible { private get; set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00055B56 File Offset: 0x00053D56
		public bool IsBannerWindowAccessAllowed
		{
			get
			{
				return (Game.Current.GameType.IsBannerWindowAccessibleAtMission || this._isScreenAccessAllowed) && this.IsBannerWindowAccessible;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x00055B79 File Offset: 0x00053D79
		// (set) Token: 0x06001A4E RID: 6734 RVA: 0x00055B81 File Offset: 0x00053D81
		public bool DoesMissionRequireCivilianEquipment
		{
			get
			{
				return this._doesMissionRequireCivilianEquipment;
			}
			set
			{
				this._doesMissionRequireCivilianEquipment = value;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00055B8A File Offset: 0x00053D8A
		// (set) Token: 0x06001A50 RID: 6736 RVA: 0x00055B92 File Offset: 0x00053D92
		public Mission.MissionTeamAITypeEnum MissionTeamAIType { get; set; }

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00055B9B File Offset: 0x00053D9B
		private Lazy<MissionRecorder> _recorder
		{
			get
			{
				return new Lazy<MissionRecorder>(() => new MissionRecorder(this));
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x00055BAE File Offset: 0x00053DAE
		public MissionRecorder Recorder
		{
			get
			{
				return this._recorder.Value;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x00055BBB File Offset: 0x00053DBB
		public bool CanPlayerTakeControlOfAnotherAgentWhenDead
		{
			get
			{
				return this._canPlayerTakeControlOfAnotherAgentWhenDead;
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00055BC3 File Offset: 0x00053DC3
		public readonly ref List<SiegeWeapon> GetAttackerWeaponsForFriendlyFirePreventing()
		{
			return ref this._attackerWeaponsForFriendlyFirePreventing;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x00055BCC File Offset: 0x00053DCC
		public void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnDeploymentPlanMade(team, isFirstPlan);
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00055C20 File Offset: 0x00053E20
		public WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(Vec2 directionTowards, WorldPosition originalPosition, ref float positionPenalty)
		{
			return MBAPI.IMBMission.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(this.Pointer, ref directionTowards, ref originalPosition, ref positionPenalty);
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00055C37 File Offset: 0x00053E37
		public int GetNextDynamicNavMeshIdStart()
		{
			int nextDynamicNavMeshIdStart = this._nextDynamicNavMeshIdStart;
			this._nextDynamicNavMeshIdStart += 50;
			return nextDynamicNavMeshIdStart;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00055C50 File Offset: 0x00053E50
		public FormationClass GetAgentTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			if (this.GetAgentTroopClass_Override != null)
			{
				return this.GetAgentTroopClass_Override(battleSide, agentCharacter);
			}
			FormationClass formationClass = agentCharacter.GetFormationClass();
			if (this.IsSiegeBattle || this.IsNavalBattle || this.IsNavalRaidBattle || (this.IsSallyOutBattle && battleSide == BattleSideEnum.Attacker))
			{
				formationClass = formationClass.DismountedClass();
			}
			return formationClass;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00055CB0 File Offset: 0x00053EB0
		[UsedImplicitly]
		[MBCallback(null, false)]
		public WorldPosition GetClosestFleePositionForAgent(Agent agent)
		{
			if (this.GetOverriddenFleePositionForAgent != null)
			{
				WorldPosition? worldPosition = this.GetOverriddenFleePositionForAgent(agent);
				if (worldPosition != null)
				{
					return worldPosition.Value;
				}
			}
			WorldPosition worldPosition2 = agent.GetWorldPosition();
			float maximumForwardUnlimitedSpeed = agent.GetMaximumForwardUnlimitedSpeed();
			Team team = agent.Team;
			BattleSideEnum battleSideEnum = BattleSideEnum.None;
			bool flag = agent.MountAgent != null;
			if (team != null)
			{
				team.UpdateCachedEnemyDataForFleeing();
				battleSideEnum = team.Side;
			}
			MBReadOnlyList<FleePosition> mbreadOnlyList = ((this.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut && agent.IsMount) ? this.GetFleePositionsForSide(BattleSideEnum.Attacker) : this.GetFleePositionsForSide(battleSideEnum));
			return this.GetClosestFleePosition(mbreadOnlyList, worldPosition2, maximumForwardUnlimitedSpeed, flag, (team != null) ? team.CachedEnemyDataForFleeing : null);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00055D54 File Offset: 0x00053F54
		public WorldPosition GetClosestFleePositionForFormation(Formation formation)
		{
			WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
			float movementSpeedMaximum = formation.QuerySystem.MovementSpeedMaximum;
			bool flag = formation.QuerySystem.IsCavalryFormation || formation.QuerySystem.IsRangedCavalryFormation;
			Team team = formation.Team;
			team.UpdateCachedEnemyDataForFleeing();
			MBReadOnlyList<FleePosition> fleePositionsForSide = this.GetFleePositionsForSide(team.Side);
			return this.GetClosestFleePosition(fleePositionsForSide, cachedMedianPosition, movementSpeedMaximum, flag, team.CachedEnemyDataForFleeing);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00055DBC File Offset: 0x00053FBC
		private WorldPosition GetClosestFleePosition(MBReadOnlyList<FleePosition> availableFleePositions, WorldPosition runnerPosition, float runnerSpeed, bool runnerHasMount, MBReadOnlyList<ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool>> chaserData)
		{
			int num = ((chaserData != null) ? chaserData.Count : 0);
			if (availableFleePositions.Count > 0)
			{
				float[] array = new float[availableFleePositions.Count];
				WorldPosition[] array2 = new WorldPosition[availableFleePositions.Count];
				for (int i = 0; i < availableFleePositions.Count; i++)
				{
					array[i] = 1f;
					array2[i] = new WorldPosition(this.Scene, UIntPtr.Zero, availableFleePositions[i].GetClosestPointToEscape(runnerPosition.AsVec2), false);
					array2[i].SetVec2(array2[i].AsVec2 - runnerPosition.AsVec2);
				}
				for (int j = 0; j < num; j++)
				{
					float item = chaserData[j].Item1;
					if (item > 0f)
					{
						ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool> valueTuple = chaserData[j];
						Vec2 asVec = valueTuple.Item2.AsVec2;
						int item2 = chaserData[j].Item3;
						Vec2 vec2;
						if (item2 > 1)
						{
							Vec2 item3 = chaserData[j].Item4;
							Vec2 item4 = chaserData[j].Item5;
							Vec2 vec = runnerPosition.AsVec2;
							vec2 = MBMath.GetClosestPointOnLineSegmentToPoint(in item3, in item4, in vec) - runnerPosition.AsVec2;
						}
						else
						{
							vec2 = asVec - runnerPosition.AsVec2;
						}
						for (int k = 0; k < availableFleePositions.Count; k++)
						{
							Vec2 vec = array2[k].AsVec2;
							float num2 = vec2.DotProduct(vec.Normalized());
							if (num2 > 0f)
							{
								vec = array2[k].AsVec2;
								vec = vec.LeftVec();
								float num3 = MathF.Max(MathF.Abs(vec2.DotProduct(vec.Normalized())) / item, 1f);
								float num4 = MathF.Max(num2 / runnerSpeed, 1f);
								if (num4 > num3)
								{
									float num5 = num4 / num3;
									num5 /= num2;
									array[k] += num5 * (float)item2;
								}
							}
						}
					}
				}
				for (int l = 0; l < availableFleePositions.Count; l++)
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene, UIntPtr.Zero, availableFleePositions[l].GetClosestPointToEscape(runnerPosition.AsVec2), false);
					float num6;
					if (this.Scene.GetPathDistanceBetweenPositions(ref runnerPosition, ref worldPosition, 0f, out num6))
					{
						array[l] *= num6;
					}
					else
					{
						array[l] = float.MaxValue;
					}
				}
				int num7 = -1;
				float num8 = float.MaxValue;
				for (int m = 0; m < availableFleePositions.Count; m++)
				{
					if (num8 > array[m])
					{
						num7 = m;
						num8 = array[m];
					}
				}
				if (num7 >= 0)
				{
					Vec3 closestPointToEscape = availableFleePositions[num7].GetClosestPointToEscape(runnerPosition.AsVec2);
					return new WorldPosition(this.Scene, UIntPtr.Zero, closestPointToEscape, false);
				}
			}
			float[] array3 = new float[4];
			for (int n = 0; n < num; n++)
			{
				ValueTuple<float, WorldPosition, int, Vec2, Vec2, bool> valueTuple = chaserData[n];
				Vec2 asVec2 = valueTuple.Item2.AsVec2;
				int item5 = chaserData[n].Item3;
				Vec2 vec3;
				if (item5 > 1)
				{
					Vec2 item6 = chaserData[n].Item4;
					Vec2 item7 = chaserData[n].Item5;
					Vec2 vec = runnerPosition.AsVec2;
					vec3 = MBMath.GetClosestPointOnLineSegmentToPoint(in item6, in item7, in vec) - runnerPosition.AsVec2;
				}
				else
				{
					vec3 = asVec2 - runnerPosition.AsVec2;
				}
				float num9 = vec3.Length;
				if (chaserData[n].Item6)
				{
					num9 *= 0.5f;
				}
				if (runnerHasMount)
				{
					num9 *= 2f;
				}
				float num10 = MBMath.ClampFloat(1f - (num9 - 40f) / 40f, 0.01f, 1f);
				Vec2 vec4 = vec3.Normalized();
				float num11 = 1.2f;
				float num12 = num10 * (float)item5 * num11;
				float num13 = num12 * MathF.Abs(vec4.x);
				float num14 = num12 * MathF.Abs(vec4.y);
				array3[(vec4.y < 0f) ? 0 : 1] -= num14;
				array3[(vec4.x < 0f) ? 2 : 3] -= num13;
				array3[(vec4.y < 0f) ? 1 : 0] += num14;
				array3[(vec4.x < 0f) ? 3 : 2] += num13;
			}
			float num15 = 0.04f;
			Vec3 vec5;
			Vec3 vec6;
			this.Scene.GetBoundingBox(out vec5, out vec6);
			Vec2 closestBoundaryPosition = this.GetClosestBoundaryPosition(new Vec2(runnerPosition.X, vec5.y));
			Vec2 closestBoundaryPosition2 = this.GetClosestBoundaryPosition(new Vec2(runnerPosition.X, vec6.y));
			Vec2 closestBoundaryPosition3 = this.GetClosestBoundaryPosition(new Vec2(vec5.x, runnerPosition.Y));
			Vec2 closestBoundaryPosition4 = this.GetClosestBoundaryPosition(new Vec2(vec6.x, runnerPosition.Y));
			float num16 = closestBoundaryPosition2.y - closestBoundaryPosition.y;
			float num17 = closestBoundaryPosition4.x - closestBoundaryPosition3.x;
			array3[0] += (num16 - (runnerPosition.Y - closestBoundaryPosition.y)) * num15;
			array3[1] += (num16 - (closestBoundaryPosition2.y - runnerPosition.Y)) * num15;
			array3[2] += (num17 - (runnerPosition.X - closestBoundaryPosition3.x)) * num15;
			array3[3] += (num17 - (closestBoundaryPosition4.x - runnerPosition.X)) * num15;
			Vec2 vec7;
			if (array3[0] >= array3[1] && array3[0] >= array3[2] && array3[0] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition.x, closestBoundaryPosition.y);
			}
			else if (array3[1] >= array3[2] && array3[1] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition2.x, closestBoundaryPosition2.y);
			}
			else if (array3[2] >= array3[3])
			{
				vec7 = new Vec2(closestBoundaryPosition3.x, closestBoundaryPosition3.y);
			}
			else
			{
				vec7 = new Vec2(closestBoundaryPosition4.x, closestBoundaryPosition4.y);
			}
			return new WorldPosition(this.Scene, UIntPtr.Zero, new Vec3(vec7, runnerPosition.GetNavMeshZ(), -1f), false);
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0005641A File Offset: 0x0005461A
		// (set) Token: 0x06001A5D RID: 6749 RVA: 0x00056422 File Offset: 0x00054622
		public MissionTimeTracker MissionTimeTracker { get; private set; }

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005642C File Offset: 0x0005462C
		public MBReadOnlyList<FleePosition> GetFleePositionsForSide(BattleSideEnum side)
		{
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Flee position with invalid battle side field found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetFleePositionsForSide", 1275);
				return null;
			}
			int num = (int)((side == BattleSideEnum.None) ? BattleSideEnum.Defender : (side + 1));
			return this._fleePositions[num];
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0005646B File Offset: 0x0005466B
		public void AddToWeaponListForFriendlyFirePreventing(SiegeWeapon weapon)
		{
			this._attackerWeaponsForFriendlyFirePreventing.Add(weapon);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0005647C File Offset: 0x0005467C
		public Mission(MissionInitializerRecord rec, MissionState missionState, bool needsMemoryCleanup)
		{
			this.Pointer = MBAPI.IMBMission.CreateMission(this);
			this._spawnedItemEntitiesCreatedAtRuntime = new List<SpawnedItemEntity>();
			this._missionObjects = new MBList<MissionObject>();
			this._activeMissionObjects = new MBList<MissionObject>();
			this._mountsWithoutRiders = new MBList<KeyValuePair<Agent, MissionTime>>();
			this._addedEntitiesInfo = new MBList<Mission.DynamicallyCreatedEntity>();
			this._emptyRuntimeMissionObjectIds = new Stack<ValueTuple<int, float>>();
			this.Boundaries = new Mission.MBBoundaryCollection(this);
			this.InitializerRecord = rec;
			this.CurrentState = Mission.State.NewlyCreated;
			this.IsInventoryAccessible = false;
			this.IsQuestScreenAccessible = true;
			this.IsCharacterWindowAccessible = true;
			this.IsPartyWindowAccessible = true;
			this.IsKingdomWindowAccessible = true;
			this.IsClanWindowAccessible = true;
			this.IsBannerWindowAccessible = false;
			this.IsEncyclopediaWindowAccessible = true;
			this._missilesList = new MBList<Mission.Missile>();
			this._missilesDictionary = new Dictionary<int, Mission.Missile>();
			this._activeAgents = new AgentList(256);
			this._allAgents = new AgentList(256);
			for (int i = 0; i < 3; i++)
			{
				this._fleePositions[i] = new MBList<FleePosition>(32);
			}
			for (int j = 0; j < 2; j++)
			{
				this._initialAgentCountPerSide[j] = 0;
				this._removedAgentCountPerSide[j] = 0;
			}
			this.MissionBehaviors = new List<MissionBehavior>();
			this.MissionLogics = new List<MissionLogic>();
			this._otherMissionBehaviors = new List<MissionBehavior>();
			this._missionState = missionState;
			this._battleSpawnPathSelector = new BattleSpawnPathSelector(this);
			this.Teams = new Mission.TeamCollection(this);
			this.FocusableObjectInformationProvider = new MissionFocusableObjectInformationProvider();
			this.MissionTimeTracker = new MissionTimeTracker();
			this.NeedsMemoryCleanup = needsMemoryCleanup;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000566DD File Offset: 0x000548DD
		public void SetCloseProximityWaveSoundsEnabled(bool value)
		{
			MBAPI.IMBMission.SetCloseProximityWaveSoundsEnabled(this.Pointer, value);
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000566F0 File Offset: 0x000548F0
		public void ForceDisableOcclusion(bool value)
		{
			MBAPI.IMBMission.ForceDisableOcclusion(this.Pointer, value);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00056704 File Offset: 0x00054904
		public void AddFleePosition(FleePosition fleePosition)
		{
			BattleSideEnum side = fleePosition.GetSide();
			if (side == BattleSideEnum.NumSides)
			{
				Debug.FailedAssert("Flee position with invalid battle side field found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "AddFleePosition", 1365);
				return;
			}
			if (side == BattleSideEnum.None)
			{
				for (int i = 0; i < this._fleePositions.Length; i++)
				{
					this._fleePositions[i].Add(fleePosition);
				}
				return;
			}
			int num = (int)(side + 1);
			this._fleePositions[num].Add(fleePosition);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00056770 File Offset: 0x00054970
		private void FreeResources()
		{
			this.MainAgent = null;
			this.Teams.ClearResources();
			this.SpectatorTeam = null;
			this._activeAgents = null;
			this._allAgents = null;
			if (GameNetwork.NetworkPeersValid)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null)
					{
						component.ClearAllVisuals(true);
						networkCommunicator.RemoveComponent(component);
					}
					MissionRepresentativeBase component2 = networkCommunicator.GetComponent<MissionRepresentativeBase>();
					if (component2 != null)
					{
						networkCommunicator.RemoveComponent(component2);
					}
				}
			}
			if (GameNetwork.DisconnectedNetworkPeers != null)
			{
				Debug.Print("DisconnectedNetworkPeers.Clear()", 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.DisconnectedNetworkPeers.Clear();
			}
			this._missionState = null;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00056840 File Offset: 0x00054A40
		public void RetreatMission()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnRetreatMission();
			}
			if (MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.LeaveEditMissionMode();
				return;
			}
			this.EndMission();
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x000568AC File Offset: 0x00054AAC
		public void SurrenderMission()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnSurrenderMission();
			}
			if (MBEditor.EditModeEnabled && MBEditor.IsEditModeOn)
			{
				MBEditor.LeaveEditMissionMode();
				return;
			}
			this.EndMission();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00056918 File Offset: 0x00054B18
		public bool HasMissionBehavior<T>() where T : MissionBehavior
		{
			return this.GetMissionBehavior<T>() != null;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00056928 File Offset: 0x00054B28
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentAddedAsCorpse(Agent affectedAgent, int corpsesToFadeIndex)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int i = 0; i < affectedAgent.GetAttachedWeaponsCount(); i++)
				{
					if (affectedAgent.GetAttachedWeapon(i).Item.ItemFlags.HasAnyFlag(ItemFlags.CanBePickedUpFromCorpse))
					{
						this.SpawnAttachedWeaponOnCorpse(affectedAgent, i, -1);
					}
				}
				affectedAgent.ClearAttachedWeapons();
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00056980 File Offset: 0x00054B80
		public SpawnedItemEntity SpawnAttachedWeaponOnCorpse(Agent agent, int attachedWeaponIndex, int forcedSpawnIndex)
		{
			Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
			if (skeleton != null)
			{
				skeleton.ForceUpdateBoneFrames();
			}
			MissionWeapon attachedWeapon = agent.GetAttachedWeapon(attachedWeaponIndex);
			GameEntity attachedWeaponEntity = agent.AgentVisuals.GetAttachedWeaponEntity(attachedWeaponIndex);
			attachedWeaponEntity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = attachedWeaponEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnAttachedWeaponOnCorpse(agent.Index, attachedWeaponIndex, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			return this.SpawnWeaponAux(attachedWeaponEntity.WeakEntity, attachedWeapon, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, Vec3.Zero, Vec3.Zero, false, true);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00056A31 File Offset: 0x00054C31
		public void AddMountWithoutRider(Agent mount)
		{
			this._mountsWithoutRiders.Add(new KeyValuePair<Agent, MissionTime>(mount, MissionTime.Now));
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00056A4C File Offset: 0x00054C4C
		public void RemoveMountWithoutRider(Agent mount)
		{
			for (int i = 0; i < this._mountsWithoutRiders.Count; i++)
			{
				if (this._mountsWithoutRiders[i].Key == mount)
				{
					this._mountsWithoutRiders.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00056A94 File Offset: 0x00054C94
		public void UpdateMountReservationsAfterRiderMounts(Agent rider, Agent mount)
		{
			int selectedMountIndex = rider.GetSelectedMountIndex();
			if (selectedMountIndex >= 0 && selectedMountIndex != mount.Index)
			{
				Agent agent = Mission.Current.FindAgentWithIndex(selectedMountIndex);
				if (agent != null)
				{
					rider.HumanAIComponent.UnreserveMount(agent);
				}
			}
			int num = ((mount.CommonAIComponent != null) ? mount.CommonAIComponent.ReservedRiderAgentIndex : (-1));
			if (num >= 0)
			{
				if (num == rider.Index)
				{
					rider.HumanAIComponent.UnreserveMount(mount);
					return;
				}
				Agent agent2 = Mission.Current.FindAgentWithIndex(num);
				if (agent2 != null)
				{
					agent2.HumanAIComponent.UnreserveMount(mount);
				}
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00056B1C File Offset: 0x00054D1C
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentDeleted(Agent affectedAgent)
		{
			if (affectedAgent != null)
			{
				affectedAgent.State = AgentState.Deleted;
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnAgentDeleted(affectedAgent);
				}
				this._allAgents.Remove(affectedAgent);
				affectedAgent.OnDelete();
				affectedAgent.SetTeam(null, false);
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00056B94 File Offset: 0x00054D94
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			Mission.OnBeforeAgentRemovedDelegate onBeforeAgentRemoved = this.OnBeforeAgentRemoved;
			if (onBeforeAgentRemoved != null)
			{
				onBeforeAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			affectedAgent.State = agentState;
			if (affectorAgent != null && affectorAgent.Team != affectedAgent.Team)
			{
				affectorAgent.KillCount++;
			}
			Team team = affectedAgent.Team;
			if (team != null)
			{
				team.DeactivateAgent(affectedAgent);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnEarlyAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
			{
				missionBehavior2.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			}
			bool flag = this.MainAgent == affectedAgent;
			if (flag)
			{
				affectedAgent.OnMainAgentWieldedItemChange = null;
				this.MainAgent = null;
			}
			if (this._initialPlayerAgent == affectedAgent)
			{
				this._initialPlayerAgent = null;
			}
			affectedAgent.OnAgentWieldedItemChange = null;
			affectedAgent.OnAgentMountedStateChanged = null;
			if (affectedAgent.Team != null && affectedAgent.Team.Side != BattleSideEnum.None)
			{
				this._removedAgentCountPerSide[(int)affectedAgent.Team.Side]++;
			}
			this._activeAgents.Remove(affectedAgent);
			affectedAgent.OnRemove();
			if (affectedAgent.IsMount && affectedAgent.RiderAgent == null)
			{
				this.RemoveMountWithoutRider(affectedAgent);
			}
			if (flag)
			{
				affectedAgent.Team.DelegateCommandToAI();
			}
			if (!GameNetwork.IsClientOrReplay && agentState != AgentState.Routed && affectedAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon))
			{
				EquipmentIndex offhandWieldedItemIndex = affectedAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex == EquipmentIndex.ExtraWeaponSlot)
				{
					WeaponComponentData currentUsageItem = affectedAgent.Equipment[offhandWieldedItemIndex].CurrentUsageItem;
					if (currentUsageItem != null && currentUsageItem.WeaponClass == WeaponClass.Banner)
					{
						affectedAgent.DropItem(EquipmentIndex.ExtraWeaponSlot, WeaponClass.Undefined);
					}
				}
			}
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00056D70 File Offset: 0x00054F70
		public void OnObjectDisabled(DestructableComponent destructionComponent)
		{
			UsableMachine firstScriptOfType = destructionComponent.GameEntity.GetFirstScriptOfType<UsableMachine>();
			if (firstScriptOfType != null)
			{
				firstScriptOfType.Disable();
			}
			if (destructionComponent != null)
			{
				destructionComponent.SetAbilityOfFaces(false);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectDisabled(destructionComponent);
			}
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00056DE4 File Offset: 0x00054FE4
		public bool TryGetMissileVelocityFromMissileIndex(int missileIndex, out Vec3 velocity)
		{
			velocity = Vec3.Invalid;
			Mission.Missile missile;
			if (this._missilesDictionary.TryGetValue(missileIndex, out missile))
			{
				velocity = missile.GetVelocity();
				return true;
			}
			return false;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00056E1C File Offset: 0x0005501C
		public MissionObjectId SpawnWeaponAsDropFromMissile(int missileIndex, MissionObject attachedMissionObject, in MatrixFrame attachLocalFrame, Mission.WeaponSpawnFlags spawnFlags, in Vec3 velocity, in Vec3 angularVelocity, int forcedSpawnIndex)
		{
			this.PrepareMissileWeaponForDrop(missileIndex);
			Mission.Missile missile = this._missilesDictionary[missileIndex];
			if (attachedMissionObject != null)
			{
				attachedMissionObject.AddStuckMissile(missile.Entity);
			}
			if (attachedMissionObject != null)
			{
				GameEntity entity = missile.Entity;
				MatrixFrame matrixFrame = attachedMissionObject.GameEntity.GetGlobalFrame();
				matrixFrame = matrixFrame.TransformToParent(in attachLocalFrame);
				entity.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				missile.Entity.SetGlobalFrame(in attachLocalFrame, true);
			}
			missile.Entity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = missile.Entity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			this.SpawnWeaponAux(missile.Entity.WeakEntity, missile.Weapon, spawnFlags, velocity, angularVelocity, true, false);
			return firstScriptOfType.Id;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00056EEE File Offset: 0x000550EE
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void SpawnWeaponAsDropFromAgent(Agent agent, EquipmentIndex equipmentIndex, ref Vec3 globalVelocity, ref Vec3 globalAngularVelocity, Mission.WeaponSpawnFlags spawnFlags)
		{
			this.SpawnWeaponAsDropFromAgentAux(agent, equipmentIndex, ref globalVelocity, ref globalAngularVelocity, spawnFlags, -1);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00056F00 File Offset: 0x00055100
		public void SpawnWeaponAsDropFromAgentAux(Agent agent, EquipmentIndex equipmentIndex, ref Vec3 globalVelocity, ref Vec3 globalAngularVelocity, Mission.WeaponSpawnFlags spawnFlags, int forcedSpawnIndex)
		{
			agent.AgentVisuals.GetSkeleton().ForceUpdateBoneFrames();
			agent.PrepareWeaponForDropInEquipmentSlot(equipmentIndex, (spawnFlags & Mission.WeaponSpawnFlags.WithHolster) > Mission.WeaponSpawnFlags.None);
			WeakGameEntity weaponEntityFromEquipmentSlot = agent.GetWeaponEntityFromEquipmentSlot(equipmentIndex);
			weaponEntityFromEquipmentSlot.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = weaponEntityFromEquipmentSlot.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.x);
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.y);
			CompressionMission.SpawnedItemVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalVelocity.z);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.x);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.y);
			CompressionMission.SpawnedItemAngularVelocityCompressionInfo.ClampValueAccordingToLimits(ref globalAngularVelocity.z);
			MissionWeapon missionWeapon = agent.Equipment[equipmentIndex];
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnWeaponAsDropFromAgent(agent.Index, equipmentIndex, globalVelocity, globalAngularVelocity, spawnFlags, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			agent.OnWeaponDrop(equipmentIndex);
			this.SpawnWeaponAux(weaponEntityFromEquipmentSlot, missionWeapon, spawnFlags, globalVelocity, globalAngularVelocity, true, false);
			if (!GameNetwork.IsClientOrReplay)
			{
				for (int i = 0; i < missionWeapon.GetAttachedWeaponsCount(); i++)
				{
					if (missionWeapon.GetAttachedWeapon(i).Item.ItemFlags.HasAnyFlag(ItemFlags.CanBePickedUpFromCorpse))
					{
						this.SpawnAttachedWeaponOnSpawnedWeapon(firstScriptOfType, i, -1);
					}
				}
			}
			Action<Agent, SpawnedItemEntity> onItemDrop = this.OnItemDrop;
			if (onItemDrop == null)
			{
				return;
			}
			onItemDrop(agent, firstScriptOfType);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0005708C File Offset: 0x0005528C
		public void SpawnAttachedWeaponOnSpawnedWeapon(SpawnedItemEntity spawnedWeapon, int attachmentIndex, int forcedSpawnIndex)
		{
			WeakGameEntity child = spawnedWeapon.GameEntity.GetChild(attachmentIndex);
			child.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = child.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			this.SpawnWeaponAux(child, spawnedWeapon.WeaponCopy.GetAttachedWeapon(attachmentIndex), Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, Vec3.Zero, Vec3.Zero, false, false);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnAttachedWeaponOnSpawnedWeapon(spawnedWeapon.Id, attachmentIndex, firstScriptOfType.Id.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0005712E File Offset: 0x0005532E
		public GameEntity SpawnWeaponWithNewEntity(ref MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, MatrixFrame frame)
		{
			return this.SpawnWeaponWithNewEntityAux(weapon, spawnFlags, frame, -1, null, false, false);
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00057144 File Offset: 0x00055344
		public GameEntity SpawnWeaponWithNewEntityAux(MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, MatrixFrame frame, int forcedSpawnIndex, MissionObject attachedMissionObject, bool hasLifeTime, bool spawnedOnACorpse = false)
		{
			GameEntity gameEntity = GameEntityExtensions.Instantiate(this.Scene, weapon, spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster), true);
			gameEntity.CreateAndAddScriptComponent(typeof(SpawnedItemEntity).Name, true);
			SpawnedItemEntity firstScriptOfType = gameEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			if (forcedSpawnIndex >= 0)
			{
				firstScriptOfType.Id = new MissionObjectId(forcedSpawnIndex, true);
			}
			if (attachedMissionObject != null)
			{
				attachedMissionObject.GameEntity.AddChild(gameEntity.WeakEntity, false);
			}
			if (attachedMissionObject != null)
			{
				MatrixFrame matrixFrame = attachedMissionObject.GameEntity.GetGlobalFrame().TransformToParent(in frame);
				if (!matrixFrame.rotation.IsOrthonormal())
				{
					matrixFrame.rotation.Orthonormalize();
				}
				gameEntity.SetGlobalFrame(in matrixFrame, true);
			}
			else
			{
				gameEntity.SetGlobalFrame(in frame, true);
			}
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SpawnWeaponWithNewEntity(weapon, spawnFlags, firstScriptOfType.Id.Id, frame, (attachedMissionObject != null) ? attachedMissionObject.Id : MissionObjectId.Invalid, true, hasLifeTime, spawnedOnACorpse));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				for (int i = 0; i < weapon.GetAttachedWeaponsCount(); i++)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new AttachWeaponToSpawnedWeapon(weapon.GetAttachedWeapon(i), firstScriptOfType.Id, weapon.GetAttachedWeaponFrame(i)));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			Vec3 zero = Vec3.Zero;
			this.SpawnWeaponAux(gameEntity.WeakEntity, weapon, spawnFlags, zero, zero, hasLifeTime, spawnedOnACorpse);
			return gameEntity;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000572A8 File Offset: 0x000554A8
		public void AttachWeaponWithNewEntityToSpawnedWeapon(MissionWeapon weapon, SpawnedItemEntity spawnedItem, MatrixFrame attachLocalFrame)
		{
			GameEntity gameEntity = GameEntityExtensions.Instantiate(this.Scene, weapon, false, true);
			spawnedItem.GameEntity.AddChild(gameEntity.WeakEntity, false);
			gameEntity.SetFrame(ref attachLocalFrame, true);
			spawnedItem.AttachWeaponToWeapon(weapon, ref attachLocalFrame);
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000572EC File Offset: 0x000554EC
		private SpawnedItemEntity SpawnWeaponAux(WeakGameEntity weaponEntity, MissionWeapon weapon, Mission.WeaponSpawnFlags spawnFlags, Vec3 globalVelocity, Vec3 globalAngularVelocity, bool hasLifeTime, bool spawnedOnACorpse = false)
		{
			SpawnedItemEntity firstScriptOfType = weaponEntity.GetFirstScriptOfType<SpawnedItemEntity>();
			bool flag = weapon.IsBanner();
			MissionWeapon missionWeapon = weapon;
			bool flag2 = !flag && hasLifeTime;
			Mission.WeaponSpawnFlags weaponSpawnFlags = spawnFlags;
			Vec3 vec = (flag ? globalVelocity : Vec3.Zero);
			firstScriptOfType.Initialize(missionWeapon, flag2, weaponSpawnFlags, in vec, spawnedOnACorpse);
			if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics | Mission.WeaponSpawnFlags.WithStaticPhysics))
			{
				BodyFlags bodyFlags = BodyFlags.OnlyCollideWithRaycast | BodyFlags.DroppedItem;
				if (weapon.Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp) || spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.CannotBePickedUp))
				{
					bodyFlags |= BodyFlags.DoNotCollideWithRaycast;
				}
				bodyFlags |= BodyFlags.Moveable;
				weaponEntity.AddBodyFlags(bodyFlags, false);
				WeaponData weaponData = weapon.GetWeaponData(true);
				this.RecalculateBody(ref weaponData, weapon.Item.ItemComponent, weapon.Item.WeaponDesign, ref spawnFlags);
				int num = -1;
				if (flag)
				{
					weaponEntity.AddPhysics(weaponData.BaseWeight, weaponData.CenterOfMassShift, weaponData.Shape, globalVelocity, globalAngularVelocity, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), true, num);
				}
				else if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithPhysics | Mission.WeaponSpawnFlags.WithStaticPhysics))
				{
					int num2 = (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster) ? MathF.Max((int)(weapon.IsAnyConsumable() ? weapon.ModifiedMaxAmount : weapon.MaxAmmo), 1) : 1);
					float num3 = weaponData.BaseWeight * (float)num2;
					weaponEntity.AddPhysics(num3, weaponData.CenterOfMassShift, weaponData.Shape, globalVelocity, globalAngularVelocity, PhysicsMaterial.GetFromIndex(weaponData.PhysicsMaterialIndex), spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithStaticPhysics), num);
					if (weaponEntity.Parent != WeakGameEntity.Invalid && spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithStaticPhysics))
					{
						weaponEntity.SetPhysicsMoveToBatched(true);
						weaponEntity.ConvertDynamicBodyToRayCast();
					}
					else
					{
						weaponEntity.SetPhysicsStateOnlyVariable(true, true);
					}
				}
				weaponData.DeinitializeManagedPointers();
			}
			return firstScriptOfType;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x00057484 File Offset: 0x00055684
		public void OnEquipItemsFromSpawnEquipmentBegin(Agent agent, Agent.CreationType creationType)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnEquipItemsFromSpawnEquipmentBegin(agent, creationType);
			}
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x000574D8 File Offset: 0x000556D8
		public void OnEquipItemsFromSpawnEquipment(Agent agent, Agent.CreationType creationType)
		{
			foreach (IMissionListener missionListener in this._listeners)
			{
				missionListener.OnEquipItemsFromSpawnEquipment(agent, creationType);
			}
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0005752C File Offset: 0x0005572C
		public static int GetCurrentVolumeGeneratorVersion()
		{
			return MBAPI.IMBMission.GetCurrentVolumeGeneratorVersion();
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00057538 File Offset: 0x00055738
		[CommandLineFunctionality.CommandLineArgumentFunction("flee_enemies", "mission")]
		public static string MakeEnemiesFleeCheat(List<string> strings)
		{
			Game game = Game.Current;
			if (game == null || !game.CheatMode)
			{
				return "Cheat mode is not enabled.";
			}
			if (GameNetwork.IsClientOrReplay)
			{
				return "does not work in multiplayer";
			}
			if (Mission.Current != null && Mission.Current.Agents != null)
			{
				foreach (Agent agent2 in Mission.Current.Agents.Where<Agent>((Agent agent) => agent.IsHuman && agent.IsActive() && agent.Team.IsEnemyOf(Mission.Current.PlayerTeam)))
				{
					CommonAIComponent commonAIComponent = agent2.CommonAIComponent;
					if (commonAIComponent != null)
					{
						commonAIComponent.Panic();
					}
				}
				return "enemies are fleeing";
			}
			return "mission is not available";
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000575FC File Offset: 0x000557FC
		[CommandLineFunctionality.CommandLineArgumentFunction("flee_team", "mission")]
		public static string MakeTeamFleeCheat(List<string> strings)
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return "does not work in multiplayer";
			}
			if (Mission.Current == null || Mission.Current.Agents == null)
			{
				return "mission is not available";
			}
			string text = "Usage 1: flee_team [ Attacker | AttackerAlly | Defender | DefenderAlly ]\nUsage 2: flee_team [ Attacker | AttackerAlly | Defender | DefenderAlly ] [FormationNo]";
			if (strings.IsEmpty<string>() || strings[0] == "help")
			{
				return "makes an entire team or a team's formation flee battle.\n" + text;
			}
			if (strings.Count >= 3)
			{
				return "invalid number of parameters.\n" + text;
			}
			string text2 = strings[0];
			Team targetTeam = null;
			string text3 = text2.ToLower();
			if (!(text3 == "attacker"))
			{
				if (!(text3 == "attackerally"))
				{
					if (!(text3 == "defender"))
					{
						if (text3 == "defenderally")
						{
							targetTeam = Mission.Current.DefenderAllyTeam;
						}
					}
					else
					{
						targetTeam = Mission.Current.DefenderTeam;
					}
				}
				else
				{
					targetTeam = Mission.Current.AttackerAllyTeam;
				}
			}
			else
			{
				targetTeam = Mission.Current.AttackerTeam;
			}
			if (targetTeam == null)
			{
				return "given team is not valid";
			}
			Formation targetFormation = null;
			if (strings.Count == 2)
			{
				int num = 8;
				int num2 = int.Parse(strings[1]);
				if (num2 < 0 || num2 >= num)
				{
					return "invalid formation index. formation index should be between [0, " + (num - 1) + "]";
				}
				FormationClass formationClass = (FormationClass)num2;
				targetFormation = targetTeam.GetFormation(formationClass);
			}
			if (targetFormation == null)
			{
				IEnumerable<Agent> agents = Mission.Current.Agents;
				Func<Agent, bool> <>9__0;
				Func<Agent, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (Agent agent) => agent.IsHuman && agent.Team == targetTeam);
				}
				foreach (Agent agent3 in agents.Where<Agent>(func))
				{
					CommonAIComponent commonAIComponent = agent3.CommonAIComponent;
					if (commonAIComponent != null)
					{
						commonAIComponent.Panic();
					}
				}
				return "agents in team: " + text2 + " are fleeing";
			}
			IEnumerable<Agent> agents2 = Mission.Current.Agents;
			Func<Agent, bool> <>9__1;
			Func<Agent, bool> func2;
			if ((func2 = <>9__1) == null)
			{
				func2 = (<>9__1 = (Agent agent) => agent.IsHuman && agent.Formation == targetFormation);
			}
			foreach (Agent agent2 in agents2.Where<Agent>(func2))
			{
				CommonAIComponent commonAIComponent2 = agent2.CommonAIComponent;
				if (commonAIComponent2 != null)
				{
					commonAIComponent2.Panic();
				}
			}
			return string.Concat(new object[]
			{
				"agents in team: ",
				text2,
				" and formation: ",
				(int)targetFormation.FormationIndex,
				" (",
				targetFormation.FormationIndex.ToString(),
				") are fleeing"
			});
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000578DC File Offset: 0x00055ADC
		public void RecalculateBody(ref WeaponData weaponData, ItemComponent itemComponent, WeaponDesign craftedWeaponData, ref Mission.WeaponSpawnFlags spawnFlags)
		{
			WeaponComponent weaponComponent = (WeaponComponent)itemComponent;
			ItemObject item = weaponComponent.Item;
			if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster))
			{
				weaponData.Shape = (string.IsNullOrEmpty(item.HolsterBodyName) ? null : PhysicsShape.GetFromResource(item.HolsterBodyName, false));
			}
			else
			{
				weaponData.Shape = (string.IsNullOrEmpty(item.BodyName) ? null : PhysicsShape.GetFromResource(item.BodyName, false));
			}
			PhysicsShape physicsShape = weaponData.Shape;
			if (physicsShape == null)
			{
				Debug.FailedAssert("Item has no body! Applying a default body, but this should not happen! Check this!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2205);
				physicsShape = PhysicsShape.GetFromResource("bo_axe_short", false);
			}
			if (!weaponComponent.Item.ItemFlags.HasAnyFlag(ItemFlags.DoNotScaleBodyAccordingToWeaponLength))
			{
				if (spawnFlags.HasAnyFlag(Mission.WeaponSpawnFlags.WithHolster) || !item.RecalculateBody)
				{
					weaponData.Shape = physicsShape;
				}
				else
				{
					PhysicsShape physicsShape2 = physicsShape.CreateCopy();
					weaponData.Shape = physicsShape2;
					float num = (float)weaponComponent.PrimaryWeapon.WeaponLength * 0.01f;
					if (craftedWeaponData != null)
					{
						physicsShape2.Clear();
						physicsShape2.InitDescription();
						float num2 = 0f;
						float num3 = 0f;
						float num4 = 0f;
						for (int i = 0; i < craftedWeaponData.UsedPieces.Length; i++)
						{
							WeaponDesignElement weaponDesignElement = craftedWeaponData.UsedPieces[i];
							if (weaponDesignElement.IsValid)
							{
								float scaledPieceOffset = weaponDesignElement.ScaledPieceOffset;
								float num5 = craftedWeaponData.PiecePivotDistances[i];
								float num6 = num5 + scaledPieceOffset - weaponDesignElement.ScaledDistanceToPreviousPiece;
								float num7 = num5 - scaledPieceOffset + weaponDesignElement.ScaledDistanceToNextPiece;
								num2 = MathF.Min(num6, num2);
								if (num7 > num3)
								{
									num3 = num7;
									num4 = (num7 + num6) * 0.5f;
								}
							}
						}
						WeaponDesignElement weaponDesignElement2 = craftedWeaponData.UsedPieces[2];
						if (weaponDesignElement2.IsValid)
						{
							float scaledPieceOffset2 = weaponDesignElement2.ScaledPieceOffset;
							num2 -= scaledPieceOffset2;
						}
						physicsShape2.AddCapsule(new CapsuleData(0.035f, new Vec3(0f, 0f, craftedWeaponData.CraftedWeaponLength, -1f), new Vec3(0f, 0f, num2, -1f)));
						bool flag = false;
						if (craftedWeaponData.UsedPieces[1].IsValid)
						{
							float num8 = craftedWeaponData.PiecePivotDistances[1];
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(-0.1f, 0f, num8, -1f), new Vec3(0.1f, 0f, num8, -1f)));
							flag = true;
						}
						if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.OneHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedAxe || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.ThrowingAxe)
						{
							WeaponDesignElement weaponDesignElement3 = craftedWeaponData.UsedPieces[0];
							float num9 = craftedWeaponData.PiecePivotDistances[0];
							float num10 = num9 + weaponDesignElement3.CraftingPiece.Length * 0.8f;
							float num11 = num9 - weaponDesignElement3.CraftingPiece.Length * 0.8f;
							float num12 = num9 + weaponDesignElement3.CraftingPiece.Length;
							float num13 = num9 - weaponDesignElement3.CraftingPiece.Length;
							float bladeWidth = weaponDesignElement3.CraftingPiece.BladeData.BladeWidth;
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(0f, 0f, num10, -1f), new Vec3(-bladeWidth, 0f, num12, -1f)));
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(0f, 0f, num11, -1f), new Vec3(-bladeWidth, 0f, num13, -1f)));
							physicsShape2.AddCapsule(new CapsuleData(0.05f, new Vec3(-bladeWidth, 0f, num12, -1f), new Vec3(-bladeWidth, 0f, num13, -1f)));
							flag = true;
						}
						if (weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.TwoHandedPolearm || weaponComponent.PrimaryWeapon.WeaponClass == WeaponClass.Javelin)
						{
							float num14 = craftedWeaponData.PiecePivotDistances[0];
							physicsShape2.AddCapsule(new CapsuleData(0.025f, new Vec3(-0.05f, 0f, num14, -1f), new Vec3(0.05f, 0f, num14, -1f)));
							flag = true;
						}
						if (!flag)
						{
							physicsShape2.AddCapsule(new CapsuleData(0.025f, new Vec3(-0.05f, 0f, num4, -1f), new Vec3(0.05f, 0f, num4, -1f)));
						}
					}
					else
					{
						weaponData.Shape.Prepare();
						int num15 = physicsShape.CapsuleCount();
						if (num15 == 0)
						{
							Debug.FailedAssert("Item has 0 body parts. Applying a default body, but this should not happen! Check this!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2343);
							return;
						}
						switch (weaponComponent.PrimaryWeapon.WeaponClass)
						{
						case WeaponClass.Dagger:
						case WeaponClass.OneHandedSword:
						case WeaponClass.TwoHandedSword:
						case WeaponClass.ThrowingKnife:
						{
							CapsuleData capsuleData = default(CapsuleData);
							physicsShape2.GetCapsule(ref capsuleData, 0);
							float radius = capsuleData.Radius;
							Vec3 p = capsuleData.P1;
							Vec3 p2 = capsuleData.P2;
							physicsShape2.SetCapsule(new CapsuleData(radius, new Vec3(p.x, p.y, p.z * num, -1f), p2), 0);
							break;
						}
						case WeaponClass.OneHandedAxe:
						case WeaponClass.TwoHandedAxe:
						case WeaponClass.Mace:
						case WeaponClass.TwoHandedMace:
						case WeaponClass.OneHandedPolearm:
						case WeaponClass.TwoHandedPolearm:
						case WeaponClass.LowGripPolearm:
						case WeaponClass.Arrow:
						case WeaponClass.Bolt:
						case WeaponClass.Crossbow:
						case WeaponClass.ThrowingAxe:
						case WeaponClass.Javelin:
						case WeaponClass.Banner:
						{
							CapsuleData capsuleData2 = default(CapsuleData);
							physicsShape2.GetCapsule(ref capsuleData2, 0);
							float radius2 = capsuleData2.Radius;
							Vec3 p3 = capsuleData2.P1;
							Vec3 p4 = capsuleData2.P2;
							physicsShape2.SetCapsule(new CapsuleData(radius2, new Vec3(p3.x, p3.y, p3.z * num, -1f), p4), 0);
							for (int j = 1; j < num15; j++)
							{
								CapsuleData capsuleData3 = default(CapsuleData);
								physicsShape2.GetCapsule(ref capsuleData3, j);
								float radius3 = capsuleData3.Radius;
								Vec3 p5 = capsuleData3.P1;
								Vec3 p6 = capsuleData3.P2;
								physicsShape2.SetCapsule(new CapsuleData(radius3, new Vec3(p5.x, p5.y, p5.z * num, -1f), new Vec3(p6.x, p6.y, p6.z * num, -1f)), j);
							}
							break;
						}
						case WeaponClass.SmallShield:
						case WeaponClass.LargeShield:
							Debug.FailedAssert("Shields should not have recalculate body flag.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RecalculateBody", 2417);
							break;
						}
					}
				}
			}
			weaponData.CenterOfMassShift = weaponData.Shape.GetWeaponCenterOfMass();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00057F84 File Offset: 0x00056184
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void OnFixedTick(float fixedDt)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnFixedMissionTick(fixedDt);
			}
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00057FBC File Offset: 0x000561BC
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnPreTick(float dt)
		{
			this.WaitTickCompletion();
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnPreMissionTick(dt);
			}
			this.TickDebugAgents();
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00058000 File Offset: 0x00056200
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void ApplySkeletonScaleToAllEquippedItems(string itemName)
		{
			int count = this.Agents.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < 12; j++)
				{
					EquipmentElement equipmentElement = this.Agents[i].SpawnEquipment[j];
					if (!equipmentElement.IsEmpty && equipmentElement.Item.StringId == itemName)
					{
						HorseComponent horseComponent = equipmentElement.Item.HorseComponent;
						if (((horseComponent != null) ? horseComponent.SkeletonScale : null) != null)
						{
							this.Agents[i].AgentVisuals.ApplySkeletonScale(equipmentElement.Item.HorseComponent.SkeletonScale.MountSitBoneScale, equipmentElement.Item.HorseComponent.SkeletonScale.MountRadiusAdder, equipmentElement.Item.HorseComponent.SkeletonScale.BoneIndices, equipmentElement.Item.HorseComponent.SkeletonScale.Scales);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00058104 File Offset: 0x00056304
		[CommandLineFunctionality.CommandLineArgumentFunction("set_facial_anim_to_agent", "mission")]
		public static string SetFacialAnimToAgent(List<string> strings)
		{
			Mission mission = Mission.Current;
			if (mission == null)
			{
				return "Mission could not be found";
			}
			if (strings.Count != 2)
			{
				return "Enter agent index and animation name please";
			}
			int num;
			if (int.TryParse(strings[0], out num) && num >= 0)
			{
				foreach (Agent agent in mission.Agents)
				{
					if (agent.Index == num)
					{
						agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.High, strings[1], true);
						return "Done";
					}
				}
			}
			return "Please enter a valid agent index";
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000581AC File Offset: 0x000563AC
		private void WaitTickCompletion()
		{
			while (!this.tickCompleted)
			{
				Thread.Sleep(1);
			}
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x000581C0 File Offset: 0x000563C0
		private void AgentTickMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.AllAgents[i].TickParallel(dt);
			}
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x000581EC File Offset: 0x000563EC
		public void TickAgentsAndTeamsImp(float dt, bool tickPaused)
		{
			float num = (tickPaused ? 0f : dt);
			TWParallel.For(0, this.AllAgents.Count, num, new TWParallel.ParallelForWithDtAuxPredicate(this.AgentTickMT), 16);
			foreach (Agent agent in this.AllAgents)
			{
				agent.Tick(num);
			}
			foreach (Team team in this.Teams)
			{
				team.Tick(dt);
			}
			this.tickCompleted = true;
			foreach (MBSubModuleBase mbsubModuleBase in this._cachedSubModuleList)
			{
				mbsubModuleBase.AfterAsyncTickTick(dt);
			}
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000582F0 File Offset: 0x000564F0
		[CommandLineFunctionality.CommandLineArgumentFunction("formation_speed_adjustment_enabled", "ai")]
		public static string EnableSpeedAdjustmentCommand(List<string> strings)
		{
			if (!GameNetwork.IsSessionActive)
			{
				HumanAIComponent.FormationSpeedAdjustmentEnabled = !HumanAIComponent.FormationSpeedAdjustmentEnabled;
				string text = "Speed Adjustment ";
				if (HumanAIComponent.FormationSpeedAdjustmentEnabled)
				{
					text += "enabled";
				}
				else
				{
					text += "disabled";
				}
				return text;
			}
			return "Does not work on multiplayer.";
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00058340 File Offset: 0x00056540
		public void OnTick(float dt, float realDt, bool updateCamera, bool doAsyncAITick)
		{
			this.ApplyGeneratedCombatLogs();
			if (this.InputManager == null)
			{
				this.InputManager = new EmptyInputContext();
			}
			for (int i = 0; i < this._tickActions.Count; i++)
			{
				ValueTuple<Mission.MissionTickAction, Agent, int, int> valueTuple = this._tickActions[i];
				Agent item = valueTuple.Item2;
				if (item.IsActive())
				{
					switch (valueTuple.Item1)
					{
					case Mission.MissionTickAction.TryToSheathWeaponInHand:
						item.TryToSheathWeaponInHand((Agent.HandIndex)valueTuple.Item3, (Agent.WeaponWieldActionType)valueTuple.Item4);
						break;
					case Mission.MissionTickAction.RemoveEquippedWeapon:
						item.RemoveEquippedWeapon((EquipmentIndex)valueTuple.Item3);
						break;
					case Mission.MissionTickAction.TryToWieldWeaponInSlot:
						item.TryToWieldWeaponInSlot((EquipmentIndex)valueTuple.Item3, (Agent.WeaponWieldActionType)valueTuple.Item4, false);
						break;
					case Mission.MissionTickAction.DropItem:
						if (!item.Equipment[valueTuple.Item3].IsEmpty)
						{
							item.DropItem((EquipmentIndex)valueTuple.Item3, WeaponClass.Undefined);
						}
						break;
					case Mission.MissionTickAction.RegisterDrownBlow:
					{
						Blow blow = new Blow(item.Index);
						blow.DamageType = DamageTypes.Blunt;
						blow.BoneIndex = item.Monster.HeadLookDirectionBoneIndex;
						blow.BaseMagnitude = 10f;
						blow.GlobalPosition = item.Position;
						blow.GlobalPosition.z = blow.GlobalPosition.z + item.GetEyeGlobalHeight();
						blow.DamagedPercentage = 1f;
						blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
						blow.SwingDirection = item.LookDirection;
						blow.Direction = blow.SwingDirection;
						blow.InflictedDamage = 10;
						blow.DamageCalculated = true;
						sbyte mainHandItemBoneIndex = item.Monster.MainHandItemBoneIndex;
						AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, item.Velocity, Vec3.Up);
						item.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
						item.MakeVoice(SkinVoiceManager.VoiceType.Drown, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
						if (item.Controller == AgentControllerType.AI)
						{
							Agent agent = item;
							Vec3 vec = new Vec3(0f, 0f, -20f, -1f);
							agent.AddAcceleration(in vec);
						}
						CombatLogData combatLogData = new CombatLogData(true, true, item.IsMine, false, false, false, true, item.IsMine, item.Health <= 0f, false, false, false, null, true, false, false, 0f);
						combatLogData.InflictedDamage = blow.InflictedDamage;
						combatLogData.IsSpecialDamage = true;
						Mission.Current.AddCombatLogSafe(item, item, combatLogData);
						break;
					}
					case Mission.MissionTickAction.RegisterBurnBlow:
						if (item.Health - 2f > 1f)
						{
							item.Health -= 2f;
						}
						else
						{
							Blow blow2 = new Blow(item.Index);
							blow2.DamageType = DamageTypes.Blunt;
							blow2.BaseMagnitude = item.Health + 1f;
							blow2.InflictedDamage = (int)blow2.BaseMagnitude;
							blow2.GlobalPosition = item.Position;
							blow2.GlobalPosition.z = blow2.GlobalPosition.z + item.GetEyeGlobalHeight();
							blow2.DamagedPercentage = 1f;
							item.Die(blow2, Agent.KillInfo.Invalid);
						}
						break;
					}
				}
			}
			this._tickActions.Clear();
			this.MissionTimeTracker.Tick(dt);
			this.CheckMissionEnd(this.CurrentTime);
			if (this.IsFastForward && this.MissionEnded)
			{
				this.IsFastForward = false;
			}
			if (this.CurrentState == Mission.State.Continuing)
			{
				if (this._inMissionLoadingScreenTimer != null && this._inMissionLoadingScreenTimer.Check(this.CurrentTime))
				{
					this._inMissionLoadingScreenTimer = null;
					Action onLoadingEndedAction = this._onLoadingEndedAction;
					if (onLoadingEndedAction != null)
					{
						onLoadingEndedAction();
					}
					LoadingWindow.DisableGlobalLoadingWindow();
				}
				for (int j = this.MissionBehaviors.Count - 1; j >= 0; j--)
				{
					this.MissionBehaviors[j].OnPreDisplayMissionTick(dt);
				}
				if (!GameNetwork.IsDedicatedServer && updateCamera)
				{
					this._missionState.Handler.UpdateCamera(this, realDt);
				}
				this.tickCompleted = false;
				for (int k = this.MissionBehaviors.Count - 1; k >= 0; k--)
				{
					this.MissionBehaviors[k].OnMissionTick(dt);
				}
				for (int l = this._dynamicEntities.Count - 1; l >= 0; l--)
				{
					Mission.DynamicEntityInfo dynamicEntityInfo = this._dynamicEntities[l];
					if (dynamicEntityInfo.TimerToDisable.Check(this.CurrentTime))
					{
						dynamicEntityInfo.Entity.RemoveEnginePhysics();
						dynamicEntityInfo.Entity.Remove(79);
						this._dynamicEntities.RemoveAt(l);
					}
				}
				this.HandleSpawnedItems();
				DebugNetworkEventStatistics.EndTick(dt);
				if (this.CurrentState == Mission.State.Continuing && this.IsFriendlyMission && !this.IsInPhotoMode)
				{
					if (this.InputManager.IsGameKeyDown(4))
					{
						this.OnEndMissionRequest();
					}
					else
					{
						this._leaveMissionTimer = null;
					}
				}
				if (doAsyncAITick)
				{
					this.TickAgentsAndTeamsAsync(dt);
					return;
				}
				this.TickAgentsAndTeamsImp(dt, false);
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x0005884E File Offset: 0x00056A4E
		public void AddTickAction(Mission.MissionTickAction action, Agent agent, int param1, int param2)
		{
			this._tickActions.Add(new ValueTuple<Mission.MissionTickAction, Agent, int, int>(action, agent, param1, param2));
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00058868 File Offset: 0x00056A68
		public void AddTickActionMT(Mission.MissionTickAction action, Agent agent, int param1, int param2)
		{
			object tickActionsLock = this._tickActionsLock;
			lock (tickActionsLock)
			{
				this._tickActions.Add(new ValueTuple<Mission.MissionTickAction, Agent, int, int>(action, agent, param1, param2));
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000588B8 File Offset: 0x00056AB8
		public void RemoveSpawnedItemsAndMissiles()
		{
			this.ClearMissiles();
			this._missilesList.Clear();
			this._missilesDictionary.Clear();
			this.RemoveSpawnedMissionObjects();
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000588DC File Offset: 0x00056ADC
		public void AfterStart()
		{
			this._activeAgents.Clear();
			this._allAgents.Clear();
			this._tickActions.Clear();
			this._cachedSubModuleList = Module.CurrentModule.CollectSubModules();
			foreach (MBSubModuleBase mbsubModuleBase in this._cachedSubModuleList)
			{
				mbsubModuleBase.OnBeforeMissionBehaviorInitialize(this);
			}
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				this.MissionBehaviors[i].OnBehaviorInitialize();
			}
			foreach (MBSubModuleBase mbsubModuleBase2 in this._cachedSubModuleList)
			{
				mbsubModuleBase2.OnMissionBehaviorInitialize(this);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.EarlyStart();
			}
			this._battleSpawnPathSelector.Initialize();
			this._deploymentPlan.Initialize();
			foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
			{
				missionBehavior2.AfterStart();
			}
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				missionObject.AfterMissionStart();
			}
			if (MissionGameModels.Current.ApplyWeatherEffectsModel != null)
			{
				MissionGameModels.Current.ApplyWeatherEffectsModel.ApplyWeatherEffects();
			}
			this.CurrentState = Mission.State.Continuing;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00058ABC File Offset: 0x00056CBC
		public void OnEndMissionRequest()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				bool flag;
				InquiryData inquiryData = missionLogic.OnEndMissionRequest(out flag);
				if (!flag)
				{
					this._leaveMissionTimer = null;
					return;
				}
				if (inquiryData != null)
				{
					this._leaveMissionTimer = null;
					InformationManager.ShowInquiry(inquiryData, true, false);
					return;
				}
			}
			if (this._leaveMissionTimer != null)
			{
				if (this._leaveMissionTimer.ElapsedTime > 0.6f)
				{
					this._leaveMissionTimer = null;
					this.EndMission();
					return;
				}
			}
			else
			{
				this._leaveMissionTimer = new BasicMissionTimer();
			}
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00058B64 File Offset: 0x00056D64
		public float GetMissionEndTimeInSeconds()
		{
			return 0.6f;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00058B6B File Offset: 0x00056D6B
		public float GetMissionEndTimerValue()
		{
			if (this._leaveMissionTimer == null)
			{
				return -1f;
			}
			return this._leaveMissionTimer.ElapsedTime;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00058B88 File Offset: 0x00056D88
		private void ApplyGeneratedCombatLogs()
		{
			if (!this._combatLogsCreated.IsEmpty)
			{
				CombatLogData combatLogData;
				while (this._combatLogsCreated.TryDequeue(out combatLogData))
				{
					CombatLogManager.GenerateCombatLog(combatLogData);
				}
			}
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00058BBC File Offset: 0x00056DBC
		public int GetMemberCountOfSide(BattleSideEnum side)
		{
			int num = 0;
			foreach (Team team in this.Teams)
			{
				if (team.Side == side)
				{
					num += team.ActiveAgents.Count;
				}
			}
			return num;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00058C24 File Offset: 0x00056E24
		public Path GetInitialSpawnPath()
		{
			return this._battleSpawnPathSelector.InitialPath;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x00058C34 File Offset: 0x00056E34
		public SpawnPathData GetInitialSpawnPathData(BattleSideEnum battleSide)
		{
			SpawnPathData spawnPathData;
			this._battleSpawnPathSelector.GetInitialPathDataOfSide(battleSide, out spawnPathData);
			return spawnPathData;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00058C51 File Offset: 0x00056E51
		public MBReadOnlyList<SpawnPathData> GetReinforcementPathsDataOfSide(BattleSideEnum battleSide)
		{
			return this._battleSpawnPathSelector.GetReinforcementPathsDataOfSide(battleSide);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00058C60 File Offset: 0x00056E60
		public void GetTroopSpawnFrameWithIndex(AgentBuildData buildData, int troopSpawnIndex, int troopSpawnCount, out Vec3 troopSpawnPosition, out Vec2 troopSpawnDirection)
		{
			Formation agentFormation = buildData.AgentFormation;
			BasicCharacterObject agentCharacter = buildData.AgentCharacter;
			troopSpawnPosition = Vec3.Invalid;
			WorldPosition worldPosition;
			Vec2 direction;
			if (buildData.AgentSpawnsIntoOwnFormation)
			{
				worldPosition = agentFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
				direction = agentFormation.Direction;
			}
			else
			{
				IAgentOriginBase agentOrigin = buildData.AgentOrigin;
				bool agentIsReinforcement = buildData.AgentIsReinforcement;
				Team agentTeam = buildData.AgentTeam;
				BattleSideEnum side = agentTeam.Side;
				if (buildData.AgentSpawnsUsingOwnTroopClass)
				{
					FormationClass agentTroopClass = this.GetAgentTroopClass(side, agentCharacter);
					this.GetFormationSpawnFrame(agentTeam, agentTroopClass, agentIsReinforcement, out worldPosition, out direction, true);
				}
				else if (agentCharacter.IsHero && agentOrigin != null && agentOrigin.BattleCombatant != null && agentCharacter == agentOrigin.BattleCombatant.General && this.GetFormationSpawnClass(agentTeam, FormationClass.NumberOfRegularFormations, agentIsReinforcement) == FormationClass.NumberOfRegularFormations)
				{
					this.GetFormationSpawnFrame(agentTeam, FormationClass.NumberOfRegularFormations, agentIsReinforcement, out worldPosition, out direction, true);
				}
				else
				{
					this.GetFormationSpawnFrame(agentTeam, agentFormation.FormationIndex, agentIsReinforcement, out worldPosition, out direction, true);
				}
			}
			bool flag = !buildData.AgentNoHorses && agentFormation.HasAnyMountedUnit;
			WorldPosition? worldPosition2;
			Vec2? vec;
			agentFormation.GetUnitSpawnFrameWithIndex(troopSpawnIndex, in worldPosition, in direction, agentFormation.Width, troopSpawnCount, agentFormation.UnitSpacing, flag, out worldPosition2, out vec);
			if (worldPosition2 != null && buildData.MakeUnitStandOutDistance != 0f)
			{
				worldPosition2.Value.SetVec2(worldPosition2.Value.AsVec2 + vec.Value * buildData.MakeUnitStandOutDistance);
			}
			if (worldPosition2 != null)
			{
				if (worldPosition2.Value.GetNavMesh() == UIntPtr.Zero)
				{
					troopSpawnPosition = this.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref worldPosition, worldPosition2.Value.AsVec2);
				}
				else
				{
					troopSpawnPosition = worldPosition2.Value.GetGroundVec3();
				}
			}
			if (!troopSpawnPosition.IsValid)
			{
				troopSpawnPosition = worldPosition.GetGroundVec3();
			}
			troopSpawnDirection = ((vec != null) ? vec.Value : direction);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00058E54 File Offset: 0x00057054
		public void GetFormationSpawnFrame(Team team, FormationClass formationClass, bool isReinforcement, out WorldPosition spawnPosition, out Vec2 spawnDirection, bool useDefaultClassIfNotFound = true)
		{
			IFormationDeploymentPlan formationDeploymentPlan = this._deploymentPlan.GetFormationPlan(team, formationClass, isReinforcement);
			if (!formationDeploymentPlan.HasFrame() && useDefaultClassIfNotFound && !formationClass.IsDefaultFormationClass())
			{
				FormationClass formationClass2 = formationClass.DefaultClass();
				formationDeploymentPlan = this._deploymentPlan.GetFormationPlan(team, formationClass2, isReinforcement);
			}
			if (formationDeploymentPlan.HasFrame())
			{
				spawnPosition = formationDeploymentPlan.CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
				spawnDirection = formationDeploymentPlan.GetDirection();
				return;
			}
			MatrixFrame deploymentFrame = this._deploymentPlan.GetDeploymentFrame(team);
			spawnPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, deploymentFrame.origin, false);
			spawnDirection = deploymentFrame.rotation.f.AsVec2.Normalized();
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00058F10 File Offset: 0x00057110
		public WorldFrame GetSpawnPathFrame(BattleSideEnum battleSide, float pathOffset = 0f, float targetOffset = 0f)
		{
			SpawnPathData initialSpawnPathData = this.GetInitialSpawnPathData(battleSide);
			if (initialSpawnPathData.IsValid)
			{
				Vec2 vec;
				Vec2 vec2;
				initialSpawnPathData.GetSpawnPathFrameFacingTarget(pathOffset, targetOffset, false, out vec, out vec2, false, 0.2f);
				Mat3 identity = Mat3.Identity;
				identity.RotateAboutUp(vec2.RotationInRadians);
				WorldPosition worldPosition = new WorldPosition(this.Scene, UIntPtr.Zero, vec.ToVec3(0f), false);
				return new WorldFrame(identity, worldPosition);
			}
			return WorldFrame.Invalid;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00058F84 File Offset: 0x00057184
		private void BuildAgent(Agent agent, AgentBuildData agentBuildData)
		{
			if (agent == null)
			{
				throw new MBNullParameterException("agent");
			}
			agent.Build(agentBuildData);
			if (agent.Controller == AgentControllerType.Player)
			{
				this._initialPlayerAgent = agent;
			}
			if (!agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				EquipmentElement equipmentElement = agent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot];
				if (equipmentElement.Item.HorseComponent.BodyLength != 0)
				{
					agent.SetInitialAgentScale(0.01f * (float)equipmentElement.Item.HorseComponent.BodyLength);
				}
			}
			agent.EquipItemsFromSpawnEquipment(true, (agentBuildData != null && agentBuildData.PrepareImmediately) || agent == Agent.Main, agentBuildData != null && agentBuildData.UseFaceCache, (agentBuildData != null) ? agentBuildData.FaceCacheId : 0);
			agent.InitializeAgentRecord();
			agent.AgentVisuals.BatchLastLodMeshes();
			agent.PreloadForRendering();
			ActionIndexCache currentAction = agent.GetCurrentAction(0);
			if (currentAction != ActionIndexCache.act_none)
			{
				agent.SetActionChannel(0, in currentAction, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, MBRandom.RandomFloat * 0.8f, false, -0.2f, 0, true);
			}
			agent.InitializeComponents();
			if (agent.Controller == AgentControllerType.Player)
			{
				this.ResetFirstThirdPersonView();
			}
			this._activeAgents.Add(agent);
			this._allAgents.Add(agent);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000590D0 File Offset: 0x000572D0
		private Agent CreateAgent(Monster monster, bool isFemale, int instanceNo, Agent.CreationType creationType, float stepSize, int forcedAgentIndex, int weight, BasicCharacterObject characterObject)
		{
			AnimationSystemData animationSystemData = monster.FillAnimationSystemData(stepSize, false, isFemale);
			AgentCapsuleData agentCapsuleData = monster.FillCapsuleData();
			AgentSpawnData agentSpawnData = monster.FillSpawnData(null);
			Mission.AgentCreationResult agentCreationResult = this.CreateAgentInternal(monster.Flags, forcedAgentIndex, isFemale, ref agentSpawnData, ref agentCapsuleData, ref animationSystemData, instanceNo);
			Agent agent = new Agent(this, agentCreationResult, creationType, monster, this._agentCreationIndex);
			this._agentCreationIndex++;
			agent.Character = characterObject;
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentCreated(agent);
			}
			return agent;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00059180 File Offset: 0x00057380
		public void SetBattleAgentCount(int agentCount)
		{
			if (this._agentCount == 0 || this._agentCount > agentCount)
			{
				this._agentCount = agentCount;
			}
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0005919C File Offset: 0x0005739C
		public Vec2 GetFormationSpawnPosition(Team team, FormationClass formationClass)
		{
			return this._deploymentPlan.GetFormationPlan(team, formationClass, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.None).AsVec2;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000591C5 File Offset: 0x000573C5
		public FormationClass GetFormationSpawnClass(Team team, FormationClass formationClass, bool isReinforcement = false)
		{
			return this._deploymentPlan.GetFormationPlan(team, formationClass, isReinforcement).SpawnClass;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000591DC File Offset: 0x000573DC
		public Agent SpawnAgent(AgentBuildData agentBuildData, bool spawnFromAgentVisuals = false)
		{
			this.Scene.WaitWaterRendererCPUSimulation();
			BasicCharacterObject agentCharacter = agentBuildData.AgentCharacter;
			if (agentCharacter == null)
			{
				throw new MBNullParameterException("npcCharacterObject");
			}
			int num = -1;
			if (agentBuildData.AgentIndexOverriden)
			{
				num = agentBuildData.AgentIndex;
			}
			Agent agent = this.CreateAgent(agentBuildData.AgentMonster, agentBuildData.GenderOverriden ? agentBuildData.AgentIsFemale : agentCharacter.IsFemale, 0, Agent.CreationType.FromCharacterObj, agentCharacter.GetStepSize(), num, agentBuildData.AgentMonster.Weight, agentCharacter);
			agent.FormationPositionPreference = agentCharacter.FormationPositionPreference;
			float num2 = (agentBuildData.AgeOverriden ? ((float)agentBuildData.AgentAge) : agentCharacter.Age);
			if (num2 == 0f)
			{
				agentBuildData.Age(29);
			}
			else if (MBBodyProperties.GetMaturityType(num2) < BodyMeshMaturityType.Teenager && (this.Mode == MissionMode.Battle || this.Mode == MissionMode.Duel || this.Mode == MissionMode.Tournament || this.Mode == MissionMode.Stealth))
			{
				agentBuildData.Age(27);
			}
			if (agentBuildData.BodyPropertiesOverriden)
			{
				agent.UpdateBodyProperties(agentBuildData.AgentBodyProperties);
				if (!agentBuildData.AgeOverriden)
				{
					agent.Age = agentCharacter.Age;
				}
			}
			agent.BodyPropertiesSeed = agentBuildData.AgentEquipmentSeed;
			if (agentBuildData.AgeOverriden)
			{
				agent.Age = (float)agentBuildData.AgentAge;
			}
			if (agentBuildData.GenderOverriden)
			{
				agent.IsFemale = agentBuildData.AgentIsFemale;
			}
			agent.SetTeam(agentBuildData.AgentTeam, false);
			agent.SetClothingColor1(agentBuildData.AgentClothingColor1);
			agent.SetClothingColor2(agentBuildData.AgentClothingColor2);
			agent.SetRandomizeColors(agentBuildData.RandomizeColors);
			agent.Origin = agentBuildData.AgentOrigin;
			Formation agentFormation = agentBuildData.AgentFormation;
			if (agentFormation != null && !agentFormation.HasBeenPositioned)
			{
				if (this._deploymentPlan.IsPlanMade(agentFormation.Team))
				{
					this.SetFormationPositioningFromDeploymentPlan(agentFormation);
				}
				else
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene.Pointer, UIntPtr.Zero, agentBuildData.AgentInitialPosition.Value, false);
					agentFormation.SetPositioning(new WorldPosition?(worldPosition), null, null);
				}
			}
			if (agentBuildData.AgentInitialPosition == null)
			{
				Team agentTeam = agentBuildData.AgentTeam;
				BattleSideEnum side = agentBuildData.AgentTeam.Side;
				Vec3 vec = Vec3.Invalid;
				Vec2 vec2 = Vec2.Invalid;
				if (agentCharacter == Game.Current.PlayerTroop && this._deploymentPlan.HasPlayerSpawnFrame(side))
				{
					WorldPosition worldPosition2;
					Vec2 vec3;
					this._deploymentPlan.GetPlayerSpawnFrame(side, out worldPosition2, out vec3);
					vec = worldPosition2.GetGroundVec3();
					vec2 = vec3;
				}
				else if (agentFormation != null)
				{
					int num3;
					int num4;
					if (agentBuildData.AgentSpawnsIntoOwnFormation)
					{
						num3 = agentFormation.CountOfUnits;
						num4 = num3 + 1;
					}
					else if (agentBuildData.AgentFormationTroopSpawnIndex >= 0 && agentBuildData.AgentFormationTroopSpawnCount > 0)
					{
						num3 = agentBuildData.AgentFormationTroopSpawnIndex;
						num4 = agentBuildData.AgentFormationTroopSpawnCount;
					}
					else
					{
						num3 = agentFormation.GetNextSpawnIndex();
						num4 = num3 + 1;
					}
					if (num3 >= num4)
					{
						num4 = num3 + 1;
					}
					this.GetTroopSpawnFrameWithIndex(agentBuildData, num3, num4, out vec, out vec2);
				}
				else
				{
					WorldPosition worldPosition3;
					this.GetFormationSpawnFrame(agentTeam, FormationClass.NumberOfAllFormations, agentBuildData.AgentIsReinforcement, out worldPosition3, out vec2, true);
					vec = worldPosition3.GetGroundVec3();
				}
				agentBuildData.InitialPosition(in vec).InitialDirection(in vec2);
			}
			Agent agent2 = agent;
			Vec3 vec4 = agentBuildData.AgentInitialPosition.GetValueOrDefault();
			Vec2 vec5 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
			agent2.SetInitialFrame(in vec4, in vec5, agentBuildData.AgentCanSpawnOutsideOfMissionBoundary);
			if (agentCharacter.BattleEquipments == null && agentCharacter.CivilianEquipments == null)
			{
				Debug.Print("characterObject.AllEquipments is null for \"" + agentCharacter.StringId + "\".", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			if (agentCharacter.BattleEquipments != null)
			{
				if (agentCharacter.BattleEquipments.Any<Equipment>((Equipment eq) => eq == null) && agentCharacter.CivilianEquipments != null)
				{
					if (agentCharacter.CivilianEquipments.Any<Equipment>((Equipment eq) => eq == null))
					{
						Debug.Print("Character with id \"" + agentCharacter.StringId + "\" has a null equipment in its AllEquipments.", 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
			if (agentCharacter.CivilianEquipments == null)
			{
				agentBuildData.CivilianEquipment(false);
			}
			if (agentCharacter.IsHero)
			{
				agentBuildData.FixedEquipment(true);
			}
			Equipment equipment;
			if (agentBuildData.AgentOverridenSpawnEquipment != null)
			{
				equipment = agentBuildData.AgentOverridenSpawnEquipment.Clone(false);
			}
			else if (!agentBuildData.AgentFixedEquipment)
			{
				equipment = Equipment.GetRandomEquipmentElements(agent.Character, !Game.Current.GameType.IsCoreOnlyGameMode, agentBuildData.AgentCivilianEquipment ? Equipment.EquipmentType.Civilian : Equipment.EquipmentType.Battle, agentBuildData.AgentEquipmentSeed);
			}
			else if (agentBuildData.AgentCivilianEquipment)
			{
				equipment = agentCharacter.FirstCivilianEquipment.Clone(false);
			}
			else
			{
				equipment = agentCharacter.FirstBattleEquipment.Clone(false);
			}
			Agent agent3 = null;
			if (agentBuildData.AgentNoHorses)
			{
				equipment[EquipmentIndex.ArmorItemEndSlot] = default(EquipmentElement);
				equipment[EquipmentIndex.HorseHarness] = default(EquipmentElement);
			}
			if (agentBuildData.AgentNoWeapons)
			{
				equipment[EquipmentIndex.WeaponItemBeginSlot] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon1] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon2] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon3] = default(EquipmentElement);
				equipment[EquipmentIndex.ExtraWeaponSlot] = default(EquipmentElement);
			}
			if (agentCharacter.IsHero)
			{
				ItemObject itemObject = null;
				ItemObject item = equipment[EquipmentIndex.ExtraWeaponSlot].Item;
				if (item != null && item.IsBannerItem && item.BannerComponent != null)
				{
					itemObject = item;
					equipment[EquipmentIndex.ExtraWeaponSlot] = default(EquipmentElement);
				}
				else if (agentBuildData.AgentBannerItem != null)
				{
					itemObject = agentBuildData.AgentBannerItem;
				}
				if (itemObject != null)
				{
					agent.SetFormationBanner(itemObject);
				}
			}
			else if (agentBuildData.AgentBannerItem != null)
			{
				equipment[EquipmentIndex.Weapon1] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon2] = default(EquipmentElement);
				equipment[EquipmentIndex.Weapon3] = default(EquipmentElement);
				if (agentBuildData.AgentBannerReplacementWeaponItem != null)
				{
					equipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(agentBuildData.AgentBannerReplacementWeaponItem, null, null, false);
				}
				else
				{
					equipment[EquipmentIndex.WeaponItemBeginSlot] = default(EquipmentElement);
				}
				equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(agentBuildData.AgentBannerItem, null, null, false);
				if (agentBuildData.AgentOverridenSpawnMissionEquipment != null)
				{
					agentBuildData.AgentOverridenSpawnMissionEquipment[EquipmentIndex.ExtraWeaponSlot] = new MissionWeapon(agentBuildData.AgentBannerItem, null, agentBuildData.AgentBanner);
				}
			}
			if (agentBuildData.AgentNoArmor)
			{
				equipment[EquipmentIndex.Gloves] = default(EquipmentElement);
				equipment[EquipmentIndex.Body] = default(EquipmentElement);
				equipment[EquipmentIndex.Cape] = default(EquipmentElement);
				equipment[EquipmentIndex.NumAllWeaponSlots] = default(EquipmentElement);
				equipment[EquipmentIndex.Leg] = default(EquipmentElement);
			}
			for (int i = 0; i < 5; i++)
			{
				if (!equipment[(EquipmentIndex)i].IsEmpty && equipment[(EquipmentIndex)i].Item.ItemFlags.HasAnyFlag(ItemFlags.CannotBePickedUp))
				{
					equipment[(EquipmentIndex)i] = default(EquipmentElement);
				}
			}
			agent.InitializeSpawnEquipment(equipment);
			agent.InitializeMissionEquipment(agentBuildData.AgentOverridenSpawnMissionEquipment, agentBuildData.AgentBanner);
			if (agent.RandomizeColors)
			{
				agent.Equipment.SetGlossMultipliersOfWeaponsRandomly(agentBuildData.AgentEquipmentSeed);
			}
			ItemObject item2 = equipment[EquipmentIndex.ArmorItemEndSlot].Item;
			if (item2 != null && item2.HasHorseComponent && item2.HorseComponent.IsRideable)
			{
				int num5 = -1;
				if (agentBuildData.AgentMountIndexOverriden)
				{
					num5 = agentBuildData.AgentMountIndex;
				}
				EquipmentElement equipmentElement = equipment[EquipmentIndex.ArmorItemEndSlot];
				EquipmentElement equipmentElement2 = equipment[EquipmentIndex.HorseHarness];
				vec4 = agentBuildData.AgentInitialPosition.GetValueOrDefault();
				vec5 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
				agent3 = this.CreateHorseAgentFromRosterElements(equipmentElement, equipmentElement2, in vec4, in vec5, num5, agentBuildData.AgentMountKey);
				Equipment equipment2 = new Equipment();
				equipment2[EquipmentIndex.ArmorItemEndSlot] = equipment[EquipmentIndex.ArmorItemEndSlot];
				equipment2[EquipmentIndex.HorseHarness] = equipment[EquipmentIndex.HorseHarness];
				Equipment equipment3 = equipment2;
				agent3.InitializeSpawnEquipment(equipment3);
				agent.SetMountAgentBeforeBuild(agent3);
			}
			if (spawnFromAgentVisuals || !GameNetwork.IsClientOrReplay)
			{
				agent.Equipment.CheckLoadedAmmos();
			}
			if (!agentBuildData.BodyPropertiesOverriden)
			{
				BodyProperties bodyProperties;
				if (this.OnComputeTroopBodyProperties != null)
				{
					bodyProperties = this.OnComputeTroopBodyProperties(agentBuildData, agentCharacter, equipment, agentBuildData.AgentEquipmentSeed);
					agentBuildData.UseFaceCache = !agentCharacter.IsHero;
				}
				else
				{
					bodyProperties = agentCharacter.GetBodyProperties(equipment, agentBuildData.AgentEquipmentSeed);
				}
				agent.UpdateBodyProperties(bodyProperties);
			}
			if (GameNetwork.IsServerOrRecorder && agent.RiderAgent == null)
			{
				Vec3 valueOrDefault = agentBuildData.AgentInitialPosition.GetValueOrDefault();
				Vec2 valueOrDefault2 = agentBuildData.AgentInitialDirection.GetValueOrDefault();
				if (agent.IsMount)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new CreateFreeMountAgent(agent, valueOrDefault, valueOrDefault2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				else
				{
					bool flag = agentBuildData.AgentMissionPeer != null;
					NetworkCommunicator networkCommunicator;
					if (!flag)
					{
						MissionPeer owningAgentMissionPeer = agentBuildData.OwningAgentMissionPeer;
						networkCommunicator = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.GetNetworkPeer() : null);
					}
					else
					{
						networkCommunicator = agentBuildData.AgentMissionPeer.GetNetworkPeer();
					}
					NetworkCommunicator networkCommunicator2 = networkCommunicator;
					bool flag2 = agent.MountAgent != null && agent.MountAgent.RiderAgent == agent;
					GameNetwork.BeginBroadcastModuleEvent();
					int index = agent.Index;
					BasicCharacterObject character = agent.Character;
					Monster monster = agent.Monster;
					Equipment spawnEquipment = agent.SpawnEquipment;
					MissionEquipment equipment4 = agent.Equipment;
					BodyProperties bodyPropertiesValue = agent.BodyPropertiesValue;
					int bodyPropertiesSeed = agent.BodyPropertiesSeed;
					bool isFemale = agent.IsFemale;
					Team team = agent.Team;
					int num6 = ((team != null) ? team.TeamIndex : (-1));
					Formation formation = agent.Formation;
					int num7 = ((formation != null) ? formation.Index : (-1));
					uint clothingColor = agent.ClothingColor1;
					uint clothingColor2 = agent.ClothingColor2;
					int num8 = (flag2 ? agent.MountAgent.Index : (-1));
					Agent mountAgent = agent.MountAgent;
					GameNetwork.WriteMessage(new CreateAgent(index, character, monster, spawnEquipment, equipment4, bodyPropertiesValue, bodyPropertiesSeed, isFemale, num6, num7, clothingColor, clothingColor2, num8, (mountAgent != null) ? mountAgent.SpawnEquipment : null, flag, valueOrDefault, valueOrDefault2, networkCommunicator2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = this.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			if (missionBehavior != null && agentBuildData.AgentMissionPeer != null && agentBuildData.AgentMissionPeer.IsMine && agentBuildData.AgentVisualsIndex == 0)
			{
				missionBehavior.OnMyAgentSpawned();
			}
			if (agent3 != null)
			{
				agent3.SetClothingColor1(agentBuildData.AgentClothingColor1);
				agent3.SetClothingColor2(agentBuildData.AgentClothingColor2);
				this.BuildAgent(agent3, agentBuildData);
				foreach (MissionBehavior missionBehavior2 in this.MissionBehaviors)
				{
					missionBehavior2.OnAgentBuild(agent3, null);
				}
			}
			this.BuildAgent(agent, agentBuildData);
			if (agentBuildData.AgentMissionPeer != null)
			{
				agent.MissionPeer = agentBuildData.AgentMissionPeer;
			}
			if (agentBuildData.OwningAgentMissionPeer != null)
			{
				agent.SetOwningAgentMissionPeer(agentBuildData.OwningAgentMissionPeer);
			}
			foreach (MissionBehavior missionBehavior3 in this.MissionBehaviors)
			{
				Agent agent4 = agent;
				Banner banner;
				if ((banner = agentBuildData.AgentBanner) == null)
				{
					Team agentTeam2 = agentBuildData.AgentTeam;
					banner = ((agentTeam2 != null) ? agentTeam2.Banner : null);
				}
				missionBehavior3.OnAgentBuild(agent4, banner);
			}
			agent.AgentVisuals.CheckResources(true);
			if (agent.IsAIControlled)
			{
				if (agent3 == null)
				{
					AgentFlag agentFlag = agent.GetAgentFlags() & ~AgentFlag.CanRide;
					agent.SetAgentFlags(agentFlag);
				}
				else if (agent.Formation == null)
				{
					agent.SetRidingOrder(RidingOrder.RidingOrderEnum.Mount);
				}
			}
			Mission mission = Mission.Current;
			if (mission != null && mission.IsDeploymentFinished)
			{
				MissionGameModels.Current.AgentStatCalculateModel.InitializeAgentStatsAfterDeploymentFinished(agent);
				MissionGameModels.Current.AgentStatCalculateModel.InitializeMissionEquipmentAfterDeploymentFinished(agent);
			}
			return agent;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x00059D3C File Offset: 0x00057F3C
		public void SetInitialAgentCountForSide(BattleSideEnum side, int agentCount)
		{
			if (side >= BattleSideEnum.Defender && side < BattleSideEnum.NumSides)
			{
				this._initialAgentCountPerSide[(int)side] = agentCount;
				return;
			}
			Debug.FailedAssert("Cannot set initial agent count.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "SetInitialAgentCountForSide", 3971);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00059D78 File Offset: 0x00057F78
		public void SetFormationPositioningFromDeploymentPlan(Formation formation)
		{
			IFormationDeploymentPlan formationPlan = this._deploymentPlan.GetFormationPlan(formation.Team, formation.FormationIndex, false);
			if (formationPlan.HasDimensions)
			{
				formation.SetFormOrder(FormOrder.FormOrderCustom(formationPlan.PlannedWidth), true);
			}
			formation.SetPositioning(new WorldPosition?(formationPlan.CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.None)), new Vec2?(formationPlan.GetDirection()), null);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00059DDE File Offset: 0x00057FDE
		public Agent SpawnMonster(ItemRosterElement rosterElement, ItemRosterElement harnessRosterElement, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentIndex = -1)
		{
			return this.SpawnMonster(rosterElement.EquipmentElement, harnessRosterElement.EquipmentElement, in initialPosition, in initialDirection, forcedAgentIndex);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00059DFC File Offset: 0x00057FFC
		public Agent SpawnMonster(EquipmentElement equipmentElement, EquipmentElement harnessRosterElement, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentIndex = -1)
		{
			Agent agent = this.CreateHorseAgentFromRosterElements(equipmentElement, harnessRosterElement, in initialPosition, in initialDirection, forcedAgentIndex, MountCreationKey.GetRandomMountKeyString(equipmentElement.Item, MBRandom.RandomInt()));
			Equipment equipment = new Equipment();
			equipment[EquipmentIndex.ArmorItemEndSlot] = equipmentElement;
			equipment[EquipmentIndex.HorseHarness] = harnessRosterElement;
			Equipment equipment2 = equipment;
			agent.InitializeSpawnEquipment(equipment2);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateFreeMountAgent(agent, initialPosition, initialDirection));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			this.BuildAgent(agent, null);
			return agent;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00059E7C File Offset: 0x0005807C
		public Agent SpawnTroop(IAgentOriginBase troopOrigin, bool isPlayerSide, bool hasFormation, bool spawnWithHorse, bool isReinforcement, int formationTroopCount, int formationTroopIndex, bool isAlarmed, bool wieldInitialWeapons, Vec3? initialPosition, Vec2? initialDirection, string specialActionSetSuffix = null, ItemObject bannerItem = null, FormationClass formationIndex = FormationClass.NumberOfAllFormations, bool useTroopClassForSpawn = false)
		{
			BasicCharacterObject troop = troopOrigin.Troop;
			Team agentTeam = Mission.GetAgentTeam(troopOrigin, isPlayerSide);
			AgentBuildData agentBuildData = new AgentBuildData(troop).Team(agentTeam).Banner(troopOrigin.Banner).ClothingColor1(agentTeam.Color)
				.ClothingColor2(agentTeam.Color2)
				.TroopOrigin(troopOrigin)
				.NoHorses(!spawnWithHorse)
				.CivilianEquipment(this.DoesMissionRequireCivilianEquipment)
				.SpawnsUsingOwnTroopClass(useTroopClassForSpawn);
			if (hasFormation)
			{
				Formation formation;
				if (formationIndex == FormationClass.NumberOfAllFormations)
				{
					formation = agentTeam.GetFormation(this.GetAgentTroopClass(agentTeam.Side, troop));
				}
				else
				{
					formation = agentTeam.GetFormation(formationIndex);
				}
				agentBuildData.Formation(formation);
				agentBuildData.FormationTroopSpawnCount(formationTroopCount).FormationTroopSpawnIndex(formationTroopIndex);
			}
			if (!troop.IsPlayerCharacter)
			{
				agentBuildData.IsReinforcement(isReinforcement);
			}
			if (bannerItem != null)
			{
				if (bannerItem.IsBannerItem && bannerItem.BannerComponent != null)
				{
					agentBuildData.BannerItem(bannerItem);
					ItemObject bannerBearerReplacementWeapon = MissionGameModels.Current.BattleBannerBearersModel.GetBannerBearerReplacementWeapon(troop);
					agentBuildData.BannerReplacementWeaponItem(bannerBearerReplacementWeapon);
				}
				else
				{
					Debug.FailedAssert("Passed banner item with name: " + bannerItem.Name + " is not a proper banner item", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "SpawnTroop", 4073);
					Debug.Print("Invalid banner item: " + bannerItem.Name + " is passed to a troop to be spawned", 0, Debug.DebugColor.Yellow, 17592186044416UL);
				}
			}
			if (initialPosition != null)
			{
				AgentBuildData agentBuildData2 = agentBuildData;
				Vec3 value = initialPosition.Value;
				agentBuildData2.InitialPosition(in value);
				AgentBuildData agentBuildData3 = agentBuildData;
				Vec2 value2 = initialDirection.Value;
				agentBuildData3.InitialDirection(in value2);
			}
			if (spawnWithHorse)
			{
				agentBuildData.MountKey(MountCreationKey.GetRandomMountKeyString(troop.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, troop.GetMountKeySeed()));
			}
			if (isPlayerSide && troop == Game.Current.PlayerTroop)
			{
				agentBuildData.Controller(AgentControllerType.Player);
			}
			Agent agent = this.SpawnAgent(agentBuildData, false);
			if (agent.Character.IsHero)
			{
				agent.SetAgentFlags(agent.GetAgentFlags() | AgentFlag.IsUnique);
			}
			if (agent.IsAIControlled && isAlarmed)
			{
				agent.SetWatchState(Agent.WatchState.Alarmed);
			}
			if (wieldInitialWeapons)
			{
				agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
			}
			if (!string.IsNullOrEmpty(specialActionSetSuffix))
			{
				AnimationSystemData animationSystemData = agentBuildData.AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSetWithSuffix(agentBuildData.AgentMonster, agentBuildData.AgentIsFemale, specialActionSetSuffix), agent.Character.GetStepSize(), false);
				agent.SetActionSet(ref animationSystemData);
			}
			return agent;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0005A0C4 File Offset: 0x000582C4
		public Agent ReplaceBotWithPlayer(Agent botAgent, MissionPeer missionPeer)
		{
			if (!GameNetwork.IsClientOrReplay && botAgent != null)
			{
				if (GameNetwork.IsServer)
				{
					NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
					if (!networkPeer.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						NetworkCommunicator networkCommunicator = networkPeer;
						int index = botAgent.Index;
						float health = botAgent.Health;
						Agent mountAgent = botAgent.MountAgent;
						GameNetwork.WriteMessage(new ReplaceBotWithPlayer(networkCommunicator, index, health, (mountAgent != null) ? mountAgent.Health : (-1f)));
						GameNetwork.EndModuleEventAsServer();
					}
				}
				if (botAgent.Formation != null)
				{
					botAgent.Formation.PlayerOwner = botAgent;
				}
				botAgent.SetOwningAgentMissionPeer(null);
				botAgent.MissionPeer = missionPeer;
				botAgent.Formation = missionPeer.ControlledFormation;
				AgentFlag agentFlags = botAgent.GetAgentFlags();
				if (!agentFlags.HasAnyFlag(AgentFlag.CanRide))
				{
					botAgent.SetAgentFlags(agentFlags | AgentFlag.CanRide);
				}
				int botsUnderControlAlive = missionPeer.BotsUnderControlAlive;
				missionPeer.BotsUnderControlAlive = botsUnderControlAlive - 1;
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new BotsControlledChange(missionPeer.GetNetworkPeer(), missionPeer.BotsUnderControlAlive, missionPeer.BotsUnderControlTotal));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				if (botAgent.Formation != null)
				{
					missionPeer.Team.AssignPlayerAsSergeantOfFormation(missionPeer, missionPeer.ControlledFormation.FormationIndex);
				}
				return botAgent;
			}
			return null;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0005A1D8 File Offset: 0x000583D8
		private Agent CreateHorseAgentFromRosterElements(EquipmentElement mount, EquipmentElement mountHarness, in Vec3 initialPosition, in Vec2 initialDirection, int forcedAgentMountIndex, string horseCreationKey)
		{
			HorseComponent horseComponent = mount.Item.HorseComponent;
			Agent agent = this.CreateAgent(horseComponent.Monster, false, 0, Agent.CreationType.FromHorseObj, 1f, forcedAgentMountIndex, (int)mount.Weight, null);
			agent.SetInitialFrame(in initialPosition, in initialDirection, false);
			agent.BaseHealthLimit = (float)mount.GetModifiedMountHitPoints();
			agent.HealthLimit = agent.BaseHealthLimit;
			agent.Health = agent.HealthLimit;
			agent.SetMountInitialValues(mount.GetModifiedItemName(), horseCreationKey);
			return agent;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x0005A250 File Offset: 0x00058450
		public void OnAgentInteraction(Agent requesterAgent, Agent targetAgent, sbyte agentBoneIndex)
		{
			if (requesterAgent == Agent.Main && targetAgent.IsMount)
			{
				Agent.Main.Mount(targetAgent);
				return;
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentInteraction(requesterAgent, targetAgent, agentBoneIndex);
			}
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x0005A2C0 File Offset: 0x000584C0
		[UsedImplicitly]
		[MBCallback(null, false)]
		public void EndMission()
		{
			Debug.Print("I called EndMission", 0, Debug.DebugColor.White, 17179869184UL);
			this._missionEndTime = -1f;
			this.NextCheckTimeEndMission = -1f;
			this.MissionEnded = true;
			this.CurrentState = Mission.State.EndingNextFrame;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x0005A2FC File Offset: 0x000584FC
		private void EndMissionInternal()
		{
			MBDebug.Print("I called EndMissionInternal", 0, Debug.DebugColor.White, 17179869184UL);
			this._deploymentPlan.ClearAll();
			IMissionListener[] array = this._listeners.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnEndMission();
			}
			this.StopSoundEvents();
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnEndMissionInternal();
			}
			foreach (Agent agent in this.Agents)
			{
				agent.OnRemove();
			}
			foreach (Agent agent2 in this.AllAgents)
			{
				agent2.OnDelete();
				agent2.Clear();
			}
			this.Teams.Clear();
			this.FocusableObjectInformationProvider.OnFinalize();
			foreach (MissionObject missionObject in this.MissionObjects)
			{
				missionObject.OnEndMission();
			}
			this.CurrentState = Mission.State.Over;
			this.FreeResources();
			this.FinalizeMission();
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0005A484 File Offset: 0x00058684
		private void StopSoundEvents()
		{
			if (this._ambientSoundEvent != null)
			{
				this._ambientSoundEvent.Stop();
			}
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0005A49C File Offset: 0x0005869C
		public void AddMissionBehavior(MissionBehavior missionBehavior)
		{
			this.MissionBehaviors.Add(missionBehavior);
			missionBehavior.Mission = this;
			MissionBehaviorType behaviorType = missionBehavior.BehaviorType;
			if (behaviorType != MissionBehaviorType.Logic)
			{
				if (behaviorType == MissionBehaviorType.Other)
				{
					this._otherMissionBehaviors.Add(missionBehavior);
				}
			}
			else
			{
				this.MissionLogics.Add(missionBehavior as MissionLogic);
			}
			missionBehavior.OnCreated();
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x0005A4F4 File Offset: 0x000586F4
		public T GetMissionBehavior<T>() where T : class, IMissionBehavior
		{
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				T t;
				if ((t = this.MissionBehaviors[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x0005A544 File Offset: 0x00058744
		public void RemoveMissionBehavior(MissionBehavior missionBehavior)
		{
			missionBehavior.OnRemoveBehavior();
			MissionBehaviorType behaviorType = missionBehavior.BehaviorType;
			if (behaviorType != MissionBehaviorType.Logic)
			{
				if (behaviorType != MissionBehaviorType.Other)
				{
					Debug.FailedAssert("Invalid behavior type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "RemoveMissionBehavior", 4369);
				}
				else
				{
					this._otherMissionBehaviors.Remove(missionBehavior);
				}
			}
			else
			{
				this.MissionLogics.Remove(missionBehavior as MissionLogic);
			}
			this.MissionBehaviors.Remove(missionBehavior);
			missionBehavior.Mission = null;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x0005A5B8 File Offset: 0x000587B8
		public void JoinEnemyTeam()
		{
			if (this.PlayerTeam == this.DefenderTeam)
			{
				Agent leader = this.AttackerTeam.Leader;
				if (leader != null)
				{
					if (this.MainAgent != null && this.MainAgent.IsActive())
					{
						this.MainAgent.Controller = AgentControllerType.AI;
					}
					leader.Controller = AgentControllerType.Player;
					this.PlayerTeam = this.AttackerTeam;
					return;
				}
			}
			else if (this.PlayerTeam == this.AttackerTeam)
			{
				Agent leader2 = this.DefenderTeam.Leader;
				if (leader2 != null)
				{
					if (this.MainAgent != null && this.MainAgent.IsActive())
					{
						this.MainAgent.Controller = AgentControllerType.AI;
					}
					leader2.Controller = AgentControllerType.Player;
					this.PlayerTeam = this.DefenderTeam;
					return;
				}
			}
			else
			{
				Debug.FailedAssert("Player is neither attacker nor defender.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "JoinEnemyTeam", 4413);
			}
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0005A688 File Offset: 0x00058888
		public void OnEndMissionResult()
		{
			MissionLogic[] array = this.MissionLogics.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnBattleEnded();
			}
			this.RetreatMission();
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x0005A6C0 File Offset: 0x000588C0
		public bool IsAgentInteractionAllowed()
		{
			if (this.IsAgentInteractionAllowed_AdditionalCondition != null)
			{
				Delegate[] invocationList = this.IsAgentInteractionAllowed_AdditionalCondition.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					object obj;
					if ((obj = invocationList[i].DynamicInvoke(Array.Empty<object>())) is bool && !(bool)obj)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x0005A714 File Offset: 0x00058914
		public bool IsOrderGesturesEnabled()
		{
			if (this.AreOrderGesturesEnabled_AdditionalCondition != null)
			{
				Delegate[] invocationList = this.AreOrderGesturesEnabled_AdditionalCondition.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					object obj;
					if ((obj = invocationList[i].DynamicInvoke(Array.Empty<object>())) is bool && !(bool)obj)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0005A768 File Offset: 0x00058968
		public List<EquipmentElement> GetExtraEquipmentElementsForCharacter(BasicCharacterObject character, bool getAllEquipments = false)
		{
			List<EquipmentElement> list = new List<EquipmentElement>();
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				List<EquipmentElement> extraEquipmentElementsForCharacter = missionLogic.GetExtraEquipmentElementsForCharacter(character, getAllEquipments);
				if (extraEquipmentElementsForCharacter != null)
				{
					list.AddRange(extraEquipmentElementsForCharacter);
				}
			}
			return list;
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x0005A7CC File Offset: 0x000589CC
		private bool CheckMissionEnded()
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				MissionResult missionResult = null;
				if (missionLogic.MissionEnded(ref missionResult))
				{
					Debug.Print("CheckMissionEnded::ended", 0, Debug.DebugColor.White, 17592186044416UL);
					this.MissionResult = missionResult;
					this.MissionEnded = true;
					this.MissionResultReady(missionResult);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x0005A854 File Offset: 0x00058A54
		private void MissionResultReady(MissionResult missionResult)
		{
			foreach (MissionLogic missionLogic in this.MissionLogics)
			{
				missionLogic.OnMissionResultReady(missionResult);
			}
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0005A8A8 File Offset: 0x00058AA8
		private void CheckMissionEnd(float currentTime)
		{
			if (!GameNetwork.IsClient && currentTime > this.NextCheckTimeEndMission)
			{
				if (this.CurrentState == Mission.State.Continuing)
				{
					if (this.MissionEnded)
					{
						return;
					}
					this.NextCheckTimeEndMission += 0.1f;
					this.CheckMissionEnded();
					if (!this.MissionEnded)
					{
						return;
					}
					this._missionEndTime = currentTime + this.MissionCloseTimeAfterFinish;
					this.NextCheckTimeEndMission += 5f;
					using (List<MissionLogic>.Enumerator enumerator = this.MissionLogics.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MissionLogic missionLogic = enumerator.Current;
							missionLogic.ShowBattleResults();
						}
						return;
					}
				}
				if (currentTime > this._missionEndTime)
				{
					this.EndMissionInternal();
					return;
				}
				this.NextCheckTimeEndMission += 5f;
				return;
			}
			else if (this.CurrentState != Mission.State.Continuing && currentTime > this.NextCheckTimeEndMission)
			{
				this.EndMissionInternal();
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x0005A9A4 File Offset: 0x00058BA4
		public bool IsPlayerCloseToAnEnemy(float distance = 5f)
		{
			if (this.MainAgent == null)
			{
				return false;
			}
			Vec3 position = this.MainAgent.Position;
			float num = distance * distance;
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, position.AsVec2, distance, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent != this.MainAgent && lastFoundAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack) && lastFoundAgent.Position.DistanceSquared(position) <= num && (!lastFoundAgent.IsAIControlled || lastFoundAgent.IsAlarmed()) && lastFoundAgent.IsEnemyOf(this.MainAgent) && !lastFoundAgent.IsRetreating())
				{
					return true;
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			return false;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x0005AA4C File Offset: 0x00058C4C
		public Vec3 GetRandomPositionAroundPoint(Vec3 center, float minDistance, float maxDistance, bool nearFirst = false)
		{
			Vec3 vec = new Vec3(-1f, 0f, 0f, -1f);
			vec.RotateAboutZ(6.2831855f * MBRandom.RandomFloat);
			float num = maxDistance - minDistance;
			if (nearFirst)
			{
				for (int i = 4; i > 0; i--)
				{
					int num2 = 0;
					while ((float)num2 <= 10f)
					{
						vec.RotateAboutZ(1.2566371f);
						Vec3 vec2 = center + vec * (minDistance + num / (float)i);
						if (this.Scene.GetNavigationMeshForPosition(in vec2) != UIntPtr.Zero)
						{
							return vec2;
						}
						num2++;
					}
				}
			}
			else
			{
				for (int j = 1; j < 5; j++)
				{
					int num3 = 0;
					while ((float)num3 <= 10f)
					{
						vec.RotateAboutZ(1.2566371f);
						Vec3 vec3 = center + vec * (minDistance + num / (float)j);
						if (this.Scene.GetNavigationMeshForPosition(in vec3) != UIntPtr.Zero)
						{
							return vec3;
						}
						num3++;
					}
				}
			}
			return center;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0005AB54 File Offset: 0x00058D54
		public WorldPosition FindBestDefendingPosition(WorldPosition enemyPosition, WorldPosition defendedPosition)
		{
			return this.GetBestSlopeAngleHeightPosForDefending(enemyPosition, defendedPosition, 10, 0.5f, 4f, 0.5f, 0.70710677f, 0.1f, 1f, 0.7f, 0.5f, 1.2f, 20f, 0.6f);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x0005ABA2 File Offset: 0x00058DA2
		public WorldPosition FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref WorldPosition center, float halfSize, ref WorldPosition referencePosition)
		{
			return this.GetBestSlopeTowardsDirection(ref center, halfSize, ref referencePosition);
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x0005ABB0 File Offset: 0x00058DB0
		public Mission.Missile AddCustomMissile(Agent shooterAgent, MissionWeapon missileWeapon, Vec3 position, Vec3 direction, Mat3 orientation, float baseSpeed, float speed, bool addRigidBody, MissionObject missionObjectToIgnore, int forcedMissileIndex = -1)
		{
			WeaponData weaponData = missileWeapon.GetWeaponData(true);
			GameEntity gameEntity;
			int num;
			if (missileWeapon.WeaponsCount == 1)
			{
				WeaponStatsData weaponStatsDataForUsage = missileWeapon.GetWeaponStatsDataForUsage(0);
				num = this.AddMissileSingleUsageAux(forcedMissileIndex, false, shooterAgent, in weaponData, in weaponStatsDataForUsage, 0f, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, (missionObjectToIgnore != null) ? missionObjectToIgnore.GameEntity : WeakGameEntity.Invalid, false, out gameEntity);
			}
			else
			{
				WeaponStatsData[] weaponStatsData = missileWeapon.GetWeaponStatsData();
				num = this.AddMissileAux(forcedMissileIndex, false, shooterAgent, in weaponData, weaponStatsData, 0f, ref position, ref direction, ref orientation, baseSpeed, speed, addRigidBody, (missionObjectToIgnore != null) ? missionObjectToIgnore.GameEntity : WeakGameEntity.Invalid, false, out gameEntity);
			}
			weaponData.DeinitializeManagedPointers();
			Mission.Missile missile = new Mission.Missile(this, num, gameEntity, shooterAgent, missileWeapon, missionObjectToIgnore);
			this._missilesList.Add(missile);
			this._missilesDictionary.Add(num, missile);
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateMissile(num, shooterAgent.Index, EquipmentIndex.None, missileWeapon, position, direction, speed, orientation, addRigidBody, missionObjectToIgnore.Id, false));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			return missile;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x0005ACB8 File Offset: 0x00058EB8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, bool isPrimaryWeaponShot, int forcedMissileIndex)
		{
			bool flag = GameNetwork.IsClient && forcedMissileIndex == -1;
			float num = 0f;
			MissionWeapon missionWeapon;
			if (shooterAgent.Equipment[weaponIndex].CurrentUsageItem != null && shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsRangedWeapon && shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsConsumable)
			{
				missionWeapon = shooterAgent.Equipment[weaponIndex];
			}
			else
			{
				missionWeapon = shooterAgent.Equipment[weaponIndex].AmmoWeapon;
				if (shooterAgent.Equipment[weaponIndex].CurrentUsageItem != null)
				{
					num = (float)shooterAgent.Equipment[weaponIndex].GetModifiedThrustDamageForCurrentUsage();
				}
			}
			if (!missionWeapon.IsEmpty)
			{
				missionWeapon.Amount = 1;
				WeaponData weaponData = missionWeapon.GetWeaponData(true);
				Vec3 vec = velocity;
				float num2 = vec.Normalize();
				float num3 = (float)shooterAgent.Equipment[shooterAgent.GetPrimaryWieldedItemIndex()].GetModifiedMissileSpeedForCurrentUsage();
				GameEntity gameEntity;
				int num4;
				if (missionWeapon.WeaponsCount == 1)
				{
					WeaponStatsData weaponStatsDataForUsage = missionWeapon.GetWeaponStatsDataForUsage(0);
					num4 = this.AddMissileSingleUsageAux(forcedMissileIndex, flag, shooterAgent, in weaponData, in weaponStatsDataForUsage, num, ref position, ref vec, ref orientation, num3, num2, hasRigidBody, WeakGameEntity.Invalid, isPrimaryWeaponShot, out gameEntity);
				}
				else
				{
					WeaponStatsData[] weaponStatsData = missionWeapon.GetWeaponStatsData();
					num4 = this.AddMissileAux(forcedMissileIndex, flag, shooterAgent, in weaponData, weaponStatsData, num, ref position, ref vec, ref orientation, num3, num2, hasRigidBody, WeakGameEntity.Invalid, isPrimaryWeaponShot, out gameEntity);
				}
				weaponData.DeinitializeManagedPointers();
				if (!flag)
				{
					Mission.Missile missile = new Mission.Missile(this, num4, gameEntity, shooterAgent, missionWeapon, null);
					this._missilesList.Add(missile);
					this._missilesDictionary.Add(num4, missile);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new CreateMissile(num4, shooterAgent.Index, weaponIndex, MissionWeapon.Invalid, position, vec, num2, orientation, hasRigidBody, MissionObjectId.Invalid, isPrimaryWeaponShot));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
				}
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);
				}
				if (shooterAgent != null)
				{
					shooterAgent.UpdateLastRangedAttackTimeDueToAnAttack(MBCommon.GetTotalMissionTime());
				}
			}
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x0005AEF8 File Offset: 0x000590F8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal AgentState GetAgentState(Agent affectorAgent, Agent agent, DamageTypes damageType, WeaponFlags weaponFlags)
		{
			float num;
			float agentStateProbability = MissionGameModels.Current.AgentDecideKilledOrUnconsciousModel.GetAgentStateProbability(affectorAgent, agent, damageType, weaponFlags, out num);
			AgentState agentState = AgentState.None;
			bool flag = false;
			using (List<MissionBehavior>.Enumerator enumerator = this.MissionBehaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IAgentStateDecider agentStateDecider;
					if ((agentStateDecider = enumerator.Current as IAgentStateDecider) != null)
					{
						agentState = agentStateDecider.GetAgentState(agent, agentStateProbability, out flag);
						break;
					}
				}
			}
			if (agentState == AgentState.None)
			{
				float randomFloat = MBRandom.RandomFloat;
				if (randomFloat < agentStateProbability)
				{
					agentState = AgentState.Killed;
					flag = true;
				}
				else
				{
					agentState = AgentState.Unconscious;
					if (randomFloat > 1f - num)
					{
						flag = true;
					}
				}
			}
			if (flag && affectorAgent != null && affectorAgent.Team != null && agent.Team != null && affectorAgent.Team == agent.Team)
			{
				flag = false;
			}
			for (int i = 0; i < this.MissionBehaviors.Count; i++)
			{
				this.MissionBehaviors[i].OnGetAgentState(agent, flag);
			}
			return agentState;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0005AFF0 File Offset: 0x000591F0
		public void OnAgentMount(Agent agent)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentMount(agent);
			}
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0005B044 File Offset: 0x00059244
		public void OnAgentDismount(Agent agent)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentDismount(agent);
			}
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0005B098 File Offset: 0x00059298
		public void OnObjectUsed(Agent userAgent, UsableMissionObject usableGameObject)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectUsed(userAgent, usableGameObject);
			}
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0005B0EC File Offset: 0x000592EC
		public void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usableGameObject)
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnObjectStoppedBeingUsed(userAgent, usableGameObject);
			}
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x0005B140 File Offset: 0x00059340
		public void InitializeStartingBehaviors(MissionLogic[] logicBehaviors, MissionBehavior[] otherBehaviors, MissionNetwork[] networkBehaviors)
		{
			foreach (MissionLogic missionLogic in logicBehaviors)
			{
				this.AddMissionBehavior(missionLogic);
			}
			foreach (MissionNetwork missionNetwork in networkBehaviors)
			{
				this.AddMissionBehavior(missionNetwork);
			}
			foreach (MissionBehavior missionBehavior in otherBehaviors)
			{
				this.AddMissionBehavior(missionBehavior);
			}
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0005B1A5 File Offset: 0x000593A5
		public Agent GetClosestEnemyAgent(Team team, Vec3 position, float radius)
		{
			return this.GetClosestEnemyAgent(team.MBTeam, position, radius);
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0005B1B5 File Offset: 0x000593B5
		public Agent GetClosestAllyAgent(Team team, Vec3 position, float radius)
		{
			return this.GetClosestAllyAgent(team.MBTeam, position, radius);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x0005B1C5 File Offset: 0x000593C5
		public int GetNearbyEnemyAgentCount(Team team, Vec2 position, float radius)
		{
			return this.GetNearbyEnemyAgentCount(team.MBTeam, position, radius);
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0005B1D8 File Offset: 0x000593D8
		public bool HasAnyAgentsOfSideInRange(Vec3 origin, float radius, BattleSideEnum side)
		{
			Team team = ((side == BattleSideEnum.Attacker) ? this.AttackerTeam : this.DefenderTeam);
			return MBAPI.IMBMission.HasAnyAgentsOfTeamAround(this.Pointer, origin, radius, team.MBTeam.Index);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0005B215 File Offset: 0x00059415
		public void AddSoundAlarmFactorToAgents(Agent alarmCreatorAgent, in Vec3 soundPosition, float soundLevelSquareRoot)
		{
			Mission.OnAddSoundAlarmFactorToAgentsDelegate onAddSoundAlarmFactorToAgents = this.OnAddSoundAlarmFactorToAgents;
			if (onAddSoundAlarmFactorToAgents == null)
			{
				return;
			}
			onAddSoundAlarmFactorToAgents(alarmCreatorAgent, in soundPosition, soundLevelSquareRoot);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0005B22C File Offset: 0x0005942C
		private void HandleSpawnedItems()
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				int num = 0;
				for (int i = this._spawnedItemEntitiesCreatedAtRuntime.Count - 1; i >= 0; i--)
				{
					SpawnedItemEntity spawnedItemEntity = this._spawnedItemEntitiesCreatedAtRuntime[i];
					if (!spawnedItemEntity.IsRemoved)
					{
						if (!spawnedItemEntity.IsDeactivated && !spawnedItemEntity.HasUser && spawnedItemEntity.HasLifeTime && !spawnedItemEntity.HasAIMovingTo && (num > 500 || spawnedItemEntity.IsReadyToBeDeleted()))
						{
							spawnedItemEntity.GameEntity.Remove(80);
						}
						else
						{
							num++;
						}
					}
					if (spawnedItemEntity.IsRemoved)
					{
						this._spawnedItemEntitiesCreatedAtRuntime.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0005B2CC File Offset: 0x000594CC
		public bool OnMissionObjectRemoved(MissionObject missionObject, int removeReason)
		{
			if (!GameNetwork.IsClientOrReplay && missionObject.CreatedAtRuntime)
			{
				this.ReturnRuntimeMissionObjectId(missionObject.Id.Id);
				if (GameNetwork.IsServerOrRecorder)
				{
					this.RemoveDynamicallySpawnedMissionObjectInfo(missionObject.Id);
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new RemoveMissionObject(missionObject.Id));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
			}
			this._activeMissionObjects.Remove(missionObject);
			return this._missionObjects.Remove(missionObject);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0005B344 File Offset: 0x00059544
		public bool AgentLookingAtAgent(Agent agent1, Agent agent2)
		{
			Vec3 vec = agent2.Position - agent1.Position;
			float num = vec.Normalize();
			float num2 = Vec3.DotProduct(vec, agent1.LookDirection);
			return num2 < 1f && num2 > 0.86f && num < 4f;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0005B397 File Offset: 0x00059597
		public Agent FindAgentWithIndex(int agentId)
		{
			return this.FindAgentWithIndexAux(agentId);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0005B3A0 File Offset: 0x000595A0
		public static Agent.UnderAttackType GetUnderAttackTypeOfAgents(IEnumerable<Agent> agents, float timeLimit = 3f)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			timeLimit += MBCommon.GetTotalMissionTime();
			foreach (Agent agent in agents)
			{
				num = MathF.Max(num, agent.LastMeleeHitTime);
				num2 = MathF.Max(num2, agent.LastRangedHitTime);
				if (num2 >= 0f && num2 < timeLimit)
				{
					return Agent.UnderAttackType.UnderRangedAttack;
				}
				if (num >= 0f && num < timeLimit)
				{
					return Agent.UnderAttackType.UnderMeleeAttack;
				}
			}
			return Agent.UnderAttackType.NotUnderAttack;
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0005B438 File Offset: 0x00059638
		public static Team GetAgentTeam(IAgentOriginBase troopOrigin, bool isPlayerSide)
		{
			if (Mission.Current == null)
			{
				Debug.FailedAssert("Mission current is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetAgentTeam", 5104);
				return null;
			}
			Team team;
			if (isPlayerSide)
			{
				if (Mission.Current.PlayerAllyTeam == null || troopOrigin.IsUnderPlayersCommand || troopOrigin.IsInSameArmyAsPlayer)
				{
					team = Mission.Current.PlayerTeam;
				}
				else
				{
					team = Mission.Current.PlayerAllyTeam;
				}
			}
			else
			{
				team = Mission.Current.PlayerEnemyTeam;
			}
			return team;
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0005B4AC File Offset: 0x000596AC
		public static Team GetTeam(TeamSideEnum teamSide)
		{
			if (Mission.Current == null)
			{
				Debug.FailedAssert("Mission current is null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "GetTeam", 5139);
				return null;
			}
			switch (teamSide)
			{
			case TeamSideEnum.PlayerTeam:
				return Mission.Current.PlayerTeam;
			case TeamSideEnum.PlayerAllyTeam:
				return Mission.Current.PlayerAllyTeam;
			case TeamSideEnum.EnemyTeam:
				return Mission.Current.PlayerEnemyTeam;
			default:
				return null;
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0005B514 File Offset: 0x00059714
		public static IEnumerable<Team> GetTeamsOfSide(BattleSideEnum side)
		{
			return Mission.Current.Teams.Where<Team>((Team t) => t.Side == side);
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0005B54C File Offset: 0x0005974C
		public static float ComputeSpawnPathDeploymentOffset(int troopCount, Path path)
		{
			float totalLength = path.GetTotalLength();
			float num = 200f;
			if (troopCount > 20)
			{
				int num2 = MathF.Max(BannerlordConfig.MaxBattleSize - 20, 1);
				float num3 = (float)Math.Min(troopCount - 20, num2) / (float)num2;
				num += 400f * MathF.Pow(num3, 0.6f);
			}
			num = MathF.Min(totalLength, num);
			return -num / 2f + 1f;
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0005B5B4 File Offset: 0x000597B4
		public void OnRenderingStarted()
		{
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnRenderingStarted();
			}
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0005B604 File Offset: 0x00059804
		public unsafe Agent.MovementBehaviorType GetMovementTypeOfAgents(IEnumerable<Agent> agents)
		{
			float totalMissionTime = MBCommon.GetTotalMissionTime();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Agent agent in agents)
			{
				num++;
				if (agent.IsAIControlled)
				{
					if (!agent.IsRetreating())
					{
						if (agent.Formation == null)
						{
							goto IL_0060;
						}
						MovementOrder movementOrder = *agent.Formation.GetReadonlyMovementOrderReference();
						if (movementOrder.OrderType != OrderType.Retreat)
						{
							goto IL_0060;
						}
					}
					num2++;
				}
				IL_0060:
				if (totalMissionTime - agent.LastMeleeAttackTime < 3f)
				{
					num3++;
				}
			}
			if ((float)num2 * 1f / (float)num > 0.3f)
			{
				return Agent.MovementBehaviorType.Flee;
			}
			if (num3 > 0)
			{
				return Agent.MovementBehaviorType.Engaged;
			}
			return Agent.MovementBehaviorType.Idle;
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0005B6C8 File Offset: 0x000598C8
		public void ShowInMissionLoadingScreen(int durationInSecond, Action onLoadingEndedAction)
		{
			this._inMissionLoadingScreenTimer = new Timer(this.CurrentTime, (float)durationInSecond, true);
			this._onLoadingEndedAction = onLoadingEndedAction;
			LoadingWindow.EnableGlobalLoadingWindow();
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0005B6EC File Offset: 0x000598EC
		public bool CanAgentRout(Agent agent)
		{
			return (agent.IsRunningAway || (agent.CommonAIComponent != null && agent.CommonAIComponent.IsRetreating) || (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWander) && agent.IsWandering())) && agent.RiderAgent == null && (this.CanAgentRout_AdditionalCondition == null || this.CanAgentRout_AdditionalCondition(agent));
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0005B750 File Offset: 0x00059950
		internal bool CanGiveDamageToAgentShield(Agent attacker, WeaponComponentData attackerWeapon, Agent defender)
		{
			return MissionGameModels.Current.AgentApplyDamageModel.CanWeaponIgnoreFriendlyFireChecks(attackerWeapon) || !this.CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(attacker, defender);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0005B774 File Offset: 0x00059974
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void MeleeHitCallback(ref AttackCollisionData collisionData, Agent attacker, Agent victim, GameEntity realHitEntity, ref float inOutMomentumRemaining, ref MeleeCollisionReaction colReaction, CrushThroughState crushThroughState, Vec3 blowDir, Vec3 swingDir, ref HitParticleResultData hitParticleResultData, bool crushedThroughWithoutAgentCollision)
		{
			hitParticleResultData.Reset();
			bool flag = collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked;
			if (collisionData.IsAlternativeAttack && !flag && victim != null && victim.IsHuman && collisionData.CollisionBoneIndex != -1 && (collisionData.VictimHitBodyPart == BoneBodyPartType.ArmLeft || collisionData.VictimHitBodyPart == BoneBodyPartType.ArmRight) && victim.IsHuman)
			{
				colReaction = MeleeCollisionReaction.ContinueChecking;
			}
			if (colReaction != MeleeCollisionReaction.ContinueChecking)
			{
				bool flag2 = this.CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(attacker, victim);
				bool flag3 = victim != null && victim.CurrentMortalityState == Agent.MortalityState.Invulnerable;
				bool flag4 = victim == null && realHitEntity == null;
				bool flag5;
				if (flag2)
				{
					collisionData.AttackerStunPeriod = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunPeriodAttackerFriendlyFire);
					flag5 = true;
				}
				else
				{
					flag5 = flag3 || flag4 || (flag && !collisionData.AttackBlockedWithShield);
				}
				int affectorWeaponSlotOrMissileIndex = collisionData.AffectorWeaponSlotOrMissileIndex;
				MissionWeapon missionWeapon = ((affectorWeaponSlotOrMissileIndex >= 0) ? attacker.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
				if (crushThroughState == CrushThroughState.CrushedThisFrame && !collisionData.IsAlternativeAttack)
				{
					Blow blow = default(Blow);
					MissionCombatMechanicsHelper.UpdateMomentumRemaining(ref inOutMomentumRemaining, in blow, in collisionData, attacker, victim, in missionWeapon, true);
				}
				WeaponComponentData weaponComponentData = null;
				CombatLogData combatLogData = default(CombatLogData);
				if (!flag5)
				{
					this.GetAttackCollisionResults(attacker, victim, (realHitEntity != null) ? realHitEntity.WeakEntity : WeakGameEntity.Invalid, inOutMomentumRemaining, in missionWeapon, crushThroughState > CrushThroughState.None, flag5, crushedThroughWithoutAgentCollision, ref collisionData, out weaponComponentData, out combatLogData);
					if (!collisionData.IsAlternativeAttack && attacker.IsDoingPassiveAttack && !GameNetwork.IsSessionActive && ManagedOptions.GetConfig(ManagedOptions.ManagedOptionsType.ReportDamage) > 0f)
					{
						if (attacker.HasMount)
						{
							if (attacker.IsMainAgent)
							{
								InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_delivered_couched_lance_damage", null).ToString(), Color.ConvertStringToColor("#AE4AD9FF")));
							}
							else if (victim != null && victim.IsMainAgent)
							{
								InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_received_couched_lance_damage", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
							}
						}
						else if (attacker.IsMainAgent)
						{
							InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_delivered_braced_polearm_damage", null).ToString(), Color.ConvertStringToColor("#AE4AD9FF")));
						}
						else if (victim != null && victim.IsMainAgent)
						{
							InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_received_braced_polearm_damage", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
						}
					}
					if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
					}
				}
				else
				{
					collisionData.InflictedDamage = 0;
					collisionData.BaseMagnitude = 0f;
					collisionData.AbsorbedByArmor = 0;
					collisionData.SelfInflictedDamage = 0;
				}
				if (!crushedThroughWithoutAgentCollision)
				{
					Blow blow2 = this.CreateMeleeBlow(attacker, victim, in collisionData, in missionWeapon, crushThroughState, blowDir, swingDir, flag5);
					if (!flag && ((victim != null && victim.IsActive()) || realHitEntity != null))
					{
						this.RegisterBlow(attacker, victim, (realHitEntity != null) ? realHitEntity.WeakEntity : WeakGameEntity.Invalid, blow2, ref collisionData, in missionWeapon, ref combatLogData);
					}
					MissionCombatMechanicsHelper.UpdateMomentumRemaining(ref inOutMomentumRemaining, in blow2, in collisionData, attacker, victim, in missionWeapon, false);
					bool flag6 = victim != null && victim.Health <= 0f;
					bool flag7 = (blow2.BlowFlag & BlowFlags.ShrugOff) > BlowFlags.None;
					this.DecideAgentHitParticles(attacker, victim, in blow2, in collisionData, ref hitParticleResultData);
					MissionGameModels.Current.AgentApplyDamageModel.DecideWeaponCollisionReaction(in blow2, in collisionData, attacker, victim, in missionWeapon, flag6, flag7, inOutMomentumRemaining, out colReaction);
				}
				else
				{
					colReaction = MeleeCollisionReaction.ContinueChecking;
				}
				foreach (MissionBehavior missionBehavior in Mission.Current.MissionBehaviors)
				{
					missionBehavior.OnMeleeHit(attacker, victim, flag5, collisionData);
				}
			}
			if (collisionData.IsShieldBroken)
			{
				Vec3 vec = collisionData.CollisionGlobalPosition;
				this.AddSoundAlarmFactorToAgents(attacker, in vec, 15f);
				return;
			}
			if (!collisionData.IsMissile && (collisionData.CollisionResult == CombatCollisionResult.HitWorld || collisionData.CollisionResult == CombatCollisionResult.Blocked || collisionData.CollisionResult == CombatCollisionResult.Parried || collisionData.CollisionResult == CombatCollisionResult.ChamberBlocked))
			{
				Vec3 vec = collisionData.CollisionGlobalPosition;
				this.AddSoundAlarmFactorToAgents(attacker, in vec, 8f);
			}
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x0005BB9C File Offset: 0x00059D9C
		private void DecideAgentHitParticles(Agent attacker, Agent victim, in Blow blow, in AttackCollisionData collisionData, ref HitParticleResultData hprd)
		{
			if (victim != null && (blow.InflictedDamage > 0 || victim.Health <= 0f))
			{
				BlowWeaponRecord weaponRecord = blow.WeaponRecord;
				bool flag;
				if (weaponRecord.HasWeapon() && !blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.NoBlood))
				{
					AttackCollisionData attackCollisionData = collisionData;
					flag = attackCollisionData.IsAlternativeAttack;
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					MissionGameModels.Current.DamageParticleModel.GetMeleeAttackSweatParticles(attacker, victim, in blow, in collisionData, out hprd);
					return;
				}
				MissionGameModels.Current.DamageParticleModel.GetMeleeAttackBloodParticles(attacker, victim, in blow, in collisionData, out hprd);
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0005BC2C File Offset: 0x00059E2C
		private void RegisterBlow(Agent attacker, Agent victim, WeakGameEntity realHitEntity, Blow b, ref AttackCollisionData collisionData, in MissionWeapon attackerWeapon, ref CombatLogData combatLogData)
		{
			b.VictimBodyPart = collisionData.VictimHitBodyPart;
			if (!collisionData.AttackBlockedWithShield)
			{
				if (collisionData.IsColliderAgent)
				{
					if (b.SelfInflictedDamage > 0 && attacker != null && attacker.IsActive() && attacker.IsFriendOf(victim))
					{
						Blow blow;
						AttackCollisionData attackCollisionData;
						attacker.CreateBlowFromBlowAsReflection(in b, in collisionData, out blow, out attackCollisionData);
						if (victim.IsMount && attacker.MountAgent != null)
						{
							attacker.MountAgent.RegisterBlow(blow, in attackCollisionData);
						}
						else
						{
							attacker.RegisterBlow(blow, in attackCollisionData);
						}
					}
					if (b.InflictedDamage > 0)
					{
						combatLogData.IsFatalDamage = victim != null && victim.Health - (float)b.InflictedDamage < 1f;
						combatLogData.InflictedDamage = b.InflictedDamage - combatLogData.ModifiedDamage;
						this.PrintAttackCollisionResults(attacker, victim, null, ref collisionData, ref combatLogData);
					}
					victim.RegisterBlow(b, in collisionData);
				}
				else if (collisionData.EntityExists)
				{
					MissionWeapon missionWeapon = (b.IsMissile ? this._missilesDictionary[b.WeaponRecord.AffectorWeaponSlotOrMissileIndex].Weapon : ((attacker != null && b.WeaponRecord.HasWeapon()) ? attacker.Equipment[b.WeaponRecord.AffectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid));
					this.OnEntityHit(realHitEntity, attacker, collisionData, b.InflictedDamage, (DamageTypes)collisionData.DamageType, b.GlobalPosition, b.SwingDirection, in missionWeapon, b.WeaponRecord.AffectorWeaponSlotOrMissileIndex, ref combatLogData);
					if (attacker != null && b.SelfInflictedDamage > 0 && attacker.IsActive())
					{
						Blow blow2;
						AttackCollisionData attackCollisionData2;
						attacker.CreateBlowFromBlowAsReflection(in b, in collisionData, out blow2, out attackCollisionData2);
						attacker.RegisterBlow(blow2, in attackCollisionData2);
					}
				}
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnRegisterBlow(attacker, victim, realHitEntity, b, ref collisionData, in attackerWeapon);
			}
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0005BE2C File Offset: 0x0005A02C
		private Blow CreateMissileBlow(Agent attackerAgent, in AttackCollisionData collisionData, in MissionWeapon attackerWeapon, Vec3 missilePosition, Vec3 missileStartingPosition)
		{
			Blow blow = new Blow((attackerAgent != null) ? attackerAgent.Index : (-1));
			MissionWeapon missionWeapon = attackerWeapon;
			blow.BlowFlag = (missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) ? BlowFlags.KnockDown : BlowFlags.None);
			AttackCollisionData attackCollisionData = collisionData;
			blow.Direction = attackCollisionData.MissileVelocity.NormalizedCopy();
			blow.SwingDirection = blow.Direction;
			attackCollisionData = collisionData;
			blow.GlobalPosition = attackCollisionData.CollisionGlobalPosition;
			attackCollisionData = collisionData;
			blow.BoneIndex = attackCollisionData.CollisionBoneIndex;
			attackCollisionData = collisionData;
			blow.StrikeType = (StrikeType)attackCollisionData.StrikeType;
			attackCollisionData = collisionData;
			blow.DamageType = (DamageTypes)attackCollisionData.DamageType;
			attackCollisionData = collisionData;
			blow.VictimBodyPart = attackCollisionData.VictimHitBodyPart;
			sbyte b;
			if (attackerAgent == null)
			{
				b = -1;
			}
			else
			{
				Monster monster = attackerAgent.Monster;
				missionWeapon = attackerWeapon;
				b = monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			sbyte b2 = b;
			missionWeapon = attackerWeapon;
			ItemObject item = missionWeapon.Item;
			missionWeapon = attackerWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			attackCollisionData = collisionData;
			int affectorWeaponSlotOrMissileIndex = attackCollisionData.AffectorWeaponSlotOrMissileIndex;
			sbyte b3 = b2;
			attackCollisionData = collisionData;
			blow.WeaponRecord.FillAsMissileBlow(item, currentUsageItem, affectorWeaponSlotOrMissileIndex, b3, missileStartingPosition, missilePosition, attackCollisionData.MissileVelocity);
			blow.BaseMagnitude = collisionData.BaseMagnitude;
			blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
			blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
			blow.InflictedDamage = collisionData.InflictedDamage;
			blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
			blow.DamageCalculated = true;
			return blow;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0005BFC8 File Offset: 0x0005A1C8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal float OnAgentHitBlocked(Agent affectedAgent, Agent affectorAgent, ref AttackCollisionData collisionData, Vec3 blowDirection, Vec3 swingDirection, bool isMissile)
		{
			Blow blow;
			if (isMissile)
			{
				Mission.Missile missile = this._missilesDictionary[collisionData.AffectorWeaponSlotOrMissileIndex];
				MissionWeapon weapon = missile.Weapon;
				blow = this.CreateMissileBlow(affectorAgent, in collisionData, in weapon, missile.GetPosition(), collisionData.MissileStartingPosition);
			}
			else
			{
				int affectorWeaponSlotOrMissileIndex = collisionData.AffectorWeaponSlotOrMissileIndex;
				MissionWeapon missionWeapon = ((affectorWeaponSlotOrMissileIndex >= 0) ? affectorAgent.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
				blow = this.CreateMeleeBlow(affectorAgent, affectedAgent, in collisionData, in missionWeapon, CrushThroughState.None, blowDirection, swingDirection, true);
			}
			return this.OnAgentHit(affectedAgent, affectorAgent, in blow, in collisionData, true, 0f);
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0005C050 File Offset: 0x0005A250
		private Blow CreateMeleeBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, in MissionWeapon attackerWeapon, CrushThroughState crushThroughState, Vec3 blowDirection, Vec3 swingDirection, bool cancelDamage)
		{
			Blow blow = new Blow(attackerAgent.Index);
			AttackCollisionData attackCollisionData = collisionData;
			blow.VictimBodyPart = attackCollisionData.VictimHitBodyPart;
			bool flag = MissionCombatMechanicsHelper.HitWithAnotherBone(in collisionData, attackerAgent, in attackerWeapon);
			attackCollisionData = collisionData;
			MissionWeapon missionWeapon;
			if (attackCollisionData.IsAlternativeAttack)
			{
				missionWeapon = attackerWeapon;
				blow.AttackType = (missionWeapon.IsEmpty ? AgentAttackType.Kick : AgentAttackType.Bash);
			}
			else
			{
				blow.AttackType = AgentAttackType.Standard;
			}
			missionWeapon = attackerWeapon;
			sbyte b;
			if (!missionWeapon.IsEmpty)
			{
				Monster monster = attackerAgent.Monster;
				missionWeapon = attackerWeapon;
				b = monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			else
			{
				b = -1;
			}
			sbyte b2 = b;
			missionWeapon = attackerWeapon;
			ItemObject item = missionWeapon.Item;
			missionWeapon = attackerWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			attackCollisionData = collisionData;
			blow.WeaponRecord.FillAsMeleeBlow(item, currentUsageItem, attackCollisionData.AffectorWeaponSlotOrMissileIndex, b2);
			attackCollisionData = collisionData;
			blow.StrikeType = (StrikeType)attackCollisionData.StrikeType;
			missionWeapon = attackerWeapon;
			DamageTypes damageTypes;
			if (!missionWeapon.IsEmpty && !flag)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.IsAlternativeAttack)
				{
					attackCollisionData = collisionData;
					damageTypes = (DamageTypes)attackCollisionData.DamageType;
					goto IL_0121;
				}
			}
			damageTypes = DamageTypes.Blunt;
			IL_0121:
			blow.DamageType = damageTypes;
			attackCollisionData = collisionData;
			blow.NoIgnore = attackCollisionData.IsAlternativeAttack;
			attackCollisionData = collisionData;
			blow.AttackerStunPeriod = attackCollisionData.AttackerStunPeriod;
			attackCollisionData = collisionData;
			blow.DefenderStunPeriod = attackCollisionData.DefenderStunPeriod;
			blow.BlowFlag = BlowFlags.None;
			attackCollisionData = collisionData;
			blow.GlobalPosition = attackCollisionData.CollisionGlobalPosition;
			attackCollisionData = collisionData;
			blow.BoneIndex = attackCollisionData.CollisionBoneIndex;
			blow.Direction = blowDirection;
			attackCollisionData = collisionData;
			if (attackCollisionData.CollidedWithLastBoneSegment)
			{
				attackCollisionData = collisionData;
				blow.SwingDirection = attackCollisionData.LastBoneSegmentSwingDir;
			}
			else
			{
				blow.SwingDirection = swingDirection;
			}
			if (cancelDamage)
			{
				blow.BaseMagnitude = 0f;
				blow.MovementSpeedDamageModifier = 0f;
				blow.InflictedDamage = 0;
				blow.SelfInflictedDamage = 0;
				blow.AbsorbedByArmor = 0f;
			}
			else
			{
				blow.BaseMagnitude = collisionData.BaseMagnitude;
				blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
				blow.InflictedDamage = collisionData.InflictedDamage;
				blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
				blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
			}
			blow.DamageCalculated = true;
			if (crushThroughState != CrushThroughState.None)
			{
				blow.BlowFlag |= BlowFlags.CrushThrough;
			}
			if (blow.StrikeType == StrikeType.Thrust)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.ThrustTipHit)
				{
					blow.BlowFlag |= BlowFlags.NonTipThrust;
				}
			}
			attackCollisionData = collisionData;
			if (attackCollisionData.IsColliderAgent)
			{
				if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow))
				{
					blow.BlowFlag |= BlowFlags.ShrugOff;
				}
				if (victimAgent.IsHuman)
				{
					Agent mountAgent = victimAgent.MountAgent;
					if (mountAgent != null)
					{
						if (mountAgent.RiderAgent == victimAgent)
						{
							AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
							missionWeapon = attackerWeapon;
							if (agentApplyDamageModel.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
							{
								blow.BlowFlag |= BlowFlags.CanDismount;
							}
						}
					}
					else
					{
						AgentApplyDamageModel agentApplyDamageModel2 = MissionGameModels.Current.AgentApplyDamageModel;
						missionWeapon = attackerWeapon;
						if (agentApplyDamageModel2.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
						{
							blow.BlowFlag |= BlowFlags.KnockBack;
						}
						AgentApplyDamageModel agentApplyDamageModel3 = MissionGameModels.Current.AgentApplyDamageModel;
						missionWeapon = attackerWeapon;
						if (agentApplyDamageModel3.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
						{
							blow.BlowFlag |= BlowFlags.KnockDown;
						}
					}
				}
				else if (victimAgent.IsMount)
				{
					AgentApplyDamageModel agentApplyDamageModel4 = MissionGameModels.Current.AgentApplyDamageModel;
					missionWeapon = attackerWeapon;
					if (agentApplyDamageModel4.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, missionWeapon.CurrentUsageItem, in blow))
					{
						blow.BlowFlag |= BlowFlags.MakesRear;
					}
				}
			}
			return blow;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0005C430 File Offset: 0x0005A630
		internal float OnAgentHit(Agent affectedAgent, Agent affectorAgent, in Blow b, in AttackCollisionData collisionData, bool isBlocked, float damagedHp)
		{
			float num = -1f;
			bool flag = false;
			int affectorWeaponSlotOrMissileIndex = b.WeaponRecord.AffectorWeaponSlotOrMissileIndex;
			Blow blow = b;
			bool isMissile = blow.IsMissile;
			int inflictedDamage = b.InflictedDamage;
			blow = b;
			float num2 = (blow.IsMissile ? (b.GlobalPosition - b.WeaponRecord.StartingPosition).Length : 0f);
			MissionWeapon missionWeapon;
			if (isMissile)
			{
				Mission.Missile missile = this._missilesDictionary[affectorWeaponSlotOrMissileIndex];
				missionWeapon = missile.Weapon;
				flag = missile.MissionObjectToIgnore != null;
			}
			else
			{
				missionWeapon = ((affectorAgent != null && affectorWeaponSlotOrMissileIndex >= 0) ? affectorAgent.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid);
			}
			if (affectorAgent != null && isMissile)
			{
				num = this.GetShootDifficulty(affectedAgent, affectorAgent, b.VictimBodyPart == BoneBodyPartType.Head);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnAgentHit(affectedAgent, affectorAgent, in missionWeapon, in b, in collisionData);
				missionBehavior.OnScoreHit(affectedAgent, affectorAgent, missionWeapon.CurrentUsageItem, isBlocked, flag, in b, in collisionData, damagedHp, num2, num);
			}
			foreach (AgentComponent agentComponent in affectedAgent.Components)
			{
				agentComponent.OnHit(affectorAgent, inflictedDamage, in missionWeapon, in b, in collisionData);
			}
			affectedAgent.CheckToDropFlaggedItem();
			return (float)inflictedDamage;
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0005C5B4 File Offset: 0x0005A7B4
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void MissileAreaDamageCallback(ref AttackCollisionData collisionDataInput, ref Blow blowInput, Agent alreadyDamagedAgent, Agent shooterAgent, bool isBigExplosion)
		{
			float num = (isBigExplosion ? 2.8f : 1.2f);
			float num2 = (isBigExplosion ? 1.6f : 1f);
			float num3 = 1f;
			if (collisionDataInput.MissileVelocity.LengthSquared < 484f)
			{
				num2 *= 0.8f;
				num3 = 0.5f;
			}
			AttackCollisionData attackCollisionData = collisionDataInput;
			blowInput.VictimBodyPart = collisionDataInput.VictimHitBodyPart;
			List<Agent> list = new List<Agent>();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, blowInput.GlobalPosition.AsVec2, num, true);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent.CurrentMortalityState != Agent.MortalityState.Invulnerable && lastFoundAgent != shooterAgent && lastFoundAgent != alreadyDamagedAgent)
				{
					list.Add(lastFoundAgent);
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			foreach (Agent agent in list)
			{
				Blow blow = blowInput;
				blow.DamageCalculated = false;
				attackCollisionData = collisionDataInput;
				float num4 = float.MaxValue;
				sbyte b = -1;
				Skeleton skeleton = agent.AgentVisuals.GetSkeleton();
				sbyte boneCount = skeleton.GetBoneCount();
				MatrixFrame globalFrame = agent.AgentVisuals.GetGlobalFrame();
				for (sbyte b2 = 0; b2 < boneCount; b2 += 1)
				{
					float num5 = globalFrame.TransformToParent(in skeleton.GetBoneEntitialFrame(b2).origin).DistanceSquared(blowInput.GlobalPosition);
					if (num5 < num4)
					{
						b = b2;
						num4 = num5;
					}
				}
				if (num4 <= num * num)
				{
					float num6 = MathF.Sqrt(num4);
					float num7 = 1f;
					if (num6 > num2)
					{
						float num8 = MBMath.Lerp(1f, 3f, (num6 - num2) / (num - num2), 1E-05f);
						num7 = 1f / (num8 * num8);
					}
					num7 *= num3;
					attackCollisionData.SetCollisionBoneIndexForAreaDamage(b);
					MissionWeapon weapon = this._missilesDictionary[attackCollisionData.AffectorWeaponSlotOrMissileIndex].Weapon;
					WeaponComponentData weaponComponentData;
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(shooterAgent, agent, WeakGameEntity.Invalid, 1f, in weapon, false, false, false, ref attackCollisionData, out weaponComponentData, out combatLogData);
					blow.BaseMagnitude = attackCollisionData.BaseMagnitude;
					blow.MovementSpeedDamageModifier = attackCollisionData.MovementSpeedDamageModifier;
					blow.InflictedDamage = attackCollisionData.InflictedDamage;
					blow.SelfInflictedDamage = attackCollisionData.SelfInflictedDamage;
					blow.AbsorbedByArmor = (float)attackCollisionData.AbsorbedByArmor;
					blow.DamageCalculated = true;
					blow.InflictedDamage = MathF.Round((float)blow.InflictedDamage * num7);
					blow.SelfInflictedDamage = MathF.Round((float)blow.SelfInflictedDamage * num7);
					combatLogData.ModifiedDamage = MathF.Round((float)combatLogData.ModifiedDamage * num7);
					this.RegisterBlow(shooterAgent, agent, WeakGameEntity.Invalid, blow, ref attackCollisionData, in weapon, ref combatLogData);
				}
			}
			Mission.OnCameraShakeTriggeredDelegate onCameraShakeTriggered = this.OnCameraShakeTriggered;
			if (onCameraShakeTriggered == null)
			{
				return;
			}
			onCameraShakeTriggered(in blowInput.GlobalPosition, 10f + num * 5f);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0005C8C0 File Offset: 0x0005AAC0
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void OnMissileRemoved(int missileIndex)
		{
			this._missilesDictionary.Remove(missileIndex);
			for (int i = 0; i < this._missilesList.Count; i++)
			{
				if (this._missilesList[i].Index == missileIndex)
				{
					this._missilesList.RemoveAt(i);
					break;
				}
			}
			Action<int> onMissileRemovedEvent = this.OnMissileRemovedEvent;
			if (onMissileRemovedEvent == null)
			{
				return;
			}
			onMissileRemovedEvent(missileIndex);
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0005C924 File Offset: 0x0005AB24
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal bool MissileHitCallback(out int extraHitParticleIndex, ref AttackCollisionData collisionData, Vec3 missileStartingPosition, Vec3 missilePosition, Vec3 missileAngularVelocity, Vec3 movementVelocity, MatrixFrame attachGlobalFrame, MatrixFrame affectedShieldGlobalFrame, int numDamagedAgents, Agent attacker, Agent victim, GameEntity hitEntity)
		{
			WeakGameEntity weakGameEntity = ((hitEntity != null) ? hitEntity.WeakEntity : WeakGameEntity.Invalid);
			Mission.Missile missile = this._missilesDictionary[collisionData.AffectorWeaponSlotOrMissileIndex];
			MissionWeapon weapon = missile.Weapon;
			WeaponFlags weaponFlags = weapon.CurrentUsageItem.WeaponFlags;
			float num = 1f;
			WeaponComponentData weaponComponentData = null;
			AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
			MissionWeapon missionWeapon = missile.Weapon;
			agentApplyDamageModel.DecideMissileWeaponFlags(attacker, in missionWeapon, ref weaponFlags);
			extraHitParticleIndex = -1;
			Mission.MissileCollisionReaction missileCollisionReaction = Mission.MissileCollisionReaction.Invalid;
			bool flag = !GameNetwork.IsSessionActive;
			bool missileHasPhysics = collisionData.MissileHasPhysics;
			PhysicsMaterial fromIndex = PhysicsMaterial.GetFromIndex(collisionData.PhysicsMaterialIndex);
			object obj = (fromIndex.IsValid ? fromIndex.GetFlags() : PhysicsMaterialFlags.None);
			bool flag2 = (weaponFlags & WeaponFlags.AmmoSticksWhenShot) > (WeaponFlags)0UL;
			object obj2 = obj;
			bool flag3 = (obj2 & 1) == 0;
			bool flag4 = (obj2 & 8) != 0;
			MissionObject missionObject = null;
			if (victim == null && weakGameEntity.IsValid)
			{
				WeakGameEntity weakGameEntity2 = weakGameEntity;
				do
				{
					missionObject = weakGameEntity2.GetFirstScriptOfType<MissionObject>();
					weakGameEntity2 = weakGameEntity2.Parent;
				}
				while (missionObject == null && weakGameEntity2.IsValid);
				weakGameEntity = ((missionObject != null) ? missionObject.GameEntity : WeakGameEntity.Invalid);
			}
			Mission.MissileCollisionReaction missileCollisionReaction2;
			if (flag4)
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.PassThrough;
			}
			else if (weaponFlags.HasAnyFlag(WeaponFlags.Burning))
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.BecomeInvisible;
			}
			else if (!flag3 || !flag2)
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.BounceBack;
			}
			else
			{
				missileCollisionReaction2 = Mission.MissileCollisionReaction.Stick;
			}
			bool flag5 = false;
			bool flag6 = victim != null && victim.CurrentMortalityState == Agent.MortalityState.Invulnerable;
			if (collisionData.MissileGoneUnderWater || collisionData.MissileGoneOutOfBorder || flag6)
			{
				missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
			}
			else if (victim == null)
			{
				if (weakGameEntity.IsValid)
				{
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
					Blow blow = this.CreateMissileBlow(attacker, in collisionData, in weapon, missilePosition, missileStartingPosition);
					this.RegisterBlow(attacker, null, weakGameEntity, blow, ref collisionData, in weapon, ref combatLogData);
				}
				missileCollisionReaction = missileCollisionReaction2;
			}
			else if (collisionData.AttackBlockedWithShield)
			{
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				if (!collisionData.IsShieldBroken)
				{
					this.MakeSound(ItemPhysicsSoundContainer.SoundCodePhysicsArrowlikeStone, collisionData.CollisionGlobalPosition, false, false, -1, -1);
				}
				bool flag7 = false;
				if (weaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield))
				{
					if (!collisionData.IsShieldBroken)
					{
						EquipmentIndex offhandWieldedItemIndex = victim.GetOffhandWieldedItemIndex();
						float num2 = (float)collisionData.InflictedDamage;
						float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldPenetrationOffset);
						float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldPenetrationFactor);
						missionWeapon = victim.Equipment[offhandWieldedItemIndex];
						if (num2 > managedParameter + managedParameter2 * (float)missionWeapon.GetGetModifiedArmorForCurrentUsage())
						{
							flag7 = true;
						}
					}
					else
					{
						flag7 = true;
					}
				}
				else if (victim.State == AgentState.Active && collisionData.IsShieldBroken && MissionGameModels.Current.AgentApplyDamageModel.ShouldMissilePassThroughAfterShieldBreak(attacker, weapon.CurrentUsageItem))
				{
					flag7 = true;
				}
				if (flag7)
				{
					victim.MakeVoice(SkinVoiceManager.VoiceType.Pain, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
					num *= 0.4f + MBRandom.RandomFloat * 0.2f;
					missileCollisionReaction = Mission.MissileCollisionReaction.PassThrough;
				}
				else
				{
					missileCollisionReaction = (collisionData.IsShieldBroken ? Mission.MissileCollisionReaction.BecomeInvisible : missileCollisionReaction2);
				}
			}
			else if (collisionData.MissileBlockedWithWeapon)
			{
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, weakGameEntity, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				missileCollisionReaction = Mission.MissileCollisionReaction.BounceBack;
			}
			else
			{
				if (attacker != null && attacker.IsFriendOf(victim))
				{
					if (this.ForceNoFriendlyFire)
					{
						flag5 = true;
					}
					else if (!missileHasPhysics)
					{
						if (flag)
						{
							if (attacker.Controller == AgentControllerType.AI)
							{
								flag5 = true;
							}
						}
						else if ((MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 && MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0) || this.Mode == MissionMode.Duel)
						{
							flag5 = true;
						}
					}
				}
				else if (victim.IsHuman && attacker != null && !attacker.IsEnemyOf(victim))
				{
					flag5 = true;
				}
				else if (flag && attacker != null && attacker.Controller == AgentControllerType.AI && victim.RiderAgent != null && attacker.IsFriendOf(victim.RiderAgent))
				{
					flag5 = true;
				}
				if (flag5)
				{
					if (flag && attacker != null && attacker == Agent.Main && attacker.IsFriendOf(victim))
					{
						InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_you_hit_a_friendly_troop", null).ToString(), Color.ConvertStringToColor("#D65252FF")));
					}
					missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
				}
				else
				{
					bool flag8 = (weaponFlags & WeaponFlags.MultiplePenetration) > (WeaponFlags)0UL;
					CombatLogData combatLogData;
					this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, num, in weapon, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
					Blow blow2 = this.CreateMissileBlow(attacker, in collisionData, in weapon, missilePosition, missileStartingPosition);
					if (collisionData.IsColliderAgent && flag8 && numDamagedAgents > 0)
					{
						blow2.InflictedDamage /= numDamagedAgents;
						blow2.SelfInflictedDamage /= numDamagedAgents;
						combatLogData.InflictedDamage = blow2.InflictedDamage - combatLogData.ModifiedDamage;
					}
					if (collisionData.IsColliderAgent)
					{
						if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentShrugOffBlow(victim, in collisionData, in blow2))
						{
							blow2.BlowFlag |= BlowFlags.ShrugOff;
						}
						else if (victim.IsHuman)
						{
							Agent mountAgent = victim.MountAgent;
							if (mountAgent != null)
							{
								if (mountAgent.RiderAgent == victim && MissionGameModels.Current.AgentApplyDamageModel.DecideAgentDismountedByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.CanDismount;
								}
							}
							else
							{
								if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedBackByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.KnockBack;
								}
								if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedDownByBlow(attacker, victim, in collisionData, weapon.CurrentUsageItem, in blow2))
								{
									blow2.BlowFlag |= BlowFlags.KnockDown;
								}
							}
						}
					}
					if (victim.State == AgentState.Active)
					{
						this.RegisterBlow(attacker, victim, WeakGameEntity.Invalid, blow2, ref collisionData, in weapon, ref combatLogData);
					}
					extraHitParticleIndex = MissionGameModels.Current.DamageParticleModel.GetMissileAttackParticle(attacker, victim, in blow2, in collisionData);
					if (flag8 && numDamagedAgents < 3)
					{
						missileCollisionReaction = Mission.MissileCollisionReaction.PassThrough;
					}
					else
					{
						missileCollisionReaction = missileCollisionReaction2;
						if (missileCollisionReaction2 == Mission.MissileCollisionReaction.Stick && !collisionData.CollidedWithShieldOnBack)
						{
							bool flag9 = this.CombatType == Mission.MissionCombatType.Combat;
							if (flag9)
							{
								bool flag10 = victim.IsHuman && collisionData.VictimHitBodyPart == BoneBodyPartType.Head;
								flag9 = victim.State != AgentState.Active || !flag10;
							}
							if (flag9)
							{
								float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.MissileMinimumDamageToStick);
								float num3 = 2f * managedParameter3;
								if ((float)blow2.InflictedDamage < managedParameter3 && blow2.AbsorbedByArmor > num3 && !GameNetwork.IsClientOrReplay)
								{
									missileCollisionReaction = Mission.MissileCollisionReaction.BounceBack;
								}
							}
							else
							{
								missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
							}
						}
					}
				}
			}
			if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
			{
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
			}
			bool flag11;
			MatrixFrame matrixFrame;
			if (!collisionData.MissileHasPhysics && missileCollisionReaction == Mission.MissileCollisionReaction.Stick)
			{
				AttackCollisionData attackCollisionData = collisionData;
				missionWeapon = missile.Weapon;
				matrixFrame = this.CalculateAttachedLocalFrame(in attachGlobalFrame, attackCollisionData, missionWeapon.CurrentUsageItem, victim, weakGameEntity, movementVelocity, missileAngularVelocity, affectedShieldGlobalFrame, true, out flag11);
			}
			else
			{
				missionWeapon = missile.Weapon;
				MatrixFrame matrixFrame2 = missionWeapon.CurrentUsageItem.GetMissileStartingFrame();
				missionWeapon = missile.Weapon;
				MatrixFrame matrixFrame3 = missionWeapon.CurrentUsageItem.StickingFrame;
				matrixFrame2 = matrixFrame2.TransformToParent(in matrixFrame3);
				matrixFrame = attachGlobalFrame.TransformToParent(in matrixFrame2);
				missionWeapon = missile.Weapon;
				matrixFrame3 = missionWeapon.CurrentUsageItem.GetMissileStartingFrame();
				matrixFrame = matrixFrame.TransformToParent(in matrixFrame3);
				matrixFrame.origin.z = Math.Max(matrixFrame.origin.z, -100f);
				missionObject = null;
				flag11 = false;
			}
			Vec3 zero = Vec3.Zero;
			Vec3 zero2 = Vec3.Zero;
			if (missileCollisionReaction == Mission.MissileCollisionReaction.BounceBack)
			{
				WeaponFlags weaponFlags2 = weaponFlags & WeaponFlags.AmmoBreakOnBounceBackMask;
				if (weaponFlags2 == WeaponFlags.AmmoCanBreakOnBounceBack)
				{
					Vec3 vec = collisionData.MissileVelocity;
					if (vec.Length > ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BreakableProjectileMinimumBreakSpeed))
					{
						goto IL_0774;
					}
				}
				if (weaponFlags2 != WeaponFlags.AmmoBreaksOnBounceBack)
				{
					missile.CalculateBounceBackVelocity(missileAngularVelocity, collisionData, out zero, out zero2);
					goto IL_07A6;
				}
				IL_0774:
				missileCollisionReaction = Mission.MissileCollisionReaction.BecomeInvisible;
				if (weapon.Item.ItemType != ItemObject.ItemTypeEnum.SlingStones)
				{
					extraHitParticleIndex = ParticleSystemManager.GetRuntimeIdByName("psys_game_broken_arrow");
				}
			}
			IL_07A6:
			if (missile.ShooterAgent != null && (missileCollisionReaction == Mission.MissileCollisionReaction.Stick || missileCollisionReaction == Mission.MissileCollisionReaction.BounceBack) && (victim == null || collisionData.AttackBlockedWithShield || collisionData.MissileBlockedWithWeapon))
			{
				missionWeapon = missile.Weapon;
				bool flag12;
				if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.Stone)
				{
					missionWeapon = missile.Weapon;
					if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.Boulder)
					{
						missionWeapon = missile.Weapon;
						if (missionWeapon.CurrentUsageItem.WeaponClass != WeaponClass.BallistaStone)
						{
							missionWeapon = missile.Weapon;
							flag12 = missionWeapon.CurrentUsageItem.WeaponClass == WeaponClass.BallistaBoulder;
							goto IL_0837;
						}
					}
				}
				flag12 = true;
				IL_0837:
				float num4;
				if (!flag12)
				{
					missionWeapon = missile.Weapon;
					num4 = (missionWeapon.CurrentUsageItem.IsAmmo ? 7f : 9f);
				}
				else
				{
					num4 = 13.1f;
				}
				float num5 = num4;
				Agent shooterAgent = missile.ShooterAgent;
				Vec3 vec = missile.GetPosition();
				this.AddSoundAlarmFactorToAgents(shooterAgent, in vec, num5);
			}
			this.HandleMissileCollisionReaction(collisionData.AffectorWeaponSlotOrMissileIndex, missileCollisionReaction, matrixFrame, flag11, attacker, victim, collisionData.AttackBlockedWithShield, collisionData.CollisionBoneIndex, missionObject, zero, zero2, -1);
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissileHit(attacker, victim, flag5, collisionData);
			}
			return missileCollisionReaction != Mission.MissileCollisionReaction.PassThrough;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0005D230 File Offset: 0x0005B430
		public void HandleMissileCollisionReaction(int missileIndex, Mission.MissileCollisionReaction collisionReaction, MatrixFrame attachLocalFrame, bool isAttachedFrameLocal, Agent attackerAgent, Agent attachedAgent, bool attachedToShield, sbyte attachedBoneIndex, MissionObject attachedMissionObject, Vec3 bounceBackVelocity, Vec3 bounceBackAngularVelocity, int forcedSpawnIndex)
		{
			Mission.Missile missile = this._missilesDictionary[missileIndex];
			MissionObjectId missionObjectId = new MissionObjectId(-1, true);
			switch (collisionReaction)
			{
			case Mission.MissileCollisionReaction.Stick:
				missile.Entity.SetVisibilityExcludeParents(true);
				if (attachedAgent != null)
				{
					this.PrepareMissileWeaponForDrop(missileIndex);
					if (attachedToShield)
					{
						EquipmentIndex offhandWieldedItemIndex = attachedAgent.GetOffhandWieldedItemIndex();
						attachedAgent.AttachWeaponToWeapon(offhandWieldedItemIndex, missile.Weapon, missile.Entity, ref attachLocalFrame);
					}
					else
					{
						attachedAgent.AttachWeaponToBone(missile.Weapon, missile.Entity, attachedBoneIndex, ref attachLocalFrame);
					}
				}
				else
				{
					Vec3 zero = Vec3.Zero;
					missionObjectId = this.SpawnWeaponAsDropFromMissile(missileIndex, attachedMissionObject, in attachLocalFrame, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithStaticPhysics, in zero, in zero, forcedSpawnIndex);
				}
				break;
			case Mission.MissileCollisionReaction.BounceBack:
				missile.Entity.SetVisibilityExcludeParents(true);
				missionObjectId = this.SpawnWeaponAsDropFromMissile(missileIndex, null, in attachLocalFrame, Mission.WeaponSpawnFlags.AsMissile | Mission.WeaponSpawnFlags.WithPhysics, in bounceBackVelocity, in bounceBackAngularVelocity, forcedSpawnIndex);
				break;
			case Mission.MissileCollisionReaction.BecomeInvisible:
				missile.Entity.Remove(81);
				break;
			}
			bool flag = collisionReaction != Mission.MissileCollisionReaction.PassThrough;
			if (GameNetwork.IsServerOrRecorder)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new HandleMissileCollisionReaction(missileIndex, collisionReaction, attachLocalFrame, isAttachedFrameLocal, attackerAgent.Index, (attachedAgent != null) ? attachedAgent.Index : (-1), attachedToShield, attachedBoneIndex, (attachedMissionObject != null) ? attachedMissionObject.Id : MissionObjectId.Invalid, bounceBackVelocity, bounceBackAngularVelocity, missionObjectId.Id));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
			}
			else if (GameNetwork.IsClientOrReplay && flag)
			{
				this.RemoveMissileAsClient(missileIndex);
			}
			foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
			{
				missionBehavior.OnMissileCollisionReaction(collisionReaction, attackerAgent, attachedAgent, attachedBoneIndex);
			}
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0005D3D0 File Offset: 0x0005B5D0
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void MissileCalculatePassbySoundParametersCallbackMT(int missileIndex, ref SoundEventParameter soundEventParameter)
		{
			this._missilesDictionary[missileIndex].CalculatePassbySoundParametersMT(ref soundEventParameter);
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x0005D3E4 File Offset: 0x0005B5E4
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void ChargeDamageCallback(ref AttackCollisionData collisionData, Blow blow, Agent attacker, Agent victim)
		{
			if (victim.CurrentMortalityState != Agent.MortalityState.Invulnerable && (attacker.RiderAgent == null || attacker.IsEnemyOf(victim) || this.IsFriendlyFireAllowedForChargeDamage()))
			{
				WeaponComponentData weaponComponentData;
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, 1f, in MissionWeapon.Invalid, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				if (collisionData.CollidedWithShieldOnBack && weaponComponentData != null && victim != null && victim.IsMainAgent)
				{
					InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("ui_hit_shield_on_back", null).ToString(), Color.ConvertStringToColor("#FFFFFFFF")));
				}
				if ((float)collisionData.InflictedDamage > 0f)
				{
					blow.BaseMagnitude = collisionData.BaseMagnitude;
					blow.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
					blow.InflictedDamage = collisionData.InflictedDamage;
					blow.SelfInflictedDamage = collisionData.SelfInflictedDamage;
					blow.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
					blow.DamageCalculated = true;
					if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedBackByBlow(attacker, victim, in collisionData, null, in blow))
					{
						blow.BlowFlag |= BlowFlags.KnockBack;
					}
					else
					{
						blow.BlowFlag &= ~BlowFlags.KnockBack;
					}
					if (MissionGameModels.Current.AgentApplyDamageModel.DecideAgentKnockedDownByBlow(attacker, victim, in collisionData, null, in blow))
					{
						blow.BlowFlag |= BlowFlags.KnockDown;
					}
					WeakGameEntity invalid = WeakGameEntity.Invalid;
					Blow blow2 = blow;
					MissionWeapon missionWeapon = default(MissionWeapon);
					this.RegisterBlow(attacker, victim, invalid, blow2, ref collisionData, in missionWeapon, ref combatLogData);
				}
			}
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x0005D548 File Offset: 0x0005B748
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void FallDamageCallback(ref AttackCollisionData collisionData, Blow b, Agent attacker, Agent victim)
		{
			if (victim.CurrentMortalityState != Agent.MortalityState.Invulnerable)
			{
				WeaponComponentData weaponComponentData;
				CombatLogData combatLogData;
				this.GetAttackCollisionResults(attacker, victim, WeakGameEntity.Invalid, 1f, in MissionWeapon.Invalid, false, false, false, ref collisionData, out weaponComponentData, out combatLogData);
				b.BaseMagnitude = collisionData.BaseMagnitude;
				b.MovementSpeedDamageModifier = collisionData.MovementSpeedDamageModifier;
				b.InflictedDamage = collisionData.InflictedDamage;
				b.SelfInflictedDamage = collisionData.SelfInflictedDamage;
				b.AbsorbedByArmor = (float)collisionData.AbsorbedByArmor;
				b.DamageCalculated = true;
				if (b.InflictedDamage > 0)
				{
					Agent riderAgent = victim.RiderAgent;
					WeakGameEntity invalid = WeakGameEntity.Invalid;
					Blow blow = b;
					MissionWeapon missionWeapon = default(MissionWeapon);
					this.RegisterBlow(attacker, victim, invalid, blow, ref collisionData, in missionWeapon, ref combatLogData);
					if (riderAgent != null)
					{
						this.FallDamageCallback(ref collisionData, b, riderAgent, riderAgent);
					}
				}
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x0005D608 File Offset: 0x0005B808
		public void KillAgentsOnEntity(GameEntity entity, Agent destroyerAgent, bool burnAgents)
		{
			if (entity == null)
			{
				return;
			}
			int num;
			sbyte b;
			if (destroyerAgent != null)
			{
				num = destroyerAgent.Index;
				b = destroyerAgent.Monster.MainHandItemBoneIndex;
			}
			else
			{
				num = -1;
				b = -1;
			}
			Vec3 vec;
			Vec3 vec2;
			entity.GetPhysicsMinMax(true, out vec, out vec2, false);
			Vec2 vec3 = (vec2.AsVec2 + vec.AsVec2) * 0.5f;
			float num2 = (vec2.AsVec2 - vec.AsVec2).Length * 0.5f;
			Blow blow = new Blow(num);
			blow.DamageCalculated = true;
			blow.BaseMagnitude = 2000f;
			blow.InflictedDamage = 2000;
			blow.Direction = new Vec3(0f, 0f, -1f, -1f);
			blow.DamageType = DamageTypes.Blunt;
			blow.BoneIndex = 0;
			blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, 0);
			if (burnAgents)
			{
				blow.WeaponRecord.WeaponFlags = blow.WeaponRecord.WeaponFlags | (WeaponFlags.AffectsArea | WeaponFlags.Burning);
				blow.WeaponRecord.CurrentPosition = blow.GlobalPosition;
				blow.WeaponRecord.StartingPosition = blow.GlobalPosition;
			}
			MatrixFrame globalFrame = entity.GetGlobalFrame();
			Vec3 vec4 = vec3.ToVec3(0f);
			Vec2 asVec = globalFrame.TransformToParent(in vec4).AsVec2;
			List<Agent> list = new List<Agent>();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(this, asVec, num2, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				WeakGameEntity weakGameEntity = lastFoundAgent.GetSteppedEntity();
				while (weakGameEntity.IsValid && !(weakGameEntity == entity))
				{
					weakGameEntity = weakGameEntity.Parent;
				}
				if (weakGameEntity.IsValid)
				{
					list.Add(lastFoundAgent);
				}
				AgentProximityMap.FindNext(this, ref proximityMapSearchStruct);
			}
			foreach (Agent agent in list)
			{
				blow.GlobalPosition = agent.Position;
				AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Abdomen, b, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
				agent.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0005D894 File Offset: 0x0005BA94
		public void KillAgentCheat(Agent agent)
		{
			if (!GameNetwork.IsClientOrReplay && this.Mode != MissionMode.CutScene && this.Mode != MissionMode.Conversation && this.Mode != MissionMode.Barter)
			{
				Agent agent2 = this.MainAgent ?? agent;
				Blow blow = new Blow(agent2.Index);
				blow.DamageType = DamageTypes.Blunt;
				blow.BoneIndex = agent.Monster.HeadLookDirectionBoneIndex;
				blow.GlobalPosition = agent.Position;
				blow.GlobalPosition.z = blow.GlobalPosition.z + agent.GetEyeGlobalHeight();
				blow.BaseMagnitude = 2000f;
				blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
				blow.InflictedDamage = 2000;
				blow.SwingDirection = agent.LookDirection;
				if (this.InputManager.IsGameKeyDown(2))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(-1f, 0f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(3))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(1f, 0f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(1))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(0f, -1f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				else if (this.InputManager.IsGameKeyDown(0))
				{
					MatrixFrame matrixFrame = agent.Frame;
					Vec3 vec = new Vec3(0f, 1f, 0f, -1f);
					blow.SwingDirection = matrixFrame.rotation.TransformToParent(in vec);
					blow.SwingDirection.Normalize();
				}
				blow.Direction = blow.SwingDirection;
				blow.DamageCalculated = true;
				sbyte mainHandItemBoneIndex = agent2.Monster.MainHandItemBoneIndex;
				AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
				agent.RegisterBlow(blow, in attackCollisionDataForDebugPurpose);
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x0005DB4C File Offset: 0x0005BD4C
		public bool KillCheats(bool killAll, bool killEnemy, bool killHorse, bool killYourself)
		{
			bool flag = false;
			if (!GameNetwork.IsClientOrReplay && this.Mode != MissionMode.CutScene && this.Mode != MissionMode.Conversation && this.Mode != MissionMode.Barter)
			{
				if (killYourself)
				{
					if (this.MainAgent != null)
					{
						if (killHorse)
						{
							if (this.MainAgent.MountAgent != null)
							{
								Agent mountAgent = this.MainAgent.MountAgent;
								this.KillAgentCheat(mountAgent);
								flag = true;
							}
						}
						else
						{
							Agent mainAgent = this.MainAgent;
							this.KillAgentCheat(mainAgent);
							flag = true;
						}
					}
				}
				else
				{
					bool flag2 = false;
					int num = this.Agents.Count - 1;
					while (num >= 0 && !flag2)
					{
						Agent agent = this.Agents[num];
						if (agent != this.MainAgent && agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanAttack) && this.PlayerTeam != null)
						{
							if (killEnemy)
							{
								if (agent.Team != null && agent.Team.IsValid && this.PlayerTeam.IsEnemyOf(agent.Team))
								{
									if (killHorse && agent.HasMount)
									{
										if (agent.MountAgent != null)
										{
											this.KillAgentCheat(agent.MountAgent);
											if (!killAll)
											{
												flag2 = true;
											}
											flag = true;
										}
									}
									else
									{
										this.KillAgentCheat(agent);
										if (!killAll)
										{
											flag2 = true;
										}
										flag = true;
									}
								}
							}
							else if (agent.Team != null && agent.Team.IsValid && this.PlayerTeam.IsFriendOf(agent.Team))
							{
								if (killHorse)
								{
									if (agent.MountAgent != null)
									{
										this.KillAgentCheat(agent.MountAgent);
										if (!killAll)
										{
											flag2 = true;
										}
										flag = true;
									}
								}
								else
								{
									this.KillAgentCheat(agent);
									if (!killAll)
									{
										flag2 = true;
									}
									flag = true;
								}
							}
						}
						num--;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x0005DD10 File Offset: 0x0005BF10
		private bool CancelsDamageAndBlocksAttackBecauseOfNonEnemyCase(Agent attacker, Agent victim)
		{
			if (victim == null || attacker == null)
			{
				return false;
			}
			bool flag = !GameNetwork.IsSessionActive || this.ForceNoFriendlyFire || (MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 && MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0) || this.Mode == MissionMode.Duel || attacker.Controller == AgentControllerType.AI;
			bool flag2 = attacker.IsFriendOf(victim);
			return (flag && flag2) || (victim.IsHuman && !flag2 && !attacker.IsEnemyOf(victim));
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x0005DD8C File Offset: 0x0005BF8C
		private bool IsFriendlyFireAllowedForChargeDamage()
		{
			if (!GameNetwork.IsServer)
			{
				return false;
			}
			if (this._doesMissionAllowChargeDamageOnFriendly == null || this._doesMissionAllowChargeDamageOnFriendly == null)
			{
				MissionMultiplayerGameModeBase missionBehavior = this.GetMissionBehavior<MissionMultiplayerGameModeBase>();
				this._doesMissionAllowChargeDamageOnFriendly = new bool?(missionBehavior.IsGameModeAllowChargeDamageOnFriendly);
			}
			return this._doesMissionAllowChargeDamageOnFriendly.Value && (MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0 || MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0005DDFC File Offset: 0x0005BFFC
		public bool CanTakeControlOfAgent(Agent agentToTakeControlOf)
		{
			return this._canPlayerTakeControlOfAnotherAgentWhenDead && this.MainAgent == null && agentToTakeControlOf != null && agentToTakeControlOf.IsHuman && agentToTakeControlOf.IsActive() && agentToTakeControlOf.Team != null && agentToTakeControlOf.Team == this.PlayerTeam && !agentToTakeControlOf.IsUsingGameObject && !agentToTakeControlOf.Character.IsHero && agentToTakeControlOf.Health / agentToTakeControlOf.HealthLimit >= 0.25f;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0005DE70 File Offset: 0x0005C070
		public void SetPlayerCanTakeControlOfAnotherAgentWhenDead()
		{
			this._canPlayerTakeControlOfAnotherAgentWhenDead = true;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0005DE79 File Offset: 0x0005C079
		public void TakeControlOfAgent(Agent agentToTakeControlOf)
		{
			if (this.IsFastForward)
			{
				this.IsFastForward = false;
			}
			agentToTakeControlOf.Controller = AgentControllerType.Player;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0005DE91 File Offset: 0x0005C091
		public float GetDamageMultiplierOfCombatDifficulty(Agent victimAgent, Agent attackerAgent = null)
		{
			if (MissionGameModels.Current.MissionDifficultyModel != null)
			{
				return MissionGameModels.Current.MissionDifficultyModel.GetDamageMultiplierOfCombatDifficulty(victimAgent, attackerAgent);
			}
			return 1f;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0005DEB8 File Offset: 0x0005C0B8
		public float GetShootDifficulty(Agent affectedAgent, Agent affectorAgent, bool isHeadShot)
		{
			Vec2 vec = affectedAgent.MovementVelocity - affectorAgent.MovementVelocity;
			Vec3 vec2 = new Vec3(vec.x, vec.y, 0f, -1f);
			Vec3 vec3 = affectedAgent.Position - affectorAgent.Position;
			float num = vec3.Normalize();
			float num2 = vec2.Normalize();
			float length = Vec3.CrossProduct(vec2, vec3).Length;
			float num3 = MBMath.ClampFloat(0.3f * ((4f + num) / 4f) * ((4f + length * num2) / 4f), 1f, 12f);
			if (isHeadShot)
			{
				num3 *= 1.2f;
			}
			return num3;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0005DF70 File Offset: 0x0005C170
		private MatrixFrame CalculateAttachedLocalFrame(in MatrixFrame attachedGlobalFrame, AttackCollisionData collisionData, WeaponComponentData missileWeapon, Agent affectedAgent, WeakGameEntity hitEntity, Vec3 missileMovementVelocity, Vec3 missileRotationSpeed, MatrixFrame shieldGlobalFrame, bool shouldMissilePenetrate, out bool isAttachedFrameLocal)
		{
			isAttachedFrameLocal = false;
			MatrixFrame matrixFrame = attachedGlobalFrame;
			bool isNonZero = missileWeapon.RotationSpeed.IsNonZero;
			bool flag = affectedAgent != null && !collisionData.AttackBlockedWithShield && missileWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.AmmoSticksWhenShot);
			float managedParameter = ManagedParameters.Instance.GetManagedParameter(flag ? (isNonZero ? ManagedParametersEnum.RotatingProjectileMinPenetration : ManagedParametersEnum.ProjectileMinPenetration) : ManagedParametersEnum.ObjectMinPenetration);
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(flag ? (isNonZero ? ManagedParametersEnum.RotatingProjectileMaxPenetration : ManagedParametersEnum.ProjectileMaxPenetration) : ManagedParametersEnum.ObjectMaxPenetration);
			Vec3 vec = missileMovementVelocity;
			float num = vec.Normalize();
			float num2 = MBMath.ClampFloat(flag ? ((float)collisionData.InflictedDamage / affectedAgent.HealthLimit) : (num / ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ProjectileMaxPenetrationSpeed)), 0f, 1f);
			if (shouldMissilePenetrate)
			{
				float num3 = managedParameter + (managedParameter2 - managedParameter) * num2;
				matrixFrame.origin += vec * num3;
			}
			MatrixFrame matrixFrame2;
			if (missileRotationSpeed.IsNonZero)
			{
				float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(flag ? ManagedParametersEnum.AgentProjectileNormalWeight : ManagedParametersEnum.ProjectileNormalWeight);
				matrixFrame2 = missileWeapon.GetMissileStartingFrame();
				Vec3 vec2 = matrixFrame2.TransformToParent(in missileRotationSpeed);
				Vec3 vec3 = -collisionData.CollisionGlobalNormal;
				float num4 = vec2.x * vec2.x;
				float num5 = vec2.y * vec2.y;
				float num6 = vec2.z * vec2.z;
				int num7 = ((num4 > num5 && num4 > num6) ? 0 : ((num5 > num6) ? 1 : 2));
				vec3 -= vec3.ProjectOnUnitVector(matrixFrame.rotation[num7]);
				Vec3 vec4 = Vec3.CrossProduct(vec, vec3.NormalizedCopy());
				float num8 = vec4.Normalize();
				matrixFrame.rotation.RotateAboutAnArbitraryVector(in vec4, MathF.Asin(MathF.Clamp(num8, 0f, 1f)) * managedParameter3);
			}
			if (!collisionData.AttackBlockedWithShield && affectedAgent != null)
			{
				float num9 = Vec3.DotProduct(collisionData.CollisionGlobalNormal, vec) + 1f;
				if (num9 > 0.5f)
				{
					matrixFrame.origin -= num9 * 0.1f * collisionData.CollisionGlobalNormal;
				}
			}
			matrixFrame2 = missileWeapon.GetMissileStartingFrame();
			MatrixFrame matrixFrame3 = missileWeapon.StickingFrame;
			matrixFrame2 = matrixFrame2.TransformToParent(in matrixFrame3);
			matrixFrame = matrixFrame.TransformToParent(in matrixFrame2);
			matrixFrame3 = missileWeapon.GetMissileStartingFrame();
			matrixFrame = matrixFrame.TransformToParent(in matrixFrame3);
			if (collisionData.AttackBlockedWithShield)
			{
				matrixFrame = shieldGlobalFrame.TransformToLocal(in matrixFrame);
				isAttachedFrameLocal = true;
			}
			else if (affectedAgent != null)
			{
				if (flag)
				{
					MBAgentVisuals agentVisuals = affectedAgent.AgentVisuals;
					matrixFrame3 = agentVisuals.GetGlobalFrame();
					matrixFrame2 = agentVisuals.GetSkeleton().GetBoneEntitialFrameWithIndex(collisionData.CollisionBoneIndex);
					matrixFrame = matrixFrame3.TransformToParent(in matrixFrame2).GetUnitRotFrame(affectedAgent.AgentScale).TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
			}
			else if (hitEntity.IsValid)
			{
				if (collisionData.CollisionBoneIndex >= 0)
				{
					matrixFrame = hitEntity.Skeleton.GetBoneEntitialFrameWithIndex(collisionData.CollisionBoneIndex).TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
				else
				{
					matrixFrame2 = hitEntity.GetGlobalFrame();
					matrixFrame = matrixFrame2.TransformToLocalNonOrthogonal(in matrixFrame);
					isAttachedFrameLocal = true;
				}
			}
			else
			{
				matrixFrame.origin.z = Math.Max(matrixFrame.origin.z, -100f);
			}
			return matrixFrame;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0005E2CC File Offset: 0x0005C4CC
		[UsedImplicitly]
		[MBCallback(null, true)]
		internal void GetDefendCollisionResults(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, int attackerWeaponSlotIndex, bool isAlternativeAttack, StrikeType strikeType, Agent.UsageDirection attackDirection, float collisionDistanceOnWeapon, float attackProgress, bool attackIsParried, bool isPassiveUsageHit, bool isHeavyAttack, ref float defenderStunPeriod, ref float attackerStunPeriod, ref bool crushedThrough)
		{
			bool flag = false;
			MissionCombatMechanicsHelper.GetDefendCollisionResults(attackerAgent, defenderAgent, collisionResult, attackerWeaponSlotIndex, isAlternativeAttack, strikeType, attackDirection, collisionDistanceOnWeapon, attackProgress, attackIsParried, isPassiveUsageHit, isHeavyAttack, ref defenderStunPeriod, ref attackerStunPeriod, ref crushedThrough, ref flag);
			if ((crushedThrough || flag) && (attackerAgent.CanLogCombatFor || defenderAgent.CanLogCombatFor))
			{
				CombatLogData combatLogData = new CombatLogData(false, attackerAgent.IsHuman, attackerAgent.IsMine, attackerAgent.RiderAgent != null, attackerAgent.RiderAgent != null && attackerAgent.RiderAgent.IsMine, attackerAgent.IsMount, defenderAgent.IsHuman, defenderAgent.IsMine, defenderAgent.Health <= 0f, defenderAgent.HasMount, defenderAgent.RiderAgent != null && defenderAgent.RiderAgent.IsMine, defenderAgent.IsMount, null, defenderAgent.RiderAgent == attackerAgent, crushedThrough, flag, 0f);
				this.AddCombatLogSafe(attackerAgent, defenderAgent, combatLogData);
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0005E3B0 File Offset: 0x0005C5B0
		private CombatLogData GetAttackCollisionResults(Agent attackerAgent, Agent victimAgent, WeakGameEntity hitObject, float momentumRemaining, in MissionWeapon attackerWeapon, bool crushedThrough, bool cancelDamage, bool crushedThroughWithoutAgentCollision, ref AttackCollisionData attackCollisionData, out WeaponComponentData shieldOnBack, out CombatLogData combatLog)
		{
			AttackInformation attackInformation = new AttackInformation(attackerAgent, victimAgent, hitObject, in attackCollisionData, in attackerWeapon);
			shieldOnBack = attackInformation.ShieldOnBack;
			int num;
			MissionCombatMechanicsHelper.GetAttackCollisionResults(in attackInformation, crushedThrough, momentumRemaining, cancelDamage, ref attackCollisionData, out combatLog, out num);
			float num2 = (float)attackCollisionData.InflictedDamage;
			if (num2 > 0f)
			{
				float num3 = MissionGameModels.Current.AgentApplyDamageModel.CalculateDamage(in attackInformation, in attackCollisionData, num2);
				combatLog.ModifiedDamage = MathF.Round(num3 - num2);
				attackCollisionData.InflictedDamage = MathF.Round(num3);
			}
			else
			{
				combatLog.ModifiedDamage = 0;
				attackCollisionData.InflictedDamage = 0;
			}
			combatLog.ReflectedDamage = 0;
			if (!attackCollisionData.IsFallDamage && attackInformation.IsFriendlyFire)
			{
				if (!attackInformation.IsAttackerAIControlled && GameNetwork.IsSessionActive)
				{
					int num4 = (attackCollisionData.IsMissile ? MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					attackCollisionData.SelfInflictedDamage = MathF.Round((float)attackCollisionData.InflictedDamage * ((float)num4 * 0.01f));
					attackCollisionData.SelfInflictedDamage = MBMath.ClampInt(attackCollisionData.SelfInflictedDamage, 0, 2000);
					int num5 = (attackCollisionData.IsMissile ? MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					attackCollisionData.InflictedDamage = MathF.Round((float)attackCollisionData.InflictedDamage * ((float)num5 * 0.01f));
					attackCollisionData.InflictedDamage = MBMath.ClampInt(attackCollisionData.InflictedDamage, 0, 2000);
					combatLog.InflictedDamage = attackCollisionData.InflictedDamage;
				}
				combatLog.IsFriendlyFire = true;
				combatLog.ReflectedDamage = attackCollisionData.SelfInflictedDamage;
			}
			if (attackCollisionData.AttackBlockedWithShield && attackCollisionData.InflictedDamage > 0 && (int)attackInformation.VictimShield.HitPoints - attackCollisionData.InflictedDamage <= 0)
			{
				attackCollisionData.IsShieldBroken = true;
			}
			if (!crushedThroughWithoutAgentCollision)
			{
				combatLog.BodyPartHit = attackCollisionData.VictimHitBodyPart;
			}
			return combatLog;
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0005E588 File Offset: 0x0005C788
		private void PrintAttackCollisionResults(Agent attackerAgent, Agent victimAgent, MissionObject missionObjectHit, ref AttackCollisionData attackCollisionData, ref CombatLogData combatLog)
		{
			if (attackCollisionData.IsColliderAgent && !attackCollisionData.AttackBlockedWithShield && attackerAgent != null && (attackerAgent.CanLogCombatFor || victimAgent.CanLogCombatFor) && victimAgent.State == AgentState.Active)
			{
				combatLog.MissionObjectHit = missionObjectHit;
				this.AddCombatLogSafe(attackerAgent, victimAgent, combatLog);
			}
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0005E5E0 File Offset: 0x0005C7E0
		public void AddCombatLogSafe(Agent attackerAgent, Agent victimAgent, CombatLogData combatLog)
		{
			MissionObject missionObjectHit = combatLog.MissionObjectHit;
			combatLog.SetVictimAgent(victimAgent);
			if (GameNetwork.IsServerOrRecorder)
			{
				CombatLogNetworkMessage combatLogNetworkMessage = new CombatLogNetworkMessage(attackerAgent.Index, (victimAgent != null) ? victimAgent.Index : (-1), (missionObjectHit != null) ? missionObjectHit.Id : MissionObjectId.Invalid, combatLog);
				object obj = ((attackerAgent == null) ? null : (attackerAgent.IsHuman ? attackerAgent : attackerAgent.RiderAgent));
				object obj2;
				if (obj == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer missionPeer = obj.MissionPeer;
					obj2 = ((missionPeer != null) ? missionPeer.Peer.Communicator : null);
				}
				NetworkCommunicator networkCommunicator = obj2 as NetworkCommunicator;
				object obj3 = ((victimAgent == null) ? null : (victimAgent.IsHuman ? victimAgent : victimAgent.RiderAgent));
				object obj4;
				if (obj3 == null)
				{
					obj4 = null;
				}
				else
				{
					MissionPeer missionPeer2 = obj3.MissionPeer;
					obj4 = ((missionPeer2 != null) ? missionPeer2.Peer.Communicator : null);
				}
				NetworkCommunicator networkCommunicator2 = obj4 as NetworkCommunicator;
				if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator);
					GameNetwork.WriteMessage(combatLogNetworkMessage);
					GameNetwork.EndModuleEventAsServer();
				}
				if (networkCommunicator2 != null && !networkCommunicator2.IsServerPeer && networkCommunicator2 != networkCommunicator)
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
					GameNetwork.WriteMessage(combatLogNetworkMessage);
					GameNetwork.EndModuleEventAsServer();
				}
			}
			this._combatLogsCreated.Enqueue(combatLog);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0005E6EC File Offset: 0x0005C8EC
		public MissionObject CreateMissionObjectFromPrefab(string prefab, MatrixFrame frame, bool hasCustomRestOffset, float restOffset, Action<GameEntity> actionAppliedBeforeScriptInitialization)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				GameEntity gameEntity;
				if (hasCustomRestOffset)
				{
					gameEntity = GameEntity.InstantiateWithRestOffset(this.Scene, prefab, true, frame, restOffset, false, "");
				}
				else
				{
					gameEntity = GameEntity.Instantiate(this.Scene, prefab, frame, false);
				}
				actionAppliedBeforeScriptInitialization(gameEntity);
				gameEntity.CallScriptCallbacks(true);
				MissionObject firstScriptOfType = gameEntity.GetFirstScriptOfType<MissionObject>();
				List<MissionObjectId> list = new List<MissionObjectId>();
				using (IEnumerator<GameEntity> enumerator = gameEntity.GetChildren().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MissionObject firstScriptOfType2;
						if ((firstScriptOfType2 = enumerator.Current.GetFirstScriptOfType<MissionObject>()) != null)
						{
							list.Add(firstScriptOfType2.Id);
						}
					}
				}
				if (GameNetwork.IsServerOrRecorder)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new CreateMissionObject(firstScriptOfType.Id, prefab, frame, list));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					this.AddDynamicallySpawnedMissionObjectInfo(new Mission.DynamicallyCreatedEntity(prefab, firstScriptOfType.Id, frame, ref list));
				}
				return firstScriptOfType;
			}
			return null;
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0005E7D8 File Offset: 0x0005C9D8
		public int GetNearbyAllyAgentsCount(Vec2 center, float radius, Team team)
		{
			return this.GetNearbyAgentsCountAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Friend);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0005E7E9 File Offset: 0x0005C9E9
		public MBList<Agent> GetNearbyAllyAgents(Vec2 center, float radius, Team team, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Friend, agents);
			return agents;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0005E805 File Offset: 0x0005CA05
		public MBList<Agent> GetNearbyEnemyAgents(Vec2 center, float radius, Team team, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, team.MBTeam, Mission.GetNearbyAgentsAuxType.Enemy, agents);
			return agents;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0005E821 File Offset: 0x0005CA21
		public MBList<Agent> GetNearbyAgents(Vec2 center, float radius, MBList<Agent> agents)
		{
			agents.Clear();
			this.GetNearbyAgentsAux(center, radius, MBTeam.InvalidTeam, Mission.GetNearbyAgentsAuxType.All, agents);
			return agents;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0005E83C File Offset: 0x0005CA3C
		public bool IsFormationUnitPositionAvailableMT(ref WorldPosition formationPosition, ref WorldPosition unitPosition, ref WorldPosition nearestAvailableUnitPosition, float manhattanDistance, Team team)
		{
			if (!formationPosition.IsValid || formationPosition.GetNavMeshMT() == UIntPtr.Zero || !unitPosition.IsValid || unitPosition.GetNavMeshMT() == UIntPtr.Zero)
			{
				return false;
			}
			if (this.IsFormationUnitPositionAvailable_AdditionalCondition != null && !this.IsFormationUnitPositionAvailable_AdditionalCondition(unitPosition, team))
			{
				return false;
			}
			if (this.Mode == MissionMode.Deployment && this.DeploymentPlan.HasDeploymentBoundaries(team))
			{
				IMissionDeploymentPlan deploymentPlan = this.DeploymentPlan;
				Vec2 asVec = unitPosition.AsVec2;
				if (!deploymentPlan.IsPositionInsideDeploymentBoundaries(team, in asVec))
				{
					return false;
				}
			}
			return this.IsFormationUnitPositionAvailableAuxMT(ref formationPosition, ref unitPosition, ref nearestAvailableUnitPosition, manhattanDistance);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0005E8DC File Offset: 0x0005CADC
		public bool IsOrderPositionAvailable(in WorldPosition orderPosition, Team team)
		{
			WorldPosition worldPosition = orderPosition;
			if (worldPosition.IsValid)
			{
				worldPosition = orderPosition;
				if (!(worldPosition.GetNavMesh() == UIntPtr.Zero))
				{
					if (this.IsFormationUnitPositionAvailable_AdditionalCondition != null && !this.IsFormationUnitPositionAvailable_AdditionalCondition(orderPosition, team))
					{
						return false;
					}
					worldPosition = orderPosition;
					return this.IsPositionInsideBoundaries(worldPosition.AsVec2);
				}
			}
			return false;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0005E948 File Offset: 0x0005CB48
		public bool IsFormationUnitPositionAvailable(ref WorldPosition unitPosition, Team team)
		{
			WorldPosition worldPosition = unitPosition;
			float num = 1f;
			WorldPosition invalid = WorldPosition.Invalid;
			return this.IsFormationUnitPositionAvailableMT(ref worldPosition, ref unitPosition, ref invalid, num, team);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0005E975 File Offset: 0x0005CB75
		public bool HasSceneMapPatch()
		{
			return this.InitializerRecord.SceneHasMapPatch;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0005E984 File Offset: 0x0005CB84
		public bool GetPatchSceneEncounterPosition(out Vec3 position)
		{
			if (this.InitializerRecord.SceneHasMapPatch)
			{
				Vec2 patchCoordinates = this.InitializerRecord.PatchCoordinates;
				float northRotation = this.Scene.GetNorthRotation();
				Vec2 vec;
				Vec2 vec2;
				this.Boundaries.GetOrientedBoundariesBox(out vec, out vec2, northRotation);
				Vec2 side = Vec2.Side;
				side.RotateCCW(northRotation);
				Vec2 vec3 = side.LeftVec();
				Vec2 vec4 = vec2 - vec;
				Vec2 vec5 = vec.x * side + vec.y * vec3 + vec4.x * patchCoordinates.x * side + vec4.y * patchCoordinates.y * vec3;
				position = vec5.ToVec3(this.Scene.GetTerrainHeight(vec5, true));
				return true;
			}
			position = Vec3.Invalid;
			return false;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0005EA68 File Offset: 0x0005CC68
		public bool GetPatchSceneEncounterDirection(out Vec2 direction)
		{
			if (this.InitializerRecord.SceneHasMapPatch)
			{
				float northRotation = this.Scene.GetNorthRotation();
				direction = this.InitializerRecord.PatchEncounterDir;
				direction.RotateCCW(northRotation);
				return true;
			}
			direction = Vec2.Invalid;
			return false;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0005EAB4 File Offset: 0x0005CCB4
		private void TickDebugAgents()
		{
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
		public void AddTimerToDynamicEntity(GameEntity gameEntity, float timeToKill = 10f)
		{
			Mission.DynamicEntityInfo dynamicEntityInfo = new Mission.DynamicEntityInfo
			{
				Entity = gameEntity,
				TimerToDisable = new Timer(this.CurrentTime, timeToKill, true)
			};
			this._dynamicEntities.Add(dynamicEntityInfo);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0005EAF1 File Offset: 0x0005CCF1
		public void AddListener(IMissionListener listener)
		{
			this._listeners.Add(listener);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0005EAFF File Offset: 0x0005CCFF
		public void RemoveListener(IMissionListener listener)
		{
			this._listeners.Remove(listener);
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0005EB10 File Offset: 0x0005CD10
		public void OnAgentFleeing(Agent agent)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAgentFleeing(agent);
			}
			agent.OnFleeing();
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0005EB50 File Offset: 0x0005CD50
		public void OnAgentPanicked(Agent agent)
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAgentPanicked(agent);
			}
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0005EB88 File Offset: 0x0005CD88
		public void OnTeamDeployed(Team team)
		{
			if (this.MissionBehaviors != null)
			{
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnTeamDeployed(team);
				}
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0005EBE4 File Offset: 0x0005CDE4
		public void OnBattleSideDeployed(BattleSideEnum side)
		{
			if (this.MissionBehaviors != null)
			{
				foreach (MissionBehavior missionBehavior in this.MissionBehaviors)
				{
					missionBehavior.OnBattleSideDeployed(side);
				}
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0005EC40 File Offset: 0x0005CE40
		public void OnDeploymentFinished()
		{
			this.IsDeploymentFinished = true;
			foreach (Team team in this.Teams)
			{
				if (team.TeamAI != null)
				{
					team.TeamAI.OnDeploymentFinished();
				}
			}
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnDeploymentFinished();
			}
			Action deploymentFinishedEvent = this.DeploymentFinishedEvent;
			if (deploymentFinishedEvent == null)
			{
				return;
			}
			deploymentFinishedEvent();
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0005ECE0 File Offset: 0x0005CEE0
		public void OnAfterDeploymentFinished()
		{
			for (int i = this.MissionBehaviors.Count - 1; i >= 0; i--)
			{
				this.MissionBehaviors[i].OnAfterDeploymentFinished();
			}
			foreach (Agent agent in this.Agents)
			{
				AgentStatCalculateModel agentStatCalculateModel = MissionGameModels.Current.AgentStatCalculateModel;
				if (agentStatCalculateModel != null)
				{
					agentStatCalculateModel.InitializeAgentStatsAfterDeploymentFinished(agent);
				}
				AgentStatCalculateModel agentStatCalculateModel2 = MissionGameModels.Current.AgentStatCalculateModel;
				if (agentStatCalculateModel2 != null)
				{
					agentStatCalculateModel2.InitializeMissionEquipmentAfterDeploymentFinished(agent);
				}
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0005ED84 File Offset: 0x0005CF84
		public void OnFormationCaptainChanged(Formation formation)
		{
			Action<Formation> formationCaptainChanged = this.FormationCaptainChanged;
			if (formationCaptainChanged == null)
			{
				return;
			}
			formationCaptainChanged(formation);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0005ED97 File Offset: 0x0005CF97
		public void SetFastForwardingFromUI(bool fastForwarding)
		{
			this.IsFastForward = fastForwarding;
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0005EDA0 File Offset: 0x0005CFA0
		public bool CheckIfBattleInRetreat()
		{
			Func<bool> isBattleInRetreatEvent = this.IsBattleInRetreatEvent;
			return isBattleInRetreatEvent != null && isBattleInRetreatEvent();
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0005EDB3 File Offset: 0x0005CFB3
		public void AddSpawnedItemEntityCreatedAtRuntime(SpawnedItemEntity spawnedItemEntity)
		{
			this._spawnedItemEntitiesCreatedAtRuntime.Add(spawnedItemEntity);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0005EDC1 File Offset: 0x0005CFC1
		public void TriggerOnItemPickUpEvent(Agent agent, SpawnedItemEntity spawnedItemEntity)
		{
			Action<Agent, SpawnedItemEntity> onItemPickUp = this.OnItemPickUp;
			if (onItemPickUp == null)
			{
				return;
			}
			onItemPickUp(agent, spawnedItemEntity);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0005EDD8 File Offset: 0x0005CFD8
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal static void DebugLogNativeMissionNetworkEvent(int eventEnum, string eventName, int bitCount)
		{
			int num = eventEnum + CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo.GetMaximumValue() + 1;
			DebugNetworkEventStatistics.StartEvent(eventName, num);
			DebugNetworkEventStatistics.AddDataToStatistic(bitCount);
			DebugNetworkEventStatistics.EndEvent();
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0005EE06 File Offset: 0x0005D006
		[UsedImplicitly]
		[MBCallback(null, false)]
		internal void PauseMission()
		{
			this._missionState.Paused = true;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0005EE14 File Offset: 0x0005D014
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_n_allies", "mission")]
		public static string KillNAllies(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'mission.kill_n_allies [count]";
			}
			if (Mission.Current != null && num > 0)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					if (num <= 0)
					{
						break;
					}
					if (team.IsPlayerTeam)
					{
						foreach (Agent agent in team.ActiveAgents.ToList<Agent>())
						{
							if (agent.IsAIControlled)
							{
								Mission.Current.KillAgentCheat(agent);
								if (--num <= 0)
								{
									break;
								}
							}
						}
					}
				}
				return "n allied agents killed.";
			}
			return "No active mission found or less than 1 agent to kill.";
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0005EF20 File Offset: 0x0005D120
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_all_allies", "mission")]
		public static string KillAllAllies(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (Mission.Current != null)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.IsPlayerTeam)
					{
						foreach (Agent agent in team.ActiveAgents.ToList<Agent>())
						{
							if (agent.IsAIControlled)
							{
								Mission.Current.KillAgentCheat(agent);
							}
						}
					}
				}
				return "Allied agents killed.";
			}
			return "No active mission found";
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0005EFF0 File Offset: 0x0005D1F0
		[CommandLineFunctionality.CommandLineArgumentFunction("toggleDisableDying", "mission")]
		public static string ToggleDisableDying(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'toggleDisableDying [index]' or just 'toggleDisableDying' for making all agents invincible.";
			}
			if (Mission.Current == null)
			{
				return "No active mission found";
			}
			if (strings.Count == 0 || num == -1)
			{
				Mission.Current.DisableDying = !Mission.Current.DisableDying;
				if (Mission.Current.DisableDying)
				{
					return "Dying disabled for all";
				}
				return "Dying not disabled for all";
			}
			else
			{
				Agent agent = Mission.Current.FindAgentWithIndex(num);
				if (agent != null)
				{
					agent.ToggleInvulnerable();
					return "Disable Dying for agent " + num.ToString() + ": " + (agent.CurrentMortalityState == Agent.MortalityState.Invulnerable).ToString();
				}
				return "Invalid agent index " + num.ToString();
			}
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0005F0C8 File Offset: 0x0005D2C8
		[CommandLineFunctionality.CommandLineArgumentFunction("toggleDisableDyingTeam", "mission")]
		public static string ToggleDisableDyingTeam(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			int num = 0;
			if (strings.Count > 0 && !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'toggleDisableDyingTeam [team_no]' for making all active agents of a team invincible.";
			}
			int num2 = 0;
			foreach (Agent agent in Mission.Current.AllAgents)
			{
				if (agent.Team != null && agent.Team.MBTeam.Index == num)
				{
					agent.ToggleInvulnerable();
					num2++;
				}
			}
			return string.Concat(new object[]
			{
				"Toggled invulnerability for active agents of team ",
				num.ToString(),
				", agent count: ",
				num2
			});
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0005F1A0 File Offset: 0x0005D3A0
		[CommandLineFunctionality.CommandLineArgumentFunction("killAgent", "mission")]
		public static string KillAgent(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			if (Mission.Current == null)
			{
				return "Current mission does not exist.";
			}
			int num;
			if (strings.Count == 0 || !int.TryParse(strings[0], out num))
			{
				return "Please write the arguments in the correct format. Correct format is: 'killAgent [index]'";
			}
			Agent agent = Mission.Current.FindAgentWithIndex(num);
			if (agent == null)
			{
				return "Agent " + num.ToString() + " not found.";
			}
			if (agent.State == AgentState.Active)
			{
				Mission.Current.KillAgentCheat(agent);
				return "Agent " + num.ToString() + " died.";
			}
			return "Agent " + num.ToString() + " already dead.";
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0005F254 File Offset: 0x0005D454
		[CommandLineFunctionality.CommandLineArgumentFunction("set_battering_ram_speed", "mission")]
		public static string IncreaseBatteringRamSpeeds(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			float num;
			if (strings.Count == 0 || !float.TryParse(strings[0], out num))
			{
				return "Please enter a speed value";
			}
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				if (missionObject.GameEntity.HasScriptOfType<BatteringRam>())
				{
					missionObject.GameEntity.GetFirstScriptOfType<BatteringRam>().MovementComponent.MaxSpeed = num;
					missionObject.GameEntity.GetFirstScriptOfType<BatteringRam>().MovementComponent.MinSpeed = num;
				}
			}
			return "Battering ram max speed increased.";
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0005F31C File Offset: 0x0005D51C
		[CommandLineFunctionality.CommandLineArgumentFunction("set_siege_tower_speed", "mission")]
		public static string IncreaseSiegeTowerSpeed(List<string> strings)
		{
			if (GameNetwork.IsSessionActive)
			{
				return "Does not work on multiplayer.";
			}
			float num;
			if (strings.Count == 0 || !float.TryParse(strings[0], out num))
			{
				return "Please enter a speed value";
			}
			foreach (MissionObject missionObject in Mission.Current.ActiveMissionObjects)
			{
				if (missionObject.GameEntity.HasScriptOfType<SiegeTower>())
				{
					missionObject.GameEntity.GetFirstScriptOfType<SiegeTower>().MovementComponent.MaxSpeed = num;
					missionObject.GameEntity.GetFirstScriptOfType<SiegeTower>().MovementComponent.MinSpeed = num;
				}
			}
			return "Siege tower max speed increased.";
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0005F3E4 File Offset: 0x0005D5E4
		[CommandLineFunctionality.CommandLineArgumentFunction("reload_managed_core_params", "game")]
		public static string LoadParamsDebug(List<string> strings)
		{
			if (!GameNetwork.IsSessionActive)
			{
				ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_core_parameters"));
				return "Managed core parameters reloaded.";
			}
			return "Does not work on multiplayer.";
		}

		// Token: 0x0400087A RID: 2170
		public const int MaxRuntimeMissionObjects = 8191;

		// Token: 0x0400087B RID: 2171
		private static readonly object GetNearbyAgentsAuxLock = new object();

		// Token: 0x0400087F RID: 2175
		private int _lastSceneMissionObjectIdCount;

		// Token: 0x04000880 RID: 2176
		private int _lastRuntimeMissionObjectIdCount;

		// Token: 0x04000881 RID: 2177
		private bool _isMainAgentObjectInteractionEnabled = true;

		// Token: 0x0400088C RID: 2188
		private List<Mission.TimeSpeedRequest> _timeSpeedRequests = new List<Mission.TimeSpeedRequest>();

		// Token: 0x0400088D RID: 2189
		private bool _isMainAgentItemInteractionEnabled = true;

		// Token: 0x0400088E RID: 2190
		private readonly MBList<MissionObject> _activeMissionObjects;

		// Token: 0x0400088F RID: 2191
		private readonly MBList<MissionObject> _missionObjects;

		// Token: 0x04000890 RID: 2192
		private readonly List<SpawnedItemEntity> _spawnedItemEntitiesCreatedAtRuntime;

		// Token: 0x04000891 RID: 2193
		private readonly MBList<Mission.DynamicallyCreatedEntity> _addedEntitiesInfo;

		// Token: 0x04000892 RID: 2194
		private readonly Stack<ValueTuple<int, float>> _emptyRuntimeMissionObjectIds;

		// Token: 0x04000893 RID: 2195
		private static bool _isCameraFirstPerson = false;

		// Token: 0x04000897 RID: 2199
		private MissionMode _missionMode;

		// Token: 0x04000898 RID: 2200
		private float _cachedMissionTime;

		// Token: 0x0400089A RID: 2202
		public const int MaxNavMeshId = 1000000;

		// Token: 0x0400089B RID: 2203
		private const float NavigationMeshHeightLimit = 1.5f;

		// Token: 0x0400089C RID: 2204
		private const float SpeedBonusFactorForSwing = 0.7f;

		// Token: 0x0400089D RID: 2205
		private const float SpeedBonusFactorForThrust = 0.5f;

		// Token: 0x0400089E RID: 2206
		private const float _exitTimeInSeconds = 0.6f;

		// Token: 0x0400089F RID: 2207
		private const int MaxNavMeshPerDynamicObject = 10;

		// Token: 0x040008AA RID: 2218
		private bool? _doesMissionAllowChargeDamageOnFriendly;

		// Token: 0x040008AC RID: 2220
		private bool _missionEnded;

		// Token: 0x040008AD RID: 2221
		private Dictionary<int, Mission.Missile> _missilesDictionary;

		// Token: 0x040008AE RID: 2222
		private MBList<Mission.Missile> _missilesList;

		// Token: 0x040008AF RID: 2223
		private readonly List<Mission.DynamicEntityInfo> _dynamicEntities = new List<Mission.DynamicEntityInfo>();

		// Token: 0x040008B2 RID: 2226
		public bool DisableDying;

		// Token: 0x040008B4 RID: 2228
		public bool ForceNoFriendlyFire;

		// Token: 0x040008B5 RID: 2229
		public const int MaxDamage = 2000;

		// Token: 0x040008B6 RID: 2230
		public bool IsFriendlyMission = true;

		// Token: 0x040008B7 RID: 2231
		public BasicCultureObject MusicCulture;

		// Token: 0x040008B8 RID: 2232
		private int _nextDynamicNavMeshIdStart = 1000050;

		// Token: 0x040008B9 RID: 2233
		private MissionState _missionState;

		// Token: 0x040008BA RID: 2234
		private List<IMissionListener> _listeners = new List<IMissionListener>();

		// Token: 0x040008BB RID: 2235
		private BasicMissionTimer _leaveMissionTimer;

		// Token: 0x040008BC RID: 2236
		private MBReadOnlyList<MBSubModuleBase> _cachedSubModuleList;

		// Token: 0x040008BD RID: 2237
		private readonly MBList<KeyValuePair<Agent, MissionTime>> _mountsWithoutRiders;

		// Token: 0x040008C0 RID: 2240
		private List<MissionBehavior> _otherMissionBehaviors;

		// Token: 0x040008C1 RID: 2241
		private readonly object _lockHelper = new object();

		// Token: 0x040008C2 RID: 2242
		private AgentList _activeAgents;

		// Token: 0x040008C3 RID: 2243
		private IMissionDeploymentPlan _deploymentPlan;

		// Token: 0x040008C5 RID: 2245
		public bool IsOrderMenuOpen;

		// Token: 0x040008C6 RID: 2246
		public bool IsTransferMenuOpen;

		// Token: 0x040008C7 RID: 2247
		public bool IsInPhotoMode;

		// Token: 0x040008C8 RID: 2248
		private Agent _initialPlayerAgent;

		// Token: 0x040008C9 RID: 2249
		private Agent _mainAgent;

		// Token: 0x040008CA RID: 2250
		private Action _onLoadingEndedAction;

		// Token: 0x040008CB RID: 2251
		private Timer _inMissionLoadingScreenTimer;

		// Token: 0x040008CC RID: 2252
		public bool AllowAiTicking = true;

		// Token: 0x040008CD RID: 2253
		private int _agentCreationIndex;

		// Token: 0x040008CE RID: 2254
		private readonly MBList<FleePosition>[] _fleePositions = new MBList<FleePosition>[3];

		// Token: 0x040008CF RID: 2255
		private bool _doesMissionRequireCivilianEquipment;

		// Token: 0x040008D0 RID: 2256
		public IAgentVisualCreator AgentVisualCreator;

		// Token: 0x040008D1 RID: 2257
		private readonly int[] _initialAgentCountPerSide = new int[2];

		// Token: 0x040008D2 RID: 2258
		private readonly int[] _removedAgentCountPerSide = new int[2];

		// Token: 0x040008D5 RID: 2261
		private ConcurrentQueue<CombatLogData> _combatLogsCreated = new ConcurrentQueue<CombatLogData>();

		// Token: 0x040008D6 RID: 2262
		private AgentList _allAgents;

		// Token: 0x040008D7 RID: 2263
		[TupleElementNames(new string[] { "Action", "Agent", "Param1", "Param2" })]
		private MBList<ValueTuple<Mission.MissionTickAction, Agent, int, int>> _tickActions = new MBList<ValueTuple<Mission.MissionTickAction, Agent, int, int>>();

		// Token: 0x040008D8 RID: 2264
		private readonly object _tickActionsLock = new object();

		// Token: 0x040008D9 RID: 2265
		private List<SiegeWeapon> _attackerWeaponsForFriendlyFirePreventing = new List<SiegeWeapon>();

		// Token: 0x040008DB RID: 2267
		private bool _isFastForward;

		// Token: 0x040008E3 RID: 2275
		private float _missionEndTime;

		// Token: 0x040008E4 RID: 2276
		public float MissionCloseTimeAfterFinish = 30f;

		// Token: 0x040008E5 RID: 2277
		private static Mission _current = null;

		// Token: 0x040008E8 RID: 2280
		public float NextCheckTimeEndMission = 10f;

		// Token: 0x040008EA RID: 2282
		public int NumOfFormationsSpawnedTeamOne;

		// Token: 0x040008EB RID: 2283
		private SoundEvent _ambientSoundEvent;

		// Token: 0x040008EC RID: 2284
		private readonly BattleSpawnPathSelector _battleSpawnPathSelector;

		// Token: 0x040008ED RID: 2285
		private int _agentCount;

		// Token: 0x040008EE RID: 2286
		public int NumOfFormationsSpawnedTeamTwo;

		// Token: 0x040008FA RID: 2298
		private bool _canPlayerTakeControlOfAnotherAgentWhenDead;

		// Token: 0x040008FC RID: 2300
		private bool tickCompleted = true;

		// Token: 0x020004EB RID: 1259
		public class MBBoundaryCollection : IDictionary<string, ICollection<Vec2>>, ICollection<KeyValuePair<string, ICollection<Vec2>>>, IEnumerable<KeyValuePair<string, ICollection<Vec2>>>, IEnumerable, INotifyCollectionChanged
		{
			// Token: 0x06003B37 RID: 15159 RVA: 0x000ED715 File Offset: 0x000EB915
			IEnumerator IEnumerable.GetEnumerator()
			{
				int count = this.Count;
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					string boundaryName = MBAPI.IMBMission.GetBoundaryName(this._mission.Pointer, i);
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(boundaryName);
					yield return new KeyValuePair<string, ICollection<Vec2>>(boundaryName, boundaryPoints);
					num = i;
				}
				yield break;
			}

			// Token: 0x06003B38 RID: 15160 RVA: 0x000ED724 File Offset: 0x000EB924
			public IEnumerator<KeyValuePair<string, ICollection<Vec2>>> GetEnumerator()
			{
				int count = this.Count;
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					string boundaryName = MBAPI.IMBMission.GetBoundaryName(this._mission.Pointer, i);
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(boundaryName);
					yield return new KeyValuePair<string, ICollection<Vec2>>(boundaryName, boundaryPoints);
					num = i;
				}
				yield break;
			}

			// Token: 0x17000A2F RID: 2607
			// (get) Token: 0x06003B39 RID: 15161 RVA: 0x000ED733 File Offset: 0x000EB933
			public int Count
			{
				get
				{
					return MBAPI.IMBMission.GetBoundaryCount(this._mission.Pointer);
				}
			}

			// Token: 0x06003B3A RID: 15162 RVA: 0x000ED74A File Offset: 0x000EB94A
			public float GetBoundaryRadius(string name)
			{
				return MBAPI.IMBMission.GetBoundaryRadius(this._mission.Pointer, name);
			}

			// Token: 0x17000A30 RID: 2608
			// (get) Token: 0x06003B3B RID: 15163 RVA: 0x000ED762 File Offset: 0x000EB962
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06003B3C RID: 15164 RVA: 0x000ED768 File Offset: 0x000EB968
			public void GetOrientedBoundariesBox(out Vec2 boxMinimum, out Vec2 boxMaximum, float rotationInRadians = 0f)
			{
				Vec2 side = Vec2.Side;
				side.RotateCCW(rotationInRadians);
				Vec2 vec = side.LeftVec();
				boxMinimum = new Vec2(float.MaxValue, float.MaxValue);
				boxMaximum = new Vec2(float.MinValue, float.MinValue);
				foreach (ICollection<Vec2> collection in this.Values)
				{
					foreach (Vec2 vec2 in collection)
					{
						float num = Vec2.DotProduct(vec2, side);
						float num2 = Vec2.DotProduct(vec2, vec);
						boxMinimum.x = ((num < boxMinimum.x) ? num : boxMinimum.x);
						boxMinimum.y = ((num2 < boxMinimum.y) ? num2 : boxMinimum.y);
						boxMaximum.x = ((num > boxMaximum.x) ? num : boxMaximum.x);
						boxMaximum.y = ((num2 > boxMaximum.y) ? num2 : boxMaximum.y);
					}
				}
			}

			// Token: 0x06003B3D RID: 15165 RVA: 0x000ED8A0 File Offset: 0x000EBAA0
			internal MBBoundaryCollection(Mission mission)
			{
				this._mission = mission;
			}

			// Token: 0x06003B3E RID: 15166 RVA: 0x000ED8AF File Offset: 0x000EBAAF
			public void Add(KeyValuePair<string, ICollection<Vec2>> item)
			{
				this.Add(item.Key, item.Value);
			}

			// Token: 0x06003B3F RID: 15167 RVA: 0x000ED8C8 File Offset: 0x000EBAC8
			public void Clear()
			{
				foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
				{
					this.Remove(keyValuePair.Key);
				}
			}

			// Token: 0x06003B40 RID: 15168 RVA: 0x000ED918 File Offset: 0x000EBB18
			public bool Contains(KeyValuePair<string, ICollection<Vec2>> item)
			{
				return this.ContainsKey(item.Key);
			}

			// Token: 0x06003B41 RID: 15169 RVA: 0x000ED928 File Offset: 0x000EBB28
			public void CopyTo(KeyValuePair<string, ICollection<Vec2>>[] array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex");
				}
				foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
				{
					array[arrayIndex] = keyValuePair;
					arrayIndex++;
					if (arrayIndex >= array.Length)
					{
						throw new ArgumentException("Not enough size in array.");
					}
				}
			}

			// Token: 0x06003B42 RID: 15170 RVA: 0x000ED9A4 File Offset: 0x000EBBA4
			public bool Remove(KeyValuePair<string, ICollection<Vec2>> item)
			{
				return this.Remove(item.Key);
			}

			// Token: 0x17000A31 RID: 2609
			// (get) Token: 0x06003B43 RID: 15171 RVA: 0x000ED9B4 File Offset: 0x000EBBB4
			public ICollection<string> Keys
			{
				get
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
					{
						list.Add(keyValuePair.Key);
					}
					return list;
				}
			}

			// Token: 0x17000A32 RID: 2610
			// (get) Token: 0x06003B44 RID: 15172 RVA: 0x000EDA0C File Offset: 0x000EBC0C
			public ICollection<ICollection<Vec2>> Values
			{
				get
				{
					List<ICollection<Vec2>> list = new List<ICollection<Vec2>>();
					foreach (KeyValuePair<string, ICollection<Vec2>> keyValuePair in this)
					{
						list.Add(keyValuePair.Value);
					}
					return list;
				}
			}

			// Token: 0x17000A33 RID: 2611
			public ICollection<Vec2> this[string name]
			{
				get
				{
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					List<Vec2> boundaryPoints = this.GetBoundaryPoints(name);
					if (boundaryPoints.Count == 0)
					{
						throw new KeyNotFoundException();
					}
					return boundaryPoints;
				}
				set
				{
					if (name == null)
					{
						throw new ArgumentNullException("name");
					}
					this.Add(name, value);
				}
			}

			// Token: 0x06003B47 RID: 15175 RVA: 0x000EDAAE File Offset: 0x000EBCAE
			public void Add(string name, ICollection<Vec2> points)
			{
				this.Add(name, points, true);
			}

			// Token: 0x06003B48 RID: 15176 RVA: 0x000EDABC File Offset: 0x000EBCBC
			public void Add(string name, ICollection<Vec2> points, bool isAllowanceInside)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				if (points.Count < 3)
				{
					throw new ArgumentException("At least three points are required.");
				}
				bool flag = MBAPI.IMBMission.AddBoundary(this._mission.Pointer, name, points.ToArray<Vec2>(), points.Count, isAllowanceInside);
				if (!flag)
				{
					throw new ArgumentException("An element with the same name already exists.");
				}
				if (flag)
				{
					NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
					if (collectionChanged != null)
					{
						collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, name));
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						formation.ResetMovementOrderPositionCache();
					}
				}
			}

			// Token: 0x06003B49 RID: 15177 RVA: 0x000EDBC8 File Offset: 0x000EBDC8
			public bool ContainsKey(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				return this.GetBoundaryPoints(name).Count > 0;
			}

			// Token: 0x06003B4A RID: 15178 RVA: 0x000EDBE8 File Offset: 0x000EBDE8
			public bool Remove(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				bool flag = MBAPI.IMBMission.RemoveBoundary(this._mission.Pointer, name);
				if (flag)
				{
					NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
					if (collectionChanged != null)
					{
						collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, name));
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						formation.ResetMovementOrderPositionCache();
					}
				}
				return flag;
			}

			// Token: 0x06003B4B RID: 15179 RVA: 0x000EDCB8 File Offset: 0x000EBEB8
			public bool TryGetValue(string name, out ICollection<Vec2> points)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				points = this.GetBoundaryPoints(name);
				return points.Count > 0;
			}

			// Token: 0x06003B4C RID: 15180 RVA: 0x000EDCDC File Offset: 0x000EBEDC
			private List<Vec2> GetBoundaryPoints(string name)
			{
				List<Vec2> list = new List<Vec2>();
				Vec2[] array = new Vec2[10];
				for (int i = 0; i < 1000; i += 10)
				{
					int num = -1;
					MBAPI.IMBMission.GetBoundaryPoints(this._mission.Pointer, name, i, array, 10, ref num);
					list.AddRange(array.Take<Vec2>(num));
					if (num < 10)
					{
						break;
					}
				}
				return list;
			}

			// Token: 0x140000AF RID: 175
			// (add) Token: 0x06003B4D RID: 15181 RVA: 0x000EDD3C File Offset: 0x000EBF3C
			// (remove) Token: 0x06003B4E RID: 15182 RVA: 0x000EDD74 File Offset: 0x000EBF74
			public event NotifyCollectionChangedEventHandler CollectionChanged;

			// Token: 0x04001C55 RID: 7253
			private readonly Mission _mission;
		}

		// Token: 0x020004EC RID: 1260
		private enum GetNearbyAgentsAuxType
		{
			// Token: 0x04001C58 RID: 7256
			Friend = 1,
			// Token: 0x04001C59 RID: 7257
			Enemy,
			// Token: 0x04001C5A RID: 7258
			All
		}

		// Token: 0x020004ED RID: 1261
		public class DynamicallyCreatedEntity
		{
			// Token: 0x06003B4F RID: 15183 RVA: 0x000EDDA9 File Offset: 0x000EBFA9
			public DynamicallyCreatedEntity(string prefab, MissionObjectId objectId, MatrixFrame frame, ref List<MissionObjectId> childObjectIds)
			{
				this.Prefab = prefab;
				this.ObjectId = objectId;
				this.Frame = frame;
				this.ChildObjectIds = childObjectIds;
			}

			// Token: 0x04001C5B RID: 7259
			public string Prefab;

			// Token: 0x04001C5C RID: 7260
			public MissionObjectId ObjectId;

			// Token: 0x04001C5D RID: 7261
			public MatrixFrame Frame;

			// Token: 0x04001C5E RID: 7262
			public List<MissionObjectId> ChildObjectIds;
		}

		// Token: 0x020004EE RID: 1262
		[Flags]
		[EngineStruct("Weapon_spawn_flag", true, "wsf", false)]
		public enum WeaponSpawnFlags : uint
		{
			// Token: 0x04001C60 RID: 7264
			None = 0U,
			// Token: 0x04001C61 RID: 7265
			WithHolster = 1U,
			// Token: 0x04001C62 RID: 7266
			WithoutHolster = 2U,
			// Token: 0x04001C63 RID: 7267
			AsMissile = 4U,
			// Token: 0x04001C64 RID: 7268
			WithPhysics = 8U,
			// Token: 0x04001C65 RID: 7269
			WithStaticPhysics = 16U,
			// Token: 0x04001C66 RID: 7270
			UseAnimationSpeed = 32U,
			// Token: 0x04001C67 RID: 7271
			CannotBePickedUp = 64U
		}

		// Token: 0x020004EF RID: 1263
		[EngineStruct("Mission_combat_type", false, null)]
		public enum MissionCombatType
		{
			// Token: 0x04001C69 RID: 7273
			Combat,
			// Token: 0x04001C6A RID: 7274
			ArenaCombat,
			// Token: 0x04001C6B RID: 7275
			NoCombat
		}

		// Token: 0x020004F0 RID: 1264
		public enum BattleSizeType
		{
			// Token: 0x04001C6D RID: 7277
			Battle,
			// Token: 0x04001C6E RID: 7278
			Siege,
			// Token: 0x04001C6F RID: 7279
			SallyOut
		}

		// Token: 0x020004F1 RID: 1265
		[EngineStruct("Agent_creation_result", false, null)]
		internal struct AgentCreationResult
		{
			// Token: 0x04001C70 RID: 7280
			internal int Index;

			// Token: 0x04001C71 RID: 7281
			internal UIntPtr AgentPtr;

			// Token: 0x04001C72 RID: 7282
			internal UIntPtr PositionPtr;

			// Token: 0x04001C73 RID: 7283
			internal UIntPtr IndexPtr;

			// Token: 0x04001C74 RID: 7284
			internal UIntPtr FlagsPtr;

			// Token: 0x04001C75 RID: 7285
			internal UIntPtr StatePtr;

			// Token: 0x04001C76 RID: 7286
			internal UIntPtr MovementModePointer;

			// Token: 0x04001C77 RID: 7287
			internal UIntPtr ControllerPointer;

			// Token: 0x04001C78 RID: 7288
			internal UIntPtr MovementDirectionPointer;

			// Token: 0x04001C79 RID: 7289
			internal UIntPtr PrimaryWieldedItemIndexPointer;

			// Token: 0x04001C7A RID: 7290
			internal UIntPtr OffHandWieldedItemIndexPointer;

			// Token: 0x04001C7B RID: 7291
			internal UIntPtr Channel0CurrentActionPointer;

			// Token: 0x04001C7C RID: 7292
			internal UIntPtr Channel1CurrentActionPointer;

			// Token: 0x04001C7D RID: 7293
			internal UIntPtr MaximumForwardUnlimitedSpeed;
		}

		// Token: 0x020004F2 RID: 1266
		public struct TimeSpeedRequest
		{
			// Token: 0x17000A34 RID: 2612
			// (get) Token: 0x06003B50 RID: 15184 RVA: 0x000EDDCF File Offset: 0x000EBFCF
			// (set) Token: 0x06003B51 RID: 15185 RVA: 0x000EDDD7 File Offset: 0x000EBFD7
			public float RequestedTimeSpeed { get; private set; }

			// Token: 0x17000A35 RID: 2613
			// (get) Token: 0x06003B52 RID: 15186 RVA: 0x000EDDE0 File Offset: 0x000EBFE0
			// (set) Token: 0x06003B53 RID: 15187 RVA: 0x000EDDE8 File Offset: 0x000EBFE8
			public int RequestID { get; private set; }

			// Token: 0x06003B54 RID: 15188 RVA: 0x000EDDF1 File Offset: 0x000EBFF1
			public TimeSpeedRequest(float requestedTime, int requestID)
			{
				this.RequestedTimeSpeed = requestedTime;
				this.RequestID = requestID;
			}
		}

		// Token: 0x020004F3 RID: 1267
		public static class MissionNetworkHelper
		{
			// Token: 0x06003B55 RID: 15189 RVA: 0x000EDE04 File Offset: 0x000EC004
			public static Agent GetAgentFromIndex(int agentIndex, bool canBeNull = false)
			{
				Agent agent = Mission.Current.FindAgentWithIndex(agentIndex);
				if (!canBeNull && agent == null && agentIndex >= 0)
				{
					Debug.Print("Agent with index: " + agentIndex + " could not be found while reading reference from packet.", 0, Debug.DebugColor.White, 17592186044416UL);
					throw new MBNotFoundException("Agent with index: " + agentIndex + " could not be found while reading reference from packet.");
				}
				return agent;
			}

			// Token: 0x06003B56 RID: 15190 RVA: 0x000EDE69 File Offset: 0x000EC069
			public static MBTeam GetMBTeamFromTeamIndex(int teamIndex)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (teamIndex < 0)
				{
					return MBTeam.InvalidTeam;
				}
				return new MBTeam(Mission.Current, teamIndex);
			}

			// Token: 0x06003B57 RID: 15191 RVA: 0x000EDE94 File Offset: 0x000EC094
			public static Team GetTeamFromTeamIndex(int teamIndex)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (teamIndex < 0)
				{
					return Team.Invalid;
				}
				MBTeam mbteamFromTeamIndex = Mission.MissionNetworkHelper.GetMBTeamFromTeamIndex(teamIndex);
				return Mission.Current.Teams.Find(mbteamFromTeamIndex);
			}

			// Token: 0x06003B58 RID: 15192 RVA: 0x000EDED4 File Offset: 0x000EC0D4
			public static MissionObject GetMissionObjectFromMissionObjectId(MissionObjectId missionObjectId)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				if (missionObjectId.Id < 0)
				{
					return null;
				}
				MissionObject missionObject = Mission.Current.MissionObjects.FirstOrDefault<MissionObject>((MissionObject mo) => mo.Id == missionObjectId);
				if (missionObject == null)
				{
					MBDebug.Print(string.Concat(new object[]
					{
						"MissionObject with ID: ",
						missionObjectId.Id,
						" runtime: ",
						missionObjectId.CreatedAtRuntime.ToString(),
						" could not be found."
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				return missionObject;
			}

			// Token: 0x06003B59 RID: 15193 RVA: 0x000EDF90 File Offset: 0x000EC190
			public static CombatLogData GetCombatLogDataForCombatLogNetworkMessage(CombatLogNetworkMessage message)
			{
				if (Mission.Current == null)
				{
					throw new Exception("Mission.Current is null!");
				}
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(message.AttackerAgentIndex, false);
				Agent agentFromIndex2 = Mission.MissionNetworkHelper.GetAgentFromIndex(message.VictimAgentIndex, true);
				bool flag = agentFromIndex != null;
				bool flag2 = flag && agentFromIndex.IsHuman;
				bool flag3 = flag && agentFromIndex.IsMine;
				bool flag4 = flag && agentFromIndex.RiderAgent != null;
				bool flag5 = flag4 && agentFromIndex.RiderAgent.IsMine;
				bool flag6 = flag && agentFromIndex.IsMount;
				bool flag7 = agentFromIndex2 != null && agentFromIndex2.Health <= 0f;
				bool flag8 = agentFromIndex != null && ((agentFromIndex2 != null) ? agentFromIndex2.RiderAgent : null) == agentFromIndex;
				bool flag9 = agentFromIndex == agentFromIndex2;
				bool flag10 = flag2;
				bool flag11 = flag3;
				bool flag12 = flag4;
				bool flag13 = flag5;
				bool flag14 = flag6;
				bool flag15 = agentFromIndex2 != null && agentFromIndex2.IsHuman;
				bool flag16 = agentFromIndex2 != null && agentFromIndex2.IsMine;
				bool flag17 = flag7;
				bool flag18 = ((agentFromIndex2 != null) ? agentFromIndex2.RiderAgent : null) != null;
				bool? flag19;
				if (agentFromIndex2 == null)
				{
					flag19 = null;
				}
				else
				{
					Agent riderAgent = agentFromIndex2.RiderAgent;
					flag19 = ((riderAgent != null) ? new bool?(riderAgent.IsMine) : null);
				}
				CombatLogData combatLogData = new CombatLogData(flag9, flag10, flag11, flag12, flag13, flag14, flag15, flag16, flag17, flag18, flag19 ?? false, agentFromIndex2 != null && agentFromIndex2.IsMount, Mission.MissionNetworkHelper.GetMissionObjectFromMissionObjectId(message.MissionObjectHitId), flag8, message.CrushedThrough, message.Chamber, message.Distance);
				combatLogData.DamageType = message.DamageType;
				combatLogData.IsRangedAttack = message.IsRangedAttack;
				combatLogData.IsFriendlyFire = message.IsFriendlyFire;
				combatLogData.IsFatalDamage = message.IsFatalDamage;
				combatLogData.IsSpecialDamage = message.IsSpecialDamage;
				combatLogData.BodyPartHit = message.BodyPartHit;
				combatLogData.HitSpeed = message.HitSpeed;
				combatLogData.InflictedDamage = message.InflictedDamage;
				combatLogData.AbsorbedDamage = message.AbsorbedDamage;
				combatLogData.ModifiedDamage = message.ModifiedDamage;
				combatLogData.ReflectedDamage = message.ReflectedDamage;
				string text;
				if (agentFromIndex2 == null)
				{
					text = null;
				}
				else
				{
					MissionPeer missionPeer = agentFromIndex2.MissionPeer;
					text = ((missionPeer != null) ? missionPeer.DisplayedName : null);
				}
				string text2;
				if ((text2 = text) == null)
				{
					text2 = ((agentFromIndex2 != null) ? agentFromIndex2.Name : null) ?? "";
				}
				combatLogData.VictimAgentName = text2;
				return combatLogData;
			}
		}

		// Token: 0x020004F4 RID: 1268
		public class Missile : MBMissile
		{
			// Token: 0x17000A36 RID: 2614
			// (get) Token: 0x06003B5A RID: 15194 RVA: 0x000EE1BC File Offset: 0x000EC3BC
			// (set) Token: 0x06003B5B RID: 15195 RVA: 0x000EE1C4 File Offset: 0x000EC3C4
			public GameEntity Entity { get; private set; }

			// Token: 0x17000A37 RID: 2615
			// (get) Token: 0x06003B5C RID: 15196 RVA: 0x000EE1CD File Offset: 0x000EC3CD
			// (set) Token: 0x06003B5D RID: 15197 RVA: 0x000EE1D5 File Offset: 0x000EC3D5
			public MissionWeapon Weapon { get; private set; }

			// Token: 0x17000A38 RID: 2616
			// (get) Token: 0x06003B5E RID: 15198 RVA: 0x000EE1DE File Offset: 0x000EC3DE
			// (set) Token: 0x06003B5F RID: 15199 RVA: 0x000EE1E6 File Offset: 0x000EC3E6
			public Agent ShooterAgent { get; private set; }

			// Token: 0x17000A39 RID: 2617
			// (get) Token: 0x06003B60 RID: 15200 RVA: 0x000EE1EF File Offset: 0x000EC3EF
			// (set) Token: 0x06003B61 RID: 15201 RVA: 0x000EE1F7 File Offset: 0x000EC3F7
			public MissionObject MissionObjectToIgnore { get; private set; }

			// Token: 0x17000A3A RID: 2618
			// (get) Token: 0x06003B62 RID: 15202 RVA: 0x000EE200 File Offset: 0x000EC400
			// (set) Token: 0x06003B63 RID: 15203 RVA: 0x000EE208 File Offset: 0x000EC408
			public GameEntity AlreadyHitEntityToIgnore { get; private set; }

			// Token: 0x06003B64 RID: 15204 RVA: 0x000EE211 File Offset: 0x000EC411
			public Missile(Mission mission, int index, GameEntity entity, Agent shooterAgent, MissionWeapon weapon, MissionObject missionObjectToIgnore)
				: base(mission)
			{
				base.Index = index;
				this.Entity = entity;
				this.Weapon = weapon;
				this.ShooterAgent = shooterAgent;
				this.MissionObjectToIgnore = missionObjectToIgnore;
			}

			// Token: 0x06003B65 RID: 15205 RVA: 0x000EE240 File Offset: 0x000EC440
			public void CalculatePassbySoundParametersMT(ref SoundEventParameter soundEventParameter)
			{
				if (this.Weapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield))
				{
					soundEventParameter.Update("impactModifier", 0.3f);
				}
			}

			// Token: 0x06003B66 RID: 15206 RVA: 0x000EE280 File Offset: 0x000EC480
			public void CalculateBounceBackVelocity(Vec3 rotationSpeed, AttackCollisionData collisionData, out Vec3 velocity, out Vec3 angularVelocity)
			{
				Vec3 missileVelocity = collisionData.MissileVelocity;
				float num = (float)this.Weapon.CurrentUsageItem.WeaponLength * 0.01f * this.Weapon.Item.ScaleFactor;
				PhysicsMaterial fromIndex = PhysicsMaterial.GetFromIndex(collisionData.PhysicsMaterialIndex);
				float num2;
				float num3;
				if (fromIndex.IsValid)
				{
					num2 = fromIndex.GetDynamicFriction();
					num3 = fromIndex.GetRestitution();
				}
				else
				{
					num2 = 0.3f;
					num3 = 0.4f;
				}
				PhysicsMaterial fromName = PhysicsMaterial.GetFromName(this.Weapon.Item.PrimaryWeapon.PhysicsMaterial);
				float num4;
				float num5;
				if (fromName.IsValid)
				{
					num4 = fromName.GetDynamicFriction();
					num5 = fromName.GetRestitution();
				}
				else
				{
					num4 = 0.3f;
					num5 = 0.4f;
				}
				float num6 = (num2 + num4) * 0.5f;
				float num7 = (num3 + num5) * 0.5f;
				Vec3 vec = missileVelocity.Reflect(collisionData.CollisionGlobalNormal);
				float num8 = Vec3.DotProduct(vec, collisionData.CollisionGlobalNormal);
				Vec3 vec2 = collisionData.CollisionGlobalNormal;
				Vec3 vec3 = vec2.RotateAboutAnArbitraryVector(Vec3.CrossProduct(vec, collisionData.CollisionGlobalNormal).NormalizedCopy(), 1.5707964f);
				float num9 = Vec3.DotProduct(vec, vec3);
				velocity = collisionData.CollisionGlobalNormal * (num7 * num8) + vec3 * (num9 * num6);
				velocity += collisionData.CollisionGlobalNormal;
				angularVelocity = -Vec3.CrossProduct(collisionData.CollisionGlobalNormal, velocity);
				float lengthSquared = angularVelocity.LengthSquared;
				float weight = this.Weapon.GetWeight();
				WeaponClass weaponClass = this.Weapon.CurrentUsageItem.WeaponClass;
				float num10;
				if (weaponClass == WeaponClass.Arrow || weaponClass == WeaponClass.Bolt)
				{
					num10 = 0.25f * weight * 0.055f * 0.055f + 0.08333333f * weight * num * num;
				}
				else if (weaponClass == WeaponClass.ThrowingKnife)
				{
					num10 = 0.25f * weight * 0.2f * 0.2f + 0.08333333f * weight * num * num;
					num10 += 0.5f * weight * 0.2f * 0.2f;
					rotationSpeed * num3;
					MatrixFrame matrixFrame = this.Entity.GetGlobalFrame();
					vec2 = rotationSpeed * num3;
					angularVelocity = matrixFrame.rotation.TransformToParent(in vec2);
				}
				else if (weaponClass == WeaponClass.ThrowingAxe)
				{
					num10 = 0.25f * weight * 0.2f * 0.2f + 0.08333333f * weight * num * num;
					num10 += 0.5f * weight * 0.2f * 0.2f;
					rotationSpeed * num3;
					MatrixFrame matrixFrame = this.Entity.GetGlobalFrame();
					vec2 = rotationSpeed * num3;
					angularVelocity = matrixFrame.rotation.TransformToParent(in vec2);
				}
				else if (weaponClass == WeaponClass.Javelin)
				{
					num10 = 0.25f * weight * 0.155f * 0.155f + 0.08333333f * weight * num * num;
				}
				else if (weaponClass == WeaponClass.Stone || weaponClass == WeaponClass.BallistaStone || weaponClass == WeaponClass.SlingStone)
				{
					num10 = 0.4f * weight * 0.1f * 0.1f;
				}
				else if (weaponClass == WeaponClass.Boulder || weaponClass == WeaponClass.BallistaBoulder)
				{
					num10 = 0.4f * weight * 0.4f * 0.4f;
				}
				else
				{
					Debug.FailedAssert("Unknown missile type!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Mission.cs", "CalculateBounceBackVelocity", 275);
					num10 = 0f;
				}
				float num11 = 0.5f * num10 * lengthSquared;
				float length = missileVelocity.Length;
				float num12 = MathF.Sqrt((0.5f * weight * length * length - num11) * 2f / weight);
				velocity *= num12 / length;
				float maximumValue = CompressionMission.SpawnedItemVelocityCompressionInfo.GetMaximumValue();
				float maximumValue2 = CompressionMission.SpawnedItemAngularVelocityCompressionInfo.GetMaximumValue();
				if (velocity.LengthSquared > maximumValue * maximumValue)
				{
					velocity = velocity.NormalizedCopy() * maximumValue;
				}
				if (angularVelocity.LengthSquared > maximumValue2 * maximumValue2)
				{
					angularVelocity = angularVelocity.NormalizedCopy() * maximumValue2;
				}
			}

			// Token: 0x06003B67 RID: 15207 RVA: 0x000EE6B0 File Offset: 0x000EC8B0
			public void PassThroughEntity(GameEntity entity)
			{
				this.AlreadyHitEntityToIgnore = entity;
				Vec3 vec = base.GetVelocity() * 0.8f;
				base.SetVelocity(in vec);
			}
		}

		// Token: 0x020004F5 RID: 1269
		public struct SpectatorData
		{
			// Token: 0x17000A3B RID: 2619
			// (get) Token: 0x06003B68 RID: 15208 RVA: 0x000EE6DD File Offset: 0x000EC8DD
			// (set) Token: 0x06003B69 RID: 15209 RVA: 0x000EE6E5 File Offset: 0x000EC8E5
			public Agent AgentToFollow { get; private set; }

			// Token: 0x17000A3C RID: 2620
			// (get) Token: 0x06003B6A RID: 15210 RVA: 0x000EE6EE File Offset: 0x000EC8EE
			// (set) Token: 0x06003B6B RID: 15211 RVA: 0x000EE6F6 File Offset: 0x000EC8F6
			public IAgentVisual AgentVisualToFollow { get; private set; }

			// Token: 0x17000A3D RID: 2621
			// (get) Token: 0x06003B6C RID: 15212 RVA: 0x000EE6FF File Offset: 0x000EC8FF
			// (set) Token: 0x06003B6D RID: 15213 RVA: 0x000EE707 File Offset: 0x000EC907
			public SpectatorCameraTypes CameraType { get; private set; }

			// Token: 0x06003B6E RID: 15214 RVA: 0x000EE710 File Offset: 0x000EC910
			public SpectatorData(Agent agentToFollow, IAgentVisual agentVisualToFollow, SpectatorCameraTypes cameraType)
			{
				this.AgentToFollow = agentToFollow;
				this.CameraType = cameraType;
				this.AgentVisualToFollow = agentVisualToFollow;
			}
		}

		// Token: 0x020004F6 RID: 1270
		private class DynamicEntityInfo
		{
			// Token: 0x04001C88 RID: 7304
			public GameEntity Entity;

			// Token: 0x04001C89 RID: 7305
			public Timer TimerToDisable;
		}

		// Token: 0x020004F7 RID: 1271
		public enum State
		{
			// Token: 0x04001C8B RID: 7307
			NewlyCreated,
			// Token: 0x04001C8C RID: 7308
			Initializing,
			// Token: 0x04001C8D RID: 7309
			Continuing,
			// Token: 0x04001C8E RID: 7310
			EndingNextFrame,
			// Token: 0x04001C8F RID: 7311
			Over
		}

		// Token: 0x020004F8 RID: 1272
		public enum BattleSizeQualifier
		{
			// Token: 0x04001C91 RID: 7313
			Small,
			// Token: 0x04001C92 RID: 7314
			Medium
		}

		// Token: 0x020004F9 RID: 1273
		public enum MissionTeamAITypeEnum
		{
			// Token: 0x04001C94 RID: 7316
			NoTeamAI,
			// Token: 0x04001C95 RID: 7317
			FieldBattle,
			// Token: 0x04001C96 RID: 7318
			Siege,
			// Token: 0x04001C97 RID: 7319
			SallyOut,
			// Token: 0x04001C98 RID: 7320
			NavalBattle,
			// Token: 0x04001C99 RID: 7321
			NavalRaid
		}

		// Token: 0x020004FA RID: 1274
		public enum MissileCollisionReaction
		{
			// Token: 0x04001C9B RID: 7323
			Invalid = -1,
			// Token: 0x04001C9C RID: 7324
			Stick,
			// Token: 0x04001C9D RID: 7325
			PassThrough,
			// Token: 0x04001C9E RID: 7326
			BounceBack,
			// Token: 0x04001C9F RID: 7327
			BecomeInvisible,
			// Token: 0x04001CA0 RID: 7328
			Count
		}

		// Token: 0x020004FB RID: 1275
		public enum MissionTickAction
		{
			// Token: 0x04001CA2 RID: 7330
			TryToSheathWeaponInHand,
			// Token: 0x04001CA3 RID: 7331
			RemoveEquippedWeapon,
			// Token: 0x04001CA4 RID: 7332
			TryToWieldWeaponInSlot,
			// Token: 0x04001CA5 RID: 7333
			DropItem,
			// Token: 0x04001CA6 RID: 7334
			RegisterDrownBlow,
			// Token: 0x04001CA7 RID: 7335
			RegisterBurnBlow
		}

		// Token: 0x020004FC RID: 1276
		// (Invoke) Token: 0x06003B71 RID: 15217
		public delegate void OnBeforeAgentRemovedDelegate(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow);

		// Token: 0x020004FD RID: 1277
		// (Invoke) Token: 0x06003B75 RID: 15221
		public delegate void OnAddSoundAlarmFactorToAgentsDelegate(Agent alarmCreatorAgent, in Vec3 soundPosition, float soundLevelSquareRoot);

		// Token: 0x020004FE RID: 1278
		// (Invoke) Token: 0x06003B79 RID: 15225
		public delegate void OnMainAgentChangedDelegate(Agent oldAgent);

		// Token: 0x020004FF RID: 1279
		// (Invoke) Token: 0x06003B7D RID: 15229
		public delegate void OnCameraShakeTriggeredDelegate(in Vec3 position, float radius);

		// Token: 0x02000500 RID: 1280
		// (Invoke) Token: 0x06003B81 RID: 15233
		public delegate BodyProperties ComputeTroopBodyPropertiesDelegate(AgentBuildData agentBuildData, BasicCharacterObject characterObject, Equipment equipment, int seed);

		// Token: 0x02000501 RID: 1281
		public sealed class TeamCollection : List<Team>
		{
			// Token: 0x140000B0 RID: 176
			// (add) Token: 0x06003B84 RID: 15236 RVA: 0x000EE730 File Offset: 0x000EC930
			// (remove) Token: 0x06003B85 RID: 15237 RVA: 0x000EE768 File Offset: 0x000EC968
			public event Action<Team, Team> OnPlayerTeamChanged;

			// Token: 0x17000A3E RID: 2622
			// (get) Token: 0x06003B86 RID: 15238 RVA: 0x000EE79D File Offset: 0x000EC99D
			// (set) Token: 0x06003B87 RID: 15239 RVA: 0x000EE7A5 File Offset: 0x000EC9A5
			public Team Attacker { get; private set; }

			// Token: 0x17000A3F RID: 2623
			// (get) Token: 0x06003B88 RID: 15240 RVA: 0x000EE7AE File Offset: 0x000EC9AE
			// (set) Token: 0x06003B89 RID: 15241 RVA: 0x000EE7B6 File Offset: 0x000EC9B6
			public Team Defender { get; private set; }

			// Token: 0x17000A40 RID: 2624
			// (get) Token: 0x06003B8A RID: 15242 RVA: 0x000EE7BF File Offset: 0x000EC9BF
			// (set) Token: 0x06003B8B RID: 15243 RVA: 0x000EE7C7 File Offset: 0x000EC9C7
			public Team AttackerAlly { get; private set; }

			// Token: 0x17000A41 RID: 2625
			// (get) Token: 0x06003B8C RID: 15244 RVA: 0x000EE7D0 File Offset: 0x000EC9D0
			// (set) Token: 0x06003B8D RID: 15245 RVA: 0x000EE7D8 File Offset: 0x000EC9D8
			public Team DefenderAlly { get; private set; }

			// Token: 0x17000A42 RID: 2626
			// (get) Token: 0x06003B8E RID: 15246 RVA: 0x000EE7E1 File Offset: 0x000EC9E1
			// (set) Token: 0x06003B8F RID: 15247 RVA: 0x000EE7E9 File Offset: 0x000EC9E9
			public Team Player
			{
				get
				{
					return this._playerTeam;
				}
				set
				{
					if (this._playerTeam != value)
					{
						this.SetPlayerTeamAux((value == null) ? (-1) : base.IndexOf(value));
					}
				}
			}

			// Token: 0x17000A43 RID: 2627
			// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000EE807 File Offset: 0x000ECA07
			// (set) Token: 0x06003B91 RID: 15249 RVA: 0x000EE80F File Offset: 0x000ECA0F
			public Team PlayerEnemy { get; private set; }

			// Token: 0x17000A44 RID: 2628
			// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000EE818 File Offset: 0x000ECA18
			// (set) Token: 0x06003B93 RID: 15251 RVA: 0x000EE820 File Offset: 0x000ECA20
			public Team PlayerAlly { get; private set; }

			// Token: 0x06003B94 RID: 15252 RVA: 0x000EE829 File Offset: 0x000ECA29
			public TeamCollection(Mission mission)
				: base(new List<Team>())
			{
				this._mission = mission;
			}

			// Token: 0x06003B95 RID: 15253 RVA: 0x000EE83D File Offset: 0x000ECA3D
			private MBTeam AddNative()
			{
				return new MBTeam(this._mission, MBAPI.IMBMission.AddTeam(this._mission.Pointer));
			}

			// Token: 0x06003B96 RID: 15254 RVA: 0x000EE85F File Offset: 0x000ECA5F
			public new void Add(Team t)
			{
				MBDebug.ShowWarning("Pre-created Team can not be added to TeamCollection!");
			}

			// Token: 0x06003B97 RID: 15255 RVA: 0x000EE86C File Offset: 0x000ECA6C
			public Team Add(BattleSideEnum side, uint color = 4294967295U, uint color2 = 4294967295U, Banner banner = null, bool isPlayerGeneral = true, bool isPlayerSergeant = false, bool isSettingRelations = true)
			{
				MBDebug.Print("----------Mission-AddTeam-" + side, 0, Debug.DebugColor.White, 17592186044416UL);
				Team team = new Team(this.AddNative(), side, this._mission, color, color2, banner);
				if (!GameNetwork.IsClientOrReplay)
				{
					team.SetPlayerRole(isPlayerGeneral, isPlayerSergeant);
				}
				base.Add(team);
				foreach (MissionBehavior missionBehavior in this._mission.MissionBehaviors)
				{
					missionBehavior.OnAddTeam(team);
				}
				if (isSettingRelations)
				{
					this.SetRelations(team);
				}
				if (side == BattleSideEnum.Attacker)
				{
					if (this.Attacker == null)
					{
						this.Attacker = team;
					}
					else if (this.AttackerAlly == null)
					{
						this.AttackerAlly = team;
					}
				}
				else if (side == BattleSideEnum.Defender)
				{
					if (this.Defender == null)
					{
						this.Defender = team;
					}
					else if (this.DefenderAlly == null)
					{
						this.DefenderAlly = team;
					}
				}
				this.AdjustPlayerTeams();
				foreach (MissionBehavior missionBehavior2 in this._mission.MissionBehaviors)
				{
					missionBehavior2.AfterAddTeam(team);
				}
				return team;
			}

			// Token: 0x06003B98 RID: 15256 RVA: 0x000EE9B4 File Offset: 0x000ECBB4
			public Team Find(MBTeam mbTeam)
			{
				if (mbTeam.IsValid)
				{
					for (int i = 0; i < base.Count; i++)
					{
						Team team = base[i];
						if (team.MBTeam == mbTeam)
						{
							return team;
						}
					}
				}
				return Team.Invalid;
			}

			// Token: 0x06003B99 RID: 15257 RVA: 0x000EE9F8 File Offset: 0x000ECBF8
			public void ClearResources()
			{
				this.Attacker = null;
				this.AttackerAlly = null;
				this.Defender = null;
				this.DefenderAlly = null;
				this._playerTeam = null;
				this.PlayerEnemy = null;
				this.PlayerAlly = null;
				Team.Invalid = null;
			}

			// Token: 0x06003B9A RID: 15258 RVA: 0x000EEA34 File Offset: 0x000ECC34
			public new void Clear()
			{
				foreach (Team team in this)
				{
					team.Clear();
				}
				base.Clear();
				this.ClearResources();
				MBAPI.IMBMission.ResetTeams(this._mission.Pointer);
			}

			// Token: 0x06003B9B RID: 15259 RVA: 0x000EEAA0 File Offset: 0x000ECCA0
			private void SetRelations(Team team)
			{
				BattleSideEnum side = team.Side;
				for (int i = 0; i < base.Count; i++)
				{
					Team team2 = base[i];
					if (side.IsOpponentOf(team2.Side))
					{
						team.SetIsEnemyOf(team2, true);
					}
				}
			}

			// Token: 0x06003B9C RID: 15260 RVA: 0x000EEAE4 File Offset: 0x000ECCE4
			private void SetPlayerTeamAux(int index)
			{
				Team playerTeam = this._playerTeam;
				this._playerTeam = ((index == -1) ? null : base[index]);
				this.AdjustPlayerTeams();
				Action<Team, Team> onPlayerTeamChanged = this.OnPlayerTeamChanged;
				if (onPlayerTeamChanged == null)
				{
					return;
				}
				onPlayerTeamChanged(playerTeam, this._playerTeam);
			}

			// Token: 0x06003B9D RID: 15261 RVA: 0x000EEB2C File Offset: 0x000ECD2C
			private void AdjustPlayerTeams()
			{
				if (this.Player == null)
				{
					this.PlayerEnemy = null;
					this.PlayerAlly = null;
					return;
				}
				if (this.Player != this.Attacker)
				{
					if (this.Player == this.Defender)
					{
						if (this.Attacker != null && this.Player.IsEnemyOf(this.Attacker))
						{
							this.PlayerEnemy = this.Attacker;
						}
						else
						{
							this.PlayerEnemy = null;
						}
						if (this.DefenderAlly != null && this.Player.IsFriendOf(this.DefenderAlly))
						{
							this.PlayerAlly = this.DefenderAlly;
							return;
						}
						this.PlayerAlly = null;
					}
					return;
				}
				if (this.Defender != null && this.Player.IsEnemyOf(this.Defender))
				{
					this.PlayerEnemy = this.Defender;
				}
				else
				{
					this.PlayerEnemy = null;
				}
				if (this.AttackerAlly != null && this.Player.IsFriendOf(this.AttackerAlly))
				{
					this.PlayerAlly = this.AttackerAlly;
					return;
				}
				this.PlayerAlly = null;
			}

			// Token: 0x17000A45 RID: 2629
			// (get) Token: 0x06003B9E RID: 15262 RVA: 0x000EEC2B File Offset: 0x000ECE2B
			private int TeamCountNative
			{
				get
				{
					return MBAPI.IMBMission.GetNumberOfTeams(this._mission.Pointer);
				}
			}

			// Token: 0x04001CA9 RID: 7337
			private Mission _mission;

			// Token: 0x04001CAE RID: 7342
			private Team _playerTeam;
		}
	}
}
