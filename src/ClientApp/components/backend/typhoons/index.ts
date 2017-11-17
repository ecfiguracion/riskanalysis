import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { PagedBaseVM } from "../../../core/pagedbasevm";
import bootbox from 'bootbox';

@Component({
    components: {
        typhoonForm: require('./form.vue.html')
    }
})

export default class IndexComponent extends Vue {

    // Data Property
    vm: PagedBaseVM = new PagedBaseVM("api/typhoons",true);

    // Life Cycle Hook
    mounted() {
        this.vm.onSearch();

        eventBus.$on('saveTyphoon',() => {
            this.vm.onSearch();
        })           
    }

    // Component Methods
    onNew() {
        //this.$router.push("/typhoons/0");
        eventBus.$emit('typhoonForm',0);
    }

    onUpdate(id: number) {
        //this.$router.push("/typhoons/" + id.toString());
        eventBus.$emit('typhoonForm',id);
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
        eventBus.$off('saveTyphoon');
    }       
}
