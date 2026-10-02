using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000DD RID: 221
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CustomCreationConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0003016D File Offset: 0x0002E36D
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0003017C File Offset: 0x0002E37C
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000C16 RID: 3094
		public abstract T Create(Type objectType);

		// Token: 0x06000C17 RID: 3095 RVA: 0x000301C4 File Offset: 0x0002E3C4
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000301D6 File Offset: 0x0002E3D6
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
