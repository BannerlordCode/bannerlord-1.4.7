using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006F RID: 111
	public class TypeSaveId : SaveId
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x000110BA File Offset: 0x0000F2BA
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x000110C2 File Offset: 0x0000F2C2
		public int Id { get; private set; }

		// Token: 0x060003C8 RID: 968 RVA: 0x000110CC File Offset: 0x0000F2CC
		public TypeSaveId(int id)
		{
			this.Id = id;
			this._stringId = this.Id.ToString();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000110FA File Offset: 0x0000F2FA
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00011102 File Offset: 0x0000F302
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(0);
			writer.WriteInt(this.Id);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00011117 File Offset: 0x0000F317
		public static TypeSaveId ReadFrom(IReader reader)
		{
			return new TypeSaveId(reader.ReadInt());
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00011124 File Offset: 0x0000F324
		public override int GetSizeInBytes()
		{
			return 5;
		}

		// Token: 0x04000125 RID: 293
		private readonly string _stringId;
	}
}
