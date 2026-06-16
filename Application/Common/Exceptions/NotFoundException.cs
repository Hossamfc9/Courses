namespace Application.Common.Exceptions;

public class NotFoundException(Guid Id, string Entity) : 
    Exception($"the {Entity} with Id:{Id} not found");