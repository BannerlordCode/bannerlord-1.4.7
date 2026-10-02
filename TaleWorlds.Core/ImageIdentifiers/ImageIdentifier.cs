using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E7 RID: 231
	public abstract class ImageIdentifier
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0002563D File Offset: 0x0002383D
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00025645 File Offset: 0x00023845
		public string Id { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0002564E File Offset: 0x0002384E
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00025656 File Offset: 0x00023856
		public string TextureProviderName { get; protected set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0002565F File Offset: 0x0002385F
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00025667 File Offset: 0x00023867
		public string AdditionalArgs { get; protected set; }

		// Token: 0x06000B94 RID: 2964 RVA: 0x00025670 File Offset: 0x00023870
		public bool Equals(ImageIdentifier other)
		{
			return other != null && this.Id.Equals(other.Id) && this.AdditionalArgs.Equals(other.AdditionalArgs) && this.TextureProviderName.Equals(other.TextureProviderName);
		}
	}
}
