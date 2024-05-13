using System;

namespace BafangLib.Messages;

public abstract record Request;

public record GetRpmRequest : Request;
