using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsBookElixirDetailReq_1004009 : IMessage<GoodsBookElixirDetailReq_1004009>, IMessage, IEquatable<GoodsBookElixirDetailReq_1004009>, IDeepCloneable<GoodsBookElixirDetailReq_1004009>
{
	private static readonly MessageParser<GoodsBookElixirDetailReq_1004009> _parser = new MessageParser<GoodsBookElixirDetailReq_1004009>(() => new GoodsBookElixirDetailReq_1004009());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private string id_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBookElixirDetailReq_1004009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsBookElixirDetailReq_1004009()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBookElixirDetailReq_1004009(GoodsBookElixirDetailReq_1004009 other)
		: this()
	{
		id_ = other.id_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBookElixirDetailReq_1004009 Clone()
	{
		return new GoodsBookElixirDetailReq_1004009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBookElixirDetailReq_1004009);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBookElixirDetailReq_1004009 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
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
		if (Id.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Id);
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
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsBookElixirDetailReq_1004009 other)
	{
		if (other != null)
		{
			if (other.Id.Length != 0)
			{
				Id = other.Id;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				Id = input.ReadString();
			}
		}
	}
}
