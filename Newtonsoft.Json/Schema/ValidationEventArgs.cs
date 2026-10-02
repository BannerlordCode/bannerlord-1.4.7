using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000B0 RID: 176
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x00026F07 File Offset: 0x00025107
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00026F21 File Offset: 0x00025121
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00026F29 File Offset: 0x00025129
		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00026F36 File Offset: 0x00025136
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x04000352 RID: 850
		private readonly JsonSchemaException _ex;
	}
}
