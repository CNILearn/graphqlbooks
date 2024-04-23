using BookSample.Data.Models;

namespace BookSample.GraphQL.GraphQL.Types;

internal class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(x => x.Id).ID();
        descriptor.Field(x => x.AuthorId).Ignore();
        descriptor.Field(x => x.GenreId).Ignore();
        descriptor.Field(x => x.PublisherId).Ignore();
        //descriptor.Field(x => x.Title)
        //    .Name("BookName")
        //    .Description("ASDfasdfasf");
        //descriptor.Field(x => x.ISBN).Ignore();
        //descriptor.Field(x => x.LastModifiedAt).Deprecated("sakldöfjasldöfkj");

        descriptor.Field("myNewCodeFirstField")
            .Type<NonNullType<StringType>>()
            .Resolve(context =>
            {
                // optional: get service here and ....
                return "The new value";
            });
    }
}
