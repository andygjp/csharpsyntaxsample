namespace CSharpSyntaxSample
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    class Program
    {
        static void Main(string[] args)
        {
            // This is a comment - everything left of the double forward slash is a comment.

            /* This is also a comment - everything between the slash and asterisk is a comment, 
            therefore, everything outside of slash and asterisk is compilable code. */

            // To declare a variable, you specify the data type followed by a variable name:
            int i;

            // To initialize the variable, you assign the value using the assignment operator (the = symbol):
            i = 1;

            // The declaration and assignment can be done on a single line:
            int j = 2;

            // You don't have to include the type in the declaration. You can use the var keyword instead.
            // C# is a staticly typed language, but the compiler can work out the type for you. And it will enforce
            // the type strictness by raising a compiler error if you try to assign a value of different type.
            var aDouble = 0.0;

            // The line below fails to compile because the variable was initialised with a double literal and therefore is a double not a string.
            // aDouble = "";

            // There are 15 built-in types. Most of these types are just for representing numbers of various size and 
            // precision. Although you are free to use any type, the built-in types that you'd typically use are:
            int a = 3;
            double b = 0.1;
            string c = "some text";
            bool d = true;

            // (.Net has been optimised for int, integral numbers, and double, floating point numbers. You'd probably 
            // only use a smaller integral type if the value could be safely held within the type and memory consumption 
            // was of utmost importance. You can find more about types here: https://msdn.microsoft.com/en-us/library/ya5y69ds.aspx)

            // You can create your own custom types.
            MyType e = new MyType();
            MyStruct f = new MyStruct(1,2);
            MyEnum g = MyEnum.SomeValue;
            IMyInterface h;

            // You'll often work with collection types. The two widely used types are...

            string[] stringArray = new string[] {"Hello", "World"}; // An array declaration and initialisation.

            List<string> stringList = new List<string>(); // A list of strings.
            stringList.Add("Hello");
            stringList.Add("World"); // You can add to (and remove from) the list, whilst arrays are readonly.

            // Note the declaration of the list type above - this is a generic type. Generic types allow us to loosely define the type,
            // but enforce strictness when its used. With the generic list type, there is a single type definition that can be used
            // to declare a list of anything. Imagine if I wanted a list for each of the 15 built-in types - that would be 15 separate
            // type definitions. Once I've declared a list of strings, I can only add strings to the collection and when I enumerate
            // the collection, I know each item is a string - this is the strict part enforced by the compiler. For example...

            MyReadonlyCollection<string> strings;
            MyReadonlyCollection<int> integers;
            // Note - this initialisation uses the params constructor that allows me to specify zero or many arguments.
            MyReadonlyCollection<MyType> mytypes = new MyReadonlyCollection<MyType>(new MyType());
        }
    }

    // A type signature has the following items: 1. an optional visibility modifier (the default modifier is internal, 
    // if none is specified), 2. the class keyword, 3. the class name, 4. the base class type that this class inherits.
    // Everything in .Net - ints, doubles, strings and MyType - is an object and their base class is the type called
    // object. You don't have to include the base class type, as we have, (the colon and the type name) if your custom 
    // type derives from object
    internal class MyType : object
    {
        // A class is a collection of related functionality.

        int _number;

        // This is a constructor. A constructor is a method that is invoked when the type is initialised. A constructor
        // has the same name as the type name, has no return types or void and can have zero or many parameters.
        public MyType(int number)
        {
            _number = number;
        }

        public MyType()
        {
            // This is a default constructor - a constructor without any parameters. If you don't define any constructors
            // for a type, it will automatically have a default constructor. But the definition of the default constructor
            // is optional. you'd only need to define it if you needed to do some custom initialisation.
        }

        // Classes contain properties - a way to encapsulate a variable
        public int Number
        {
            // You can forego the property backing field using auto-properties, see the struct
            // example for the auto-property syntax.
            get { return _number; }
            set { _number = value; }
        }

        // Classes contain methods.
        public bool AreValuesTheSame(int a, string b)
        {
            // A method signature has the following items: 1. an optional visibility modifier (the default modifier is private
            // if none is specified), 2. a return type for functions or void for subroutines, 3. method name, 4. comma
            // separated list of parameters.

            int? parsed = ParseNumber(b); // int? is shorthand for Nullable<int>.
            if (parsed.HasValue)
            {
                return parsed.Value == a;
            }
            return false;
        }

        // Methods can be declared static, which means they cannot access instance members (methods, variables, etc) -
        // they can only access type members.
        private static Nullable<int> ParseNumber(string s)
        {
            // This method introduces nullable value types. Value types always have a value unlike reference types that
            // can be null. Sometimes it is useful to express a particular condition using null instead of using a specific
            // value. How could I represent that the argument does not parse? Would zero represent a non-parsable number?
            // Instead I can represent a non-parsable number by using nullables and returning null.
            int i;
            if (int.TryParse(s, out i))
            {
                return i;
            }
            return null;
        }

        public double GetRandomNumber()
        {
            // This method accesses the Number property. A property is essentially a method, but you don't need parenthesis
            // to access the value, instead you use it like you would a variable.
            return new Random(Number).NextDouble();
        }

        public string GetRandomNumberAsText()
        {
            // This method invokes the GetRandomNumber instance member. You can access instance members using the this
            // keyword. You don't have to use the keyword, but in can help disambiguate if, for example, you happen to
            // have a variable with the same name in the same scope you where you want to access the member.

            string GetRandomNumber = "1";
            // Won't compile without the this keyword because of the presence of a variable with the same name.
            double randomNumber = this.GetRandomNumber();
            return randomNumber.ToString();
        }
    }

    // This is a static class. Only classes can be declared with the static keyword. 
    static class MyExtensions
    {
        // Static classes can only contain static members.
        
        public static int WordCount(this string text)
        {
            // This is a special static method called an extension method. All extension methods must be
            // static and the first parameter must have the this keyword. Extension methods allow you to add
            // new functionality to existing classes - see examples of usage in the ExampleUsageOfMyExtensionMethods class.
            string[] strings = text.Split(new char []{' '}, StringSplitOptions.RemoveEmptyEntries);
            return strings.Length;
        }
        
        public static T Add<T>(this T a, T b) where T : struct, IConvertible, IComparable<T>
        {
            // This method creates a lambda expression that adds tow values together. This is a generic method that
            // uses type constraints (the stuff right of the closing parentheses) to ensure that this method can
            // only be called on and accepts only parameters that are value types and implement the IConvertible and 
            // IComparable<T> interfaces, ie numbers.
            
            var paramA = Expression.Parameter(typeof(T), "a");
            var paramB = Expression.Parameter(typeof(T), "b");
            var body = Expression.Add(paramA, paramB);
            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
            return add(a, b);
        }

        public static T Sum<T>(this T a, params T[] items) where T : struct, IConvertible, IComparable<T>
        {
            // This example simply enumerates the collection, but it does so in an imperative manner. With imperative
            // programming, you have to instruct the compiler. You tell the compiler you want to enumerate a collection by using
            // a foreach statement. The alternative to imperative programming is declarative programming. See this link:
            // http://stackoverflow.com/questions/1784664/what-is-the-difference-between-declarative-and-imperative-programming
            foreach (var value in items)
            {
                a = Add(a, value);
            }
            return a;


            // Compare the above example with this declarative version. With declarative programming you say what you want:
            // "I want to aggregate the result from the Add method." You don't tell the compiler how to do it.
            return items.Aggregate(a, (current, value) => Add(current, value));


            // This expression can be written more succinctly.
            return items.Aggregate(a, Add);

            // The two example above make use of lambda expressions, anonymous functions, and LINQ, generic extension methods
            // for collection types. 
            // See here for information about lambdas: https://msdn.microsoft.com/en-us/library/bb397687.aspx
            // See here for information about LINQ: https://msdn.microsoft.com/en-us/library/bb397926.aspx
        }
    }

    static class ExampleUsageOfMyExtensionMethods
    {
        static void HowManyWords()
        {
            // The string type, as defined in the core library, doesn't actually contain a method called WordCount, but
            // we have effectively extended the type without inheritance by using an extension method.
            var wordCount = "How many words are in this line of text".WordCount();
        }

        static void AddTwoNumbers()
        {
            var x = 1.Add(2);

            // Note - this won't compile and intellisense won't even offer you the method because the string literal
            // does not match the generic type constraints.
            //"".Add(1);

            // Note - this also won't compile because this time the argument type does not match the generic type constraints.
            //1.Add(new MyType());
        }

        static void AddManyNumbers()
        {
            var x = 1;
            x = x.Sum(2, 3, 4, 5, 6, 7, 8, 9);
        }
    }

    // Structs have the same signature as classes, but they use the struct keyword instead.
    struct MyStruct
    {
        // A struct is a value type that is typically used to encapsulate small groups of related variables. You probably
        // wouldn't create many structs in practice.

        public MyStruct(int x, int y) : this()
        {
            // Structs cannot define a default (parameterless) constructor, as it is implicitly defined already, but any 
            // constructors that you define must invoke the default constructor.
            this.X = x;
            this.Y = y;
        }

        // In this example, the X and Y values are encapsulated with properties, however, these properties
        // use the auto-property syntax which means we can forego the backing field and simplify the member declaration.
        public int X { get; private set; }
        public int Y { get; private set; }
    }

    // Enums have the same signature as classes, but they use the enum keyword instead.
    enum MyEnum
    {
        // An enum is a collection of values where each value is represented by member name. 
        // The first member has the value of 0 (zero), unless specified, and the value 
        // of each subsequent member increments by 1 after that, unless specified.

        SomeValue = 1,
        AnotherValue, // The value of this member is 2
        FinalValue = 5 // The value of this member would've been 3, however, its been overridden to equal 5
    }

    // Interfaces have the same signature as classes, but they use the interface keyword instead.
    public interface IMyInterface
    {
        // Interfaces allow you to define the member signatures of type separately from
        // the implementation of the interface.

        int GetNumber();
    }

    // This is a generic type definition. The type signature is the same as for a non-generic class, but
    // it has the generic type argument inside the angle brackets. The name of the argument is called T.
    // Note - we haven't said what type T is, just given it a name so we can refer to the type later.
    // A simple analogy would be to imagine that when you declare a MyReadonlyCollection<string>, a copy
    // of this code is sent to the compiler with every T replaced by string.
    // See here for more details: https://msdn.microsoft.com/en-us/library/512aeb7t.aspx
    class MyReadonlyCollection<T> : IEnumerable<T>
    {
        // This type also implements the generic interface IEnumerable<T>. Interface implementation and inheritance
        // both use the same : operator.

        // This member variable is readonly which means I can only assign this variable in the constructor.
        readonly T[] _internalCollection;

        public MyReadonlyCollection(params T[] items)
            : this(items.AsEnumerable())
        {
            // This constructor uses the params keyword that allows the caller to construct an object by
            // including zero or many arguments without having to first place these arguments in a collection
            // like you'd have to do if you use the constructor below.
        }

        public MyReadonlyCollection(IEnumerable<T> items)
        {
            _internalCollection = items.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            // This is an iterator statement. It simply enumerates the collection and for every item in the collection
            // it yields control back to the caller and returns the item. Once the caller is finished with that item, control
            // comes back to this method so that it can continue enumerating the collection.
            foreach (T item in _internalCollection)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
