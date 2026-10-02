using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000062 RID: 98
	internal class GenericSaveId : SaveId
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000E633 File Offset: 0x0000C833
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000E63B File Offset: 0x0000C83B
		public SaveId BaseId { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000E644 File Offset: 0x0000C844
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000E64C File Offset: 0x0000C84C
		public SaveId[] GenericTypeIDs { get; set; }

		// Token: 0x0600034F RID: 847 RVA: 0x0000E655 File Offset: 0x0000C855
		public GenericSaveId(TypeSaveId baseId, SaveId[] saveIds)
		{
			this.BaseId = baseId;
			this.GenericTypeIDs = saveIds;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000E678 File Offset: 0x0000C878
		private string CalculateStringId()
		{
			string text = "";
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				if (i != 0)
				{
					text += ",";
				}
				SaveId saveId = this.GenericTypeIDs[i];
				text += saveId.GetStringId();
			}
			return string.Concat(new string[]
			{
				"G(",
				this.BaseId.GetStringId(),
				")-(",
				text,
				")"
			});
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000E6F8 File Offset: 0x0000C8F8
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000E700 File Offset: 0x0000C900
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(1);
			this.BaseId.WriteTo(writer);
			writer.WriteByte((byte)this.GenericTypeIDs.Length);
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				this.GenericTypeIDs[i].WriteTo(writer);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000E750 File Offset: 0x0000C950
		public static GenericSaveId ReadFrom(IReader reader)
		{
			reader.ReadByte();
			TypeSaveId typeSaveId = TypeSaveId.ReadFrom(reader);
			byte b = reader.ReadByte();
			List<SaveId> list = new List<SaveId>();
			for (int i = 0; i < (int)b; i++)
			{
				SaveId saveId = null;
				byte b2 = reader.ReadByte();
				if (b2 == 0)
				{
					saveId = TypeSaveId.ReadFrom(reader);
				}
				else if (b2 == 1)
				{
					saveId = GenericSaveId.ReadFrom(reader);
				}
				else if (b2 == 2)
				{
					saveId = ContainerSaveId.ReadFrom(reader);
				}
				list.Add(saveId);
			}
			return new GenericSaveId(typeSaveId, list.ToArray());
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		public override int GetSizeInBytes()
		{
			int num = 2 + this.BaseId.GetSizeInBytes();
			for (int i = 0; i < this.GenericTypeIDs.Length; i++)
			{
				SaveId saveId = this.GenericTypeIDs[i];
				num += saveId.GetSizeInBytes();
			}
			return num;
		}

		// Token: 0x040000FC RID: 252
		private readonly string _stringId;
	}
}
