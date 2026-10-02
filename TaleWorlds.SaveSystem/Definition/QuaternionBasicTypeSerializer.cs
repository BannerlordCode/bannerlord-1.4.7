using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000055 RID: 85
	internal class QuaternionBasicTypeSerializer : IBasicTypeSerializer
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000CFEC File Offset: 0x0000B1EC
		void IBasicTypeSerializer.Serialize(IWriter writer, object value)
		{
			Quaternion quaternion = (Quaternion)value;
			writer.WriteFloat(quaternion.X);
			writer.WriteFloat(quaternion.Y);
			writer.WriteFloat(quaternion.Z);
			writer.WriteFloat(quaternion.W);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D030 File Offset: 0x0000B230
		object IBasicTypeSerializer.Deserialize(IReader reader)
		{
			float num = reader.ReadFloat();
			float num2 = reader.ReadFloat();
			float num3 = reader.ReadFloat();
			float num4 = reader.ReadFloat();
			return new Quaternion(num, num2, num3, num4);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D065 File Offset: 0x0000B265
		int IBasicTypeSerializer.GetSizeInBytes()
		{
			return 16;
		}
	}
}
