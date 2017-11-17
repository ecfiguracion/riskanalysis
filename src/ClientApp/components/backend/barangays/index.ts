import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { PagedBaseVM } from "../../../core/pagedbasevm";
import bootbox from 'bootbox';

@Component({
    components: {
        barangayForm: require('./form.vue.html')
    }
})

export default class IndexComponent extends Vue {

    // Data Property
    vm: PagedBaseVM = new PagedBaseVM("api/barangays",true);

    // Life Cycle Hook
    mounted() {
        this.vm.onSearch();

        eventBus.$on('saveBarangay',() => {
            this.vm.onSearch();
        })   
    }

    // Component Methods
    onNew() {
        //this.$router.push("/barangays/0");
        eventBus.$emit('barangayForm',0);
    }

    onUpdate(id: number) {
        //this.$router.push("/barangays/" + id.toString());
        eventBus.$emit('barangayForm',id);
    }

    onDelete(id: number) {  
        bootbox.confirm({
            title: "Delete Record",
            message: "Are you sure you wish to delete this record?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: (result) => {
                if (result) {
                    this.vm.onDelete(id)
                    .then(data => { 
                        this.vm.onSearch();
                    })
                    .catch(error => { }); 
                }    
            }
        });            

    }

    beforeDestroy() {
        eventBus.$off('saveBarangay');
    }      
}
