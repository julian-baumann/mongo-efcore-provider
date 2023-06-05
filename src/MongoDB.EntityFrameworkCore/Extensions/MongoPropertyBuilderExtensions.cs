﻿/* Copyright 2023-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/


using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Metadata;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore;

/// <summary>
/// MongoDB-specific extension methods for <see cref="PropertyBuilder" />.
/// </summary>
public static class MongoPropertyBuilderExtensions
{
    /// <summary>
    /// Configures the field that the property is mapped to when targeting MongoDB.
    /// </summary>
    /// <param name="propertyBuilder">The builder for the property being configured.</param>
    /// <param name="name">The field name for the property.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static PropertyBuilder ToField(
        this PropertyBuilder propertyBuilder,
        string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        propertyBuilder.Metadata.SetFieldName(name);
        return propertyBuilder;
    }

    /// <summary>
    /// Configures the field that the property is mapped to when targeting MongoDB.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property being configured.</typeparam>
    /// <param name="propertyBuilder">The builder for the property being configured.</param>
    /// <param name="name">The field name for the property.</param>
    /// <returns>The same builder instance so that multiple calls can be chained.</returns>
    public static PropertyBuilder<TProperty> ToField<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder,
        string name)
        => (PropertyBuilder<TProperty>)ToField((PropertyBuilder)propertyBuilder, name);


    /// <summary>
    /// Configures the field that the property is mapped to when targeting MongoDB. If an empty string is
    /// supplied then the property will not be persisted.
    /// </summary>
    /// <param name="propertyBuilder">The builder for the property being configured.</param>
    /// <param name="name">The field name for the property.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns>
    /// The same builder instance if the configuration was applied, <see langword="null" /> otherwise.
    /// </returns>
    public static IConventionPropertyBuilder? ToField(
        this IConventionPropertyBuilder propertyBuilder,
        string? name,
        bool fromDataAnnotation = false)
    {
        if (!CanSetField(propertyBuilder, name, fromDataAnnotation))
        {
            return null;
        }

        propertyBuilder.Metadata.SetFieldName(name, fromDataAnnotation);
        return propertyBuilder;
    }

    /// <summary>
    /// Returns a value indicating whether the given field name can be set.
    /// </summary>
    /// <param name="propertyBuilder">The builder for the property being configured.</param>
    /// <param name="name">The field name for the property.</param>
    /// <param name="fromDataAnnotation">Indicates whether the configuration was specified using a data annotation.</param>
    /// <returns><see langword="true" /> if the field name can be set.</returns>
    public static bool CanSetField(
        this IConventionPropertyBuilder propertyBuilder,
        string? name,
        bool fromDataAnnotation = false)
        => propertyBuilder.CanSetAnnotation(MongoAnnotationNames.FieldName, name, fromDataAnnotation);
}
