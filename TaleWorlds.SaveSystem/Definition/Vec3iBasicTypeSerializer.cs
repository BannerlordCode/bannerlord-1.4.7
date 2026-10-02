using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000051 RID: 81
	internal class Vec3iBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000CE24 File Offset: 0x0000B024
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec3i vec3i = (Vec3i)value;
			writer.WriteVec3Int(vec3i);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000CE3F File Offset: 0x0000B03F
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec3Int();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000CE4C File Offset: 0x0000B04C
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 12;
		}
	}
}
