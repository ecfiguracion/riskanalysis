<template>
  <div class="mainbackend">
    <md-layout style="background:red">
      <h1>header</h1> 
    </md-layout>
    <md-layout md-offset="10" md-flex="80" style="background:green; padding: 10px">
      <md-layout md-flex="30" md-column style="background:blue">
          <h1>right</h1> 
      </md-layout>  
      <md-layout md-column style="background:orange">
          <md-layout>
            <md-layout md-column md-flex="20" style="margin:10px">
              <md-whiteframe style="background: cyan">1dp</md-whiteframe>
            </md-layout>
          </md-layout>
          <md-layout>
            <md-layout md-column md-flex="20" style="margin:10px">
              <md-whiteframe style="background: cyan">1dp</md-whiteframe>
            </md-layout>

            <md-layout md-column md-flex="20" style="margin:10px">
              <md-whiteframe style="background: cyan">1dp</md-whiteframe>
            </md-layout>

            <md-layout md-column md-flex="20" style="margin:10px">
              <md-whiteframe style="background: cyan">1dp</md-whiteframe>
            </md-layout>

            <md-layout md-column md-flex="20" style="margin:10px">
              <md-whiteframe style="background: cyan">1dp</md-whiteframe>
            </md-layout>
            
          </md-layout>      
          <md-layout>
            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>
          </md-layout>           
          <md-layout>
            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>

            <md-layout>
              <span>24px</span>
            </md-layout>
          </md-layout>                 
      </md-layout> 
             
    </md-layout>    
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

  .sidenav-container {
  position: absolute;
  min-height: 100% !important;
  }
</style>
