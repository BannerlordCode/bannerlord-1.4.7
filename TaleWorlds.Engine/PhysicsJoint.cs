using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000076 RID: 118
	public sealed class PhysicsJoint
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0000AE74 File Offset: 0x00009074
		internal UIntPtr Pointer
		{
			get
			{
				return this._pointer;
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0000AE7C File Offset: 0x0000907C
		internal PhysicsJoint(UIntPtr ptr)
		{
			this._pointer = ptr;
		}

		// Token: 0x04000163 RID: 355
		private readonly UIntPtr _pointer;
	}
}
