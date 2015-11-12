using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace angularjs
{
    public static class ajs
    {
        

        public static string textbox_cshtml = "textbox.cshtml";


        public static string getClass<T>(Expression<Func<T, object>> expression) {
            string str = expression.Parameters[0].Type.Name;
            return str;
        }

        public static string field<T>(
            this T instance,
            Expression<Func<T, object>> expression)
        {
            return field(expression);
        }

        public static string field<T>(
            Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return field(expression.Body);
        }

        public static string field<T>(
            this T instance,
            Expression<Action<T>> expression)
        {
            return method(expression);
        }

        public static string method<T>(
            Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return field(expression.Body);
        }

        private static string field(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                MemberExpression memberExpression =
                    (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                MethodCallExpression methodCallExpression =
                    (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                UnaryExpression unaryExpression = (UnaryExpression)expression;
                return field(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string field(
            UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                MethodCallExpression methodExpression =
                    (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand)
                .Member.Name;
        }

      
    }
   
}