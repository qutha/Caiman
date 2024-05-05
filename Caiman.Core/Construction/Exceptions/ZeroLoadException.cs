namespace Caiman.Core.Construction.Exceptions;

public class ZeroLoadException(string message = "Zero load is not allowed") : Exception(message);
