import Vue from 'vue';
import { eventBus } from "../../../boot";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";
import moment from 'moment';

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/typhoons", true);
    showForm: boolean = false; 

    // Life Cycle Hook
    mounted() {
        eventBus.$on('typhoonForm', (id: number) => {
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
        eventBus.$off('typhoonForm');
    }      
}
