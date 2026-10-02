using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000048 RID: 72
	internal class ByteBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000CC75 File Offset: 0x0000AE75
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteByte((byte)value);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000CC83 File Offset: 0x0000AE83
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadByte();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000CC90 File Offset: 0x0000AE90
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
