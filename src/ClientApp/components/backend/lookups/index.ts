import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { PagedBaseVM } from "../../../core/pagedbasevm";
import bootbox from 'bootbox';
import { store } from "../../../boot";

interface Category {
    id: number;
    name: string;
}

@Component({
    components: {
        lookupForm: require('./form.vue.html')
    }
})

export default class IndexComponent extends Vue {

    // Data Property
    vm: PagedBaseVM = new PagedBaseVM("api/lookups", true);
    categories: Category[] = [];

    // Life Cycle Hook
    mounted() {
        //this.vm.onSearch();
        axios.get("api/lookups/datalookups", {
            headers: { tokenAuthorization: store.getters.token }
        })
        .then(response => {
            this.categories = response.data;
            if (this.categories) {
                this.vm.PagedParams.parameter1 = this.categories[0].id;
            }
            this.vm.onSearch();
        })

        eventBus.$on('saveLookup',() => {
            this.vm.onSearch();
        })          
    }

    // Component Methods
    onNew() {
        var categoryId = this.vm.PagedParams.parameter1;
        eventBus.$emit('lookupForm',0,categoryId);
        //this.$router.push("/lookups/" + categoryId + "/0");
    }

    onUpdate(id: number) {
        var categoryId = this.vm.PagedParams.parameter1;
        eventBus.$emit('lookupForm',id,categoryId);
        //this.$router.push("/lookups/" + categoryId + "/" + id.toString());
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
                    .catch(error => { 
                        bootbox.alert("Error occurs:" + error.data);
                    }); 
                }    
            }
        });       
    }

    beforeDestroy() {
        eventBus.$off('saveLookup');
    }      
}
