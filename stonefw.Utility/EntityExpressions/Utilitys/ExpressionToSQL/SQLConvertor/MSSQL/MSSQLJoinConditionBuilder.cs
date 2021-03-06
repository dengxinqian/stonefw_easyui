﻿using System;
using System.Linq.Expressions;
using stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys;
using stonefw.Utility.EntityExpressions.Entitys;

namespace stonefw.Utility.EntityExpressions.Utilitys.ExpressionToSQL.SQLConvertor.MSSQL
{
    internal static class  MSSQLJoinConditionBuilder
    {
        /// <summary>
        /// 获取连接查询条件
        /// </summary>
        /// <param name="theJoinEntity">连接定义实体</param>
        /// <returns>连接查询条件</returns>
        public static string GetJoinCondition<WA, WB>(GenericJoinEntity<WA, WB> theJoinEntity)
        {
            string condition = GetSubConditions(theJoinEntity.MainEntity, theJoinEntity.EntityToJoin, theJoinEntity.JoinConditionExpression, theJoinEntity.JoinConditionFirstParameter);
            return condition;
        }

        private static string GetSubConditions<WA, WB>(GenericWhereEntity<WA> mainEntity, GenericWhereEntity<WB> joinEntity, Expression joinExpression, string firstParameter)
        {
            if (joinExpression is MemberExpression)
            {
                MemberExpression me = (MemberExpression)joinExpression;
                if (me.Expression.ToString() == firstParameter)
                {
                    string dbColumnName = me.GetDBColumnName(mainEntity.EntityType);
                    return string.Format("{0}.[{1}]", mainEntity.TableNameDeclare, dbColumnName);
                }
                else
                {
                    string dbColumnName = me.GetDBColumnName(joinEntity.EntityType);
                    return string.Format("{0}.[{1}]", joinEntity.TableNameDeclare, dbColumnName);
                }
            }

            string opr;
            BinaryExpression be = (BinaryExpression)joinExpression;
            switch (be.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                default:
                    throw new NotSupportedException("不支持连接条件类型：" + joinExpression.NodeType);
            }
            string left = GetSubConditions(mainEntity, joinEntity, be.Left, firstParameter);
            string right = GetSubConditions(mainEntity, joinEntity, be.Right, firstParameter);

            string con = string.Format("({0} {1} {2})", left, opr, right);

            return con;
        }
    }
}