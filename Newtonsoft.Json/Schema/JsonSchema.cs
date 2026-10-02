using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A3 RID: 163
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class JsonSchema
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00023E2C File Offset: 0x0002202C
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x00023E34 File Offset: 0x00022034
		public string Id { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00023E3D File Offset: 0x0002203D
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x00023E45 File Offset: 0x00022045
		public string Title { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00023E4E File Offset: 0x0002204E
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00023E56 File Offset: 0x00022056
		public bool? Required { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00023E5F File Offset: 0x0002205F
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00023E67 File Offset: 0x00022067
		public bool? ReadOnly { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00023E70 File Offset: 0x00022070
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x00023E78 File Offset: 0x00022078
		public bool? Hidden { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00023E81 File Offset: 0x00022081
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00023E89 File Offset: 0x00022089
		public bool? Transient { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00023E92 File Offset: 0x00022092
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00023E9A File Offset: 0x0002209A
		public string Description { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00023EA3 File Offset: 0x000220A3
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00023EAB File Offset: 0x000220AB
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00023EB4 File Offset: 0x000220B4
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00023EBC File Offset: 0x000220BC
		public string Pattern { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00023EC5 File Offset: 0x000220C5
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00023ECD File Offset: 0x000220CD
		public int? MinimumLength { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00023ED6 File Offset: 0x000220D6
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00023EDE File Offset: 0x000220DE
		public int? MaximumLength { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00023EE7 File Offset: 0x000220E7
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00023EEF File Offset: 0x000220EF
		public double? DivisibleBy { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00023EF8 File Offset: 0x000220F8
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00023F00 File Offset: 0x00022100
		public double? Minimum { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00023F09 File Offset: 0x00022109
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00023F11 File Offset: 0x00022111
		public double? Maximum { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00023F1A File Offset: 0x0002211A
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00023F22 File Offset: 0x00022122
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00023F2B File Offset: 0x0002212B
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00023F33 File Offset: 0x00022133
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00023F3C File Offset: 0x0002213C
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00023F44 File Offset: 0x00022144
		public int? MinimumItems { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00023F4D File Offset: 0x0002214D
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00023F55 File Offset: 0x00022155
		public int? MaximumItems { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00023F5E File Offset: 0x0002215E
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00023F66 File Offset: 0x00022166
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00023F6F File Offset: 0x0002216F
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00023F77 File Offset: 0x00022177
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00023F80 File Offset: 0x00022180
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00023F88 File Offset: 0x00022188
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00023F91 File Offset: 0x00022191
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00023F99 File Offset: 0x00022199
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00023FA2 File Offset: 0x000221A2
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00023FAA File Offset: 0x000221AA
		public bool UniqueItems { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00023FB3 File Offset: 0x000221B3
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00023FBB File Offset: 0x000221BB
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00023FC4 File Offset: 0x000221C4
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00023FCC File Offset: 0x000221CC
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00023FD5 File Offset: 0x000221D5
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00023FDD File Offset: 0x000221DD
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00023FE6 File Offset: 0x000221E6
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00023FEE File Offset: 0x000221EE
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00023FF7 File Offset: 0x000221F7
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00023FFF File Offset: 0x000221FF
		public string Requires { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00024008 File Offset: 0x00022208
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00024010 File Offset: 0x00022210
		public IList<JToken> Enum { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00024019 File Offset: 0x00022219
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00024021 File Offset: 0x00022221
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0002402A File Offset: 0x0002222A
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00024032 File Offset: 0x00022232
		public JToken Default { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0002403B File Offset: 0x0002223B
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x00024043 File Offset: 0x00022243
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x0002404C File Offset: 0x0002224C
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00024054 File Offset: 0x00022254
		public string Format { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0002405D File Offset: 0x0002225D
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00024065 File Offset: 0x00022265
		internal string Location { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0002406E File Offset: 0x0002226E
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00024076 File Offset: 0x00022276
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0002407E File Offset: 0x0002227E
		internal string DeferredReference { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00024087 File Offset: 0x00022287
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0002408F File Offset: 0x0002228F
		internal bool ReferencesResolved { get; set; }

		// Token: 0x060008C1 RID: 2241 RVA: 0x00024098 File Offset: 0x00022298
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000240D1 File Offset: 0x000222D1
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000240DE File Offset: 0x000222DE
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			return new JsonSchemaBuilder(resolver).Read(reader);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00024102 File Offset: 0x00022302
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00024110 File Offset: 0x00022310
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema jsonSchema;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				jsonSchema = JsonSchema.Read(jsonReader, resolver);
			}
			return jsonSchema;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002415C File Offset: 0x0002235C
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002416A File Offset: 0x0002236A
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			new JsonSchemaWriter(writer, resolver).WriteSchema(this);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00024190 File Offset: 0x00022390
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x040002EF RID: 751
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
