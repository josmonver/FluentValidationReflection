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
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, T> CustomNotEmpty<T>(
            this IRuleBuilder<T, T> ruleBuilder, PropertyInfo propertyInfo)
        {
            return ruleBuilder.SetValidator(new CustomNotEmpty<T>(propertyInfo));
        }
    }
}
