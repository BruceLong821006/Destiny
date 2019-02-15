using Aop.Api.Domain;
using Destiny.Common;
using Destiny.Web.AliPay;
using Destiny.Web.Controllers.Base;
using Destiny.Web.DB;
using Destiny.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Destiny.Web.Controllers
{
    public class ForecastController : Master
    {
        private DBHelper db = new DBHelper();
        // GET: Forecast
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NumForecast()
        {
            return View();
        }

        public ActionResult NameForecast()
        {
            return View();
        }

        public JsonResult NumSubmit(string num, string OrderNum, string tradeNO)
        {
            int frontNum = 0;
            int backNum = 0;
            Int64 fullNum = 0;
            int num1 = 0;
            int num2 = 0;
            Int64 num3 = 0;
            string result = "";
            long longNum = 0;
            string number = "";
            try
            {
                string decodeResult = Base64Helper.Base64Decode(num);
                OrderPaid(OrderNum, tradeNO);
                if (!string.IsNullOrEmpty(decodeResult) && Int64.TryParse(decodeResult, out longNum))
                {
                    switch (decodeResult.Length)
                    {
                        case 3:
                        case 5:
                            int a = decodeResult.Length / 2;
                            int b = decodeResult.Length / 2 + 1;
                            frontNum = int.Parse(decodeResult.Substring(0, a));
                            backNum = int.Parse(decodeResult.Substring(a, b));
                            break;
                        case 4:
                        case 6:
                            frontNum = int.Parse(decodeResult.Substring(0, decodeResult.Length / 2));
                            backNum = int.Parse(decodeResult.Substring(decodeResult.Length / 2, decodeResult.Length / 2));
                            break;
                        case 7:
                            frontNum = int.Parse(decodeResult.Substring(0, 3));
                            backNum = int.Parse(decodeResult.Substring(3, 4));
                            break;
                        case 8:
                            frontNum = int.Parse(decodeResult.Substring(0, 4));
                            backNum = int.Parse(decodeResult.Substring(4, 4));
                            break;
                        case 9:
                            frontNum = int.Parse(decodeResult.Substring(0, 4));
                            backNum = int.Parse(decodeResult.Substring(4, 5));
                            break;
                        case 10:
                            frontNum = int.Parse(decodeResult.Substring(0, 5));
                            backNum = int.Parse(decodeResult.Substring(5, 5));
                            break;
                        case 11:
                            frontNum = int.Parse(decodeResult.Substring(0, 5));
                            backNum = int.Parse(decodeResult.Substring(5, 6));
                            break;
                        case 12:
                            frontNum = int.Parse(decodeResult.Substring(0, 6));
                            backNum = int.Parse(decodeResult.Substring(6, 6));
                            break;
                        default:
                            result = "数字位数不正确！";
                            break;
                    }
                    fullNum = Int64.Parse(decodeResult);
                    if (string.IsNullOrEmpty(result))
                    {
                        while (frontNum > 0)
                        {
                            num1 += frontNum % 10;
                            frontNum /= 10;
                        }
                        while (backNum > 0)
                        {
                            num2 += backNum % 10;
                            backNum /= 10;
                        }
                        while (fullNum > 0)
                        {
                            num3 += fullNum % 10;
                            fullNum /= 10;
                        }
                        num1 = (num1 % 8) == 0 || (num1 % 8) == 8 ? 8 : (num1 % 8);
                        num2 = (num2 % 8) == 0 || (num2 % 8) == 8 ? 8 : (num2 % 8);
                        num3 = (num3 % 6) == 0 || (num3 % 6) == 6 ? 6 : (num3 % 6);
                        number = num1.ToString() + num2.ToString() + num3.ToString();

                        result = db.GetContentByNum(number);
                    }
                }
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", Num = number, Result = result }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult NameSubmit(string name, string OrderNum, string tradeNO)
        {
            string result = "";
            long longNum = 0;
            string number = "";
            try
            {
                OrderPaid(OrderNum, tradeNO);
                if (!Int64.TryParse(name, out longNum))
                {
                    DBHelper db = new DBHelper();
                    result = db.GetNumberByName(name, ref number);
                }
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", Num = number, Result = result }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SixtyFourDong()
        {
            return View();
        }

        public JsonResult SixtyFourDongSubmit()
        {
            SixtyFourModel model = new SixtyFourModel();
            Random rdNum = new Random();
            int num1 = rdNum.Next(1, 64);
            int num2 = rdNum.Next(1, 64);
            int num3 = rdNum.Next(1, 64);
            int num4 = rdNum.Next(1, 64);
            int num5 = rdNum.Next(1, 64);
            int num6 = rdNum.Next(1, 64);
            string[] nums = new string[6];
            for (int i = 0; i < 6; i++)
            {
                nums[i] = model.Pools[rdNum.Next(1, 64)];
            }
            try
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", Num = nums }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SixtyFourDongCalc(string firstNum, string secondNum)
        {
            string content = "";
            try
            {
                DBHelper db = new DBHelper();
                content = db.GetSixtyFourDongContent(firstNum, secondNum);
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", Content = content }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NumPay(string num)
        {
            try
            {
                // 组装业务参数model
                AlipayTradePagePayModel model = new AlipayTradePagePayModel();
                model.Body = "号码现象";
                model.Subject = "号码现象";
                model.TotalAmount = ConfigurationManager.AppSettings["TotalAmount"];
                model.OutTradeNo = "NUM" + DBHelper.GetOrderNumber("NUM");
                model.ProductCode = "FAST_INSTANT_TRADE_PAY";
                CreateOrder(model);
                string WebUrl = ConfigurationManager.AppSettings["WebUrl"];
                string base64Result = Base64Helper.Base64Encode(num);
                string returnUrl = WebUrl + "/Forecast/NumForecast?num=" + base64Result;
                string body = AliPayHelper.PayAction(model, num, returnUrl);
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", body = body }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }
        }

        private void CreateOrder(AlipayTradePagePayModel model)
        {
            try
            {
                OrderViewModel OrderModel = new OrderViewModel();
                OrderModel.Body = model.Body;
                OrderModel.Subject = model.Body;
                OrderModel.OrderNumber = model.OutTradeNo;
                OrderModel.ProductCode = model.ProductCode;
                OrderModel.Amount = decimal.Parse(model.TotalAmount);
                DBHelper db = new DBHelper();
                db.CreateOrder(OrderModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OrderPaid(string OrderNum, string tradeNO)
        {
            try
            {
                OrderViewModel model = new OrderViewModel();
                model.OrderNumber = OrderNum;
                model.AliPayCode = tradeNO;
                DBHelper db = new DBHelper();
                db.PayOrder(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult NamePay(string Name)
        {
            try
            {
                // 组装业务参数model
                AlipayTradePagePayModel model = new AlipayTradePagePayModel();
                model.Body = "姓名现象";
                model.Subject = "姓名现象";
                model.TotalAmount = ConfigurationManager.AppSettings["TotalAmount"];
                model.OutTradeNo = "NAME" + DBHelper.GetOrderNumber("NAME");
                model.ProductCode = "FAST_INSTANT_TRADE_PAY";
                CreateOrder(model);
                string WebUrl = ConfigurationManager.AppSettings["WebUrl"];
                string base64Result = Base64Helper.Base64Encode(Name);
                string returnUrl = WebUrl + "/Forecast/NameForecast?num=" + base64Result;
                string body = AliPayHelper.PayAction(model, Name, returnUrl);
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Success", body = body }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JavaScriptSerializer().Serialize(new { Status = "Error", ErrorMessage = ex.Message }), JsonRequestBehavior.AllowGet);
            }
        }
    }
}