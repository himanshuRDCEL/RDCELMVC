using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPFileUpload.Helper
{
    public class ListToDatatable
    {
        public DataTable ConvertModelListToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable();

            if (models.Count > 0)
            {
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                }

                foreach (var model in models)
                {
                    var values = new object[properties.Length];

                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(model);
                    }

                    dataTable.Rows.Add(values);
                }

                
            }

            return dataTable;
        }
    }
}
