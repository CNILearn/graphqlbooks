using BookSample.Data.Models;
using BookSample.GraphQL.GraphQL.DataLoader;
using BookSample.ReviewAPIClient;
using BookSample.ReviewAPIClient.Models;

namespace BookSample.GraphQL.GraphQL.TypeExtensions;

[ExtendObjectType<Book>]
internal class BookTypeExtension
{
    [GraphQLDescription("Just for demonstration")]
    public string MyAnnotationBasedField { get; } = "My value from annotation based field";

    // Loading reviews without using the DataLoader
    //public async Task<IEnumerable<Review>> GetReviewsAsync([Parent] Book book, [Service] ReviewClient reviewClient, [Service] ILogger)
    //{
    //    return (await reviewClient.Api.Reviews.GetAsync(options =>
    //    {
    //        options.QueryParameters.BookIds = [book.Id.ToString()];
    //    })) ?? [];
    //}

    // Instead of using the ReviewClient directly (as in the commented method above), we use the DataLoader "ReviewBatchDataloader".
    public async Task<IEnumerable<Review>> GetReviews([Parent] Book book, [Argument] int? take, ReviewBatchDataloader reviewBatchDataloader, CancellationToken cancellationToken)
    {
        reviewBatchDataloader.TakeParameter = take;
        return await reviewBatchDataloader.LoadAsync(book.Id, cancellationToken);
    }

    // Loading the rating without using the DataLoader
    //public async Task<Rating?> GetRatingAsync([Parent] Book book, [Service] ReviewClient reviewClient)
    //{
    //    return (await reviewClient.Api.Ratings.GetAsync(options =>
    //    {
    //        options.QueryParameters.BookIds = [book.Id.ToString()];
    //    }))?.FirstOrDefault();
    //}

    // Using the source-generator to create a DataLoader for the Rating
    // The source-generators of HotChocolate are available in the HotChocolate.Types.Analyzers NuGet package
    [DataLoader(AccessModifier = DataLoaderAccessModifier.PublicInterface)]
    internal static async Task<IReadOnlyDictionary<long, Rating>> RatingById(IReadOnlyList<long> bookIds, ReviewClient reviewClient, CancellationToken cancellationToken)
    {
        var parameterQueryIds = bookIds.Select(x => (long?)x).ToArray();
        var result = await reviewClient.Api
            .Ratings
            .GetAsync(x => x.QueryParameters.BookIds = parameterQueryIds, cancellationToken);

        return result?
            .Where(x => x.BookId.HasValue)
            .ToDictionary(x => x.BookId!.Value)
            ?? [];
    }

    // Using the DataLoader for the Rating (created by the source-generator)
    public async Task<Rating?> GetRatingAsync([Parent] Book book, IRatingByIdDataLoader dataLoader, CancellationToken cancellationToken) =>
        await dataLoader.LoadAsync(book.Id, cancellationToken);
}
