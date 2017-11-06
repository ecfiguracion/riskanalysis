import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/category", true);
    

    // Life Cycle Hook
    mounted() {
        var id = Number(this.$route.params.id);
        this.vm.find(id).then(data => {});
    }

    // Component Methods
    onSave() {
        this.vm.save()
        .then(data => { 
            this.$router.go(-1);
        })
        .catch(error => { });
    }   

    onCancel() {
        this.$router.go(-1);
    }
}
