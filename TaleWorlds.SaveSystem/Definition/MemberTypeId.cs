using System;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000066 RID: 102
	public struct MemberTypeId
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000E86D File Offset: 0x0000CA6D
		public short SaveId
		{
			get
			{
				return (short)(this.TypeLevel << 8) + this.LocalSaveId;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000E880 File Offset: 0x0000CA80
		public static MemberTypeId Invalid
		{
			get
			{
				return new MemberTypeId(0, -1);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000E88C File Offset: 0x0000CA8C
		public override string ToString()
		{
			return string.Concat(new object[] { "(", this.TypeLevel, ",", this.LocalSaveId, ")" });
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		public MemberTypeId(byte typeLevel, short localSaveId)
		{
			this.TypeLevel = typeLevel;
			this.LocalSaveId = localSaveId;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
		public override bool Equals(object obj)
		{
			if (obj is MemberTypeId)
			{
				MemberTypeId memberTypeId = (MemberTypeId)obj;
				return memberTypeId.TypeLevel == this.TypeLevel && memberTypeId.LocalSaveId == this.LocalSaveId;
			}
			return false;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000E926 File Offset: 0x0000CB26
		public static bool operator ==(MemberTypeId m1, MemberTypeId m2)
		{
			if (m1 == null)
			{
				return m2 == null;
			}
			return m1.Equals(m2);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000E94D File Offset: 0x0000CB4D
		public static bool operator !=(MemberTypeId m1, MemberTypeId m2)
		{
			return !(m1 == m2);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000E959 File Offset: 0x0000CB59
		public override int GetHashCode()
		{
			return (int)((short)((17 * 31 + this.TypeLevel) * 31) + this.LocalSaveId);
		}

		// Token: 0x040000FF RID: 255
		public byte TypeLevel;

		// Token: 0x04000100 RID: 256
		public short LocalSaveId;
	}
}
