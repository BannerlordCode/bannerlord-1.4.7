using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009C RID: 156
	public struct WeakGameEntity
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0000F95C File Offset: 0x0000DB5C
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x0000F964 File Offset: 0x0000DB64
		public UIntPtr Pointer { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0000F96D File Offset: 0x0000DB6D
		public bool IsValid
		{
			get
			{
				return this.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0000F97F File Offset: 0x0000DB7F
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetName(this.Pointer);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0000F991 File Offset: 0x0000DB91
		public Scene Scene
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetScene(this.Pointer);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0000F9A3 File Offset: 0x0000DBA3
		public EntityFlags EntityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityFlags(this.Pointer);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0000F9B5 File Offset: 0x0000DBB5
		public EntityVisibilityFlags EntityVisibilityFlags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetEntityVisibilityFlags(this.Pointer);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0000F9C7 File Offset: 0x0000DBC7
		public BodyFlags BodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetBodyFlags(this.Pointer);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0000F9D9 File Offset: 0x0000DBD9
		public BodyFlags PhysicsDescBodyFlag
		{
			get
			{
				return (BodyFlags)EngineApplicationInterface.IGameEntity.GetPhysicsDescBodyFlags(this.Pointer);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0000F9EB File Offset: 0x0000DBEB
		public float Mass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetMass(this.Pointer);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0000F9FD File Offset: 0x0000DBFD
		public Vec3 CenterOfMass
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetCenterOfMass(this.Pointer);
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0000FA0F File Offset: 0x0000DC0F
		internal WeakGameEntity(UIntPtr pointer)
		{
			this.Pointer = pointer;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0000FA18 File Offset: 0x0000DC18
		public void Invalidate()
		{
			this.Pointer = (UIntPtr)0UL;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0000FA27 File Offset: 0x0000DC27
		public UIntPtr GetScenePointer()
		{
			return EngineApplicationInterface.IGameEntity.GetScenePointer(this.Pointer);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		public override string ToString()
		{
			return this.Pointer.ToString();
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0000FA57 File Offset: 0x0000DC57
		public void ClearEntityComponents(bool resetAll, bool removeScripts, bool deleteChildEntities)
		{
			EngineApplicationInterface.IGameEntity.ClearEntityComponents(this.Pointer, resetAll, removeScripts, deleteChildEntities);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		public void ClearComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearComponents(this.Pointer);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0000FA7E File Offset: 0x0000DC7E
		public void ClearOnlyOwnComponents()
		{
			EngineApplicationInterface.IGameEntity.ClearOnlyOwnComponents(this.Pointer);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0000FA90 File Offset: 0x0000DC90
		public bool CheckResources(bool addToQueue, bool checkFaceResources)
		{
			return EngineApplicationInterface.IGameEntity.CheckResources(this.Pointer, addToQueue, checkFaceResources);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		public void SetMobility(GameEntity.Mobility mobility)
		{
			EngineApplicationInterface.IGameEntity.SetMobility(this.Pointer, mobility);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0000FAB7 File Offset: 0x0000DCB7
		public GameEntity.Mobility GetMobility()
		{
			return EngineApplicationInterface.IGameEntity.GetMobility(this.Pointer);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0000FAC9 File Offset: 0x0000DCC9
		public void AddMesh(Mesh mesh, bool recomputeBoundingBox = true)
		{
			EngineApplicationInterface.IGameEntity.AddMesh(this.Pointer, mesh.Pointer, recomputeBoundingBox);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0000FAE2 File Offset: 0x0000DCE2
		public void AddMultiMeshToSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeleton(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0000FAFA File Offset: 0x0000DCFA
		public void AddMultiMeshToSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMeshToSkeletonBone(this.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0000FB13 File Offset: 0x0000DD13
		public void SetColorToAllMeshesWithTagRecursive(uint color, string tag)
		{
			EngineApplicationInterface.IGameEntity.SetColorToAllMeshesWithTagRecursive(this.Pointer, color, tag);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0000FB27 File Offset: 0x0000DD27
		public IEnumerable<Mesh> GetAllMeshesWithTag(string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.GetChildrenRecursive(ref list);
			list.Add(ref this);
			foreach (WeakGameEntity entity in list)
			{
				int num;
				for (int i = 0; i < entity.MultiMeshComponentCount; i = num + 1)
				{
					MetaMesh multiMesh = entity.GetMetaMesh(i);
					for (int j = 0; j < multiMesh.MeshCount; j = num + 1)
					{
						Mesh meshAtIndex = multiMesh.GetMeshAtIndex(j);
						if (meshAtIndex.HasTag(tag))
						{
							yield return meshAtIndex;
						}
						num = j;
					}
					multiMesh = null;
					num = i;
				}
				for (int i = 0; i < entity.ClothSimulatorComponentCount; i = num + 1)
				{
					ClothSimulatorComponent clothSimulator = entity.GetClothSimulator(i);
					MetaMesh multiMesh = clothSimulator.GetFirstMetaMesh();
					for (int j = 0; j < multiMesh.MeshCount; j = num + 1)
					{
						Mesh meshAtIndex2 = multiMesh.GetMeshAtIndex(j);
						if (meshAtIndex2.HasTag(tag))
						{
							yield return meshAtIndex2;
						}
						num = j;
					}
					multiMesh = null;
					num = i;
				}
			}
			List<WeakGameEntity>.Enumerator enumerator = default(List<WeakGameEntity>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0000FB43 File Offset: 0x0000DD43
		public void SetName(string name)
		{
			EngineApplicationInterface.IGameEntity.SetName(this.Pointer, name);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0000FB56 File Offset: 0x0000DD56
		public void SetEntityFlags(EntityFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetEntityFlags(this.Pointer, flags);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0000FB69 File Offset: 0x0000DD69
		public void SetEntityVisibilityFlags(EntityVisibilityFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetEntityVisibilityFlags(this.Pointer, flags);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		public PhysicsMaterial GetPhysicsMaterial()
		{
			return PhysicsMaterial.GetFromIndex(EngineApplicationInterface.IGameEntity.GetPhysicsMaterialIndex(this.Pointer));
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0000FB93 File Offset: 0x0000DD93
		public void SetBodyFlags(BodyFlags flags)
		{
			EngineApplicationInterface.IGameEntity.SetBodyFlags(this.Pointer, (uint)flags);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0000FBA6 File Offset: 0x0000DDA6
		public void SetBodyFlagsRecursive(BodyFlags bodyFlags)
		{
			EngineApplicationInterface.IGameEntity.SetBodyFlagsRecursive(this.Pointer, (uint)bodyFlags);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		public void AddBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.SetBodyFlags(this.BodyFlag | bodyFlags);
			if (applyToChildren)
			{
				foreach (WeakGameEntity weakGameEntity in this.GetChildren())
				{
					weakGameEntity.AddBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		internal static WeakGameEntity GetFirstEntityWithTag(Scene scene, string tag)
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetFirstEntityWithTag(scene.Pointer, tag));
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0000FC34 File Offset: 0x0000DE34
		internal static WeakGameEntity GetNextEntityWithTag(Scene scene, WeakGameEntity startEntity, string tag)
		{
			if (!(startEntity == null))
			{
				return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetNextEntityWithTag(startEntity.Pointer, tag));
			}
			return WeakGameEntity.GetFirstEntityWithTag(scene, tag);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0000FC5E File Offset: 0x0000DE5E
		internal static WeakGameEntity GetFirstEntityWithTagExpression(Scene scene, string tagExpression)
		{
			return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetFirstEntityWithTagExpression(scene.Pointer, tagExpression));
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0000FC78 File Offset: 0x0000DE78
		internal static WeakGameEntity GetNextEntityWithTagExpression(Scene scene, WeakGameEntity startEntity, string tagExpression)
		{
			if (startEntity == null)
			{
				return WeakGameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			}
			UIntPtr nextEntityWithTagExpression = EngineApplicationInterface.IGameEntity.GetNextEntityWithTagExpression(startEntity.Pointer, tagExpression);
			if (nextEntityWithTagExpression != UIntPtr.Zero)
			{
				return new WeakGameEntity(nextEntityWithTagExpression);
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0000FCC2 File Offset: 0x0000DEC2
		internal static IEnumerable<WeakGameEntity> GetEntitiesWithTag(Scene scene, string tag)
		{
			WeakGameEntity entity = WeakGameEntity.GetFirstEntityWithTag(scene, tag);
			while (entity != null)
			{
				yield return entity;
				entity = WeakGameEntity.GetNextEntityWithTag(scene, entity, tag);
			}
			yield break;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0000FCD9 File Offset: 0x0000DED9
		internal static IEnumerable<WeakGameEntity> GetEntitiesWithTagExpression(Scene scene, string tagExpression)
		{
			WeakGameEntity entity = WeakGameEntity.GetFirstEntityWithTagExpression(scene, tagExpression);
			while (entity != null)
			{
				yield return entity;
				entity = WeakGameEntity.GetNextEntityWithTagExpression(scene, entity, tagExpression);
			}
			yield break;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0000FCF0 File Offset: 0x0000DEF0
		public void RemoveBodyFlags(BodyFlags bodyFlags, bool applyToChildren = true)
		{
			this.SetBodyFlags(this.BodyFlag & ~bodyFlags);
			if (applyToChildren)
			{
				foreach (WeakGameEntity weakGameEntity in this.GetChildren())
				{
					weakGameEntity.RemoveBodyFlags(bodyFlags, true);
				}
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0000FD54 File Offset: 0x0000DF54
		public void SetLocalPosition(Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.SetLocalPosition(this.Pointer, position);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0000FD67 File Offset: 0x0000DF67
		public void SetGlobalPosition(Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.SetGlobalPosition(this.Pointer, in position);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0000FD7C File Offset: 0x0000DF7C
		public void SetColor(uint color1, uint color2, string meshTag)
		{
			foreach (Mesh mesh in this.GetAllMeshesWithTag(meshTag))
			{
				mesh.Color = color1;
				mesh.Color2 = color2;
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
		public uint GetFactorColor()
		{
			return EngineApplicationInterface.IGameEntity.GetFactorColor(this.Pointer);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0000FDE2 File Offset: 0x0000DFE2
		public void SetFactorColor(uint color)
		{
			EngineApplicationInterface.IGameEntity.SetFactorColor(this.Pointer, color);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0000FDF5 File Offset: 0x0000DFF5
		public void SetAsReplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsReplayEntity(this.Pointer);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0000FE07 File Offset: 0x0000E007
		public void SetClothMaxDistanceMultiplier(float multiplier)
		{
			EngineApplicationInterface.IGameEntity.SetClothMaxDistanceMultiplier(this.Pointer, multiplier);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0000FE1A File Offset: 0x0000E01A
		public void RemoveMultiMeshFromSkeleton(MetaMesh metaMesh)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeleton(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0000FE32 File Offset: 0x0000E032
		public void RemoveMultiMeshFromSkeletonBone(MetaMesh metaMesh, sbyte boneIndex)
		{
			EngineApplicationInterface.IGameEntity.RemoveMultiMeshFromSkeletonBone(this.Pointer, metaMesh.Pointer, boneIndex);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0000FE4B File Offset: 0x0000E04B
		public bool RemoveComponentWithMesh(Mesh mesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponentWithMesh(this.Pointer, mesh.Pointer);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0000FE63 File Offset: 0x0000E063
		public void AddComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.IGameEntity.AddComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0000FE7B File Offset: 0x0000E07B
		public bool HasComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.HasComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0000FE93 File Offset: 0x0000E093
		public bool IsInEditorScene()
		{
			return false;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0000FE96 File Offset: 0x0000E096
		public bool RemoveComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.IGameEntity.RemoveComponent(this.Pointer, component.Pointer);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0000FEAE File Offset: 0x0000E0AE
		public string GetGuid()
		{
			return EngineApplicationInterface.IGameEntity.GetGuid(this.Pointer);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0000FEC0 File Offset: 0x0000E0C0
		public bool IsGuidValid()
		{
			return EngineApplicationInterface.IGameEntity.IsGuidValid(this.Pointer);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0000FED2 File Offset: 0x0000E0D2
		public void SetEnforcedMaximumLodLevel(int lodLevel)
		{
			EngineApplicationInterface.IGameEntity.SetEnforcedMaximumLodLevel(this.Pointer, lodLevel);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0000FEE5 File Offset: 0x0000E0E5
		public float GetLodLevelForDistanceSq(float distSq)
		{
			return EngineApplicationInterface.IGameEntity.GetLodLevelForDistanceSq(this.Pointer, distSq);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		public void GetQuickBoneEntitialFrame(sbyte index, out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetQuickBoneEntitialFrame(this.Pointer, index, out frame);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0000FF0C File Offset: 0x0000E10C
		public void UpdateVisibilityMask()
		{
			EngineApplicationInterface.IGameEntity.UpdateVisibilityMask(this.Pointer);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0000FF1E File Offset: 0x0000E11E
		public void CallScriptCallbacks(bool registerScriptComponents)
		{
			EngineApplicationInterface.IGameEntity.CallScriptCallbacks(this.Pointer, registerScriptComponents);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0000FF31 File Offset: 0x0000E131
		public int GetScriptCount()
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentCount(this.Pointer);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0000FF43 File Offset: 0x0000E143
		public bool IsGhostObject()
		{
			return EngineApplicationInterface.IGameEntity.IsGhostObject(this.Pointer);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0000FF55 File Offset: 0x0000E155
		public void CreateAndAddScriptComponent(string name, bool callScriptCallbacks)
		{
			EngineApplicationInterface.IGameEntity.CreateAndAddScriptComponent(this.Pointer, name, callScriptCallbacks);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0000FF69 File Offset: 0x0000E169
		public void RemoveScriptComponent(UIntPtr scriptComponent, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveScriptComponent(this.Pointer, scriptComponent, removeReason);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0000FF7D File Offset: 0x0000E17D
		public void SetEntityEnvMapVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetEntityEnvMapVisibility(this.Pointer, value);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0000FF90 File Offset: 0x0000E190
		public ScriptComponentBehavior GetScriptAtIndex(int index)
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentAtIndex(this.Pointer, index);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0000FFA3 File Offset: 0x0000E1A3
		internal int GetScriptComponentIndex(uint nameHash)
		{
			return EngineApplicationInterface.IGameEntity.GetScriptComponentIndex(this.Pointer, nameHash);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
		public bool HasScene()
		{
			return EngineApplicationInterface.IGameEntity.HasScene(this.Pointer);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		public bool HasScriptComponent(string scName)
		{
			return EngineApplicationInterface.IGameEntity.HasScriptComponent(this.Pointer, scName);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0000FFDB File Offset: 0x0000E1DB
		public bool HasScriptComponent(uint scNameHash)
		{
			return EngineApplicationInterface.IGameEntity.HasScriptComponentHash(this.Pointer, scNameHash);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0000FFEE File Offset: 0x0000E1EE
		public IEnumerable<ScriptComponentBehavior> GetScriptComponents()
		{
			int count = this.GetScriptCount();
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetScriptAtIndex(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00010003 File Offset: 0x0000E203
		public IEnumerable<T> GetScriptComponents<T>() where T : ScriptComponentBehavior
		{
			int count = this.GetScriptCount();
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00010018 File Offset: 0x0000E218
		public bool HasScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0001004C File Offset: 0x0000E24C
		public bool HasScriptWithInterfaceOfType<T>()
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00010080 File Offset: 0x0000E280
		public T GetFirstScriptOfTypeInFamily<T>() where T : ScriptComponentBehavior
		{
			T t = this.GetFirstScriptOfType<T>();
			WeakGameEntity weakGameEntity = this;
			while (t == null)
			{
				WeakGameEntity parent = weakGameEntity.Parent;
				if (!parent.IsValid)
				{
					break;
				}
				weakGameEntity = parent;
				t = weakGameEntity.GetFirstScriptOfType<T>();
			}
			return t;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000100C4 File Offset: 0x0000E2C4
		public ScriptComponentBehavior GetFirstScriptWithNameHash(uint nameHash)
		{
			int scriptComponentIndex = this.GetScriptComponentIndex(nameHash);
			if (scriptComponentIndex != -1)
			{
				return this.GetScriptAtIndex(scriptComponentIndex);
			}
			return null;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000100E8 File Offset: 0x0000E2E8
		public T GetFirstScriptOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00010130 File Offset: 0x0000E330
		public T GetFirstScriptWithInterfaceOfType<T>() where T : class
		{
			int scriptCount = this.GetScriptCount();
			for (int i = 0; i < scriptCount; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00010178 File Offset: 0x0000E378
		public T GetFirstScriptOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.GetScriptCount();
			for (int i = 0; i < num; i++)
			{
				T t;
				if ((t = this.GetScriptAtIndex(i) as T) != null)
				{
					return t;
				}
			}
			num = this.ChildCount;
			for (int j = 0; j < num; j++)
			{
				T firstScriptOfTypeRecursive = this.GetChild(j).GetFirstScriptOfTypeRecursive<T>();
				if (firstScriptOfTypeRecursive != null)
				{
					return firstScriptOfTypeRecursive;
				}
			}
			return default(T);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000101F0 File Offset: 0x0000E3F0
		public WeakGameEntity GetFirstChildEntityWithTag(string tag)
		{
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					return weakGameEntity;
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0001024C File Offset: 0x0000E44C
		public int GetScriptCountOfType<T>() where T : ScriptComponentBehavior
		{
			int scriptCount = this.GetScriptCount();
			int num = 0;
			for (int i = 0; i < scriptCount; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00010284 File Offset: 0x0000E484
		public int GetScriptCountOfTypeRecursive<T>() where T : ScriptComponentBehavior
		{
			int num = this.GetScriptCount();
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (this.GetScriptAtIndex(i) is T)
				{
					num2++;
				}
			}
			num = this.ChildCount;
			for (int j = 0; j < num; j++)
			{
				num2 += this.GetChild(j).GetScriptCountOfTypeRecursive<T>();
			}
			return num2;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000102DF File Offset: 0x0000E4DF
		internal static GameEntity GetFirstEntityWithName(Scene scene, string entityName)
		{
			return EngineApplicationInterface.IGameEntity.FindWithName(scene.Pointer, entityName);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000102F2 File Offset: 0x0000E4F2
		public void SetAlpha(float alpha)
		{
			EngineApplicationInterface.IGameEntity.SetAlpha(this.Pointer, alpha);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00010305 File Offset: 0x0000E505
		public void SetVisibilityExcludeParents(bool visible)
		{
			EngineApplicationInterface.IGameEntity.SetVisibilityExcludeParents(this.Pointer, visible);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00010318 File Offset: 0x0000E518
		public void SetReadyToRender(bool ready)
		{
			EngineApplicationInterface.IGameEntity.SetReadyToRender(this.Pointer, ready);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0001032B File Offset: 0x0000E52B
		public bool GetVisibilityExcludeParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityExcludeParents(this.Pointer);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0001033D File Offset: 0x0000E53D
		public bool IsVisibleIncludeParents()
		{
			return EngineApplicationInterface.IGameEntity.IsVisibleIncludeParents(this.Pointer);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0001034F File Offset: 0x0000E54F
		public uint GetVisibilityLevelMaskIncludingParents()
		{
			return EngineApplicationInterface.IGameEntity.GetVisibilityLevelMaskIncludingParents(this.Pointer);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00010361 File Offset: 0x0000E561
		public bool GetEditModeLevelVisibility()
		{
			return EngineApplicationInterface.IGameEntity.GetEditModeLevelVisibility(this.Pointer);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00010373 File Offset: 0x0000E573
		public void Remove(int removeReason)
		{
			EngineApplicationInterface.IGameEntity.Remove(this.Pointer, removeReason);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00010386 File Offset: 0x0000E586
		public void SetUpgradeLevelMask(GameEntity.UpgradeLevelMask mask)
		{
			EngineApplicationInterface.IGameEntity.SetUpgradeLevelMask(this.Pointer, (uint)mask);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00010399 File Offset: 0x0000E599
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMask()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMask(this.Pointer);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000103AB File Offset: 0x0000E5AB
		public GameEntity.UpgradeLevelMask GetUpgradeLevelMaskCumulative()
		{
			return (GameEntity.UpgradeLevelMask)EngineApplicationInterface.IGameEntity.GetUpgradeLevelMaskCumulative(this.Pointer);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000103C0 File Offset: 0x0000E5C0
		public int GetUpgradeLevelOfEntity()
		{
			int upgradeLevelMask = (int)this.GetUpgradeLevelMask();
			if ((upgradeLevelMask & 1) > 0)
			{
				return 0;
			}
			if ((upgradeLevelMask & 2) > 0)
			{
				return 1;
			}
			if ((upgradeLevelMask & 4) > 0)
			{
				return 2;
			}
			if ((upgradeLevelMask & 8) > 0)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000103F5 File Offset: 0x0000E5F5
		public string GetOldPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetOldPrefabName(this.Pointer);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00010407 File Offset: 0x0000E607
		public string GetPrefabName()
		{
			return EngineApplicationInterface.IGameEntity.GetPrefabName(this.Pointer);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00010419 File Offset: 0x0000E619
		public void RefreshMeshesToRenderToHullWater(UIntPtr visualRecord, string entityTag)
		{
			EngineApplicationInterface.IGameEntity.RefreshMeshesToRenderToHullWater(this.Pointer, visualRecord, entityTag);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0001042D File Offset: 0x0000E62D
		public void DeRegisterWaterMeshMaterials(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.DeRegisterWaterMeshMaterials(this.Pointer, visualRecord);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00010440 File Offset: 0x0000E640
		public void SetVisualRecordWakeParams(UIntPtr visualRecord, Vec3 wakeParams)
		{
			EngineApplicationInterface.IGameEntity.SetVisualRecordWakeParams(visualRecord, in wakeParams);
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0001044F File Offset: 0x0000E64F
		public void ChangeResolutionMultiplierOfWaterVisual(UIntPtr visualRecord, float multiplier, in Vec3 waterEffectsBB)
		{
			EngineApplicationInterface.IGameEntity.ChangeResolutionMultiplierOfWaterVisual(visualRecord, multiplier, in waterEffectsBB);
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0001045E File Offset: 0x0000E65E
		public void ResetHullWater(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.ResetHullWater(visualRecord);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0001046B File Offset: 0x0000E66B
		public void SetWaterVisualRecordFrameAndDt(UIntPtr visualRecord, MatrixFrame frame, float dt)
		{
			EngineApplicationInterface.IGameEntity.SetWaterVisualRecordFrameAndDt(this.Pointer, visualRecord, in frame, dt);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00010481 File Offset: 0x0000E681
		public void AddSplashPositionToWaterVisualRecord(UIntPtr visualRecord, Vec3 position)
		{
			EngineApplicationInterface.IGameEntity.AddSplashPositionToWaterVisualRecord(this.Pointer, visualRecord, in position);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00010496 File Offset: 0x0000E696
		public void UpdateHullWaterEffectFrames(UIntPtr visualRecord)
		{
			EngineApplicationInterface.IGameEntity.UpdateHullWaterEffectFrames(this.Pointer, visualRecord);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000104A9 File Offset: 0x0000E6A9
		public void CopyScriptComponentFromAnotherEntity(GameEntity otherEntity, string scriptName)
		{
			EngineApplicationInterface.IGameEntity.CopyScriptComponentFromAnotherEntity(this.Pointer, otherEntity.Pointer, scriptName);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000104C2 File Offset: 0x0000E6C2
		public void SetFrame(ref MatrixFrame frame, bool isTeleportation = true)
		{
			EngineApplicationInterface.IGameEntity.SetLocalFrame(this.Pointer, ref frame, isTeleportation);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000104D6 File Offset: 0x0000E6D6
		public void SetLocalFrame(ref MatrixFrame frame, bool isTeleportation)
		{
			EngineApplicationInterface.IGameEntity.SetLocalFrame(this.Pointer, ref frame, isTeleportation);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000104EA File Offset: 0x0000E6EA
		public void SetClothComponentKeepState(MetaMesh metaMesh, bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepState(this.Pointer, metaMesh.Pointer, state);
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00010503 File Offset: 0x0000E703
		public void SetClothComponentKeepStateOfAllMeshes(bool state)
		{
			EngineApplicationInterface.IGameEntity.SetClothComponentKeepStateOfAllMeshes(this.Pointer, state);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00010516 File Offset: 0x0000E716
		public void SetPreviousFrameInvalid()
		{
			EngineApplicationInterface.IGameEntity.SetPreviousFrameInvalid(this.Pointer);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00010528 File Offset: 0x0000E728
		public MatrixFrame GetFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00010548 File Offset: 0x0000E748
		public void GetLocalFrame(out MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out frame);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0001055B File Offset: 0x0000E75B
		public bool HasBatchedKinematicPhysicsFlag()
		{
			return EngineApplicationInterface.IGameEntity.HasBatchedKinematicPhysicsFlag(this.Pointer);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0001056D File Offset: 0x0000E76D
		public bool HasBatchedRayCastPhysicsFlag()
		{
			return EngineApplicationInterface.IGameEntity.HasBatchedRayCastPhysicsFlag(this.Pointer);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00010580 File Offset: 0x0000E780
		public MatrixFrame GetLocalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetLocalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000105A0 File Offset: 0x0000E7A0
		public MatrixFrame GetGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000105C0 File Offset: 0x0000E7C0
		public void SetWaterSDFClipData(int slotIndex, in MatrixFrame frame, bool visibility)
		{
			EngineApplicationInterface.IGameEntity.SetWaterSDFClipData(this.Pointer, slotIndex, in frame, visibility);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000105D5 File Offset: 0x0000E7D5
		public int RegisterWaterSDFClip(Texture sdfTexture)
		{
			return EngineApplicationInterface.IGameEntity.RegisterWaterSDFClip(this.Pointer, (sdfTexture != null) ? sdfTexture.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000105FD File Offset: 0x0000E7FD
		public void DeRegisterWaterSDFClip(int slot)
		{
			EngineApplicationInterface.IGameEntity.DeRegisterWaterSDFClip(this.Pointer, slot);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00010610 File Offset: 0x0000E810
		public MatrixFrame GetGlobalFrameImpreciseForFixedTick()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetGlobalFrameImpreciseForFixedTick(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00010630 File Offset: 0x0000E830
		public MatrixFrame ComputePreciseGlobalFrameForFixedTickSlow()
		{
			MatrixFrame matrixFrame = this.GetLocalFrame();
			WeakGameEntity weakGameEntity = this.Parent;
			while (weakGameEntity.Parent != null)
			{
				matrixFrame = weakGameEntity.GetLocalFrame().TransformToParent(in matrixFrame);
				weakGameEntity = weakGameEntity.Parent;
			}
			matrixFrame = weakGameEntity.GetBodyWorldTransform().TransformToParent(in matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00010689 File Offset: 0x0000E889
		public void SetGlobalFrame(in MatrixFrame frame, bool isTeleportation = true)
		{
			EngineApplicationInterface.IGameEntity.SetGlobalFrame(this.Pointer, in frame, isTeleportation);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000106A0 File Offset: 0x0000E8A0
		public MatrixFrame GetPreviousGlobalFrame()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetPreviousGlobalFrame(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000106C0 File Offset: 0x0000E8C0
		public MatrixFrame GetBodyWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyWorldTransform(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000106E0 File Offset: 0x0000E8E0
		public MatrixFrame GetBodyVisualWorldTransform()
		{
			MatrixFrame matrixFrame;
			EngineApplicationInterface.IGameEntity.GetBodyVisualWorldTransform(this.Pointer, out matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00010700 File Offset: 0x0000E900
		public void UpdateTriadFrameForEditor()
		{
			EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(this.Pointer);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00010714 File Offset: 0x0000E914
		public void UpdateTriadFrameForEditorForAllChildren()
		{
			this.UpdateTriadFrameForEditor();
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			this.GetChildrenRecursive(ref list);
			foreach (WeakGameEntity weakGameEntity in list)
			{
				EngineApplicationInterface.IGameEntity.UpdateTriadFrameForEditor(weakGameEntity.Pointer);
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00010780 File Offset: 0x0000E980
		public Vec3 GetGlobalScale()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalScale(this.Pointer);
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00010794 File Offset: 0x0000E994
		public Vec3 GetLocalScale()
		{
			return this.GetFrame().rotation.GetScaleVector();
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x000107B4 File Offset: 0x0000E9B4
		public Vec3 GlobalPosition
		{
			get
			{
				return this.GetGlobalFrame().origin;
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000107C4 File Offset: 0x0000E9C4
		public void SetAnimationSoundActivation(bool activate)
		{
			EngineApplicationInterface.IGameEntity.SetAnimationSoundActivation(this.Pointer, activate);
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				weakGameEntity.SetAnimationSoundActivation(activate);
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00010824 File Offset: 0x0000EA24
		public void CopyComponentsToSkeleton()
		{
			EngineApplicationInterface.IGameEntity.CopyComponentsToSkeleton(this.Pointer);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00010836 File Offset: 0x0000EA36
		public void AddMeshToBone(sbyte boneIndex, Mesh mesh)
		{
			EngineApplicationInterface.IGameEntity.AddMeshToBone(this.Pointer, mesh.Pointer, boneIndex);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0001084F File Offset: 0x0000EA4F
		public void ActivateRagdoll()
		{
			EngineApplicationInterface.IGameEntity.ActivateRagdoll(this.Pointer);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00010861 File Offset: 0x0000EA61
		public void PauseSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(this.Pointer, true);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00010874 File Offset: 0x0000EA74
		public void ResumeSkeletonAnimation()
		{
			EngineApplicationInterface.IGameEntity.Freeze(this.Pointer, false);
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00010887 File Offset: 0x0000EA87
		public bool IsSkeletonAnimationPaused()
		{
			return EngineApplicationInterface.IGameEntity.IsFrozen(this.Pointer);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00010899 File Offset: 0x0000EA99
		public sbyte GetBoneCount()
		{
			return EngineApplicationInterface.IGameEntity.GetBoneCount(this.Pointer);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000108AB File Offset: 0x0000EAAB
		public float GetWaterLevelAtPosition(Vec2 position, bool useWaterRenderer, bool checkWaterBodyEntities)
		{
			return EngineApplicationInterface.IGameEntity.GetWaterLevelAtPosition(this.Pointer, in position, useWaterRenderer, checkWaterBodyEntities);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000108C4 File Offset: 0x0000EAC4
		public MatrixFrame GetBoneEntitialFrameWithIndex(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithIndex(this.Pointer, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000108F0 File Offset: 0x0000EAF0
		public MatrixFrame GetBoneEntitialFrameWithName(string boneName)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.IGameEntity.GetBoneEntitialFrameWithName(this.Pointer, boneName, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x00010919 File Offset: 0x0000EB19
		public string[] Tags
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetTags(this.Pointer).Split(new char[] { ' ' });
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0001093B File Offset: 0x0000EB3B
		public void AddTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.AddTag(this.Pointer, tag);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0001094E File Offset: 0x0000EB4E
		public void RemoveTag(string tag)
		{
			EngineApplicationInterface.IGameEntity.RemoveTag(this.Pointer, tag);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00010961 File Offset: 0x0000EB61
		public bool HasTag(string tag)
		{
			return EngineApplicationInterface.IGameEntity.HasTag(this.Pointer, tag);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00010974 File Offset: 0x0000EB74
		public void AddChild(WeakGameEntity gameEntity, bool autoLocalizeFrame = false)
		{
			EngineApplicationInterface.IGameEntity.AddChild(this.Pointer, gameEntity.Pointer, autoLocalizeFrame);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0001098E File Offset: 0x0000EB8E
		public void RemoveChild(WeakGameEntity childEntity, bool keepPhysics, bool keepScenePointer, bool callScriptCallbacks, int removeReason)
		{
			EngineApplicationInterface.IGameEntity.RemoveChild(this.Pointer, childEntity.Pointer, keepPhysics, keepScenePointer, callScriptCallbacks, removeReason);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000109AD File Offset: 0x0000EBAD
		public void BreakPrefab()
		{
			EngineApplicationInterface.IGameEntity.BreakPrefab(this.Pointer);
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x000109BF File Offset: 0x0000EBBF
		public int ChildCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetChildCount(this.Pointer);
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x000109D4 File Offset: 0x0000EBD4
		public WeakGameEntity GetChild(int index)
		{
			UIntPtr childPointer = EngineApplicationInterface.IGameEntity.GetChildPointer(this.Pointer, index);
			if (!(childPointer != UIntPtr.Zero))
			{
				return new WeakGameEntity(UIntPtr.Zero);
			}
			return new WeakGameEntity(childPointer);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00010A14 File Offset: 0x0000EC14
		public WeakGameEntity Parent
		{
			get
			{
				UIntPtr parentPointer = EngineApplicationInterface.IGameEntity.GetParentPointer(this.Pointer);
				if (!(parentPointer != UIntPtr.Zero))
				{
					return new WeakGameEntity(UIntPtr.Zero);
				}
				return new WeakGameEntity(parentPointer);
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00010A50 File Offset: 0x0000EC50
		public bool HasComplexAnimTree()
		{
			return EngineApplicationInterface.IGameEntity.HasComplexAnimTree(this.Pointer);
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00010A62 File Offset: 0x0000EC62
		public WeakGameEntity Root
		{
			get
			{
				return new WeakGameEntity(EngineApplicationInterface.IGameEntity.GetRootParentPointer(this.Pointer));
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00010A79 File Offset: 0x0000EC79
		public void AddMultiMesh(MetaMesh metaMesh, bool updateVisMask = true)
		{
			EngineApplicationInterface.IGameEntity.AddMultiMesh(this.Pointer, metaMesh.Pointer, updateVisMask);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00010A92 File Offset: 0x0000EC92
		public bool RemoveMultiMesh(MetaMesh metaMesh)
		{
			return EngineApplicationInterface.IGameEntity.RemoveMultiMesh(this.Pointer, metaMesh.Pointer);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00010AAA File Offset: 0x0000ECAA
		public int MultiMeshComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, GameEntity.ComponentType.MetaMesh);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00010ABD File Offset: 0x0000ECBD
		public int ClothSimulatorComponentCount
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, GameEntity.ComponentType.ClothSimulator);
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		public int GetComponentCount(GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentCount(this.Pointer, componentType);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00010AE3 File Offset: 0x0000ECE3
		public void AddAllMeshesOfGameEntity(GameEntity gameEntity)
		{
			EngineApplicationInterface.IGameEntity.AddAllMeshesOfGameEntity(this.Pointer, gameEntity.Pointer);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00010AFB File Offset: 0x0000ECFB
		public void SetFrameChanged()
		{
			EngineApplicationInterface.IGameEntity.SetFrameChanged(this.Pointer);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00010B0D File Offset: 0x0000ED0D
		public GameEntityComponent GetComponentAtIndex(int index, GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, componentType, index);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00010B21 File Offset: 0x0000ED21
		public MetaMesh GetMetaMesh(int metaMeshIndex)
		{
			return (MetaMesh)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, GameEntity.ComponentType.MetaMesh, metaMeshIndex);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00010B3A File Offset: 0x0000ED3A
		public ClothSimulatorComponent GetClothSimulator(int clothSimulatorIndex)
		{
			return (ClothSimulatorComponent)EngineApplicationInterface.IGameEntity.GetComponentAtIndex(this.Pointer, GameEntity.ComponentType.ClothSimulator, clothSimulatorIndex);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00010B53 File Offset: 0x0000ED53
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IGameEntity.SetVectorArgument(this.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public void SetMaterialForAllMeshes(Material material)
		{
			EngineApplicationInterface.IGameEntity.SetMaterialForAllMeshes(this.Pointer, material.Pointer);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00010B82 File Offset: 0x0000ED82
		public bool AddLight(Light light)
		{
			return EngineApplicationInterface.IGameEntity.AddLight(this.Pointer, light.Pointer);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00010B9A File Offset: 0x0000ED9A
		public Light GetLight()
		{
			return EngineApplicationInterface.IGameEntity.GetLight(this.Pointer);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00010BAC File Offset: 0x0000EDAC
		public void AddParticleSystemComponent(string particleid)
		{
			EngineApplicationInterface.IGameEntity.AddParticleSystemComponent(this.Pointer, particleid);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00010BBF File Offset: 0x0000EDBF
		public void RemoveAllParticleSystems()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllParticleSystems(this.Pointer);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00010BD1 File Offset: 0x0000EDD1
		public bool CheckPointWithOrientedBoundingBox(Vec3 point)
		{
			return EngineApplicationInterface.IGameEntity.CheckPointWithOrientedBoundingBox(this.Pointer, point);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		public void PauseParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.PauseParticleSystem(this.Pointer, doChildren);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00010BF7 File Offset: 0x0000EDF7
		public void ResumeParticleSystem(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.ResumeParticleSystem(this.Pointer, doChildren);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00010C0A File Offset: 0x0000EE0A
		public void BurstEntityParticle(bool doChildren)
		{
			EngineApplicationInterface.IGameEntity.BurstEntityParticle(this.Pointer, doChildren);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00010C1D File Offset: 0x0000EE1D
		public void SetRuntimeEmissionRateMultiplier(float emissionRateMultiplier)
		{
			EngineApplicationInterface.IGameEntity.SetRuntimeEmissionRateMultiplier(this.Pointer, emissionRateMultiplier);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00010C30 File Offset: 0x0000EE30
		public BoundingBox GetLocalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetLocalBoundingBox(this.Pointer);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00010C42 File Offset: 0x0000EE42
		public BoundingBox GetGlobalBoundingBox()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalBoundingBox(this.Pointer);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00010C54 File Offset: 0x0000EE54
		public Vec3 GetBoundingBoxMin()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMin(this.Pointer);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00010C66 File Offset: 0x0000EE66
		public void SetHasCustomBoundingBoxValidationSystem(bool hasCustomBoundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetHasCustomBoundingBoxValidationSystem(this.Pointer, hasCustomBoundingBox);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00010C79 File Offset: 0x0000EE79
		public void ValidateBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.ValidateBoundingBox(this.Pointer);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00010C8B File Offset: 0x0000EE8B
		public Vec3 GetBoundingBoxMax()
		{
			return EngineApplicationInterface.IGameEntity.GetBoundingBoxMax(this.Pointer);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00010C9D File Offset: 0x0000EE9D
		public void UpdateGlobalBounds()
		{
			EngineApplicationInterface.IGameEntity.UpdateGlobalBounds(this.Pointer);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00010CAF File Offset: 0x0000EEAF
		public void RecomputeBoundingBox()
		{
			EngineApplicationInterface.IGameEntity.RecomputeBoundingBox(this.Pointer);
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00010CC1 File Offset: 0x0000EEC1
		public float GetBoundingBoxRadius()
		{
			return EngineApplicationInterface.IGameEntity.GetRadius(this.Pointer);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00010CD3 File Offset: 0x0000EED3
		public void SetBoundingboxDirty()
		{
			EngineApplicationInterface.IGameEntity.SetBoundingboxDirty(this.Pointer);
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00010CE5 File Offset: 0x0000EEE5
		public Vec3 GlobalBoxMax
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMax(this.Pointer);
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00010CF8 File Offset: 0x0000EEF8
		public ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax()
		{
			MatrixFrame globalFrame = this.GetGlobalFrame();
			BoundingBox localPhysicsBoundingBox = this.GetLocalPhysicsBoundingBox(true);
			Vec3 vec = globalFrame.TransformToParent(in localPhysicsBoundingBox.min);
			Vec3 vec2 = globalFrame.TransformToParent(in localPhysicsBoundingBox.max);
			return new ValueTuple<Vec3, Vec3>(vec, vec2);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00010D3C File Offset: 0x0000EF3C
		public Vec3 ComputeGlobalPhysicsBoundingBoxCenter()
		{
			return this.GetGlobalFrame().TransformToParent(in this.GetLocalPhysicsBoundingBox(true).center);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00010D6C File Offset: 0x0000EF6C
		public void SetContourColor(uint? color, bool alwaysVisible = true)
		{
			if (color != null)
			{
				EngineApplicationInterface.IGameEntity.SetAsContourEntity(this.Pointer, color.Value);
				EngineApplicationInterface.IGameEntity.SetContourState(this.Pointer, alwaysVisible);
				return;
			}
			EngineApplicationInterface.IGameEntity.DisableContour(this.Pointer);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x00010DBB File Offset: 0x0000EFBB
		public Vec3 GlobalBoxMin
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetGlobalBoxMin(this.Pointer);
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00010DCD File Offset: 0x0000EFCD
		public bool GetHasFrameChanged()
		{
			return EngineApplicationInterface.IGameEntity.HasFrameChanged(this.Pointer);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00010DDF File Offset: 0x0000EFDF
		public Mesh GetFirstMesh()
		{
			return EngineApplicationInterface.IGameEntity.GetFirstMesh(this.Pointer);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		public int GetAttachedNavmeshFaceCount()
		{
			return EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceCount(this.Pointer);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00010E03 File Offset: 0x0000F003
		public void GetAttachedNavmeshFaceRecords(PathFaceRecord[] faceRecords)
		{
			EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceRecords(this.Pointer, faceRecords);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00010E16 File Offset: 0x0000F016
		public void GetAttachedNavmeshFaceVertexIndices(in PathFaceRecord faceRecord, int[] indices)
		{
			EngineApplicationInterface.IGameEntity.GetAttachedNavmeshFaceVertexIndices(this.Pointer, in faceRecord, indices);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00010E2A File Offset: 0x0000F02A
		public void SetCustomVertexPositionEnabled(bool customVertexPositionEnabled)
		{
			EngineApplicationInterface.IGameEntity.SetCustomVertexPositionEnabled(this.Pointer, customVertexPositionEnabled);
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00010E3D File Offset: 0x0000F03D
		public void SetPositionsForAttachedNavmeshVertices(int[] vertices, int indexCount, Vec3[] positions)
		{
			EngineApplicationInterface.IGameEntity.SetPositionsForAttachedNavmeshVertices(this.Pointer, vertices, indexCount, positions);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00010E52 File Offset: 0x0000F052
		public void SetCostAdderForAttachedFaces(float costs)
		{
			EngineApplicationInterface.IGameEntity.SetCostAdderForAttachedFaces(this.Pointer, costs);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00010E65 File Offset: 0x0000F065
		public void SetExternalReferencesUsage(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetExternalReferencesUsage(this.Pointer, value);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00010E78 File Offset: 0x0000F078
		public void SetMorphFrameOfComponents(float value)
		{
			EngineApplicationInterface.IGameEntity.SetMorphFrameOfComponents(this.Pointer, value);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00010E8B File Offset: 0x0000F08B
		public void AddEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.AddEditDataUserToAllMeshes(this.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00010E9F File Offset: 0x0000F09F
		public void ReleaseEditDataUserToAllMeshes(bool entityComponents, bool skeletonComponents)
		{
			EngineApplicationInterface.IGameEntity.ReleaseEditDataUserToAllMeshes(this.Pointer, entityComponents, skeletonComponents);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00010EB3 File Offset: 0x0000F0B3
		public void GetCameraParamsFromCameraScript(Camera cam, ref Vec3 dofParams)
		{
			EngineApplicationInterface.IGameEntity.GetCameraParamsFromCameraScript(this.Pointer, cam.Pointer, ref dofParams);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00010ECC File Offset: 0x0000F0CC
		public void GetMeshBendedFrame(MatrixFrame worldSpacePosition, ref MatrixFrame output)
		{
			EngineApplicationInterface.IGameEntity.GetMeshBendedPosition(this.Pointer, ref worldSpacePosition, ref output);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00010EE1 File Offset: 0x0000F0E1
		public void ComputeTrajectoryVolume(float missileSpeed, float verticalAngleMaxInDegrees, float verticalAngleMinInDegrees, float horizontalAngleRangeInDegrees, float airFrictionConstant)
		{
			EngineApplicationInterface.IGameEntity.ComputeTrajectoryVolume(this.Pointer, missileSpeed, verticalAngleMaxInDegrees, verticalAngleMinInDegrees, horizontalAngleRangeInDegrees, airFrictionConstant);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00010EFA File Offset: 0x0000F0FA
		public void SetAnimTreeChannelParameterForceUpdate(float phase, int channelNo)
		{
			EngineApplicationInterface.IGameEntity.SetAnimTreeChannelParameter(this.Pointer, phase, channelNo);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00010F0E File Offset: 0x0000F10E
		public void ChangeMetaMeshOrRemoveItIfNotExists(MetaMesh entityMetaMesh, MetaMesh newMetaMesh)
		{
			EngineApplicationInterface.IGameEntity.ChangeMetaMeshOrRemoveItIfNotExists(this.Pointer, (entityMetaMesh != null) ? entityMetaMesh.Pointer : UIntPtr.Zero, (newMetaMesh != null) ? newMetaMesh.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00010F4C File Offset: 0x0000F14C
		public void SetUpdateValidtyOnFrameChangedOfFacesWithId(int faceGroupId, bool updateValidity)
		{
			EngineApplicationInterface.IGameEntity.SetUpdateValidityOnFrameChangedOfFacesWithId(this.Pointer, faceGroupId, updateValidity);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00010F60 File Offset: 0x0000F160
		public void AttachNavigationMeshFaces(int faceGroupId, bool isConnected, bool isBlocker = false, bool autoLocalize = false, bool finalizeBlockerConvexHullComputation = false, bool updateEntityFrame = true)
		{
			EngineApplicationInterface.IGameEntity.AttachNavigationMeshFaces(this.Pointer, faceGroupId, isConnected, isBlocker, autoLocalize, finalizeBlockerConvexHullComputation, updateEntityFrame);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00010F7B File Offset: 0x0000F17B
		public void DetachAllAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.DetachAllAttachedNavigationMeshFaces(this.Pointer);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00010F8D File Offset: 0x0000F18D
		public void UpdateAttachedNavigationMeshFaces()
		{
			EngineApplicationInterface.IGameEntity.UpdateAttachedNavigationMeshFaces(this.Pointer);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00010F9F File Offset: 0x0000F19F
		public void RemoveSkeleton()
		{
			this.Skeleton = null;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00010FA8 File Offset: 0x0000F1A8
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x00010FBA File Offset: 0x0000F1BA
		public Skeleton Skeleton
		{
			get
			{
				return EngineApplicationInterface.IGameEntity.GetSkeleton(this.Pointer);
			}
			set
			{
				EngineApplicationInterface.IGameEntity.SetSkeleton(this.Pointer, (value != null) ? value.Pointer : UIntPtr.Zero);
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00010FDC File Offset: 0x0000F1DC
		public void RemoveAllChildren()
		{
			EngineApplicationInterface.IGameEntity.RemoveAllChildren(this.Pointer);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00010FEE File Offset: 0x0000F1EE
		public IEnumerable<WeakGameEntity> GetChildren()
		{
			int count = this.ChildCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetChild(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00011003 File Offset: 0x0000F203
		public IEnumerable<WeakGameEntity> GetEntityAndChildren()
		{
			yield return ref this;
			int count = this.ChildCount;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetChild(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00011018 File Offset: 0x0000F218
		public void GetChildrenRecursive(ref List<WeakGameEntity> children)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = this.GetChild(i);
				children.Add(child);
				child.GetChildrenRecursive(ref children);
			}
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00011050 File Offset: 0x0000F250
		public void GetChildrenWithTagRecursive(List<WeakGameEntity> children, string tag)
		{
			int childCount = this.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				WeakGameEntity child = this.GetChild(i);
				if (child.HasTag(tag))
				{
					children.Add(child);
				}
				child.GetChildrenWithTagRecursive(children, tag);
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00011092 File Offset: 0x0000F292
		public bool IsSelectedOnEditor()
		{
			return EngineApplicationInterface.IGameEntity.IsEntitySelectedOnEditor(this.Pointer);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000110A4 File Offset: 0x0000F2A4
		public void SelectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.SelectEntityOnEditor(this.Pointer);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000110B6 File Offset: 0x0000F2B6
		public void DeselectEntityOnEditor()
		{
			EngineApplicationInterface.IGameEntity.DeselectEntityOnEditor(this.Pointer);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000110C8 File Offset: 0x0000F2C8
		public void SetAsPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.SetAsPredisplayEntity(this.Pointer);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x000110DA File Offset: 0x0000F2DA
		public void RemoveFromPredisplayEntity()
		{
			EngineApplicationInterface.IGameEntity.RemoveFromPredisplayEntity(this.Pointer);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000110EC File Offset: 0x0000F2EC
		public void SetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.SetNativeScriptComponentVariable(this.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00011103 File Offset: 0x0000F303
		public void SetManualGlobalBoundingBox(Vec3 boundingBoxStartGlobal, Vec3 boundingBoxEndGlobal)
		{
			EngineApplicationInterface.IGameEntity.SetManualGlobalBoundingBox(this.Pointer, boundingBoxStartGlobal, boundingBoxEndGlobal);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00011117 File Offset: 0x0000F317
		public bool RayHitEntityWithNormal(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref Vec3 resultNormal, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntityWithNormal(this.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultNormal, ref resultLength);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00011132 File Offset: 0x0000F332
		public bool RayHitEntity(Vec3 rayOrigin, Vec3 rayDirection, float maxLength, ref float resultLength)
		{
			return EngineApplicationInterface.IGameEntity.RayHitEntity(this.Pointer, in rayOrigin, in rayDirection, maxLength, ref resultLength);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0001114B File Offset: 0x0000F34B
		public void GetNativeScriptComponentVariable(string className, string fieldName, ref ScriptComponentFieldHolder data, RglScriptFieldType variableType)
		{
			EngineApplicationInterface.IGameEntity.GetNativeScriptComponentVariable(this.Pointer, className, fieldName, ref data, variableType);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00011162 File Offset: 0x0000F362
		public void SetCustomClipPlane(Vec3 clipPosition, Vec3 clipNormal, bool setForChildren)
		{
			EngineApplicationInterface.IGameEntity.SetCustomClipPlane(this.Pointer, clipPosition, clipNormal, setForChildren);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00011177 File Offset: 0x0000F377
		public float GetBoundingBoxLongestHalfDimension()
		{
			return BoundingBox.GetLongestHalfDimensionOfBoundingBox(this.GetLocalBoundingBox());
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00011184 File Offset: 0x0000F384
		public BoundingBox ComputeBoundingBoxFromLongestHalfDimension(float longestHalfDimensionCoefficient)
		{
			BoundingBox localBoundingBox = this.GetLocalBoundingBox();
			BoundingBox boundingBox = default(BoundingBox);
			float num = this.GetBoundingBoxLongestHalfDimension() * longestHalfDimensionCoefficient;
			Vec3 vec = new Vec3(num, num, num, -1f);
			boundingBox.min = localBoundingBox.center - vec;
			boundingBox.max = localBoundingBox.center + vec;
			boundingBox.RecomputeRadius();
			return boundingBox;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public BoundingBox ComputeBoundingBoxIncludeChildren()
		{
			BoundingBox boundingBox = default(BoundingBox);
			boundingBox.BeginRelaxation();
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				weakGameEntity.ValidateBoundingBox();
				BoundingBox localBoundingBox = weakGameEntity.GetLocalBoundingBox();
				boundingBox.RelaxWithChildBoundingBox(localBoundingBox, weakGameEntity.GetFrame());
			}
			boundingBox.RecomputeRadius();
			return boundingBox;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00011264 File Offset: 0x0000F464
		public void SetManualLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.SetManualLocalBoundingBox(this.Pointer, in boundingBox);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00011277 File Offset: 0x0000F477
		public void RelaxLocalBoundingBox(in BoundingBox boundingBox)
		{
			EngineApplicationInterface.IGameEntity.RelaxLocalBoundingBox(this.Pointer, in boundingBox);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0001128A File Offset: 0x0000F48A
		public void SetCullMode(MBMeshCullingMode cullMode)
		{
			EngineApplicationInterface.IGameEntity.SetCullMode(this.Pointer, cullMode);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x000112A0 File Offset: 0x0000F4A0
		public WeakGameEntity GetFirstChildEntityWithTagRecursive(string tag)
		{
			UIntPtr firstChildWithTagRecursive = EngineApplicationInterface.IGameEntity.GetFirstChildWithTagRecursive(this.Pointer, tag);
			if (firstChildWithTagRecursive != UIntPtr.Zero)
			{
				return new WeakGameEntity(firstChildWithTagRecursive);
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x000112D8 File Offset: 0x0000F4D8
		public override bool Equals(object obj)
		{
			return ((WeakGameEntity)obj).Pointer == this.Pointer;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00011300 File Offset: 0x0000F500
		public override int GetHashCode()
		{
			return this.Pointer.GetHashCode();
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0001131B File Offset: 0x0000F51B
		public static bool operator ==(WeakGameEntity weakGameEntity, GameEntity entity)
		{
			return weakGameEntity.Pointer == ((entity != null) ? entity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00011339 File Offset: 0x0000F539
		public static bool operator !=(WeakGameEntity weakGameEntity, GameEntity entity)
		{
			return weakGameEntity.Pointer != ((entity != null) ? entity.Pointer : UIntPtr.Zero);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00011357 File Offset: 0x0000F557
		public static bool operator ==(WeakGameEntity weakGameEntity1, WeakGameEntity weakGameEntity2)
		{
			return weakGameEntity1.Pointer == weakGameEntity2.Pointer;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0001136C File Offset: 0x0000F56C
		public static bool operator !=(WeakGameEntity weakGameEntity1, WeakGameEntity weakGameEntity2)
		{
			return weakGameEntity1.Pointer != weakGameEntity2.Pointer;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00011384 File Offset: 0x0000F584
		public List<WeakGameEntity> CollectChildrenEntitiesWithTag(string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			foreach (WeakGameEntity weakGameEntity in this.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.Add(weakGameEntity);
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000113FC File Offset: 0x0000F5FC
		public IEnumerable<WeakGameEntity> CollectChildrenEntitiesWithTagAsEnumarable(string tag)
		{
			foreach (WeakGameEntity child in this.GetChildren())
			{
				if (child.HasTag(tag))
				{
					yield return child;
				}
				if (child.ChildCount > 0)
				{
					foreach (WeakGameEntity weakGameEntity in child.CollectChildrenEntitiesWithTagAsEnumarable(tag))
					{
						yield return weakGameEntity;
					}
					IEnumerator<WeakGameEntity> enumerator2 = null;
				}
			}
			IEnumerator<WeakGameEntity> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00011418 File Offset: 0x0000F618
		public void SetDoNotCheckVisibility(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetDoNotCheckVisibility(this.Pointer, value);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0001142B File Offset: 0x0000F62B
		public void SetBoneFrameToAllMeshes(int boneIndex, in MatrixFrame frame)
		{
			EngineApplicationInterface.IGameEntity.SetBoneFrameToAllMeshes(this.Pointer, boneIndex, in frame);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0001143F File Offset: 0x0000F63F
		public Vec2 GetGlobalWindStrengthVectorOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindStrengthVectorOfScene(this.Pointer);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00011451 File Offset: 0x0000F651
		public Vec2 GetGlobalWindVelocityOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindVelocityOfScene(this.Pointer);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00011463 File Offset: 0x0000F663
		public Vec3 GetLastFinalRenderCameraPositionOfScene()
		{
			return EngineApplicationInterface.IGameEntity.GetLastFinalRenderCameraPositionOfScene(this.Pointer);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00011475 File Offset: 0x0000F675
		public Vec2 GetGlobalWindVelocityWithGustNoiseOfScene(float globalTime)
		{
			return EngineApplicationInterface.IGameEntity.GetGlobalWindVelocityWithGustNoiseOfScene(this.Pointer, globalTime);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00011488 File Offset: 0x0000F688
		public void SetForceDecalsToRender(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceDecalsToRender(this.Pointer, value);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0001149B File Offset: 0x0000F69B
		public UIntPtr CreateEmptyPhysxShape(bool isVariable, int physxMaterialIndex)
		{
			return EngineApplicationInterface.IGameEntity.CreateEmptyPhysxShape(this.Pointer, isVariable, physxMaterialIndex);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000114AF File Offset: 0x0000F6AF
		public void SetForceNotAffectedBySeason(bool value)
		{
			EngineApplicationInterface.IGameEntity.SetForceNotAffectedBySeason(this.Pointer, value);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x000114C2 File Offset: 0x0000F6C2
		public bool CheckIsPrefabLinkRootPrefab(int depth)
		{
			return EngineApplicationInterface.IGameEntity.CheckIsPrefabLinkRootPrefab(this.Pointer, depth);
		}

		// Token: 0x04000203 RID: 515
		public static readonly WeakGameEntity Invalid = new WeakGameEntity(UIntPtr.Zero);
	}
}
