using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public struct DynamicBodyProperties
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0001835E File Offset: 0x0001655E
		public DynamicBodyProperties(float age, float weight, float build)
		{
			this.Age = age;
			this.Weight = weight;
			this.Build = build;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00018378 File Offset: 0x00016578
		public static bool operator ==(DynamicBodyProperties a, DynamicBodyProperties b)
		{
			return a == b || (a != null && b != null && (a.Age == b.Age && a.Weight == b.Weight) && a.Build == b.Build);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000183D3 File Offset: 0x000165D3
		public static bool operator !=(DynamicBodyProperties a, DynamicBodyProperties b)
		{
			return !(a == b);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000183DF File Offset: 0x000165DF
		public bool Equals(DynamicBodyProperties other)
		{
			return this.Age.Equals(other.Age) && this.Weight.Equals(other.Weight) && this.Build.Equals(other.Build);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001841A File Offset: 0x0001661A
		public override bool Equals(object obj)
		{
			return obj != null && obj is DynamicBodyProperties && this.Equals((DynamicBodyProperties)obj);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00018437 File Offset: 0x00016637
		public override int GetHashCode()
		{
			return (((this.Age.GetHashCode() * 397) ^ this.Weight.GetHashCode()) * 397) ^ this.Build.GetHashCode();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00018468 File Offset: 0x00016668
		public override string ToString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(150, "ToString");
			mbstringBuilder.Append<string>("age=\"");
			mbstringBuilder.Append<string>(this.Age.ToString("0.##"));
			mbstringBuilder.Append<string>("\" weight=\"");
			mbstringBuilder.Append<string>(this.Weight.ToString("0.####"));
			mbstringBuilder.Append<string>("\" build=\"");
			mbstringBuilder.Append<string>(this.Build.ToString("0.####"));
			mbstringBuilder.Append<string>("\" ");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x04000375 RID: 885
		public const float MaxAge = 128f;

		// Token: 0x04000376 RID: 886
		public const float MaxAgeTeenager = 21f;

		// Token: 0x04000377 RID: 887
		public float Age;

		// Token: 0x04000378 RID: 888
		public float Weight;

		// Token: 0x04000379 RID: 889
		public float Build;

		// Token: 0x0400037A RID: 890
		public static readonly DynamicBodyProperties Invalid;

		// Token: 0x0400037B RID: 891
		public static readonly DynamicBodyProperties Default = new DynamicBodyProperties(20f, 0.5f, 0.5f);
	}
}
