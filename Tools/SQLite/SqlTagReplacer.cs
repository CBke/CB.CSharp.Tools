using CB.CSharp.Extentions;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace CB.CSharp.Tools.SQLite
{
    public sealed class SqlTagReplacer : IDbCommandInterceptor
    {
        public string Tag { get; set; }
        public string Replacement { get; set; }

        void IDbCommandInterceptor.NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        void IDbCommandInterceptor.NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        void IDbCommandInterceptor.ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        void IDbCommandInterceptor.ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) =>
            this.RewriteQuery(command);

        void IDbCommandInterceptor.ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        void IDbCommandInterceptor.ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) =>
            this.RewriteQuery(command);
    }
}