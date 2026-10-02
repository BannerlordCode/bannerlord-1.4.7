using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000045 RID: 69
	internal class UintBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000CC03 File Offset: 0x0000AE03
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			writer.WriteUInt((uint)value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000CC11 File Offset: 0x0000AE11
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadUInt();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000CC1E File Offset: 0x0000AE1E
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 4;
		}
	}
}
