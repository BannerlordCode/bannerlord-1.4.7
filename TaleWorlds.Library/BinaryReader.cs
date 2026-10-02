using System;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200001B RID: 27
	public class BinaryReader : IReader
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002DD9 File Offset: 0x00000FD9
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002DE1 File Offset: 0x00000FE1
		public byte[] Data { get; private set; }

		// Token: 0x06000053 RID: 83 RVA: 0x00002DEA File Offset: 0x00000FEA
		public BinaryReader(byte[] data)
		{
			this.Data = data;
			this._cursor = 0;
			this._buffer = new byte[4];
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002E0C File Offset: 0x0000100C
		public int UnreadByteCount
		{
			get
			{
				return this.Data.Length - this._cursor;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002E1D File Offset: 0x0000101D
		public ISerializableObject ReadSerializableObject()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002E24 File Offset: 0x00001024
		public int Read3ByteInt()
		{
			this._buffer[0] = this.ReadByte();
			this._buffer[1] = this.ReadByte();
			this._buffer[2] = this.ReadByte();
			if (this._buffer[0] == 255 && this._buffer[1] == 255 && this._buffer[2] == 255)
			{
				this._buffer[3] = byte.MaxValue;
			}
			else
			{
				this._buffer[3] = 0;
			}
			return BitConverter.ToInt32(this._buffer, 0);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002EAC File Offset: 0x000010AC
		public int ReadInt()
		{
			int num = BitConverter.ToInt32(this.Data, this._cursor);
			this._cursor += 4;
			return num;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002ECD File Offset: 0x000010CD
		public short ReadShort()
		{
			short num = BitConverter.ToInt16(this.Data, this._cursor);
			this._cursor += 2;
			return num;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002EF0 File Offset: 0x000010F0
		public void ReadFloats(float[] output, int count)
		{
			int num = count * 4;
			Buffer.BlockCopy(this.Data, this._cursor, output, 0, num);
			this._cursor += num;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002F24 File Offset: 0x00001124
		public void ReadShorts(short[] output, int count)
		{
			int num = count * 2;
			Buffer.BlockCopy(this.Data, this._cursor, output, 0, num);
			this._cursor += num;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002F58 File Offset: 0x00001158
		public string ReadString()
		{
			int num = this.ReadInt();
			string text = null;
			if (num >= 0)
			{
				text = Encoding.UTF8.GetString(this.Data, this._cursor, num);
				this._cursor += num;
			}
			return text;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002F9C File Offset: 0x0000119C
		public Color ReadColor()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Color(num, num2, num3, num4);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002FCE File Offset: 0x000011CE
		public bool ReadBool()
		{
			int num = (int)this.Data[this._cursor];
			this._cursor++;
			return num == 1;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002FEE File Offset: 0x000011EE
		public float ReadFloat()
		{
			float num = BitConverter.ToSingle(this.Data, this._cursor);
			this._cursor += 4;
			return num;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000300F File Offset: 0x0000120F
		public uint ReadUInt()
		{
			uint num = BitConverter.ToUInt32(this.Data, this._cursor);
			this._cursor += 4;
			return num;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003030 File Offset: 0x00001230
		public ulong ReadULong()
		{
			ulong num = BitConverter.ToUInt64(this.Data, this._cursor);
			this._cursor += 8;
			return num;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003051 File Offset: 0x00001251
		public long ReadLong()
		{
			long num = BitConverter.ToInt64(this.Data, this._cursor);
			this._cursor += 8;
			return num;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003072 File Offset: 0x00001272
		public byte ReadByte()
		{
			byte b = this.Data[this._cursor];
			this._cursor++;
			return b;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003090 File Offset: 0x00001290
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Array.Copy(this.Data, this._cursor, array, 0, length);
			this._cursor += length;
			return array;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000030C8 File Offset: 0x000012C8
		public Vec2 ReadVec2()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			return new Vec2(num, num2);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000030E8 File Offset: 0x000012E8
		public Vec3 ReadVec3()
		{
			float num = this.ReadFloat();
			float num2 = this.ReadFloat();
			float num3 = this.ReadFloat();
			float num4 = this.ReadFloat();
			return new Vec3(num, num2, num3, num4);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003118 File Offset: 0x00001318
		public Vec3i ReadVec3Int()
		{
			int num = this.ReadInt();
			int num2 = this.ReadInt();
			int num3 = this.ReadInt();
			return new Vec3i(num, num2, num3);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003140 File Offset: 0x00001340
		public sbyte ReadSByte()
		{
			sbyte b = (sbyte)this.Data[this._cursor];
			this._cursor++;
			return b;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000315E File Offset: 0x0000135E
		public ushort ReadUShort()
		{
			ushort num = BitConverter.ToUInt16(this.Data, this._cursor);
			this._cursor += 2;
			return num;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000317F File Offset: 0x0000137F
		public double ReadDouble()
		{
			double num = BitConverter.ToDouble(this.Data, this._cursor);
			this._cursor += 8;
			return num;
		}

		// Token: 0x0400005E RID: 94
		private int _cursor;

		// Token: 0x0400005F RID: 95
		private byte[] _buffer;
	}
}
