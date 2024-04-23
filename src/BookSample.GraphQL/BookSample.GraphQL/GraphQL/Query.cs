namespace BookSample.GraphQL.GraphQL;

public class Query
{
    public string HelloWorld { get; } = "Hello world";

    public string HelloUniverse() =>
        "Hello universe";

    public string WhatTimeIsIt() =>
        $"The current time is: {DateTime.Now:HH:mm:ss}";

    public int Sum(int a, int b) =>
        a + b;

    //public DummyUser MyDummyUser => new()
    //{
    //    FirstName = "Sebastian",
    //    LastName = "Szvetecz",
    //    UserName = "sebastian.szvetecz",
    //    EMail = "sebastian@cninnovation.com",
    //    Phone = "0000"
    //};
}

[GraphQLName("User")]
[GraphQLDescription("My cool user type")]
public class DummyUser
{
    public required string FirstName {  get; set; }

    public required string LastName {  get; set; }

    [GraphQLDescription("The username")]
    public required string UserName { get; set; }

    [GraphQLName("eMailAddress")]
    [GraphQLDescription("The mail")]
    public required string EMail { get; set; }

    public required string Phone { get; set; }

    [GraphQLDeprecated("Skype not common anymore")]
    public string SkypeName { get; set; } = string.Empty;

    [GraphQLIgnore]
    public string Test() => "Asdfasdfsd";

    public string FullName => $"{FirstName} {LastName}";
}