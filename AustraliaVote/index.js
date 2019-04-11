/* API Server*/
const server = 'http://localhost:56988/';
/**
 * 获取服务器中的微信页面配置参数
 * @param {any} success
 * @param {any} error
 */
function get_wxconfig(success, error) {
    axios.get(server + 'sns/wxconf?url=' + location.href)
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
 * 获取投票主题的信息
 * @param {String} topicId 主题ID
 * @param {Function} callback 接口调用成功
 * @param {Function} error 接口调用失败
 */
function get_topic(topicId, success, error) {
    axios.get(server + 'vote/topic/' + topicId)
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

/*
 * 获取微信配置以及微信页面初始化
**/
get_wxconfig(res => {
    wx.config({
        debug: false,
        appId: res.appid,
        timestamp: res.timestamp,
        nonceStr: res.nonce,
        signature: res.signature,
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage']
    });
}, err => {
    console.log('微信配置获取失败: ' + err);
})

wx.ready(() => {
    console.log('微信准备就绪');
    Vue.prototype.wx = wx;
});

/* Vue plugin config */
Vue.prototype.$http = axios;

new Vue({
    el: '#app',
    data() {
        return {
            pageData: {},
            postData: {},
            
        }
    },
    methods: {
        support(cell, sId) {
            if (this.postData.subs.find(p => p.id == sId)) {
                let sub = this.postData.subs.find(p => p.id == sId)
                sub.ops = [{
                    id: cell.Id,
                    order: cell.Order
                }];
            } else {
                this.postData.subs.push({
                    id: sId,
                    ops: [{
                        id: cell.Id,
                        order: cell.Order,
                    }]
                })
            }

            //console.log('post',this.postData);


        },
        like(Id) {
            like_topic(Id, res => {
                weui.toast("支持成功");
            })
        },
        post() {
            var that = this;
            vote_topic('2166554f-14f9-43ad-ae2b-cd9f59e065bf', that.postData, res => {
                weui.toast("投票成功");
                get_topic('2166554f-14f9-43ad-ae2b-cd9f59e065bf',
                    res => {
                        that.pageData = res;
                        //console.log('ok',res);
                    });
                
            }, err => {
              
                });
        }

    },
    created() {
        this.postData.subs = [];
        this.postData.uid = 'hello';
    },
    mounted() {
        get_topic('2166554f-14f9-43ad-ae2b-cd9f59e065bf',
            res => {
                this.pageData = res;
                console.log(res);
            });

    }
});