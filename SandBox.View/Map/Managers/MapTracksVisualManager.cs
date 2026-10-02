using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000073 RID: 115
	public class MapTracksVisualManager : EntityVisualManagerBase<Track>
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00025ABE File Offset: 0x00023CBE
		public static MapTracksVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<MapTracksVisualManager>();
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00025ACA File Offset: 0x00023CCA
		public override int Priority
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00025AD0 File Offset: 0x00023CD0
		public MapTracksVisualManager()
		{
			this._trackVisuals = new Dictionary<Track, ValueTuple<TrackVisual, GameEntity>>();
			this._entityPool = new Stack<GameEntity>();
			this.PopulateEntityPool();
			this._parallelUpdateTrackColorsPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelUpdateTrackColors);
			this._parallelUpdateVisibleTracksPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelUpdateVisibleTracks);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00025B2A File Offset: 0x00023D2A
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			if (this._tracksDirty)
			{
				this.UpdateTrackMesh();
				this._tracksDirty = false;
			}
			TWParallel.For(0, MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count, this._parallelUpdateTrackColorsPredicate, 16);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00025B63 File Offset: 0x00023D63
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			if (hoveredVisual == null)
			{
				hoveredVisual = this.GetVisualOfEntity(this.GetTrackOnMouse(mouseRay, terrainIntersectionPoint));
			}
			return hoveredVisual != null;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00025B84 File Offset: 0x00023D84
		public override void OnGameLoadFinished()
		{
			base.OnGameLoadFinished();
			foreach (Track track in MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks)
			{
				this.OnTrackDetected(track);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00025BE8 File Offset: 0x00023DE8
		public override MapEntityVisual<Track> GetVisualOfEntity(Track entity)
		{
			if (entity == null)
			{
				return null;
			}
			return this._trackVisuals[entity].Item1;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00025C00 File Offset: 0x00023E00
		protected override void OnFinalize()
		{
			base.OnFinalize();
			foreach (GameEntity gameEntity in this._entityPool.ToList<GameEntity>())
			{
				gameEntity.Remove(111);
			}
			this._entityPool.Clear();
			this._trackVisuals.Clear();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00025C80 File Offset: 0x00023E80
		protected override void OnInitialize()
		{
			base.OnInitialize();
			CampaignEvents.TrackDetectedEvent.AddNonSerializedListener(this, new Action<Track>(this.OnTrackDetected));
			CampaignEvents.TrackLostEvent.AddNonSerializedListener(this, new Action<Track>(this.OnTrackLost));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00025CB8 File Offset: 0x00023EB8
		internal void ReleaseResources(Track track)
		{
			ValueTuple<TrackVisual, GameEntity> valueTuple;
			if (this._trackVisuals.TryGetValue(track, out valueTuple))
			{
				valueTuple.Item2.Remove(111);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00025CE4 File Offset: 0x00023EE4
		private void OnTrackDetected(Track track)
		{
			this._tracksDirty = true;
			GameEntity gameEntity = this.GetGameEntity();
			gameEntity.SetVisibilityExcludeParents(true);
			this._trackVisuals.Add(track, new ValueTuple<TrackVisual, GameEntity>(new TrackVisual(track), gameEntity));
			SandBoxViewSubModule.VisualsOfEntities.Add(this._trackVisuals[track].Item2.Pointer, this._trackVisuals[track].Item1);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00025D50 File Offset: 0x00023F50
		private void OnTrackLost(Track track)
		{
			this._tracksDirty = true;
			ValueTuple<TrackVisual, GameEntity> valueTuple = this._trackVisuals[track];
			this._trackVisuals.Remove(track);
			SandBoxViewSubModule.VisualsOfEntities.Remove(valueTuple.Item2.Pointer);
			this.ReleaseEntity(valueTuple.Item2);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00025DA0 File Offset: 0x00023FA0
		private void ParallelUpdateTrackColors(Track track)
		{
			(this._trackVisuals[track].Item2.GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal).SetFactor1(Campaign.Current.Models.MapTrackModel.GetTrackColor(track));
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00025DDC File Offset: 0x00023FDC
		private void ParallelUpdateTrackColors(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.ParallelUpdateTrackColors(MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i]);
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00025E10 File Offset: 0x00024010
		private void UpdateTrackMesh()
		{
			TWParallel.For(0, MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count, this._parallelUpdateVisibleTracksPredicate, 16);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00025E34 File Offset: 0x00024034
		private void UpdateTrackPoolPosition(Track track)
		{
			MatrixFrame matrixFrame = this.CalculateTrackFrame(track);
			this._trackVisuals[track].Item2.SetFrame(ref matrixFrame, true);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00025E62 File Offset: 0x00024062
		private void ParallelUpdateVisibleTracks(Track track)
		{
			this._trackVisuals[track].Item2.SetVisibilityExcludeParents(true);
			this.UpdateTrackPoolPosition(track);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00025E84 File Offset: 0x00024084
		private void ParallelUpdateVisibleTracks(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.ParallelUpdateVisibleTracks(MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i]);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00025EB8 File Offset: 0x000240B8
		private bool RaySphereIntersection(Ray ray, SphereData sphere, ref Vec3 intersectionPoint)
		{
			Vec3 origin = sphere.Origin;
			float radius = sphere.Radius;
			Vec3 vec = origin - ray.Origin;
			float num = Vec3.DotProduct(ray.Direction, vec);
			if (num > 0f)
			{
				Vec3 vec2 = ray.Origin + ray.Direction * num - origin;
				float num2 = radius * radius - vec2.LengthSquared;
				if (num2 >= 0f)
				{
					float num3 = MathF.Sqrt(num2);
					float num4 = num - num3;
					if (num4 >= 0f && num4 <= ray.MaxDistance)
					{
						intersectionPoint = ray.Origin + ray.Direction * num4;
						return true;
					}
					if (num4 < 0f)
					{
						intersectionPoint = ray.Origin;
						return true;
					}
				}
			}
			else if ((ray.Origin - origin).LengthSquared < radius * radius)
			{
				intersectionPoint = ray.Origin;
				return true;
			}
			return false;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00025FBC File Offset: 0x000241BC
		private Track GetTrackOnMouse(Ray mouseRay, Vec3 mouseIntersectionPoint)
		{
			Track track = null;
			for (int i = 0; i < MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count; i++)
			{
				Track track2 = MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i];
				float trackScale = Campaign.Current.Models.MapTrackModel.GetTrackScale(track2);
				MatrixFrame matrixFrame = this.CalculateTrackFrame(track2);
				float lengthSquared = (matrixFrame.origin - mouseIntersectionPoint).LengthSquared;
				if (lengthSquared < 0.1f)
				{
					float num = MathF.Sqrt(lengthSquared);
					this._trackSphere.Origin = matrixFrame.origin;
					this._trackSphere.Radius = 0.05f + num * 0.01f + trackScale;
					Vec3 vec = default(Vec3);
					if (this.RaySphereIntersection(mouseRay, this._trackSphere, ref vec))
					{
						track = track2;
						break;
					}
				}
			}
			return track;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0002609C File Offset: 0x0002429C
		private MatrixFrame CalculateTrackFrame(Track track)
		{
			Vec3 vec = track.Position.AsVec3();
			float scale = track.Scale;
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = vec;
			float num;
			Vec3 vec2;
			Campaign.Current.MapSceneWrapper.GetTerrainHeightAndNormal(identity.origin.AsVec2, out num, out vec2);
			identity.rotation.u = vec2;
			Vec2 asVec = identity.rotation.f.AsVec2;
			asVec.RotateCCW(track.Direction);
			identity.rotation.f = new Vec3(asVec.x, asVec.y, identity.rotation.f.z, -1f);
			identity.rotation.s = Vec3.CrossProduct(identity.rotation.f, identity.rotation.u);
			identity.rotation.s.Normalize();
			identity.rotation.f = Vec3.CrossProduct(identity.rotation.u, identity.rotation.s);
			identity.rotation.f.Normalize();
			float num2 = scale;
			identity.rotation.s = identity.rotation.s * num2;
			identity.rotation.f = identity.rotation.f * num2;
			identity.rotation.u = identity.rotation.u * num2;
			return identity;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00026218 File Offset: 0x00024418
		private GameEntity GetGameEntity()
		{
			Stack<GameEntity> entityPool = this._entityPool;
			if (entityPool.Count != 0)
			{
				return entityPool.Pop();
			}
			GameEntity gameEntity = GameEntity.Instantiate(base.MapScene, "map_track_arrow", MatrixFrame.Identity, true);
			gameEntity.SetVisibilityExcludeParents(false);
			return gameEntity;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00026258 File Offset: 0x00024458
		private void PopulateEntityPool()
		{
			for (int i = 0; i < 256; i++)
			{
				GameEntity gameEntity = GameEntity.Instantiate(base.MapScene, "map_track_arrow", MatrixFrame.Identity, true);
				gameEntity.SetVisibilityExcludeParents(false);
				this._entityPool.Push(gameEntity);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002629F File Offset: 0x0002449F
		private void ReleaseEntity(GameEntity e)
		{
			e.SetVisibilityExcludeParents(false);
			if (this._entityPool == null)
			{
				this._entityPool = new Stack<GameEntity>();
			}
			this._entityPool.Push(e);
		}

		// Token: 0x04000226 RID: 550
		private const string TrackPrefabName = "map_track_arrow";

		// Token: 0x04000227 RID: 551
		private const int DefaultObjectPoolCount = 256;

		// Token: 0x04000228 RID: 552
		private Dictionary<Track, ValueTuple<TrackVisual, GameEntity>> _trackVisuals;

		// Token: 0x04000229 RID: 553
		private SphereData _trackSphere;

		// Token: 0x0400022A RID: 554
		private bool _tracksDirty = true;

		// Token: 0x0400022B RID: 555
		private readonly TWParallel.ParallelForAuxPredicate _parallelUpdateTrackColorsPredicate;

		// Token: 0x0400022C RID: 556
		private readonly TWParallel.ParallelForAuxPredicate _parallelUpdateVisibleTracksPredicate;

		// Token: 0x0400022D RID: 557
		private Stack<GameEntity> _entityPool;
	}
}
