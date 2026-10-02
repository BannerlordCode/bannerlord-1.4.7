using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008F RID: 143
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonProperty
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001C7EC File Offset: 0x0001A9EC
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001C7FD File Offset: 0x0001A9FD
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0001C805 File Offset: 0x0001AA05
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001C827 File Offset: 0x0001AA27
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0001C82F File Offset: 0x0001AA2F
		public Type DeclaringType { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001C838 File Offset: 0x0001AA38
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0001C840 File Offset: 0x0001AA40
		public int? Order { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001C849 File Offset: 0x0001AA49
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0001C851 File Offset: 0x0001AA51
		public string UnderlyingName { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001C85A File Offset: 0x0001AA5A
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0001C862 File Offset: 0x0001AA62
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0001C86B File Offset: 0x0001AA6B
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0001C873 File Offset: 0x0001AA73
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0001C87C File Offset: 0x0001AA7C
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0001C884 File Offset: 0x0001AA84
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001C8A2 File Offset: 0x0001AAA2
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001C8AA File Offset: 0x0001AAAA
		public JsonConverter Converter { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001C8B3 File Offset: 0x0001AAB3
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001C8BB File Offset: 0x0001AABB
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001C8C4 File Offset: 0x0001AAC4
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001C8CC File Offset: 0x0001AACC
		public bool Ignored { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001C8D5 File Offset: 0x0001AAD5
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001C8DD File Offset: 0x0001AADD
		public bool Readable { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001C8E6 File Offset: 0x0001AAE6
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001C8EE File Offset: 0x0001AAEE
		public bool Writable { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0001C8F7 File Offset: 0x0001AAF7
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		public bool HasMemberAttribute { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001C908 File Offset: 0x0001AB08
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0001C91A File Offset: 0x0001AB1A
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001C92A File Offset: 0x0001AB2A
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001C96A File Offset: 0x0001AB6A
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001C977 File Offset: 0x0001AB77
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001C985 File Offset: 0x0001AB85
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001C992 File Offset: 0x0001AB92
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x0001C99A File Offset: 0x0001AB9A
		public bool? IsReference { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0001C9A3 File Offset: 0x0001ABA3
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001C9AB File Offset: 0x0001ABAB
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0001C9BC File Offset: 0x0001ABBC
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0001C9C5 File Offset: 0x0001ABC5
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0001C9CD File Offset: 0x0001ABCD
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0001C9D6 File Offset: 0x0001ABD6
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x0001C9DE File Offset: 0x0001ABDE
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001C9E7 File Offset: 0x0001ABE7
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x0001C9EF File Offset: 0x0001ABEF
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x0001C9F8 File Offset: 0x0001ABF8
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0001CA00 File Offset: 0x0001AC00
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldSerialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001CA09 File Offset: 0x0001AC09
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0001CA11 File Offset: 0x0001AC11
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldDeserialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001CA1A File Offset: 0x0001AC1A
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0001CA22 File Offset: 0x0001AC22
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> GetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001CA2B File Offset: 0x0001AC2B
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0001CA33 File Offset: 0x0001AC33
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> SetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001CA3C File Offset: 0x0001AC3C
		[NullableContext(1)]
		public override string ToString()
		{
			return this.PropertyName ?? string.Empty;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001CA4D File Offset: 0x0001AC4D
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0001CA55 File Offset: 0x0001AC55
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001CA5E File Offset: 0x0001AC5E
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0001CA66 File Offset: 0x0001AC66
		public bool? ItemIsReference { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0001CA6F File Offset: 0x0001AC6F
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x0001CA77 File Offset: 0x0001AC77
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0001CA80 File Offset: 0x0001AC80
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0001CA88 File Offset: 0x0001AC88
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x0600073C RID: 1852 RVA: 0x0001CA94 File Offset: 0x0001AC94
		[NullableContext(1)]
		internal void WritePropertyName(JsonWriter writer)
		{
			string propertyName = this.PropertyName;
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(propertyName, false);
				return;
			}
			writer.WritePropertyName(propertyName);
		}

		// Token: 0x04000284 RID: 644
		internal Required? _required;

		// Token: 0x04000285 RID: 645
		internal bool _hasExplicitDefaultValue;

		// Token: 0x04000286 RID: 646
		private object _defaultValue;

		// Token: 0x04000287 RID: 647
		private bool _hasGeneratedDefaultValue;

		// Token: 0x04000288 RID: 648
		private string _propertyName;

		// Token: 0x04000289 RID: 649
		internal bool _skipPropertyNameEscape;

		// Token: 0x0400028A RID: 650
		private Type _propertyType;
	}
}
