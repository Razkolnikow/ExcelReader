using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using Services.Contracts;

namespace Services
{
    public class ExcelDataReaderService : IExcelReaderService
    {
        public IEnumerable<T> ReadFromExcel<T>(string pathToExcelFile)
        {
            var objects = new List<T>();
            var props = new List<string>();
            using (var stream = File.Open(pathToExcelFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    T o = Activator.CreateInstance<T>();
                    Type type = o.GetType();
                    var propertiesOfType = type.GetProperties();

                    var result = reader.AsDataSet();
                    foreach (DataTable table in result.Tables)
                    {
                        // Console.WriteLine(table.TableName);
                        int counter = 0;

                        foreach (DataRow tableRow in table.Rows)
                        {
                            T obj = Activator.CreateInstance<T>();
                            int index = 0;
                            foreach (var content in tableRow.ItemArray)
                            {
                                if (counter == 0)
                                {
                                    props.Add(ProcessPropertyName(content.ToString()));
                                }
                                else
                                {
                                    bool existsProperty = CheckIfPropertyExists(
                                        propertiesOfType, 
                                        props, index);

                                    if (existsProperty)
                                    {
                                        this.SetCorrespondingPropValue<T>(
                                            propertiesOfType, 
                                            props, 
                                            index, 
                                            content.ToString(),
                                            obj);
                                    }
                                }

                                index++;
                                // Console.Write(content + " ");
                            }

                            // Console.WriteLine();
                            if (counter >= 1)
                            {
                                objects.Add(obj);
                            }
                            
                            counter++;
                        }
                    }
                }
            }

            return objects;
        }

        private void SetCorrespondingPropValue<T>(PropertyInfo[] propertiesOfType, List<string> props, int index, string val, T o)
        {
            Type type = o.GetType();
            var propertyInfo = type.GetProperty(props[index]);

            if (propertyInfo != null && propertyInfo.PropertyType.ToString().Contains("String"))
            {
                propertyInfo.SetValue(o, val);
            }
            else if (propertyInfo != null && propertyInfo.PropertyType.ToString().Contains("Int"))
            {
                propertyInfo.SetValue(o, int.Parse(val));
            }
        }

        private bool CheckIfPropertyExists(PropertyInfo[] propertiesOfType, List<string> props, int index)
        {
            if (index < props.Count)
            {
                foreach (var propertyInfo in propertiesOfType)
                {
                    if (propertyInfo.Name == props[index])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This method removes all characters that are not letters
        /// </summary>
        /// <param name="propName"></param>
        /// <returns>string</returns>
        private string ProcessPropertyName(string propName)
        {
            var output = new StringBuilder();
            for (int i = 0; i < propName.Length; i++)
            {
                if (char.IsLetter(propName[i]))
                {
                    output.Append(propName[i]);
                }
            }

            return output.ToString();
        }
    }
}
