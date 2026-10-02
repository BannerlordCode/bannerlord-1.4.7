using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004E RID: 78
	internal class Vec2BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000CD5C File Offset: 0x0000AF5C
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec2 vec = (Vec2)value;
			writer.WriteVec2(vec);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CD77 File Offset: 0x0000AF77
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec2();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CD84 File Offset: 0x0000AF84
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
