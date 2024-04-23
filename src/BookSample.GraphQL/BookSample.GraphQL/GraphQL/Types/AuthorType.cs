using BookSample.Data.Models;

namespace BookSample.GraphQL.GraphQL.Types;

internal class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Field(x => x.Id).ID();
        descriptor.Field("fullName")
            .Type<NonNullType<StringType>>()
            .Argument("lastNameUppercase", x => x.Type<BooleanType>())
            .Resolve(context =>
            {
                var lastNameUppercase = context.ArgumentOptional<bool>("lastNameUppercase");
                var author = context.Parent<Author>();
                var lastName = lastNameUppercase
                    ? author.LastName.ToUpperInvariant()
                    : author.LastName;
                return $"{author.FirstName} {lastName}";
            });
    }
}
