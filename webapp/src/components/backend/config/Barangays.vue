<template>
    <div class="columns">
      <div class="column"> 
        <p class="control">
          <button class="button is-primary" @click="onNew">New</button>
          <button class="button is-danger" @click="onDelete">Delete</button>
        </p>          
        <table class="table is-fullwidth">
            <thead>
              <th></th>
              <th>Barangay</th>
              <th>Latittude</th>                
              <th>Longitude</th>
            </thead>
            <tbody>
              <tr v-for="item in vm.pagedmodel.data" v-bind:key="item.id">
                <th> <input type="checkbox" :value="`${item.id}`" v-model="vm.SelectedIds"> </th>
                <th> <router-link :to="`/backend/barangay/${item.id}`" exact>{{ item.name }}</router-link> </th>
                <th> {{ item.latitude }} </th>
                <th> {{ item.longitude }} </th>
              </tr>                
            </tbody>
        </table>
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
      onNew () {
        this.$router.push('/backend/barangay/0')
      },
      onDelete () {
        this.$modal.confirm({
            content: 'Are you sure you want to remove selected records?',
            onOk: () => {
              this.vm.post('/api/barangay/delete')
                .then(data => {
                  this.$notify.success({content: 'Record successfully deleted..'});
                  this.refresh()                  
                })
                .catch(error => {
                  this.errorMessage = error
                })              
            } 
        });
      },
      refresh () {
        this.vm.get('/api/barangay/get')
          .then(data => {
          })
          .catch(error => {
            this.errorMessage = error
          })      
      }
    },
    mounted() {
      this.vm.IsUsePagedModel = true
      this.vm.IsUseToken = true
      this.refresh()
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
