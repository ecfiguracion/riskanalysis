<template>
    <div class="columns">
      <div class="column"> 
        <p class="control">
          <button class="button is-primary" @click="onNew">New</button>
          <button class="button is-primary" @click="vm.refresh()">Refresh</button>          
          <button class="button is-danger" @click="onDelete">Delete</button>
          <button class="button is-warning" @click="vm.goFirstPage()">|<</button>
          <button class="button is-warning" @click="vm.goPreviousPage()"><</button>          
          <button class="button is-warning" @click="vm.goNextPage()">></button>
          <button class="button is-warning" @click="vm.goLastPage()">>|</button>          
        </p>          
        <p class="control">
            <p>Search</p>
            <input id="searchText" v-validate.initial="'required'" 
                :class="{'is-danger':errors.has('searchText')}"  type="text" v-model="vm.Filters.Search.Text">          
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
                <th> <input type="checkbox" :value="`${item.id}`" v-model="vm.Filters.SelectedIds"> </th>
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
  import vmpagedbase from '../../../core/vmpagedbase'

  export default {
    data () {
      return {
        vm: new vmpagedbase(),
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
              this.vm.delete()
                .then(data => {
                  this.$notify.success({content: 'Record successfully deleted..'});
                  this.vm.refresh()             
                })
                .catch(error => {
                  this.errorMessage = error
                })              
            } 
        });
      }
    },
    mounted() {
      this.vm.IsUseToken = true
      this.vm.UrlEndPoint = '/api/barangay'
      this.vm.refresh()
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
