using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Reflection;

namespace Sparcpoint.Documentation.Sql
{
    public class DefaultSqlServerConstraintHandler : ISqlServerConstraintHandler
    {
        private readonly IServiceProvider _Provider;

        public DefaultSqlServerConstraintHandler(IServiceProvider provider)
        {
            _Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public bool CanHandle(DeferredTableConstraint constraint)
        {
            object service = GetService(constraint.Constraint);
            return service != null;
        }

        public void Handle(DeferredTableConstraint constraint, ISqlTree tree, SqlScriptGenerator generator)
        {
            object service = GetService(constraint.Constraint);
            Type serviceType = service.GetType();
            MethodInfo method = serviceType.GetMethod(nameof(ISqlServerConstraintHandler<ConstraintDefinition>.Handle));

            method.Invoke(service, new object[] { constraint.Table, constraint.Constraint, tree, generator });
        }

        private object GetService(ConstraintDefinition constraint)
        {
            Type handlerType = GetHandlerType(constraint);
            return _Provider.GetService(handlerType);
        }

        private Type GetHandlerType(ConstraintDefinition constraint)
        {
            Type inputType = constraint.GetType();
            return typeof(ISqlServerConstraintHandler<>).MakeGenericType(inputType);
        }
    }
}
