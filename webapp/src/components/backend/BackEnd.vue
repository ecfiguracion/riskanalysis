<template>
  <div class="mainbackend">
    <div class="section">
      <div class="columns">
        <div class="column is-8 is-offset-2">
          <breadcrumb>
            <breadcrumb-item>
              <router-link to="/" exact><i class="fa fa-home"></i>Home</router-link>
              <router-link to="/logout" exact><i class="fa fa-home"></i>Logout</router-link>
            </breadcrumb-item>
          </breadcrumb>        
        </div>
      </div>
    </div>
    <div class="section">
      <div class="columns">
        <div class="column is-8 is-offset-2">
          <div class="columns">
            <div class="column is-3 menu is-dark">
              <menus label="Post Assessments">
                <menu-item icon="edit" to="/backend/assessments" :is-active="true">Assessments</menu-item>                
              </menus>
              <div class="divider"></div>                              
              <menus label="Configurations">
                <menu-item icon="gear" to="/backend/barangays">Barangay</menu-item>
                <menu-item icon="gear" to="/backend/agriculture">Agriculture</menu-item>
                <menu-item icon="gear" to="/backend/lifelines">Lifelines</menu-item>                
                <menu-item icon="gear" to="/backend/structures">Structures</menu-item>                                        
              </menus>              
            </div>
            <div class="column is-9">
              <div class="backend">
                <router-view></router-view>
              </div>
            </div>            
          </div>          
        </div>        
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      msg: 'Welcome to backend.'
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
