using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000095 RID: 149
	public class JsonStringContract : JsonPrimitiveContract
	{
		// Token: 0x060007E5 RID: 2021 RVA: 0x00022815 File Offset: 0x00020A15
		[NullableContext(1)]
		public JsonStringContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.String;
		}
	}
}
