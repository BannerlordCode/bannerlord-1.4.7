using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A6 RID: 166
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0002531F File Offset: 0x0002351F
		public int LineNumber { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00025327 File Offset: 0x00023527
		public int LinePosition { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0002532F File Offset: 0x0002352F
		public string Path { get; }

		// Token: 0x060008DF RID: 2271 RVA: 0x00025337 File Offset: 0x00023537
		public JsonSchemaException()
		{
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002533F File Offset: 0x0002353F
		public JsonSchemaException(string message)
			: base(message)
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00025348 File Offset: 0x00023548
		public JsonSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00025352 File Offset: 0x00023552
		public JsonSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002535C File Offset: 0x0002355C
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
