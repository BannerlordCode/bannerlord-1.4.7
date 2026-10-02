using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008D RID: 141
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001C44E File Offset: 0x0001A64E
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001C456 File Offset: 0x0001A656
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001C45F File Offset: 0x0001A65F
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001C467 File Offset: 0x0001A667
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001C470 File Offset: 0x0001A670
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001C478 File Offset: 0x0001A678
		public Required? ItemRequired { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001C481 File Offset: 0x0001A681
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001C489 File Offset: 0x0001A689
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001C492 File Offset: 0x0001A692
		[Nullable(1)]
		public JsonPropertyCollection Properties
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001C49A File Offset: 0x0001A69A
		[Nullable(1)]
		public JsonPropertyCollection CreatorParameters
		{
			[NullableContext(1)]
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001C4BB File Offset: 0x0001A6BB
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001C4C3 File Offset: 0x0001A6C3
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001C4CC File Offset: 0x0001A6CC
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001C4D4 File Offset: 0x0001A6D4
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._parameterizedCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001C4DD File Offset: 0x0001A6DD
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001C4E5 File Offset: 0x0001A6E5
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001C4EE File Offset: 0x0001A6EE
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001C4F6 File Offset: 0x0001A6F6
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0001C4FF File Offset: 0x0001A6FF
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0001C507 File Offset: 0x0001A707
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = value != null && typeof(JToken).IsAssignableFrom(value);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001C532 File Offset: 0x0001A732
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0001C53A File Offset: 0x0001A73A
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> ExtensionDataNameResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0001C544 File Offset: 0x0001A744
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
								if (!((defaultValueHandling.GetValueOrDefault() == defaultValueHandling2) & (defaultValueHandling != null)))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001C630 File Offset: 0x0001A830
		[NullableContext(1)]
		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001C651 File Offset: 0x0001A851
		[NullableContext(1)]
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x0400027C RID: 636
		internal bool ExtensionDataIsJToken;

		// Token: 0x0400027D RID: 637
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x0400027E RID: 638
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x0400027F RID: 639
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000280 RID: 640
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x04000281 RID: 641
		private Type _extensionDataValueType;
	}
}
