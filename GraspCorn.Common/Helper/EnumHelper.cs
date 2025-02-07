using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GraspCorn.Common.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// Method to get the lit of Enums
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Enum value</param>
        /// <returns></returns>
        public static List<T> GetEnumList<T>(this T source)
        {
            List<T> enumList = Enum.GetValues(typeof(T))
                            .Cast<T>()
                            .ToList();

            return enumList;
        }

        /// <summary>
        /// Method to get the list of enum for dropdown
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>List of SelectListItem</returns>
        public static List<SelectListItem> GetEnumForDropDown<T>(this T source)
        {
            List<SelectListItem> enumDropList = new List<SelectListItem>();
            List<T> enumList = Enum.GetValues(typeof(T))
                            .Cast<T>()
                            .ToList();

            foreach (T item in enumList)
            {
                enumDropList.Add(new SelectListItem { Text = DescriptionAttr(item), Value = Convert.ToInt32(item).ToString() });
            }
            return enumDropList;
        }

        /// <summary>
        /// Method to get the description of the Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>String: Enum description</returns>
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
