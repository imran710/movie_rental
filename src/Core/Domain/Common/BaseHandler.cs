using Microsoft.AspNetCore.Http;

namespace Core.Domain.Common;

public abstract class BaseHandler<TRequest, TResponse>
{
    protected HttpContext _httpContext = null!;

    protected virtual ValueTask<Result<Success>> BeforeProcess(TRequest request, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new Result<Success>(SuccessBuilder.Default));
    }

    protected virtual ValueTask<Result<Success>> ValidateRequest(TRequest request, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new Result<Success>(SuccessBuilder.Default));
    }

    protected abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);

    protected virtual ValueTask<Result<Success>> AfterProcess(TRequest request, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new Result<Success>(SuccessBuilder.Default));
    }

    public virtual async Task<Result<TResponse>> Execute(TRequest request, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        _httpContext = httpContext;

        var beforeProcessResult = await BeforeProcess(request, cancellationToken).ConfigureAwait(false);
        if (beforeProcessResult.IsError)
            return beforeProcessResult.Error;

        var validateResult = await ValidateRequest(request, cancellationToken).ConfigureAwait(false);
        if (validateResult.IsError)
            return validateResult.Error;

        var handleResult = await Handle(request, cancellationToken);
        var afterProcessResult = await AfterProcess(request, cancellationToken).ConfigureAwait(false);

        if (handleResult.IsError)
            return handleResult.Error;

        return afterProcessResult.IsError ? afterProcessResult.Error : handleResult;
    }
}
