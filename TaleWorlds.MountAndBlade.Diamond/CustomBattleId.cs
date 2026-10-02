using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000111 RID: 273
	[Serializable]
	public struct CustomBattleId
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00007446 File Offset: 0x00005646
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x0000744E File Offset: 0x0000564E
		[JsonProperty]
		public Guid Guid { get; private set; }

		// Token: 0x060005DF RID: 1503 RVA: 0x00007457 File Offset: 0x00005657
		public CustomBattleId(Guid guid)
		{
			this.Guid = guid;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00007460 File Offset: 0x00005660
		public static CustomBattleId NewGuid()
		{
			return new CustomBattleId(Guid.NewGuid());
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000746C File Offset: 0x0000566C
		public override string ToString()
		{
			return this.Guid.ToString();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00007490 File Offset: 0x00005690
		public byte[] ToByteArray()
		{
			return this.Guid.ToByteArray();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000074AB File Offset: 0x000056AB
		public static bool operator ==(CustomBattleId a, CustomBattleId b)
		{
			return a.Guid == b.Guid;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000074C0 File Offset: 0x000056C0
		public static bool operator !=(CustomBattleId a, CustomBattleId b)
		{
			return a.Guid != b.Guid;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000074D8 File Offset: 0x000056D8
		public override bool Equals(object o)
		{
			if (o != null && o is CustomBattleId)
			{
				CustomBattleId customBattleId = (CustomBattleId)o;
				return this.Guid.Equals(customBattleId.Guid);
			}
			return false;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00007510 File Offset: 0x00005710
		public override int GetHashCode()
		{
			return this.Guid.GetHashCode();
		}

		// Token: 0x04000238 RID: 568
		[JsonIgnore]
		public static CustomBattleId Empty = new CustomBattleId(Guid.Empty);
	}
}
