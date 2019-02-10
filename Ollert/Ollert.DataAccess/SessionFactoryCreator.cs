using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using Ollert.DataAccess.Attributes;
using Ollert.DataAccess.Entitites;
using Ollert.DataAccess.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ollert.DataAccess
{
    public static class SessionFactoryCreator
    {
        public static Configuration BuildConfiguration(string connectionString)
        {
            var config = Fluently.Configure();

            // TODO json support maybe?
            var dbConfig = PostgreSQLConfiguration.Standard.ConnectionString(connectionString)
                .FormatSql()
                .AdoNetBatchSize(100);

            config = config.Database(dbConfig);

            var cfg = SetMappings(config);

#if DEBUG
            ExportScripts(cfg, config);
#endif

            return cfg;
        }

        /// <summary>
        /// Mappingek és konvenciók konfigurálása
        /// </summary>
        public static Configuration SetMappings(FluentConfiguration config)
        {
            config = config.Mappings(m => m.AutoMappings.Add(
                AutoMap.AssemblyOf<Entity>(new StoreConfiguration())
                    .IgnoreBase<Entity>()
                    .Conventions.Add<CustomTableNameConvention>()
                    .Conventions.Add<CustomPropertyConvention>()
                    .Conventions.Add<FormulaConvention>()
                    .Conventions.Add<MaxLengthConvention>()
                    .Conventions.Add<UniqueConvention>()
                    .Conventions.Add<NotNullConvention>()
                    .Conventions.Add<DateConvention>()
                    .Conventions.Add<CustomHasManyConvention>()
                    .Conventions.Add<CustomHasManyToManyConvention>()
                    .Conventions.Add<CustomReferenceConvention>()
                    .Conventions.Add<PrimaryKeySequenceConvention>()
                    .Conventions.Add<EnumConvention>()
                    .OverrideAll(DropPropertiesWithoutSetter)
                    .OverrideAll(DropProperties<IgnoreAttribute>)
            ));

            var cfg = config.BuildConfiguration();

            // a kulcsszavakat idézőjelbe teszi
            cfg.SetProperty("hbm2ddl.keywords", "auto-quote");

            return cfg;
        }

        private class StoreConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return Attribute.IsDefined(type, typeof(ComponentTypeAttribute));
            }

            public override bool ShouldMap(Type type)
            {
                return type.IsSubclassOf(typeof(Entity));
            }

            public override string GetComponentColumnPrefix(Member member)
            {
                var result = ConvertName(member.Name) + "_";
                return result;
            }
        }

        /// <summary>
        /// Entitás neveket alakítja át db-ben használatos formátummá. (ExampleEntity => example_entity)
        /// </summary>
        private static string ConvertName(string name)
        {
            name = AccentRemover.RemoveAccents(name);

            // egymás mellett lévő nagybetűket nem írja át CamelCase-re
            name = Regex.Replace(name, "[A-Z]+", x => x.Value[0].ToString().ToUpper() + x.Value.Substring(1).ToLower());

            Match m;
            while ((m = Regex.Match(name, "[A-Z]")).Success)
            {
                var substr = name.Substring(m.Index, m.Length);
                name = name.Remove(m.Index, m.Length);
                name = name.Insert(m.Index, (m.Index > 0 ? "_" : "") + substr.ToLower());
            }

            name = Regex.Replace(name, "_+", "_");

            return name.ToLower();
        }

        /// <summary>
        /// Táblák elnevezéséhez konvenció
        /// </summary>
        private class CustomTableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table(string.Format(CultureInfo.InvariantCulture, "{0}", ConvertName(instance.EntityType.Name)));
            }
        }

        /// <summary>
        /// Propertykhez konvenció
        /// </summary>
        private class CustomPropertyConvention : IPropertyConvention, IPropertyConventionAcceptance
        {
            public static string ConvertToCustomName(string propertyName)
            {
                var result = string.Format(CultureInfo.InvariantCulture, "{0}", ConvertName(propertyName));
                return result;
            }

            public void Apply(IPropertyInstance instance)
            {
                instance.Column(ConvertToCustomName(instance.Property.Name));
            }

            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                // számított értékekhez ne legyen oszlop
                criteria.Expect(x => x.Formula == null);
            }

        }

        /// <summary>
        /// Számított propertykhez konvenció (pl. életkor)
        /// </summary>
        private class FormulaConvention : AttributePropertyConvention<FormulaAttribute>
        {
            protected override void Apply(FormulaAttribute attribute, IPropertyInstance instance)
            {
                var args = attribute.FormulaArgs.Select(CustomPropertyConvention.ConvertToCustomName).ToArray<object>();
                var formula = string.Format(attribute.Formula, args);
                instance.Formula(formula);
            }
        }

        /// <summary>
        /// Meghatározott méretű adatmezőkhöz konvenció
        /// </summary>
        private class MaxLengthConvention : AttributePropertyConvention<MaxLengthAttribute>
        {
            protected override void Apply(MaxLengthAttribute attribute, IPropertyInstance instance)
            {
                var maxLength = attribute.MaxLength;

                if (instance.Property.PropertyType == typeof(string) && maxLength == int.MaxValue)
                {
                    instance.CustomSqlType("text");
                }
                else
                {
                    instance.Length(maxLength);
                }
            }
        }

        /// <summary>
        /// Egyedi property konvenció
        /// </summary>
        private class UniqueConvention : AttributePropertyConvention<UniqueAttribute>
        {
            protected override void Apply(UniqueAttribute attribute, IPropertyInstance instance)
            {
                instance.Unique();
            }
        }

        /// <summary>
        /// Not null konvenció
        /// </summary>
        private class NotNullConvention : AttributePropertyConvention<NotNullAttribute>
        {
            protected override void Apply(NotNullAttribute attribute, IPropertyInstance instance)
            {
                instance.Not.Nullable();
            }
        }

        /// <summary>
        /// Csak évszámot tartalmazó DateTime típusokhoz konvenció
        /// </summary>
        private class DateConvention : AttributePropertyConvention<DateAttribute>
        {
            protected override void Apply(DateAttribute attribute, IPropertyInstance instance)
            {
                instance.CustomSqlType("date");
            }
        }

        /// <summary>
        /// 1:N kapcsolatokhoz konvenció
        /// </summary>
        private class CustomHasManyConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Key.Column(string.Format(CultureInfo.InvariantCulture, "{0}_id", ConvertName(instance.EntityType.Name)));
                instance.Key.ForeignKey(string.Format(CultureInfo.InvariantCulture, "fk_{0}_{1}",
                    ConvertName(instance.Member.Name), ConvertName(instance.EntityType.Name)));
            }
        }

        /// <summary>
        /// N:1 kapcsolatokhoz referencia konvenció
        /// </summary>
        private class CustomReferenceConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                var entityName = ConvertName(instance.EntityType.Name);
                var columnName = ConvertName(instance.Name);
                var keyName = string.Format("fk_{0}_{1}", columnName, entityName);

                instance.ForeignKey(keyName);
                instance.Column(string.Format(CultureInfo.InvariantCulture, "{0}_id", columnName));
                instance.Index(string.Format(CultureInfo.InvariantCulture, "ix_{0}_{1}_id", entityName, columnName));
            }
        }

        /// <summary>
        /// N:M kapcsolatokhoz konvenció
        /// </summary>
        private class CustomHasManyToManyConvention : IHasManyToManyConvention
        {
            public void Apply(IManyToManyCollectionInstance instance)
            {
                var memberName = ConvertName(instance.Member.Name);
                var entityName = ConvertName(instance.EntityType.Name);
                var childTypeName = ConvertName(instance.ChildType.Name);

                var tableName = string.CompareOrdinal(entityName, childTypeName) < 0
                    ? string.Format(CultureInfo.InvariantCulture, "{0}_{1}", entityName, childTypeName)
                    : string.Format(CultureInfo.InvariantCulture, "{0}_{1}", childTypeName, entityName);

                var keyName = string.Format(CultureInfo.InvariantCulture, "fk_{0}_{1}", memberName, entityName);
                var otherKeyName = string.Format(CultureInfo.InvariantCulture, "fk_{0}_{1}", entityName, memberName);
                var columnName = string.Format(CultureInfo.InvariantCulture, "{0}_id", entityName);
                var otherColumnName = string.Format(CultureInfo.InvariantCulture, "{0}_id", childTypeName);

                instance.Table(tableName);
                instance.Key.Column(columnName);
                instance.Key.ForeignKey(keyName);
                instance.Relationship.Column(otherColumnName);
                instance.Relationship.ForeignKey(otherKeyName);
            }
        }

        /// <summary>
        /// Elsődleges kulcsok generálásához konvenció
        /// </summary>
        private class PrimaryKeySequenceConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                var sequenceName = string.Format(CultureInfo.InvariantCulture, "{0}_id_seq", ConvertName(instance.EntityType.Name));
                var columnName = string.Format(CultureInfo.InvariantCulture, "{0}", ConvertName(instance.Property.Name));

                instance.GeneratedBy.Native(sequenceName);
                instance.Column(columnName);
            }
        }

        /// <summary>
        /// Enumokhoz konvenció
        /// </summary>
        private class EnumConvention : IUserTypeConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.CustomType(instance.Property.PropertyType);
            }

            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria.Expect(x => x.Property.PropertyType.IsEnum || x.Property.PropertyType.IsNullableEnum());
            }
        }


        /// <summary>
        /// Setter nélküli propertykat dobja el
        /// </summary>
        private static void DropPropertiesWithoutSetter(IPropertyIgnorer ignorer)
        {
            ignorer.IgnoreProperties(x => !x.CanWrite);
        }

        /// <summary>
        /// T attribútummal rendelkező propertykat dobja el
        /// </summary>
        private static void DropProperties<T>(IPropertyIgnorer ignorer) where T : Attribute
        {
            ignorer.IgnoreProperties(x => Attribute.IsDefined(x.MemberInfo, typeof(T)));
        }

        private static void ExportScripts(Configuration config, FluentConfiguration database)
        {
            try
            {
                var outputFolder = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.Parent?
                    .FullName, "DB");

                if (Directory.Exists(outputFolder))
                {
                    var script = new List<string>(config.GenerateSchemaUpdateScript(new PostgreSQL83Dialect(),
                        new DatabaseMetadata(database.BuildSessionFactory().OpenSession().Connection, new PostgreSQL83Dialect())));

                    File.WriteAllText(Path.Combine(outputFolder, "_UpdateSchema.sql"), string.Join(";" + System.Environment.NewLine,
                        script.Concat(new[] { "" })));

                    script = new List<string>(config.GenerateSchemaCreationScript(new PostgreSQL83Dialect()));
                    File.WriteAllText(Path.Combine(outputFolder, "_CreateSchema.sql"), string.Join(";" + System.Environment.NewLine,
                        script.Concat(new[] { "create table dbversion (id serial, version int);" })));

                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "SessionFactoryCreator ExportScripts failed.");
            }
        }
    }
}
