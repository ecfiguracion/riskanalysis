<template>
  <div class="login">           
    <br/><br/>

    <md-layout gutter md-align="middle">    
      <md-layout md-flex="30" md-flex-offset="35" md-column>
        <md-card >
          <md-card-header>
            <div class="md-title">Typhoon Risk Analysis</div>
            <div class="md-subhead">Please enter your account</div>
          </md-card-header>
          <md-card-content>    
            <form @submit.stop.prevent="submit">
              <md-input-container :class="{'md-input-invalid':errors.has('email')}">
                <label>Email</label>
                <md-input data-vv-name="email" v-validate.initial="'required|email'" v-model="vm.model.email"></md-input>
                <!-- <span :class="{'md-error':errors.has('email')}">Password is required</span>                 -->
              </md-input-container>
              <md-input-container :class="{'md-input-invalid':errors.has('password')}">
                <label>Password</label>
                <md-input type="password" data-vv-name="password" v-validate.initial="'required'" v-model="vm.model.password"></md-input>
                <!-- <span :class="{'md-error':errors.has('password')}">{{ errors['password'].msg}}</span>                                 -->
              </md-input-container>                
            </form>          
            <br/>     
            <md-button class="md-raised md-primary" :disabled="errors.any()" v-on:click="onLogin">Login</md-button>
            <md-button class="md-raised md-accent" v-on:click="onCancel">Cancel</md-button>        
          </md-card-content>
        </md-card>
      </md-layout>
      <md-layout md-flex="35"></md-layout>       
    </md-layout>
    <div>
      <md-dialog-alert
        md-title="Login failed"
        md-content-html="Username or password is incorrect. <br> Please retry."
        md-ok-text="Ok"
        ref="errordialog">
      </md-dialog-alert>  
    </div>      
  </div>
</template>

<script>
  import vmbase from '../core/vmbase'
  import auth from '../core/authenticate'
  
  export default {
    data () {
      return {
        vm: new vmbase(),
        errorMessage: ''
      }
    },
    computed: {
      hasAuthError: function () {
        return this.errorMessage.length > 0
      }
    },
    methods: {
      onLogin () {
        this.$validator.validateAll().then((result) => {
          if (result) {
            this.vm.find()
              .then(data => {
                if (auth.login(data)) {
                  this.$router.push('/backend/assessments')
                } else {
                  this.$refs['errordialog'].open()
                }
              })
              .catch(error => {
                this.errorMessage = error
              })
          }
        })
      },
      onKeyDown () {
        this.errorMessage = ''
      },
      onCancel () {
        auth.logout()
        this.$router.push('/')
      }    
    },
    mounted () {
      this.vm.UrlEndPoint = '/authenticate'    
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
