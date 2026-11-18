using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TitleUseReq_1002007 : IMessage<TitleUseReq_1002007>, IMessage, IEquatable<TitleUseReq_1002007>, IDeepCloneable<TitleUseReq_1002007>
{
	private static readonly MessageParser<TitleUseReq_1002007> _parser = new MessageParser<TitleUseReq_1002007>(() => new TitleUseReq_1002007());

	private UnknownFieldSet _unknownFields;

	public const int TitleFieldNumber = 1;

	private int title_;

	[DebuggerNonUserCode]
	public static MessageParser<TitleUseReq_1002007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Title
	{
		get
		{
			return title_;
		}
		set
		{
			title_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TitleUseReq_1002007()
	{
	}

	[DebuggerNonUserCode]
	public TitleUseReq_1002007(TitleUseReq_1002007 other)
		: this()
	{
		title_ = other.title_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TitleUseReq_1002007 Clone()
	{
		return new TitleUseReq_1002007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TitleUseReq_1002007);
	}

	[DebuggerNonUserCode]
	public bool Equals(TitleUseReq_1002007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Title != other.Title)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Title != 0)
		{
			num ^= Title.GetHashCode();
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
		if (Title != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Title);
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
		if (Title != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Title);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TitleUseReq_1002007 other)
	{
		if (other != null)
		{
			if (other.Title != 0)
			{
				Title = other.Title;
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
				Title = input.ReadInt32();
			}
		}
	}
}
