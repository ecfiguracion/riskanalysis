import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/lookups", true);
    showForm: boolean = false; 

    // Life Cycle Hook
    mounted() {
        eventBus.$on('lookupForm', (id: number, categoryId: number) => {
            if (id == 0) {
                this.vm.Model = {};
                (this.vm.Model as any).categoryid = categoryId;
            }
            else {
                this.vm.find(id).then(data => {
                    (this.vm.Model as any).categoryid = categoryId;
                });
            }
            this.showForm = true;
        })         
    }

    // Component Methods
    onSave() {
        this.vm.save()
        .then(data => { 
            this.showForm = false;
            eventBus.$emit('saveLookup');
        })
        .catch(error => { });
    }   

    onCancel() {
        this.showForm = false;
    }

    beforeDestroy() {
        eventBus.$off('lookupForm');
    }          
}
