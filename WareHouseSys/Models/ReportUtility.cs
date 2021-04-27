using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;

namespace WareHouseSys.Models
{
    public class ReportUtility
    {
        /// <summary>    
        /// 泛型集合類別轉換成DataTable 
        /// </summary>    
        /// <typeparam name="T">集合項類型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>返回DataTable</returns>    
        public static System.Data.DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            //test
              if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            System.Data.DataTable result = new System.Data.DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


        /// <summary>    
        /// 泛型集合類別轉換成DataTable 
        /// </summary>    
        /// <typeparam name="T">集合項類型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>返回DataTable</returns>    
        public static System.Data.DataTable ToDataTable<T>(T Obj, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);
            System.Data.DataTable result = new System.Data.DataTable();
            PropertyInfo[] propertys = Obj.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (propertyNameList.Count == 0)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                else
                {
                    if (propertyNameList.Contains(pi.Name))
                        result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            ArrayList tempList = new ArrayList();
            foreach (PropertyInfo pi in propertys)
            {
                if (propertyNameList.Count == 0)
                {
                    object obj = pi.GetValue(Obj, null);
                    tempList.Add(obj);
                }
                else
                {
                    if (propertyNameList.Contains(pi.Name))
                    {
                        object obj = pi.GetValue(Obj, null);
                        tempList.Add(obj);
                    }
                }
            }
            object[] array = tempList.ToArray();
            result.LoadDataRow(array, true);
            return result;
        }
    }
}