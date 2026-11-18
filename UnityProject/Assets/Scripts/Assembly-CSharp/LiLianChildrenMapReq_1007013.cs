using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class LiLianChildrenMapReq_1007013 : IMessage<LiLianChildrenMapReq_1007013>, IMessage, IEquatable<LiLianChildrenMapReq_1007013>, IDeepCloneable<LiLianChildrenMapReq_1007013>
{
	private static readonly MessageParser<LiLianChildrenMapReq_1007013> _parser = new MessageParser<LiLianChildrenMapReq_1007013>(() => new LiLianChildrenMapReq_1007013());

	private UnknownFieldSet _unknownFields;

	public const int MapIdFieldNumber = 1;

	private string mapId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<LiLianChildrenMapReq_1007013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MapProtoReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MapId
	{
		get
		{
			return mapId_;
		}
		set
		{
			mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public LiLianChildrenMapReq_1007013()
	{
	}

	[DebuggerNonUserCode]
	public LiLianChildrenMapReq_1007013(LiLianChildrenMapReq_1007013 other)
		: this()
	{
		mapId_ = other.mapId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LiLianChildrenMapReq_1007013 Clone()
	{
		return new LiLianChildrenMapReq_1007013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LiLianChildrenMapReq_1007013);
	}

	[DebuggerNonUserCode]
	public bool Equals(LiLianChildrenMapReq_1007013 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
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
		if (MapId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MapId);
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
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LiLianChildrenMapReq_1007013 other)
	{
		if (other != null)
		{
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
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
				MapId = input.ReadString();
			}
		}
	}
}
