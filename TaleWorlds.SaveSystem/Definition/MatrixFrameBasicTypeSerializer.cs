using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000054 RID: 84
	internal class MatrixFrameBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000CF48 File Offset: 0x0000B148
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			MatrixFrame matrixFrame = (MatrixFrame)value;
			writer.WriteVec3(matrixFrame.origin);
			writer.WriteVec3(matrixFrame.rotation.s);
			writer.WriteVec3(matrixFrame.rotation.f);
			writer.WriteVec3(matrixFrame.rotation.u);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CF9C File Offset: 0x0000B19C
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			Vec3 vec = reader.ReadVec3();
			Vec3 vec2 = reader.ReadVec3();
			Vec3 vec3 = reader.ReadVec3();
			Vec3 vec4 = reader.ReadVec3();
			Mat3 mat = new Mat3(in vec3, in vec2, in vec4);
			return new MatrixFrame(in mat, in vec);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000CFE0 File Offset: 0x0000B1E0
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 48;
		}
	}
}
