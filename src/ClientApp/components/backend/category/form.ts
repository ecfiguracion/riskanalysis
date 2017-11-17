import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/category", true);
    showForm: boolean = false; 

    // Life Cycle Hook
    mounted() {
        eventBus.$on('categoryForm', (id: number) => {
            if (id == 0)
                this.vm.Model = {};
            else
                this.vm.find(id).then(data => { });
            this.showForm = true;
        })         
    }

    // Component Methods
    onSave() {
        this.vm.save()
        .then(data => { 
            this.showForm = false;
            eventBus.$emit('saveTyphoon');
        })
        .catch(error => { });
    }   

    onCancel() {
        this.showForm = false;
    }

    beforeDestroy() {
        eventBus.$off('categoryForm');
    }     
}
