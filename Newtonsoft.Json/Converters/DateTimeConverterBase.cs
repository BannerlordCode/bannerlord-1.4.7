using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E0 RID: 224
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000C24 RID: 3108 RVA: 0x00030784 File Offset: 0x0002E984
		[NullableContext(1)]
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
