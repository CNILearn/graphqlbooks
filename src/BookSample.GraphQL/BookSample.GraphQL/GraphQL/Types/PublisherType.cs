using BookSample.Data.Models;

namespace BookSample.GraphQL.GraphQL.Types;

internal class PublisherType : ObjectType<Publisher>
{
    protected override void Configure(IObjectTypeDescriptor<Publisher> descriptor)
    {
        descriptor.Field(x => x.Id).ID();
    }
}
