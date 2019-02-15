using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using Destiny.Web.Data;
using Destiny.Web.Models;

namespace Destiny.Web.DB
{
    public class DBHelper
    {
        //private string DBPath = ConfigurationManager.AppSettings["DBPath"];
        //private string DBConn = ConfigurationManager.ConnectionStrings["SqlLiteDB"].ToString();
        //private SQLiteCommand comm;
        //private SQLiteDataAdapter adpt;

        public string GetContentByNum(string number)
        {
            string content = "";
            using (Elin8999Entities db = new Elin8999Entities())
            {
                var detail = db.ForcastMessages.FirstOrDefault(s => s.Code.Equals(number));
                if (detail != null)
                {
                    content = detail.Content;
                }
            }
            return content;
        }

        public string GetNumberByName(string name, ref string number)
        {
            string result = "";
            char[] charWords = name.ToArray();
            string[] words = new string[charWords.Length];
            IList<string> cnt = new List<string>();
            IList<Names> names = new List<Names>();
            for (int i = 0; i < charWords.Length; i++)
            {
                words[i] = "'" + charWords[i].ToString() + "'";
                names.Add(new Names()
                {
                    index = i + 1,
                    name = charWords[i].ToString()
                });
            }
            string sqlName = string.Join(",", words);
            List<Names> ResultNames = null;
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    ResultNames = (from n in names
                                   join s in db.Strokes on n.name equals s.Words
                                   select new Names()
                                   {
                                       name = n.name,
                                       stroke = s.StrokeCnt.Value,
                                       index = n.index
                                   }).ToList();
                    int cntW = ResultNames.Count;
                    int frontCnt = 0;
                    int backCnt = 0;
                    int cumCnt = 0;
                    if (cntW % 2 == 1)
                    {
                        var a = (from w in ResultNames
                                 where w.index <= (cntW - 1) / 2
                                 select w.stroke).ToList();
                        var b = (from w in ResultNames
                                 where w.index > (cntW - 1) / 2
                                 select w.stroke).ToList();
                        cumCnt = (from w in ResultNames select w.stroke).Sum();
                        frontCnt = a.Sum();
                        backCnt = b.Sum();
                    }
                    else
                    {
                        var a = from w in ResultNames
                                where w.index <= (cntW) / 2
                                select w.stroke;
                        var b = from w in ResultNames
                                where w.index > (cntW) / 2
                                select w.stroke;
                        frontCnt = a.Sum();
                        backCnt = b.Sum();
                        cumCnt = (from w in ResultNames select w.stroke).Sum();
                    }
                    var resultNum = ((frontCnt % 8) == 0 ? 8 : (frontCnt % 8)).ToString()
                        + ((backCnt % 8) == 0 ? 8 : (backCnt % 8)).ToString()
                        + ((cumCnt % 6) == 0 ? 6 : (cumCnt % 6)).ToString();
                    number = resultNum;
                    result = db.ForcastMessages.FirstOrDefault(s => s.Code.Equals(resultNum)).Content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
            }

            return result;
        }

        public UserViewModel Login(UserViewModel userInfor)
        {
            UserViewModel user = new UserViewModel();
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.Users.FirstOrDefault(s => s.Mobile == userInfor.Mobile && s.Password == userInfor.Password);
                    if (detail == null)
                        throw new Exception("手机号或密码不正确");
                    else
                    {
                        detail.LoginDate = DateTime.Now;
                        user.Mobile = detail.Mobile;
                        user.UserName = detail.UserName;
                        user.RoleID = detail.RoleID;
                        user.Email = detail.Email;
                        db.SaveChanges();
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserViewModel Register(UserViewModel userInfor)
        {
            UserViewModel user = new UserViewModel();
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.Users.FirstOrDefault(s => s.Mobile == userInfor.Mobile);
                    if (detail != null)
                        throw new Exception("该手机用户已存在，请重新填写新手机注册！");
                    else
                    {
                        DateTime currentDate = DateTime.Now;
                        User newUser = db.Users.Create();
                        newUser.Mobile = userInfor.Mobile;
                        newUser.UserName = userInfor.UserName;
                        newUser.RoleID = 1;
                        newUser.Email = userInfor.Email;
                        newUser.IsActive = true;
                        newUser.LoginDate = currentDate;
                        newUser.CreateDate = currentDate;
                        newUser.CreatedBy = userInfor.UserName;
                        newUser.Password = userInfor.Password;
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        userInfor.RoleID = 1;
                        return userInfor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string GetUserName(string year, string month)
        //{
        //    DataSet ds = null;
        //    IDataReader dr = null;
        //    string userNumber = "";
        //    string userName = "";
        //    string SqlQuery = "select top 1 UserNumber from UserNumber where Type = 'User' order byUserNumber desc ";
        //    using (SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(DBConn))
        //    {
        //        conn.Open();
        //        comm = new SQLiteCommand(conn);
        //        comm.CommandText = SqlQuery;
        //        dr = comm.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            userNumber = dr["UserNumber"].ToString();
        //        }
        //        dr.Close();

        //        //if()

        //    }
        //    return userName;
        //}

        public List<Stroke> GetStrokeList(string word)
        {
            List<Stroke> list = new List<Stroke>();

            using (Elin8999Entities db = new Elin8999Entities())
            {
                var strokes = db.Strokes.Where(w => word.Equals("") || w.Words.Equals(word));
                if (strokes.FirstOrDefault() != null)
                    list = strokes.ToList();
            }

            return list;
        }

        public void SaveStroke(Stroke stroke)
        {
            using (Elin8999Entities db = new Elin8999Entities())
            {
                var detail = db.Strokes.FirstOrDefault(s => s.ID == stroke.ID);
                detail.StrokeCnt = stroke.StrokeCnt;
                detail.CreateBy = stroke.CreateBy;
                detail.CreateDate = stroke.CreateDate;
                db.SaveChanges();
            }
        }

        public void AddStroke(Stroke stroke)
        {
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.Strokes.FirstOrDefault(s => s.Words == stroke.Words);
                    if (detail != null)
                        throw new Exception("该字及笔画已经存在，请重新填写！");
                    Stroke newStroke = db.Strokes.Create();
                    newStroke.StrokeCnt = stroke.StrokeCnt;
                    newStroke.Words = stroke.Words;
                    newStroke.CreateBy = stroke.CreateBy;
                    newStroke.CreateDate = stroke.CreateDate;
                    db.Strokes.Add(newStroke);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSixtyFourDongContent(string firstNum, string secondNum)
        {
            string content = "";
            try
            {
                string[] firstSplit = firstNum.Split('-');
                string[] secondSplit = secondNum.Split('-');
                string FirstNum = firstSplit[0] + secondSplit[0];
                string SecondNum = firstSplit[1] + secondSplit[1];
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.SixtyFourDongs.FirstOrDefault(s => s.FirstNum == FirstNum && s.SecondNum == SecondNum);
                    if (detail != null)
                    {
                        content = detail.SecondNumName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return content;
        }

        public static string GetOrderNumber(string type)
        {
            string orderNumber = "";

            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.OrderNumbers.FirstOrDefault(s => s.OrderType == type);
                    if (detail == null)
                    {
                        OrderNumber newNum = db.OrderNumbers.Create();
                        newNum.OrderType = type;
                        orderNumber = "1".PadLeft(10, '0');
                        newNum.OrderNum = orderNumber;
                        db.OrderNumbers.Add(newNum);
                    }
                    else
                    {
                        int num = 0;
                        int.TryParse(detail.OrderNum, out num);
                        orderNumber = (num + 1).ToString().PadLeft(10, '0');
                        detail.OrderNum = orderNumber;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return orderNumber;
        }

        public void CreateOrder(OrderViewModel model)
        {
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    Order newOrder = db.Orders.Create();
                    newOrder.OrderID = model.OrderNumber;
                    newOrder.ProductCode = model.ProductCode;
                    newOrder.Subject = model.Subject;
                    newOrder.Body = model.Body;
                    newOrder.IsPaid = false;
                    newOrder.CreateDate = DateTime.Now;
                    newOrder.Amount = model.Amount;
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PayOrder(OrderViewModel model)
        {
            try
            {
                using (Elin8999Entities db = new Elin8999Entities())
                {
                    var detail = db.Orders.FirstOrDefault(s => s.OrderID == model.OrderNumber);
                    if (detail != null)
                    {
                        detail.AliPayCode = model.AliPayCode;
                        detail.IsPaid = true;
                        detail.PaidDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }



    public class Names
    {
        public int index { get; set; }
        public string name { get; set; }
        public int stroke { get; set; }
    }
}