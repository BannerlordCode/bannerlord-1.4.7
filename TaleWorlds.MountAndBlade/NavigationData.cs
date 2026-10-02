using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E1 RID: 737
	[EngineStruct("Navigation_data", false, null)]
	[Serializable]
	public struct NavigationData
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x000A418C File Offset: 0x000A238C
		public NavigationData(Vec3 startPoint, Vec3 endPoint, float agentRadius)
		{
			this.Points = new Vec2[1024];
			this.StartPoint = startPoint;
			this.EndPoint = endPoint;
			this.Points[0] = startPoint.AsVec2;
			this.Points[1] = endPoint.AsVec2;
			this.PointSize = 2;
			this.AgentRadius = agentRadius;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000A41EC File Offset: 0x000A23EC
		[Conditional("DEBUG")]
		public void TickDebug()
		{
			for (int i = 0; i < this.PointSize - 1; i++)
			{
			}
		}

		// Token: 0x04001039 RID: 4153
		private const int MaxPathSize = 1024;

		// Token: 0x0400103A RID: 4154
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public Vec2[] Points;

		// Token: 0x0400103B RID: 4155
		public Vec3 StartPoint;

		// Token: 0x0400103C RID: 4156
		public Vec3 EndPoint;

		// Token: 0x0400103D RID: 4157
		public readonly int PointSize;

		// Token: 0x0400103E RID: 4158
		public readonly float AgentRadius;
	}
}
