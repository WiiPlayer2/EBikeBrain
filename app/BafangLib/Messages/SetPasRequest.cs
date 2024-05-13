using System;

namespace BafangLib.Messages;

public record SetPasRequest(Pas Level) : Request;
