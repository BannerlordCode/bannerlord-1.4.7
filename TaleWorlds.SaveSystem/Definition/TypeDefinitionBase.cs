using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006E RID: 110
	public class TypeDefinitionBase
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00011028 File Offset: 0x0000F228
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00011030 File Offset: 0x0000F230
		public SaveId SaveId { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00011039 File Offset: 0x0000F239
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00011041 File Offset: 0x0000F241
		public Type Type { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001104A File Offset: 0x0000F24A
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00011052 File Offset: 0x0000F252
		public byte TypeLevel { get; private set; }

		// Token: 0x060003C4 RID: 964 RVA: 0x0001105B File Offset: 0x0000F25B
		protected TypeDefinitionBase(Type type, SaveId saveId)
		{
			this.Type = type;
			this.SaveId = saveId;
			this.TypeLevel = TypeDefinitionBase.GetClassLevel(type);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00011080 File Offset: 0x0000F280
		public static byte GetClassLevel(Type type)
		{
			byte b = 1;
			if (type.IsClass)
			{
				Type type2 = type;
				while (type2 != typeof(object))
				{
					b += 1;
					type2 = type2.BaseType;
				}
			}
			return b;
		}
	}
}
