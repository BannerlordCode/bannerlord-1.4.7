using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200005A RID: 90
	public class ContainerSaveId : SaveId
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000D132 File Offset: 0x0000B332
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000D13A File Offset: 0x0000B33A
		public ContainerType ContainerType { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000D143 File Offset: 0x0000B343
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000D14B File Offset: 0x0000B34B
		public SaveId KeyId { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000D154 File Offset: 0x0000B354
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000D15C File Offset: 0x0000B35C
		public SaveId ValueId { get; set; }

		// Token: 0x06000309 RID: 777 RVA: 0x0000D165 File Offset: 0x0000B365
		public ContainerSaveId(ContainerType containerType, SaveId elementId)
		{
			this.ContainerType = containerType;
			this.KeyId = elementId;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D187 File Offset: 0x0000B387
		public ContainerSaveId(ContainerType containerType, SaveId keyId, SaveId valueId)
		{
			this.ContainerType = containerType;
			this.KeyId = keyId;
			this.ValueId = valueId;
			this._stringId = this.CalculateStringId();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		private string CalculateStringId()
		{
			string text;
			if (this.ContainerType == ContainerType.Dictionary)
			{
				string stringId = this.KeyId.GetStringId();
				string stringId2 = this.ValueId.GetStringId();
				text = string.Concat(new object[]
				{
					"C(",
					(int)this.ContainerType,
					")-(",
					stringId,
					",",
					stringId2,
					")"
				});
			}
			else
			{
				string stringId3 = this.KeyId.GetStringId();
				text = string.Concat(new object[]
				{
					"C(",
					(int)this.ContainerType,
					")-(",
					stringId3,
					")"
				});
			}
			return text;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000D26B File Offset: 0x0000B46B
		public override string GetStringId()
		{
			return this._stringId;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000D273 File Offset: 0x0000B473
		public override void WriteTo(IWriter writer)
		{
			writer.WriteByte(2);
			writer.WriteByte((byte)this.ContainerType);
			this.KeyId.WriteTo(writer);
			if (this.ContainerType == ContainerType.Dictionary)
			{
				this.ValueId.WriteTo(writer);
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D2AC File Offset: 0x0000B4AC
		public static ContainerSaveId ReadFrom(IReader reader)
		{
			ContainerType containerType = (ContainerType)reader.ReadByte();
			int num = ((containerType == ContainerType.Dictionary) ? 2 : 1);
			List<SaveId> list = new List<SaveId>();
			for (int i = 0; i < num; i++)
			{
				SaveId saveId = null;
				byte b = reader.ReadByte();
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
				list.Add(saveId);
			}
			SaveId saveId2 = list[0];
			SaveId saveId3 = ((list.Count > 1) ? list[1] : null);
			return new ContainerSaveId(containerType, saveId2, saveId3);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000D344 File Offset: 0x0000B544
		public override int GetSizeInBytes()
		{
			int num = 2 + this.KeyId.GetSizeInBytes();
			if (this.ContainerType == ContainerType.Dictionary)
			{
				num += this.ValueId.GetSizeInBytes();
			}
			return num;
		}

		// Token: 0x040000DD RID: 221
		private readonly string _stringId;
	}
}
