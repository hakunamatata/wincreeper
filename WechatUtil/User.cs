using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatUtil
{
    public static class WechatEngine
    {

        #region === 获取关注者列表 ===
        ///// <summary>
        ///// 获取所有用户openid
        ///// </summary>
        ///// <returns></returns>
        public async static Task<string[]> GetUsers()
        {
            GetUserResult res;
            GetUserRequest req = new GetUserRequest();
            List<string> users = new List<string>();
            string next = string.Empty;
            do {
                req.next_openid = next;
                res = await req.SendAsync();
                if (res.count > 0)
                    users.AddRange(res.data.openid);
                next = res.next_openid;
            } while (!string.IsNullOrEmpty(next));
            return users.ToArray();
        }

        #endregion

        #region === 获取所有用户详细信息 ===

        /// <summary>
        /// 获取所有用户详细信息
        /// </summary>
        public static async Task<GetUserDetailResult[]> GetUserDetails()
        {
            List<GetUserDetailResult> list = new List<GetUserDetailResult>();
            string[] users = await GetUsers();
            string[] taken;
            int i = 0;
            do {
                taken = users.Skip(i * 100).Take(100).ToArray();
                var request = new BatchGetUserRequest();
                foreach (var t in taken)
                    request.user_list.Add(new BatchPostUser(t));
                var result = await request.SendAsync();
                list.AddRange(result.user_info_list);
                i++;
            } while (taken.Count() == 100);
            return list.ToArray();
        }
        #endregion
    }
}
