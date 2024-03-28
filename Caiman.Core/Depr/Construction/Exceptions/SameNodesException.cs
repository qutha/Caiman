namespace Caiman.Core.Depr.Construction.Exceptions;

public class SameNodesException(string message = "Start and end nodes are the same") : Exception(message);
