using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	public interface IAttributeProvider
	{
		// Token: 0x0600065E RID: 1630
		IList<Attribute> GetAttributes(bool inherit);

		// Token: 0x0600065F RID: 1631
		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}
