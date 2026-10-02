using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004C RID: 76
	internal class LongBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000CD0D File Offset: 0x0000AF0D
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteLong((long)value);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000CD1B File Offset: 0x0000AF1B
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadLong();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000CD28 File Offset: 0x0000AF28
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
