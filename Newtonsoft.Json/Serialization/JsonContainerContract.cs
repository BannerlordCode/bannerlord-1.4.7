using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000081 RID: 129
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001B83E File Offset: 0x00019A3E
		// (set) Token: 0x0600067C RID: 1660 RVA: 0x0001B846 File Offset: 0x00019A46
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001B880 File Offset: 0x00019A80
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001B888 File Offset: 0x00019A88
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0001B890 File Offset: 0x00019A90
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0001B899 File Offset: 0x00019A99
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0001B8A1 File Offset: 0x00019AA1
		public bool? ItemIsReference { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001B8AA File Offset: 0x00019AAA
		// (set) Token: 0x06000683 RID: 1667 RVA: 0x0001B8B2 File Offset: 0x00019AB2
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x0001B8BB File Offset: 0x00019ABB
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x0001B8C3 File Offset: 0x00019AC3
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x06000686 RID: 1670 RVA: 0x0001B8CC File Offset: 0x00019ACC
		[NullableContext(1)]
		internal JsonContainerContract(Type underlyingType)
			: base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x0400023A RID: 570
		private JsonContract _itemContract;

		// Token: 0x0400023B RID: 571
		private JsonContract _finalItemContract;
	}
}
