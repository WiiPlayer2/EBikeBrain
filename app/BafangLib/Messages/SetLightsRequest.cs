using System;

namespace BafangLib.Messages;

public record SetLightsRequest(bool State) : Request;
