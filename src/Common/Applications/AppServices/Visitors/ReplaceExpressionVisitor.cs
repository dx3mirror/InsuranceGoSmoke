using System.Linq.Expressions;

namespace InsuranceGoSmoke.Common.Applications.AppServices.Visitors
{
    /// <summary>
    /// Класс <see cref="ReplaceExpressionVisitor"/> используется для замены экземпляров выражений в дереве выражений.
    /// </summary>
    /// <remarks>
    /// Этот класс наследует от <see cref="ExpressionVisitor"/> и переопределяет метод <see cref="Visit"/>,
    /// чтобы выполнить замену одного выражения на другое в процессе обхода дерева выражений.
    /// </remarks>
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Старое значение выражения, которое нужно заменить.
        /// </summary>
        private readonly Expression _oldValue;

        /// <summary>
        /// Новое значение выражения, которое будет использоваться для замены.
        /// </summary>
        private readonly Expression _newValue;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReplaceExpressionVisitor"/>.
        /// </summary>
        /// <param name="oldValue">Выражение, которое необходимо заменить.</param>
        /// <param name="newValue">Выражение, которое будет использоваться вместо старого.</param>
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// Переопределяет метод <see cref="Visit"/> для выполнения замены старого выражения на новое.
        /// </summary>
        /// <param name="node">Узел дерева выражений, который нужно посетить.</param>
        /// <returns>Возвращает новый узел дерева выражений с замененным значением, если узел совпадает со старым значением; в противном случае возвращает узел без изменений.</returns>
        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
