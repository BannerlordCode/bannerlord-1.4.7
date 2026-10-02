using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A8 RID: 168
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModel
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00025D24 File Offset: 0x00023F24
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00025D2C File Offset: 0x00023F2C
		public bool Required { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00025D35 File Offset: 0x00023F35
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00025D3D File Offset: 0x00023F3D
		public JsonSchemaType Type { get; set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00025D46 File Offset: 0x00023F46
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x00025D4E File Offset: 0x00023F4E
		public int? MinimumLength { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00025D57 File Offset: 0x00023F57
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00025D5F File Offset: 0x00023F5F
		public int? MaximumLength { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00025D68 File Offset: 0x00023F68
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00025D70 File Offset: 0x00023F70
		public double? DivisibleBy { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00025D79 File Offset: 0x00023F79
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00025D81 File Offset: 0x00023F81
		public double? Minimum { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00025D8A File Offset: 0x00023F8A
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x00025D92 File Offset: 0x00023F92
		public double? Maximum { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x00025D9B File Offset: 0x00023F9B
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x00025DA3 File Offset: 0x00023FA3
		public bool ExclusiveMinimum { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00025DAC File Offset: 0x00023FAC
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x00025DB4 File Offset: 0x00023FB4
		public bool ExclusiveMaximum { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00025DBD File Offset: 0x00023FBD
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x00025DC5 File Offset: 0x00023FC5
		public int? MinimumItems { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00025DCE File Offset: 0x00023FCE
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x00025DD6 File Offset: 0x00023FD6
		public int? MaximumItems { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00025DDF File Offset: 0x00023FDF
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x00025DE7 File Offset: 0x00023FE7
		public IList<string> Patterns { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00025DF0 File Offset: 0x00023FF0
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x00025DF8 File Offset: 0x00023FF8
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00025E01 File Offset: 0x00024001
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x00025E09 File Offset: 0x00024009
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00025E12 File Offset: 0x00024012
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x00025E1A File Offset: 0x0002401A
		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00025E23 File Offset: 0x00024023
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x00025E2B File Offset: 0x0002402B
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00025E34 File Offset: 0x00024034
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00025E3C File Offset: 0x0002403C
		public JsonSchemaModel AdditionalItems { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00025E45 File Offset: 0x00024045
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00025E4D File Offset: 0x0002404D
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00025E56 File Offset: 0x00024056
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00025E5E File Offset: 0x0002405E
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00025E67 File Offset: 0x00024067
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00025E6F File Offset: 0x0002406F
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00025E78 File Offset: 0x00024078
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x00025E80 File Offset: 0x00024080
		public bool UniqueItems { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00025E89 File Offset: 0x00024089
		// (set) Token: 0x06000925 RID: 2341 RVA: 0x00025E91 File Offset: 0x00024091
		public IList<JToken> Enum { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x00025E9A File Offset: 0x0002409A
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x00025EA2 File Offset: 0x000240A2
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x06000928 RID: 2344 RVA: 0x00025EAB File Offset: 0x000240AB
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
			this.Required = false;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00025ED0 File Offset: 0x000240D0
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema jsonSchema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, jsonSchema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00025F20 File Offset: 0x00024120
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = model.Required || schema.Required.GetValueOrDefault();
			model.Type &= schema.Type ?? JsonSchemaType.Any;
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || schema.ExclusiveMinimum.GetValueOrDefault();
			model.ExclusiveMaximum = model.ExclusiveMaximum || schema.ExclusiveMaximum.GetValueOrDefault();
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = model.PositionalItemsValidation || schema.PositionalItemsValidation;
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			model.AllowAdditionalItems = model.AllowAdditionalItems && schema.AllowAdditionalItems;
			model.UniqueItems = model.UniqueItems || schema.UniqueItems;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct<JToken>(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow.GetValueOrDefault();
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
