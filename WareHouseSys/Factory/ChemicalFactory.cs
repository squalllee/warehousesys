using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouseSys.DBModels;
using WareHouseSys.Models;
using WareHouseSys.ViewModels;

namespace WareHouseSys.Factory
{
    public class ChemicalFactory
    {
        static public ISugarQueryable<ChemicalDataViewModel> getChemicalDataViewModel(FilterCriteria filter)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            ISugarQueryable<ChemicalDataViewModel> sugarQueryable = db.SqlQueryable<ChemicalDataViewModel>("SELECT ChemicalData.MaterialNo,MaterialInfo.MaterialName,Spec,ChemicalData.Weight, SDSMaterialName, PhysicalStatus, MaterialMix, HarmDesc, HarmLevel,harmUpperLimit, CasNo, HarmGroup1, " +
                "HarmGroup2, Maker, MakerAddress, MakerTel,StoreIndex,BottleName, IsPriority FROM ChemicalData " +
                "inner join MaterialInfo on ChemicalData.MaterialNo = MaterialInfo.MaterialNo " +
                "left join chemicalHarm on ChemicalData.HarmLevel = chemicalHarm.harmNo");


            sugarQueryable = DBUtility.Query(sugarQueryable, filter);

            return sugarQueryable;
        }

        static public List<chemicalHarm> getChemicalHarm()
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

            List<chemicalHarm> chemicalHarms = db.Queryable<chemicalHarm>().ToList();

            return chemicalHarms;
        }

        public static bool ChemicalCreate(ChemicalData chemicalData)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");


            chemicalData.IsPriority = false;


            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Insertable(chemicalData).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool ChemicalUpdate(ChemicalData chemicalData)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");

          

            chemicalData.IsPriority = false;


            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Updateable(chemicalData).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }

        public static bool ChemicalDelete(ChemicalData chemicalData)
        {
            SqlSugarClient db = DBUtility.GetConnectionDb("DefaultConnection");



            chemicalData.IsPriority = false;


            bool retValue = true;
            try
            {
                db.Ado.BeginTran();
                db.Deleteable(chemicalData).ExecuteCommand();
                db.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                retValue = false;
            }

            return retValue;

        }
    }
}