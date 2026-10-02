using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonContract
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001B933 File Offset: 0x00019B33
		public Type UnderlyingType { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001B93B File Offset: 0x00019B3B
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x0001B944 File Offset: 0x00019B44
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001B998 File Offset: 0x00019B98
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001B9A0 File Offset: 0x00019BA0
		public bool? IsReference { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001B9A9 File Offset: 0x00019BA9
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x0001B9B1 File Offset: 0x00019BB1
		[Nullable(2)]
		public JsonConverter Converter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001B9BA File Offset: 0x00019BBA
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x0001B9C2 File Offset: 0x00019BC2
		[Nullable(2)]
		public JsonConverter InternalConverter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			internal set;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001B9CB File Offset: 0x00019BCB
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001B9E6 File Offset: 0x00019BE6
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001BA01 File Offset: 0x00019C01
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001BA1C File Offset: 0x00019C1C
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001BA37 File Offset: 0x00019C37
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001BA52 File Offset: 0x00019C52
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0001BA5A File Offset: 0x00019C5A
		[Nullable(new byte[] { 2, 1 })]
		public Func<object> DefaultCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001BA63 File Offset: 0x00019C63
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0001BA6B File Offset: 0x00019C6B
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001BA74 File Offset: 0x00019C74
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this._createdType = (this.CreatedType = this.NonNullableUnderlyingType);
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001BB0C File Offset: 0x00019D0C
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001BB68 File Offset: 0x00019D68
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001BBC4 File Offset: 0x00019DC4
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001BC20 File Offset: 0x00019E20
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001BC7C File Offset: 0x00019E7C
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001BCD8 File Offset: 0x00019ED8
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[] { context });
			};
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001BCF1 File Offset: 0x00019EF1
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[] { context, econtext });
			};
		}

		// Token: 0x0400024A RID: 586
		internal bool IsNullable;

		// Token: 0x0400024B RID: 587
		internal bool IsConvertable;

		// Token: 0x0400024C RID: 588
		internal bool IsEnum;

		// Token: 0x0400024D RID: 589
		internal Type NonNullableUnderlyingType;

		// Token: 0x0400024E RID: 590
		internal ReadType InternalReadType;

		// Token: 0x0400024F RID: 591
		internal JsonContractType ContractType;

		// Token: 0x04000250 RID: 592
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x04000251 RID: 593
		internal bool IsSealed;

		// Token: 0x04000252 RID: 594
		internal bool IsInstantiable;

		// Token: 0x04000253 RID: 595
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x04000254 RID: 596
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x04000255 RID: 597
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x04000256 RID: 598
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x04000257 RID: 599
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000258 RID: 600
		private Type _createdType;
	}
}
