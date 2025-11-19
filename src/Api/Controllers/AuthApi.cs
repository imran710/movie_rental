using Api.Presentation.Common;
using Api.Presentation.Response;

using Core.Features.Auth.UseCase.EmailLogin;
using Core.Features.Auth.UseCase.EmailRegister;
using Core.Features.Auth.UseCase.ForgetPassword;
using Core.Features.Auth.UseCase.GetNewToken;
using Core.Features.Auth.UseCase.ResetPassword;
using Core.Features.Auth.UseCase.SendOtp;
using Core.Features.Auth.UseCase.VerifyOtp;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AuthApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/auth").WithTags("Auth");

        // Email auth
        group
            .MapPost("/v1/email/register", EmailRegister)
            .WithSummary("Register with email");
        group
            .MapPost("/v1/email/login", EmailLogin)
            .WithSummary("Login with email");

        // Token
        group
            .MapPost("/v1/token/new", GetNewToken)
            .WithSummary("Get new token");

    /*    // OTP
        group
            .MapPost("/v1/otp/verify", OtpVerify)
            .WithSummary("Verify OTP for registration");
        group
            .MapPost("/v1/otp/resend-otp-register", SendOtp)
            .WithSummary("Resend OTP for registration");
        group
            .MapPost("/v1/password/forget", ForgetPassword)
            .WithSummary("Forget password");
        group
            .MapPost("/v1/password/reset", ResetPassword)
            .WithSummary("Reset password");*/
    }

    private static async Task<Results<Ok<ApiResponse<EmailRegisterResponse>>, JsonHttpResult<ApiResponse<EmailRegisterResponse>>>> EmailRegister(
        [FromBody] EmailRegisterRequest request,
        [FromServices] EmailRegisterHandler emailRegisterHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await emailRegisterHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<EmailLoginResponse>>, JsonHttpResult<ApiResponse<EmailLoginResponse>>>> EmailLogin(
        [FromBody] EmailLoginRequest request,
        [FromServices] EmailLoginHandler emailLoginHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await emailLoginHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<ApiResponse<GetNewTokenResponse>>, JsonHttpResult<ApiResponse<GetNewTokenResponse>>>> GetNewToken(
        [FromBody] GetNewTokenRequest request,
        [FromServices] GetNewTokenHandler emailLoginHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await emailLoginHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }

    private static async Task<Results<Ok<EmptyApiResponse>, JsonHttpResult<EmptyApiResponse>>> OtpVerify(
        [FromBody] VerityOtpRequest request,
        [FromServices] VerifyOtpHandler verifyOtpHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await verifyOtpHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return EmptyApiResponseResult.Problem(result);

        return EmptyApiResponseResult.Success();
    }

    private static async Task<Results<Ok<EmptyApiResponse>, JsonHttpResult<EmptyApiResponse>>> SendOtp(
        [FromBody] SendOtpRequest request,
        [FromServices] SendOtpHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return EmptyApiResponseResult.Problem(result);

        return EmptyApiResponseResult.Success();
    }

    private static async Task<Results<Ok<EmptyApiResponse>, JsonHttpResult<EmptyApiResponse>>> ForgetPassword(
        [FromBody] ForgetPasswordRequest request,
        [FromServices] ForgetPasswordHandler verifyOtpHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await verifyOtpHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return EmptyApiResponseResult.Problem(result);

        return EmptyApiResponseResult.Success();
    }

    private static async Task<Results<Ok<EmptyApiResponse>, JsonHttpResult<EmptyApiResponse>>> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        [FromServices] ResetPasswordHandler verifyOtpHandler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await verifyOtpHandler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return EmptyApiResponseResult.Problem(result);

        return EmptyApiResponseResult.Success();
    }
}
