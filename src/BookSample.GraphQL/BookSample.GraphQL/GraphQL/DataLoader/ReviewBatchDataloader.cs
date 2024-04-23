using BookSample.ReviewAPIClient;
using BookSample.ReviewAPIClient.Models;

namespace BookSample.GraphQL.GraphQL.DataLoader;

internal class ReviewBatchDataloader(ReviewClient reviewClient, IBatchScheduler batchScheduler, DataLoaderOptions? options = null)
    : BatchDataLoader<long, IEnumerable<Review>>(batchScheduler, options)
{
    private static readonly List<Review> s_emptyReviewList = [];

    internal int? TakeParameter { get; set; }

    protected override async Task<IReadOnlyDictionary<long, IEnumerable<Review>>> LoadBatchAsync(IReadOnlyList<long> bookIds, CancellationToken cancellationToken)
    {
        var parameterQueryIds = bookIds.Select(x => (long?)x).ToArray();
        var result = await reviewClient.Api
            .Reviews
            .GetAsync(config =>
            {
                config.QueryParameters.BookIds = parameterQueryIds;
                config.QueryParameters.Take = TakeParameter;
            }, cancellationToken)
            ?? s_emptyReviewList;
        return result
            .Where(x => x.BookId != default)
            .GroupBy(x => x.BookId!.Value)
            .ToDictionary(x => x.Key, x => x.AsEnumerable());
    }
}
