﻿let wxConfig = {};
/*
 * 获取微信配置以及微信页面初始化
**/
get_wxconfig(res => {
    wxConfig = res;
    wx.config({
        debug: false,
        appId: res.appid,
        timestamp: res.timestamp,
        nonceStr: res.nonce,
        signature: res.signature,
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage']
    });

    wx.ready(() => {
        Vue.prototype.wx = wx;
    });

    Vue.prototype.$http = axios;
    wxLoading = weui.loading('加载中');

    // 加载页面
    pageLoad();

}, err => {
});

let wxLoading;
function pageLoad() {
    if (query.code == '' || query.code == null) {
        location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + wxConfig.oauthid + '&redirect_uri=' + encodeURIComponent(getPageUrl()) + '&response_type=code&scope=snsapi_base&state=#wechat_redirect';
        return;
    };

    auth_base(res => {
        if (res.openid == '' || res.openid == null) {
            location.href = getPageUrl();
            return;
        };
        globalData = { ...res, ...globalData };
        render_page();
    }, err => {
        weui.alert('服务器繁忙, 请稍后再试', () => {
            location.href = getPageUrl();
        });
        return;
    });
};

function render_page() {
    new Vue({
        el: '#app',
        data() {
            return {
                showRemarks: false,
                showRules: false,
                likeStatus: false,
                shareObject: {
                    title: '',
                    desc: '',
                    link: '',
                    imgUrl: ''
                },
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
                    postSub = { id: sub.Id };
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
                    localStorage.setItem('tempVotes', JSON.stringify(this.postData));
                    weui.alert('为保证投票结果的准确性，需要您的授权', () => {
                        location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=' + wxConfig.oauthid + '&redirect_uri=' + encodeURIComponent(getPageUrl()) + '&response_type=code&scope=snsapi_userinfo&state=requestingCallback#wechat_redirect';
                    });
                    return;
                }

                /**
                 * 投票检查，是否有漏选
                 * */
                if (localStorage.getItem('tempVotes') == null) {
                    // 没有临时数据, 为新一轮投票
                    // 否则为授权之后的动作
                    try {
                        this.pageData.Subjects.forEach(s => {
                            var check = that.postData.subs.find(p => p.id == s.Id);
                            if (check == null)
                                throw '请对 \"' + s.Title + '\"进行投票';
                            else if (check.ops.length == 0)
                                throw '请对 \"' + s.Title + '\"进行投票';
                            else if (check.ops.length > s.MaxOptions)
                                throw '\"' + s.Title + '\" 最多只能投 ' + s.MaxOptions + '票';
                        });
                    } catch (e) {
                        weui.alert(e);
                        return;
                    };
                }

                var loading = weui.loading("提交选票中...");
                vote_topic(query.topic, that.postData, res => {
                    window.scrollTo({ top: 0 });
                    loading.hide();
                    weui.toast("投票成功");
                    that.showRules = true;
                    this.getTopic();
                }, err => {
                    loading.hide();
                    weui.toast(err);
                });
            },

            getTopic(callback) {
                var that = this;
                get_topic(query.topic, this.postData.uid,
                    res => {
                        console.log(res);
                        res.Subjects.sort((i, j) => i.Order > j.Order);
                        let media = res.Media;
                        if (!media.startsWith('http://') && !media.startsWith('https://')) {
                            if (media.startsWith('/')) media = media.replace('/', '');
                            media = server + media;
                        }
                        pageParam.invite = this.postData.uid;

                        if (is_iOS()) {
                            that.shareObject.title = res.Title;
                            that.shareObject.desc = res.Content;
                            that.shareObject.link = getPageUrl();
                            that.shareObject.imgUrl = media;
                        }
                        else {
                            wx.onMenuShareAppMessage({
                                title: res.Title, // 分享标题
                                desc: res.Content, // 分享描述
                                link: getPageUrl(), // 分享链接
                                imgUrl: media, // 分享图标
                            });

                            wx.onMenuShareTimeline({
                                title: res.Title, // 分享标题
                                desc: res.Content, // 分享描述
                                link: getPageUrl(), // 分享链接
                                imgUrl: media, // 分享图标
                            });
                        }
                        let myvotes = res.MyVotes || [];
                        for (var value of res.Subjects) {
                            for (var opt of value.Options) {
                                if (myvotes.includes(opt.Id))
                                    opt.active = true;
                                else
                                    opt.active = false;
                            }
                        }
                        this.pageData = res;
                        this.postData.subs = res.MySubs || [];
                        if (callback) callback(res);
                        wxLoading.hide();
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
                }, err => {
                    console.log(err);
                });
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
            this.postData.uid = globalData.openid;
            this.postData.subs = [];
            this.likeStatus = localStorage.getItem('like:' + query.topic) == '1';
        },
        mounted() {
            this.getUserInfo();
            this.getTopic((res) => {
                if (localStorage.getItem('tempVotes') != null) {
                    this.postData = JSON.parse(localStorage.getItem('tempVotes'))
                    this.post();
                    localStorage.removeItem('tempVotes');
                }
            });
            if (is_iOS()) {
                wx.onMenuShareAppMessage(this.shareObject);
                wx.onMenuShareTimeline(this.shareObject);
            }
        }
    });
};