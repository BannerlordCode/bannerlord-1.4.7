using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000218 RID: 536
	public class SpawnPathData
	{
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0006BB69 File Offset: 0x00069D69
		public bool IsValid
		{
			get
			{
				return this.Scene != null && this.Path != null && this.Path.NumberOfPoints > 1;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0006BB97 File Offset: 0x00069D97
		public int FreeSegmentCount
		{
			get
			{
				return this._freeSegments.Count;
			}
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0006BBA4 File Offset: 0x00069DA4
		public SpawnPathData Invert()
		{
			float num = this.PathLength - this.PivotOffset;
			return new SpawnPathData(this.Scene, this.Path, MathF.Max(num, 0f), !this.IsInverted, this.SnapType);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0006BBEC File Offset: 0x00069DEC
		public void ClampPathOffset(ref float relativePathOffset)
		{
			float num = MathF.Clamp(this.PivotOffset + relativePathOffset, 0f, this.PathLength) - this.PivotOffset;
			relativePathOffset = num;
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0006BC20 File Offset: 0x00069E20
		public float ConvertPointToRelativePathOffset(int pointIndex)
		{
			float num = 0f;
			for (int i = 0; i < pointIndex; i++)
			{
				num += this.Path.GetArcLength(i);
			}
			num = MathF.Clamp(num, 0f, this.PathLength);
			num = (this.IsInverted ? (this.PathLength - num) : num);
			return num - this.PivotOffset;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0006BC7C File Offset: 0x00069E7C
		public float ConvertRelativePathOffsetToPathDistance(float relativePathOffset)
		{
			float num = MathF.Clamp(this.PivotOffset + relativePathOffset, 0f, this.PathLength);
			return this.IsInverted ? (this.PathLength - num) : num;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0006BCB8 File Offset: 0x00069EB8
		public int GetNodeIndexAtPathDistance(float pathDistance)
		{
			int num = this.Path.NumberOfPoints - 2;
			float num2 = 0f;
			for (int i = 0; i < this.Path.NumberOfPoints - 1; i++)
			{
				float arcLength = this.Path.GetArcLength(i);
				if (pathDistance >= num2 && pathDistance < num2 + arcLength)
				{
					num = i;
					break;
				}
				num2 += arcLength;
			}
			return num;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0006BD11 File Offset: 0x00069F11
		public float GetBaseOffset()
		{
			if (this.IsInverted)
			{
				return this.PivotOffset - this.PathLength + 1f;
			}
			return -this.PivotOffset + 1f;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0006BD3C File Offset: 0x00069F3C
		public bool IsPathOffsetValid(float relativePathOffset)
		{
			float num = this.ConvertRelativePathOffsetToPathDistance(relativePathOffset);
			int nodeIndexAtPathDistance = this.GetNodeIndexAtPathDistance(num);
			int num2 = nodeIndexAtPathDistance + 1;
			return this.Path.HasValidAlphaAtPathPoint(nodeIndexAtPathDistance, 0.5f) || this.Path.HasValidAlphaAtPathPoint(num2, 0.5f);
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0006BD84 File Offset: 0x00069F84
		public float GetOffsetOverflow(float relativePathOffset)
		{
			float num = this.PivotOffset + relativePathOffset;
			if (num < 0f)
			{
				return num;
			}
			if (num > this.PathLength)
			{
				return num - this.PathLength;
			}
			return 0f;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0006BDBC File Offset: 0x00069FBC
		public MatrixFrame GetSpawnFrame(float relativePathOffset, bool searchNearestValidFrame = false, SpawnPathData.SearchDirection searchDirection = SpawnPathData.SearchDirection.Backward)
		{
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			float num = this.ConvertRelativePathOffsetToPathDistance(relativePathOffset);
			if (searchNearestValidFrame)
			{
				bool flag = searchDirection == SpawnPathData.SearchDirection.Forward;
				flag = (this.IsInverted ? (!flag) : flag);
				matrixFrame = this.Path.GetNearestFrameWithValidAlphaForDistance(num, flag, 0.5f);
			}
			else
			{
				matrixFrame = this.Path.GetFrameForDistance(num);
			}
			matrixFrame.rotation.f = (this.IsInverted ? (-matrixFrame.rotation.f) : matrixFrame.rotation.f);
			matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			if (this.SnapType != SpawnPathData.SnapMethod.DontSnap)
			{
				if (this.SnapType == SpawnPathData.SnapMethod.SnapToTerrain)
				{
					matrixFrame.origin.z = this.Scene.GetTerrainHeight(matrixFrame.origin.AsVec2, true);
				}
				else if (this.SnapType == SpawnPathData.SnapMethod.SnapToWaterLevel)
				{
					matrixFrame.origin.z = this.Scene.GetWaterLevel();
				}
			}
			return matrixFrame;
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0006BEAC File Offset: 0x0006A0AC
		public void GetSpawnPathFrameFacingTarget(float basePathOffset, float targetPathOffset, bool useTangentDirection, out Vec2 spawnPathPosition, out Vec2 spawnPathDirection, bool decideDirectionDynamically = false, float dynamicDistancePercentage = 0.2f)
		{
			this.ClampPathOffset(ref basePathOffset);
			this.ClampPathOffset(ref targetPathOffset);
			MatrixFrame spawnFrame = this.GetSpawnFrame(basePathOffset, false, SpawnPathData.SearchDirection.Backward);
			float num = 0.01f;
			spawnPathPosition = spawnFrame.origin.AsVec2;
			if (MBMath.ApproximatelyEquals(basePathOffset, targetPathOffset, num))
			{
				if (MBMath.ApproximatelyEquals(basePathOffset, 0f, num))
				{
					spawnPathDirection = this.GetSpawnFrame(0f, false, SpawnPathData.SearchDirection.Backward).rotation.f.AsVec2.Normalized();
					return;
				}
				if (useTangentDirection)
				{
					spawnPathDirection = this.GetSpawnPathTangentDirection(basePathOffset, in spawnFrame, 0f);
					return;
				}
				spawnPathDirection = (this.GetSpawnFrame(0f, false, SpawnPathData.SearchDirection.Backward).origin.AsVec2 - spawnPathPosition).Normalized();
				return;
			}
			else
			{
				MatrixFrame spawnFrame2 = this.GetSpawnFrame(targetPathOffset, false, SpawnPathData.SearchDirection.Backward);
				if (decideDirectionDynamically)
				{
					WorldPosition worldPosition = new WorldPosition(this.Scene, spawnFrame.origin);
					WorldPosition worldPosition2 = new WorldPosition(this.Scene, spawnFrame2.origin);
					float num2;
					if (this.Scene.GetPathDistanceBetweenPositions(ref worldPosition, ref worldPosition2, 0.1f, out num2))
					{
						float length = (spawnFrame2.origin - spawnFrame.origin).Length;
						useTangentDirection = num2 >= length * (1f + dynamicDistancePercentage);
					}
				}
				if (useTangentDirection)
				{
					spawnPathDirection = this.GetSpawnPathTangentDirection(basePathOffset, in spawnFrame, targetPathOffset);
					return;
				}
				spawnPathDirection = (spawnFrame2.origin.AsVec2 - spawnPathPosition).Normalized();
				return;
			}
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0006C044 File Offset: 0x0006A244
		private Vec2 GetSpawnPathTangentDirection(float onPathOffset, in MatrixFrame onPathFrame, float referenceOffset)
		{
			Vec3 vec = onPathFrame.origin;
			Vec2 asVec = vec.AsVec2;
			if (MBMath.ApproximatelyEquals(onPathOffset, referenceOffset, 1E-05f))
			{
				vec = onPathFrame.rotation.f;
				return vec.AsVec2.Normalized();
			}
			int num = ((onPathOffset > referenceOffset) ? (-1) : 1);
			float num2 = onPathOffset + (float)num * 1f;
			this.ClampPathOffset(ref num2);
			Vec2 vec2 = this.GetSpawnFrame(num2, false, SpawnPathData.SearchDirection.Backward).origin.AsVec2 - asVec;
			Vec2 vec3;
			if (vec2.LengthSquared < 1E-06f)
			{
				float num3 = (float)num;
				vec = onPathFrame.rotation.f;
				vec3 = num3 * vec.AsVec2.Normalized();
			}
			else
			{
				vec3 = vec2.Normalized();
			}
			return vec3;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0006C108 File Offset: 0x0006A308
		private SpawnPathData(Scene scene, Path path, float pivotOffset, bool isInverted = false, SpawnPathData.SnapMethod snapType = SpawnPathData.SnapMethod.DontSnap)
		{
			this.Scene = scene;
			this.Path = path;
			this.PathLength = this.Path.GetTotalLength();
			this.PivotOffset = MathF.Clamp(pivotOffset, 1f, this.PathLength - 1f);
			this.IsInverted = isInverted;
			this.SnapType = snapType;
			this.BuildFreeSegments();
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x0006C178 File Offset: 0x0006A378
		private void BuildFreeSegments()
		{
			if (this.IsValid)
			{
				int numberOfPoints = this.Path.NumberOfPoints;
				float[] array = new float[numberOfPoints];
				for (int i = 0; i < numberOfPoints; i++)
				{
					array[i] = this.ConvertPointToRelativePathOffset(i);
				}
				bool flag = false;
				float num = 0f;
				float num2 = 0f;
				int num3 = 0;
				int num4 = numberOfPoints - 1;
				int num5 = 1;
				if (this.IsInverted)
				{
					num3 = numberOfPoints - 1;
					num4 = 0;
					num5 = -1;
				}
				for (int num6 = num3; num6 != num4; num6 += num5)
				{
					int num7 = num6 + num5;
					bool flag2 = this.Path.HasValidAlphaAtPathPoint(num6, 0.5f) || this.Path.HasValidAlphaAtPathPoint(num7, 0.5f);
					float num8 = array[num6];
					float num9 = array[num7];
					if (!flag2)
					{
						if (flag)
						{
							float num10 = num + 0.001f;
							float num11 = num2 - 0.001f;
							if (num11 > num10)
							{
								this.ClampPathOffset(ref num10);
								this.ClampPathOffset(ref num11);
								this._freeSegments.Add(new ValueTuple<float, float>(num10, num11));
							}
							flag = false;
						}
					}
					else if (!flag)
					{
						num = num8;
						num2 = num9;
						flag = true;
					}
					else
					{
						num2 = num9;
					}
				}
				if (flag)
				{
					float num12 = num + 0.001f;
					float num13 = num2 - 0.001f;
					if (num13 > num12)
					{
						this.ClampPathOffset(ref num12);
						this.ClampPathOffset(ref num13);
						if (num13 > num12)
						{
							this._freeSegments.Add(new ValueTuple<float, float>(num12, num13));
						}
					}
				}
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0006C2DF File Offset: 0x0006A4DF
		public static SpawnPathData Create(Scene scene, Path path, float pivotOffset, bool isInverted = false, SpawnPathData.SnapMethod snapType = SpawnPathData.SnapMethod.DontSnap)
		{
			return new SpawnPathData(scene, path, pivotOffset, isInverted, snapType);
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006C2EC File Offset: 0x0006A4EC
		[return: TupleElementNames(new string[] { "startOffset", "endOffset" })]
		internal ValueTuple<float, float> GetFreeSegment(int segmentIndex)
		{
			return this._freeSegments[segmentIndex];
		}

		// Token: 0x04000AA0 RID: 2720
		public const float MinimumSpawnPathOffset = 1f;

		// Token: 0x04000AA1 RID: 2721
		public readonly Scene Scene;

		// Token: 0x04000AA2 RID: 2722
		public readonly Path Path;

		// Token: 0x04000AA3 RID: 2723
		public readonly bool IsInverted;

		// Token: 0x04000AA4 RID: 2724
		public readonly float PivotOffset;

		// Token: 0x04000AA5 RID: 2725
		public readonly float PathLength;

		// Token: 0x04000AA6 RID: 2726
		public readonly SpawnPathData.SnapMethod SnapType;

		// Token: 0x04000AA7 RID: 2727
		[TupleElementNames(new string[] { "startOffset", "endOffset" })]
		private readonly MBList<ValueTuple<float, float>> _freeSegments = new MBList<ValueTuple<float, float>>();

		// Token: 0x0200051E RID: 1310
		public enum SnapMethod
		{
			// Token: 0x04001D13 RID: 7443
			DontSnap,
			// Token: 0x04001D14 RID: 7444
			SnapToTerrain,
			// Token: 0x04001D15 RID: 7445
			SnapToWaterLevel
		}

		// Token: 0x0200051F RID: 1311
		public enum SearchDirection
		{
			// Token: 0x04001D17 RID: 7447
			Forward,
			// Token: 0x04001D18 RID: 7448
			Backward
		}
	}
}
