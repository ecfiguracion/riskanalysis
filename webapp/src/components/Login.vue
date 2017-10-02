<template>
  <div class="section">
    <div class="columns">
      <div class="column is-4 is-offset-4">
        <form method="POST" @submit.prevent="onSubmit" @keydown="onKeyDown">
          <h1>This is a login page</h1>
          <label class="label">Email</label>
          <p class="control">
            <input id="email" name="email" class="input is-small" v-validate.initial="'required|email'" 
                :class="{'is-danger':errors.has('email')}"  type="text" v-model="vm.model.email">
          </p>

          <label class="label">Password</label>
          <p class="control">
            <input id="password" name="password" class="input is-small" v-validate.initial="'required'"
                :class="{'is-danger':errors.has('password')}" type="text" v-model="vm.model.password">
          </p>
          <p class="control">
            <button class="button is-primary">Submit</button>
            <button type="button" class="button is-danger" @click="cancel">Cancel</button>
          </p>  
        </form>

        <div v-if="hasAuthError">
          <hr>        
          <article class="message is-danger">
            <div class="message-header">
              <p><strong>Error!</strong></p>
            </div>
            <div class="message-body">
              {{ errorMessage }}
            </div>
          </article>
        </div>
      </div>
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
      onSubmit () {
        this.$validator.validateAll().then((result) => {
          if (result) {
            this.vm.post('/authenticate',false)
              .then(data => {
                if (auth.login(data)) {
                  this.$router.push('/backend/assessments')
                } else {
                  this.errorMessage = 'Username or password is invalid. Please recheck.'
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
      cancel () {
        auth.logout();
        this.$router.push('/')
      }
    },
    mounted () {
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
