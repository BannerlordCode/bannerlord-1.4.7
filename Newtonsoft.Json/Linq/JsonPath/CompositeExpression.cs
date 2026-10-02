using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0002F833 File Offset: 0x0002DA33
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x0002F83B File Offset: 0x0002DA3B
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002F844 File Offset: 0x0002DA44
		public CompositeExpression(QueryOperator @operator)
			: base(@operator)
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002F858 File Offset: 0x0002DA58
		public override bool IsMatch(JToken root, JToken t, [Nullable(2)] JsonSelectSettings settings)
		{
			QueryOperator @operator = this.Operator;
			if (@operator == QueryOperator.And)
			{
				using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsMatch(root, t, settings))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (@operator != QueryOperator.Or)
			{
				throw new ArgumentOutOfRangeException();
			}
			using (List<QueryExpression>.Enumerator enumerator = this.Expressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsMatch(root, t, settings))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
