using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pets.Persistence
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> leftExpression,
            Expression<Func<T, bool>> rightExpression) =>
            Combine(leftExpression, rightExpression, Expression.AndAlso);

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression,
            Expression<Func<T, bool>> rightExpression) =>
            Combine(leftExpression, rightExpression, Expression.Or);

        public static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression, Func<Expression, Expression, BinaryExpression> combineOperator)
        {
            var leftParameter = leftExpression.Parameters[0];
            var rightParameter = rightExpression.Parameters[0];

            var visitor = new ReplaceParameterVisitor(rightParameter, leftParameter);

            var leftBody = leftExpression.Body;
            var rightBody = visitor.Visit(rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(combineOperator(leftBody, rightBody), leftParameter);
        }

        //public static Expression<Func<T, bool>> Combine<T>(List<Expression<Func<T, bool>>> expressions, Func<Expression, Expression, BinaryExpression> combineOperator)
        //{
        //    var prevExp = expressions[0];
        //    for (int i = 1; i < expressions.Count; i++)
        //    {
        //        var curExprParam = expressions[i];

        //        prevExp = Combine<T>(prevExp, curExprParam, combineOperator);
        //    }
        //}

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return ReferenceEquals(node, _oldParameter) ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}
