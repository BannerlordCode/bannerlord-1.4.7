using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004A RID: 74
	internal class FloatBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000CCC1 File Offset: 0x0000AEC1
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteFloat((float)value);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000CCCF File Offset: 0x0000AECF
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadFloat();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
