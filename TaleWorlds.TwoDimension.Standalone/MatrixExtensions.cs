using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000A RID: 10
	public static class MatrixExtensions
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00004414 File Offset: 0x00002614
		public static Matrix4x4 ToMatrix4x4(this MatrixFrame matrixFrame)
		{
			return new Matrix4x4(matrixFrame.rotation.s.x, matrixFrame.rotation.s.y, matrixFrame.rotation.s.z, matrixFrame.rotation.s.w, matrixFrame.rotation.f.x, matrixFrame.rotation.f.y, matrixFrame.rotation.f.z, matrixFrame.rotation.f.w, matrixFrame.rotation.u.x, matrixFrame.rotation.u.y, matrixFrame.rotation.u.z, matrixFrame.rotation.u.w, matrixFrame.origin.x, matrixFrame.origin.y, matrixFrame.origin.z, matrixFrame.origin.w);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004514 File Offset: 0x00002714
		public static MatrixFrame ToMatrixFrame(this Matrix4x4 matrix)
		{
			return new MatrixFrame(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004588 File Offset: 0x00002788
		public static bool AreAllComponentsValid(this Matrix4x4 matrix)
		{
			return !float.IsNaN(matrix.M11) && !float.IsNaN(matrix.M12) && !float.IsNaN(matrix.M13) && !float.IsNaN(matrix.M14) && !float.IsNaN(matrix.M21) && !float.IsNaN(matrix.M22) && !float.IsNaN(matrix.M23) && !float.IsNaN(matrix.M24) && !float.IsNaN(matrix.M31) && !float.IsNaN(matrix.M32) && !float.IsNaN(matrix.M33) && !float.IsNaN(matrix.M34) && !float.IsNaN(matrix.M41) && !float.IsNaN(matrix.M42) && !float.IsNaN(matrix.M43) && !float.IsNaN(matrix.M44) && !float.IsInfinity(matrix.M11) && !float.IsInfinity(matrix.M12) && !float.IsInfinity(matrix.M13) && !float.IsInfinity(matrix.M14) && !float.IsInfinity(matrix.M21) && !float.IsInfinity(matrix.M22) && !float.IsInfinity(matrix.M23) && !float.IsInfinity(matrix.M24) && !float.IsInfinity(matrix.M31) && !float.IsInfinity(matrix.M32) && !float.IsInfinity(matrix.M33) && !float.IsInfinity(matrix.M34) && !float.IsInfinity(matrix.M41) && !float.IsInfinity(matrix.M42) && !float.IsInfinity(matrix.M43) && !float.IsInfinity(matrix.M44);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000477C File Offset: 0x0000297C
		public static bool AreAllComponentsValid(this MatrixFrame matrix)
		{
			return matrix.origin.IsValidXYZW && matrix.rotation.s.IsValidXYZW && matrix.rotation.f.IsValidXYZW && matrix.rotation.u.IsValidXYZW;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000047D0 File Offset: 0x000029D0
		public static MatrixFrame CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
		{
			return Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane).ToMatrixFrame();
		}
	}
}
