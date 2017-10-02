<template>
    <div class="columns">
      <div class="column"> 
          <h1>BARANGAY DETAIL</h1>

        <form method="POST" @submit.prevent="onSubmit">
          <label class="label">Name</label>
          <p class="control">
            <input id="name" name="name" class="input is-small" v-validate.initial="'required|max:150'" 
                :class="{'is-danger':errors.has('name')}"  type="text" v-model="vm.model.name">
          </p>

          <label class="label">Longitude</label>
          <p class="control">
            <input id="longitude" name="longitude" class="input is-small"  v-validate.initial="'numeric'" 
                :class="{'is-danger':errors.has('longitude')}"  type="text" v-model="vm.model.longitude">
          </p>

          <label class="label">Latitude</label>
          <p class="control">
            <input id="latitude" name="latitude" class="input is-small"  v-validate.initial="'numeric'" 
                :class="{'is-danger':errors.has('latitude')}"  type="text" v-model="vm.model.latitude">
          </p>

          <p class="control">
            <button class="button is-primary">Save</button>
            <button type="button" class="button is-danger" @click="cancel">Cancel</button>
          </p>  
        </form>          
      </div>        
    </div>
</template>

<script>
  import vmbase from '../../../core/vmbase'

  export default {
    data () {
      return {
        vm: new vmbase(),
        errorMessage: ''
      }
    },
    methods: {
      onSubmit () {
        this.$validator.validateAll().then((result) => {
          if (result) {
            this.vm.post('/api/barangay/save')
              .then(data => {
                this.$notify.success({content: 'Record successfully saved..'});
                this.$router.push('/backend/barangays')
              })
              .catch(error => {
                this.errorMessage = error
              })
          }
        })
      },      
      cancel () {
        this.$router.go('-1')
      }      
    },
    mounted() { 
      this.vm.IsUseToken = true

      var id = this.$route.params.id
      if (id > 0) {
        this.vm.get('/api/barangay/get/'+id)
          .then(data => {
            this.$validator.validateAll()
          })
          .catch(error => {
            this.errorMessage = error
        }) 
      }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
