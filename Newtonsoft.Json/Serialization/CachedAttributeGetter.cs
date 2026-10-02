using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006F RID: 111
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x00018BD0 File Offset: 0x00016DD0
		[NullableContext(1)]
		[return: Nullable(2)]
		public static T GetAttribute(object type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x0400020E RID: 526
		[Nullable(new byte[] { 1, 1, 2 })]
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
