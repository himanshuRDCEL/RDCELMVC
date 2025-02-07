using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.Web.API.Models;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.QRCode;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using GraspCorn.Common.Helper;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class QRCodeController : Controller
    {
        #region Variable declaration
        //ExchangeOrderManager _exchangeOrderManager;
        BusinessPartnerRepository _businessPartnerRepository;
        // BusinessUnitRepository _businessUnitRepository;
        //ABBRegistrationRepository _ABBRegistrationRepository;
        //ExchangeOrderRepository _sponsorRepository;
        #endregion

        // GET: QRCode
        public ActionResult Index()
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            List<tblBusinessPartner> businessPartnerList = new List<tblBusinessPartner>();
            try
            {
                businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true).ToList();

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("QRCode", "Index", ex);
            }
            return View(businessPartnerList);
        }

        // GET: QRCode/Details/5
        public ActionResult Details(int id)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            try
            {
                businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == id);
                //if (businessPartnerObj != null && businessPartnerObj.QRCodeURL != null)
                //{
                //    //generate QR Code and image

                //    string url = ConfigurationManager.AppSettings["BaseURL"].ToString() + businessPartnerObj.QRCodeURL;
                //    QRCodeGenerator ObjQr = new QRCodeGenerator();

                //    QRCodeData qrCodeData = ObjQr.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);

                //    Bitmap bitMap = new QRCode(qrCodeData).GetGraphic(20);

                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //        byte[] byteImage = ms.ToArray();

                //        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                //        //img.Save(Server.MapPath("Content/StoreQRCode") + "Test.Png", System.Drawing.Imaging.ImageFormat.Png);

                //        //string fileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF") + ".png";
                //        string fileName = businessPartnerObj.BusinessPartnerId + "BPId" + businessPartnerObj.BusinessUnitId + "BUId" + ".png";
                //        // string filePath = "E:\\Priyanka\\DIGI2L\\UTC_Bridge\\src\\RDCEL.DocUpload.Web.API\\Content\\DB_Files\\StoreQRCode\\" + fileName;
                //        string filePath = ConfigurationManager.AppSettings["QRCodeImage"].ToString() + fileName;

                //        // if (!File.Exists(filePath))
                //        img.Save(filePath, ImageFormat.Png);  // Or Png

                //        //string url = ConfigurationManager.AppSettings["BaseURL"].ToString() + ConfigurationManager.AppSettings["PatrolReport"].ToString();

                //        string imageUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/StoreQRCode/" + fileName;

                //        // string imageUrl = url + fileName;

                //        // ViewBag.Image = "http://localhost:44318/Content/DB_Files/StoreQRCode/" + fileName;
                //        // ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                //        businessPartnerObj.QRImage = imageUrl;

                //    }
                //}
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("QRCode", "Details", ex);
            }

            return View(businessPartnerObj);

        }

        // GET: QRCode/Create
        public ActionResult Create()
        {
            BusinessPartnerViewModel BusinessPartnerObj = new BusinessPartnerViewModel();

            try
            {
                BusinessPartnerObj.BusinessUnitId = 1;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("QRCode", "Create", ex);
            }

            return View(BusinessPartnerObj);
        }

        // POST: QRCode/Create
        [HttpPost]
        public JsonResult Create(BusinessPartnerViewModel model)
        {
            QRCodeManager QRCodeInfo = new QRCodeManager();
            int BPId = 0;
            bool flag = false;

            try
            {
                if (model != null)
                {
                    #region Code to add ABB Reg. details in database
                    BPId = QRCodeInfo.AddBusinessPartner(model);
                    if (BPId != 0)
                    {
                        model.QRCodeURL = "ABB/ABBRegistration?BUId=1&BPId=" + BPId;

                        if (model.QRCodeURL != null)
                        {
                            BPId = QRCodeInfo.UpdateBusinessPartner(model.QRCodeURL, BPId);
                            flag = true;
                        }
                        
                    }
                    #endregion
                }
               
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("QRCode", "Create", ex);
            }            
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        // GET: QRCode/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QRCode/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QRCode/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QRCode/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
