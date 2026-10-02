using System;
using System.Diagnostics;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200007E RID: 126
	[EngineClass("rglResource")]
	public abstract class Resource : NativeObject
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0000B1BA File Offset: 0x000093BA
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0000B1CC File Offset: 0x000093CC
		protected Resource()
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0000B1D4 File Offset: 0x000093D4
		internal Resource(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0000B1E3 File Offset: 0x000093E3
		[Conditional("_RGL_KEEP_ASSERTS")]
		protected void CheckResourceParameter(Resource param, string paramName = "")
		{
			if (param == null)
			{
				throw new NullReferenceException(paramName);
			}
			if (!param.IsValid)
			{
				throw new ArgumentException(paramName);
			}
		}
	}
}
