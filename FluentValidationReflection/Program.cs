using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
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
                // Create rule for each property, based on some data coming from other service...
                //RuleFor(o => o.Description).NotEmpty().When(o => // this works fine
                RuleFor(o => o.GetType().GetProperty(prop.Name)).NotEmpty().When(o =>
                {
                    return true; // do other stuff...
                });
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
}
