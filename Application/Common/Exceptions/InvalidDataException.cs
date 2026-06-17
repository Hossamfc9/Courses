namespace Application.Common.Exceptions;

public class InvalidDataException(Guid Id, string Entity) : 
    Exception($"the {Entity} with Id: {Id} can't be processed, try different {Entity}");