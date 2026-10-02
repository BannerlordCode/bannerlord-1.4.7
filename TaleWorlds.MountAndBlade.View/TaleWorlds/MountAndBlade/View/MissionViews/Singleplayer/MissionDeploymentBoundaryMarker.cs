using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x02000094 RID: 148
	public class MissionDeploymentBoundaryMarker : MissionView
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x00026E07 File Offset: 0x00025007
		public MissionDeploymentBoundaryMarker(string prefabName, float markerInterval = 2f)
		{
			this._prefabName = prefabName;
			this.MarkerInterval = Math.Max(markerInterval, 0.0001f);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00026E3C File Offset: 0x0002503C
		public override void AfterStart()
		{
			base.AfterStart();
			for (int i = 0; i < 2; i++)
			{
				this._boundaryMarkersPerSide[i] = new Dictionary<string, List<GameEntity>>();
			}
			this._boundaryMarkersRemoved = false;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00026E6F File Offset: 0x0002506F
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.TryRemoveBoundaryMarkers();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00026E80 File Offset: 0x00025080
		public override void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			if (team.IsPlayerTeam || team == base.Mission.PlayerEnemyTeam)
			{
				bool flag = base.Mission.DeploymentPlan.HasDeploymentBoundaries(team);
				if (isFirstPlan && flag)
				{
					foreach (ValueTuple<string, MBList<Vec2>> valueTuple in base.Mission.DeploymentPlan.GetDeploymentBoundaries(team))
					{
						this.AddBoundaryMarkerForSide(team.Side, new KeyValuePair<string, ICollection<Vec2>>(valueTuple.Item1, valueTuple.Item2));
					}
				}
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00026F24 File Offset: 0x00025124
		public override void OnRemoveBehavior()
		{
			this.TryRemoveBoundaryMarkers();
			base.OnRemoveBehavior();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00026F34 File Offset: 0x00025134
		private void AddBoundaryMarkerForSide(BattleSideEnum side, KeyValuePair<string, ICollection<Vec2>> boundary)
		{
			string key = boundary.Key;
			if (!this._boundaryMarkersPerSide[(int)side].ContainsKey(key))
			{
				Banner banner = ((side == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam.Banner : ((side == BattleSideEnum.Defender) ? base.Mission.DefenderTeam.Banner : null));
				List<GameEntity> list = new List<GameEntity>();
				List<Vec2> list2 = boundary.Value.ToList<Vec2>();
				for (int i = 0; i < list2.Count; i++)
				{
					this.MarkLine(new Vec3(list2[i], 0f, -1f), new Vec3(list2[(i + 1) % list2.Count], 0f, -1f), list, banner);
				}
				this._boundaryMarkersPerSide[(int)side][key] = list;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00027008 File Offset: 0x00025208
		private void TryRemoveBoundaryMarkers()
		{
			if (!this._boundaryMarkersRemoved)
			{
				for (int i = 0; i < 2; i++)
				{
					foreach (string text in this._boundaryMarkersPerSide[i].Keys.ToList<string>())
					{
						this.RemoveBoundaryMarker(text, (BattleSideEnum)i);
					}
				}
				this._boundaryMarkersRemoved = true;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00027084 File Offset: 0x00025284
		private void RemoveBoundaryMarker(string boundaryName, BattleSideEnum side)
		{
			List<GameEntity> list;
			if (this._boundaryMarkersPerSide[(int)side].TryGetValue(boundaryName, out list))
			{
				foreach (GameEntity gameEntity in list)
				{
					gameEntity.Remove(103);
				}
				this._boundaryMarkersPerSide[(int)side].Remove(boundaryName);
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000270F4 File Offset: 0x000252F4
		protected virtual void MarkLine(Vec3 startPoint, Vec3 endPoint, List<GameEntity> boundary, Banner banner = null)
		{
			Scene scene = base.Mission.Scene;
			Vec3 vec = endPoint - startPoint;
			float length = vec.Length;
			Vec3 vec2 = vec;
			vec2.Normalize();
			vec2 *= this.MarkerInterval;
			for (float num = 0f; num < length; num += this.MarkerInterval)
			{
				MatrixFrame identity = MatrixFrame.Identity;
				identity.rotation.RotateAboutUp(vec.RotationZ + 1.5707964f);
				identity.origin = startPoint;
				if (!scene.GetHeightAtPoint(identity.origin.AsVec2, BodyFlags.CommonCollisionExcludeFlagsForCombat, ref identity.origin.z))
				{
					identity.origin.z = 0f;
				}
				identity.origin.z = identity.origin.z - 0.5f;
				Vec3 vec3 = Vec3.One * 0.4f;
				identity.Scale(in vec3);
				GameEntity gameEntity = this.MakeEntity(banner);
				gameEntity.SetFrame(ref identity, true);
				boundary.Add(gameEntity);
				startPoint += vec2;
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00027208 File Offset: 0x00025408
		private GameEntity MakeEntity(Banner banner = null)
		{
			Scene scene = base.Mission.Scene;
			if (this._cachedEntity == null)
			{
				this._cachedEntity = GameEntity.Instantiate(null, this._prefabName, false, true, "");
			}
			GameEntity gameEntity = GameEntity.CopyFrom(scene, this._cachedEntity, true, true);
			gameEntity.SetMobility(GameEntity.Mobility.Dynamic);
			if (banner != null)
			{
				Mesh firstMesh = gameEntity.GetFirstMesh();
				Material material = firstMesh.GetMaterial();
				Material tableauMaterial = material.CreateCopy();
				BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
				banner.GetTableauTextureSmall(in bannerDebugInfo, delegate(Texture tex)
				{
					tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
				});
				firstMesh.SetMaterial(tableauMaterial);
			}
			return gameEntity;
		}

		// Token: 0x040002F3 RID: 755
		public const string AttackerStaticDeploymentBoundaryName = "walk_area";

		// Token: 0x040002F4 RID: 756
		public const string DefenderStaticDeploymentBoundaryName = "deployment_castle_boundary";

		// Token: 0x040002F5 RID: 757
		public readonly float MarkerInterval;

		// Token: 0x040002F6 RID: 758
		protected readonly Dictionary<string, List<GameEntity>>[] _boundaryMarkersPerSide = new Dictionary<string, List<GameEntity>>[2];

		// Token: 0x040002F7 RID: 759
		protected readonly string _prefabName;

		// Token: 0x040002F8 RID: 760
		protected GameEntity _cachedEntity;

		// Token: 0x040002F9 RID: 761
		protected bool _boundaryMarkersRemoved = true;
	}
}
