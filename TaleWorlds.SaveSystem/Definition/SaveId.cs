using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006A RID: 106
	public abstract class SaveId
	{
		// Token: 0x06000397 RID: 919
		public abstract string GetStringId();

		// Token: 0x06000398 RID: 920 RVA: 0x000109D4 File Offset: 0x0000EBD4
		public override int GetHashCode()
		{
			return this.GetStringId().GetHashCode();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000109E1 File Offset: 0x0000EBE1
		public override bool Equals(object obj)
		{
			return obj != null && !(obj.GetType() != base.GetType()) && this.GetStringId() == ((SaveId)obj).GetStringId();
		}

		// Token: 0x0600039A RID: 922
		public abstract void WriteTo(IWriter writer);

		// Token: 0x0600039B RID: 923 RVA: 0x00010A14 File Offset: 0x0000EC14
		public static SaveId ReadSaveIdFrom(IReader reader)
		{
			byte b = reader.ReadByte();
			SaveId saveId = null;
			if (b == 0)
			{
				saveId = TypeSaveId.ReadFrom(reader);
			}
			else if (b == 1)
			{
				saveId = GenericSaveId.ReadFrom(reader);
			}
			else if (b == 2)
			{
				saveId = ContainerSaveId.ReadFrom(reader);
			}
			return saveId;
		}

		// Token: 0x0600039C RID: 924
		public abstract int GetSizeInBytes();
	}
}
