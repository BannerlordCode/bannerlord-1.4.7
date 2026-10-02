using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000058 RID: 88
	[NullableContext(2)]
	[Nullable(0)]
	internal class FSharpFunction
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x000157AD File Offset: 0x000139AD
		public FSharpFunction(object instance, [Nullable(new byte[] { 1, 2, 1 })] MethodCall<object, object> invoker)
		{
			this._instance = instance;
			this._invoker = invoker;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000157C3 File Offset: 0x000139C3
		[NullableContext(1)]
		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}

		// Token: 0x040001CA RID: 458
		private readonly object _instance;

		// Token: 0x040001CB RID: 459
		[Nullable(new byte[] { 1, 2, 1 })]
		private readonly MethodCall<object, object> _invoker;
	}
}
