namespace Caiman.Core.Construction.Exceptions;

public class ConstraintNotFoundException(string message = "Constraint not found") : Exception(message);
