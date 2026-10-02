using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010F RID: 271
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00035E0D File Offset: 0x0003400D
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x00035E1A File Offset: 0x0003401A
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00035E28 File Offset: 0x00034028
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00035E4C File Offset: 0x0003404C
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00035E6B File Offset: 0x0003406B
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00035E78 File Offset: 0x00034078
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00035EA0 File Offset: 0x000340A0
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00035EAE File Offset: 0x000340AE
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00035EBC File Offset: 0x000340BC
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00035ECA File Offset: 0x000340CA
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00035ED8 File Offset: 0x000340D8
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00035EEB File Offset: 0x000340EB
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00035EFE File Offset: 0x000340FE
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00035F0E File Offset: 0x0003410E
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00035F2E File Offset: 0x0003412E
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00035F3E File Offset: 0x0003413E
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00035F51 File Offset: 0x00034151
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00035F60 File Offset: 0x00034160
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				BsonObject bsonObject = this._parent as BsonObject;
				if (bsonObject != null)
				{
					bsonObject.Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00035FF0 File Offset: 0x000341F0
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value;
				base.SetWriteState(JsonToken.Integer, null);
				this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0003602F File Offset: 0x0003422F
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00036042 File Offset: 0x00034242
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00036055 File Offset: 0x00034255
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddToken((value == null) ? BsonEmpty.Null : new BsonString(value, true));
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00036075 File Offset: 0x00034275
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0003608C File Offset: 0x0003428C
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x000360B8 File Offset: 0x000342B8
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000360CF File Offset: 0x000342CF
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000360FF File Offset: 0x000342FF
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00036115 File Offset: 0x00034315
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0003612B File Offset: 0x0003432B
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00036149 File Offset: 0x00034349
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00036160 File Offset: 0x00034360
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00036178 File Offset: 0x00034378
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(text, true));
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x000361A8 File Offset: 0x000343A8
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000361BF File Offset: 0x000343BF
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000361D6 File Offset: 0x000343D6
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000361EC File Offset: 0x000343EC
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00036211 File Offset: 0x00034411
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00036228 File Offset: 0x00034428
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00036248 File Offset: 0x00034448
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00036264 File Offset: 0x00034464
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00036286 File Offset: 0x00034486
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000362B1 File Offset: 0x000344B1
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000362E3 File Offset: 0x000344E3
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x0400043F RID: 1087
		private readonly BsonBinaryWriter _writer;

		// Token: 0x04000440 RID: 1088
		private BsonToken _root;

		// Token: 0x04000441 RID: 1089
		private BsonToken _parent;

		// Token: 0x04000442 RID: 1090
		private string _propertyName;
	}
}
