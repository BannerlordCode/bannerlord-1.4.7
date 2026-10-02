using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000044 RID: 68
	internal class IntBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0000CBDD File Offset: 0x0000ADDD
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000CBEB File Offset: 0x0000ADEB
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadInt();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
