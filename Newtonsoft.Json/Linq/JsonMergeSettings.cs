using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C0 RID: 192
	public class JsonMergeSettings
	{
		// Token: 0x06000A9C RID: 2716 RVA: 0x0002A071 File Offset: 0x00028271
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0002A080 File Offset: 0x00028280
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x0002A088 File Offset: 0x00028288
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0002A0A4 File Offset: 0x000282A4
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x0002A0AC File Offset: 0x000282AC
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0002A0C8 File Offset: 0x000282C8
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x0002A0D0 File Offset: 0x000282D0
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x0400036E RID: 878
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x0400036F RID: 879
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x04000370 RID: 880
		private StringComparison _propertyNameComparison;
	}
}
