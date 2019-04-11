/* Vue plugin config */
Vue.prototype.$http = axios;
Vue.prototype.wx = weui;

/* API */
const server = 'http://localhost:56988/';
var get_topic = (topicId, callback, error) => {
    axios.get(server + 'vote/topic/' + topicId)
        .then(res => {
            if (res.status == 200)
                callback(res.data);
            else
                error(res);
        });

}
new Vue({
    el: '#app',
    data() {
        return {
        }
    },
    mounted() {
        get_topic('2166554f-14f9-43ad-ae2b-cd9f59e065bf');
    }
});