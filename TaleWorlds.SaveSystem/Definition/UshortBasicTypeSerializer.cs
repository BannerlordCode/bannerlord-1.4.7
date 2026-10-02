using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000047 RID: 71
	internal class UshortBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000CC4F File Offset: 0x0000AE4F
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteUShort((ushort)value);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000CC5D File Offset: 0x0000AE5D
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadUShort();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 2;
		}
	}
}
