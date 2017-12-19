import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import bootbox from 'bootbox';

@Component
export default class LoginComponent extends Vue {

    username: string;
    password: string;

    onLogin() {
        axios.post("api/useraccounts/authenticate", { username: this.username, password: this.password })
            .then(response => {
                var token = response.data;
                if (token) {
                    this.$router.push('/assessments');
                } else {
                    bootbox.alert("Invalid username and password.");
                }
            })
    }

    onCancel() {
        this.$router.push('/')
    }
    
    data () {
        return {
            
        }
    }
}
