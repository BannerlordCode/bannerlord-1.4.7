using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005D RID: 93
	internal static class JsonTokenUtils
	{
		// Token: 0x06000543 RID: 1347 RVA: 0x0001684E File Offset: 0x00014A4E
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001685A File Offset: 0x00014A5A
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00016865 File Offset: 0x00014A65
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
