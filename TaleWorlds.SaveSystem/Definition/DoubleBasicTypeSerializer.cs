using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004B RID: 75
	internal class DoubleBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000CCE7 File Offset: 0x0000AEE7
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteDouble((double)value);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000CCF5 File Offset: 0x0000AEF5
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadDouble();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000CD02 File Offset: 0x0000AF02
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
