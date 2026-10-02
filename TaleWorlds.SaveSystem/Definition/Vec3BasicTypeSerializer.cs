using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000050 RID: 80
	internal class Vec3BasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Vec3 vec = (Vec3)value;
			writer.WriteVec3(vec);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000CE0B File Offset: 0x0000B00B
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			return reader.ReadVec3();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000CE18 File Offset: 0x0000B018
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
