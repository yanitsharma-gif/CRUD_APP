using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Practice.Responses
{
    public class UserResponse
    {
        public string Message { get; set; } = string.Empty;
        public int Success { get; set; }
    }
}
