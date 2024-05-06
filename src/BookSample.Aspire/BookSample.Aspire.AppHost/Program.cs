var builder = DistributedApplication.CreateBuilder(args);

// Database
var database = builder
    .AddPostgres("pg")
    .AddDatabase("books");

// Database-Seeder
builder
    .AddProject<Projects.BookSample_DatabaseSeeder>("db-seeder")
    .WithReference(database);

// Reviews API
var reviewsApi = builder
    .AddProject<Projects.BookSample_ReviewAPI>("review-api");

// GraphQL API
var graphQLApi = builder
    .AddProject<Projects.BookSample_GraphQL>("graphql-api")
    .WithReference(database)
    .WithReference(reviewsApi);

builder.Build().Run();
