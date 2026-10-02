using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008B RID: 139
	public class JsonISerializableContract : JsonContainerContract
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001C41D File Offset: 0x0001A61D
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0001C425 File Offset: 0x0001A625
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> ISerializableCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001C42E File Offset: 0x0001A62E
		[NullableContext(1)]
		public JsonISerializableContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Serializable;
		}
	}
}
