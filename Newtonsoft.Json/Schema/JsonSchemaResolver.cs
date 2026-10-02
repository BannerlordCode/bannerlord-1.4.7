using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AC RID: 172
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchemaResolver
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00026745 File Offset: 0x00024945
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x0002674D File Offset: 0x0002494D
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x06000945 RID: 2373 RVA: 0x00026756 File Offset: 0x00024956
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002676C File Offset: 0x0002496C
		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = this.LoadedSchemas.SingleOrDefault<JsonSchema>((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
}
