<template>
  <div class="section">
    <div class="columns">
      <div class="column is-8 is-offset-2">
        <h1>{{ msg }}</h1>
        <h2>Risk Analysis</h2>
        <breadcrumb>
          <breadcrumb-item>
            <router-link to="/" exact><i class="fa fa-home"></i>Home</router-link>
          </breadcrumb-item>
          <breadcrumb-item>
            <router-link to="/login" exact><i class="fa fa-user"></i>Login</router-link>
          </breadcrumb-item>
        </breadcrumb>    
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'hello',
  data () {
    return {
      msg: 'Welcome to Your Vue.js App'
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
