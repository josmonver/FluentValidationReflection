using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidationReflection
{
    public static class DefaultValidatorExtensions
    {
        public static IRuleBuilderOptions<T, T> CustomNotEmpty<T>(this IRuleBuilder<T, T> ruleBuilder, PropertyInfo propertyInfo) //Expression<PropertySelector<T, DateTime?>> expression)//Expression<Func<T, DateTime?>> expression) 
        {
            return ruleBuilder.SetValidator(new CustomNotEmpty<T>(propertyInfo));
        }

        //public static IRuleBuilderOptions<T, DateTime?> GreaterThanNullableDateList<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, Expression<Func<T, T>> expression, string pCollectionProperty, string pPropertyOfCollection) //Expression<PropertySelector<T, DateTime?>> expression)//Expression<Func<T, DateTime?>> expression) 
        //{//will get all the objects of a collection and for each object check a property.
        //    return ruleBuilder.SetValidator(new GreaterThanNullableDate<T>(expression, pCollectionProperty, pPropertyOfCollection));
        //}
    }
}
