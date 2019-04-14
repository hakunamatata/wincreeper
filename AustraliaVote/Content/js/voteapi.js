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
};

let globalData = {

};
/*
 * pageParam: 保存页面中的关键信息 
 */
if (query.invite)
    pageParam.invite = query.invite;

function getPageUrl() {
    return location.origin + location.pathname + '?' + Qs.stringify(pageParam);
};

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

};
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
        .catch(error);
};

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
};

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
};

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
};

/**
 * 设备是否为iOS
 * */
function is_iOS() {
    return /(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent);
}