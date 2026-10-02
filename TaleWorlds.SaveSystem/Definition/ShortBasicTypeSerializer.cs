using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000046 RID: 70
	internal class ShortBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000CC29 File Offset: 0x0000AE29
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteShort((short)value);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000CC37 File Offset: 0x0000AE37
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadShort();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000CC44 File Offset: 0x0000AE44
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 2;
		}
	}
}
