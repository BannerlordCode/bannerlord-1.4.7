using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000057 RID: 87
	internal class BoolBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteBool((bool)value);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadBool();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D0C3 File Offset: 0x0000B2C3
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 1;
		}
	}
}
