using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace CodeFriendly.Filtering.OData
{
    public class ExpressionBuilder
    {
        private readonly MethodInfo _oDataexpressionParameterMethod;
        private readonly Type _queryTranslatorType;
        private readonly Type _expressionLexerType;
        private readonly MethodInfo _expressionLexerScanMethod;
        private readonly MethodInfo _expressionLexerRpnMethod;
        private readonly MethodInfo _expressionLexerCreateMethod;
        private readonly MethodInfo _queryTranslatorTranslateMethod;
        private readonly Type _oDataLambdaType;
        private readonly PropertyInfo _oDataLambdaBodyProperty;
        private readonly PropertyInfo _oDataLambdaPropsProperty;
        private readonly Type _oDataParameterExpressionType;

        private readonly PropertyInfo _oDataPropertyExpressionNodeTypeProperty;
        private readonly PropertyInfo _oDataPropertyExpressionNameProperty;
        private readonly PropertyInfo _oDataPropertyExpressionExpressionProperty;


        public ExpressionBuilder()
        {
            var assembly = typeof(Sprint.Filter.OData.Filter).Assembly;
            var types = assembly.GetTypes().ToList();
            _oDataLambdaType = types.Single(t => t.Name == "ODataLambdaExpression");
            var oDataPropertyExpressionType = types.Single(t => t.Name == "ODataPropertyExpression");
            _oDataParameterExpressionType = types.Single(t => t.Name == "ODataParameterExpression");
            var oDataExpressionType = types.Single(t => t.Name == "ODataExpression");
            _queryTranslatorType = types.Single(t => t.Name == "QueryTranslator" && t.Namespace.Contains("Deserialize"));
            _expressionLexerType = types.Single(t => t.Name == "ExpressionLexer");
            _expressionLexerScanMethod = _expressionLexerType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(m => m.Name == "Scan");
            _oDataexpressionParameterMethod = oDataExpressionType.GetMethods().Single(m => m.Name == "Parameter");

            var expressionLexerMethods = _expressionLexerType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
            _expressionLexerRpnMethod = expressionLexerMethods.Single(m => m.Name == "ConvertToRpn");
            _expressionLexerCreateMethod = expressionLexerMethods.Single(m => m.Name == "CreateExpression");
            _oDataLambdaBodyProperty = _oDataLambdaType.GetProperties().Single(p => p.Name == "Body");
            _oDataLambdaPropsProperty = _oDataLambdaType.GetProperties().Single(p => p.Name == "Parameters");
            _queryTranslatorTranslateMethod = _queryTranslatorType.GetMethods().Single(m => m.Name == "Translate" && m.GetParameters().Length == 2);

            _oDataPropertyExpressionNodeTypeProperty = oDataPropertyExpressionType.GetProperty("NodeType");
            _oDataPropertyExpressionNameProperty = oDataPropertyExpressionType.GetProperty("Name");
            _oDataPropertyExpressionExpressionProperty = oDataPropertyExpressionType.GetProperty("Expression");

        }

        public Expression<Func<T, bool>> Build<T>(string filter)
            where T : class
        {
            var expressionLexer = Activator.CreateInstance(_expressionLexerType, filter);
            var translator = Activator.CreateInstance(_queryTranslatorType, null);
            var param = _oDataexpressionParameterMethod.Invoke(null, new object[] { "t" });
            var tree = _expressionLexerScanMethod.Invoke(expressionLexer, new[] { param }) as dynamic;

            AlignExpressionTree(tree, typeof(T));
            var rpn = _expressionLexerRpnMethod.Invoke(null, new object[] { tree });
            var expr = _expressionLexerCreateMethod.Invoke(null, new[] { rpn });
            var oLamb = Activator.CreateInstance(_oDataLambdaType);


            _oDataLambdaBodyProperty.SetValue(oLamb, expr);
            var arr = Array.CreateInstance(_oDataParameterExpressionType, 1);
            arr.SetValue(param, 0);
            _oDataLambdaPropsProperty.SetValue(oLamb, arr);

            var transResult = _queryTranslatorTranslateMethod.Invoke(translator, new[] { oLamb, typeof(T) });
            return (Expression<Func<T, bool>>)transResult;
        }

        private void AlignExpressionTree(object tree, Type t)
        {
            if (!(tree is IEnumerable<object> array)) return;
            foreach (var node in array)
            {
                AlignExpression(node, t);
            }
        }

        private Type AlignExpression(object node, Type t)
        {
            var type = _oDataPropertyExpressionNodeTypeProperty.GetValue(node);
            switch (type.ToString())
            {
                case "MemberAccess":
                    var expression = _oDataPropertyExpressionExpressionProperty.GetValue(node);
                    if (expression != null && expression.ToString() != "Sprint.Filter.OData.Common.ODataParameterExpression")
                    {
                        t = AlignExpression(expression, t);
                    }

                    var propertyName = _oDataPropertyExpressionNameProperty.GetValue(node).ToString();
                    var property = FindProperty(t, propertyName);
                    if (property.Name != propertyName)
                    {
                        _oDataPropertyExpressionNameProperty.SetValue(node, property.Name);

                    }
                    return property.PropertyType;
                case "Call":
                    var args = node.GetType().GetProperty("Arguments").GetValue(node);
                    var context = node.GetType().GetProperty("Context")?.GetValue(node);
                    AlignExpressionTree(args, t);

                    if (context != null)
                    {
                        AlignExpression(context, t);
                    }

                    break;
            }

            return t;
        }

        private static PropertyInfo FindProperty(Type type, string name)
        {
            var property = type.GetProperty(name);
            const StringComparison stringCompare = StringComparison.InvariantCultureIgnoreCase;
            if (property == null)
            {
                var propByDataMember = type.GetProperties()
                    .Select(prop => new { attr = prop.GetCustomAttribute<DataMemberAttribute>(), prop })
                    .SingleOrDefault(att => string.Equals(att.attr?.Name, name, stringCompare));

                property = propByDataMember?.prop;
            }

            if (property == null)
            {
                property = type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, name, stringCompare));
            }

            return property;

        }

    }
}