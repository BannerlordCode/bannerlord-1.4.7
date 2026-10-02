using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000108 RID: 264
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000D7D RID: 3453 RVA: 0x00035D02 File Offset: 0x00033F02
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00035D18 File Offset: 0x00033F18
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00035D20 File Offset: 0x00033F20
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400041F RID: 1055
		private readonly object _value;

		// Token: 0x04000420 RID: 1056
		private readonly BsonType _type;
	}
}
