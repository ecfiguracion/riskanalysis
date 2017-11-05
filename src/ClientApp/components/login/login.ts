import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class LoginComponent extends Vue {
    
    onLogin() {
        this.$router.push('/barangays')
    }

    onCancel() {
        this.$router.push('/')
    }
    
    data () {
        return {
            
        }
    }
}
