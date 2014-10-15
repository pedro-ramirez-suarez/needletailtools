using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml;
using DataAccess.Scaffold.Attributes;
using Needletail.DataAccess.Attributes;
namespace Generator
{
    class Program
    {

        static string entityName;
        static string action;
        static string projectPath;
        static Type entityType;
        static Dictionary<string, List<object>> vmAttributes = new Dictionary<string, List<object>>();

        /// <summary>
        /// This must run from the root folder of the project
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            if (args.Length < 3)
            {

                Console.Write("Please enter the action(Controller/Repository/Views): ");
                action = Console.ReadLine();

                Console.Write("Please enter the name of the entity: ");
                entityName = Console.ReadLine();

                Console.Write("Please enter the name of the folder containing the website: ");
                projectPath = Console.ReadLine();
            }
            else
            {
                action = args[0];
                entityName = args[1];
                projectPath = args[2];
            }

            //Load all assemblies that do not start with System
            DirectoryInfo di = new DirectoryInfo(projectPath + @"\bin");
            var dlls = di.GetFiles("*.dll");
            entityType = null;
            foreach (var file in dlls)
            {
                //load the assembly and check for the entity
                var library = Assembly.LoadFile(file.FullName);
                
                Type[] types = null;
                try
                {
                    types = library.GetTypes();
                }
                catch (Exception ex)
                {
                    if (ex is System.Reflection.ReflectionTypeLoadException)
                    {
                        var typeLoadException = ex as ReflectionTypeLoadException;
                        var loaderExceptions = typeLoadException.LoaderExceptions;
                    }
                }
                if (types == null)
                    continue;
                foreach (var tp in types)
                {
                    if (tp.Name.ToLower() == entityName.ToLower())
                    {
                        entityType = tp;
                        break;
                    }
                }
                if (entityType != null)
                    break;
            }

            if (entityType == null)
            {
                Console.WriteLine("Entity not found");
                return;
            }

            try
            {

                switch (action)
                {
                    case "Views":
                        GenerateView();
                        break;
                    case "Repository":
                        break;
                    case "Controller":
                        break;
                }



                Console.WriteLine("Finished!");




            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
#if DEBUG
            Console.ReadKey();
#endif
        }

        static void GenerateView()
        {
            //Check if is a view model or an entity
            var vm = entityType.GetCustomAttribute(typeof(NeedletailViewModel), true);
            if (vm == null) //is an entity
            {
                using (XmlWriter writer = XmlWriter.Create(entityName + ".xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("model");
                    WriteEntity(entityType, writer);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                }
            }
            else  // is a view model
            {
                var props = entityType.GetProperties();
                //It's a view model, get all  the Relation and UI attributes first for all the properties
                foreach (var p in props)
                {
                    var relAtts = p.GetCustomAttributes(typeof(NeedletailRelationAttribute), true);
                    var uiAtts = p.GetCustomAttributes(typeof(NeedletailUIAttribute), true);
                    if (relAtts.Length > 0 || uiAtts.Length > 0)
                    {
                        vmAttributes.Add(p.Name, new List<object>());
                        vmAttributes[p.Name].AddRange(relAtts);
                        vmAttributes[p.Name].AddRange(uiAtts);
                    }
                }

                using (XmlWriter writer = XmlWriter.Create(entityName + ".xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("model");

                    //First process all objects that are independent from others
                    var ind = props.Where(p => vmAttributes.ContainsKey(p.Name));
                    foreach (var i in ind)
                        WriteEntity(i.PropertyType, writer, i.Name);

                    //write all the SelectFrom
                    var attribs = new List<object>();
                    foreach (var e in vmAttributes)
                        attribs.AddRange(e.Value.Where(a => (a as SelectFrom) != null));
                    if (attribs.Count > 0)
                    {
                        writer.WriteStartElement("SelectFrom");
                        foreach (var s in attribs)
                        {
                            dynamic sf = s;
                            var prop = props.FirstOrDefault(p => p.Name == sf.LocalList);
                            WriteEntity(prop.PropertyType, writer, "listName", sf.LocalList, sf.ReferencedTable);
                        }
                        writer.WriteEndElement();
                    }


                    //Write all hasone
                    attribs = new List<object>();
                    foreach (var e in vmAttributes)
                        attribs.AddRange(e.Value.Where(a => (a as HasOne) != null));
                    if (attribs.Count > 0)
                    {
                        writer.WriteStartElement("HasOne");
                        foreach (var s in attribs)
                        {
                            dynamic sf = s;
                            var prop = props.FirstOrDefault(p => p.Name == sf.LocalObject);
                            WriteEntity(prop.PropertyType, writer, "objectName", sf.LocalObject, sf.ReferencedTable);
                        }
                        writer.WriteEndElement();
                    }

                    //write all hasmany
                    attribs = new List<object>();
                    foreach (var e in vmAttributes)
                        attribs.AddRange(e.Value.Where(a => (a as HasMany) != null || (a as HasManyNtoN) != null));
                    if (attribs.Count > 0)
                    {
                        writer.WriteStartElement("HasMany");
                        foreach (var s in attribs)
                        {
                            dynamic sf = s;
                            var prop = props.FirstOrDefault(p => p.Name == sf.LocalList);
                            WriteEntity(prop.PropertyType, writer, "listName", sf.LocalList, sf.ReferencedTable);
                        }
                        writer.WriteEndElement();
                    }


                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                }


            }

        }

        static void WriteEntity(Type entityType, XmlWriter writer)
        {
            WriteEntity(entityType, writer, null, null, null);
        }

        static void WriteEntity(Type entityType, XmlWriter writer, string name)
        {
            WriteEntity(entityType, writer, null, null, name);
        }




        static void WriteEntity(Type entityType, XmlWriter writer, string customAttrName, string customAttrVal, string name, bool skipElements = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = entityName;
            }
            //Determine if it's a ViewModel
            var props = entityType.GetProperties();
            if (!skipElements)
            {
                writer.WriteStartElement("entity");
                writer.WriteAttributeString("name", name);
                //determine the type of the primary key
                var key = entityType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(TableKeyAttribute), false) != null);
                if(key.PropertyType.Namespace != "System" && key.PropertyType.IsClass)
                    key = key.PropertyType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(TableKeyAttribute), false) != null);

                writer.WriteAttributeString("primaryKeyType", key.PropertyType.Name);

                if (!string.IsNullOrWhiteSpace(customAttrName) && !string.IsNullOrWhiteSpace(customAttrVal))
                {
                    writer.WriteAttributeString(customAttrName, customAttrVal);
                }
                writer.WriteStartElement("fields");
            }
            foreach (var p in props)
            {
                var atts = p.GetCustomAttributes();
                if (p.PropertyType.IsClass && p.PropertyType.Name != "String")
                {
                    //Generate xml, watch for relation and ui attributes
                    WriteEntity(p.PropertyType, writer, null, null, null, true);
                }
                else
                {
                    //Write the element
                    writer.WriteStartElement(p.Name);
                    //write any atrtibutes and elements 
                    var validators = atts.Where(a => (a as NeedletailAttribute) != null).ToList();
                    string attsVal = string.Empty;
                    switch (p.PropertyType.Name)
                    {
                        case "DateTime":
                            attsVal = "date";
                            break;
                        case "Int16":
                        case "Int32":
                        case "Int64":
                            attsVal = "numeric";
                            break;
                        case "Decimal":
                            attsVal = "numeric";
                            break;

                    }
                    if (validators != null && validators.Count > 0)
                    {
                        attsVal += attsVal == string.Empty ? "" : " ";
                        foreach (var v in validators)
                        {
                            var val = v as NeedletailAttribute;
                            attsVal += val.ValidatorName;
                            if (validators.Last() != v)
                                attsVal += " "; 
                        }
                    }
                    
                    if (attsVal.Length > 0)
                        writer.WriteAttributeString("validator", attsVal);

                    if (vmAttributes.ContainsKey(name))
                    {
                        var attr = vmAttributes[name].FirstOrDefault(a => (a as ForeignKeyAttribute) != null && (a as ForeignKeyAttribute).ForeignKey == p.Name);
                        if (attr != null)
                        {
                            //check if is a select
                            switch (attr.GetType().Name)
                            {
                                case "SelectFrom":
                                    var sf = (attr as SelectFrom);
                                    writer.WriteAttributeString("SelectFrom", sf.LocalList);
                                    writer.WriteStartElement("SelectFrom");
                                    writer.WriteAttributeString("DisplayField", sf.DisplayField);
                                    writer.WriteAttributeString("ReferencedField", sf.ReferencedField);
                                    writer.WriteEndElement();
                                    break;
                                case "HasOne":
                                    var ho = (attr as HasOne);
                                    writer.WriteAttributeString("HasOne", ho.LocalObject);
                                    writer.WriteStartElement("HasOne");
                                    writer.WriteAttributeString("ReferencedField", ho.ReferencedField);
                                    writer.WriteEndElement();
                                    break;
                                case "HasMany":
                                case "HasManyNtoN":
                                    //writer.WriteAttributeString("HasMany", (attr as dynamic).LocalList);
                                    break;
                            }
                        }
                    }
                    //Write any validator detail here
                    if (validators.Count > 0)
                    {
                        writer.WriteStartElement("Validator");
                        foreach (var v in validators)
                        {
                            var val = v as NeedletailAttribute;
                            foreach (var vd in val.ValidatorDetails)
                                writer.WriteAttributeString(vd.Key, vd.Value);
                        }
                        writer.WriteEndElement();
                    }
                    //close element
                    writer.WriteEndElement();
                }
            }
            if (!skipElements)
            {
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }


}
