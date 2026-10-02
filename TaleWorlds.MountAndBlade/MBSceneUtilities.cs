using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000251 RID: 593
	public static class MBSceneUtilities
	{
		// Token: 0x060021B5 RID: 8629 RVA: 0x00075EEC File Offset: 0x000740EC
		public static MBList<Path> GetAllSpawnPaths(Scene scene)
		{
			MBList<Path> mblist = new MBList<Path>();
			for (int i = 0; i < 32; i++)
			{
				string text = "spawn_path_" + i.ToString("D2");
				Path pathWithName = scene.GetPathWithName(text);
				if (pathWithName != null && pathWithName.NumberOfPoints > 1)
				{
					mblist.Add(pathWithName);
				}
			}
			return mblist;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00075F48 File Offset: 0x00074148
		public static MBList<Vec2> GetSoftBoundaryPoints(Scene scene)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			int softBoundaryVertexCount = scene.GetSoftBoundaryVertexCount();
			if (softBoundaryVertexCount > 2)
			{
				for (int i = 0; i < softBoundaryVertexCount; i++)
				{
					Vec2 softBoundaryVertex = scene.GetSoftBoundaryVertex(i);
					mblist.Add(softBoundaryVertex);
				}
			}
			return mblist;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00075F84 File Offset: 0x00074184
		public static MBList<Vec2> GetHardBoundaryPoints(Scene scene)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			int hardBoundaryVertexCount = scene.GetHardBoundaryVertexCount();
			for (int i = 0; i < hardBoundaryVertexCount; i++)
			{
				Vec2 hardBoundaryVertex = scene.GetHardBoundaryVertex(i);
				mblist.Add(hardBoundaryVertex);
			}
			return mblist;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00075FBC File Offset: 0x000741BC
		public static MBList<Vec2> GetSceneLimitPoints(Scene scene, out Vec2 sceneLimitMin, out Vec2 sceneLimitMax)
		{
			MBList<Vec2> mblist = new MBList<Vec2>();
			Vec3 vec;
			Vec3 vec2;
			scene.GetSceneLimits(out vec, out vec2);
			mblist.Add(new Vec2(vec.x, vec.y));
			mblist.Add(new Vec2(vec2.x, vec.y));
			mblist.Add(new Vec2(vec2.x, vec2.y));
			mblist.Add(new Vec2(vec.x, vec2.y));
			sceneLimitMin = vec.AsVec2;
			sceneLimitMax = vec2.AsVec2;
			return mblist;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00076050 File Offset: 0x00074250
		[return: TupleElementNames(new string[] { "tag", "boundaryPoints", "insideAllowance" })]
		public static MBList<ValueTuple<string, MBList<Vec2>, bool>> GetDeploymentBoundaries(BattleSideEnum battleSide)
		{
			IEnumerable<GameEntity> enumerable = Mission.Current.Scene.FindEntitiesWithTagExpression("deployment_castle_boundary(_\\d+)*");
			List<ValueTuple<string, List<GameEntity>>> list = new List<ValueTuple<string, List<GameEntity>>>();
			foreach (GameEntity gameEntity in enumerable)
			{
				if (gameEntity.HasTag(battleSide.ToString()))
				{
					string[] tags = gameEntity.Tags;
					for (int i = 0; i < tags.Length; i++)
					{
						string tag = tags[i];
						if (tag.Contains("deployment_castle_boundary"))
						{
							ValueTuple<string, List<GameEntity>> valueTuple = list.FirstOrDefault<ValueTuple<string, List<GameEntity>>>(([TupleElementNames(new string[] { "tag", "boundaryEntities" })] ValueTuple<string, List<GameEntity>> tuple) => tuple.Item1.Equals(tag));
							if (valueTuple.Item1 == null)
							{
								valueTuple = new ValueTuple<string, List<GameEntity>>(tag, new List<GameEntity>());
								list.Add(valueTuple);
							}
							valueTuple.Item2.Add(gameEntity);
							break;
						}
					}
				}
			}
			MBList<ValueTuple<string, MBList<Vec2>, bool>> mblist = new MBList<ValueTuple<string, MBList<Vec2>, bool>>();
			foreach (ValueTuple<string, List<GameEntity>> valueTuple2 in list)
			{
				string item = valueTuple2.Item1;
				bool flag = !valueTuple2.Item2.Any<GameEntity>((GameEntity e) => e.HasTag("out"));
				MBList<Vec2> mblist2 = valueTuple2.Item2.Select<GameEntity, Vec2>((GameEntity bp) => bp.GlobalPosition.AsVec2).ToMBList<Vec2>();
				MBSceneUtilities.RadialSortBoundary(ref mblist2);
				mblist.Add(new ValueTuple<string, MBList<Vec2>, bool>(item, mblist2, flag));
			}
			return mblist;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00076214 File Offset: 0x00074414
		public static void GetAxisAlignedBoundaryRectangle(List<Vec2> boundaryPoints, out Vec2 boundsMin, out Vec2 boundsMax)
		{
			boundsMin = new Vec2(float.MaxValue, float.MaxValue);
			boundsMax = new Vec2(float.MinValue, float.MinValue);
			for (int i = 0; i < boundaryPoints.Count; i++)
			{
				Vec2 vec = boundaryPoints[i];
				if (vec.x < boundsMin.X)
				{
					boundsMin.x = vec.x;
				}
				if (vec.y < boundsMin.Y)
				{
					boundsMin.y = vec.y;
				}
				if (vec.x > boundsMax.X)
				{
					boundsMax.x = vec.x;
				}
				if (vec.y > boundsMax.Y)
				{
					boundsMax.y = vec.y;
				}
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000762CC File Offset: 0x000744CC
		public static void FindConvexHull(ref MBList<Vec2> boundary)
		{
			Vec2[] array = boundary.ToArray();
			int num = 0;
			MBAPI.IMBMission.FindConvexHull(array, boundary.Count, ref num);
			boundary = array.ToMBList<Vec2>();
			boundary.RemoveRange(num, boundary.Count - num);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00076310 File Offset: 0x00074510
		public static void RadialSortBoundary(ref MBList<Vec2> boundary)
		{
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals1 = new MBSceneUtilities.<>c__DisplayClass18_0();
			if (boundary.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.boundaryCenter = Vec2.Zero;
			foreach (Vec2 vec in boundary)
			{
				CS$<>8__locals1.boundaryCenter += vec;
			}
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.boundaryCenter.x = CS$<>8__locals2.boundaryCenter.x / (float)boundary.Count;
			MBSceneUtilities.<>c__DisplayClass18_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.boundaryCenter.y = CS$<>8__locals3.boundaryCenter.y / (float)boundary.Count;
			boundary = boundary.OrderBy<Vec2, float>((Vec2 b) => (b - CS$<>8__locals1.boundaryCenter).RotationInRadians).ToMBList<Vec2>();
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000763D0 File Offset: 0x000745D0
		public static void RadialSortBoundary(ref MBList<Vec3> boundary)
		{
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals1 = new MBSceneUtilities.<>c__DisplayClass19_0();
			if (boundary.Count == 0)
			{
				return;
			}
			CS$<>8__locals1.boundaryCenter = Vec2.Zero;
			foreach (Vec3 vec in boundary)
			{
				CS$<>8__locals1.boundaryCenter += vec.AsVec2;
			}
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.boundaryCenter.x = CS$<>8__locals2.boundaryCenter.x / (float)boundary.Count;
			MBSceneUtilities.<>c__DisplayClass19_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.boundaryCenter.y = CS$<>8__locals3.boundaryCenter.y / (float)boundary.Count;
			boundary = boundary.OrderBy<Vec3, float>((Vec3 b) => (b.AsVec2 - CS$<>8__locals1.boundaryCenter).RotationInRadians).ToMBList<Vec3>();
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00076494 File Offset: 0x00074694
		public static bool IsConvexAndRadiallySorted(MBList<Vec2> boundary)
		{
			int count = boundary.Count;
			if (count < 3)
			{
				return false;
			}
			Vec2 vec = new Vec2(0f, 0f);
			foreach (Vec2 vec2 in boundary)
			{
				vec += vec2;
			}
			vec /= (float)count;
			Vec2 vec3 = (boundary[0] - vec).Normalized();
			vec3.RotateCCW(-0.001f);
			Vec2 vec4 = vec3;
			for (int i = 0; i < count; i++)
			{
				Vec2 vec5 = boundary[i];
				vec3 = (vec5 - vec).Normalized();
				if (vec4.AngleBetween(vec3) <= 0f)
				{
					return false;
				}
				vec4 = vec3;
				Vec2 vec6 = boundary[(i + 1) % count];
				Vec2 vec7 = boundary[(i + 2) % count];
				Vec2 vec8 = vec6 - vec5;
				Vec2 vec9 = vec7 - vec5;
				if (Vec2.Determinant(in vec8, in vec9) < 0f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000765B4 File Offset: 0x000747B4
		public static bool IsPointInsideBoundaries(in Vec2 point, MBList<Vec2> boundaries, float acceptanceThreshold = 0.05f)
		{
			if (boundaries.Count <= 2)
			{
				return false;
			}
			acceptanceThreshold = MathF.Max(0f, acceptanceThreshold);
			bool flag = true;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count] - vec;
				Vec2 vec3 = point - vec;
				if (vec2.x * vec3.y - vec2.y * vec3.x < 0f)
				{
					vec2.Normalize();
					Vec2 vec4 = vec3.DotProduct(vec2) * vec2;
					if ((vec3 - vec4).LengthSquared > acceptanceThreshold * acceptanceThreshold)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x00076678 File Offset: 0x00074878
		public static float FindClosestPointToBoundaries(in Vec2 position, MBList<Vec2> boundaries, out Vec2 closestPoint)
		{
			closestPoint = position;
			float num = float.MaxValue;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count];
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec, in vec2, in position);
				Vec2 vec3 = position;
				float num2 = vec3.DistanceSquared(closestPointOnLineSegmentToPoint);
				if (num2 <= num)
				{
					num = num2;
					closestPoint = closestPointOnLineSegmentToPoint;
				}
			}
			return MathF.Sqrt(num);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000766F8 File Offset: 0x000748F8
		public static float FindClosestPointToBoundariesReturnDistanceSquared(in Vec2 position, MBList<Vec2> boundaries, out Vec2 closestPoint, out bool isPositionInsideBoundaries)
		{
			closestPoint = position;
			float num = float.MaxValue;
			for (int i = 0; i < boundaries.Count; i++)
			{
				Vec2 vec = boundaries[i];
				Vec2 vec2 = boundaries[(i + 1) % boundaries.Count];
				Vec2 closestPointOnLineSegmentToPoint = MBMath.GetClosestPointOnLineSegmentToPoint(in vec, in vec2, in position);
				Vec2 vec3 = position;
				float num2 = vec3.DistanceSquared(closestPointOnLineSegmentToPoint);
				if (num2 <= num)
				{
					num = num2;
					closestPoint = closestPointOnLineSegmentToPoint;
				}
			}
			isPositionInsideBoundaries = MBSceneUtilities.IsPointInsideBoundaries(in position, boundaries, 0.05f);
			return MathF.Sqrt(num);
		}

		// Token: 0x04000CFD RID: 3325
		public const int MaxNumberOfSpawnPaths = 32;

		// Token: 0x04000CFE RID: 3326
		public const string SpawnPathPrefix = "spawn_path_";

		// Token: 0x04000CFF RID: 3327
		public const string SoftBorderVertexTag = "walk_area_vertex";

		// Token: 0x04000D00 RID: 3328
		public const string HardBorderVertexTag = "walk_area_vertex_hard";

		// Token: 0x04000D01 RID: 3329
		public const string SoftBoundaryName = "walk_area";

		// Token: 0x04000D02 RID: 3330
		public const string SceneBoundaryName = "scene_boundary";

		// Token: 0x04000D03 RID: 3331
		public const float SceneToHardBoundaryMargin = 100f;

		// Token: 0x04000D04 RID: 3332
		public const string DefenderDeploymentReferencePositionTag = "defender_infantry";

		// Token: 0x04000D05 RID: 3333
		public const string AttackerDeploymentReferencePositionTag = "attacker_infantry";

		// Token: 0x04000D06 RID: 3334
		private const string DeploymentBoundaryTag = "deployment_castle_boundary";

		// Token: 0x04000D07 RID: 3335
		private const string DeploymentBoundaryTagExpression = "deployment_castle_boundary(_\\d+)*";
	}
}
