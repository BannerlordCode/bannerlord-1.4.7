using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D0 RID: 976
	public class CaravanBattleMissionHandler : MissionLogic
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x000E0780 File Offset: 0x000DE980
		public CaravanBattleMissionHandler(int unitCount, bool isCamelCulture, bool isCaravan)
		{
			this._unitCount = unitCount;
			this._isCamelCulture = isCamelCulture;
			this._isCaravan = isCaravan;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000E0820 File Offset: 0x000DEA20
		public override void AfterStart()
		{
			base.AfterStart();
			float num = Mission.ComputeSpawnPathDeploymentOffset((int)((float)this._unitCount * 1.5f), base.Mission.GetInitialSpawnPath());
			WorldFrame spawnPathFrame = base.Mission.GetSpawnPathFrame(base.Mission.DefenderTeam.Side, num, 0f);
			Scene scene = Mission.Current.Scene;
			string text = (this._isCaravan ? "caravan_scattered_goods_prop" : "villager_scattered_goods_prop");
			Vec3 vec = spawnPathFrame.Origin.GetGroundVec3();
			this._entity = GameEntity.Instantiate(scene, text, new MatrixFrame(in spawnPathFrame.Rotation, in vec), true);
			this._entity.SetMobility(GameEntity.Mobility.Dynamic);
			foreach (GameEntity gameEntity in this._entity.GetChildren())
			{
				Scene scene2 = Mission.Current.Scene;
				vec = gameEntity.GlobalPosition;
				float num2;
				Vec3 vec2;
				scene2.GetTerrainHeightAndNormal(vec.AsVec2, out num2, out vec2);
				MatrixFrame globalFrame = gameEntity.GetGlobalFrame();
				globalFrame.origin.z = num2;
				globalFrame.rotation.u = vec2;
				globalFrame.rotation.Orthonormalize();
				gameEntity.SetGlobalFrame(in globalFrame, true);
			}
			IEnumerable<GameEntity> enumerable = from c in this._entity.GetChildren()
				where c.HasTag("caravan_animal_spawn")
				select c;
			int num3 = (int)((float)enumerable.Count<GameEntity>() * 0.4f);
			foreach (GameEntity gameEntity2 in enumerable)
			{
				MatrixFrame globalFrame2 = gameEntity2.GetGlobalFrame();
				string text2;
				if (this._isCamelCulture)
				{
					if (num3 > 0)
					{
						int num4 = MBRandom.RandomInt(this._camelMountableHarnesses.Length);
						text2 = this._camelMountableHarnesses[num4];
					}
					else
					{
						int num5 = MBRandom.RandomInt(this._camelLoadHarnesses.Length);
						text2 = this._camelLoadHarnesses[num5];
					}
				}
				else if (num3 > 0)
				{
					int num6 = MBRandom.RandomInt(this._muleMountableHarnesses.Length);
					text2 = this._muleMountableHarnesses[num6];
				}
				else
				{
					int num7 = MBRandom.RandomInt(this._muleLoadHarnesses.Length);
					text2 = this._muleLoadHarnesses[num7];
				}
				ItemRosterElement itemRosterElement = new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>(text2), 0, null);
				ItemRosterElement itemRosterElement2 = (this._isCamelCulture ? ((num3-- > 0) ? new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("pack_camel"), 0, null) : new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("pack_camel_unmountable"), 0, null)) : ((num3-- > 0) ? new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("mule"), 0, null) : new ItemRosterElement(Game.Current.ObjectManager.GetObject<ItemObject>("mule_unmountable"), 0, null)));
				Mission mission = Mission.Current;
				ItemRosterElement itemRosterElement3 = itemRosterElement2;
				ItemRosterElement itemRosterElement4 = itemRosterElement;
				Vec2 vec3 = globalFrame2.rotation.f.AsVec2;
				vec3 = vec3.Normalized();
				Agent agent = mission.SpawnMonster(itemRosterElement3, itemRosterElement4, in globalFrame2.origin, in vec3, -1);
				agent.SetAgentFlags(agent.GetAgentFlags() & ~AgentFlag.CanWander);
			}
			TacticalPosition firstScriptInFamilyDescending = this._entity.GetFirstScriptInFamilyDescending<TacticalPosition>();
			if (firstScriptInFamilyDescending != null)
			{
				foreach (Team team in Mission.Current.Teams)
				{
					team.TeamAI.TacticalPositions.Add(firstScriptInFamilyDescending);
				}
			}
		}

		// Token: 0x0400175A RID: 5978
		private GameEntity _entity;

		// Token: 0x0400175B RID: 5979
		private int _unitCount;

		// Token: 0x0400175C RID: 5980
		private bool _isCamelCulture;

		// Token: 0x0400175D RID: 5981
		private bool _isCaravan;

		// Token: 0x0400175E RID: 5982
		private readonly string[] _camelLoadHarnesses = new string[] { "camel_saddle_a", "camel_saddle_b" };

		// Token: 0x0400175F RID: 5983
		private readonly string[] _camelMountableHarnesses = new string[] { "camel_saddle" };

		// Token: 0x04001760 RID: 5984
		private readonly string[] _muleLoadHarnesses = new string[] { "mule_load_a", "mule_load_b", "mule_load_c" };

		// Token: 0x04001761 RID: 5985
		private readonly string[] _muleMountableHarnesses = new string[] { "aseran_village_harness", "steppe_fur_harness", "steppe_harness" };

		// Token: 0x04001762 RID: 5986
		private const string CaravanPrefabName = "caravan_scattered_goods_prop";

		// Token: 0x04001763 RID: 5987
		private const string VillagerGoodsPrefabName = "villager_scattered_goods_prop";
	}
}
