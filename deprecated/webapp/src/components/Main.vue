<template>
  <div class="section">
    <div class="columns">
      <div class="column is-8 is-offset-2">
        <h2>Risk Analysis</h2>
        <nav class="breadcrumb" aria-label="breadcrumbs">
          <ul>
            <li><router-link to="/" exact><i class="fa fa-home"></i>Home</router-link></li>
            <li><router-link to="/" exact><i class="fa fa-home"></i>Map</router-link></li>            
            <li><router-link to="/" exact><i class="fa fa-home"></i>Charts</router-link></li>
            <li><router-link to="/" exact><i class="fa fa-home"></i>Data</router-link></li>            
            <li><router-link to="/" exact><i class="fa fa-home"></i>Analysis</router-link></li>                        
            <li><router-link to="/login" exact><i class="fa fa-home"></i>Login</router-link></li>
          </ul>
        </nav>        
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'main',
  data () {
    return {
      menus: [
        { text: 'Home',url:'/' },
        { text: 'Map', url:'/'},
        { text: 'Charts', url:'/'},
        { text: 'Data', url:'/' },
        { text: 'Analysis', url:'/'},
        { text: 'Login', url:'/login' }
      ]
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
