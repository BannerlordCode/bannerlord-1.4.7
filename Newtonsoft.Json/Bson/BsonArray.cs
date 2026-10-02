using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000106 RID: 262
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		// Token: 0x06000D75 RID: 3445 RVA: 0x00035C8D File Offset: 0x00033E8D
		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00035CA2 File Offset: 0x00033EA2
		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00035CA5 File Offset: 0x00033EA5
		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00035CB7 File Offset: 0x00033EB7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400041B RID: 1051
		private readonly List<BsonToken> _children = new List<BsonToken>();
	}
}
