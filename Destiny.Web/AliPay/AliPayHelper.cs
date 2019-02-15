using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Destiny.Common;
using Destiny.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Destiny.Web.AliPay
{
    public class AliPayHelper
    {
        public static string PayAction(AlipayTradePagePayModel model, string num, string retrunUrl)
        {
            AliPayConfig config = new AliPayConfig();

            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl,
                config.app_id,
                config.private_key,
                "json",
                "1.0",
                config.sign_type,
                config.alipay_public_key,
                config.charset,
                false);

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(retrunUrl);
            // 设置异步通知接收地址
            request.SetNotifyUrl("");
            // 将业务model载入到request
            request.SetBizModel(model);
            AlipayTradePagePayResponse response = null;
            response = client.pageExecute(request, null, "post");
            return response.Body;
        }
    }
}