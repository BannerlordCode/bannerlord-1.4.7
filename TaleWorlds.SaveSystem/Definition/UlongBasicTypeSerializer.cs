using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004D RID: 77
	internal class UlongBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000CD33 File Offset: 0x0000AF33
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteULong((ulong)value);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000CD41 File Offset: 0x0000AF41
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadULong();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000CD4E File Offset: 0x0000AF4E
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
