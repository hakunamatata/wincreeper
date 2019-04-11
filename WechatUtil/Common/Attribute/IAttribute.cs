using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatUtil.Common
{
    public interface IVerifyAttribute//验证属性接口
    {
        bool Verify(Type type, object obj,out string message);
    }
}
