import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import { PagedBaseVM } from "../../../core/pagedbasevm";

interface Category {
    id: number;
    name: string;
}

@Component
export default class IndexComponent extends Vue {

    // Data Property
    vm: PagedBaseVM = new PagedBaseVM("api/lookups", true);
    categories: Category[] = [];

    // Life Cycle Hook
    mounted() {
        //this.vm.onSearch();
        axios.get("api/lookups/datalookups")
        .then(response => {
            this.categories = response.data;
            if (this.categories) {
                this.vm.PagedParams.parameter1 = this.categories[0].id;
            }
            this.vm.onSearch();
        })
    }

    // Component Methods
    onNew() {
        var categoryId = this.vm.PagedParams.parameter1;
        this.$router.push("/lookups/" + categoryId + "/0");
    }

    onUpdate(id: number) {
        var categoryId = this.vm.PagedParams.parameter1;
        this.$router.push("/lookups/" + categoryId + "/" + id.toString());
    }

    onDelete(id: number) {
        this.vm.onDelete(id)
            .then(data => {
                this.vm.onSearch();
            })
            .catch(error => { });
    }
}
