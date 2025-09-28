using GraphQL;
using GraphQL.Types;
using GraphQL.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
var app = builder.Build();

app.UseCors("AllowAll");
var store = new UserStore();

var schema = new Schema
{
    Query = new RootQuery(store),
    Mutation = new UserMutation(store)
};

app.MapPost("/graphql", async (GraphQLRequest req) =>
{
    var executer = new DocumentExecuter();
    var result = await executer.ExecuteAsync(opts =>
    {
        opts.Schema = schema;
        opts.Query = req.Query;
        opts.OperationName = req.OperationName;
        if (req.Variables is not null)
            opts.Variables = req.Variables.ToInputs();
    });

    var json = new GraphQLSerializer(indent: true).Serialize(result);
    return Results.Text(json, "application/json");
});

app.MapGet("/", () => "POST GraphQL to /graphql");
app.Run();

public sealed class GraphQLRequest
{
    public string Query { get; set; } = default!;
    public string? OperationName { get; set; }
    public Dictionary<string, object?>? Variables { get; set; }
}

public sealed class User
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}
public sealed class UserType : ObjectGraphType<User>
{
    public UserType()
    {
        Field(x => x.Id).Description("User id");
        Field(x => x.Name).Description("User name");
    }
}


public sealed class RootQuery : ObjectGraphType
{
    public RootQuery(UserStore store)
    {
        Field<ListGraphType<UserType>>("users")
            .Description("All users")
            .Resolve(_ => store.All());
    }
}

public sealed class UserMutation : ObjectGraphType
{
    public UserMutation(UserStore store)
    {
        Field<UserType>(
            "createUser",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
            ),
            resolve: ctx => store.Add(ctx.GetArgument<string>("name"))
        );
    }
}

public sealed class UserStore
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public User Add(string name)
    {
        lock (_users)
        {
            var u = new User { Id = _nextId.ToString(), Name = name };
            _users.Add(u);
            _nextId++;
            return u;
        }
    }

    public IReadOnlyList<User> All()
    {
        return _users.ToArray();
    }
}
