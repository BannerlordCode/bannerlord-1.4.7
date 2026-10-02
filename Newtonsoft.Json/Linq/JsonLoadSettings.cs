using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BF RID: 191
	public class JsonLoadSettings
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x00029FE8 File Offset: 0x000281E8
		public JsonLoadSettings()
		{
			this._lineInfoHandling = LineInfoHandling.Load;
			this._commentHandling = CommentHandling.Ignore;
			this._duplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002A005 File Offset: 0x00028205
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002A00D File Offset: 0x0002820D
		public CommentHandling CommentHandling
		{
			get
			{
				return this._commentHandling;
			}
			set
			{
				if (value < CommentHandling.Ignore || value > CommentHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._commentHandling = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002A029 File Offset: 0x00028229
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0002A031 File Offset: 0x00028231
		public LineInfoHandling LineInfoHandling
		{
			get
			{
				return this._lineInfoHandling;
			}
			set
			{
				if (value < LineInfoHandling.Ignore || value > LineInfoHandling.Load)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lineInfoHandling = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002A04D File Offset: 0x0002824D
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0002A055 File Offset: 0x00028255
		public DuplicatePropertyNameHandling DuplicatePropertyNameHandling
		{
			get
			{
				return this._duplicatePropertyNameHandling;
			}
			set
			{
				if (value < DuplicatePropertyNameHandling.Replace || value > DuplicatePropertyNameHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._duplicatePropertyNameHandling = value;
			}
		}

		// Token: 0x0400036B RID: 875
		private CommentHandling _commentHandling;

		// Token: 0x0400036C RID: 876
		private LineInfoHandling _lineInfoHandling;

		// Token: 0x0400036D RID: 877
		private DuplicatePropertyNameHandling _duplicatePropertyNameHandling;
	}
}
