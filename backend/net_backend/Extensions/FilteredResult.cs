using Microsoft.EntityFrameworkCore;
using CustomAttributes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace CustomExtensions
{
    public static partial class ExpressionExtensions
    {
        private static Expression BuildNested(ParameterExpression param, List<FilterRule> filters, string op)
        {
            Expression? body = null;

            foreach (var rule in filters)
            {
                if (rule.Op == "and" || rule.Op == "or")
                {
                    Expression comparison = BuildNested(param, rule.Sub, rule.Op);
                    if (op == "and")
                        body = body == null ? comparison : Expression.AndAlso(body, comparison);
                    else
                        body = body == null ? comparison : Expression.OrElse(body, comparison);
                }
                else if (rule.Field != null)
                {
                    var member = Expression.PropertyOrField(param, rule.Field);
                    var value = Expression.Constant(Convert.ChangeType(rule.Value, member.Type));

                    Expression comparison = rule.Op switch
                    {
                        "=" => Expression.Equal(member, value),
                        "in" => Expression.Call(member, "Contains", null, value),
                        "start" => Expression.Call(member, "StartsWith", null, value),
                        "end" => Expression.Call(member, "EndsWith", null, value),
                        ">" => Expression.GreaterThan(member, value),
                        "<" => Expression.LessThan(member, value),
                        ">=" => Expression.GreaterThanOrEqual(member, value),
                        "<=" => Expression.LessThanOrEqual(member, value),
                        "any" => Expression.Constant(true),
                        _ => throw new NotSupportedException($"Operation {rule.Op} is not supported")
                    };

                    if (op == "and")
                        body = body == null ? comparison : Expression.AndAlso(body, comparison);
                    else
                        body = body == null ? comparison : Expression.OrElse(body, comparison);
                }


            }

            return body ?? Expression.Constant(true);
        }
        public static Expression<Func<T, bool>> BuildPredicate<T>(this FilterRule rule)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? body = null;

            if (rule.Op == "and" || rule.Op == "or")
            {
                body = BuildNested(parameter, rule.Sub, rule.Op);
            }
            else if (rule.Field != null)
            {
                Console.WriteLine($"Field is: {rule.Field}");
                var member = Expression.PropertyOrField(parameter, rule.Field);
                var value = Expression.Constant(Convert.ChangeType(rule.Value, member.Type));

                Expression comparison = rule.Op switch
                {
                    "=" => Expression.Equal(member, value),
                    "in" => Expression.Call(member, "Contains", null, value),
                    "start" => Expression.Call(member, "StartsWith", null, value),
                    "end" => Expression.Call(member, "EndsWith", null, value),
                    ">" => Expression.GreaterThan(member, value),
                    "<" => Expression.LessThan(member, value),
                    ">=" => Expression.GreaterThanOrEqual(member, value),
                    "<=" => Expression.LessThanOrEqual(member, value),
                    "any" => Expression.Constant(true),
                    _ => throw new NotSupportedException($"Operation {rule.Op} is not supported")
                };

                body = comparison;
            }

            return Expression.Lambda<Func<T, bool>>(body ?? Expression.Constant(true), parameter);
        }
    }
}
