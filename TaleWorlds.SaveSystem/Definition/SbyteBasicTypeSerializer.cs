using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000049 RID: 73
	internal class SbyteBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000CC9B File Offset: 0x0000AE9B
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteSByte((sbyte)value);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000CCA9 File Offset: 0x0000AEA9
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadSByte();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000CCB6 File Offset: 0x0000AEB6
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
