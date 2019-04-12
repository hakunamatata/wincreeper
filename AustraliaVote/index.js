/* API Server*/
const server = location.origin + '/';

//querymap: {
//    // 主题编号
//    topic: 'xxxxx-xxxx-xxxx-xxxx-xxxxxxxx';
//    // 微信授权的 code
//    code: 'xxxxx';
//    // 来源微信openid
//    invite: '';
//}

/**
 *  页面参数
 */
let query = location.search.charAt(0) == '?' ?
    Qs.parse(location.search.substring(1)) :
    {};

let pageParam = {
    topic: query.topic
}

let globalData = {

}
/*
 * pageParam: 保存页面中的关键信息 
 */
if (query.invite)
    pageParam.invite = query.invite;

function getPageUrl() {
    return location.origin + location.pathname + '?' + Qs.stringify(pageParam);
}

/**
 * 获取服务器中的微信页面配置参数
 * @param {any} success
 * @param {any} error
 */
function get_wxconfig(success, error) {
    axios.post(server + 'sns/wxconf', { url: location.href })
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);

}
/**
 * 基础授权,获取openid
 * @param {any} success
 * @param {any} error
 */
function auth_base(success, error) {
    let invite = query.invite == undefined ? '' : query.invite;
    axios.post(server + 'sns/authbase?code=' + query.code + '&topic=' + query.topic + '&invite=' + invite)
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error)
}

/**
 * 获取用户详细信息
 * @param {any} success
 * @param {any} error
 */
function user_info(success, error) {

    axios.get(server + 'sns/userinfo?code=' + query.code)
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data)
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);
}

/**
 * 获取投票主题的信息
 * @param {String} topicId 主题ID
 * @param {Function} callback 接口调用成功    
 * @param {Function} error 接口调用失败
 */
function get_topic(topicId, userId, success, error) {
    axios.get(server + 'vote/topic/' + topicId + '?uid=' + userId)
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);
};


/**
 * 对主题进行投票
 * @param {String} topicId 主题ID
 * @param {Object} data 投票数据
 * {
 *   uid: String 微信用户的openid,
 *   subs: Array 主题中所有投票题目
 *   [{
 *      id: String 题目编号
 *      ops: Array 题目中的选项
 *      [{
 *        id: String 选项编号
 *        order:Number 选择顺序
 *      }]
 *   }]
 * }
 * @param {Function} success 接口调用成功回调
 * @param {Function} error 接口调用失败回调
 */
function vote_topic(topicId, data, success, error) {
    axios.post(server + 'vote/topic/' + topicId, data)
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);

};

/**
 * 获取某人在某主题下的投票结果
 * @param {any} topicId
 * @param {any} userId
 * @param {any} success
 * @param {any} error
 */
function get_myvotes(topicId, userId, success, error) {
    axios.post(server + 'vote/topic/myvotes', { tid: topicId, uid: userId })
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        }).catch(error);
}

/**
 * 喜欢某一个主题
 * @param {String} topicId 主题编号
 * @param {Function} success 接口调用成功回调
 * @param {Function} error 接口调用失败回调
 * @return {Number} 返回当前主题被喜欢的数量
 */
function like_topic(topicId, success, error) {
    axios.post(server + 'vote/topic/like', { tid: topicId })
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);
}

/**
 * 获取某主题的拉票排行
 * @param {any} topicId 主题编号
 * @param {any} openId 用户openId
 * @param {any} success 接口调用成功回调
 * @param {any} error 接口调用失败回调
 */
function get_rank(topicId, openId, success, error) {
    axios.post(server + 'vote/topic/rank', { tid: topicId, uid: openId })
        .then(res => {
            if (res.status == 200) {
                if (success) success(res.data);
            }
            else {
                if (error) error(res);
            }
        })
        .catch(error);
}

let wxConfig = {};
/*
 * 获取微信配置以及微信页面初始化
**/
get_wxconfig(res => {
    console.log('微信初始化成功');
    wxConfig = res;
    wx.config({
        debug: false,
        appId: res.appid,
        timestamp: res.timestamp,
        nonceStr: res.nonce,
        signature: res.signature,
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage']
    });
    // 加载页面
    pageLoad();

}, err => {
    console.log('页面加载失败: ' + err);
})

/**
 * 分享至朋友圈
 * @param {any} title 分享标题
 * @param {any} desc 分享描述
 * @param {any} link 分享连接
 * @param {any} imgUrl 图片地址
 */
function shareTimeline(title, desc, link, imgUrl) {
    if (WeixinJSBridge)
        WeixinJSBridge.invoke('shareTimeline', {
            "img_url": imgUrl,
            "link": link,
            "desc": desc,
            "title": title
        });
}

/**
 * 发送给好友
 * @param {any} title 分享标题
 * @param {any} desc 分享描述
 * @param {any} link 分享连接
 * @param {any} imgUrl 图片地址
 */
function sendAppMessage(title, desc, link, imgUrl) {
    if (WeixinJSBridge)
        WeixinJSBridge.invoke('sendAppMessage', {
            "img_url": imgUrl,
            "link": link,
            "desc": desc,
            "title": title
        });
}

let shareTitle = '';
let shareContent = '';
let shareLink = '';
let shareImg = '';

wx.ready(() => {
    console.log('微信准备就绪');
    wx.onMenuShareAppMessage({
        title: shareTitle, // 分享标题
        desc: shareContent, // 分享描述
        link: shareLink, // 分享链接
        imgUrl: shareImg, // 分享图标
    });

    wx.onMenuShareTimeline({
        title: shareTitle, // 分享标题
        desc: shareContent, // 分享描述
        link: shareLink, // 分享链接
        imgUrl: shareImg, // 分享图标
    });
    Vue.prototype.wx = wx;
});

Vue.prototype.$http = axios;

function pageLoad() {
    if (query.code === '' || query.code == null) {
        location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + wxConfig.oauthid + '&redirect_uri=' + encodeURIComponent(location.href) + '&response_type=code&scope=snsapi_base&state=requestingCallback#wechat_redirect';
        return;
    }



    auth_base(res => {
        if (res.openid == '' || res.openid == null) {
            location.href = getPageUrl();
            return;
        }
        globalData = { ...res, ...globalData }
        render_page();
    }, err => {
        location.href = getPageUrl();
        return;
    });
}

function render_page() {
    new Vue({
        el: '#app',
        data() {
            return {
                showRemarks: false,
                likeStatus: false,
                pageData: {},
                postData: {},
                userInfo: {},
                rankData: [],
                item: []
            }
        },
        methods: {
            /**
             * 投票支持
             * @param {any} opt Option
             * @param {any} sub SubjectId
             */
            support(opt, sub) {
                if (this.isVoted) return;
                sub.Options.forEach(o => {
                    o.active = false;
                });
                opt.active = true;
                let postSub = null;
                this.postData.subs.forEach(s => {
                    if (s.id == sub.Id) {
                        postSub = s;
                    }
                });
                if (postSub == null) {
                    postSub = { id: sub.Id }
                    this.postData.subs.push(postSub);
                }
                postSub.ops = sub.Options.map((o, i) => {
                    if (o.active) {
                        return {
                            id: o.Id,
                            order: o.Order
                        }
                    }
                    return null;
                }).filter(p => p != null);
            },
            like(Id) {
                like_topic(Id, res => {
                    this.pageData.Likes = res;
                    localStorage.setItem('like:' + query.topic, '1');
                    this.likeStatus = true;
                }, err => {
                    console.log(err);
                })
            },
            post() {
                var that = this;
                if (!this.userObtained) {
                    //localStorage.setItem('tempVotes', JSON.stringify(this.postData));
                    weui.alert('为保证投票结果的准确性，需要您的授权', () => {
                        location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + wxConfig.oauthid + '&redirect_uri=' + encodeURIComponent(getPageUrl()) + '&response_type=code&scope=snsapi_userinfo&state=requestingCallback#wechat_redirect';
                    });
                    return;
                }

                /**
                 * 投票检查，是否有漏选
                 * */
                try {
                    this.pageData.Subjects.forEach(s => {
                        var check = that.postData.subs.find(p => p.id == s.Id);
                        if (check == null)
                            throw '请对 \"' + s.Title + '\"进行投票'
                        else if (check.ops.length == 0)
                            throw '请对 \"' + s.Title + '\"进行投票'
                        else if (check.ops.length > s.MaxOptions)
                            throw '\"' + s.Title + '\" 最多只能投 ' + s.MaxOptions + '票';
                    });
                } catch (e) {
                    weui.alert(e);
                    return;
                };

                var loading = weui.loading("提交选票中...");
                vote_topic(query.topic, that.postData, res => {
                    document.documentElement.scrollTop = 0;
                    loading.hide();
                    weui.toast("投票成功");
                    that.showRemarks = true;
                    this.getTopic();
                }, err => {
                    loading.hide();
                    weui.toast(err);
                });
            },

            getTopic(callback) {
                get_topic(query.topic, this.postData.uid,
                    res => {
                        let media = res.Media;
                        if (!media.startsWith('http://') && !media.startsWith('https://'))
                            media = server + media;
                        pageParam.invite = this.postData.uid;
                        shareTitle = res.Title;
                        shareContent = res.Content;
                        shareLink = getPageUrl();
                        shareImg = media;
                        let myvotes = res.MyVotes || []
                        for (var value of res.Subjects) {
                            for (var opt of value.Options) {
                                if (myvotes.includes(opt.Id))
                                    opt.active = true;
                                else
                                    opt.active = false;
                            }
                        }
                        this.pageData = res;
                        //if (localStorage.getItem('tempVotes') == null) {
                        this.postData.subs = res.MySubs || [];
                        //} else {
                        //    this.postData = JSON.parse(localStorage.getItem('tempVotes'))
                        //     localStorage.removeItem('tempVotes');
                        // }
                        if (callback) callback(res);
                    }, err => {
                        console.log(err);
                    });

            },

            getUserInfo() {
                user_info(res => {
                    this.userInfo = res;
                    this.getRank();
                }, err => {
                    console.log(err);
                })
            },
            getRank() {
                get_rank(query.topic, this.userInfo.openid, res => {
                    this.rankData = res;
                    console.log('rank:');
                    console.log(res);
                }, err => {
                    console.log(err);
                });
            },
            shareTimeline() {
            }

        },
        computed: {
            userObtained() {
                return this.userInfo.openid != null && this.userInfo.headimgurl != null && this.userInfo.nickname != null;
            },
            isVoted() {
                return this.pageData.MyVotes && this.pageData.MyVotes.length > 0;
            }
        },
        created() {
            this.postData.uid = globalData.openid
            this.postData.subs = [];
            this.likeStatus = localStorage.getItem('like:' + query.topic) == '1';
        },
        mounted() {
            this.getTopic();
            this.getUserInfo();
        }
    });
}