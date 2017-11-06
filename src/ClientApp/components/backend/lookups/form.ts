import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { FormBaseVM } from "../../../core/formbasevm";

@Component
export default class FormComponent extends Vue {

    // Data Property
    vm: FormBaseVM = new FormBaseVM("api/lookups", true);
    

    // Life Cycle Hook
    mounted() {
        var categoryId = Number(this.$route.params.categoryid);
        var id = Number(this.$route.params.id);
        this.vm.find(id).then(data => {
            (this.vm.Model as any).categoryid = categoryId;
        });
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
