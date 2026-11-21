using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class Msg : IMessage<Msg>, IMessage, IEquatable<Msg>, IDeepCloneable<Msg>
{
	private static readonly MessageParser<Msg> _parser = new MessageParser<Msg>(() => new Msg());

	private UnknownFieldSet _unknownFields;

	public const int CmdFieldNumber = 1;

	private int cmd_;

	public const int MsidFieldNumber = 2;

	private int msid_;

	public const int DataFieldNumber = 3;

	private ByteString data_ = ByteString.Empty;

	[DebuggerNonUserCode]
	public static MessageParser<Msg> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PacketProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Cmd
	{
		get
		{
			return cmd_;
		}
		set
		{
			cmd_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Msid
	{
		get
		{
			return msid_;
		}
		set
		{
			msid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ByteString Data
	{
		get
		{
			return data_;
		}
		set
		{
			data_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public Msg()
	{
	}

	[DebuggerNonUserCode]
	public Msg(Msg other)
		: this()
	{
		cmd_ = other.cmd_;
		msid_ = other.msid_;
		data_ = other.data_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Msg Clone()
	{
		return new Msg(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Msg);
	}

	[DebuggerNonUserCode]
	public bool Equals(Msg other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Cmd != other.Cmd)
		{
			return false;
		}
		if (Msid != other.Msid)
		{
			return false;
		}
		if (Data != other.Data)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Cmd != 0)
		{
			num ^= Cmd.GetHashCode();
		}
		if (Msid != 0)
		{
			num ^= Msid.GetHashCode();
		}
		if (Data.Length != 0)
		{
			num ^= Data.GetHashCode();
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
		if (Cmd != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Cmd);
		}
		if (Msid != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Msid);
		}
		if (Data.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteBytes(Data);
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
		if (Cmd != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Cmd);
		}
		if (Msid != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Msid);
		}
		if (Data.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeBytesSize(Data);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Msg other)
	{
		if (other != null)
		{
			if (other.Cmd != 0)
			{
				Cmd = other.Cmd;
			}
			if (other.Msid != 0)
			{
				Msid = other.Msid;
			}
			if (other.Data.Length != 0)
			{
				Data = other.Data;
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				Cmd = input.ReadInt32();
				break;
			case 16u:
				Msid = input.ReadInt32();
				break;
			case 26u:
				Data = input.ReadBytes();
				break;
			}
		}
	}
}
