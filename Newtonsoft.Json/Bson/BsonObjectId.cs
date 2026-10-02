using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000102 RID: 258
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonObjectId
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00034FA9 File Offset: 0x000331A9
		public byte[] Value { get; }

		// Token: 0x06000D47 RID: 3399 RVA: 0x00034FB1 File Offset: 0x000331B1
		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			this.Value = value;
		}
	}
}
