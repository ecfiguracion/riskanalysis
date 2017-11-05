

<template>
  <div>
    <md-layout>
      <md-layout md-flex="75" md-column>
        <div>
          <md-input-container>
            <label>Search</label>
            <md-input data-vv-name="searchText" v-model="vm.Filters.Search.Text"></md-input>
          </md-input-container>
        </div>
      </md-layout>
      <md-layout md-align="end">
        <div>
          <md-button class="md-icon-button md-raised md-primary"  @click="onNew">
            <md-icon>add</md-icon>
          </md-button>

          <md-button class="md-icon-button md-raised md-warn" @click="vm.refresh()">
            <md-icon>refresh</md-icon>
          </md-button>

          <md-button class="md-icon-button md-raised md-accent" @click="onDelete">
            <md-icon>delete</md-icon>
          </md-button> 
        </div>
      </md-layout>      
    </md-layout>
    <md-table-card>
      <md-table @select="onSelect">
          <md-table-header>
            <md-table-row>
              <md-table-head>Name</md-table-head>
              <md-table-head md-tooltip="Latitude location on map">
                <md-icon>location_on</md-icon>
                <span>Latitude</span>
              </md-table-head>            
              <md-table-head md-tooltip="Longitude location on map">
                <md-icon>location_on</md-icon>
                <span>Longitude</span>
              </md-table-head>
            </md-table-row>
          </md-table-header>
          <md-table-body>
            <md-table-row v-for="item in vm.pagedmodel.data" :key="item.id" :md-item="item" md-selection>
              <md-table-cell>
                <router-link :to="`${UI}${item.id}`" exact>{{ item.name }}</router-link>
              </md-table-cell>
              <md-table-cell>
                {{ item.latitude }}
              </md-table-cell>
              <md-table-cell>
                {{ item.longitude }}
              </md-table-cell>            
            </md-table-row>
          </md-table-body>               
      </md-table>  
      <md-table-pagination
        md-size="5"
        md-total="10"
        md-page="1"
        md-label="Rows"
        md-separator="of"
        :md-page-options="[5, 10, 25, 50]"
        @pagination="onPagination">
      </md-table-pagination>    
    </md-table-card>
  </div>
</template>

<script>
  import vmpagedbase from '../../../core/vmpagedbase'
  import UI from '../../../core/constants/pages'

  export default {
    data () {
      return {
        vm: new vmpagedbase(),
        UI: UI.CONFIG.Barangay,
        errorMessage: ''
      }
    },
    methods: {
      onNew () {
        this.$router.push(this.UI + '0')
      },
      onSelect (selected) {
        this.vm.checkedFilterIds(selected)
      },
      onDelete () {
        console.log(this.vm.Filters.Search.SelectedIds)
        // this.$modal.confirm({
        //     content: 'Are you sure you want to remove selected records?',
        //     onOk: () => {
        //       this.vm.delete()
        //         .then(data => {
        //           this.$notify.success({content: 'Record successfully deleted..'});
        //           this.vm.refresh()             
        //         })
        //         .catch(error => {
        //           this.errorMessage = error
        //         })              
        //     } 
        // });
      },
      onPagination (pages) {
        console.log(pages)
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
  /* .md-input-container { 
    margin: 4px 0 4px;
  } */

  /* .md-table .md-table-head-container {
      height: 32px !important;
      padding: 2px 0;
  }   */
</style>