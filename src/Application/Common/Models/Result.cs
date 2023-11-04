namespace Auth.Api.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<IdentityErrorResult> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; init; }

    public IdentityErrorResult[] Errors { get; init; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<IdentityErrorResult>());
    }

    public static Result Failure(IEnumerable<IdentityErrorResult> errors)
    {
        return new Result(false, errors);
    }
}

public class IdentityErrorResult
{
    public IdentityErrorResult(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; set; }
    public string Description { get; set; }
}
