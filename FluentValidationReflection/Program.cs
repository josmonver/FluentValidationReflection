using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidationReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Foo foo = new Foo();
            IValidator<Foo> validator = new FooValidator(foo);
            var result = validator.Validate(foo);

            if (result.IsValid)
            {
                Console.WriteLine("All is valid!");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            Console.ReadKey();
        }
    }

    public class FooValidator : AbstractValidator<Foo>
    {
        public FooValidator(Foo obj)
        {
            // Iterate properties using reflection
            var properties = ReflectionHelper.GetShallowPropertiesInfo(obj);
            foreach (var prop in properties)
            {
                RuleFor(o => o)
                    .CustomNotEmpty(obj.GetType().GetProperty(prop.Name))
                    .NotEmpty()
                    .When(o =>
                {
                    return true; // do other stuff...
                });

                // Create rule for each property, based on some data coming from other service...
                ////RuleFor(o => o.Description).NotEmpty().When(o => // this works fine when foo.Description is null
                //RuleFor(o => o.GetType().GetProperty(prop.Name)).NotEmpty().When(o =>
                //{
                //    return true; // do other stuff...
                //});
            }
        }
    }

    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetShallowPropertiesInfo<T>(T o) where T : class
        {
            var type = typeof(T);
            var properties =
                from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where pi.PropertyType.Module.ScopeName == "CommonLanguageRuntimeLibrary"
                    && !(pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                select pi;

            return properties;
        }
    }

    public class Foo
    {
        public string Description { get; set; }
    }

    public class CustomNotEmpty<T> : PropertyValidator
    {
        private PropertyInfo _propertyInfo;

        public CustomNotEmpty(PropertyInfo propertyInfo)
            : base(string.Format("{0} is required", propertyInfo.Name))
        {
            _propertyInfo = propertyInfo;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            //Expression<Func<T, PropertyInfo>> expression = o => o.GetType().GetProperty(_propertyInfo.Name);
            //Func<T, PropertyInfo> oFunc = expression.Compile();
            //PropertyInfo oTargetDateTime = oFunc.Invoke((T)context.Instance);

            return !IsNullOrEmpty(_propertyInfo, (T)context.Instance);
        }

        private bool IsNullOrEmpty<T>(PropertyInfo property, T obj)
        {
            var t = property.PropertyType;
            var v = property.GetValue(obj);

            if (t == typeof(string))
                return string.IsNullOrEmpty(v as string);

            if (t == typeof(Int32?) || t == typeof(Int32))
            {
                Int32? value = Convert.ToInt32(v);
                return value == null || value == 0;
            }
            if (t == typeof(Boolean?) || t == typeof(Boolean))
            {
                Boolean? value = Convert.ToBoolean(v);
                return value == null || value == false;
            }
            if (t == typeof(Decimal?) || t == typeof(Decimal))
            {
                Decimal? value = Convert.ToDecimal(v);
                return value == null || value == 0;
            }
            if (t == typeof(DateTime?) || t == typeof(DateTime))
            {
                DateTime value = Convert.ToDateTime(v);
                return value == null || value == DateTime.MinValue;
            }

            return v == null;
        }
    }
    
}
