using SqlSugar;
using System.Collections.Generic;
using WareHouseSys.DBModels;
using WareHouseSys.Models;

namespace WareHouseSys.Factory
{
    public class MeasurementUnitFactory
    {
        public static List<MeasurementUnit> getMeasurementUnit()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<MeasurementUnit> measurementUnits = db.Queryable<MeasurementUnit>().ToList();


            return measurementUnits;
        }
    }
}