using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RolePickGoodsReq_8001007 : IMessage<RolePickGoodsReq_8001007>, IMessage, IEquatable<RolePickGoodsReq_8001007>, IDeepCloneable<RolePickGoodsReq_8001007>
{
	private static readonly MessageParser<RolePickGoodsReq_8001007> _parser = new MessageParser<RolePickGoodsReq_8001007>(() => new RolePickGoodsReq_8001007());

	private UnknownFieldSet _unknownFields;

	public const int ObjectIndexFieldNumber = 1;

	private int objectIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<RolePickGoodsReq_8001007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int ObjectIndex
	{
		get
		{
			return objectIndex_;
		}
		set
		{
			objectIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RolePickGoodsReq_8001007()
	{
	}

	[DebuggerNonUserCode]
	public RolePickGoodsReq_8001007(RolePickGoodsReq_8001007 other)
		: this()
	{
		objectIndex_ = other.objectIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RolePickGoodsReq_8001007 Clone()
	{
		return new RolePickGoodsReq_8001007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RolePickGoodsReq_8001007);
	}

	[DebuggerNonUserCode]
	public bool Equals(RolePickGoodsReq_8001007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ObjectIndex != other.ObjectIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ObjectIndex != 0)
		{
			num ^= ObjectIndex.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (ObjectIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ObjectIndex);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (ObjectIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ObjectIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RolePickGoodsReq_8001007 other)
	{
		if (other != null)
		{
			if (other.ObjectIndex != 0)
			{
				ObjectIndex = other.ObjectIndex;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				ObjectIndex = input.ReadInt32();
			}
		}
	}
}
