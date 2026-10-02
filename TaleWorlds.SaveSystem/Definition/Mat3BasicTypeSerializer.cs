using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000053 RID: 83
	internal class Mat3BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000CED0 File Offset: 0x0000B0D0
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Mat3 mat = (Mat3)value;
			writer.WriteVec3(mat.s);
			writer.WriteVec3(mat.f);
			writer.WriteVec3(mat.u);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000CF08 File Offset: 0x0000B108
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec3 vec = reader.ReadVec3();
			Vec3 vec2 = reader.ReadVec3();
			Vec3 vec3 = reader.ReadVec3();
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000CF3A File Offset: 0x0000B13A
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 48;
		}
	}
}
