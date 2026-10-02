using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000074 RID: 116
	[EngineClass("rglPath")]
	public sealed class Path : NativeObject
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0000ABC7 File Offset: 0x00008DC7
		public int NumberOfPoints
		{
			get
			{
				return EngineApplicationInterface.IPath.GetNumberOfPoints(base.Pointer);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0000ABD9 File Offset: 0x00008DD9
		public float TotalDistance
		{
			get
			{
				return EngineApplicationInterface.IPath.GetTotalLength(base.Pointer);
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0000ABEB File Offset: 0x00008DEB
		internal Path(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0000ABFC File Offset: 0x00008DFC
		public MatrixFrame GetHermiteFrameForDt(float phase, int first_point)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameForDt(base.Pointer, ref identity, phase, first_point);
			return identity;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0000AC24 File Offset: 0x00008E24
		public MatrixFrame GetFrameForDistance(float distance)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameForDistance(base.Pointer, ref identity, distance);
			return identity;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0000AC4C File Offset: 0x00008E4C
		public MatrixFrame GetNearestFrameWithValidAlphaForDistance(float distance, bool searchForward = true, float alphaThreshold = 0.5f)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetNearestHermiteFrameWithValidAlphaForDistance(base.Pointer, ref identity, distance, searchForward, alphaThreshold);
			return identity;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0000AC75 File Offset: 0x00008E75
		public void GetFrameAndColorForDistance(float distance, out MatrixFrame frame, out Vec3 color)
		{
			frame = MatrixFrame.Identity;
			EngineApplicationInterface.IPath.GetHermiteFrameAndColorForDistance(base.Pointer, out frame, out color, distance);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0000AC95 File Offset: 0x00008E95
		public float GetArcLength(int first_point)
		{
			return EngineApplicationInterface.IPath.GetArcLength(base.Pointer, first_point);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		public void GetPoints(MatrixFrame[] points)
		{
			EngineApplicationInterface.IPath.GetPoints(base.Pointer, points);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0000ACBB File Offset: 0x00008EBB
		public float GetTotalLength()
		{
			return EngineApplicationInterface.IPath.GetTotalLength(base.Pointer);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0000ACCD File Offset: 0x00008ECD
		public int GetVersion()
		{
			return EngineApplicationInterface.IPath.GetVersion(base.Pointer);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0000ACDF File Offset: 0x00008EDF
		public void SetFrameOfPoint(int pointIndex, ref MatrixFrame frame)
		{
			EngineApplicationInterface.IPath.SetFrameOfPoint(base.Pointer, pointIndex, ref frame);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0000ACF3 File Offset: 0x00008EF3
		public void SetTangentPositionOfPoint(int pointIndex, int tangentIndex, ref Vec3 position)
		{
			EngineApplicationInterface.IPath.SetTangentPositionOfPoint(base.Pointer, pointIndex, tangentIndex, ref position);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0000AD08 File Offset: 0x00008F08
		public int AddPathPoint(int newNodeIndex)
		{
			return EngineApplicationInterface.IPath.AddPathPoint(base.Pointer, newNodeIndex);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0000AD1B File Offset: 0x00008F1B
		public void DeletePathPoint(int nodeIndex)
		{
			EngineApplicationInterface.IPath.DeletePathPoint(base.Pointer, nodeIndex);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0000AD2E File Offset: 0x00008F2E
		public bool HasValidAlphaAtPathPoint(int nodeIndex, float alphaThreshold = 0.5f)
		{
			return EngineApplicationInterface.IPath.HasValidAlphaAtPathPoint(base.Pointer, nodeIndex, alphaThreshold);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0000AD42 File Offset: 0x00008F42
		public string GetName()
		{
			return EngineApplicationInterface.IPath.GetName(base.Pointer);
		}
	}
}
