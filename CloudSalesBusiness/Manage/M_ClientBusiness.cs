﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using CloudSalesEntity;
using CloudSalesDAL;
using CloudSalesTool;


namespace CloudSalesBusiness
{
    public class M_ClientBusiness
    {
        #region 查询

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <param name="keyWords"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<M_Clients> GetClients(string keyWords, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            DataTable dt = CommonBusiness.GetPagerData("M_Clients", "*", "Status<>9", "AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<M_Clients> list = new List<M_Clients>();
            M_Clients model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Clients();
                model.FillData(item);
                model.City = CommonCache.Citys.Where(c => c.CityCode == model.CityCode).FirstOrDefault();
                model.IndustryEntity = C_IndustryBusiness.GetIndustryByClientID(AppSettings.Settings[AppSettingsWEB.Manage, "ClientID"]).Where(i => i.IndustryID.ToLower() == model.Industry.ToLower()).FirstOrDefault();
                list.Add(model);
            }

            return list;
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="model">M_Clients 对象</param>
        /// <param name="loginName">账号</param>
        /// <param name="loginPwd">密码</param>
        /// <param name="userid">操作人</param>
        /// <param name="result">0：失败 1：成功 2：账号已存在 3：模块未选择</param>
        /// <returns></returns>
        public static string InsertClient(M_Clients model, string loginName, string loginPwd, string userid, out int result)
        {
            string modules = "";
            foreach (var item in model.Modules)
            {
                modules += item.ModulesID + ",";
            }
            if (modules == "")
            {
                result = 3;
                return "";
            }
            loginPwd = CloudSalesTool.Encrypt.GetEncryptPwd(loginPwd, loginName);
            modules = modules.Substring(0, modules.Length - 1);
            string clientid = new M_ClientDAL().InsertClient(model.CompanyName, model.ContactName, model.MobilePhone, model.Industry, model.CityCode,
                                                             model.Address, model.Description, loginName, loginPwd, modules, userid, out result);
            return clientid;
        }

        #endregion

        #region 编辑
        #endregion

    }
}
