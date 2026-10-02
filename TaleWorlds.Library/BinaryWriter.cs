using System;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200001C RID: 28
	public class BinaryWriter : IWriter
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000031A0 File Offset: 0x000013A0
		public byte[] Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000031A8 File Offset: 0x000013A8
		public int Length
		{
			get
			{
				return this._availableIndex;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000031B0 File Offset: 0x000013B0
		public BinaryWriter()
		{
			this._data = new byte[4096];
			this._availableIndex = 0;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000031CF File Offset: 0x000013CF
		public BinaryWriter(int capacity)
		{
			this._data = new byte[capacity];
			this._availableIndex = 0;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000031EA File Offset: 0x000013EA
		public void Clear()
		{
			Array.Clear(this._data, 0, this._data.Length);
			this._availableIndex = 0;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003208 File Offset: 0x00001408
		public void EnsureLength(int added)
		{
			int num = this._availableIndex + added;
			if (num > this._data.Length)
			{
				int num2 = this._data.Length * 2;
				if (num > num2)
				{
					num2 = num;
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(this._data, 0, array, 0, this._availableIndex);
				this._data = array;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000325C File Offset: 0x0000145C
		public void WriteSerializableObject(ISerializableObject serializableObject)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003263 File Offset: 0x00001463
		public void WriteByte(byte value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = value;
			this._availableIndex++;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003288 File Offset: 0x00001488
		public void WriteBytes(byte[] bytes)
		{
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000032BC File Offset: 0x000014BC
		public void Write3ByteInt(int value)
		{
			this.EnsureLength(3);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003324 File Offset: 0x00001524
		public void WriteInt(int value)
		{
			this.EnsureLength(4);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000033A8 File Offset: 0x000015A8
		public void WriteShort(short value)
		{
			this.EnsureLength(2);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000033F4 File Offset: 0x000015F4
		public void WriteString(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				this.WriteInt(bytes.Length);
				this.WriteBytes(bytes);
				return;
			}
			this.WriteInt(0);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003430 File Offset: 0x00001630
		public void WriteFloats(float[] value, int count)
		{
			int num = count * 4;
			this.EnsureLength(num);
			Buffer.BlockCopy(value, 0, this._data, this._availableIndex, num);
			this._availableIndex += num;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000346C File Offset: 0x0000166C
		public void WriteShorts(short[] value, int count)
		{
			int num = count * 2;
			this.EnsureLength(num);
			Buffer.BlockCopy(value, 0, this._data, this._availableIndex, num);
			this._availableIndex += num;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000034A6 File Offset: 0x000016A6
		public void WriteColor(Color value)
		{
			this.WriteFloat(value.Red);
			this.WriteFloat(value.Green);
			this.WriteFloat(value.Blue);
			this.WriteFloat(value.Alpha);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000034D8 File Offset: 0x000016D8
		public void WriteBool(bool value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = (value ? 1 : 0);
			this._availableIndex++;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003504 File Offset: 0x00001704
		public void WriteFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003548 File Offset: 0x00001748
		public void WriteUInt(uint value)
		{
			this.EnsureLength(4);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000035CC File Offset: 0x000017CC
		public void WriteULong(ulong value)
		{
			this.EnsureLength(8);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
			byte[] data5 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data5[num] = (byte)(value >> 32);
			byte[] data6 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data6[num] = (byte)(value >> 40);
			byte[] data7 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data7[num] = (byte)(value >> 48);
			byte[] data8 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data8[num] = (byte)(value >> 56);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000036C4 File Offset: 0x000018C4
		public void WriteLong(long value)
		{
			this.EnsureLength(8);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
			byte[] data3 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data3[num] = (byte)(value >> 16);
			byte[] data4 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data4[num] = (byte)(value >> 24);
			byte[] data5 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data5[num] = (byte)(value >> 32);
			byte[] data6 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data6[num] = (byte)(value >> 40);
			byte[] data7 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data7[num] = (byte)(value >> 48);
			byte[] data8 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data8[num] = (byte)(value >> 56);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000037BC File Offset: 0x000019BC
		public void WriteVec2(Vec2 vec2)
		{
			this.WriteFloat(vec2.x);
			this.WriteFloat(vec2.y);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000037D6 File Offset: 0x000019D6
		public void WriteVec3(Vec3 vec3)
		{
			this.WriteFloat(vec3.x);
			this.WriteFloat(vec3.y);
			this.WriteFloat(vec3.z);
			this.WriteFloat(vec3.w);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003808 File Offset: 0x00001A08
		public void WriteVec3Int(Vec3i vec3)
		{
			this.WriteInt(vec3.X);
			this.WriteInt(vec3.Y);
			this.WriteInt(vec3.Z);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000382E File Offset: 0x00001A2E
		public void WriteSByte(sbyte value)
		{
			this.EnsureLength(1);
			this._data[this._availableIndex] = (byte)value;
			this._availableIndex++;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003854 File Offset: 0x00001A54
		public void WriteUShort(ushort value)
		{
			this.EnsureLength(2);
			byte[] data = this._data;
			int num = this._availableIndex;
			this._availableIndex = num + 1;
			data[num] = (byte)value;
			byte[] data2 = this._data;
			num = this._availableIndex;
			this._availableIndex = num + 1;
			data2[num] = (byte)(value >> 8);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000038A0 File Offset: 0x00001AA0
		public void WriteDouble(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.EnsureLength(bytes.Length);
			Buffer.BlockCopy(bytes, 0, this._data, this._availableIndex, bytes.Length);
			this._availableIndex += bytes.Length;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000038E3 File Offset: 0x00001AE3
		public void AppendData(BinaryWriter writer)
		{
			this.EnsureLength(writer._availableIndex);
			Buffer.BlockCopy(writer._data, 0, this._data, this._availableIndex, writer._availableIndex);
			this._availableIndex += writer._availableIndex;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003924 File Offset: 0x00001B24
		public byte[] GetFinalData()
		{
			byte[] array = new byte[this._availableIndex];
			Buffer.BlockCopy(this._data, 0, array, 0, this._availableIndex);
			return array;
		}

		// Token: 0x04000061 RID: 97
		private byte[] _data;

		// Token: 0x04000062 RID: 98
		private int _availableIndex;
	}
}
