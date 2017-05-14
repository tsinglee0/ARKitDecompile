using System;

namespace Vuforia
{
	public interface InstanceId
	{
		InstanceIdType DataType
		{
			get;
		}

		byte[] Buffer
		{
			get;
		}

		string HexStringValue
		{
			get;
		}

		string StringValue
		{
			get;
		}

		ulong NumericValue
		{
			get;
		}
	}
}
