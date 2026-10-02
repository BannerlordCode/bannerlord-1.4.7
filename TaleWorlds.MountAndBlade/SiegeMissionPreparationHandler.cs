using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029C RID: 668
	public class SiegeMissionPreparationHandler : MissionLogic
	{
		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x00086310 File Offset: 0x00084510
		private Scene MissionScene
		{
			get
			{
				return Mission.Current.Scene;
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0008631C File Offset: 0x0008451C
		public SiegeMissionPreparationHandler(bool isSallyOut, bool isReliefForceAttack, float[] wallHitPointPercentages, bool hasAnySiegeTower)
		{
			if (isSallyOut)
			{
				this._siegeMissionType = SiegeMissionPreparationHandler.SiegeMissionType.SallyOut;
			}
			else if (isReliefForceAttack)
			{
				this._siegeMissionType = SiegeMissionPreparationHandler.SiegeMissionType.ReliefForce;
			}
			else
			{
				this._siegeMissionType = SiegeMissionPreparationHandler.SiegeMissionType.Assault;
			}
			this._wallHitPointPercentages = wallHitPointPercentages;
			this._hasAnySiegeTower = hasAnySiegeTower;
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00086352 File Offset: 0x00084552
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.SetUpScene();
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x00086360 File Offset: 0x00084560
		private void SetUpScene()
		{
			this.ArrangeBesiegerDeploymentPointsAndMachines();
			this.ArrangeEntitiesForMissionType();
			this.ArrangeDestructedMeshes();
			if (this._siegeMissionType != SiegeMissionPreparationHandler.SiegeMissionType.Assault)
			{
				this.ArrangeSiegeMachinesForNonAssaultMission();
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00086384 File Offset: 0x00084584
		private void ArrangeBesiegerDeploymentPointsAndMachines()
		{
			bool flag = this._siegeMissionType == SiegeMissionPreparationHandler.SiegeMissionType.Assault;
			Debug.Print("{SIEGE} ArrangeBesiegerDeploymentPointsAndMachines", 0, Debug.DebugColor.DarkCyan, 64UL);
			Debug.Print("{SIEGE} MissionType: " + this._siegeMissionType, 0, Debug.DebugColor.DarkCyan, 64UL);
			if (!flag)
			{
				SiegeLadder[] array = base.Mission.ActiveMissionObjects.FindAllWithType<SiegeLadder>().ToArray<SiegeLadder>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetDisabledSynched();
				}
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000863F8 File Offset: 0x000845F8
		private void ArrangeEntitiesForMissionType()
		{
			string text = ((this._siegeMissionType == SiegeMissionPreparationHandler.SiegeMissionType.Assault) ? "sally_out" : "siege_assault");
			Debug.Print("{SIEGE} ArrangeEntitiesForMissionType", 0, Debug.DebugColor.DarkCyan, 64UL);
			Debug.Print("{SIEGE} MissionType: " + this._siegeMissionType, 0, Debug.DebugColor.DarkCyan, 64UL);
			Debug.Print("{SIEGE} TagToBeRemoved: " + text, 0, Debug.DebugColor.DarkCyan, 64UL);
			foreach (WeakGameEntity weakGameEntity in this.MissionScene.FindWeakEntitiesWithTag(text).ToList<WeakGameEntity>())
			{
				weakGameEntity.Remove(77);
			}
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000864B0 File Offset: 0x000846B0
		private void ArrangeDestructedMeshes()
		{
			float num = 0f;
			foreach (float num2 in this._wallHitPointPercentages)
			{
				num += num2;
			}
			if (!this._wallHitPointPercentages.IsEmpty<float>())
			{
				num /= (float)this._wallHitPointPercentages.Length;
			}
			float num3 = MBMath.Lerp(0f, 0.7f, 1f - num, 1E-05f);
			IEnumerable<SynchedMissionObject> enumerable = base.Mission.MissionObjects.OfType<SynchedMissionObject>();
			IEnumerable<DestructableComponent> enumerable2 = enumerable.OfType<DestructableComponent>();
			foreach (StrategicArea strategicArea in base.Mission.ActiveMissionObjects.OfType<StrategicArea>().ToList<StrategicArea>())
			{
				strategicArea.DetermineAssociatedDestructibleComponents(enumerable2);
			}
			foreach (SynchedMissionObject synchedMissionObject in enumerable)
			{
				if (this._hasAnySiegeTower && synchedMissionObject.GameEntity.HasTag("tower_merlon"))
				{
					synchedMissionObject.SetVisibleSynched(false, true);
				}
				else
				{
					DestructableComponent firstScriptOfType = synchedMissionObject.GameEntity.GetFirstScriptOfType<DestructableComponent>();
					if (firstScriptOfType != null && firstScriptOfType.CanBeDestroyedInitially && num3 > 0f && MBRandom.RandomFloat <= num3)
					{
						firstScriptOfType.PreDestroy();
					}
				}
			}
			if (num3 >= 0.1f)
			{
				List<WeakGameEntity> list = base.Mission.Scene.FindWeakEntitiesWithTag("damage_decal").ToList<WeakGameEntity>();
				foreach (WeakGameEntity weakGameEntity in list)
				{
					weakGameEntity.GetFirstScriptOfType<SynchedMissionObject>().SetVisibleSynched(false, false);
				}
				for (int j = MathF.Floor((float)list.Count * num3); j > 0; j--)
				{
					WeakGameEntity weakGameEntity2 = list[MBRandom.RandomInt(list.Count)];
					list.Remove(weakGameEntity2);
					weakGameEntity2.GetFirstScriptOfType<SynchedMissionObject>().SetVisibleSynched(true, false);
				}
			}
			List<WallSegment> list2 = new List<WallSegment>();
			List<WallSegment> list3 = base.Mission.ActiveMissionObjects.FindAllWithType<WallSegment>().Where<WallSegment>(delegate(WallSegment ws)
			{
				if (ws.DefenseSide != FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					return ws.GameEntity.GetChildren().Any<WeakGameEntity>((WeakGameEntity ge) => ge.HasTag("broken_child"));
				}
				return false;
			}).ToList<WallSegment>();
			foreach (float num4 in this._wallHitPointPercentages)
			{
				WallSegment wallSegment = this.FindRightMostWall(list3);
				if (MathF.Abs(num4) < 1E-05f)
				{
					wallSegment.OnChooseUsedWallSegment(true);
					list2.Add(wallSegment);
				}
				else
				{
					wallSegment.OnChooseUsedWallSegment(false);
				}
				list3.Remove(wallSegment);
			}
			foreach (WallSegment wallSegment2 in list3)
			{
				wallSegment2.OnChooseUsedWallSegment(false);
			}
			if (num3 >= 0.1f)
			{
				List<SiegeWeapon> list4 = new List<SiegeWeapon>();
				using (IEnumerator<SiegeWeapon> enumerator5 = (from sw in base.Mission.ActiveMissionObjects.FindAllWithType<SiegeWeapon>()
					where sw is IPrimarySiegeWeapon
					select sw).GetEnumerator())
				{
					while (enumerator5.MoveNext())
					{
						SiegeWeapon primarySiegeWeapon = enumerator5.Current;
						if (list2.Any<WallSegment>((WallSegment b) => b.DefenseSide == ((IPrimarySiegeWeapon)primarySiegeWeapon).WeaponSide))
						{
							list4.Add(primarySiegeWeapon);
						}
					}
				}
				list4.ForEach(delegate(SiegeWeapon siegeWeaponToRemove)
				{
					siegeWeaponToRemove.SetDisabledSynched();
				});
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00086888 File Offset: 0x00084A88
		private WallSegment FindRightMostWall(List<WallSegment> wallList)
		{
			int count = wallList.Count;
			if (count == 1)
			{
				return wallList[0];
			}
			BatteringRam batteringRam = base.Mission.ActiveMissionObjects.FindAllWithType<BatteringRam>().First<BatteringRam>();
			if (count != 2)
			{
				return null;
			}
			if (Vec3.CrossProduct(wallList[0].GameEntity.GlobalPosition - batteringRam.GameEntity.GlobalPosition, wallList[1].GameEntity.GlobalPosition - batteringRam.GameEntity.GlobalPosition).z < 0f)
			{
				return wallList[1];
			}
			return wallList[0];
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00086934 File Offset: 0x00084B34
		private void ArrangeSiegeMachinesForNonAssaultMission()
		{
			foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<SiegeWeapon>())
			{
				SiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SiegeWeapon>();
				if (!(firstScriptOfType is RangedSiegeWeapon))
				{
					firstScriptOfType.Deactivate();
				}
			}
		}

		// Token: 0x04000E4D RID: 3661
		private const string SallyOutTag = "sally_out";

		// Token: 0x04000E4E RID: 3662
		private const string AssaultTag = "siege_assault";

		// Token: 0x04000E4F RID: 3663
		private const string DamageDecalTag = "damage_decal";

		// Token: 0x04000E50 RID: 3664
		private float[] _wallHitPointPercentages;

		// Token: 0x04000E51 RID: 3665
		private bool _hasAnySiegeTower;

		// Token: 0x04000E52 RID: 3666
		private SiegeMissionPreparationHandler.SiegeMissionType _siegeMissionType;

		// Token: 0x0200056E RID: 1390
		private enum SiegeMissionType
		{
			// Token: 0x04001E33 RID: 7731
			Assault,
			// Token: 0x04001E34 RID: 7732
			SallyOut,
			// Token: 0x04001E35 RID: 7733
			ReliefForce
		}
	}
}
