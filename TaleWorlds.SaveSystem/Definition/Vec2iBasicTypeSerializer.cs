using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200004F RID: 79
	internal class Vec2iBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000CD90 File Offset: 0x0000AF90
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec2i vec2i = (Vec2i)value;
			writer.WriteFloat((float)vec2i.Item1);
			writer.WriteFloat((float)vec2i.Item2);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CDC0 File Offset: 0x0000AFC0
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			int num = reader.ReadInt();
			int num2 = reader.ReadInt();
			return new Vec2i(num, num2);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CDE5 File Offset: 0x0000AFE5
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 8;
		}
	}
}
