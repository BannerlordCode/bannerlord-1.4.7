using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000105 RID: 261
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		// Token: 0x06000D70 RID: 3440 RVA: 0x00035C30 File Offset: 0x00033E30
		public void Add(string name, BsonToken token)
		{
			this._children.Add(new BsonProperty
			{
				Name = new BsonString(name, false),
				Value = token
			});
			token.Parent = this;
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00035C5D File Offset: 0x00033E5D
		public override BsonType Type
		{
			get
			{
				return BsonType.Object;
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00035C60 File Offset: 0x00033E60
		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00035C72 File Offset: 0x00033E72
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400041A RID: 1050
		private readonly List<BsonProperty> _children = new List<BsonProperty>();
	}
}
