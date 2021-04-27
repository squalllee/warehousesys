using Kendo.Mvc;
using Kendo.Mvc.UI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Configuration;
using WareHouseSys.DBModels;

namespace WareHouseSys.Models
{
    public class DBUtility
    {
        static string[] DateTimeList = {
                            "yyyy/M/d tt hh:mm:ss",
                            "yyyy/MM/dd tt hh:mm:ss",
                            "yyyy/MM/dd HH:mm",
                            "yyyy/M/d HH:mm",
                            "yyyy/M/d",
                            "yyyy/MM/dd"
                        };

        private static void CreateFilter(CompositeFilterDescriptor f, FilterCriteria filterCriteria)
        {
            foreach (IFilterDescriptor filterDescriptor in ((CompositeFilterDescriptor)f).FilterDescriptors)
            {
                if (filterDescriptor is CompositeFilterDescriptor)
                {
                    CreateFilter((CompositeFilterDescriptor)filterDescriptor, filterCriteria);
                }
                else
                {
                    var descriptor = filterDescriptor as FilterDescriptor;

                    DateTime dateTime = new DateTime();
                    string val = descriptor.Value.ToString();

                    if (DateTime.TryParseExact(descriptor.Value.ToString(), DateTimeList, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime))
                    {
                        val = dateTime.ToString("yyyy/MM/dd");
                    }
                    filterCriteria.Filters.Add(new GridFilter
                    {
                        Field = descriptor.Member,
                        Operator = descriptor.Operator.ToString(),
                        Value = val
                    });
                }
            }
        }

        public static ISugarQueryable<T> Query<T>(ISugarQueryable<T> queryable, DataSourceRequest request)
        {
            FilterCriteria filter = new FilterCriteria();
            filter.Logic = "and";
            filter.Filters = new List<GridFilter>();
            if (request.Filters.Any())
            {
                foreach (var f in request.Filters)
                {   
                    var descriptor = f as FilterDescriptor;
                    if (descriptor != null)
                    {
                        
                        DateTime dateTime = new DateTime();
                        string val = descriptor.Value.ToString();
                        bool tmpbool = false;

                        if(bool.TryParse(val,out tmpbool))
                        {
                            val = tmpbool.ToString().ToLower();
                        }
                       

                        if (DateTime.TryParseExact(descriptor.Value.ToString(), DateTimeList, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out dateTime))
                        {
                            val = dateTime.ToString("yyyy-MM-dd");
                        }
                        filter.Filters.Add(new GridFilter
                        {
                            Field = descriptor.Member,
                            Operator = descriptor.Operator.ToString(),
                            Value = val
                        });

                    }
                    else if (f is CompositeFilterDescriptor)
                    {
                        CreateFilter((CompositeFilterDescriptor)f, filter);
                    }
                }
            }

            try
            {
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;

                    for (var i = 0; i < filters.Count; i++)
                    {
                        if (i == 0)
                            whereClause += string.Format(" {0}",
                                DBUtility.BuildWhereClause<T>(i, filter.Logic, filters[i],
                                parameters));
                        else
                            whereClause += string.Format(" {0} {1}",
                                DBUtility.ToLinqOperator(filter.Logic),
                                DBUtility.BuildWhereClause<T>(i, filter.Logic, filters[i],
                                parameters));
                    }

                    Dictionary<string, object> paramterList = new Dictionary<string, object>();
                    int count = 0;
                    foreach (object o in parameters)
                    {
                        paramterList.Add(count.ToString(), o);
                        count++;
                    }
                    queryable = queryable.Where(whereClause, paramterList);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return queryable;
        }

        public static ISugarQueryable<T> Query<T>(ISugarQueryable<T> queryable, FilterCriteria filter)
        {
            try
            {
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;

                    for (var i = 0; i < filters.Count; i++)
                    {
                        if (i == 0)
                            whereClause += string.Format(" {0}",
                                DBUtility.BuildWhereClause<T>(i, filter.Logic, filters[i],
                                parameters));
                        else
                            whereClause += string.Format(" {0} {1}",
                                DBUtility.ToLinqOperator(filter.Logic),
                                DBUtility.BuildWhereClause<T>(i, filter.Logic, filters[i],
                                parameters));
                    }

                    Dictionary<string, object> paramterList = new Dictionary<string, object>();
                    int count = 0;
                    foreach (object o in parameters)
                    {
                        paramterList.Add(count.ToString(), o);
                        count++;
                    }
                    queryable = queryable.Where(whereClause, paramterList);

                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return queryable;
        }

        public static string BuildWhereClause<T>(int index, string logic,
        GridFilter filter, List<object> parameters)
        {
            var entityType = (typeof(T));
            var property = entityType.GetProperty(filter.Field);

            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                case "islessthanorequalto":
                case "isgreaterthanorequalto":
                case "isgreaterthan":
                case "isequalto":
                case "islessthan":
                    //if (typeof(DateTime).IsAssignableFrom(property.PropertyType))
                    //{
                    //    parameters.Add(DateTime.Parse(filter.Value).Date);
                    //    return string.Format("EntityFunctions.TruncateTime({0}){1}@{2}",
                    //        filter.Field,
                    //        ToLinqOperator(filter.Operator),
                    //        index);
                    //}
                    if (typeof(int).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(int.Parse(filter.Value));
                        return string.Format("{0}{1}@{2}",
                            filter.Field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    parameters.Add(filter.Value);
                    return string.Format("{0}{1}@{2}",
                        filter.Field,
                        ToLinqOperator(filter.Operator),
                        index);
                case "startswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.StartsWith(" + "@{1})",
                        filter.Field,
                        index);
                case "endswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0}.EndsWith(" + "@{1})",
                        filter.Field,
                        index);

                case "contains":
                    parameters.Add(filter.Value);
                    return string.Format("{0} like '%' + @{1}+'%'",
                        filter.Field,
                        index);

                default:
                    throw new ArgumentException(
                        "This operator is not yet supported for this Grid",
                        filter.Operator);
            }
        }

        public static string ToLinqOperator(string @operator)
        {
            switch (@operator.ToLower())
            {
                case "eq":
                case "isequalto":

                    return " = ";
                case "neq": return " <> ";
                case "gte":
                case "isgreaterthanorequalto":
                    return " >= ";
                case "gt":
                case "isgreaterthan":
                    return " > ";
                case "lte":
                case "islessthanorequalto":
                    return " <= ";
                case "lt":
                case "islessthan":
                    return " < ";
                case "or": return " || ";
                case "and": return " and ";
                default: return null;
            }
        }

        static public SqlSugarClient GetConnectionDb(string ConfigName)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings[ConfigName];

            return SugarFactory.GetInstance(settings.ConnectionString);
        }


    }
}