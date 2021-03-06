using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="System.Type"/> meets certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeAssertions : ReferenceTypeAssertions<Type, TypeAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public TypeAssertions(Type type)
        {
            Subject = type;
        }

        /// <summary>
        /// Asserts that the current type is equal to the specified <typeparamref name="TExpected"/> type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be<TExpected>(string because = "", params object[] becauseArgs)
        {
            return Be(typeof(TExpected), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type is equal to the specified <paramref name="expected"/> type.
        /// </summary>
        /// <param name="expected">The expected type</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be(Type expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type to be {0}{reason}, but found <null>.", expected);

            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith(GetFailureMessageIfTypesAreDifferent(Subject, expected));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts than an instance of the subject type is assignable variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which instances of the type should be assignable.</typeparam>
        /// <param name="because">The reason why instances of the type should be assignable to the type.</param>
        /// <param name="becauseArgs">The parameters used when formatting the <paramref name="because"/>.</param>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public new AndConstraint<TypeAssertions> BeAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(typeof(T).IsAssignableFrom(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:" + Context + "} {0} to be assignable to {1}{reason}, but it is not",
                    Subject,
                    typeof(T));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Creates an error message in case the specified <paramref name="actual"/> type differs from the 
        /// <paramref name="expected"/> type.
        /// </summary>
        /// <returns>
        /// An empty <see cref="string"/> if the two specified types are the same, or an error message that describes that
        /// the two specified types are not the same.
        /// </returns>
        private static string GetFailureMessageIfTypesAreDifferent(Type actual, Type expected)
        {
            if (actual == expected)
            {
                return "";
            }

            string expectedType = (expected != null) ? expected.FullName : "<null>";
            string actualType = (actual != null) ? actual.FullName : "<null>";

            if (expectedType == actualType)
            {
                expectedType = "[" + expected.AssemblyQualifiedName + "]";
                actualType = "[" + actual.AssemblyQualifiedName + "]";
            }

            return string.Format("Expected type to be {0}{{reason}}, but found {1}.", expectedType, actualType);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <typeparamref name="TUnexpected"/> type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe<TUnexpected>(string because = "", params object[] becauseArgs)
        {
            return NotBe(typeof(TUnexpected), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <paramref name="unexpected"/> type.
        /// </summary>
        /// <param name="unexpected">The unexpected type</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe(Type unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != unexpected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type not to be [" + unexpected.AssemblyQualifiedName + "]{reason}, but it is.");

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Execute.Assertion
                .ForCondition(Subject.IsDecoratedWith<TAttribute>())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type {0} to be decorated with {1}{reason}, but the attribute was not found.",
                    Subject, typeof (TAttribute));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            BeDecoratedWith<TAttribute>(because, becauseArgs);

            Execute.Assertion
                .ForCondition(Subject.HasMatchingAttribute(isMatchingAttributePredicate))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type {0} to be decorated with {1} that matches {2}{reason}, but no matching attribute was found.",
                    Subject, typeof(TAttribute), isMatchingAttributePredicate.Body);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> implements Interface <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="interfaceType">The interface that should be implemented.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> Implement(Type interfaceType, string because = "", params object[] becauseArgs)
        {
            if (!interfaceType.GetTypeInfo().IsInterface)
            {
                throw new ArgumentException("Must be an interface Type.", "interfaceType");
            }

            Execute.Assertion.ForCondition(Subject.GetInterfaces().Contains(interfaceType))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type {0} to implement interface {1}{reason}, but it does not.", Subject, interfaceType);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> implements Interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface that should be implemented.</typeparam>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> Implement<TInterface>(string because = "", params object[] becauseArgs) where TInterface : class
        {
            return Implement(typeof (TInterface), because, becauseArgs); 
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> does not implement Interface <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="interfaceType">The interface that should be not implemented.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotImplement(Type interfaceType, string because = "", params object[] becauseArgs)
        {
            if (!interfaceType.GetTypeInfo().IsInterface)
            {
                throw new ArgumentException("Must be an interface Type.", "interfaceType");
            }

            Execute.Assertion.ForCondition(!Subject.GetInterfaces().Contains(interfaceType))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type {0} to not implement interface {1}{reason}, but it does.", Subject, interfaceType);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> does not implement Interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface that should not be implemented.</typeparam>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotImplement<TInterface>(string because = "", params object[] becauseArgs) where TInterface : class
        {
            return NotImplement(typeof(TInterface), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> is derived from <see cref="System.Type"/> <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType">The Type that should be derived from.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> BeDerivedFrom(Type baseType, string because = "", params object[] becauseArgs)
        {
            if (baseType.GetTypeInfo().IsInterface)
            {
                throw new ArgumentException("Must not be an interface Type.", "baseType");
            }

            Execute.Assertion.ForCondition(Subject.IsSubclassOf(baseType))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type {0} to be derived from {1}{reason}, but it is not.", Subject, baseType);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="System.Type"/> is derived from <typeparamref name="TBaseClass"/>.
        /// </summary>
        /// <typeparam name="TBaseClass">The Type that should be derived from.</typeparam>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> BeDerivedFrom<TBaseClass>(string because = "", params object[] becauseArgs) where TBaseClass : class
        {
            return BeDerivedFrom(typeof(TBaseClass), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type has a property of type <paramref name="propertyType"/> named <paramref name="name"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveProperty(Type propertyType, string name, string because = "", params object[] becauseArgs)
        {
            PropertyInfo propertyInfo = Subject.GetPropertyByName(name);

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} {1}.{2} to exist{{reason}}, but it does not.",
                    propertyType.Name, Subject.FullName, name));

            Execute.Assertion.ForCondition(propertyInfo.PropertyType == propertyType)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to be of type {1}{{reason}}, but it is not.",
                    propertyInfoDescription, propertyType));

            return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo);
        }
        
        /// <summary>
        /// Asserts that the current type has a property of type <typeparamref name="TProperty"/> named <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveProperty<TProperty>(string name, string because = "", params object[] becauseArgs)
        {
            return HaveProperty(typeof(TProperty), name, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type does not have a property named <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveProperty(string name, string because = "", params object[] becauseArgs)
        {
            PropertyInfo propertyInfo = Subject.GetPropertyByName(name);

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo == null)
                .BecauseOf(because, becauseArgs)
                .FailWith(string.Format("Expected {0} to not exist{{reason}}, but it does.", propertyInfoDescription));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type explicitly implements a property named 
        /// <paramref name="name"/> from interface <paramref name="interfaceType" />.
        /// </summary>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> HaveExplicitProperty(Type interfaceType, string name, string because = "", params object[] becauseArgs)
        {
            Subject.Should().Implement(interfaceType, because, becauseArgs);

            var explicitlyImplementsProperty = Subject.HasExplicitlyImplementedProperty(interfaceType, name);
                
            Execute.Assertion.ForCondition(explicitlyImplementsProperty)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to explicitly implement {1}.{2}{{reason}}, but it does not.",
                    Subject.FullName, interfaceType.FullName, name));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type explicitly implements a property named 
        /// <paramref name="name"/> from interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface whose member is being explicitly implemented.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> HaveExplicitProperty<TInterface>(string name, string because = "", params object[] becauseArgs) where TInterface : class
        {
            return HaveExplicitProperty(typeof (TInterface), name, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type does not explicitly implement a property named 
        /// <paramref name="name"/> from interface <paramref name="interfaceType" />.
        /// </summary>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveExplicitProperty(Type interfaceType, string name, string because = "", params object[] becauseArgs)
        {
            Subject.Should().Implement(interfaceType, because, becauseArgs);

            var explicitlyImplementsProperty = Subject.HasExplicitlyImplementedProperty(interfaceType, name);

            Execute.Assertion.ForCondition(!explicitlyImplementsProperty)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to not explicitly implement {1}.{2}{{reason}}, but it does.",
                    Subject.FullName, interfaceType.FullName, name));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type does not explicitly implement a property named 
        /// <paramref name="name"/> from interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface whose member is not being explicitly implemented.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveExplicitProperty<TInterface>(string name, string because = "", params object[] becauseArgs) where TInterface : class
        {
            return NotHaveExplicitProperty(typeof(TInterface), name, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type explicitly implements a method named <paramref name="name"/> 
        /// from interface <paramref name="interfaceType" />.
        /// </summary>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The expected types of the method parameters.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> HaveExplicitMethod(Type interfaceType, string name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            Subject.Should().Implement(interfaceType, because, becauseArgs);

            var explicitlyImplementsMethod = Subject.HasMethod(string.Format("{0}.{1}", interfaceType.FullName, name), parameterTypes);

            Execute.Assertion.ForCondition(explicitlyImplementsMethod)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to explicitly implement {1}.{2}{{reason}}, but it does not.",
                    Subject.FullName, interfaceType.FullName, name));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type explicitly implements a method named <paramref name="name"/> 
        /// from interface  <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface whose member is being explicitly implemented.</typeparam>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The expected types of the method parameters.</param>
        /// /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> HaveExplicitMethod<TInterface>(string name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs) where TInterface : class
        {
            return HaveExplicitMethod(typeof(TInterface), name, parameterTypes, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type does not explicitly implement a method named <paramref name="name"/> 
        /// from interface <paramref name="interfaceType" />.
        /// </summary>
        /// <param name="interfaceType">The type of the interface.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The expected types of the method parameters.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveExplicitMethod(Type interfaceType, string name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            Subject.Should().Implement(interfaceType, because, becauseArgs);

            var explicitlyImplementsMethod = Subject.HasMethod(string.Format("{0}.{1}", interfaceType.FullName, name), parameterTypes);

            Execute.Assertion.ForCondition(!explicitlyImplementsMethod)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to not explicitly implement {1}.{2}{{reason}}, but it does.",
                    Subject.FullName, interfaceType.FullName, name));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type does not explicitly implement a method named <paramref name="name"/> 
        /// from interface <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface whose member is not being explicitly implemented.</typeparam>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The expected types of the method parameters.</param>
        /// /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///     is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveExplicitMethod<TInterface>(string name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs) where TInterface : class
        {
            return NotHaveExplicitMethod(typeof(TInterface), name, parameterTypes, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current type has an indexer of type <paramref name="indexerType"/>.
        /// with parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="indexerType">The type of the indexer.</param>
        /// <param name="parameterTypes">The parameter types for the indexer.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveIndexer(Type indexerType, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            PropertyInfo propertyInfo = Subject.GetIndexerByParameterTypes(parameterTypes);

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} {1}[{2}] to exist{{reason}}, but it does not.",
                    indexerType.Name, Subject.FullName,
                    GetParameterString(parameterTypes)));

            Execute.Assertion.ForCondition(propertyInfo.PropertyType == indexerType)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected {0} to be of type {1}{{reason}}, but it is not.",
                    propertyInfoDescription, indexerType));

            return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo);
        }

        /// <summary>
        /// Asserts that the current type does not have an indexer that takes parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="parameterTypes">The expected indexer's parameter types.</param>
        public AndConstraint<TypeAssertions> NotHaveIndexer(IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            PropertyInfo propertyInfo = Subject.GetIndexerByParameterTypes(parameterTypes);

            Execute.Assertion.ForCondition(propertyInfo == null)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected indexer {0}[{1}] to not exist{{reason}}, but it does.",
                    Subject.FullName,
                    GetParameterString(parameterTypes)));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type has a method named <paramref name="name"/>with parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The parameter types for the indexer.</param>
        public AndWhichConstraint<TypeAssertions, MethodInfo> HaveMethod(String name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            MethodInfo methodInfo = Subject.GetMethod(name, parameterTypes);

            Execute.Assertion.ForCondition(methodInfo != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected method {0}.{1}({2}) to exist{{reason}}, but it does not.",
                    Subject.FullName, name,
                    GetParameterString(parameterTypes)));

            return new AndWhichConstraint<TypeAssertions, MethodInfo>(this, methodInfo);
        }

        /// <summary>
        /// Asserts that the current type does not expose a method named <paramref name="name"/>
        /// with parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameterTypes">The method parameter types.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotHaveMethod(string name, IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            MethodInfo methodInfo = Subject.GetMethod(name, parameterTypes);

            string methodInfoDescription = "";

            if (methodInfo != null)
            {
                methodInfoDescription = MethodInfoAssertions.GetDescriptionFor(methodInfo);
            }

            Execute.Assertion.ForCondition(methodInfo == null)
                .BecauseOf(because, becauseArgs)
                .FailWith(string.Format("Expected method {0}({1}) to not exist{{reason}}, but it does.", methodInfoDescription,
                    GetParameterString(parameterTypes)));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type has a constructor with parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="parameterTypes">The parameter types for the indexer.</param>
        public AndWhichConstraint<TypeAssertions, ConstructorInfo> HaveConstructor(IEnumerable<Type> parameterTypes, string because = "", params object[] becauseArgs)
        {
            ConstructorInfo constructorInfo = Subject.GetConstructor(parameterTypes);

            Execute.Assertion.ForCondition(constructorInfo != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(String.Format("Expected constructor {0}({1}) to exist{{reason}}, but it does not.",
                    Subject.FullName,
                    GetParameterString(parameterTypes)));

            return new AndWhichConstraint<TypeAssertions, ConstructorInfo>(this, constructorInfo);
        }

        /// <summary>
        /// Asserts that the current type has a default constructor.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndWhichConstraint<TypeAssertions, ConstructorInfo> HaveDefaultConstructor(string because = "", params object[] becauseArgs)
        {
            return HaveConstructor(new Type[] {}, because, becauseArgs);
        }

        private string GetParameterString(IEnumerable<Type> parameterTypes)
        {
            if (!parameterTypes.Any())
            {
                return String.Empty;
            }

            return parameterTypes.Select(p => p.FullName).Aggregate((p, c) => p + ", " + c);
        }

        /// <summary>
        /// Asserts that the selected type has the specified C# <paramref name="accessModifier"/>.
        /// </summary>
        /// <param name="accessModifier">The expected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<Type> HaveAccessModifier(
            CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(accessModifier == Subject.GetCSharpAccessModifier())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected type " + Subject.Name + " to be {0}{reason}, but it is {1}.",
                    accessModifier, Subject.GetCSharpAccessModifier());

            return new AndConstraint<Type>(Subject);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "type"; }
        }
    }
}
