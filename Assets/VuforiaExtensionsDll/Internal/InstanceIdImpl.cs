using System;
using System.Text;

namespace Vuforia
{
	internal class InstanceIdImpl : InstanceId
	{
		private InstanceIdType mDataType;

		private byte[] mBuffer;

		private ulong mNumericValue;

		private uint mDataLength;

		private string mCachedStringValue;

		public InstanceIdType DataType
		{
			get
			{
				return this.mDataType;
			}
		}

		public byte[] Buffer
		{
			get
			{
				return this.mBuffer;
			}
		}

		public string HexStringValue
		{
			get
			{
				byte[] array = new byte[this.mBuffer.Length];
				Array.Copy(this.mBuffer, array, this.mBuffer.Length);
				Array.Reverse(array);
				string str = BitConverter.ToString(array).Replace("-", string.Empty);
				return "0x" + str;
			}
		}

		public string StringValue
		{
			get
			{
				return this.mCachedStringValue;
			}
		}

		public ulong NumericValue
		{
			get
			{
				return this.mNumericValue;
			}
		}

		public InstanceIdImpl(byte[] buffer, ulong numericValue, InstanceIdType dataType, uint dataLength)
		{
			this.mBuffer = buffer;
			this.mDataType = dataType;
			this.mDataLength = dataLength;
			this.mNumericValue = numericValue;
			this.mCachedStringValue = "";
			if (this.mDataType == InstanceIdType.STRING)
			{
				byte[] array = new byte[this.mBuffer.Length];
				Array.Copy(this.mBuffer, array, this.mBuffer.Length);
				Array.Reverse(array);
				this.mCachedStringValue = Encoding.ASCII.GetString(buffer);
			}
		}

		public override string ToString()
		{
			switch (this.DataType)
			{
			case InstanceIdType.BYTES:
				return this.HexStringValue;
			case InstanceIdType.STRING:
				return this.StringValue;
			case InstanceIdType.NUMERIC:
				return this.NumericValue.ToString();
			default:
				return "Unknown";
			}
		}
	}
}
