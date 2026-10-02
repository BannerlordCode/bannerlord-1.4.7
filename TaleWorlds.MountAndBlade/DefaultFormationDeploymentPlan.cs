using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020F RID: 527
	public class DefaultFormationDeploymentPlan : IFormationDeploymentPlan
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x0006A037 File Offset: 0x00068237
		public FormationClass Class
		{
			get
			{
				return this._class;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x0006A03F File Offset: 0x0006823F
		public FormationClass SpawnClass
		{
			get
			{
				return this._spawnClass;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x0006A047 File Offset: 0x00068247
		public float PlannedWidth
		{
			get
			{
				return this._plannedWidth;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x0006A04F File Offset: 0x0006824F
		public float PlannedDepth
		{
			get
			{
				return this._plannedDepth;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x0006A057 File Offset: 0x00068257
		public int PlannedTroopCount
		{
			get
			{
				return this._plannedFootTroopCount + this._plannedMountedTroopCount;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x0006A066 File Offset: 0x00068266
		public int PlannedFootTroopCount
		{
			get
			{
				return this._plannedFootTroopCount;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0006A06E File Offset: 0x0006826E
		public int PlannedMountedTroopCount
		{
			get
			{
				return this._plannedMountedTroopCount;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x0006A076 File Offset: 0x00068276
		public bool HasDimensions
		{
			get
			{
				return this._plannedWidth >= 1E-05f && this._plannedDepth >= 1E-05f;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0006A097 File Offset: 0x00068297
		public bool HasSignificantMountedTroops
		{
			get
			{
				return DefaultMissionDeploymentPlan.HasSignificantMountedTroops(this._plannedFootTroopCount, this._plannedMountedTroopCount);
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0006A0AA File Offset: 0x000682AA
		public DefaultFormationDeploymentPlan(FormationClass fClass)
		{
			this._class = fClass;
			this._spawnClass = fClass;
			this.Clear();
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0006A0C6 File Offset: 0x000682C6
		public bool HasFrame()
		{
			return this._spawnFrame.IsValid;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0006A0D3 File Offset: 0x000682D3
		public FormationDeploymentFlank GetDefaultFlank(int formationTroopCount, bool teamPlanHasAnyFootTroops, bool spawnWithHorses = false)
		{
			return DefaultFormationDeploymentPlan.GetFormationDefaultFlankAux(this._class, formationTroopCount, teamPlanHasAnyFootTroops, this.HasSignificantMountedTroops, spawnWithHorses);
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0006A0E9 File Offset: 0x000682E9
		public FormationDeploymentOrder GetFlankDeploymentOrder(int offset = 0)
		{
			return FormationDeploymentOrder.GetDeploymentOrder(this._class, offset);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0006A0F7 File Offset: 0x000682F7
		public MatrixFrame GetFrame()
		{
			return this._spawnFrame.ToGroundMatrixFrame();
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0006A104 File Offset: 0x00068304
		public Vec3 GetPosition()
		{
			return this._spawnFrame.Origin.GetGroundVec3();
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0006A118 File Offset: 0x00068318
		public Vec2 GetDirection()
		{
			return this._spawnFrame.Rotation.f.AsVec2.Normalized();
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0006A144 File Offset: 0x00068344
		public WorldPosition CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
			{
				return new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._spawnFrame.Origin.GetNavMeshVec3(), false);
			}
			if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.GroundVec3)
			{
				return this._spawnFrame.Origin;
			}
			return new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._spawnFrame.Origin.GetGroundVec3(), false);
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0006A1B2 File Offset: 0x000683B2
		public void Clear()
		{
			this._plannedWidth = 0f;
			this._plannedDepth = 0f;
			this._plannedFootTroopCount = 0;
			this._plannedMountedTroopCount = 0;
			this._spawnFrame = WorldFrame.Invalid;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0006A1E3 File Offset: 0x000683E3
		public void SetPlannedTroopCount(int footTroopCount, int mountedTroopCount)
		{
			this._plannedFootTroopCount = footTroopCount;
			this._plannedMountedTroopCount = mountedTroopCount;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0006A1F3 File Offset: 0x000683F3
		public void SetPlannedDimensions(float width, float depth)
		{
			this._plannedWidth = MathF.Max(0f, width);
			this._plannedDepth = MathF.Max(0f, depth);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0006A217 File Offset: 0x00068417
		public void SetFrame(in WorldFrame frame)
		{
			this._spawnFrame = frame;
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0006A225 File Offset: 0x00068425
		public void SetSpawnClass(FormationClass spawnClass)
		{
			this._spawnClass = spawnClass;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0006A230 File Offset: 0x00068430
		public static FormationDeploymentFlank GetFormationDefaultFlankAux(FormationClass formationClass, int formationTroopCount, bool teamPlanHasAnyFootTroops, bool hasSignificantMountedTroops, bool canSpawnWithHorses)
		{
			FormationDeploymentFlank formationDeploymentFlank;
			if (!formationClass.IsMounted() && formationTroopCount == 0)
			{
				formationDeploymentFlank = FormationDeploymentFlank.Rear;
			}
			else if (hasSignificantMountedTroops && (!canSpawnWithHorses || !teamPlanHasAnyFootTroops))
			{
				if (formationTroopCount == 0 || formationClass == FormationClass.LightCavalry || formationClass == FormationClass.HorseArcher)
				{
					formationDeploymentFlank = FormationDeploymentFlank.Rear;
				}
				else
				{
					formationDeploymentFlank = FormationDeploymentFlank.Front;
				}
			}
			else
			{
				switch (formationClass)
				{
				case FormationClass.Ranged:
				case FormationClass.NumberOfRegularFormations:
				case FormationClass.Bodyguard:
				case FormationClass.NumberOfAllFormations:
					return FormationDeploymentFlank.Rear;
				case FormationClass.Cavalry:
				case FormationClass.HeavyCavalry:
					return FormationDeploymentFlank.Left;
				case FormationClass.HorseArcher:
				case FormationClass.LightCavalry:
					return FormationDeploymentFlank.Right;
				}
				formationDeploymentFlank = FormationDeploymentFlank.Front;
			}
			return formationDeploymentFlank;
		}

		// Token: 0x04000A7B RID: 2683
		private WorldFrame _spawnFrame;

		// Token: 0x04000A7C RID: 2684
		private FormationClass _spawnClass;

		// Token: 0x04000A7D RID: 2685
		private readonly FormationClass _class;

		// Token: 0x04000A7E RID: 2686
		private float _plannedWidth;

		// Token: 0x04000A7F RID: 2687
		private float _plannedDepth;

		// Token: 0x04000A80 RID: 2688
		private int _plannedFootTroopCount;

		// Token: 0x04000A81 RID: 2689
		private int _plannedMountedTroopCount;
	}
}
