using ExcuteJobList.Models.RECV;
using Models.RECV;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;
using WareHouseSys.DBModels;
using WareHouseSys.Models.PUR;

namespace WareHouseSys.Factory
{
    public class UtilityFactory
    {
        public static bool sendDataToEIP(object obj)
        {
            var purJSON = JsonConvert.SerializeObject(obj);

            var jsonBytes = Encoding.UTF8.GetBytes(purJSON);
            string purUrl = ConfigurationSettings.AppSettings["EIPURL"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(purUrl);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.ContentLength = jsonBytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(jsonBytes, 0, jsonBytes.Length);
                requestStream.Flush();
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    if (result.IndexOf("SUCCESS") >= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool CreateRECV( string PurchaseNo,string Lot,string CaseOfficer)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            PurchaseHeader purHeader = db.Queryable<PurchaseHeader>().Where(e => e.PurchaseNo == PurchaseNo).Single();
            List<RECV01> recvs = new List<RECV01>();
            List<recvMaterial> materials = null;
            MaterialInfo materialInfo = null;
            //List<PurchaseBody> purBodyList = null;
            List<ReceiveBody> receiveBodies = db.Ado.SqlQuery<ReceiveBody>("select ReceiveBody.* from ReceiveHeader inner join " +
                                                                           "ReceiveBody on ReceiveBody .OrderNo = ReceiveHeader.OrderNo " + "" +
                                                                           "where PurchaseNo = @PurchaseNo and  ReceiveHeader.DeliveryLot = @Lot",new { PurchaseNo= PurchaseNo,Lot=Lot });

            //purBodyList = db.Queryable<PurchaseBody>().Where(e => e.PurchaseNo == PurchaseNo && e.DeliveryLot == Lot).ToList();
            var purBodyList = db.Ado.SqlQuery<PurchaseBody>("select MaterialNo, DeliveryLot, DeliveryPlace, PerformancePeriod, SUM(Quantity) AS Quantity " +
                                                            "FROM PurchaseBody WHERE (PurchaseNo = @PurchaseNo) AND (DeliveryLot = @Lot) GROUP BY MaterialNo, DeliveryLot, DeliveryPlace, PerformancePeriod", 
                                                            new { PurchaseNo = PurchaseNo, Lot = Lot });
            List<Employee> employees = db.Queryable<Employee>().ToList();


            RECV01 recv = null;

            recv = new RECV01
            {
                apikey = "Rp6pu5AauFnT",
                fid = "370",
                fowner = CaseOfficer,
                fdraft = "1",
                承辦人 = CaseOfficer,
                契約編號 = PurchaseNo,
                契約金額 = purHeader.ContractPriceIncludeVAT.ToString(),
                廠商名稱 = purHeader.VendorName,
                廠商聯絡人 = purHeader.VendorContact,
                手機 = purHeader.Mobile,
                採購方式 = purHeader.PurchaseMethod,
                案名 = purHeader.PurchaseName,
                電話 = purHeader.Tel,
                預算來源 = purHeader.BudgetSource,
                採購承辦人 = purHeader.PurchaseMan + "[" + employees.Where(e => e.KEYNO == purHeader.PurchaseMan).Single().TMNAME + "]",
                採購承辦人ID = purHeader.PurchaseMan
            };
           
            materials = new List<recvMaterial>();
            foreach (PurchaseBody body in purBodyList)
            {
                materialInfo = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo == body.MaterialNo).Single();

                List<ReceiveBody> receives =  receiveBodies.Where(e => e.MaterialNo == materialInfo.MaterialNo ).ToList();

                int recvedQty = 0;
                if(receives.Count > 0)
                {
                    recvedQty = (int)receives[0].Quantity;
                }

                if (body.Quantity - recvedQty <= 0) continue;

                recv.交貨批次 = body.DeliveryLot;
                recv.履約期限 = DateTime.Parse(body.PerformancePeriod.ToString()).ToString("yyyy/MM/dd");
                recv.交貨地點 = body.DeliveryPlace;
                materials.Add(new recvMaterial
                {
                    品名 = materialInfo.MaterialName,
                    單位 = materialInfo.Unit,
                    料號 = materialInfo.MaterialNo,
                    規格 = materialInfo.Spec,
                    應交數量 = (body.Quantity - recvedQty).ToString()
                });
                body.IsCreateRecv = true;
            }
            recv.AR0001 = materials;

            try
            {
                if (sendDataToEIP(recv))
                {
                    int retValue = db.Ado.ExecuteCommand("Update PurchaseBody set IsCreateRecv=1 where PurchaseNo=@No and DeliveryLot=@Lot", new { No = PurchaseNo, Lot = Lot });
                    if (retValue == 0)
                    {
                        return true;
                    }

                }
            }
            catch
            {
                return false;
            }
            
            return true;
        }


        public static bool CreatePUR(string RequireNo, string CaseOfficer)
        {
            ConnectionStringSettings settings = WebConfigurationManager.ConnectionStrings["DefaultConnection"];

            SqlSugarClient db = SugarFactory.GetInstance(settings.ConnectionString);

            RequirementHeader reqHeader = db.Queryable<RequirementHeader>().Where(e => e.OrderNo == RequireNo).Single();

            Employee emp = EmployeeFactory.getEmployee(CaseOfficer);

            UNIT unit = UnitFactory.getUint(emp.UNITNO);
            
            List<PUR01> purs = new List<PUR01>();
            List<purMaterial> materials = null;
            MaterialInfo materialInfo = null;
            List<RequirementBody> reqBodyList = null;

            reqBodyList = db.Queryable<RequirementBody>().Where(e => e.OrderNo == RequireNo).ToList();

            PUR01 pur = null;

            pur = new PUR01
            {
                apikey = "Rp6pu5AauFnT",
                fid = "304",
                fdraft = "1",
                fowner = CaseOfficer,
                承辦人 = CaseOfficer,
                採購承辦人 = CaseOfficer,
                採購單位 = unit.UNITNO,
                CreateDate = DateTime.Now.ToString("yyyyMMdd"),
                需求單號 = RequireNo
            };

            materials = new List<purMaterial>();
            foreach (RequirementBody body in reqBodyList)
            {
                materialInfo = db.Queryable<MaterialInfo>().Where(e => e.MaterialNo == body.MaterialNo).Single();

                materials.Add(new purMaterial
                {
                    品名 = materialInfo.MaterialName,
                    單位 = materialInfo.Unit,
                    料號 = materialInfo.MaterialNo,
                    規格 = materialInfo.Spec,
                    契約數量 = body.RequirementQty.ToString(),
                    單價 = body.EstPrice.ToString(),
                    需求單位 = body.RequireUnit
                   
                });
                
            }
            pur.AR0001 = materials;

            try
            {
                if (sendDataToEIP(pur))
                {
                    int retValue = db.Ado.ExecuteCommand("Update RequirementHeader set IsCreateForm=1 where OrderNo=@No ", new { No = RequireNo});
                    if (retValue == 0)
                    {
                        return true;
                    }

                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}