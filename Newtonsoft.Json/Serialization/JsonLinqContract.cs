using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008C RID: 140
	public class JsonLinqContract : JsonContract
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x0001C43E File Offset: 0x0001A63E
		[NullableContext(1)]
		public JsonLinqContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Linq;
		}
	}
}
