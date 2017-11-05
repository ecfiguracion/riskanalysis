import { store } from './config/store'

function login(apikey) {
    if (apikey.length > 0) {
        store.commit('setAPIKey',apikey)
        return true
    } else {
        this.logout()
        return false
    }
}

function logout() {
    store.commit('setAPIKey','')
}

function islogged(){
    return store.state.isLoggedIn;
}

function getAPIKey() {
    if (this.islogged)
        return localStorage.getItem('API_KEY')
    else
        return ''
}

export default { login, logout, islogged, getAPIKey }