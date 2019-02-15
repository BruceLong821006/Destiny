using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Destiny.Web.Models
{
    public class AliPayConfig
    {
        public AliPayConfig()
        { }

        // 应用ID,您的APPID
        public string app_id = ConfigurationManager.AppSettings["AliPayAppID"];

        // 支付宝网关
        public string gatewayUrl = "https://openapi.alipay.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥
        public string private_key = ConfigurationManager.AppSettings["AliPayPrivateKey"];

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public string alipay_public_key = ConfigurationManager.AppSettings["AliPayPublicKey"];

        // 签名方式
        public string sign_type = "RSA2";

        // 编码格式
        public string charset = "UTF-8";
    }
}