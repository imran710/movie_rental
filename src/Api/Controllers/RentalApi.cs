using Api.Presentation.Common;
using Api.Presentation.Response;
using Core.Features.Rentals.UseCases.GetMovies;
using Core.Features.Rentals.UseCases.MovieRental;
using Core.Features.Rentals.UseCases.RentalHistory;
using Core.Features.Rentals.UseCases.ReturnRental;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class RentalApi : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var userGroup = routes.MapGroup("/rental").WithTags("rental").RequireAuthorization(); 

        userGroup
            .MapPost("/v1/api/rentals/checkout", ChekoutRental)
            .WithSummary("rent a movie ");
        userGroup
            .MapPost("/v1/api/rentals/return", ReturnRental)
            .WithSummary("Return Rental movies");
        userGroup
              .MapGet("/v1/api/movies/available", GetAvailableMovies)
              .WithSummary("See available movie");
        userGroup
            .MapGet("/v1/api/customers/rental-history", GetRentalHistory)
            .WithSummary("My Rental history ");

    }
    private static async Task<Results<Ok<ApiResponse<MovieRentalResponse>>, JsonHttpResult<ApiResponse<MovieRentalResponse>>>> ChekoutRental(
        [FromBody] MovieRentalRequest request,
        [FromServices] MovieRentalHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
    private static async Task<Results<Ok<ApiResponse<ReturnRentalResponse>>, JsonHttpResult<ApiResponse<ReturnRentalResponse>>>> ReturnRental(
     [FromBody] ReturnRentalRequest request,
     [FromServices] ReturnRentalHandler handler,
     HttpContext httpContext,
     CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
    private static async Task<Results<Ok<ApiResponse<GetMoviesResponse>>, JsonHttpResult<ApiResponse<GetMoviesResponse>>>> GetAvailableMovies(
       
       [FromServices] GetMoviesHandler handler,
       HttpContext httpContext,
       CancellationToken cancellationToken = default)
    {
        var request = new GetMoviesRequest();
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
    private static async Task<Results<Ok<ApiResponse<RentalHistoryResponse>>, JsonHttpResult<ApiResponse<RentalHistoryResponse>>>> GetRentalHistory(

    [FromServices] RentalHistoryHandler handler,
    HttpContext httpContext,
    CancellationToken cancellationToken = default)
    {
        var request = new RentalHistoryRequest();
        var result = await handler.Execute(request, httpContext, cancellationToken).ConfigureAwait(false);
        if (result.IsError)
            return ApiResponseResult.Problem(result);

        return ApiResponseResult.Success(result);
    }
}
