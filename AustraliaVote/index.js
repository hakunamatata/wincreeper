/* Vue plugin config */
Vue.prototype.$http = axios;
Vue.prototype.wx = weui;

/* API */
const server = 'http://localhost:56988/';
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


new Vue({
    el: '#app',
    data() {
        return {
        }
    },
    mounted() {
        get_topic('2166554f-14f9-43ad-ae2b-cd9f59e065bf',
            res => {
                console.log(res);
            });

    }
});