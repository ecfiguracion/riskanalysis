import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { eventBus } from "../../../boot";
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/barangays", true);
    showForm: boolean = false;    

    // Life Cycle Hook
    mounted() {
        // var id = Number(this.$route.params.id);
        // this.vm.find(id).then(data => { });
        
        eventBus.$on('barangayForm', (id: number) => {
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
            //this.$router.go(-1);
            this.showForm = false;
            eventBus.$emit('saveBarangay');
        })
        .catch(error => { });
    }   

    onCancel() {
        //this.$router.go(-1);
        this.showForm = false;
    }

    beforeDestroy() {
        eventBus.$off('barangayForm');
    }    
}
