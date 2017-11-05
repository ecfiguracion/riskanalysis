import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export const store = new Vuex.Store({
    state: {
      isLoggedIn: !!localStorage.getItem("API_KEY")
    },
    mutations: {
        setAPIKey (API_KEY, value) {
            if (value) {
                localStorage.setItem("API_KEY", value);
                this.state.isLoggedIn = true
            } else {
                if (localStorage.getItem("API_KEY")) {
                    localStorage.removeItem("API_KEY")
                }
                this.state.isLoggedIn = false                    
            }
        }
    }
})