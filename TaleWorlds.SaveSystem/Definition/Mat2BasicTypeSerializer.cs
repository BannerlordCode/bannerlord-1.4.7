using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000052 RID: 82
	internal class Mat2BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000CE58 File Offset: 0x0000B058
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Mat2 mat = (Mat2)value;
			writer.WriteVec2(mat.s);
			writer.WriteVec2(mat.f);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000CE84 File Offset: 0x0000B084
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec2 vec = reader.ReadVec2();
			Vec2 vec2 = reader.ReadVec2();
			return new Mat2(vec.x, vec.y, vec2.x, vec2.y);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000CEC1 File Offset: 0x0000B0C1
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
